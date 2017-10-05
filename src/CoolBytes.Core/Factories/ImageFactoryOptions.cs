using System;
using CoolBytes.Core.Extensions;
using CoolBytes.Core.Interfaces;

namespace CoolBytes.Core.Factories
{
    public class ImageFactoryOptions : IImageFactoryOptions
    {
        public string UploadPath { get; }
        public Func<string, string> FileName { get; } =
            fileExtension => $"{Guid.NewGuid().ToString().ToLower()}{fileExtension}";
        public Func<string, string, string> Directory { get; } =
            (directory, fileName) => $@"{directory}\{fileName.Substring(0, 3)}";
        public Func<string, string> UriPath { get; } =
            fileName => $@"/images/{fileName.Substring(0, 3)}/{fileName}";

        public ImageFactoryOptions(string uploadPath)
        {
            uploadPath.IsNotNullOrWhiteSpace();
            UploadPath = uploadPath;
        }

        public ImageFactoryOptions(string uploadPath, 
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