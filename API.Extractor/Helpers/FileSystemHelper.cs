using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Helpers
{
    public class FileSystemHelper
    {

        public static string GetFilePath(string directory, string prefix, string extension)
        {
            var fullPath = "";
            do
            {
                fullPath = Path.Combine(directory, $"{prefix}_{Guid.NewGuid()}{extension}");
            } while (File.Exists(fullPath));


            return fullPath;
        }

        public static void ClearDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
            Directory.CreateDirectory(directory);
        }
    }
}
