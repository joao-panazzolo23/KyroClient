using Avalonia.Controls;
using KyroClient.Desktop.Main.ViewModels;
using KyroClient.Desktop.Sidebar.Views;

namespace KyroClient.Desktop.Main.Views;

public partial class MainWindow : Window
{
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
}