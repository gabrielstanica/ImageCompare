using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageCompare
{
    class AllImages
    {
        public string CopyImagePath
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
                    Console.WriteLine("----------------------");
                    var imageName = String.Format("{1}{2}{3}_{0}", firstImage.ImageName, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    File.Move(firstImage.ImagePath, Path.Combine(CopyImagePath, imageName));
                    //File.Delete(firstImage.ImagePath);
                    firstImage.ToDelete = true;
                    return true;
                }
            }
            return false;
        }

        public void Method(string[] path)
        {
            List<List<ImageProperties>> listOfImages = new List<List<ImageProperties>>();
            List<ImageProperties> temList = new List<ImageProperties>();
            List<List<string>> imageFiles = new List<List<string>>();

            List<string[]> allFiles = new List<string[]>();
            allFiles = GetDirectories(path);

            foreach (var directory in allFiles)
            {
                imageFiles.Add(GetImages(directory));
            }

            foreach (var file in imageFiles)
            {
                Console.WriteLine(String.Format("Total images to be processed {0}", file.Count));
                for (int i = 0; i < file.Count; i++)
                {
                    Console.Write("\rImage {0}/{1} ", i + 1, file.Count);
                    //Console.WriteLine(String.Format("Image {0} processed", i));
                    temList.Add(new ImageProperties(file[i]));
                }

                for (int i = 0; i < temList.Count - 1; i++)
                {
                    for (int j = i + 1; j < temList.Count; j++)
                    {
                        var found = CompareTwoImages(temList[i], temList[j]);
                        if (found)
                        {
                            break;
                        }
                    }
                }
                listOfImages.Add(temList);
            }
        }
    }
}
