using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace MyBoilerPlate.Web.Infrastructure.Extensions
{
    public static class FormFileExtensions
    {
        /// <summary>
        /// Asynchronously saves the contents of an uploaded file.
        /// </summary>
        /// <param name="formFile">The <see cref="IFormFile"/>.</param>
        public static byte[] GetBytes(
            this IFormFile formFile)
        {
            if(formFile == null)
            {
                throw new ArgumentNullException(nameof(formFile));
            }

            using(MemoryStream ms = new MemoryStream())
            {
                formFile.OpenReadStream().CopyTo(ms);

                return ms.ToArray();
            }
        }
    }
}