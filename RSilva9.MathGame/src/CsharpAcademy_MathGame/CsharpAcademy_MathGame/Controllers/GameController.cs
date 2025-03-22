using CsharpAcademy_MathGame.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using static CsharpAcademy_MathGame.Enums;

namespace CsharpAcademy_MathGame.Controllers;

internal class GameController : BaseController, IBaseController
{
    public void StartGame(DifficultyOption difficulty, MenuOption game_type)
    {
        DisplayMessage($"Starting game with {difficulty} difficulty!", "blue");
        DisplayMessage($"Press any key to begin.", "blue");
        Console.ReadKey();

        Stopwatch stopwatch = Stopwatch.StartNew();
        /* User selects difficulty and an empty list is created for answers
           a random class is created as well for numbers to get their values
        */
        var user_answers = new List<(int, bool)>();

        int max_rounds = difficulty switch
        {
            DifficultyOption.Easy => max_rounds = 6,
            DifficultyOption.Normal => max_rounds = 9,
            DifficultyOption.Hard => max_rounds = 11,
        };

        for (var i = 1; i < max_rounds; i++)
        {
            int number1, number2;
            MenuOption random_game_type;

            // Numbers take random values
            (number1, number2, random_game_type) = AssignNumbers(game_type, difficulty);

            // Question is displayed and user answer is stored in the list
            DisplayMessage($"Round {i}", "blue");
            if(game_type == MenuOption.RandomGame)
            {
                user_answers.Add(PlayGame(number1, number2, random_game_type));
            }
            else
            {
                user_answers.Add(PlayGame(number1, number2, game_type));
            }
        }

        stopwatch.Stop();
        TimeSpan elapsed_time = stopwatch.Elapsed;

        Console.ReadKey();
        ShowResult(user_answers, elapsed_time);
        DisplayMessage($"Press any key to continue.", "blue");
        Console.ReadKey();

        // Creates a new Game to add to game history in MockDatabase
        int score = user_answers.Count(a => a.Item2 == true);
        string formatted_difficulty = difficulty switch
        {
            DifficultyOption.Easy => "[lightgreen]Easy[/]",
            DifficultyOption.Normal => "[orange3]Normal[/]",
            DifficultyOption.Hard => "[deeppink3]Hard[/]",
            _ => difficulty.ToString() // Fallback
        };
        Game new_game = new(game_type.ToString().Replace("Game", ""), formatted_difficulty, user_answers, score, elapsed_time);
        SaveGame(new_game);
    }

    public void ShowResult(List<(int answer,bool isCorrect)> user_answers, TimeSpan elapsed_time)
    {
        foreach(var (answer, isCorrect) in user_answers)
        {
            DisplayMessage($"Answer: {answer}, Correct: {(isCorrect ? "[green]Yes[/]" : "[red]No[/]")}");
        }
        DisplayMessage($"Time: {elapsed_time.ToString(@"mm\:ss")}", "orange3");
    }

    private (int, bool) PlayGame(int number1, int number2, MenuOption game_type)
    {
        // The two random numbers are summed and user's answer is compared to said sum to determine if it's correct
        int operation_result = 0;
        int user_result = 0;

        switch (game_type)
        {
            // Assigns operation numbers and results based on selected game type
            case MenuOption.SumsGame:
                operation_result = number1 + number2;
                user_result = AnsiConsole.Ask<int>($"What is the result of {number1} + {number2}?");
                break;
            case MenuOption.SubstractionsGame:
                operation_result = number1 - number2;
                user_result = AnsiConsole.Ask<int>($"What is the result of {number1} - {number2}?");
                break;
            case MenuOption.MultiplicationsGame:
                operation_result = number1 * number2;
                user_result = AnsiConsole.Ask<int>($"What is the result of {number1} x {number2}?");
                break;
            case MenuOption.DivisionsGame:
                operation_result = number1 / number2;
                user_result = AnsiConsole.Ask<int>($"What is the result of {number1} / {number2}?");
                break;
            case MenuOption.RandomGame:
                break;
        }

        if (operation_result == user_result)
        {
            DisplayMessage("Correct!", "green");
            DisplayMessage($"Press any key to continue.", "blue");
            return (user_result, true);
        }
        else
        {
            DisplayMessage("Incorrect!", "red");
            DisplayMessage($"Press any key to continue.", "blue");
            return (user_result, false);
        }
    }

    private void SaveGame(Game new_game)
    {
        MockDatabase.PastGames.Add(new_game);
    }

    private (int, int, MenuOption) AssignNumbers(MenuOption game_type, DifficultyOption difficulty)
    {
        int number1 = 0;
        int number2 = 0;
        int multiplier = 0;
        Random rnd = new Random();

        // Asigns values for divisions game
        if (game_type == MenuOption.DivisionsGame)
        {
            switch (difficulty)
            {
                case DifficultyOption.Easy:
                    number2 = rnd.Next(1, 11);
                    multiplier = rnd.Next(1, 11);
                    break;
                case DifficultyOption.Normal:
                    number2 = rnd.Next(1, 21);
                    multiplier = rnd.Next(1, 21);
                    break;
                case DifficultyOption.Hard:
                    number2 = rnd.Next(1, 51);
                    multiplier = rnd.Next(1, 51);
                    break;
            }
            number1 = number2 * multiplier;

            return (number1, number2, MenuOption.DivisionsGame);
        }
        // Asigns values for substractions game
        else if (game_type == MenuOption.SubstractionsGame)
        {
            switch (difficulty)
            {
                case DifficultyOption.Easy:
                    number2 = rnd.Next(1, 11);
                    number1 = rnd.Next(number2, 21);
                    break;
                case DifficultyOption.Normal:
                    number2 = rnd.Next(1, 51);
                    number1 = rnd.Next(number2, 101);
                    break;
                case DifficultyOption.Hard:
                    number2 = rnd.Next(1, 101);
                    number1 = rnd.Next(number2, 201);
                    break;
            }

            return (number1, number2, MenuOption.SubstractionsGame);
        }
        // Asigns values for random game
        else if (game_type == MenuOption.RandomGame)
        {
            // Generates a random number to choose between the four gamemodes.
            int random_number = rnd.Next(1, 5);
            MenuOption random_game_type = MenuOption.SumsGame;

            switch (random_number)
            {
                case 1:
                    switch (difficulty)
                    {
                        case DifficultyOption.Easy:
                            number1 = rnd.Next(1, 11);
                            number2 = rnd.Next(1, 11);
                            break;
                        case DifficultyOption.Normal:
                            number1 = rnd.Next(1, 51);
                            number2 = rnd.Next(1, 51);
                            break;
                        case DifficultyOption.Hard:
                            number1 = rnd.Next(1, 101);
                            number2 = rnd.Next(1, 101);
                            break;
                    }
                    random_game_type = MenuOption.SumsGame;
                    break;
                case 2:
                    switch (difficulty)
                    {
                        case DifficultyOption.Easy:
                            number2 = rnd.Next(1, 11);
                            number1 = rnd.Next(number2, 21);
                            break;
                        case DifficultyOption.Normal:
                            number2 = rnd.Next(1, 51);
                            number1 = rnd.Next(number2, 101);
                            break;
                        case DifficultyOption.Hard:
                            number2 = rnd.Next(1, 101);
                            number1 = rnd.Next(number2, 201);
                            break;
                    }
                    random_game_type = MenuOption.SubstractionsGame;
                    break;
                case 3:
                    switch (difficulty)
                    {
                        case DifficultyOption.Easy:
                            number2 = rnd.Next(1, 11);
                            multiplier = rnd.Next(1, 11);
                            break;
                        case DifficultyOption.Normal:
                            number2 = rnd.Next(1, 21);
                            multiplier = rnd.Next(1, 21);
                            break;
                        case DifficultyOption.Hard:
                            number2 = rnd.Next(1, 51);
                            multiplier = rnd.Next(1, 51);
                            break;
                    }
                    number1 = number2 * multiplier;
                    random_game_type = MenuOption.DivisionsGame;
                    break;
                case 4:
                    switch (difficulty)
                    {
                        case DifficultyOption.Easy:
                            number1 = rnd.Next(1, 11);
                            number2 = rnd.Next(1, 11);
                            break;
                        case DifficultyOption.Normal:
                            number1 = rnd.Next(1, 51);
                            number2 = rnd.Next(1, 51);
                            break;
                        case DifficultyOption.Hard:
                            number1 = rnd.Next(1, 101);
                            number2 = rnd.Next(1, 101);
                            break;
                    }
                    random_game_type = MenuOption.MultiplicationsGame;
                    break;
            }

            return (number1, number2, random_game_type);
        }
        // Asigns values for other games
        else
        {
            switch (difficulty)
            {
                case DifficultyOption.Easy:
                    number1 = rnd.Next(1, 11);
                    number2 = rnd.Next(1, 11);
                    break;
                case DifficultyOption.Normal:
                    number1 = rnd.Next(1, 51);
                    number2 = rnd.Next(1, 51);
                    break;
                case DifficultyOption.Hard:
                    number1 = rnd.Next(1, 101);
                    number2 = rnd.Next(1, 101);
                    break;
            }

            return (number1, number2, MenuOption.MultiplicationsGame);
        }
    }
}