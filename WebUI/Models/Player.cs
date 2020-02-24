using System.Collections.Generic;

namespace TicTacToe.WebUI.Models
{
    public class Player
    {
        public Player(string id, string name)
        {
            Id = id;
            Name = name;
            ConnectionIds = new HashSet<string>();
            Actions = new List<SynchronizationAction>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string GroupName { get; set; }

        public HashSet<string> ConnectionIds { get; set; }

        public List<SynchronizationAction> Actions { get; set; }
    }
}
