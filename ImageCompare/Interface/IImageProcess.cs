using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCompare.Interface
{
    interface IImageProcess
    {
        List<string[]> GetDirectories(string[] allDirectories);
        List<string> GetImages(string[] listFiles);
        bool CompareTwoImages(ImageProperties firstImage, ImageProperties secondImage);
    }
}
