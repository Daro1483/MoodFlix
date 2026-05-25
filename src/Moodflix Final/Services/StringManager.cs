using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StringManager
    {
        public static string ExtraerPalabraAntesPng(string path)
        {

            int pngIndex = path.LastIndexOf(".png", StringComparison.OrdinalIgnoreCase);
            int slashIndex = path.LastIndexOf('/');

            string word = path.Substring(slashIndex + 1, pngIndex - slashIndex - 1);

            return word;
        }
    }
}
