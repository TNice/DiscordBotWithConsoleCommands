using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotWithConsoleCommands.Commands
{
    class ConnectCommand : Program, ICommand
    {
        
        public string Name
        {
            get
            {
                return "Connect";
            }
        }

        public string HelpText
        {
            get
            {
                return "Connecets Bot To Discord Server";
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
            bot.Connect();
            return "Connecting";
        }
    }
}
