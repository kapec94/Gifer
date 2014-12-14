using System;

namespace Gifer
{
    public class GifConverter
    {
        public static void Convert(String inputVideo, String outputFile)
        {
            /* -y overwrites files without asking */
            var cmdLine = String.Format("-i {0} {1} -y", inputVideo, outputFile);
            FFmpeg.Run(cmdLine);
        }
    }
}
