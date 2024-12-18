//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from MyGrammar.g4 by ANTLR 4.7.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Maui.Calc;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="MyGrammarParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
public interface IMyGrammarVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="MyGrammarParser.compileUnit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompileUnit([NotNull] MyGrammarParser.CompileUnitContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>MinFunction</c>
	/// labeled alternative in <see cref="MyGrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMinFunction([NotNull] MyGrammarParser.MinFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Add</c>
	/// labeled alternative in <see cref="MyGrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAdd([NotNull] MyGrammarParser.AddContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Divide</c>
	/// labeled alternative in <see cref="MyGrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDivide([NotNull] MyGrammarParser.DivideContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Number</c>
	/// labeled alternative in <see cref="MyGrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumber([NotNull] MyGrammarParser.NumberContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Exponent</c>
	/// labeled alternative in <see cref="MyGrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExponent([NotNull] MyGrammarParser.ExponentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Parens</c>
	/// labeled alternative in <see cref="MyGrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParens([NotNull] MyGrammarParser.ParensContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>IdentifierExpr</c>
	/// labeled alternative in <see cref="MyGrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIdentifierExpr([NotNull] MyGrammarParser.IdentifierExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>UnaryPlus</c>
	/// labeled alternative in <see cref="MyGrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnaryPlus([NotNull] MyGrammarParser.UnaryPlusContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>MaxFunction</c>
	/// labeled alternative in <see cref="MyGrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMaxFunction([NotNull] MyGrammarParser.MaxFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Multiply</c>
	/// labeled alternative in <see cref="MyGrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMultiply([NotNull] MyGrammarParser.MultiplyContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Subtract</c>
	/// labeled alternative in <see cref="MyGrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSubtract([NotNull] MyGrammarParser.SubtractContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>UnaryMinus</c>
	/// labeled alternative in <see cref="MyGrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnaryMinus([NotNull] MyGrammarParser.UnaryMinusContext context);
}
internal class MyGrammarVisitor : MyGrammarBaseVisitor<double>
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
        var editedCellName = Calculator.sheet.EvaluatingCell;
        var resultCell = Calculator.sheet.Cells[result];
        Calculator.sheet.Cells[editedCellName].linkInCell.Add(result);
        if (Calculator.sheet.HasItself(result))
        {
            throw new System.Exception("���������� ���� �� ����");
        }

        if (!resultCell.linkedIn.Contains(editedCellName))
        {
            resultCell.linkedIn.Add(editedCellName);
        }

        return Convert.ToDouble(resultCell.Value);
    }
}