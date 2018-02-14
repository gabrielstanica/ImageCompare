using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageCompare
{
    class ImageProperties
    {
        public string ImagePath { get; set; }
        public string Base64 { get; set; }
        public DateTime CreationDate { get; set; }
        public Size Resolution { get; set; }
        public long Size { get; set; }
        public string ImageName { get; set; }
        public bool ToDelete { get; set; }

        public ImageProperties(string imagePath)
        {
            this.ImagePath = imagePath;
            Regex r = new Regex(":");
            while (true)
            {
                try
                {
                    using (Image image = Image.FromFile(imagePath))
                    {
                        try
                        {
                            CreationDate = DateTime.Parse(r.Replace(Encoding.UTF8.GetString(image.GetPropertyItem(36867).Value), "-", 2));
                        }
                        catch (Exception ex)
                        {
                            Console.Write("\r  Creation date couldn't be taken, falling back to modified date");
                            //Console.WriteLine("Creation date couldn't be taken, falling back to modified date");
                            CreationDate = File.GetLastWriteTime(ImagePath);
                        }
                        Resolution = image.Size;
                        FileInfo details = new FileInfo(ImagePath);
                        Size = details.Length;
                        ImageName = details.Name;
                        image.Dispose();
                    }
                    break;
                }
                catch (OutOfMemoryException ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public ImageProperties(ImageProperties image)
        {
            byte[] imageArray = File.ReadAllBytes(image.ImagePath);
            Base64 = Convert.ToBase64String(imageArray);
            Array.Clear(imageArray, 0, imageArray.Length);
            //Base64 = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(imagePath));
        }
    }
}
