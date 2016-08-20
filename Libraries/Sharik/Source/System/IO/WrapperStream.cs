// Free open-source Sharik library - http://code.google.com/p/sharik/

using System;
using System.IO;

namespace Sharik.IO
{
    public class WrapperStream : Stream
    {
        public WrapperStream(Stream baseStream, Stream echoStream, Action disposingAction, Action disposedAction)
        {
            fBaseStream = baseStream;
            fEchoStream = echoStream;
            fDisposingAction = disposingAction;
            fDisposedAction = disposedAction;
        }

        public override bool CanRead
        {
            get { return fBaseStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return fBaseStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return fBaseStream.CanWrite; }
        }

        public override bool CanTimeout
        {
            get { return fBaseStream.CanTimeout; }
        }

        public override int ReadTimeout
        {
            get { return fBaseStream.ReadTimeout; } set { fBaseStream.ReadTimeout = value; }
        }

        public override int WriteTimeout
        {
            get { return fBaseStream.WriteTimeout; } set { fBaseStream.WriteTimeout = value; }
        }

        public override long Length
        {
            get { return fBaseStream.Length; }
        }

        public override long Position 
        {
            get { return fBaseStream.Position; } set { fBaseStream.Position = value; }
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
            //return fBaseStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
            //return fBaseStream.BeginWrite(buffer, offset, count, callback, state);
        }

        protected override void Dispose(bool disposingAction)
        {
            try
            {
                if (disposingAction)
                {
                    try
                    {
                        try
                        {
                            try
                            {
                                if (fDisposingAction != null)
                                    fDisposingAction();
                            }
                            finally
                            {
                                if (fEchoStream != null)
                                    fEchoStream.Dispose();
                            }
                        }
                        finally
                        {
                            fBaseStream.Dispose();
                        }
                    }
                    finally
                    {
                        if (fDisposedAction != null)
                            fDisposedAction();
                    }
                }
            }
            finally
            {
                base.Dispose(disposingAction);
            }
        }

        public override int EndRead(IAsyncResult result)
        {
            throw new NotImplementedException();
            //return fBaseStream.EndRead(result);
        }

        public override void EndWrite(IAsyncResult result)
        {
            throw new NotImplementedException();
            //fBaseStream.EndWrite(result);
        }

        public override void Flush()
        {
            fBaseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var result = fBaseStream.Read(buffer, offset, count);
            if (fEchoStream != null)
                fEchoStream.Write(buffer, offset, result);
            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return fBaseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            fBaseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (fEchoStream != null)
                fEchoStream.Write(buffer, offset, count);
            fBaseStream.Write(buffer, offset, count);
        }

        // Internal
        private Stream fBaseStream;
        private Stream fEchoStream;
        private Action fDisposingAction;
        private Action fDisposedAction;
    }
}
