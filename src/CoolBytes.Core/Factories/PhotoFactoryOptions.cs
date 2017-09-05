using System;
using CoolBytes.Core.Extensions;

namespace CoolBytes.Core.Factories
{
    public class PhotoFactoryOptions
    {
        public string UploadPath { get; }
        public Func<string, string> GetFileName { get; } =
            fileExtension => $"{Guid.NewGuid().ToString().ToLower()}{fileExtension}";

        public PhotoFactoryOptions(string uploadPath)
        {
            uploadPath.IsNotNullOrWhiteSpace();

            UploadPath = uploadPath;
        }

        public PhotoFactoryOptions(string uploadPath, Func<string, string> getFileName)
        {
            uploadPath.IsNotNullOrWhiteSpace();
            getFileName.IsNotNull();

            UploadPath = uploadPath;
            GetFileName = getFileName;
        }
    }
}