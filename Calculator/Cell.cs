using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class Cell
    {
        public string Value { get; set; }
        public string Expression {  get; set; }
        public List<string> linkedIn {  get; }
        public List<string> linkInCell { get; set; }

        public Cell():this("") { }
        public Cell(string expression)
        {
            Expression = expression;
            linkedIn = new List<string>();
            linkInCell = new List<string>();
            Value = "";
        }
    }
}
