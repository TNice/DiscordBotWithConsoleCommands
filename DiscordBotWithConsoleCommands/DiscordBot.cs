using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;

namespace DiscordBotWithConsoleCommands
{

    struct BotUser
    {
        public string name;
        public ulong id;
        public int score;
        public int strike;
        public List<ulong> servers;
    }

    class DiscordBot
    {
        //Declare Client
        public DiscordClient client = new DiscordClient();
        

        public List<Server> serverList = new List<Server>();

        public List<BotUser> userList = new List<BotUser>();

        char commandPrefix = Settings.Default.Command; 

        public DiscordBot()
        {
            /* Voice Channel Attempt Still In Progress
             * 
            //Enable Audio Client
            client.UsingAudio(x =>
            {
                x.Mode = AudioMode.Outgoing;
            });

            Channel voiceChannel = client.FindServers("Test").FirstOrDefault().VoiceChannels.FirstOrDefault();
            AudioService vClient = client.GetService<AudioService>();
            */

            //Executed When Bot Connects To Server   
            client.ServerAvailable += (s, e) =>
            {
                bool add = true;
                Console.WriteLine($"Bot Connected To {e.Server.Name}");
                Console.Write("~");
                //Add Server To Server List If Not In
                foreach (Server serv in serverList)
                {
                    if(e.Server.Id == serv.Id)
                    {
                        add = false;
                    }
                }

                if (add)
                {
                    serverList.Add(e.Server);
                }
            };

            //Executes When Server Is Not Avalible (Not Sure If Working)
            client.ServerUnavailable += (s, e) =>
            {
                Console.WriteLine($"Bot Disconnected From {e.Server.Name}");
                Console.Write("~");
            };

            client.MessageReceived += async (s, e) =>
            {
                if (e.Message.IsAuthor) return;

                //Update User List
                bool add = false;
                if (userList == null) add = true;
                else foreach (BotUser u in userList)
                {
                     if (e.User.Id != u.id)
                     {
                         add = true;
                     }
                }
                
                if(add)
                {
                    AddUserToList(e.User, e.Server.Id);
                }
                else
                {
                    int index = FindUser(e.User.Id);
                    if (index == -1) Console.Write("Unexpected User Error In User List");

                    if (userList[index].servers.Count == 0)
                    {
                        userList[index].servers[0] = e.Server.Id;
                    }
                    else
                    {
                        for(int i = 0; i < userList[index].servers.Count(); i++)
                        {
                            if (userList[index].servers[i] == e.Server.Id)
                            {
                                add = false;
                                break;
                            }
                            add = true;
                        }

                        if(add)
                        {
                            AddServerToUser(e.Server.Id, index);
                        }
                    }
                }

                //Check for command prefix
                if (e.Message.Text[0] == commandPrefix)
                {
                    //Break Command Into Parts
                    string rawText = e.Message.Text.Remove(0);
                    List<string> commandParts = rawText.Split(' ').ToList<string>();
                    string command = commandParts[0];
                    commandParts.RemoveAt(0);
                    string[] args = commandParts.ToArray<string>();

                    //Make Sure Command Is Valid And Can Be Run Through Discord
                    try
                    {
                        string result = Program.registry.DiscordExecute(command, args);
                    }
                    catch (CommandNotFoundException)
                    {
                        await e.Channel.SendMessage("Command Not Found");
                    }
                    catch (ArgumentException)
                    {
                        await e.Channel.SendMessage("Command Only Accessable To Console");
                    }
                }
            }; 

            //Executed When User Joins Server
            client.UserJoined += async (s, e) =>
            {
                //Search For an anouncement channel
                Channel anounce = e.Server.FindChannels("anouncement").FirstOrDefault();
                if (anounce == null)
                {
                    await e.Server.DefaultChannel.SendMessage($"Welcome {e.User} To {e.Server.Name}");
                }
                else
                {
                   await anounce.SendMessage($"Welcome {e.User} To {e.Server.Name}");
                }
            };
            //Executes When User Leaves Server
            client.UserLeft += async (s, e) =>
            {
                //Search For an anouncement channel
                Channel anounce = e.Server.FindChannels("anouncement").FirstOrDefault();
                if (anounce == null)
                {
                    await e.Server.DefaultChannel.SendMessage($"{e.User} Has Decided To Leave {e.Server.Name}");
                }
                else
                {
                    await anounce.SendMessage($"{e.User} Has Decided To Leave {e.Server.Name}");
                }
            };
            //Executes When User Is Banned
            client.UserBanned += async (s, e) =>
            {
                //Search For an anouncement channel
                Channel anounce = e.Server.FindChannels("anouncement").FirstOrDefault();
                if (anounce == null)
                {
                    await e.Server.DefaultChannel.SendMessage($"{e.User} Banned From {e.Server.Name}");
                }
                else
                {
                    await anounce.SendMessage($"{e.User} Banned From {e.Server.Name}");
                }
            };
            //Executes When User Is UnBanned
            client.UserUnbanned += async (s, e) =>
            {
                //Search For an anouncement channel
                Channel anounce = e.Server.FindChannels("anouncement").FirstOrDefault();
                if (anounce == null)
                {
                    await e.Server.DefaultChannel.SendMessage($"{e.User} Has Been Unbanned From {e.Server.Name}");
                }
                else
                {
                    await anounce.SendMessage($"{e.User} Has Been Unbanned From {e.Server.Name}");
                }
            };

        }
        
        void AddUserToList(User u, ulong sId)
        {
            BotUser b = new BotUser();
            b.name = u.Name;
            b.id = u.Id;

            b.servers = new List<ulong>();
            b.servers.Add(sId);

            userList.Add(b);
        }

        void AddServerToUser(ulong server, int index)
        {
            userList[index].servers.Add(server);
        }

        public int FindUser(ulong id)
        {
            for(int i = 0; i < userList.Count(); i++)
            {
                if (userList[i].id == id) return i;
            }

            return -1;
        }

        public void ConsoleAddUser(string name, ulong id, ulong[] servers)
        {
            BotUser b = new BotUser();
            b.name = name;
            b.id = id;

            b.servers = new List<ulong>();
            foreach(ulong i in servers)
            {
                b.servers.Add(i);
            }

            userList.Add(b);
        }

        public async void Greet(string name, string p, Channel channel)
        {
            await channel.SendMessage($"{name} greets {p}");
        }

        public void Connect()
        {
            client.Connect(Settings.Default.Token, TokenType.Bot);
        }

        public void Disconnect()
        {
            serverList.Clear();
            client.Disconnect();
        }

        public bool CheckUser(ulong id)
        {
            foreach(BotUser u in userList)
            {
                if (id == u.id) return true;
            }

            return false;
        }

        public void AddPoints(BotUser user, int points)
        {
            user.score += points;           
        }

        public void AddStrike(BotUser user, int strikes)
        {
            user.strike += strikes;
        }
    }
}
