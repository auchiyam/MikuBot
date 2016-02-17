using System;
using System.Linq;
using System.IO;
using HtmlAgilityPack;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord;
using System.Drawing;

namespace HatsuneMikuBot
{
    public class Util
    {
        static Random r = new Random();
        
        public static void WriteToTextFile(string dir, string line)
        {
            StreamWriter sw = File.AppendText(dir);
            sw.WriteLine(line);
            sw.Dispose();
        }

        public static void WriteToTextFile(string dir, string line, int index)
        {
            string[] lines = File.ReadAllLines(dir);
            if (index >= lines.Length)
            {
                WriteToTextFile(dir, line);
                return;
            }

            StreamWriter sw = new StreamWriter(dir);
            for (int i = 0; i <= lines.Length; i++)
            {
                if (i < index)
                    sw.WriteLine(lines[i]);
                else if (i == index)
                    sw.WriteLine(line);
                else
                    sw.WriteLine(lines[i - 1]);
            }
            sw.Dispose();
        }

        public static void DeleteLineFromFile(string dir, int index)
        {
            string[] lines = File.ReadAllLines(dir);
            StreamWriter sw = new StreamWriter(dir);

            for (int i = 0; i < lines.Length; i++)
            {
                if (i == index)
                    continue;
                sw.WriteLine(lines[i]);
            }
            sw.Dispose();
        }

        public static void EditLineFromFile(string dir, string line, int index)
        {
            string[] lines = File.ReadAllLines(dir);
            if (index >= lines.Length)
            {
                WriteToTextFile(dir, line);
                return;
            }

            StreamWriter sw = new StreamWriter(dir);
            for (int i = 0; i < lines.Length; i++)
            {
                if (i < index)
                    sw.WriteLine(lines[i]);
                else if (i == index)
                    sw.WriteLine(line);
                else
                    sw.WriteLine(lines[i]);
            }
            sw.Dispose();
        }

        public static int Randomize(int last)
        {
            return r.Next(1, last);
        }

        public static int Randomize(int beginning, int end)
        {
            return r.Next(beginning, end);
        }
    }
}
