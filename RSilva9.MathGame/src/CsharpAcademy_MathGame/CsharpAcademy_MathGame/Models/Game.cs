using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpAcademy_MathGame.Models;

internal class Game
{
    internal string GameType { get; set; }
    internal string Difficulty { get; set; }
    internal List<(int, bool)> Answers { get; set; }
    internal int Score { get; set; }
    internal TimeSpan Time { get; set; }

    internal Game(string game_type, string difficulty, List<(int, bool)> answers, int score, TimeSpan time)
    {
        GameType = game_type;
        Difficulty = difficulty;
        Answers = answers;
        Score = score;
        Time = time;
    }

    public void DisplayGameDetails()
    {
        // Formats the answers so they appear green when correct and red when incorrect
        var answers_formatted = string.Join(", ", Answers.Select(a => 
            $"{(a.Item2 ? $"[green]{a.Item1}[/]" : $"[red]{a.Item1}[/]")}"
        ));

        var panel = new Panel(new Markup($"[bold]Game type:[/] {GameType} - [bold]Difficulty:[/] {Difficulty} \n[bold]Answers:[/] {answers_formatted}\n[bold]Score:[/] {Score}/5 - [bold]Time: {Time.ToString(@"mm\:ss")}[/]" ))
        {
            Border = BoxBorder.Rounded
        };

        AnsiConsole.Write(panel);
    }
}