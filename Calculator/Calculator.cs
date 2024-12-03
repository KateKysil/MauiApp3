using Antlr4.Runtime;

namespace Calc
{
    public static class Calculator
    {
        public static Sheet SheetC {  get; }
        static Calculator() {
            SheetC = new Sheet();
        }
        public static double Evaluate(string expression)
        {
            var lexer = new MyGrammarLexer(new AntlrInputStream(expression));
            var tokens = new CommonTokenStream(lexer);
            var parser = new MyGrammarParser(tokens);
            var tree = parser.compileUnit();
            var visitor = new MyGrammarVisitorr();
            double s = visitor.Visit(tree);
            s = s + 0;
            return s;
        }
    }
}
