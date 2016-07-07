using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCompare.Interface
{
    interface IImageActions
    {
        List<List<ImageProperties>> ProcessFiles(string[] path);
        List<List<ImageProperties>> RemoveDuplicates(List<List<ImageProperties>> list);
        void CreateDirectoriesMoveFiles(List<ImageProperties> list);
    }
}
