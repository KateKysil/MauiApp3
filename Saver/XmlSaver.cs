using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Maui.Calc;

namespace Saver
{
    public class XmlSaver
    {
        public string GenerateContent(IDictionary<string, Cell> cells, int rows, int columns)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<Cells Rows=\"{rows}\" Columns=\"{columns}\">");

            foreach (var one in cells)
            {
                string address = one.Key;
                Cell cell = one.Value;

                sb.AppendLine("  <Cell>");
                sb.AppendLine($"    <Address>{address}</Address>");
                sb.AppendLine($"    <Expression>{SecurityElement.Escape(cell.Expression)}</Expression>");
                sb.AppendLine($"    <Value>{SecurityElement.Escape(cell.Value)}</Value>");

                sb.AppendLine("    <LinkInCell>");
                foreach (var link in cell.linkInCell)
                {
                    sb.AppendLine($"      <Link>{link}</Link>");
                }
                sb.AppendLine("    </LinkInCell>");

                sb.AppendLine("    <LinkedIn>");
                foreach (var linked in cell.linkedIn)
                {
                    sb.AppendLine($"      <Link>{linked}</Link>");
                }
                sb.AppendLine("    </LinkedIn>");

                sb.AppendLine("  </Cell>");
            }

            sb.AppendLine("</Cells>");
            return sb.ToString();
        }

        public (IDictionary<string, Cell> Cells, int Rows, int Columns) ParseContent(string xmlContent)
        {
            var cells = new Dictionary<string, Cell>();
            int rows = 0, columns = 0;
            var document = XDocument.Parse(xmlContent);
            var root = document.Root;

            if (root == null || root.Name != "Cells")
                throw new InvalidOperationException("Wrong XML file: Root element is not 'Cells'.");

            if (int.TryParse(root.Attribute("Rows")?.Value, out int parsedRows))
                rows = parsedRows;

            if (int.TryParse(root.Attribute("Columns")?.Value, out int parsedColumns))
                columns = parsedColumns;
            foreach (var cellElement in document.Root.Elements("Cell"))
            {
                string address = cellElement.Element("Address")?.Value;
                string value = cellElement.Element("Value")?.Value ?? "";
                string expression = cellElement.Element("Expression")?.Value ?? "";

                var linkedIn = cellElement.Element("LinkedIn")?.Elements("Item").Select(x => x.Value).ToList() ?? new List<string>();
                var linkInCell = cellElement.Element("LinkInCell")?.Elements("Item").Select(x => x.Value).ToList() ?? new List<string>();

                cells[address] = new Cell
                {
                    Value = value,
                    Expression = expression,
                    linkedIn = linkedIn,
                    linkInCell = linkInCell
                };
            }

            return (cells, rows, columns);
        }

    }
}
