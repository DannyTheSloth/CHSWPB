using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CHSWPBMP
{
    public class MenuBuilder
    {
        private List<string> options;
        private string header;
        private int counter;
        private int currentPage;
        private List<int[]> pages;
        private List<KeyValuePair<string, ConsoleKey>> buttons;

        public MenuBuilder(string header)
        {
            options = new List<string>();
            this.header = header;
            counter = 0;
            currentPage = 1;
            pages = new List<int[]>();
            buttons = new List<KeyValuePair<string, ConsoleKey>>();
        }

        public void addButton(string name, ConsoleKey key)
        {
            buttons.Add(new KeyValuePair<string, ConsoleKey>(name, key));
        }

        public void addOption(string name)
        {
            options.Add(name);
        }

        public Response getResponse()
        {
            renderPage(currentPage);

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    if (currentPage != pages.Count)
                    {
                        currentPage++;
                        renderPage(currentPage);
                    }
                } else if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    if (currentPage != 1)
                    {
                        currentPage--;
                        renderPage(currentPage);
                    }
                }

                if (keyInfo.Key == ConsoleKey.RightArrow || keyInfo.Key == ConsoleKey.LeftArrow) continue;

                try
                {
                    int additional = 9 * (currentPage - 1);
                        
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.D1:
                            return new Response("OPTION", 1 + additional);
                        case ConsoleKey.D2:
                            return new Response("OPTION", 2 + additional);
                        case ConsoleKey.D3:
                            return new Response("OPTION", 3 + additional);
                        case ConsoleKey.D4:
                            return new Response("OPTION", 4 + additional);
                        case ConsoleKey.D5:
                            return new Response("OPTION", 5 + additional);
                        case ConsoleKey.D6:
                            return new Response("OPTION", 6 + additional);
                        case ConsoleKey.D7:
                            return new Response("OPTION", 7 + additional);
                        case ConsoleKey.D8:
                            return new Response("OPTION", 8 + additional);
                        case ConsoleKey.D9:
                            return new Response("OPTION", 9 + additional);
                    }

                    if (buttons.Count > 0)
                    {
                        foreach (KeyValuePair<string, ConsoleKey> button in buttons)
                        {
                            if (keyInfo.Key == button.Value)
                            {
                                Response response = new ("BUTTON", 0);
                                response.Key = keyInfo.Key;
                                return response;
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }

        private void renderPage(int p)
        {
            Console.Clear();
            string fullHeader = header.ToUpper() + $" | Page {p} of {pages.Count}";
            Console.WriteLine(fullHeader);
            StringBuilder sb = new();
            for (int i = 0; i < fullHeader.Length; i++)
                sb.Append("-");
            Console.WriteLine(sb.ToString());
            

            if (buttons.Count > 0)
            {
                int longestButton = 0;
                string fullButton = string.Empty;
                foreach (KeyValuePair<string, ConsoleKey> button in buttons)
                {
                    fullButton = $"[{Enum.GetName(button.Value)}] | {button.Key}";
                    if (fullButton.Length > longestButton)
                        longestButton = fullButton.Length;
                    Console.WriteLine(fullButton);
                }

                sb.Clear();
                for (int i = 0; i < fullButton.Length; i++)
                    sb.Append("-");
                Console.WriteLine(sb.ToString());
            }
            
            sb.Clear();

            int optionNumber = 1;
            try
            {
                foreach (int index in pages[p - 1])
                {
                    Console.WriteLine($"[{optionNumber}] | {options[index]}");
                    optionNumber++;
                }
            }
            catch
            {
            }
        }

        public void createMenu()
        {
            Console.Clear();

            List<int> indexes = new();

            int wholePageNumber = Convert.ToInt16(Math.Floor((double)(options.Count / 9)));
            int remainingOptions = options.Count % 9;

            for (int i = 0; i < options.Count; i++)
            {
                indexes.Add(i);

                counter++;

                if (counter == 9)
                {
                    counter = 0;
                    pages.Add(indexes.ToArray());
                    indexes.Clear();
                }
            }

            if (pages.Count == wholePageNumber)
            {
                if (remainingOptions > 0)
                    pages.Add(indexes.ToArray());
            }
        }

        public void ClearOptions()
        {
            options.Clear();
            counter = 0;
        }

        public class Response
        {
            public string Type;
            public int Value;
            public ConsoleKey Key;

            public Response(string type, int value)
            {
                Type = type;
                Value = value;
            }
        }
    }
}
