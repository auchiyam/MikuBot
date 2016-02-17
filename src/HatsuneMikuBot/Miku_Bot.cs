using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace HatsuneMikuBot
{
    public class Miku_Bot
    {
        public static Dictionary<Server, Dictionary<int, List<string>>> music, pictures;
        public static Dictionary<Server, Channel> musicChannels, pictureChannels;
        public static Dictionary<Server, List<string>> quotes;
        public static Dictionary<Server, Dictionary<string, DateTime>> birthdays;
        public static List<string> featureRequests;

        public static Dictionary<int, List<string>> list;

        public Miku_Bot()
        {
            Initialize();
        }

        public static void Initialize()
        {
            music = new Dictionary<Server, Dictionary<int, List<string>>>();
            pictures = new Dictionary<Server, Dictionary<int, List<string>>>();
            quotes = new Dictionary<Server, List<string>>();
            birthdays = new Dictionary<Server, Dictionary<string, DateTime>>();
            featureRequests = new List<string>();
        }

    }
}
