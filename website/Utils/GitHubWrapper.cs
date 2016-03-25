using Newtonsoft.Json.Linq;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace website.Utils
{
    public static class GitHubWrapper
    {
        public static async Task<Repository> GetRepository(string name)
        {
            var search = new SearchRepositoriesRequest();
            var splittedName = name.Split('/');

            if(splittedName.Length != 2)
                return new Repository();

            var repository = await GitHubClientSingelton.Client.Repository.Get(splittedName[0], splittedName[1]);
            return repository;
        }

        public static async Task<IReadOnlyList<UserStar>> GetStargazers(string name)
        {
            var splitted = name.Split('/');

            if (splitted.Length != 2)
                return new List<UserStar>();

            var stargazers = await GitHubClientSingelton.Client.Activity.Starring
                .GetAllStargazersWithTimestamps(splitted[0], splitted[1]);

            return stargazers;
        }

        public static async Task<IEnumerable<Activity>> GetPushes(string name)
        {
            var splitted = name.Split('/');

            if (splitted.Length != 2)
                return new List<Activity>();

            var contributors = await GitHubClientSingelton.Client.Activity.Events
                .GetAllForRepository(splitted[0], splitted[1]);
            
            return contributors.Where(c => c.Type == "PushEvent");
        }
    }
}