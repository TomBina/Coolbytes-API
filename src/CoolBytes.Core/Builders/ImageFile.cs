using CoolBytes.Core.Interfaces;
using System;
using System.IO;

namespace CoolBytes.Core.Builders
{
    public class ImageFile : IImageFile
    {
        public Func<Stream> OpenStream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
