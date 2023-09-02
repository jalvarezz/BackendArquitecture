using FFMpegCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Business.Tools
{
    // The FF Tools are for video processing.
    // They use the ffmpeg and ffprobe tools
    public static class FFtools
    {
        public static FFOptions GetCurrentOSOptions()
        {
            FFOptions options = null;

            if (OperatingSystem.IsWindows())
            {
                options = new FFOptions
                {
                    BinaryFolder = Path.Combine(AppContext.BaseDirectory, @"Tools\windows")
                };
                options.TemporaryFilesFolder = @"Files\Temp";
            }
            else
            {
                options = new FFOptions
                {
                    BinaryFolder = Path.Combine(AppContext.BaseDirectory, @"Tools/linux")
                };
                options.TemporaryFilesFolder = @"Files/Temp";
            }

            return options;
        }
    }
}
