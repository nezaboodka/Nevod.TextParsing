// Free open-source Sharik library - http://code.google.com/p/sharik/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;

namespace Sharik.Threading
{
    // Pipeline class provides simple and easy facilities for
    // parallel processing of incoming items. Items are provided
    // by an enumeration callback. The operation performed for
    // each item is also provided in a form of a callback. The
    // processing is performed by a limited number of threads
    // acquired from a thread pool. The maximum number of threads
    // to be used and the error handling rules can be specified
    // as pipeline properties. All pipeline properties and methods
    // are thread-safe.

    // Public API consists of the following properties:

    //     Hint: string
    //     MaxWorkerCount: int
    //     DispatchTimeoutMs: int
    //     SuccessfulItemCountThreshold: int
    //     StopOnError: bool
    //     SkipErrorItems: bool
    //     IsRunning: bool    (read-only)
    //     IdleWorkerCount: int    (read-only)
    //     HasProcessedAllItems: bool    (read-only)

    // and methods:

    //     Start(readItem: Reader<TItem>; processItem: Processor)
    //     Stop(error: Exception; timeout: int): bool
    //     GetNextResult(out result: WorkItem; timeout: int): bool
    //     GetResultReader(timeout: int): Reader<WorkItem>
    //     WaitForExit(timeout: int): bool
    //     Dump(): string

    // A sample illustrating typical Pipeline usage is at the
    // end of this file (class PipelineSample).

    public enum PipelineResultStatus
    {
        EndOfWork = 0,
        Ready = 1,
        Timeout = 2
    }

    public class AsyncWorkItem<TItem, TResult>
    {
        public TItem Item;
        public TResult Result;
        public Exception Error;
        public Thread ProcessingThread;
        public override string ToString() { return Item.ToString(); }
    }

    public class Pipeline<TItem, TResult>
    {
        public delegate TResult Processor(TItem item);

        public class WorkItem : AsyncWorkItem<TItem, TResult>
        {
            public Pipeline<TItem, TResult> Pipeline;
            public int WorkerNumber;
            public DateTime StartTimeUtc;
            public DateTime FinishTimeUtc;
        }

        public string Hint { get { lock (this) return fHint; } set { lock (this) fHint = value; } }
        public int MaxWorkerCount { get { lock (this) return fMaxWorkerCount; } set { SetMaxWorkerCount(value); } }
        public int DispatchTimeoutMs { get { lock (this) return fDispatchTimeoutMs; } set { lock (this) fDispatchTimeoutMs = value; } }
        public int SuccessfulItemCountThreshold { get { lock (this) return fSuccessfulItemCountThreshold; } set { lock (this) fSuccessfulItemCountThreshold = value; } }
        public bool StopOnError { get { lock (this) return fStopOnError; } set { lock (this) fStopOnError = value; } }
        public bool SkipErrorItems { get { lock (this) return fSkipErrorItems; } set { lock (this) fSkipErrorItems = value; } }

        public Reader<WorkItem> Start(Reader<TItem> itemReader, Processor itemProcessor)
        {
            lock (this)
            {
                if (IsRunning)
                    throw new InvalidOperationException(string.Format(
                        "pipeline {0} is started already", Hint));
                fReadItem = itemReader;
                fProcessItem = itemProcessor;
                fSuccessfulItemCount = 0;
                Monitor.PulseAll(this);
            }
            ThreadPool.QueueUserWorkItem(this.Dispatch, itemReader);
            return GetResultReader(Timeout.Infinite);
        }

        public bool Stop(Exception error, int timeout)
        {
            RequestStop(error, false);
            return WaitForExit(timeout);
        }

        public bool IsRunning
        {
            get
            {
                lock (this)
                    return fReadItem != null || fInProgress.Count > 0 || fCompleted.Count > 0;
            }
        }

        public int IdleWorkerCount
        {
            get
            {
                lock (this)
                    return IdleWorkerCountInternal;
            }
        }

        public bool HasProcessedAllItems
        {
            get
            {
                lock (this)
                    return fHasReadAllItems && fReadItem == null &&
                        fInProgress.Count < 1 && fCompleted.Count < 1;
            }
        }

        public PipelineResultStatus GetNextResult(out WorkItem result, int timeout)
        {
            return GetNextResultInternal(out result, timeout); // may set item to null in case of timeout
        }

        public Reader<WorkItem> GetResultReader(int timeout)
        {
            return delegate(out WorkItem item)
            {
                var status = GetNextResultInternal(out item, timeout);
                if (status == PipelineResultStatus.Timeout)
                    throw new OperationCanceledException(string.Format(
                        "cannot read result: timeout occured in pipeline {0}\n{1}",
                        fHint, Dump(true, (TItem x) => x.ToString())));
                return status == PipelineResultStatus.Ready;
            };
        }

        public bool WaitForExit(int timeout)
        {
            lock (this)
            {
                var result = true;
                while (result && (fReadItem != null || fInProgress.Count > 0 || fCompleted.Count > 0))
                    result = Monitor.Wait(this, timeout);
                return result;
            }
        }

        public string Dump(bool stackTrace, Func<TItem, string> inputItemToString)
        {
            lock (this)
            {
                var sb = new StringBuilder();
                sb.AppendFormat("{0} > Max Workers: {1}, Running: {2}, Completed: {3}",
                    Hint, MaxWorkerCount, fInProgress.Count, fCompleted.Count);
                foreach (var wi in fInProgress)
                {
                    var t = wi.ProcessingThread;
                    sb.AppendFormat("\n{0} #{1} @{2} > {3}",
                        fHint, wi.WorkerNumber.ToString("D3"),
                        t != null ? t.ManagedThreadId.ToString("D3") : "???",
                        inputItemToString != null ? inputItemToString(wi.Item) : wi.Item.ToString());
                    if (stackTrace && t == Thread.CurrentThread)
                    {
                        var st = new StackTrace(false);
                        sb.AppendFormat("\n{0}", st);
                    }
                }
                foreach (var wi in fCompleted)
                {
                    var t = wi.ProcessingThread;
                    sb.AppendFormat("\n{0} #{1} @{2} < {3}",
                        fHint, wi.WorkerNumber.ToString("D3"),
                        t != null ? t.ManagedThreadId.ToString("D3") : "???",
                        inputItemToString != null ? inputItemToString(wi.Item) : wi.Item.ToString());
                }
                return sb.ToString();
            }
        }

        // Internal

        private void SetMaxWorkerCount(int value)
        {
            if (value < 1)
                throw new ArgumentException(string.Format(
                    "max working thread number cannot be {0}", value));
            lock (this)
                fMaxWorkerCount = value;
        }

        private int IdleWorkerCountInternal
        {
            get
            {
                var result = fMaxWorkerCount - fInProgress.Count - fCompleted.Count;
                if (fSuccessfulItemCountThreshold > 0)
                    result = Math.Min(result, fSuccessfulItemCountThreshold - fInProgress.Count - fSuccessfulItemCount);
                return result;
            }
        }

        private bool IsFullyBusy
        {
            get
            {
                return fReadItem != null &&
                    (fSuccessfulItemCountThreshold == 0 || fSuccessfulItemCount < fSuccessfulItemCountThreshold) &&
                    IdleWorkerCountInternal < 1;
            }
        }

        private void Dispatch(object obj)
        {
            try
            {
                TItem item;
                var stop = false;
                while (!stop && fReadItem(out item)) // fReadItem may be null!
                {
                    lock (this)
                    {
                        while (IsFullyBusy)
                        {
                            if (!Monitor.Wait(this, fDispatchTimeoutMs))
                                throw new OperationCanceledException(string.Format(
                                    "cannot dispatch item ({0}): timeout occured in pipeline {1}\n{2}",
                                    item, fHint, Dump(true, (TItem x) => x.ToString())));
                        }
                        var workItem = PrepareWorkItem(item);
                        if (fSuccessfulItemCountThreshold == 0 || fSuccessfulItemCount < fSuccessfulItemCountThreshold)
                        {
                            if (!ThreadPool.QueueUserWorkItem(DoWork, workItem))
                                throw new OperationCanceledException(string.Format(
                                    "cannot process item ({0}): no threads available for pipeline {1}",
                                    item, fHint));
                            fInProgress.Add(workItem);
                        }
                        else
                        {
                            workItem.Error = new OperationCanceledException("SuccessfulItemCountThreshold is reached");
                            CompleteWorkItem(workItem);
                            stop = true;
                        }
                        Monitor.PulseAll(this);
                    }
                }
                RequestStop(null, !stop);
            }
            catch (Exception e)
            {
                RequestStop(e, false);
            }
            finally
            {
                // Wait for all working threads to complete, or abort them in case of timeout
                lock (this)
                {
                    while (fInProgress.Count > 0 || fReadItem != null)
                    {
                        if (!Monitor.Wait(this, fDispatchTimeoutMs))
                        {
                            RequestStop(new OperationCanceledException(string.Format(
                                "cannot finalize pipeline ({0}): workers timed out\n{1}",
                                fHint, Dump(true, (TItem x) => x.ToString()))), true);
                            break;
                        }
                    }
                }
            }
        }

        private void DoWork(object obj)
        {
            var item = (WorkItem)obj;
            try
            {
                item.ProcessingThread = Thread.CurrentThread;
                item.StartTimeUtc = DateTime.UtcNow;
                item.Result = fProcessItem(item.Item);
            }
            catch (Exception e)
            {
                item.Error = e;
            }
            finally
            {
                // (!) Should we wrap the code below into try/catch? Or we can assume that no exceptions possible?
                item.FinishTimeUtc = DateTime.UtcNow;
                lock (this)
                {
                    CompleteWorkItem(item);
                    Monitor.PulseAll(this);
                }
            }
        }

        private WorkItem PrepareWorkItem(TItem item)
        {
            var workItem = new WorkItem();
            workItem.Item = item;
            workItem.Pipeline = this;
            workItem.WorkerNumber = AcquireWorkerNumber();
            return workItem;
        }

        private void CompleteWorkItem(WorkItem item)
        {
            if (item.Error == null)
                ++fSuccessfulItemCount;
            if (item.Error == null || !fSkipErrorItems)
                fCompleted.Enqueue(item);
            fInProgress.Remove(item);
            ReleaseWorkerNumber(item.WorkerNumber);
            if (item.Error != null && fStopOnError)
                RequestStop(item.Error, false);
        }

        private void RequestStop(Exception error, bool hasReadAllItems)
        {
            lock (this)
            {
                var pulse = fReadItem != null || error != null;
                if (fReadItem != null)
                {
                    fReadItem = null; // causes closing of result readers
                    fHasReadAllItems = hasReadAllItems;
                }
                if (error != null)
                    fErrors.Add(error);
                if (pulse)
                    Monitor.PulseAll(this);
            }
        }

        private int AcquireWorkerNumber()
        {
            var result = fInProgress.Count;
            while (result >= fInProgress.Count && fIdleWorkers.Count > 0)
            {
                result = Math.Min(fIdleWorkers[fIdleWorkers.Count - 1], result);
                fIdleWorkers.RemoveAt(fIdleWorkers.Count - 1);
            }
            return result;
        }

        private void ReleaseWorkerNumber(int workerNumber)
        {
            if (workerNumber < fInProgress.Count) // this condition is minor optimization
                fIdleWorkers.Add(workerNumber);
        }

        private PipelineResultStatus GetNextResultInternal(out WorkItem item, int timeout)
        {
            lock (this)
            {
                var result = PipelineResultStatus.EndOfWork;
                while (result != PipelineResultStatus.Timeout &&
                    fCompleted.Count == 0 && fErrors.Count == 0 &&
                    (fInProgress.Count > 0 || fReadItem != null))
                {
                    if (!Monitor.Wait(this, timeout))
                        result = PipelineResultStatus.Timeout;
                }
                if (result != PipelineResultStatus.Timeout && fCompleted.Count > 0)
                {
                    item = fCompleted.Dequeue();
                    result = PipelineResultStatus.Ready;
                    Monitor.PulseAll(this);
                }
                else
                    item = null;
                if (result == PipelineResultStatus.EndOfWork)
                    ThreadUnsafeRethrowErrorsIfAny(fErrors); // re-throw errors upon closing
                return result;
            }
        }

        private void ThreadUnsafeRethrowErrorsIfAny(IEnumerable<Exception> errors)
        {
            if (fErrors.Count == 1)
                throw fErrors[0];
            else if (fErrors.Count > 1)
                //throw new NotImplementedException(string.Format(
                //    "exceptions aggregation is not implemented (total count is {0}): {1}",
                //    fErrors.Count, fErrors[0].Message), fErrors[0]);
                throw fErrors[0]; // throw first exception only
        }

        private static void RequestThreadAbort(Thread thread)
        {
            try
            {
                thread.Abort();
            }
            catch (Exception)
            {
            }
        }

        // Fields - Parameters
        private string fHint = null;
        private int fMaxWorkerCount = 2;
        private int fDispatchTimeoutMs = Timeout.Infinite;
        private bool fStopOnError = true;
        private bool fSkipErrorItems = false;
        private int fSuccessfulItemCountThreshold = 0; // 0 means unlimited
        // Fields - Operational
        private Reader<TItem> fReadItem;
        private Processor fProcessItem;
        private readonly HashSet<WorkItem> fInProgress = new HashSet<WorkItem>();
        private readonly Queue<WorkItem> fCompleted = new Queue<WorkItem>();
        private int fSuccessfulItemCount = 0;
        private readonly List<int> fIdleWorkers = new List<int>();
        private readonly List<Exception> fErrors = new List<Exception>();
        private bool fHasReadAllItems = false;
    }

    public static class PipelineSample
    {
        public static byte[] DownloadBytesFromUrl(string url)
        {
            if (url.EndsWith("tut.by"))
                Thread.Sleep(100000000);
            return new WebClient().DownloadData(url);
        }

        public static void Main()
        {
            var urls = new string[]
            {
                "http://www.facebook.com", "http://www.facebook.comm",
                "http://www.microsoft.commm", "http://www.microsoft.com",
                "http://www.google.com",
                "http://www.apple.com", "http://www.oracle.com",
                "http://www.windows.commmmmm", "http://www.cnn.com",
                "http://www.bbc.co.uk", "http://www.tut.by"
            };
            var pipeline = new Pipeline<string, byte[]>()
            {
                Hint = "Downloader",
                MaxWorkerCount = 5,
                DispatchTimeoutMs = 10000,
                StopOnError = false,
                SkipErrorItems = false,
                SuccessfulItemCountThreshold = 0 // unlimited
            };
            pipeline.Start(ReaderUtil.GetReader(urls), DownloadBytesFromUrl);
            var resultReader = pipeline.GetResultReader(Timeout.Infinite);
            foreach (var result in ReaderUtil.GetEnumerable(resultReader))
            {
                var seconds = (result.FinishTimeUtc - result.StartTimeUtc).TotalSeconds;
                var worker = result.WorkerNumber;
                if (result.Error == null)
                    Console.WriteLine("#{0} > {1}: {2} bytes downloaded in {3} sec",
                        worker, result.Item, result.Item.Length, seconds.ToString("G"));
                else
                    Console.WriteLine("#{0} > {1}: ERROR: {2} ({3} sec)",
                        worker, result.Item, result.Error.Message, seconds.ToString("G"));
            }
        }
    }
}
