using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingOOAD.Entities
{
    public class Player
    {
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public int Result { get; set; }
        public DateTime GameDate { get; set; }

        private static int _idCounter = 1;

        public Player(string name)
        {
            PlayerID = _idCounter++;
            Name = name;
            Result = 0;
            GameDate = DateTime.Now;
        }
    }
}
