using System;
using System.Collections.Generic;
using System.Data;
using Avalonia.Controls;
using Avalonia.Media;

namespace KyroClient.Desktop.ResultConsole.Views;

public partial class ResultConsoleView : UserControl
{
    public ResultConsoleView()
    {
        InitializeComponent();
    }

    public void ShowResults(DataTable table, TimeSpan elapsed)
    {
        ResultsGrid.Columns.Clear();

        // build columns dynamically from DataTable schema
        foreach (DataColumn col in table.Columns)
        {
            var isNullable = col.AllowDBNull;
            var typeName = MapTypeName(col.DataType);
            var header = BuildHeader(col.ColumnName, typeName, isNullable);

            ResultsGrid.Columns.Add(new DataGridTextColumn
            {
                Header = header,
                Binding = new Avalonia.Data.Binding($"[{col.Ordinal}]"),
                CanUserResize = true,
                CanUserSort = true,
            });
        }

        // wrap rows for indexer binding
        var rows = new List<DataRowWrapper>();
        foreach (DataRow row in table.Rows)
            rows.Add(new DataRowWrapper(row));

        ResultsGrid.ItemsSource = rows;

        RowCountText.Text = $"{table.Rows.Count} rows";
        ElapsedText.Text = $"{elapsed.TotalMilliseconds:F0} ms";
    }

    public void ShowError(string message)
    {
        ResultsGrid.Columns.Clear();
        ResultsGrid.ItemsSource = null;
        RowCountText.Text = message;
        ElapsedText.Text = "";
    }

    private static Control BuildHeader(string name, string typeName, bool nullable)
    {
        var panel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 4 };

        panel.Children.Add(new TextBlock
        {
            Text = name,
            Foreground = new SolidColorBrush(Color.Parse("#BCBEC4")),
            FontWeight = FontWeight.SemiBold,
        });

        panel.Children.Add(new TextBlock
        {
            Text = typeName,
            Foreground = new SolidColorBrush(Color.Parse("#4D8CC8")),
            FontSize = 10,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
        });

        if (nullable)
            panel.Children.Add(new TextBlock
            {
                Text = "?",
                Foreground = new SolidColorBrush(Avalonia.Media.Color.Parse("#7A7E85")),
                FontSize = 10,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            });

        return panel;
    }

    //todo: refactor
    private static string MapTypeName(Type type) => type switch
    {
        _ when type == typeof(int) || type == typeof(long) || type == typeof(short) => "int",
        _ when type == typeof(string) => "text",
        _ when type == typeof(bool) => "bool",
        _ when type == typeof(DateTime) => "timestamp",
        _ when type == typeof(decimal) || type == typeof(double) || type == typeof(float) => "numeric",
        _ when type == typeof(Guid) => "uuid",
        _ when type == typeof(byte[]) => "bytea",
        _ => type.Name.ToLower()
    };
}

// todo: new file, maybe rewrite
public class DataRowWrapper(DataRow row)
{
    public object? this[int index] => row[index] is DBNull ? null : row[index];
}