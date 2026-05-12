using CommunityToolkit.Mvvm.ComponentModel;
using KyroClient.Core.Schemas.Models;
using KyroClient.Core.Schemas.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace KyroClient.Desktop.Sidebar.Viewmodels;

public partial class TableViewModel : ObservableObject
{
    private readonly ISchemaExplorer _explorer;
    private readonly string _database;
    public string Name { get; }
    public string Schema { get; }

    public ObservableCollection<ColumnInfo> Columns { get; } = [];

    [ObservableProperty]
    private bool isExpanded;
    private bool _loaded;

    public TableViewModel(
        ISchemaExplorer explorer,
        string database,
        TableInfo table)
    {
        _explorer = explorer;
        _database = database;

        Name = table.Name;
        Schema = table.Schema;
    }

    partial void OnIsExpandedChanged(bool value)
    {
        if (value)
            _ = LoadColumnsAsync();
    }

    private async Task LoadColumnsAsync()
    {
        if (_loaded)
            return;

        var cols = await _explorer.GetColumns(
            _database,
            Schema,
            Name);

        foreach (var col in cols)
            Columns.Add(col);

        _loaded = true;
    }
}
