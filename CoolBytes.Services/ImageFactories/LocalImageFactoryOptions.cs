using System;
using CoolBytes.Core.Attributes;
using CoolBytes.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBytes.Services.ImageFactories
{
    [Inject(typeof(IImageFactoryOptions), ServiceLifetime.Scoped, "production")]
    public class LocalImageFactoryOptions : IImageFactoryOptions
    {
        public string UploadPath { get; }
        public Func<string, string> FileName { get; } =
            fileExtension => $"{Guid.NewGuid().ToString().ToLower()}{fileExtension}";
        public Func<string, string, string> Directory { get; } =
            (directory, fileName) => $@"{directory}\{fileName.Substring(0, 3)}";
        public Func<string, string> UriPath { get; } =
            fileName => $@"/images/{fileName.Substring(0, 3)}/{fileName}";

        public LocalImageFactoryOptions(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            UploadPath = configuration["ImagesUploadPath"] ?? throw new ArgumentNullException(nameof(UploadPath));
        }

        public LocalImageFactoryOptions(string uploadPath, 
                                   Func<string, string> fileName, 
                                   Func<string, string, string> directory, 
                                   Func<string, string> uriPath)
        {
            UploadPath = uploadPath;
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            Directory = directory ?? throw new ArgumentNullException(nameof(directory));
            UriPath = uriPath;
        }
    }
}