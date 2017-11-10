using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Core.Interfaces;

namespace CoolBytes.Core.Builders
{
    public class ImageFile : IImageFile
    {
        public Func<Stream> OpenStream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
