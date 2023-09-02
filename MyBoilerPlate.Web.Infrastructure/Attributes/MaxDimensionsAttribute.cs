using FFMpegCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using MyBoilerPlate.Business.Contracts;
using MyBoilerPlate.Business.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Web.Infrastructure.Attributes
{
    public sealed class MaxDimensionsAttribute : ValidationAttribute
    {
        private readonly int _Width;
        private readonly int _Height;

        public MaxDimensionsAttribute(int width, int height)
        {
            _Width = width;
            _Height = height;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (value != null && value is IFormFile file)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);

                    (new FileExtensionContentTypeProvider()).TryGetContentType(file.FileName, out string contentType);

                    if (contentType.StartsWith("image/"))
                    {
                        var image = SixLabors.ImageSharp.Image.Load(ms.ToArray());

                        if (image.Width > _Width || image.Height > _Height)
                        {
                            var messageHandler = (IMessageHandler)validationContext.GetService(typeof(IMessageHandler));

                            return new ValidationResult(string.Format(messageHandler.GetMessage("0002").Name, _Width, _Height));
                        }
                    }
                    else
                    {
                        // We need to use another MemoryStream, working with the memorystream from the iform file gives error
                        // with the ffprobe.
                        using (MemoryStream videoStream = new MemoryStream(ms.ToArray()))
                        {
                            var options = FFtools.GetCurrentOSOptions();

                            var probe = FFProbe.Analyse(videoStream, options);

                            if (probe.VideoStreams.Any())
                            {
                                var videoContent = probe.VideoStreams.First();

                                if (videoContent.Width > _Width || videoContent.Height > _Height)
                                {
                                    var messageHandler = (IMessageHandler)validationContext.GetService(typeof(IMessageHandler));

                                    return new ValidationResult(string.Format(messageHandler.GetMessage("0009").Name, _Width, _Height));
                                }
                            }
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
