using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;


namespace pingweb
{  
    public struct LinkItem
    {
        public string Href;
        public string Text;

        public override string ToString()
        {
            return Href + "\n\t" + Text;
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                System.Console.WriteLine("enter a URL:");
                string input = System.Console.ReadLine();

                string url = input;


                System.Console.WriteLine(url);
                string webpage = getWebPage(url);

                int counter = 0;

                foreach (LinkItem i in getImagesNames(webpage))
                {
                    counter++;
                    if (counter > 5)
                        break;

                    System.Console.WriteLine();
                    System.Console.WriteLine(url + "/" + i);
                    string image1 = getWebPage(url + "/" + i);
                }
            }catch(Exception ex)
            {
                System.Console.WriteLine("Unable to process the specified URL");
            }

            System.Console.ReadLine();
        }

        static string getWebPage(string url)
        {
            double[] diffs = new double[10];

            string webpage = "";

            for(int i = 0 ; i < 10; i++)
            {
                WebClient wc = new System.Net.WebClient();
                DateTime start = DateTime.Now;
                webpage = wc.DownloadString(url);
                DateTime end = DateTime.Now;
                wc.Dispose();

                TimeSpan ts = end - start;

                System.Console.WriteLine("time ("+i+") = " + ts.TotalMilliseconds + " ms");

                diffs[i] = ts.TotalMilliseconds;
            }

            System.Console.WriteLine("average time=" + diffs.Average() + " ms");

            return webpage;
        }


        public static List<LinkItem> getImagesNames(string file)
        {
            //The following code, came from feedback posted on stackoverflow.com

            List<LinkItem> list = new List<LinkItem>();

            // 1.
            // Find all matches in file.
            MatchCollection m1 = Regex.Matches(file, @"(<img.*?>)",
                RegexOptions.Singleline);

            // 2.
            // Loop over each match.
            foreach (Match m in m1)
            {
                string value = m.Groups[1].Value;
                LinkItem i = new LinkItem();

                // 3.
                // Get href attribute.
                Match m2 = Regex.Match(value, @"src=\""(.*?)\""",
                RegexOptions.Singleline);
                if (m2.Success)
                {
                    i.Href = m2.Groups[1].Value;
                }

                // 4.
                // Remove inner tags from text.
                string t = Regex.Replace(value, @"\s*<.*?>\s*", "",
                RegexOptions.Singleline);
                i.Text = t;

                list.Add(i);
            }
            return list;
        }
    }
}
