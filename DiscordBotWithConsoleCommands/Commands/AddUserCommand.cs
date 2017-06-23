using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotWithConsoleCommands.Commands
{
    class AddUserCommand: Program, ICommand
    {
        public string Name
        {
            get
            {
                return "AddUser";
            }
        }

        public string HelpText
        {
            get
            {
                return "AddUser {name} {id} {server 1(optional)} {server 2(optional)} {server 3(optional)}\nAdds User To User List";
            }
        }

        //isPrivliledged = true { Command Can Only Be Used In Console }
        public bool isPriviledged
        {
            get
            {
                return true;
            }
        }

        //argument index (0 = name 1 = id 2-4 = servers)
        public string Execute(string[] args)
        {
            if (bot.CheckUser(UInt64.Parse(args[1])) == true)
            {
                return $"User with Id of {args[1]} is already in the user list";
            }

            if (args.Count() > 5) return "Too Many Arguments Please Limit Servers To 3";

            ulong[] servers = new ulong[3];
            if(args.Count() > 2)
            {
                for(int i = 2; i < args.Count(); i++)
                {
                    servers[i - 2] = UInt64.Parse(args[i]);
                }
            }

            bot.ConsoleAddUser(args[0], UInt64.Parse(args[1]), servers);
            return $"{args[0]} was add to user list with and Id of {args[1]}";
        }
    }
}
