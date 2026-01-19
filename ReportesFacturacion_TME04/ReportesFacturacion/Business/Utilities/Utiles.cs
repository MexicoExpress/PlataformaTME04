using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities
{
    public class Utiles
    {
        public static string GeneraCodigo(int largo)
        {
            string s = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= largo; i++)
            {
                int idx = r.Next(0, s.Length - 1);
                sb.Append(s.Substring(idx, 1));
            }

            return sb.ToString();
        }
    }
}
