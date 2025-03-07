using BowlingOOAD.Services;

namespace BowlingOOAD
{
    internal class Program
    {

        static void Main(string[] args)
        {
            {
                // Börjar att logga
                var logger = SingletonLogger.Instance;

                // BowlingGameFacade startar spelet
                BowlingGameFacade bowlingGame = new BowlingGameFacade();
                bowlingGame.StartBowling();

                // Slutmeddelande
                logger.Log("Programmet avslutades. Resultatet har sparats och tack för att ni spelade!");
            }
        }
    }
}
