﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotWithConsoleCommands
{
    
    class CommadHandler
    {
       //Makes a list of all commands that implament ICommand interface
       public static List<ICommand> commands = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(ICommand)) && x.GetConstructor(Type.EmptyTypes) != null)
                .Select(x => Activator.CreateInstance(x) as ICommand).ToList<ICommand>();

        public static void RegisterCommands()
        {
           
            foreach (ICommand command in commands)
            {
                Program.registry.RegisterCommand(command);
            }
        }
    }
}
