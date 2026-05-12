using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using KyroClient.Desktop.Main.ViewModels;

namespace KyroClient.Desktop.ResultConsole.Views;

public partial class ResultConsoleView : UserControl
{
    private static readonly SolidColorBrush BrushColumnName = new(Color.Parse("#BCBEC4"));
    private static readonly SolidColorBrush BrushTypeName = new(Color.Parse("#4D8CC8"));
    private static readonly SolidColorBrush BrushNullable = new(Color.Parse("#7A7E85"));

    public ResultConsoleView()
    {
        InitializeComponent();
        
        DataContextChanged += (_, _) =>
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.OnQueryExecuted = (table, elapsed) =>
                    Avalonia.Threading.Dispatcher.UIThread.Post(() => ShowResults(table, elapsed));
                vm.OnQueryFailed = msg =>
                    Avalonia.Threading.Dispatcher.UIThread.Post(() => ShowError(msg));
            }
        };
    }
    
    public void ShowResults(DataTable table, TimeSpan elapsed)
    {
        ResultsGrid.Columns.Clear();
        ResultsGrid.ItemsSource = null;

        foreach (DataColumn col in table.Columns)
        {
            ResultsGrid.Columns.Add(new DataGridTextColumn
            {
                Header = BuildHeader(col.ColumnName, MapTypeName(col.DataType), col.AllowDBNull),
                Binding = new Avalonia.Data.Binding($"C{col.Ordinal}")
                {
                    Mode = Avalonia.Data.BindingMode.OneWay
                },
                CanUserResize = true,
                CanUserSort = false,
            });
        }

        var rows = new ObservableCollection<GridRow>();
        foreach (DataRow row in table.Rows)
        {
            var arr = new string[table.Columns.Count];
            for (int i = 0; i < table.Columns.Count; i++)
                arr[i] = row[i] is DBNull ? "" : row[i]?.ToString() ?? "";
            rows.Add(new GridRow(arr));
        }

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
            Foreground = BrushColumnName,
            FontWeight = FontWeight.SemiBold,
        });

        panel.Children.Add(new TextBlock
        {
            Text = typeName,
            Foreground = BrushTypeName,
            FontSize = 10,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
        });

        if (nullable)
            panel.Children.Add(new TextBlock
            {
                Text = "?",
                Foreground = BrushNullable,
                FontSize = 10,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            });

        return panel;
    }

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

public class SimpleRow
{
    public string Col0 { get; set; } = "";
}

// todo: new file, maybe rewrite
public class DataRowWrapper(DataRow row)
{
    public DataRow Row => row;
}

public class GridRow
{
    private readonly string[] _values;
    public GridRow(string[] values) => _values = values;
    public string C0 => _values.ElementAtOrDefault(0) ?? "";
    public string C1 => _values.ElementAtOrDefault(1) ?? "";
    public string C2 => _values.ElementAtOrDefault(2) ?? "";
    public string C3 => _values.ElementAtOrDefault(3) ?? "";
    public string C4 => _values.ElementAtOrDefault(4) ?? "";
    public string C5 => _values.ElementAtOrDefault(5) ?? "";
    public string C6 => _values.ElementAtOrDefault(6) ?? "";
    public string C7 => _values.ElementAtOrDefault(7) ?? "";
    public string C8 => _values.ElementAtOrDefault(8) ?? "";
    public string C9 => _values.ElementAtOrDefault(9) ?? "";
}