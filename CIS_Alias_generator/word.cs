using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS_Alias_generator
{

    class word
    {
        private string native;
        private string[] variations;
        private int exceptions;
        private int position;


        public word(string native, string[] variations, int exceptions, int position)
        {
            this.native = native;
            this.variations = variations;
            this.exceptions = exceptions;
            this.position = position;

        }

        public string getNative()
        {
            return native;
        }

        public string[] getVariation()
        {
            return variations;
        }

    }
}
