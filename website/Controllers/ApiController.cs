using Newtonsoft.Json.Linq;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using website.Utils;

namespace website.Controllers
{
    public class ApiController : Controller
    {
        /// <summary>
        /// Return list of repositories
        /// </summary>
        /// <param name="period">0 - weekly; 1 - monthly</param>
        /// <param name="type">0 - stars; 1 - contributors; 2 - commits</param>
        /// <returns>Repositories in json</returns>
        public async Task<string> Repositories(int period, int type)
        {
            var repositories = await CacheService.GetRepositories();
            Dictionary<string, int> sortDictionary = new Dictionary<string, int>();
            if (type == 0)
            {               
                repositories.ToList().ForEach(repo =>
                {                    
                    var stargazers = Task.Run(() => CacheService.GetStargazers(repo.FullName, period)).Result;
                    sortDictionary.Add(repo.FullName, stargazers.Count());
                });                
            }
            else if (type == 1)
            {
                repositories.ToList().ForEach(repo =>
                {
                    var commits = Task.Run(() => CacheService.GetCommits(repo.FullName, period)).Result;
                    sortDictionary.Add(repo.FullName, commits.GroupBy(c => c.Committer != null ? c.Committer.Id : 0).Count());
                });
            }
            else if (type == 2)
            {
                repositories.ToList().ForEach(repo =>
                {
                    var commits = Task.Run(() => CacheService.GetCommits(repo.FullName, period)).Result;
                    sortDictionary.Add(repo.FullName, commits.Count());
                });
            }
            else
            {
                return "error";
            }

            repositories = repositories.OrderBy(repo => sortDictionary[repo.FullName]).ToList();
            return JObject.FromObject(new
            {
                data = repositories.Select(repository => new string[]{
                repository.FullName,
                repository.Description,
                repository.Language,
                repository.StargazersCount.ToString()
            })
            }).ToString();
        }

        public async Task<string> GetStargazers(string name, int type)
        {
            var stargazers = await CacheService.GetStargazers(name, type);


            var result = stargazers.GroupBy(star =>
                DateTime.Parse(star.StarredAt.ToString()).ToShortDateString())
                .Select(group => new
                {
                    date = group.Key,
                    stars = group.Count()
                }).OrderBy(x => DateTime.Parse(x.date));

            return JObject.FromObject(new
            {
                days = result.Select(r => r.date).ToArray(),
                stars = result.Select(r => r.stars).ToArray()
            }).ToString();
        }

        public async Task<string> GetCommits(string name, int type)
        {
            var commits = await CacheService.GetCommits(name, type);
            var result = commits.GroupBy(commit =>
                DateTime.Parse(commit.Commit.Committer.Date.ToString()).ToShortDateString())
                .Select(group => new
                {
                    date = group.Key,
                    commits = group.Count()
                }).OrderBy(x => DateTime.Parse(x.date));

            return JObject.FromObject(new
            {
                days = result.Select(r => r.date).ToArray(),
                commits = result.Select(r => r.commits).ToArray()
            }).ToString();
        }

        public async Task<string> GetContributors(string name, int type)
        {
            var commits = await CacheService.GetCommits(name, type);
            var result = commits.GroupBy(commit =>
                DateTime.Parse(commit.Commit.Committer.Date.ToString()).ToShortDateString())
                .Select(group => new
                {
                    date = group.Key,
                    contributors = group.GroupBy(commit =>
                        commit.Author.Id).Count()
                }).OrderBy(x => DateTime.Parse(x.date));

            return JObject.FromObject(new
            {
                days = result.Select(r => r.date).ToArray(),
                contributors = result.Select(r => r.contributors).ToArray()
            }).ToString();
        }

        /// <summary>
        /// Check for rate limits
        /// </summary>
        /// <returns>true - rate limits enought, false - rate limits finished</returns>
        public async Task<string> IsRateLimitsValid()
        {
            var limits = await GitHubClientSingelton.Client.Miscellaneous.GetRateLimits();
            /*
           
             * */
            return (limits.Rate.Remaining != 0 &&
                limits.Resources.Core.Remaining != 0 &&
                limits.Resources.Search.Remaining != 0).ToString();
        }
        public async Task<string> RateLimits()
        {
            var limits = await GitHubClientSingelton.Client.Miscellaneous.GetRateLimits();
            return String.Format(
                "Rate(Limit:{0}, Remaining:{1})<br/>" +
                "Core(Limit:{2}, Remaining:{3})<br/>" +
                "Search(Limits:{4}, Remaining:{5})<br/>",
                limits.Rate.Limit, limits.Rate.Remaining,
                limits.Resources.Core.Limit, limits.Resources.Core.Remaining,
                limits.Resources.Search.Limit, limits.Resources.Search.Remaining);
        }
    }
}