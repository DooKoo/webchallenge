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
    /// Data chacher
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

        private static JArray Repositories;

        /// <summary>
        /// Return cached details of repository
        /// </summary>
        /// <param name="name">Name of repository</param>
        /// <returns>Repository in json format</returns>
        public static async Task<JObject> GetRepositoryDetails(string name)
        {
            if (!RepositoryDetailsDictionary.Keys.Contains(name))
            {
                var repo = await GitHubWrapper.GetRepository(name);
                RepositoryDetailsDictionary.Add(name, repo);
            }

            return JObject.FromObject(RepositoryDetailsDictionary[name]);
        }

        public static async Task<JArray> GetRepositories()
        {
            if (Repositories == null)
            {
                
            }
            return Repositories;
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

            if (!StargazersDictionary.Keys.Contains(name))
            {
                StargazersDictionary.Add(name, await GitHubWrapper.GetStargazers(name));
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
            {
                commitsDictionary = WeekCommitsDictionary;
            }
            else if(type == 1)
            {
                commitsDictionary = MonthCommitsDictionary;
            }
            else
            {
                return new JArray();
            }
            
            if (!commitsDictionary.Keys.Contains(name))
            {
                commitsDictionary.Add(name, await GitHubWrapper.GetCommits(name, type));
            }
            var commits = commitsDictionary[name];
            
            return JArray.FromObject(commits);
        }        
    }
}