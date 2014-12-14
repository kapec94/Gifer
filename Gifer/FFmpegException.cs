using System;

namespace Gifer
{
    public class FFmpegException : Exception
    {
        public FFmpegException(int exitcode, String stderr)
        {
            ExitCode = exitcode;
            ErrorOutput = stderr;
        }

        public override string Message
        {
            get {
                return String.Format(
                        "ffmpeg exited with code {0}. Stderr output:\n{1}",
                        ExitCode,
                        ErrorOutput); 
            }
        }

        public int ExitCode { get; private set; }
        public String ErrorOutput { get; private set; }
    }
}
