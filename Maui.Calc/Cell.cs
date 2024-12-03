namespace Maui.Calc
{
    public class Cell
    {
        public string Value { get; set; }
        public string Expression { get; set; }
        public List<string> linkedIn { get; set; }
        public List<string> linkInCell { get; set; }

        public Cell()
        {
            Value = "";
            Expression = "";
            linkedIn = new List<string>();
            linkInCell = new List<string>();
        }
        public Cell(string expression)
        {
            Value = "";
            Expression = expression;
            linkedIn = new List<string>();
            linkInCell = new List<string>();

        }
    }

}
