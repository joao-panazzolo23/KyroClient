using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KyroClient.Core.Schemas.Models;
using KyroClient.Core.Schemas.Services;

namespace KyroClient.Desktop.Main.ViewModels;

public partial class MainWindowViewModel(ISchemaExplorer explorer) : ViewModelBase
{
    [ObservableProperty] public partial ObservableCollection<DatabaseInfo> Databases { get; private set; } = [];
    [ObservableProperty] public partial ObservableCollection<TableInfo> Tables { get; private set; } = [];
    [ObservableProperty] public partial DatabaseInfo CurrentDatabase { get; private set; }
    partial void OnCurrentDatabaseChanged(DatabaseInfo? value)
    {
        if (value is null) return;
        LoadTablesCommand.Execute(value.Name);
    }

    [RelayCommand]
    private async Task LoadDatabases(CancellationToken ct)
    {
        var result = await explorer.GetDatabases(ct);
        Databases = new ObservableCollection<DatabaseInfo>(result);
    }
    [RelayCommand]
    private async Task LoadTables(string database, CancellationToken ct)
    {
        var result = await explorer.GetTables(database, ct);
        Tables = new ObservableCollection<TableInfo>(result);
    }
    
}