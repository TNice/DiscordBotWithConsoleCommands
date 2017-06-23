using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DiscordBotWithConsoleCommands.Commands
{
    class GetServerListCommand : Program, ICommand
    {
        public string Name
        {
            get
            {
                return "GetServers";
            }
        }

        public string HelpText
        {
            get
            {
                return "Gets List of Connected Servers";
            }
        }

        public bool isPriviledged
        {
            get
            {
                return false;
            }
        }

        public string Execute(string[] args)
        {
            Console.WriteLine("\n%-20s %-20s", "Server", "Id");

            foreach (Server s in bot.serverList)
            {
                Console.WriteLine($"\n%-20s %-20s", s.Name, s.Id.ToString());
            }
            return null;
        }
    }
}

