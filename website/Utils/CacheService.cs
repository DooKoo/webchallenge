using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using Octokit;
using System.Threading.Tasks;

namespace website.Utils
{
    /// <summary>
    /// Data cacher
    /// Data updating every hour
    /// </summary>
    public static class CacheService
    {
        private static Dictionary<string, Repository> RepositoryDetailsDictionary = 
            new Dictionary<string, Repository>();

        private static Dictionary<string, IReadOnlyList<UserStar>> StargazersDictionary = 
            new Dictionary<string, IReadOnlyList<UserStar>>();

        private static Dictionary<string, IReadOnlyList<GitHubCommit>> MonthCommitsDictionary = 
            new Dictionary<string, IReadOnlyList<GitHubCommit>>();

        private static Dictionary<string, IReadOnlyList<GitHubCommit>> WeekCommitsDictionary = 
            new Dictionary<string, IReadOnlyList<GitHubCommit>>();

        private static Dictionary<string, DateTime> ExpirationTimesDictionary = 
            new Dictionary<string, DateTime>();

        /// <summary>
        /// Add expiration time to dictionary
        /// </summary>
        /// <param name="name">object key</param>
        /// <param name="type">object type</param>
        private static void AddExpirationTime(string name, Type type, string aditionalDesc = "")
        {
            string key = String.Format("{0}_{1}_{2}",type.Name, name, aditionalDesc);
            ExpirationTimesDictionary[key] = DateTime.Now.AddHours(1);
        }

        /// <summary>
        /// Checking data expiration
        /// </summary>
        /// <param name="name">object key</param>
        /// <param name="type">object type</param>
        /// <returns>returns false if data expired and true if valid</returns>
        private static bool IsDataValid(string name, Type type, string aditionalDesc = "")
        {
            string key = String.Format("{0}_{1}_{2}",type.Name, name, aditionalDesc);
            if(!ExpirationTimesDictionary.Keys.Contains(key))
            {
                return false;
            }

            if (ExpirationTimesDictionary[key] < DateTime.Now)
                return false;

            return true;
        }

        /// <summary>
        /// Return cached details of repository
        /// </summary>
        /// <param name="name">Name of repository</param>
        /// <returns>Repository in json format</returns>
        public static async Task<JObject> GetRepositoryDetails(string name)
        {
            if (!IsDataValid(name, typeof(Repository)))
            {
                RepositoryDetailsDictionary[name] = await GitHubWrapper.GetRepository(name);
                AddExpirationTime(name, typeof(Repository));
            }

            return JObject.FromObject(RepositoryDetailsDictionary[name]);
        }        

        /// <summary>
        /// Return chached stargazers
        /// </summary>
        /// <param name="name">Name of repository</param>
        /// <param name="from">Minimal date of stargazing</param>
        /// <param name="to">Maximal date of stargazing</param>
        /// <returns>Stargazers of repository on json format</returns>
        public static async Task<JArray> GetStargazers(string name, DateTime from, DateTime to)
        {
            if (from >= to)
                return new JArray();

            if (!IsDataValid(name, typeof(UserStar)))
            {
                StargazersDictionary[name] = await GitHubWrapper.GetStargazers(name);
                AddExpirationTime(name, typeof(UserStar));
            }

            var stargazers = StargazersDictionary[name];
            var stargazersFiltered = stargazers.Where(stargazer =>
                stargazer.StarredAt >= from && stargazer.StarredAt <= to);

            return JArray.FromObject(stargazersFiltered);
        }

        /// <summary>
        /// Return cached commits
        /// </summary>
        /// <param name="name">Name of the repository</param>
        /// <param name="type">Describe period: 0 - weekly commits, 1 - monthly commits</param>
        /// <returns>Commits of repository on json format</returns>
        public static async Task<JArray> GetCommits(string name, int type)
        {
            Dictionary<string, IReadOnlyList<GitHubCommit>> commitsDictionary;

            if(type == 0)
                commitsDictionary = WeekCommitsDictionary;
            else if(type == 1)
                commitsDictionary = MonthCommitsDictionary;
            else
                return new JArray();
            
            if (!IsDataValid(name, typeof(GitHubCommit), type.ToString()))
            {
                commitsDictionary[name] = await GitHubWrapper.GetCommits(name, type);
                AddExpirationTime(name, typeof(GitHubCommit), type.ToString());
            }
            var commits = commitsDictionary[name];
            
            return JArray.FromObject(commits);
        }        
    }
}