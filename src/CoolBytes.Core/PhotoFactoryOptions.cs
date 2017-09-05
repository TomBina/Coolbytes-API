using System;

namespace CoolBytes.Core
{
    public class PhotoFactoryOptions
    {
        public string UploadPath { get; }
        public Func<string, string> GetFileName { get; } =
            fileExtension => $"{Guid.NewGuid().ToString().ToLower()}{fileExtension}";

        public PhotoFactoryOptions(string uploadPath)
        {
            UploadPath = uploadPath;
        }

        public PhotoFactoryOptions(string uploadPath, Func<string, string> getFileName)
        {
            UploadPath = uploadPath;
            GetFileName = getFileName;
        }
    }
}