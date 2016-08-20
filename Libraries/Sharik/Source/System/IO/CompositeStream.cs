// Free open-source Sharik library - http://code.google.com/p/sharik/

using System;
using System.IO;

namespace Sharik.IO
{
    public enum StreamAccess { Read, Write };
    
    public delegate void OpenFragmentCallback(long logicalPosition, out Stream fragmentStream,
        out long fragmentMaxLength);

    public class CompositeStream : Stream
    {
        public CompositeStream(StreamAccess access, long startPosition, OpenFragmentCallback openFragmentCallback,
            Stream echoStream, Action disposeAction)
        {
            fAccess = access;
            fOpenFragmentCallback = openFragmentCallback;
            fLogicalPosition = startPosition;
            fEchoStream = echoStream;
            fDisposeAction = disposeAction;
            fOpenFragmentCallback(fLogicalPosition, out fFragmentStream, out fFragmentMaxLength);
            if (access == StreamAccess.Write)
                fStartingStream = fFragmentStream;
        }

        public override bool CanRead { get { return fAccess == StreamAccess.Read; } }
        public override bool CanWrite { get { return fAccess == StreamAccess.Write; } }
        public override bool CanSeek { get { throw new NotSupportedException(); } }
        public override bool CanTimeout { get { return fFragmentStream != null ? fFragmentStream.CanTimeout : false; } }
        public override int ReadTimeout { get { return GetReadTimeout(); } set { SetReadTimeout(value); } }
        public override int WriteTimeout { get { return GetWriteTimeout(); } set { SetWriteTimeout(value); } }
        public override long Length { get { throw new NotSupportedException(); } }
        public override long Position { get { return fLogicalPosition; } set { throw new NotSupportedException(); } }

        public override void Flush()
        {
            fFragmentStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (fAccess == StreamAccess.Read)
            {
                int result = 0;
                while (result == 0 && (fFragmentMaxLength > 0 || OpenFragment(fLogicalPosition)))
                {
                    count = (int)Math.Min(count, fFragmentMaxLength);
                    result = fFragmentStream.Read(buffer, offset, count);
                    if (fEchoStream != null)
                        fEchoStream.Write(buffer, offset, result);
                    fLogicalPosition += result;
                    fFragmentMaxLength -= result;
                    if (result == 0)
                        fFragmentMaxLength = 0;
                    if (fFragmentMaxLength == 0 && fFragmentStream != fStartingStream)
                        fFragmentStream.Dispose();
                }
                return result;
            }
            else
                throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (fAccess == StreamAccess.Write)
            {
                while (count > 0 && (fFragmentMaxLength > 0 || OpenFragment(fLogicalPosition)))
                {
                    int n = (int)Math.Min(count, fFragmentMaxLength);
                    fFragmentStream.Write(buffer, offset, n);
                    if (fEchoStream != null)
                        fEchoStream.Write(buffer, offset, count);
                    fLogicalPosition += n;
                    fFragmentMaxLength -= n;
                    count -= n;
                    offset += n;
                    if (fFragmentMaxLength == 0 && fFragmentStream != fStartingStream)
                        fFragmentStream.Dispose();
                }
            }
            else
                throw new NotSupportedException();
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public override int EndRead(IAsyncResult result)
        {
            throw new NotSupportedException();
        }

        public override void EndWrite(IAsyncResult result)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        // Internal

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    try
                    {
                        try
                        {
                            if (fDisposeAction != null)
                            {
                                fDisposeAction();
                                fDisposeAction = null;
                            }
                        }
                        finally
                        {
                            if (fFragmentStream != null)
                                fFragmentStream.Dispose();
                        }
                    }
                    finally
                    {
                        if (fStartingStream != null)
                            fStartingStream.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private bool OpenFragment(long logicalPosition)
        {
            fOpenFragmentCallback(logicalPosition, out fFragmentStream, out fFragmentMaxLength);
            return fFragmentStream != null && fFragmentMaxLength > 0;
        }

        private int GetReadTimeout()
        {
            if (fReadTimeout < 0 && fFragmentStream != null)
                return fFragmentStream.ReadTimeout;
            else
                return fReadTimeout;
        }

        private void SetReadTimeout(int value)
        {
            fReadTimeout = value;
            if (fFragmentStream != null)
                fFragmentStream.ReadTimeout = value;
        }

        private int GetWriteTimeout()
        {
            if (fWriteTimeout < 0 && fFragmentStream != null)
                return fFragmentStream.WriteTimeout;
            else
                return fWriteTimeout;
        }

        private void SetWriteTimeout(int value)
        {
            fWriteTimeout = value;
            if (fFragmentStream != null)
                fFragmentStream.WriteTimeout = value;
        }

        // Fields

        private StreamAccess fAccess;
        private OpenFragmentCallback fOpenFragmentCallback;
        private Action fDisposeAction;
        private Stream fEchoStream;
        private Stream fStartingStream;
        private Stream fFragmentStream;
        private long fLogicalPosition;
        private long fFragmentMaxLength;
        private int fReadTimeout = -1;
        private int fWriteTimeout = -1;
    }
}
