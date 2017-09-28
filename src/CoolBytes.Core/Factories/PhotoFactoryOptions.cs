using System;
using CoolBytes.Core.Extensions;

namespace CoolBytes.Core.Factories
{
    public class PhotoFactoryOptions
    {
        public string UploadPath { get; }
        public Func<string, string> FileName { get; } =
            fileExtension => $"{Guid.NewGuid().ToString().ToLower()}{fileExtension}";
        public Func<string, string, string> Directory { get; } =
            (directory, fileName) => $@"{directory}\{fileName.Substring(0, 3)}";
        public Func<string, string> UriPath { get; } =
            fileName => $@"/{fileName.Substring(0, 3)}/{fileName}";

        public PhotoFactoryOptions(string uploadPath)
        {
            uploadPath.IsNotNullOrWhiteSpace();
            UploadPath = uploadPath;
        }

        public PhotoFactoryOptions(string uploadPath, 
                                   Func<string, string> fileName, 
                                   Func<string, string, string> directory, 
                                   Func<string, string> uriPath) : this(uploadPath)
        {
            fileName.IsNotNull();
            FileName = fileName;

            directory.IsNotNull();
            Directory = directory;

            uriPath.IsNotNull();
            UriPath = uriPath;
        }
    }
}