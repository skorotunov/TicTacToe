namespace TicTacToe.WebUI.Models
{
    /// <summary>
    /// Action that is used to sync UI of the multiple connections to the same player.
    /// </summary>
    public class SynchronizationAction
    {
        /// <summary>
        /// Parameterless method needs to be registered.
        /// </summary>
        /// <param name="name"></param>
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

        // Name of the method to invoke.
        public string Name { get; set; }

        // Array of the parameters to pass to the method.
        public string[] Parameters { get; set; }
    }
}
