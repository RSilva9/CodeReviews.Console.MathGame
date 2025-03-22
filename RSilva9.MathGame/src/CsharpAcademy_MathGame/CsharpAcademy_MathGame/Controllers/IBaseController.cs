using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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