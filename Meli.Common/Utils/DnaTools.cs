using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meli.Common.Utils
{
    public class DnaTools
    {

        public static string GenerateDnaSecuence(List<string> dna) 
        {
            return string.Join(' ', dna.ToArray()).Replace(" ","").ToUpper();
        }
    }
}
