using System;
using System.Diagnostics;
using System.IO;
using Gifer.Properties;

namespace Gifer
{
// ReSharper disable once InconsistentNaming
    public class FFmpeg
    {
        static FFmpeg()
        {
            UnpackExecutable();
        }

        public static void Run(String cmdLine)
        {
            var process = Start(cmdLine);
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new FFmpegException(process.ExitCode, process.StandardError.ReadToEnd());
            }
        }

        public static Process Start(String cmdLine)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = cmdLine,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true
                }
            };

            process.Start();
            return process;
        }

        private static void UnpackExecutable()
        {
            if (File.Exists(exePath)) return;

            var dir = Path.GetDirectoryName(exePath);
            if (dir != null) Directory.CreateDirectory(dir);

            var data = Resources.ffmpeg;
            Stream stream = null;

            try
            {
                stream = new FileStream(exePath, FileMode.CreateNew);
                stream.Write(data, 0, data.Length);
            }
            finally
            {
                if (stream != null) stream.Close();
            }
        }

        private static readonly String exePath = Path.Combine(Directory.GetCurrentDirectory(), "bin", "ffmpeg.exe");
    }
}
