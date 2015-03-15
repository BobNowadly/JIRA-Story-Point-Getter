using CommandLine;

namespace JIRAStoryPointCounter
{
    class Options 
    {
        [Option('b', "base url", Required = true,
            HelpText = "base url to JIRA.")]
        public string BaseUrl { get; set; }

        [Option('u', "username", Required = true,
            HelpText = "username to  JIRA")]
        public string UserName { get; set; }

        [Option('p', "password", Required = true,
            HelpText = "password to  JIRA")]
        public string Password { get; set; }

        [Option('r', "Project", Required = true,
          HelpText = "Project in JIRA")]
        public string Project { get; set; }
    }
}