using ImageCompare.Interface;
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
    class ImageActions : IImageActions
    {
        private ImageProcess ProcessActions;
        private string NewPath
        {
            get
            {
                return ConfigurationManager.AppSettings["NewLocation"]; ;
            }
        }

        public ImageActions()
        {
            ProcessActions = new ImageProcess();
        }

        public List<List<ImageProperties>> ProcessFiles(string[] path)
        {
            List<List<ImageProperties>> listOfImages = new List<List<ImageProperties>>();
            List<ImageProperties> temList = new List<ImageProperties>();
            List<List<string>> imageFiles = new List<List<string>>();

            List<string[]> allFiles = new List<string[]>();
            allFiles = ProcessActions.GetDirectories(path);

            foreach (var directory in allFiles)
            {
                imageFiles.Add(ProcessActions.GetImages(directory));
            }

            foreach (var file in imageFiles)
            {
                Console.WriteLine(String.Format("Total images to be processed {0}", file.Count));
                for (int i = 0; i < file.Count; i++)
                {
                    Console.Write("\rImage {0}/{1} ", i + 1, file.Count);
                    temList.Add(new ImageProperties(file[i]));
                }

                listOfImages.Add(temList);
            }
            return listOfImages;
        }
        public List<List<ImageProperties>> RemoveDuplicates(List<List<ImageProperties>> list)
        {
            List<List<ImageProperties>> newList = new List<List<ImageProperties>>();

            foreach (var temList in list)
            {
                for (int i = 0; i < temList.Count - 1; i++)
                {
                    for (int j = i + 1; j < temList.Count; j++)
                    {
                        var found = ProcessActions.CompareTwoImages(temList[i], temList[j]);
                        if (found)
                        {
                            break;
                        }
                    }
                }
                newList.Add(temList);
            }
            return newList;
        }
        public void CreateDirectoriesMoveFiles(List<ImageProperties> list)
        {
            foreach(var item in list)
            {
                MoveFile(item);
            }
        }
        private void MoveFile(ImageProperties image)
        {
            // https://csharp.today/log4net-tutorial-great-library-for-logging/
            var directoryDate = String.Format("{0}.{1}", image.CreationDate.Year,image.CreationDate.Month);
            var newImagesPath = Path.Combine(NewPath, directoryDate);
            if(!Directory.Exists(newImagesPath))
            {
                Directory.CreateDirectory(newImagesPath);
            }
            Console.Write(String.Format("\rMoving image {0}", image.ImagePath));
            var fileName = Path.Combine(newImagesPath, image.ImageName);
            var currentDate = DateTime.Now;
            if (File.Exists(fileName))
            {
                fileName.Replace(image.ImageName, String.Format("{0}{1}{2}", image.ImageName,currentDate.Minute,currentDate.Millisecond));
            }
            File.Move(image.ImagePath, Path.Combine(newImagesPath, fileName));
        }

    }
}
