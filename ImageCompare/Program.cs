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
            ImageActions s = new ImageActions();
            var g = s.ProcessFiles(args);
            var d = s.RemoveDuplicates(g);
            foreach (var item in d)
            {
                s.CreateDirectoriesMoveFiles(item);
            }

            Console.WriteLine("Changed");
            Console.ReadLine();
        }
    }
}