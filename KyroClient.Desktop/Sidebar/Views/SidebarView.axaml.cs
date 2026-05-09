using System.Collections.Generic;
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
        ShowPanel("Connections");
    }

    private void OnIconClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        var tag = btn.Tag?.ToString();
        if (tag == null) return;

        if (_activePanel == tag)
        {
            PanelContainer.IsVisible = false;
            _activePanel = null;
            SetActiveButton(null);
        }
        else
        {
            ShowPanel(tag);
        }
    }

    private void OnClosePanel(object? sender, RoutedEventArgs e)
    {
        PanelContainer.IsVisible = false;
        _activePanel = null;
        SetActiveButton(null);
    }

    private void ShowPanel(string panelName)
    {
        _activePanel = panelName;
        PanelContainer.IsVisible = true;
        PanelTitle.Text = panelName switch
        {
            "Connections" => "Connections",
            "SwitchDb"    => "Switch Database",
            "Settings"    => "Settings",
            _             => panelName
        };
        SetActiveButton(panelName);
        PanelContent.Children.Clear(); // content wired up later
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
            "SwitchDb"    => (BtnSwitchDb,    (MaterialIcon)BtnSwitchDb.Content!),
            "Settings"    => (BtnSettings,    (MaterialIcon)BtnSettings.Content!),
            _             => ((Button?)null,  (MaterialIcon?)null)
        };

        if (active.Item1 == null) return;
        active.Item1.Classes.Add("active");
        active.Item2!.Foreground = (Avalonia.Media.IBrush)Resources["FgActiveBrush"]!;
    }

    private IEnumerable<(Button, MaterialIcon)> Buttons()
    {
        yield return (BtnConnections, (MaterialIcon)BtnConnections.Content!);
        yield return (BtnSwitchDb,    (MaterialIcon)BtnSwitchDb.Content!);
        yield return (BtnSettings,    (MaterialIcon)BtnSettings.Content!);
    }
}