using Utilities;

namespace My_Table;
public class MyTable
{
    private readonly List<string[]> Rows;
    private readonly int ColumnCount;
    private readonly int[] ColumnLength;

    public MyTable(string[] column)
    {
        ColumnCount = column.Length;
        ColumnLength = new int[ColumnCount];

        for (int i = 0; i < column.Length; i++)
        {
            ColumnLength[i] = Math.Max(ColumnLength[i], column[i].Length);
        }

        Rows = new List<string[]>();
        this.Rows.Add(column);
    }

    public void AddRow(string[] row)
    {
        if (row.Length != ColumnCount)
        {
            throw new ArgumentException($"Add {ColumnCount} no of data to rows.");
        }

        for (int i = 0; i < row.Length; i++)
        {
            ColumnLength[i] = Math.Max(ColumnLength[i], row[i].Length);
        }
        this.Rows.Add(row);
    }

    public void DrawTable(bool centerTable = true)
    {
        if (Rows.Count < 2) return;

        List<string> rows = GetFormattedRow();

        int maxWidth = ColumnLength.Sum() + ColumnCount * 3;
        string separator = new('-', maxWidth);

        foreach (string row in rows)
        {
            if (centerTable)
            {
                Utils.CenterConsoleText(separator);
                Utils.CenterConsoleText(row);
            }
            else
            {
                Console.WriteLine(separator);
                Console.WriteLine(row);
            }
            Thread.Sleep(100);
        }
        if (centerTable) Utils.CenterConsoleText(separator);
        else Console.WriteLine(separator);
    }

    public List<string> GetFormattedRow()
    {
        List<string> formattedRow = new();

        foreach (var row in Rows)
        {
            string rowData = "";
            for (int i = 0; i < ColumnCount; i++)
            {
                rowData += $"|{PadStringWithSpaces(row[i], ColumnLength[i] + 2)}";
                rowData += i == ColumnCount - 1 ? "|" : "";
            }
            formattedRow.Add(rowData);
        }

        return formattedRow;
    }

    public static string PadStringWithSpaces(string text, int length)
    {
        string leftPad = new(' ', (length - text.Length) / 2);
        string rightPad = new(' ', length - text.Length - leftPad.Length);
        return leftPad + text + rightPad;
    }
}
