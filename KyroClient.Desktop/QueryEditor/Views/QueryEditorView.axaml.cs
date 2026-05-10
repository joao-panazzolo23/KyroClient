using Avalonia.Controls;
using AvaloniaEdit.Highlighting;

namespace KyroClient.Desktop.QueryEditor.Views;

public partial class QueryEditorView : UserControl
{
    public QueryEditorView()
    {
        InitializeComponent();
        SetupSqlHighlighting();
    }

    private void SetupSqlHighlighting()
    {
        // load built-in SQL highlighting — AvaloniaEdit ships with it
        //not working rn. need to see this later
        Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("SQL");
        Editor.Options.EnableHyperlinks = false;
        Editor.Options.EnableEmailHyperlinks = false;
        Editor.Options.ShowBoxForControlCharacters = true;
        Editor.Options.ConvertTabsToSpaces = true;
        Editor.Options.IndentationSize = 4;
    }
}