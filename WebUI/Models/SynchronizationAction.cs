namespace TicTacToe.WebUI.Models
{
    public class SynchronizationAction
    {
        public SynchronizationAction(string name)
        {
            Name = name;
            Parameters = new object[] { };
        }

        public SynchronizationAction(string name, object[] parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public string Name { get; set; }

        public object[] Parameters { get; set; }
    }
}
