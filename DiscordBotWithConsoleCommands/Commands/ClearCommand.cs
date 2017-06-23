using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotWithConsoleCommands
{
    class ClearCommand : ICommand
    {
        public string Name
        {
            get
            {
                return "Clear";
            }
        }

        public string HelpText
        {
            get
            {
                return "Clears Console";
            }
        }

        public bool isPriviledged
        {
            get
            {
                return true;
            }
        }

        public string Execute(string[] args)
        {           
            Console.Clear();
            return null;
        }
    }
}
