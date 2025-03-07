using BowlingOOAD.Entities;
using System.Text.Json;

namespace BowlingOOAD.Services
{
    public class BowlingGameFacade
    {
        private readonly SingletonLogger _logger = SingletonLogger.Instance;
        private readonly string _jsonFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "results.json");
        private readonly List<Player> _gameData = new();
        private readonly Random _random = new Random();
        private readonly MemberRegistrationService _memberService = new MemberRegistrationService();

        public void RandomResultGenerator(Player player)
        {
            player.Result = _random.Next(50, 300);
        }

        //Detta som startar ifrån Program.cs MAIN är StartBowling(), vilket också laddar upp startmenyn för navigering
        public void StartBowling()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Välkommen till Bertils Bowlinghall!");
                Console.WriteLine("\n--- Bertils Bowlinghall ---");
                Console.WriteLine("1. Registrera ny medlem");
                Console.WriteLine("2. Lista medlemmar");
                Console.WriteLine("3. Starta ny match");
                Console.WriteLine("4. Vinnarhistorik");
                Console.WriteLine("5. Ta en Öl i baren");
                Console.WriteLine("6. Spara och Avsluta");
                Console.Write("Välj ett alternativ mellan 1-6: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        _memberService.RegisterNewMember();
                        break;
                    case "2":
                        _memberService.ListMembers();
                        PressAnyKeyToContinue();
                        break;
                    case "3":
                        Console.Clear();
                        StartGame();
                        break;
                    case "4":
                        Console.Clear();
                        ShowGameHistory();
                        break;
                    case "5":
                        Console.Clear();
                        TaEnÖl();
                        break;
                    case "6":
                        Console.Clear();
                        return;
                    default:
                        Console.WriteLine("Något gick fel. Försök igen.");
                        break;
                }
            }
        }

        private void StartGame()
        {
            Console.Clear();
            Console.WriteLine("\n--- Ny Match ---");
            Console.WriteLine("Välj spelare nummer 1:");
            Player player1 = _memberService.ChooseMemberForGame();

            Console.Clear();
            Console.WriteLine("\n--- Ny Match ---");
            Console.WriteLine("Välj spelare nummer 2:");
            Player player2 = _memberService.ChooseMemberForGame();

            // Genererar random poäng för spelarna, random genereraren är deklarerad längst upp i klassen.
            RandomResultGenerator(player1);
            RandomResultGenerator(player2);

            //Resultat med logik för vem som vinner med mest poäng, vinnaren loggas i loggen.
            Console.WriteLine($"\nResultat:");
            Console.WriteLine($"{player1.Name} fick {player1.Result} poäng.");
            Console.WriteLine($"{player2.Name} fick {player2.Result} poäng.");

            if (player1.Result > player2.Result)
            {
                Console.WriteLine($"\nVinnaren är {player1.Name} med {player1.Result} poäng!");
                _logger.Log($"Vinnare: {player1.Name} med {player1.Result} poäng.");
            }
            else if (player2.Result > player1.Result)
            {
                Console.WriteLine($"\nVinnaren är {player2.Name} med {player2.Result} poäng!");
                _logger.Log($"Vinnare: {player2.Name} med {player2.Result} poäng.");
            }
            else
            {
                Console.WriteLine("\nDet blev oavgjort!");
                _logger.Log("Matchen slutade oavgjort.");
            }

            _gameData.Add(player1);
            _gameData.Add(player2);
            SaveGameData();
            
        }

        private void TaEnÖl()
        {
            Console.Clear();
            _logger.Log("Du tog dig en öl i baren och tanken slår dig... har man någonsin bowlat helt nykter?");
            _logger.Log("Efter en stund känner du dig modigare och redo att fortsätta spela, samt beger dig tillbaks till bowlingbanorna.");
            PressAnyKeyToContinue();
        }

        private void ShowGameHistory()
        {
            Console.Clear();
            try
            {
                if (File.Exists(_jsonFilePath))
                {
                    string existingData = File.ReadAllText(_jsonFilePath);
                    if (!string.IsNullOrWhiteSpace(existingData))
                    {
                        var existingPlayers = JsonSerializer.Deserialize<List<Player>>(existingData);

                        if (existingPlayers != null && existingPlayers.Count > 0)
                        {
                            Console.WriteLine("\n--- Vinnare tidigare matcher: ---");
                            foreach (var player in existingPlayers)
                            {
                                Console.WriteLine($"Spelare: {player.Name}, Poäng: {player.Result} Tid: {player.GameDate}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ingen matchhistorik finns ännu.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ingen matchhistorik finns ännu.");
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Hoppsan!! Något gick fel... : {ex.Message}");
            }
            PressAnyKeyToContinue();
        }

        private void SaveGameData()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_jsonFilePath) ?? string.Empty);

                if (File.Exists(_jsonFilePath))
                {
                    string existingData = File.ReadAllText(_jsonFilePath);
                    if (!string.IsNullOrWhiteSpace(existingData))
                    {
                        var existingPlayers = JsonSerializer.Deserialize<List<Player>>(existingData);
                        if (existingPlayers != null)
                        {
                            _gameData.AddRange(existingPlayers);
                        }
                    }
                }

                string json = JsonSerializer.Serialize(_gameData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_jsonFilePath, json);
                _logger.Log("Spelresultat loggat i results.json.");
            }
            catch (Exception ex)
            {
                _logger.Log($"Det gick inte att spara resultatet pga: {ex.Message}");
            }
            PressAnyKeyToContinue();
        }

        private void PressAnyKeyToContinue()
        {
            Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
        }
    }
}