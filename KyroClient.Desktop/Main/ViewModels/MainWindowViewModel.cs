using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KyroClient.Core.Schemas.Models;
using KyroClient.Core.Schemas.Services;
using KyroClient.Desktop.Sidebar.Sidebar;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KyroClient.Desktop.Main.ViewModels;

public partial class MainWindowViewModel(
    ISchemaExplorer explorer
    ) : ViewModelBase
{

    [ObservableProperty] public partial ObservableCollection<DatabaseInfo> Databases { get; set; } = [];
    [ObservableProperty] public partial ObservableCollection<SidebarTableInfo> Tables { get; set; } = [];
    [ObservableProperty] public partial DatabaseInfo CurrentDatabase { get; set; }

    private readonly Dictionary<string, IReadOnlyList<ColumnInfo>> _columnsCache = [];
    [RelayCommand]
    public async Task ExpandTable(SidebarTableInfo table)
    {
        await ExpandTableAsync(table);
    }
    public async Task ExpandTableAsync(
    SidebarTableInfo table,
    CancellationToken ct = default)
    {
        if (table.ColumnsLoaded || table.IsLoading)
            return;

        table.IsLoading = true;

        try
        {
            var key = BuildKey(
                CurrentDatabase.Name,
                table.Schema,
                table.Name);

            if (!_columnsCache.TryGetValue(key, out var columns))
            {
                columns = await explorer.GetColumns(
                    CurrentDatabase.Name,
                    table.Schema,
                    table.Name,
                    ct);

                _columnsCache[key] = columns;
            }

            //cleans the old mocked data
            table.Columns.Clear();

            foreach (var column in columns)
                table.Columns.Add(column);

            table.ColumnsLoaded = true;
        }
        finally
        {
            table.IsLoading = false;
        }
    }

    partial void OnCurrentDatabaseChanged(DatabaseInfo value)
    {
        if (value is null) return;
        LoadTablesCommand.ExecuteAsync(value.Name);
    }

    [RelayCommand]
    private async Task LoadDatabases(CancellationToken ct)
    {
        var result = await explorer.GetDatabases(ct);
        Databases = new ObservableCollection<DatabaseInfo>(result);
    }

    [RelayCommand]
    private async Task LoadTables(
        string database,
        CancellationToken ct
        )
    {
        var result = await explorer.GetTables(database, ct);
        Tables = new ObservableCollection<SidebarTableInfo>(
            result.Select(x => new SidebarTableInfo
            {
                Name = x.Name,
                Kind = x.Kind,
                Schema = x.Schema,
                IsLoading = false,
                ColumnsLoaded = false,
                Columns = [new ColumnInfo { Name = "loading..." }], //-> avalonia just renders chevron when there is at least one item in the list (gambiarra)
                OnExpandedAsync = async (t) => await ExpandTableAsync(t)
            }));
    }

    private static string BuildKey(
    string database,
    string schema,
    string table)
    => $"{database}:{schema}:{table}";
}




