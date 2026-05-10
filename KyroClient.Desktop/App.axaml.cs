using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using KyroClient.Desktop.Main.ViewModels;
using KyroClient.Desktop.Main.Views;
using KyroClient.Desktop.Sidebar.Views;
using KyroClient.PostgreSql;
using Material.Icons.Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace KyroClient.Desktop;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var provider = BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = provider.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    //todo: separate this into something else
    private IServiceProvider BuildServiceProvider()
    {
        var collection = new ServiceCollection();
        collection.AddSingleton<MainWindowViewModel>();
        collection.AddSingleton<SidebarView>();
        collection.AddSingleton<MainWindow>();
        collection.AddPostgreSql();
        //TODO: Providers will be dinamically set through USER INPUT.
        //At this very moment, this is not possible.
        //Change it into some external file and do a separated interface to create a new database connection
        collection.AddTransient<NpgsqlConnection>(_ =>
            new NpgsqlConnection("Host=localhost;Database=postgres;Username=postgres;Password=postgres"));
        var provider = collection.BuildServiceProvider();
        Styles.Add(new MaterialIconStyles(provider));

        return provider;
    }
}