namespace TicTacToe.WebUI.Models
{
    public class SynchronizationAction
    {
        public SynchronizationAction(string name)
        {
            Name = name;
            Parameters = new string[] { };
        }

        public SynchronizationAction(string name, string[] parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public string Name { get; set; }

        public string[] Parameters { get; set; }
    }
}
