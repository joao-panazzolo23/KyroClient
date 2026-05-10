using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Material.Icons.Avalonia;

namespace KyroClient.Desktop.Sidebar.Views;

public partial class SidebarView : UserControl
{
    private string? _activePanel;

    public SidebarView()
    {
        InitializeComponent();
        // ShowPanel("Connections");
        PanelContainer.IsVisible = false;
        IsPanelOpen = false;
    }

    private void OnIconClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        var tag = btn.Tag?.ToString();
        if (tag == null) return;

        if (_activePanel == tag)
        {
            PanelContainer.IsVisible = false;
            IsPanelOpen = false;
            IsConnectionsPanel = false;
            _activePanel = null;
            SetActiveButton(null);
        }
        else
        {
            ShowPanel(tag);
        }
    }

    public static readonly StyledProperty<bool> IsPanelOpenProperty =
        AvaloniaProperty.Register<SidebarView, bool>(nameof(IsPanelOpen), defaultValue: true);

    public bool IsPanelOpen
    {
        get => GetValue(IsPanelOpenProperty);
        set => SetValue(IsPanelOpenProperty, value);
    }

    private void OnClosePanel(object? sender, RoutedEventArgs e)
    {
        PanelContainer.IsVisible = false;
        _activePanel = null;
        IsPanelOpen = false;
        SetActiveButton(null);
    }

    private void ShowPanel(string panelName)
    {
        IsConnectionsPanel = panelName == "Connections";
        _activePanel = panelName;
        IsPanelOpen = true;
        PanelContainer.IsVisible = true;
        PanelTitle.Text = panelName switch
        {
            "Connections" => "Connections",
            "SwitchDb" => "Switch Database",
            "Settings" => "Settings",
            _ => panelName
        };
        SetActiveButton(panelName);
        
        PanelContent.Content = panelName switch
        {
            "Connections" => new ConnectionsSidebarView() { DataContext = DataContext },
            "SwitchDb"    => new TextBlock { Text = "Coming soon" },
            "Settings"    => new TextBlock { Text = "Coming soon" },
            _             => null
        };

        // PanelContent.Children.Clear();
    }

    public static readonly StyledProperty<bool> IsConnectionsPanelProperty =
        AvaloniaProperty.Register<SidebarView, bool>(nameof(IsConnectionsPanel), defaultValue: false);

    public bool IsConnectionsPanel
    {
        get => GetValue(IsConnectionsPanelProperty);
        private set => SetValue(IsConnectionsPanelProperty, value);
    }

    private void SetActiveButton(string? tag)
    {
        foreach (var (btn, icon) in Buttons())
        {
            btn.Classes.Remove("active");
            icon.Foreground = (Avalonia.Media.IBrush)Resources["FgMutedBrush"]!;
        }

        if (tag == null) return;

        var active = tag switch
        {
            "Connections" => (BtnConnections, (MaterialIcon)BtnConnections.Content!),
            "SwitchDb" => (BtnSwitchDb, (MaterialIcon)BtnSwitchDb.Content!),
            "Settings" => (BtnSettings, (MaterialIcon)BtnSettings.Content!),
            _ => ((Button?)null, (MaterialIcon?)null)
        };


        if (active.Item1 == null) return;
        active.Item1.Classes.Add("active");
        active.Item2!.Foreground = (Avalonia.Media.IBrush)Resources["FgActiveBrush"]!;
    }

    private IEnumerable<(Button, MaterialIcon)> Buttons()
    {
        yield return (BtnConnections, (MaterialIcon)BtnConnections.Content!);
        yield return (BtnSwitchDb, (MaterialIcon)BtnSwitchDb.Content!);
        yield return (BtnSettings, (MaterialIcon)BtnSettings.Content!);
    }
}