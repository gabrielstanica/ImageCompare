using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            AllImages s = new AllImages();
            s.Method(args);
            
            Console.WriteLine("Changed");
            Console.ReadLine();
        }
    }
}