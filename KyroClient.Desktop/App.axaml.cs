using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using KyroClient.Desktop.Main.ViewModels;
using KyroClient.Desktop.Main.Views;
using KyroClient.Desktop.Sidebar.Views;
using Material.Icons.Avalonia;
using Microsoft.Extensions.DependencyInjection;

namespace KyroClient.Desktop;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        //todo: separate this into something else
        var collection = new ServiceCollection();
        collection.AddSingleton<MainWindowViewModel>();
        collection.AddSingleton<SidebarView>();
        collection.AddSingleton<MainWindow>();
        var provider = collection.BuildServiceProvider();
        Styles.Add(new MaterialIconStyles(provider));

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = provider.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}