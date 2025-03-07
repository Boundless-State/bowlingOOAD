namespace BowlingOOAD.Entities
{
    public class GameResult
    {
        public int GameID { get; set; }
        public int Player1ID { get; set; }
        public string Player1Name { get; set; }
        public int Player1Score { get; set; }
        public int Player2ID { get; set; }
        public string Player2Name { get; set; }
        public int Player2Score { get; set; }
        public string Winner { get; set; }
        public DateTime GameDate { get; set; }

        public GameResult(Player player1, Player player2)
        {
            GameID = new Random().Next(1000, 9999);
            Player1ID = player1.PlayerID;
            Player1Name = player1.Name;
            Player1Score = player1.Result;
            Player2ID = player2.PlayerID;
            Player2Name = player2.Name;
            Player2Score = player2.Result;
            GameDate = DateTime.Now;

            // Bestämmer vinnare och sparar i results.json
            Winner = player1.Result > player2.Result ? player1.Name :
                     player2.Result > player1.Result ? player2.Name : "Oavgjort";
        }

    }
}