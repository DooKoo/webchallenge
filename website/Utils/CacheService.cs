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
        private static Dictionary<string, IEnumerable<Activity>> PushesDictionary = new Dictionary<string, IEnumerable<Activity>>();
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

        public static async Task<JArray> GetPushes(string name, DateTime from, DateTime to)
        {
            if (from >= to)
                return new JArray();
            
            if (!PushesDictionary.Keys.Contains(name))
            {
                PushesDictionary.Add(name, await GitHubWrapper.GetPushes(name));
            }
            var pushes = PushesDictionary[name];
            var pushesFiltered = pushes.Where(push =>
                push.CreatedAt >= from && push.CreatedAt <= to);

            return JArray.FromObject(pushesFiltered);

        }
    }
}