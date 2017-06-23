using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotWithConsoleCommands.Commands
{
    class UserAddServer: Program, ICommand
    {
        public string Name
        {
            get
            {
                return "UserAddServer";
            }
        }

        public string HelpText
        {
            get
            {
                return "Add Server(s) To User In User List\n UserAddServer <user id> <server1 id> <server2 id> .... <last server id>";
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

        //argument index (0 = user id, rest = server ids)
        public string Execute(string[] args)
        {
            if (bot.CheckUser(UInt64.Parse(args[0])) == false)
            {
                return $"User with Id of {args[0]} is not in the user list";
            }

            int index = bot.FindUser(UInt64.Parse(args[0]));

            for(int i = 0; i < (args.Count() - 1); i++)
            {
                bot.userList[index].servers.Add(UInt64.Parse(args[i]));
            }

            return $"{(args.Count() - 1).ToString()} Servers Added To User {args[0]}";
        }
    }
}
