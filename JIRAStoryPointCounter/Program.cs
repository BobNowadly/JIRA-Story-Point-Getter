using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JIRAStoryPointCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                var client = new HttpClient { BaseAddress = new Uri(options.BaseUrl) };

                var connection = new BasicConnection(options.UserName,
                        options.Password, client);

                string url = string.Format("rest/api/latest/search?fields=customfield_10005&jql=project={0}%20AND%20issuetype=Story%20AND%20(fixVersion%20!=%20Post-V2.0%20OR%20fixVersion%20is%20EMPTY)&maxResults=500", options.Project);

                Console.WriteLine("Today is: {0}", DateTime.Now);

                GetStoryPoints(connection, url, "Total Story Points");

                url = string.Format("rest/api/latest/search?fields=customfield_10005&jql=project={0}%20AND%20issuetype=Story%20AND%20status=Done&maxResults=500", options.Project);
                GetStoryPoints(connection, url, "Completed Story Points");
            }
        }

        private static void GetStoryPoints(BasicConnection connection, string url, string label)
        {
            using (Task<HttpResponseMessage> response = connection.GetAsync(url))
            {
                var workItems =
                    JsonConvert.DeserializeObject<SearchResult>(
                        response.Result.Content.ReadAsStringAsync().Result);

                if (workItems != null)
                {
                    Console.WriteLine("{0}={1}", label,
                        workItems.issues.Sum(s => Convert.ToDouble(s.fields.customfield_10005)));
                }
            }
        }
    }
}
