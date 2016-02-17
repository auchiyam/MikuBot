using Discord;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using System.Net;
using System.Text;

namespace HatsuneMikuBot
{
    class BotCommands
    {
        DiscordClient client;
        public BotCommands(DiscordClient c)
        {
            client = c;
        }
        #region Help
        public Task<Message[]> Help(Channel channel)
        {
            return client.SendMessage(channel,
                "List of commands:\n" +
                "・basics\n・birthday\n・music\n・picture\n・quote(wip)\n・timezone(wip)\n・game(wip)\n・feature request\n" +
                "Type !help <command> to learn more about the command"
            );
        }

        public Task<Message[]> Help(Channel channel, string type)
        {
            Task<Message[]> ret;
            switch (type.ToLower())
            {
                case "basics":
                    ret = client.SendMessage(channel,
                        "Basics:\n" +
                        "・<> - Fields you need to fill out\n" +
                        "・[] - optional field you can leave out\n"
                    );
                    break;

                case "birthday":
                    ret = client.SendMessage(channel,
                        "Birthday:\n" +
                        "・\"!birthday\" - Gives the closest birthday stored\n" +
                        "・\"!birthday next\" - Gives the closest birthday stored\n" +
                        "・\"!birthday list [month]\" - Lists all birthday stored\n" +
                        "・\"!birthday add <name> <month/day/year>\" - Adds the birthday\n" +
                        "・\"!birthday delete <name>\" - Deletes the birthday\n"
                    );
                    break;

                case "music":
                    ret = client.SendMessage(channel,
                        "Music\n" +
                        "・\"!music\" - Randomly plays a music\n" +
                        "・\"!music add <music link>\" - Adds the music\n" +
                        "・\"!music add <music link> <artist> <title> [tags]\" - Adds the music with the artist, title, and tags.  The artist and title must be in <>\n" +
                        "・\"!music delete <id>\" - Deletes the music\n" +
                        "・\"!music find <list of tags>\" - Lists all ID that corresponds to the tags given. if only one music fits the criteria, it'll play that music.\n" +
                        "・\"!music post <id>\" - Plays the music with that ID\n" +
                        "・\"!music tag add <id> <list of tags>\" - Adds the list of tags to the ID\n" +
                        "・\"!music tag delete <id> <list of tags>\" - Deletes the list of tags from the ID\n" +
                        "・\"!music artist <id> <artist name>\" - Changes the artist name with the new artist\n" +
                        "・\"!music title <id> <title name>\" - Changes the title name with the new title\n" +
                        "・\"!music channel set\" - Set the current channel as music channel (~waiting for ability to read lines made when offline~)\n" +
                        "・\"!music channel remove\" - Removes the current channel as music channel (~waiting for ability to read lines made when offline~)\n"
                    );
                    break;

                case "picture":
                    ret = client.SendMessage(channel,
                        "Picture\n" +
                        "・\"!picture\" - Randomly posts a picture\n" +
                        "・\"!picture add <picture link>\" - Adds the picture \n" +
                        "・\"!picture add <picture link> <artist> <title> [tags]\" - Adds the pictre with the artist, title, and tags. The artist and title must be in <>\n" +
                        "・\"!picture delete <id>\" - Deletes the music\n" +
                        "・\"!picture find <list of tags>\" - Lists all ID that corresponds to the tags given. if only one picture fits the criteria, it'll post that picture.\n" +
                        "・\"!picture post <id>\" - Posts the picture with that ID\n" +
                        "・\"!picture tag add <id> <list of tags>\" - Adds the list of tags to the ID\n" +
                        "・\"!picture tag delete <id> <list of tags>\" - Deletes the list of tags from the ID\n" +
                        "・\"!picture artist <id> <artist name>\" - Changes the artist name with the new artist\n" +
                        "・\"!picture title <id> <title name>\" - Changes the title name with the new title\n" +
                        "・\"!picture channel set\" - Set the current channel as picture channel (~waiting for ability to read lines made when offline~)\n" +
                        "・\"!picture channel remove\" - Removes the current channel as picture channel (~waiting for ability to read lines made when offline~)\n"
                    );
                    break;

                case "quote":
                    ret = client.SendMessage(channel,
                        "Quote\n" +
                        "・\"!quote\" - Randomly posts a quote\n" +
                        "・\"!quote find <list of tags>\" - Lists all ID that corresponds to the tags given. if only one music fits the criteria, it'll play that music.\n" +
                        "・\"!quote post <id>\" - Posts the music with that ID\n" +
                        "・\"!quote delete <id>\" - Deletes the quote with that ID\n" +
                        "・\"!quote add <quote>\" - Adds the quote\n\n"
                    );
                    break;

                case "timezone":
                    ret = client.SendMessage(channel,
                        "Timezone\n" +
                        "\"!time offsets\" - Lists UTC offsets for the common timezones\n" +
                        "\"!time <UTC offset>\" - Current time at the timezone\n" +
                        "\"!time <UTC offset> <time in 00:00 format>\" - Calculates how long it will be until that time\n" +
                        "\"!time <UTC offset> <year-month-date> <time in 00:00 format>\" - Calculates how long it will be until that date and time\n"
                    );
                    break;

                case "feature request":
                    ret = client.SendMessage(channel,
                        "Feature Request:\n" +
                        "\"!feature request\" - view all feature requests given\n" +
                        "\"!feature request <request>\" - Adds the request to the list of requests wanted\n"
                    );
                    break;

                case "game":
                    ret = client.SendMessage(channel,
                        "Game:\n" +
                        "\"!game black jack\" - Starts a Black Jack game\n"
                    );
                    break;

                default:
                    client.SendMessage(channel, "Invalid command.  Please make sure you typed the command properly.");
                    ret = Help(channel);
                    break;
            }
            return ret;
        }
        #endregion

        #region Birthday
        public Task<Message[]> Birthday(Channel channel, User user, string[] command)
        {
            Task<Message[]> ret = null;
            if (command.Length == 1)
            {
                return FindClosestBirthday(channel);
            }
            switch (command[1])
            {
                case "next":
                    ret = FindClosestBirthday(channel);
                    break;

                case "list":
                    if (command.Length == 3)
                        ret = ListBirthdays(channel, command[2]);
                    else
                        ret = ListBirthdays(channel, "");
                    break;

                case "add":
                    ret = AddBirthday(channel, command);
                    break;

                case "delete":
                    ret = DeleteBirthday(channel, command);
                    break;

                default:
                    client.SendMessage(channel, "Invalid command. Please make sure you typed the command properly.");
                    ret = Help(channel, "birthday");
                    break;
            }
            return ret;
        }

        public Task<Message[]> FindClosestBirthday(Channel channel)
        {
            Server s = channel.Server;
            if (Miku_Bot.birthdays[s].Count == 0)
            {
                return client.SendMessage(channel, "No birthday has been added for this server");
            }

            else
            {
                DateTime today = DateTime.Today;
                List<KeyValuePair<string, DateTime>> same = new List<KeyValuePair<string, DateTime>>();
                List<KeyValuePair<string, DateTime>> same2 = new List<KeyValuePair<string, DateTime>>();
                KeyValuePair<string, DateTime> next = new KeyValuePair<string, DateTime>("", DateTime.MaxValue);
                KeyValuePair<string, DateTime> min = new KeyValuePair<string, DateTime>("", DateTime.MaxValue);

                foreach (KeyValuePair<string, DateTime> bday in Miku_Bot.birthdays[s])
                {
                    DateTime temp = new DateTime(today.Year, bday.Value.Month, bday.Value.Day);
                    Console.Write($"{bday.Key}");
                    if (temp.Month > today.Month)
                    {

                        if (temp.Month == next.Value.Month && temp.Day == next.Value.Day)
                        {
                            same.Add(bday);
                        }

                        else if (temp.Month < next.Value.Month)
                        {
                            next = bday;
                            same.Clear();
                            same.Add(next);
                        }

                        else if (temp.Month == next.Value.Month && temp.Day < next.Value.Day)
                        {
                            next = bday;
                            same.Clear();
                            same.Add(next);
                        }
                    }

                    else if (temp.Month == today.Month && temp.Day > today.Day)
                    {
                        if (temp.Month == next.Value.Month && temp.Day == next.Value.Day)
                        {
                            same.Add(bday);
                        }

                        else if (temp.Month < next.Value.Month)
                        {
                            next = bday;
                            same.Clear();
                            same.Add(next);
                        }

                        else if (temp.Month == next.Value.Month && temp.Day < next.Value.Day)
                        {
                            next = bday;
                            same.Clear();
                            same.Add(next);
                        }
                    }

                    if (temp.Month == min.Value.Month && temp.Day == min.Value.Day)
                    {
                        same2.Add(bday);
                    }

                    else if (temp.Month < min.Value.Month)
                    {
                        min = bday;
                        same2.Clear();
                        same2.Add(min);
                    }

                    else if (temp.Month == min.Value.Month && temp.Day < min.Value.Day)
                    {
                        min = bday;
                        same2.Clear();
                        same2.Add(min);
                    }
                }

                if (next.Value == DateTime.MaxValue)
                {
                    next = min;
                }

                string message = "Next birthday is";
                if (!next.Equals(min))
                {
                    if (same2.Count == 1)
                    {
                        message += $" {same2[0].Key} on {same2[0].Value.Month}/{same2[0].Value.Day}";
                    }
                    else
                    {
                        message += ":";
                        foreach (KeyValuePair<string, DateTime> s1 in same2)
                        {
                            message += "\n";
                            message += $"・{s1.Key} on {s1.Value.Month}/{s1.Value.Day}";
                        }
                    }
                }
                else
                {
                    if (same.Count == 1)
                    {
                        message += $" {same[0].Key} on {same[0].Value.Month}/{same[0].Value.Day}";
                    }
                    else
                    {
                        message += ":";
                        foreach (KeyValuePair<string, DateTime> s1 in same)
                        {
                            message += "\n";
                            message += $"・{s1.Key} on {s1.Value.Month}/{s1.Value.Day}";
                        }
                    }
                }

                return client.SendMessage(channel, message);
            }

        }

        public Task<Message[]> ListBirthdays(Channel channel, string filter)
        {
            int month = 0;
            switch (filter.ToLower())
            {
                case "january":
                    month = 1;
                    break;
                case "february":
                    month = 2;
                    break;
                case "march":
                    month = 3;
                    break;
                case "april":
                    month = 4;
                    break;
                case "may":
                    month = 5;
                    break;
                case "june":
                    month = 6;
                    break;
                case "july":
                    month = 7;
                    break;
                case "august":
                    month = 8;
                    break;
                case "september":
                    month = 9;
                    break;
                case "october":
                    month = 10;
                    break;
                case "november":
                    month = 11;
                    break;
                case "december":
                    month = 12;
                    break;
                case "":
                    month = 0;
                    break;
                default:
                    bool test = Int32.TryParse(filter, out month);
                    if (!test)
                    {
                        client.SendMessage(channel, $"{filter} is invalid. Please type in a <month>");
                        return Help(channel, "birthday");
                    }
                    break;
            }

            Server s = channel.Server;

            if (Miku_Bot.birthdays[s].Count == 0)
            {
                return client.SendMessage(channel, "No birthday has been added for this server");
            }

            string print = "List of birthdays:\n";

            Dictionary<string, DateTime> b = new Dictionary<string, DateTime>();

            if (month != 0)
            {
                foreach (KeyValuePair<string, DateTime> item in Miku_Bot.birthdays[s])
                {
                    if (item.Value.Month == month)
                    {
                        b.Add(item.Key, item.Value);
                    }
                }

            }
            else
            {
                b = Miku_Bot.birthdays[s];
            }

            foreach (KeyValuePair<string, DateTime> item in b)
            {
                string key = item.Key;
                string value = $"{item.Value.Month}-{item.Value.Day}";

                print += String.Format("・{0}: {1}\n", key, value);
            }

            return client.SendMessage(channel, print);
        }

        public Task<Message[]> AddBirthday(Channel channel, string[] info)
        {
            string name = "";
            DateTime date = DateTime.MinValue;

            Server s = channel.Server;

            for (int i = 2; i < info.Length; i++)
            {
                if (!DateTime.TryParse(info[i], out date))
                {
                    name += info[i] + " ";
                }
            }

            name = name.Substring(0, name.Length - 1);

            if (date == DateTime.MinValue)
            {
                return client.SendMessage(channel, "The date entered is in invalid format.  Please type the date in <year/month/date> format.");
            }

            Miku_Bot.birthdays[s][name] = date;

            if (!Program.hasFolder(s, "birthday"))
            {
                Program.CreateFolders(s, "birthday");
            }

            Util.WriteToTextFile(string.Format(@"D:\Library\Document\Miku Bot\{0}\birthday\NAME.txt", s.Id), name);
            Util.WriteToTextFile(string.Format(@"D:\Library\Document\Miku Bot\{0}\birthday\DATE.txt", s.Id), date.ToShortDateString());

            return client.SendMessage(channel, $"Added {name}'s birthday");
        }

        public Task<Message[]> DeleteBirthday(Channel channel, string[] name)
        {
            Server s = channel.Server;
            string n = "";
            for (int i = 2; i < name.Length; i++)
            {
                n += name[i] + " ";
            }

            n = n.Substring(0, n.Length - 1);

            int index = 0;
            foreach (KeyValuePair<string, DateTime> item in Miku_Bot.birthdays[s])
            {
                if (item.Key.Equals(n))
                {
                    Util.DeleteLineFromFile($@"D:\Library\Document\Miku Bot\{s.Id}\birthday\NAME.txt", index);
                    Util.DeleteLineFromFile($@"D:\Library\Document\Miku Bot\{s.Id}\birthday\DATE.txt", index);
                    Miku_Bot.birthdays[s].Remove(n);
                    return client.SendMessage(channel, $"{n} has been deleted.");
                }
                index++;
            }

            client.SendMessage(channel, $"The name \"{n}\" was not found.");
            return ListBirthdays(channel, "");
        }
        #endregion

        #region Media
        public Task<Message[]> Media(Channel channel, User user, string[] command)
        {
            string type = command[0].Substring(1);
            Server s = channel.Server;
            Task<Message[]> ret = null;            
            GetList(s, type);

            if (command.Length == 1)
            {
                return Post(channel, Util.Randomize(Miku_Bot.list.Count), type);
            }

            switch (command[1])
            {
                case "add":
                    ret = AddMedia(channel, type, command);
                    break;
                case "delete":
                    int id = 0;
                    bool isNum = Int32.TryParse(command[2], out id);
                    if (isNum && id < Miku_Bot.list.Count)
                    {
                        ret = DeleteMedia(channel, type, id);
                    }
                    else
                    {
                        ret = client.SendMessage(channel, "Invalid ID.");
                    }
                    break;

                case "find":
                    ret = FindMedia(channel, type, command);
                    break;

                case "post":
                    id = 0;
                    isNum = Int32.TryParse(command[2], out id);
                    if (isNum)
                    {
                        if (id < Miku_Bot.list.Count && !Miku_Bot.list[id][0].Equals("empty"))
                        {
                            ret = Post(channel, id, type);
                        }
                        else
                        {
                            ret = client.SendMessage(channel, "Invalid ID. The ID does not exist");
                        }
                    }
                    else
                        ret = client.SendMessage(channel, "Invalid ID. Please type in a number for ID");
                    break;

                case "tag":
                    if (command.Length < 5)
                    {
                        ret = client.SendMessage(channel, "Invalid Input.");
                        Help(channel, type);
                    }
                    else
                    {
                        switch (command[2])
                        {
                            case "add":
                                ret = EditTags(channel, type, command);
                                break;

                            case "delete":
                                ret = DeleteTags(channel, type, command);
                                break;
                        }
                    }
                    break;

                case "artist":
                    if (command.Length < 2)
                    {
                        ret = client.SendMessage(channel, "Invalid input");
                        Help(channel, type);
                    }
                    else
                    {
                        ret = ChangeMetadata(channel, type, 1, command);
                    }
                    break;

                case "title":
                    if (command.Length < 2)
                    {
                        ret = client.SendMessage(channel, "Invalid input");
                        Help(channel, type);
                    }
                    else
                    {
                        ret = ChangeMetadata(channel, type, 2, command);
                    }
                    break;

                case "channel":
                    switch (command[2])
                    {
                        case "set":
                            ret = client.SendMessage(channel, "not implemented");
                            break;
                    }
                    break;

                default:
                    client.SendMessage(channel, "Invalid command.  Please make sure you typed the command properly.");
                    ret = Help(channel, type);
                    break;
            }
            UpdateList(s, type);
            return ret;
        }

        private void GetList(Server s, string type)
        {
            switch (type)
            {
                case "music":
                    Miku_Bot.list = Miku_Bot.music[s];
                    break;
                case "picture":
                    Miku_Bot.list = Miku_Bot.pictures[s];
                    break;
            }
        }
        private void UpdateList(Server s, string type)
        {
            switch (type)
            {
                case "music":
                    Miku_Bot.music[s] = Miku_Bot.list;
                    break;
                case "picture":
                    Miku_Bot.pictures[s] = Miku_Bot.list;
                    break;
            }
        }
        private Task<Message[]> Post(Channel c, int id, string type)
        {
            string tags = "";
            if (Miku_Bot.list.Count <= 0)
            {
                return client.SendMessage(c, $"No {type} found for this server");
            }
            string link = Miku_Bot.list[id][0];
            for (int i = 3; i < Miku_Bot.list[id].Count - 1; i++)
            {
                tags += Miku_Bot.list[id][i] + ", ";
            }
            tags += Miku_Bot.list[id][Miku_Bot.list[id].Count - 1];

            if (tags.Length == 0)
            {
                tags = "No tags";
            }

            string message =
                $"__**{id}. {Miku_Bot.list[id][1]} - {Miku_Bot.list[id][2]}**__\n" +
                $"```{tags}```" +
                $"{link}";

            return client.SendMessage(c, message);
        }
        private Task<Message[]> AddMedia(Channel c, string type, string[] str)
        {
            Task<Message[]> ret = null;
            List<string> tags = new List<string>();

            if (str.Length == 3)
            {
                tags.Add(str[2]);
                tags.Add("Unknown Artist");
                tags.Add("Unknown Title");
                ret = client.SendMessage(c, $"Added {str[2]} to the {type} list");
            }

            else if (str.Length >= 5)
            {
                string temp = "";
                Stack<int> quotes = new Stack<int>();
                int quoteCount = 0;
                tags.Add(str[2]);
                for (int i = 3; i < str.Length; i++)
                {
                    if (quoteCount < 2)
                    {
                        foreach (char s in str[i])
                        {
                            if (quotes.Count == 1 && s != '>')
                            {
                                temp += s;
                            }

                            if (s == '<')
                            {
                                if (quotes.Count == 0)
                                    quotes.Push(1);
                            }
                            else if (s == '>')
                            {
                                quotes.Pop();
                            }


                        }
                    }

                    if (quoteCount >= 2)
                    {
                        tags.Add(str[i]);
                    }
                    else if (quotes.Count == 0)
                    {
                        tags.Add(temp);
                        temp = "";
                        quoteCount++;
                    }
                    else if (quotes.Count == 1)
                    {
                        temp += " ";
                    }
                }
                if (quoteCount < 2)
                {
                    return client.SendMessage(c, "Invalid input. Please put artist and title in <>.");
                }
                else
                {
                    ret = client.SendMessage(c, $"Added {tags[1]} - {tags[2]} to the {type} list");
                }
            }

            foreach (string sb in ReadTags(str[2], type))
            {
                tags.Add(sb);
            }

            Miku_Bot.list[Miku_Bot.list.Count] = tags;
            Util.WriteToTextFile(string.Format(@"D:\Library\Document\Miku Bot\{0}\{1}\URL.txt", c.Server.Id, type), tags[0]);
            string t = "";
            for (int i = 1; i < tags.Count - 1; i++)
            {
                t += tags[i] + "<|>";
            }
            t += tags[tags.Count - 1];
            Util.WriteToTextFile(string.Format(@"D:\Library\Document\Miku Bot\{0}\{1}\TAG.txt", c.Server.Id, type), t);

            if (ret == null)
            {
                ret = client.SendMessage(c, "Invalid input.");
            }

            return ret;
        }
        private Task<Message[]> DeleteMedia(Channel c, string type, int id)
        {
            Task<Message[]> ret = client.SendMessage(c, $"ID{id}. {Miku_Bot.list[id][1]} - {Miku_Bot.list[id][2]} has been removed");

            Util.EditLineFromFile(($@"D:\Library\Document\Miku Bot\{c.ServerId}\{type}\TAG.txt"), "empty", id - 1);
            Util.EditLineFromFile(($@"D:\Library\Document\Miku Bot\{c.ServerId}\{type}\URL.txt"), "empty", id - 1);
            Miku_Bot.list[id] = new List<string>();
            Miku_Bot.list[id].Add("empty");
            return ret;
        }
        private Task<Message[]> FindMedia(Channel c, string type, string[] str)
        {
            List<int> ids = new List<int>();
            int count = 0;
            string[] tags = new string[str.Length - 2];

            for (int i = 2; i < str.Length; i++)
            {
                tags[i - 2] = str[i];
            }

            foreach (KeyValuePair<int, List<string>> k in Miku_Bot.list)
            {
                for (int i = 1; i < k.Value.Count; i++)
                {
                    for (int j = 0; j < tags.Length; j++)
                    {
                        if (k.Value[i].ToLower().Contains(tags[j].ToLower()) && !k.Value[i][0].Equals("empty"))
                        {
                            count++;
                        }
                    }

                    if (count == tags.Length && !ids.Contains(k.Key))
                    {
                        ids.Add(k.Key);
                    }
                    count = 0;
                }
            }
            string message = $"{type[0].ToString().ToUpper() + type.Substring(1)} with \n   tags (";
            string tag = "";
            foreach (string t in tags)
            {
                tag += t + ", ";
            }
            tag = tag.Substring(0, tag.Length - 2);
            message += tag + "):\n";

            foreach (int i in ids)
            {
                message += $"・ID{i}: {Miku_Bot.list[i][1]} - {Miku_Bot.list[i][2]}\n";
            }

            if (ids.Count == 0)
            {
                message = $"Could not find any {type} with \n   tags ({tag}).";
            }

            if (ids.Count == 1)
            {
                return Post(c, ids[0], type);
            }

            return client.SendMessage(c, message);
        }
        private Task<Message[]> EditTags(Channel c, string type, string[] str)
        {
            int id;
            bool isID = Int32.TryParse(str[3], out id);
            if (!isID || id > Miku_Bot.list.Count || Miku_Bot.list[id][0].Equals("empty"))
            {
                return client.SendMessage(c, "Invalid ID.");
            }

            string newt = "";
            for (int i = 1; i < Miku_Bot.list[id].Count; i++)
            {
                newt += Miku_Bot.list[id][i] + "<|>";
            }

            for (int i = 4; i < str.Length; i++)
            {
                newt += str[i] + "<|>";
                Miku_Bot.list[id].Add(str[i]);
            }
            newt = newt.Substring(0, newt.Length - "<|>".Length);

            Util.EditLineFromFile(string.Format(@"D:\Library\Document\Miku Bot\{0}\{1}\TAG.txt", c.Server.Id, type), newt, id - 1);

            return Post(c, id, type);
        }
        private Task<Message[]> DeleteTags(Channel c, string type, string[] str)
        {
            int id;
            bool isID = Int32.TryParse(str[3], out id);
            if (!isID || id > Miku_Bot.list.Count || Miku_Bot.list[id][0].Equals("empty"))
            {
                return client.SendMessage(c, "Invalid ID");
            }

            string newt = "";
            for (int i = 1; i < Miku_Bot.list[id].Count; i++)
            {
                for (int j = 4; j < str.Length; j++)
                {
                    if (!Miku_Bot.list[id][i].Equals(str[j]) && i > 2)
                    {
                        newt += Miku_Bot.list[id][i] + "<|>";
                    }
                    else
                    {
                        Miku_Bot.list[id].Remove(str[j]);
                    }
                }
            }

            newt = newt.Substring(0, newt.Length - "<|>".Length);

            Util.EditLineFromFile(string.Format(@"D:\Library\Document\Miku Bot\{0}\{1}\TAG.txt", c.Server.Id, type), newt, id - 1);

            return Post(c, id, type);
        }
        private Task<Message[]> ChangeMetadata(Channel c, string type, int loc, string[] str)
        {
            int id;
            bool isID = Int32.TryParse(str[2], out id);
            if (!isID || id > Miku_Bot.list.Count || Miku_Bot.list[id][0].Equals("empty"))
            {
                return client.SendMessage(c, "Invalid ID.");
            }

            string newt = "";
            for (int i = 1; i < Miku_Bot.list[id].Count; i++)
            {
                if (i < loc)
                    newt += Miku_Bot.list[id][i] + "<|>";
                if (i == loc)
                {
                    string t = "";
                    for (int j = 3; j < str.Length; j++)
                    {
                        t += str[j] + " ";
                    }
                    Miku_Bot.list[id].RemoveAt(loc);
                    Miku_Bot.list[id].Insert(loc, t);
                    newt += t.Substring(0, t.Length - 1) + "<|>";
                }
                if (i > loc)
                {
                    newt += Miku_Bot.list[id][i] + "<|>";
                }
            }
            if (newt.Equals(""))
            {
                string temp = loc == 1 ? "Artist" : "Title";
                newt = $"Unknown {temp}<|>";
            }
            newt = newt.Substring(0, newt.Length - "<|>".Length);

            Util.EditLineFromFile(string.Format(@"D:\Library\Document\Miku Bot\{0}\{1}\TAG.txt", c.Server.Id, type), newt, id - 1);

            return Post(c, id, type);
        }
        public string[] ReadTags(string url, string type)
        {
            switch (type)
            {
                case "music":
                    return ReadMusicURLs(url);
                case "picture":
                    return ReadPictureURLs(url);
                default:
                    return new string[] {""};
            }
            
        }
        public string[] ReadMusicURLs(string url)
        {
            string[] tags;
            if (url.Contains("youtube"))
            {
                var html = new HtmlDocument();
                var data = new WebClient().DownloadData(url);
                var st = Encoding.UTF8.GetString(data);
                html.LoadHtml(st);

                var title = html.DocumentNode.SelectNodes(@"//title");
                string t = title.Select(a => a.InnerText).Last<string>();
                t = t.Substring(0, t.IndexOf(" - YouTube"));

                tags = t.Split(new char[] { ' ', '　' });
            }
            else if (url.Contains("nicovideo"))
            {
                var html = new HtmlDocument();
                var data = new WebClient().DownloadData(url);
                var st = Encoding.UTF8.GetString(data);
                html.LoadHtml(st);

                var title = html.DocumentNode.SelectNodes(@"//title");
                string t = title.Select(a => a.InnerText).Last<string>();
                t = t.Substring(0, t.IndexOf("ニコニコ動画:GINZA") - 3);

                tags = t.Split(new char[]{ ' ', '　'});
            }
            else if (url.Contains(".mp3") || url.Contains(".wav") || url.Contains(".flac"))
            {
                return new string[] {""};
            }
            else
            {
                return new string[] {""};
            }
            return tags;
        }
        public string[] ReadPictureURLs(string url)
        {
            string[] tags;
            if (url.Contains("pixiv"))
            {
                var html = new HtmlDocument();
                var data = new WebClient().DownloadData(url);
                var st = Encoding.UTF8.GetString(data);
                html.LoadHtml(st);

                var title = html.DocumentNode.SelectNodes(@"//head/title");
                string t = title.Select(a => a.InnerText).Last<string>();
                t = t.Substring(0, t.IndexOf(" [pixiv]"));
                string to = t.Substring(t.IndexOf("】") + 1, t.IndexOf("」") - t.IndexOf("】"));
                to += " ";
                to += t.Substring(t.IndexOf("イラスト/") + "イラスト/".Length, t.Length - to.Length - t.IndexOf("】") - "イラスト/".Length);

                tags = to.Split(new char[] { ' ', '　' });
            }
            else if (url.Contains("nicovideo"))
            {
                var html = new HtmlDocument();
                var data = new WebClient().DownloadData(url);
                var st = Encoding.UTF8.GetString(data);
                html.LoadHtml(st);

                var title = html.DocumentNode.SelectNodes(@"//title");
                string t = title.Select(a => a.InnerText).Last<string>();
                t = t.Substring(0, t.IndexOf(" さんのイラスト - ニコニコ静画 (イラスト)"));
                string to = t.Substring(0, t.IndexOf(" / "));
                to += " ";
                to += t.Substring(t.IndexOf(" / ") + " / ".Length, t.Length - to.Length - 2);

                tags = to.Split(new char[] { ' ', '　' });
            }
            else if (url.Contains(".jpg") || url.Contains(".png") || url.Contains(".gif") || url.Contains("jpeg"))
            {
                return new string[] { "" };
            }
            else
            {
                return new string[] { "<|>invalid<|>" };
            }
            return tags;
        }
        //private Task<Message[]> SetChannel(Channel c, string type)
        //{
        //    Util.WriteToTextFile($@"D:\Library\Document\Miku Bot\{c.Server.Id}\{type}\CH.txt", c.Id);
        //}
        #endregion

        #region Game
        public Task<Message[]> Game(Channel channel, User user, string[] commands)
        {
            Task<Message[]> ret = null;
            string game = "";
            for (int i = 1; i < commands.Length; i++)
            {
                game += commands[i] += " ";
            }
            game = game.Substring(0, game.Length - 1);
            switch (game.ToLower())
            {
                case "black jack":
                    ret = BlackJack(channel, user);
                    break;
                default:
                    client.SendMessage(channel, "Invalid command.  Please make sure you typed the command properly.");
                    ret = Help(channel, "game");
                    break;
            }
            return ret;
        }

        async public Task<Message[]> BlackJack(Channel channel, User user)
        {
            int miku = 0;
            int u = 0;
            await client.SendMessage(channel, "Starting Black Jack.");
            client.MessageCreated += async (s, e) =>
            {
                if (e.Message.User.Equals(user))
                {

                }
            };
            return null;
        }
        #endregion

        #region Feature Request
        public Task<Message[]> FeatureRequests(Channel channel, User user, string[] command)
        {
            Task<Message[]> ret = null;
            
            if (command.Length == 2)
            {
                ret = ListAllRequests(channel);
            }
            else
            {
                switch (command[2])
                {
                    case "delete":
                        ret = DeleteRequest(channel, user, command[3]);
                        break;

                    default:
                        ret = AddRequest(channel, command);
                        break;
                }
            }

            return ret;
        }

        public Task<Message[]> ListAllRequests(Channel channel)
        {
            string req = "";
            foreach (string request in Miku_Bot.featureRequests)
            {
                req += request + "\n";
            }
            return client.SendMessage(channel, $"List of requests:\n{req}");
        }

        public Task<Message[]> AddRequest(Channel channel, string[] req)
        {
            string r = "";
            for (int i = 2; i < req.Length; i++)
            {
                r += req[i] + " ";
            }

            r = r.Substring(0, r.Length - 1);
            Miku_Bot.featureRequests.Add(r);

            Util.WriteToTextFile(string.Format(@"D:\Library\Document\Miku Bot\feature requests.txt"), r);

            return client.SendMessage(channel, "Request has been added to the list");
        }

        public Task<Message[]> DeleteRequest(Channel channel, User user, string id)
        {
            if ((ulong.Parse(user.Id) != 93062570772021248))
            {
                return client.SendMessage(channel, "Only Koinuri can delete the request.");
            }

            int i = 0;
            if (!Int32.TryParse(id, out i))
            {
                return client.SendMessage(channel, "Invalid ID");
            }

            Miku_Bot.featureRequests.RemoveAt(i);

            Util.DeleteLineFromFile(@"D:\Library\Document\Miku Bot\feature requests.txt", i);

            return client.SendMessage(channel, "Successfully deleted the request");
        }
        #endregion

        #region etc
        async public Task<Message[]> Message(Channel channel, int count, string[] lines)
        {
            for (int i = 0; i < count; i++)
            {
                await client.SendMessage(channel, lines[i]);
            }
            return await client.SendMessage(channel, "");
        }
        #endregion
    }
}