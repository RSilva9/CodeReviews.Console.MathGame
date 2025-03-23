using Spectre.Console;
using System.Linq;
using static CsharpAcademy_MathGame.Enums;

namespace CsharpAcademy_MathGame.Controllers;

internal interface IBaseController
{
    void StartGame(DifficultyOption difficulty, MenuOption game_type);
}

internal abstract class BaseController
{
    protected void DisplayMessage(string message, string color = "blue")
    {
        AnsiConsole.MarkupLine($"[{color}]{message}[/]");
    }
}