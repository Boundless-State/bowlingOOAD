using BowlingOOAD.Entities;
using System.Text.Json;

namespace BowlingOOAD.Services
{
    public class MemberRegistrationService
    {
        private readonly SingletonLogger _logger = SingletonLogger.Instance;
        private readonly string _membersFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "players.json");
        private List<Player> _registeredMembers = new();

        public MemberRegistrationService()
        {
            LoadExistingMembers();
        }

        private void LoadExistingMembers()
        {
            try
            {
                if (File.Exists(_membersFilePath))
                {
                    string existingData = File.ReadAllText(_membersFilePath);
                    if (!string.IsNullOrWhiteSpace(existingData))
                    {
                        _registeredMembers = JsonSerializer.Deserialize<List<Player>>(existingData)?? new List<Player>();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Fel vid inläsning av medlemmar: {ex.Message}");
            }
        }

        public void RegisterNewMember()
        {
            Console.WriteLine("\n--- Medlemsregistrering ---");

            while (true)
            {
                Console.Write("Ange namn på ny spelare att registrera (eller tryck Enter för att gå tillbaka): ");
                string name = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(name))
                    break;

                // Kontrollera om medlemmen redan finns
                if (_registeredMembers.Any(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"Medlemmen {name} finns redan registrerad i systemet.");
                    continue;
                }

                Player newMember = new Player(name);
                _registeredMembers.Add(newMember);

                Console.WriteLine($"Medlemmen {name} har registrerats med ID {newMember.PlayerID}.");
                _logger.Log($"Ny medlem registrerad: {name}");
            }

            SaveMembers();
        }

        public void ListMembers()
        {
            if (_registeredMembers.Count == 0)
            {
                Console.WriteLine("Inga medlemmar är registrerade dessvärre.");
                return;
            }

            Console.WriteLine("\n--- Registrerade Medlemmar ---");
            foreach (var member in _registeredMembers)
            {
                Console.WriteLine($"ID: {member.PlayerID}, Namn: {member.Name}");
            }
        }

        public Player ChooseMemberForGame()
        {
            while (true)
            {
                ListMembers();
                Console.Write("\nAnge medlems-ID för spelaren (eller 0 för att registrera ny): ");

                if (!int.TryParse(Console.ReadLine(), out int memberId))
                {
                    Console.WriteLine("Ogiltigt ID-Nummer. Försök igen.");
                    continue;
                }

                if (memberId == 0)
                {
                    RegisterNewMember();
                    continue;
                }

                var selectedMember = _registeredMembers.FirstOrDefault(m => m.PlayerID == memberId);

                if (selectedMember != null)
                {
                    return selectedMember;
                }

                Console.WriteLine("Ingen medlem hittades med det ID:t. Försök igen eller välj 0 för att skapa ny medlem.");
            }
            
        }

        private void SaveMembers()
        {
            try
            {
                // Skapa katalogen om den inte finns (hade problem med detta ett tag tills jag fattade vad som behövde göras)
                Directory.CreateDirectory(Path.GetDirectoryName(_membersFilePath) ?? string.Empty);

                string json = JsonSerializer.Serialize(_registeredMembers, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_membersFilePath, json);
                _logger.Log("Ändringar sparade.");
            }
            catch (Exception ex)
            {
                _logger.Log($"Fel vid sparande av medlemmar: {ex.Message}");
            }
        }
    }
}