using System;
using System.IO;

namespace CoolBytes.Core.Interfaces
{
    public interface IImageFile
    {
        Func<Stream> OpenStream { get; set; }
        string FileName { get; set; }
        string ContentType { get; set; }
    }
}