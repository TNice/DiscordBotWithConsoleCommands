using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotWithConsoleCommands.Commands
{
    class DisconnectCommand : Program, ICommand
    {
        public string Name
        {
            get
            {
                return "Disconnect";
            }
        }

        public string HelpText
        {
            get
            {
                return "Disonnecets Bot From Discord Server";
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
            bot.Disconnect();
            return "Disconnecting";
        }
    }

}

