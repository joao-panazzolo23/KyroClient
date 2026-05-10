using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KyroClient.Desktop.ResultConsole.Dto;

public partial class ResultConsoleDto : ObservableObject
{
    [ObservableProperty] private IEnumerable<dynamic>? _resultTable;
    [ObservableProperty] private string _statusText = "Ready";
    [ObservableProperty] private string _elapsedText = "";
}