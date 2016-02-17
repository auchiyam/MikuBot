using System.Collections.Generic;
using Discord;
using Discord.Collections;
using System.IO;
using System;

namespace HatsuneMikuBot
{
    public class Program
    {
        public static bool GameStarted;
        [STAThread]
        static void Main()
        {
            var client = new DiscordClient();
            var commands = new BotCommands(client);
            GameStarted = false;
            Miku_Bot.Initialize();

            #region Message Created
            client.MessageCreated += async (s, e) =>
            {
                if (!e.Message.IsAuthor)
                {
                    if (e.Message.Text.Substring(0, 1).Equals("!"))
                    {
                        var str = e.Message.Text;

                        var command = str.Split(' ');
                        var server = e.Channel.ServerId;
                        var channel = e.Channel;
                        var user = e.Message.User;

                        switch (command[0].ToLower())
                        {
                            case ("!help"):
                                if (command.Length == 1)
                                {
                                    await commands.Help(channel);
                                }
                                else if (command.Length == 2)
                                {
                                    await commands.Help(channel, command[1]);
                                }
                                else
                                {
                                    await commands.Help(channel, $"{command[1]} {command[2]}");
                                }
                                break;

                            case ("!birthday"):
                                await commands.Birthday(channel, user, command);
                                break;

                            case ("!music"):
                                await commands.Media(channel, user, command);
                                break;

                            case ("!picture"):
                                await commands.Media(channel, user, command);
                                break;

                            case "!feature":
                                await commands.FeatureRequests(channel, user, command);
                                break;
                            case "!game":
                                //await commands.Game(channel, user, command);
                                break;

                            #region dumb features
                            case ("!nou"):
                                    await client.SendMessage(e.Channel, "no u");
                                break;

                            case "!recap":
                                await client.SendMessage(e.Channel, "Banana is life and death. Banana is what creates and destroys. Banana is all. Banana is God.");
                                break;
                            #endregion
                        } 
                    }
                    else
                    {
                        var str = e.Message.Text;

                        var command = str.Split(' ');
                        var server = e.Channel.ServerId;
                        var channel = e.Channel;
                        var user = e.Message.User;

                        if (str.Contains("no"))
                        {
                            int c = 0;
                            for (int i = 0; i < command.Length; i++)
                            {
                                if (command[i].Contains("no") && i != command.Length - 1)
                                {
                                    c = i;
                                    break;
                                }
                            }
                            switch (command[c].ToLower())
                            {
                                case "no":
                                    if (command[c+1].ToLower().Contains("u") || command[c+1].ToLower().Contains("you"))
                                    {
                                            await client.SendMessage(e.Channel, $"{command[c]} {command[c + 1]}");
                                    }
                                    break;
                            }
                        }
                    }
                }

            };
            #endregion

            #region Start
            client.Run(async () =>
            {
                //login
                await client.Connect("hatsune.miku.koinuri@gmail.com", "592358803zDf");

                //find all servers joined
                Servers servers = client.Servers;

                //fill the dictionaries
                foreach (Server s in servers)
                {
                    if (hasFolder(s))
                    {
                        ReadContents(s);
                        UpdateContents(s);
                    }
                    else
                    {
                        CreateFolders(s);
                    }
                }

                ReadRequests();
            });
            #endregion

            Console.ReadLine();
        }   

        #region contents
        public static void ReadContents(Server s)
        {
            string[] musicURL = File.ReadAllLines(string.Format(@"D:\Library\Document\Miku Bot\{0}\music\URL.txt", s.Id));
            string[] musicTAG = File.ReadAllLines(string.Format(@"D:\Library\Document\Miku Bot\{0}\music\TAG.txt", s.Id));
            string[] musicCH = File.ReadAllLines(string.Format(@"D:\Library\Document\Miku Bot\{0}\music\TAG.txt", s.Id));

            string[] picturesURL = File.ReadAllLines(string.Format(@"D:\Library\Document\Miku Bot\{0}\picture\URL.txt", s.Id));
            string[] picturesTAG = File.ReadAllLines(string.Format(@"D:\Library\Document\Miku Bot\{0}\picture\TAG.txt", s.Id));

            string[] quotes = File.ReadAllLines(string.Format(@"D:\Library\Document\Miku Bot\{0}\quote\quote.txt", s.Id));

            string[] birthdaysNAME = File.ReadAllLines(string.Format(@"D:\Library\Document\Miku Bot\{0}\birthday\NAME.txt", s.Id));
            string[] birthdaysDATE = File.ReadAllLines(string.Format(@"D:\Library\Document\Miku Bot\{0}\birthday\DATE.txt", s.Id));

            Dictionary<int, List<string>> music = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> pictures = new Dictionary<int, List<string>>();

            List<string> quote = new List<string>();
            Dictionary<string, DateTime> birthdays = new Dictionary<string, DateTime>();

            music[0] = new List<string>();
            pictures[0] = new List<string>();

            for (int i = 0; i < musicURL.Length; i++)
            {
                List<string> m = new List<string>();
                m.Add(musicURL[i]);
                string[] tags = musicTAG[i].Split(new string[] { "<|>" }, StringSplitOptions.None);
                for (int j = 0; j < tags.Length; j++)
                {
                    m.Add(tags[j]);
                }
                music[i+1] = m;
            }

            for (int i = 0; i < picturesURL.Length; i++)
            {
                List<string> p = new List<string>();
                p.Add(picturesURL[i]);
                string[] tags = picturesTAG[i].Split(new string[] { "<|>" }, StringSplitOptions.None);
                for (int j = 0; j < tags.Length; j++)
                {
                    p.Add(tags[j]);
                }
                pictures[i+1] = p;
            }

            for (int i = 1; i < quotes.Length; i++)
            {
                quote.Add(quotes[i]);
            }

            for (int i = 0; i < birthdaysNAME.Length; i++)
            {
                birthdays[birthdaysNAME[i]] = DateTime.Parse(birthdaysDATE[i]);
            }

            Miku_Bot.music[s] = music;
            Miku_Bot.pictures[s] = pictures;
            Miku_Bot.quotes[s] = quote;
            Miku_Bot.birthdays[s] = birthdays;

        }

        public static void UpdateContents(Server s)
        {

        }

        public static void ReadRequests()
        {
            string[] requests = File.ReadAllLines(@"D:\Library\Document\Miku Bot\feature requests.txt");

            foreach(string req in requests)
            {
                Miku_Bot.featureRequests.Add(req);
            }
            
        }

        public static bool hasFolder(Server s)
        {
            return Directory.Exists(string.Format(@"D:\Library\Document\Miku Bot\{0}", s.Id));
        }

        public static bool hasFolder(Server s, string command)
        {
            return Directory.Exists($@"D:\Library\Document\Miku Bot\{s.Id}\{command}");
        }

        public static void CreateFolders(Server s)
        {
            Directory.CreateDirectory(string.Format(@"D:\Library\Document\Miku Bot\{0}", s.Id));
            CreateFolders(s, "music");
            CreateFolders(s, "picture");
            CreateFolders(s, "quote");
            CreateFolders(s, "birthday");
        }

        public static void CreateFolders(Server s, string type)
        {
            switch (type)
            {
                case "music":
                    Directory.CreateDirectory(string.Format(@"D:\Library\Document\Miku Bot\{0}\music", s.Id));
                    File.Create(string.Format(@"D:\Library\Document\Miku Bot\{0}\music\URL.txt", s.Id));
                    File.Create(string.Format(@"D:\Library\Document\Miku Bot\{0}\music\TAG.txt", s.Id));
                    File.Create(string.Format(@"D:\Library\Document\Miku Bot\{0}\music\CH.txt", s.Id));
                    break;
                case "picture":
                    Directory.CreateDirectory(string.Format(@"D:\Library\Document\Miku Bot\{0}\picture", s.Id));
                    File.Create(string.Format(@"D:\Library\Document\Miku Bot\{0}\picture\URL.txt", s.Id));
                    File.Create(string.Format(@"D:\Library\Document\Miku Bot\{0}\picture\TAG.txt", s.Id));
                    File.Create(string.Format(@"D:\Library\Document\Miku Bot\{0}\picture\CH.txt", s.Id));
                    break;
                case "quote":
                    Directory.CreateDirectory(string.Format(@"D:\Library\Document\Miku Bot\{0}\quote", s.Id));
                    File.Create(string.Format(@"D:\Library\Document\Miku Bot\{0}\quote\quote.txt", s.Id));
                    break;
                case "birthday":
                    Directory.CreateDirectory(string.Format(@"D:\Library\Document\Miku Bot\{0}\birthday", s.Id));
                    File.Create(string.Format(@"D:\Library\Document\Miku Bot\{0}\birthday\NAME.txt", s.Id));
                    File.Create(string.Format(@"D:\Library\Document\Miku Bot\{0}\birthday\DATE.txt", s.Id));
                    break;
            }
        }
        #endregion

        #region Channels
        public static void CheckChannels(Channel channel, string type)
        {
            Server s = channel.Server;

            switch (type)
            {
                case "music":
                    break;

                case "picture":
                    break;
            }
        }

        public static List<string> CheckMusicinChannel(Channel channel)
        {
            List<string> music= new List<string>();
            IEnumerable<Message> messages = channel.Messages;
            foreach (Message m in messages)
            {
                Console.WriteLine(m.Text);
            }

            return music;
        }

        public static void CheckPicturesinChannel(Channel channel)
        {

        }
        #endregion

    }
}
