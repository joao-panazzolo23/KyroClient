using CommunityToolkit.Mvvm.ComponentModel;
using KyroClient.Core.Schemas.Enums;
using KyroClient.Core.Schemas.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace KyroClient.Desktop.Sidebar.Sidebar;

public partial class SidebarTableInfo : ObservableObject
{
    public string Schema { get; set; } = "";
    public string Name { get; set; } = "";
    public TableKind Kind { get; set; }
    public ObservableCollection<ColumnInfo> Columns { get; set; } = [new ColumnInfo { Name = "loading..." }];
    [ObservableProperty]
    public bool isExpanded;
    [ObservableProperty]
    private bool isLoading;
    public bool ColumnsLoaded { get; set; }

    // A ponte para o carregamento
    public Func<SidebarTableInfo, Task>? OnExpandedAsync { get; set; }

    partial void OnIsExpandedChanged(bool value)
    {
        // Só dispara se estiver expandindo e ainda não tiver carregado
        if (value && !ColumnsLoaded && !IsLoading)
        {
            // Dispara sem travar a thread de UI
            OnExpandedAsync?.Invoke(this);
        }
    }

}
