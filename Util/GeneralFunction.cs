using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PinBackendSystem.Util
{
    public class GeneralFunction
    {
        //public byte[] ResizeImage(byte[] ImageInBytes)
        //{

        //    //first MS for reading Image from byte[] ImageInBytes
        //    using (var ms = new MemoryStream(ImageInBytes))
        //    {
        //        //Run Resizer
        //        var settings = new ResizeSettings("maxwidth=3200;maxheight=3200;format=jpg;mode=max");

        //        //second MS for the ImageResizer to workon
        //        using (var returnms = new MemoryStream())
        //        {
        //            ImageResizer.ImageBuilder.Current.Build(Image.FromStream(ms), returnms, settings);

        //            //return byte[]
        //            return returnms.ToArray();

        //        }

        //    }

        //}

    }
}
