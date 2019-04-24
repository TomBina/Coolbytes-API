using System;
using System.IO;

namespace CoolBytes.Core.Abstractions
{
    public interface IImageFile
    {
        Func<Stream> OpenStream { get; set; }
        string FileName { get; set; }
        string ContentType { get; set; }
    }
}