using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    internal class MyGrammarVisitorr: MyGrammarBaseVisitor<double>
    {
        public override double VisitUnaryMinus(MyGrammarParser.UnaryMinusContext context)
        {
            double value = Visit(context.expression());
            return -value; 
        }
        public override double VisitUnaryPlus(MyGrammarParser.UnaryPlusContext context)
        {
            double value = Visit(context.expression());
            return value; 
        }
        public override double VisitExponent(MyGrammarParser.ExponentContext context)
        {
            double baseValue = Visit(context.expression(0));
            double exponentValue = Visit(context.expression(1));
            return Math.Pow(baseValue, exponentValue); 
        }
        public override double VisitMaxFunction(MyGrammarParser.MaxFunctionContext context)
        {
            double value1 = Visit(context.expression(0));
            double value2 = Visit(context.expression(1));
            return Math.Max(value1, value2);
        }

        public override double VisitMinFunction(MyGrammarParser.MinFunctionContext context)
        {
            double value1 = Visit(context.expression(0));
            double value2 = Visit(context.expression(1));
            return Math.Min(value1, value2);
        }
        public override double VisitAdd(MyGrammarParser.AddContext context)
        {
            double left = Visit(context.expression(0)); 
            double right = Visit(context.expression(1));
            return left + right;
        }
        public override double VisitSubtract(MyGrammarParser.SubtractContext context)
        {
            double left = Visit(context.expression(0));
            double right = Visit(context.expression(1));
            return left - right;
        }
        public override double VisitMultiply(MyGrammarParser.MultiplyContext context)
        {
            double left = Visit(context.expression(0));
            double right = Visit(context.expression(1));
            return left * right;
        }
        public override double VisitDivide(MyGrammarParser.DivideContext context)
        {
            double left = Visit(context.expression(0));
            double right = Visit(context.expression(1));
            if (right == 0) throw new DivideByZeroException("Division by zero.");
            return left / right;
        }
        public override double VisitNumber(MyGrammarParser.NumberContext context)
        {
            //return double.Parse(context.GetText());
            double a = Convert.ToDouble(context.GetText());
            return a;
        }
        public override double VisitParens(MyGrammarParser.ParensContext context)
        {
            return Visit(context.expression());
        }
        public override double VisitIdentifierExpr([NotNull] MyGrammarParser.IdentifierExprContext context)
        {
            var result = context.GetText();
            var editedCellName = Calculator.SheetC.EvaluatingCell;
            var resultCell = Calculator.SheetC.Cells[result];
            Calculator.SheetC.Cells[editedCellName].linkInCell.Add(result);
            if (Calculator.SheetC.HasItself(result))
            {
                throw new System.Exception("Посилається саме на себе");
            }

            if (!resultCell.linkedIn.Contains(editedCellName))
            {
                resultCell.linkedIn.Add(editedCellName);
            }

            return Convert.ToDouble(resultCell.Value);
        }
    }
}
