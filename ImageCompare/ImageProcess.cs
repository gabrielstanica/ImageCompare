using ImageCompare.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageCompare
{
    class ImageProcess : IImageProcess
    {
        private string CopyImagePath
        {
            get
            {
                return ConfigurationManager.AppSettings["BeforMovePath"]; ;
            }
        }

        public List<string[]> GetDirectories(string[] allDirectories)
        {
            List<string[]> directoryList = new List<string[]>();

            foreach (var item in allDirectories)
            {
                if (new DirectoryInfo(item).Exists)
                {
                    directoryList.Add(Directory.GetFiles(item, "*.*", SearchOption.AllDirectories));
                }
                else
                {
                    Console.WriteLine(String.Format("A valid path should be provided - {0}", item));
                }
            }
            return directoryList;
        }

        public List<string> GetImages(string[] listFiles)
        {
            List<string> imageFiles = new List<string>();
            List<string> noImageFiles = new List<string>();

            foreach (string filename in listFiles)
            {
                if (Regex.IsMatch(filename.ToLower(), @".jpg|.png|.gif$"))
                {
                    imageFiles.Add(filename);
                }
                else
                {
                    noImageFiles.Add(filename);
                }
            }
            return imageFiles;
        }

        public bool CompareTwoImages(ImageProperties firstImage, ImageProperties secondImage)
        {

            try
            {
                if (firstImage.Size.Equals(secondImage.Size) && firstImage.Resolution.Equals(secondImage.Resolution))
                {
                    var first = new ImageProperties(firstImage);
                    var second = new ImageProperties(secondImage);
                    if (first.Base64.Equals(second.Base64))
                    {
                        Console.WriteLine("----------------------");
                        Console.WriteLine("Image equals by base64");
                        Console.WriteLine(firstImage.ImagePath);
                        Console.WriteLine(secondImage.ImagePath);
                        Console.WriteLine("----------------------");

                        if (!new DirectoryInfo(CopyImagePath).Exists)
                        {
                            Directory.CreateDirectory(CopyImagePath);
                        }

                        Console.WriteLine(String.Format("Moved image {0}", firstImage.ImagePath));
                        Console.WriteLine(String.Format("Moved image {0}", secondImage.ImagePath));
                        Console.WriteLine("----------------------");
                        var currentDate = DateTime.Now;
                        var firstImageName = String.Format("{0}_{1}{2}{3}{4}", firstImage.ImageName, currentDate.Hour, currentDate.Minute, currentDate.Second, currentDate.Millisecond);
                        var secondImageName = String.Format("{0}_{1}{2}{3}{4}Copy", secondImage.ImageName, currentDate.Hour, currentDate.Minute, currentDate.Second, currentDate.Millisecond);

                        File.Move(firstImage.ImagePath, Path.Combine(CopyImagePath, firstImageName));
                        File.Copy(secondImage.ImagePath, Path.Combine(CopyImagePath, secondImageName));
                        //File.Delete(firstImage.ImagePath);
                        firstImage.ToDelete = true;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }
}
