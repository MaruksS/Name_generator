using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS_Alias_generator
{
    class transliteration
    {
        private string original;
        private string translit;

        public transliteration(string original, string translit)
        {
            this.original = original;
            this.translit = translit;
        }

        public void setOriginal(string original)
        {
            this.original = original;
        }
        public void setTranslit(string translit)
        {
            this.translit = translit;
        }

        public string getOriginal()
        {
            return original;
        }
        public string getTranslit()
        {
            return translit;
        }
    }
}
