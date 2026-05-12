using Avalonia.Controls;
using Avalonia.Interactivity;
using KyroClient.Desktop.Main.ViewModels;
using KyroClient.Desktop.Sidebar.Sidebar;
using KyroClient.Desktop.Sidebar.Views;

namespace KyroClient.Desktop.Main.Views;

public partial class MainWindow : Window
{
    public MainWindowViewModel _viewmodel { get; set; }
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.LoadDatabasesCommand.ExecuteAsync(null);

        var sidebar = this.FindControl<SidebarView>("Sidebar")!;

        sidebar.PropertyChanged += (_, e) =>
        {
            if (e.Property != SidebarView.IsPanelOpenProperty) return;
            var isOpen = (bool)e.NewValue!;
            var col = (this.Content as Grid)!.ColumnDefinitions[0];
            col.MinWidth = isOpen ? 200 : 48;
            col.Width = isOpen ? new GridLength(280) : new GridLength(48);
        };
    }

    private async void TreeViewItem_Expanded(
        object? sender,
        RoutedEventArgs e)
    {
        if (sender is TreeViewItem item &&
            item.DataContext is SidebarTableInfo table)
        {
            await _viewmodel.ExpandTableAsync(table);
        }
    }
}