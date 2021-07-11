using Core.Common.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;

namespace MyBoilerPlate.Tests.Helpers
{
    public static class Record
    {
        /// <summary>
        /// Return Absolute Path for Recording files
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string GetPath(string fileName)
        {
            string basePath = Environment.CurrentDirectory;
            //Return from: bin\Debug\netcoreapp3.1 
            string relativePath = "../../../../Recording/" + fileName + ".json";

            return basePath + relativePath;
        }
        public static bool Exists<T>()
        {
            var entityName = typeof(T).Name;
            var exists = File.Exists(GetPath(entityName));
            return exists;
        }
        /// <summary>
        /// Find .json recording and return all values of entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> LoadData<T>()
        {
            var entityName = typeof(T).Name;
            var path = GetPath(entityName);


            //IEnumerable<T> data = JsonConvert.DeserializeObject<IEnumerable<T>>(File.ReadAllText(path));
            // deserialize JSON directly from a file

            var stringFile = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<IEnumerable<T>>(stringFile);

            return data;
        }

        /// <summary>
        /// Record Collection into .json for create started files
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Collection to Serialze</param>
        /// <param name="overrideFile">Override File</param>
        public static void RecordData<T>(IEnumerable<T> collection, bool overrideFile = false)
        {
            //Use TestNamespace + entityName; 
            var entityName = typeof(T).Name;
            var path = GetPath(entityName);

            // serialize JSON to a string and then write string to a file
            var exists = File.Exists(path);
            if (exists && overrideFile)
            {
                File.Delete(path);
            }
            if (!exists || (exists && overrideFile))
            {
                // serialize JSON directly to a file
                File.WriteAllText(path, JsonSerializer.Serialize(collection));
            }
        }

        public static byte[] GetBytes(int length)
        {
            return new byte[length];
        }
    }
}
