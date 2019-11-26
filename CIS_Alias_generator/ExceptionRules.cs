using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS_Alias_generator
{
    class ExceptionRules
    {

        private string condition;
        private string indicators;
        private string symbol;
        private string[] replacement;

        public ExceptionRules(string condition, string indicators, string symbol, string[] replacement)
        {
            this.condition = condition;
            this.indicators = indicators;
            this.symbol = symbol;
            this.replacement = replacement;
        }

        public string getCondition()
        {
            return condition;
        }
        public string getSymbol()
        {
            return symbol;
        }
        public string[] getReplacement()
        {
            return replacement;
        }
        public string getIndicator()
        {
            return indicators;
        }
    }
}
