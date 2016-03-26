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
    public static class CacheService
    {
        private static Dictionary<string, Repository> RepositoryDetailsDictionary = new Dictionary<string, Repository>();
        private static Dictionary<string, IReadOnlyList<UserStar>> StargazersDictionary = new Dictionary<string, IReadOnlyList<UserStar>>();
        private static Dictionary<string, IReadOnlyList<GitHubCommit>> MonthCommitsDictionary = new Dictionary<string, IReadOnlyList<GitHubCommit>>();
        private static Dictionary<string, IReadOnlyList<GitHubCommit>> WeekCommitsDictionary = new Dictionary<string, IReadOnlyList<GitHubCommit>>();
        private static JArray Repositories;

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
        /// return cached commits
        /// </summary>
        /// <param name="name">name of the repository</param>
        /// <param name="type">describe period: 0 - weekly commits, 1 - monthly commits</param>
        /// <returns></returns>
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