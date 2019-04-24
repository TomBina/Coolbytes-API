using System;

namespace CoolBytes.Core.Abstractions
{
    public interface IImageFactoryOptions
    {
        string UploadPath { get; }
        Func<string, string> FileName { get; }
        Func<string, string, string> Directory { get; }
        Func<string, string> UriPath { get; }
    }
}