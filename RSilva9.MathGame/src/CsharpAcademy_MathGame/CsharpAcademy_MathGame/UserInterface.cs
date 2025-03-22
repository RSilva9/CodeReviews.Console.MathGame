using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using CsharpAcademy_MathGame.Controllers;
using static CsharpAcademy_MathGame.Enums;
using static System.Reflection.Metadata.BlobBuilder;

namespace CsharpAcademy_MathGame;

internal class UserInterface
{
    private readonly GameController _gameController = new();
    internal void MainMenu()
    {
        while (true)
        {
            Console.Clear();

            // Displays menu options
            var selected_menu_option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Welcome to Math Games 3000!")
                .AddChoices("Play", "View previous games", "Exit"));

            if (selected_menu_option == "Play") 
            { 
                // Displays types of game for player to choose
                var selected_game = AnsiConsole.Prompt(
                    new SelectionPrompt<MenuOption>()
                    .Title("What game do you wish to play?")
                    .UseConverter(option =>
                    {
                        string name = option.ToString();
                        return name.Replace("Game", " Game");
                    })
                    .AddChoices(Enum.GetValues<MenuOption>()));

                var difficulty = SelectDifficulty();

                _gameController.StartGame(difficulty, selected_game);

            }
            else if (selected_menu_option == "View previous games")
            {
                // Displays all previously played games stored in MockDatabase
                ViewGames();
            }
            else
            {
                return;
            }
        }
    }

    private DifficultyOption SelectDifficulty()
    {
        return AnsiConsole.Prompt(
                new SelectionPrompt<DifficultyOption>()
                .Title("Please select a difficulty:")
                .AddChoices(Enum.GetValues<DifficultyOption>()));
    }

    private void ViewGames()
    {
        // Takes past games from MockDatabase and displays them
        var past_games = MockDatabase.PastGames;

        if (MockDatabase.PastGames.Count == 0)
        {
            AnsiConsole.Markup("[red]No past games found.[/]");
            Console.ReadKey();
            return;
        }

        foreach (var game in past_games)
        {
            game.DisplayGameDetails();
        }

        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }
}