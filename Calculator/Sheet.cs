using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class Sheet
    {
        public IDictionary<string, Cell> Cells { get; }

        public string EvaluatingCell { get; private set; }

        public IList<string> UsedCells { get; }

        public Sheet()
        {
            Cells = new Dictionary<string, Cell>();
            EvaluatingCell = "";
            UsedCells = new List<string>();
        }

        public void EditCell(string cellName, string expression)
        {
            if (!Cells.ContainsKey(cellName))
                Cells[cellName] = new Cell();

            var cell = Cells[cellName];
            var oldExpression = cell.Expression;
            var oldDependencies = new List<string>(cell.linkInCell);

            cell.Expression = expression;
            UsedCells.Clear();

            try
            {
                RefreshRecursively(cellName);
            }
            catch
            {
                // Revert in case of an invalid expression or cyclic dependency
                cell.Expression = oldExpression;
                cell.linkInCell = oldDependencies;
                throw new InvalidOperationException("Invalid expression or circular dependency detected.");
            }
        }

        public void RefreshRecursively(string cellName)
        {
            if (!Cells.ContainsKey(cellName)) return;

            var cell = Cells[cellName];
            cell.linkInCell.Clear();
            EvaluatingCell = cellName;
            cell.Value = Calculator.Evaluate(cell.Expression).ToString();
            if (!UsedCells.Contains(cellName))
            {
                UsedCells.Add(cellName);
            }
            foreach (var dependentCellName in Cells[cellName].linkedIn.ToList())
            {
                if (Cells[dependentCellName].linkInCell.Contains(cellName))
                {
                    RefreshRecursively(dependentCellName);
                }
                else
                {
                    Cells[cellName].linkedIn.Remove(dependentCellName);
                }
            }
        }

        public bool HasItself(string dependentCellName)
        {
            return dependentCellName == EvaluatingCell || Cells[dependentCellName].linkInCell.Any(HasItself);
        }
    }
}

