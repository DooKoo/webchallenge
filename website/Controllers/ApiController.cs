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
        public string GetRepositories()
        {
            return CacheService.GetRepositories().ToString(); ;
        }
                
        public string GetOrganizations()
        {
            return "";
        }
            
        public async Task<string> GetStargazers(string name, int type)
        {
            JArray stargazers = new JArray();
            if (type == 0)
            {
                stargazers = await CacheService.GetStargazers(name, DateTime.Now.AddDays(-7), DateTime.Now);                
            }
            else if(type == 1)
            {
                stargazers = await CacheService.GetStargazers(name, DateTime.Now.AddMonths(-1), DateTime.Now);
            }

            var result = stargazers.GroupBy(star => DateTime.Parse(star["StarredAt"].ToString()).ToShortDateString())
                .Select(group => new
                {
                    date=group.Key,
                    stars=group.Count()
                });

            return JObject.FromObject(new { 
                days=result.Select(r => r.date).ToArray(),
                stars=result.Select(r => r.stars).ToArray()
            }).ToString();
        }
        /*
        public async Task<string> GetContributors(string name, int type)
        {

        }
        */

        public async Task<string> GetCommits(string name, int type)
        {
            JArray commits = new JArray();
            if (type == 0)
            {
                commits = await CacheService.GetPushes(name, DateTime.Now.AddDays(-7), DateTime.Now);
            }
            else if (type == 1)
            {
                commits = await CacheService.GetPushes(name, DateTime.Now.AddMonths(-1), DateTime.Now);
            }
            var result = commits.GroupBy(star => DateTime.Parse(star["CreatedAt"].ToString()).ToShortDateString())
                .Select(group => new
                {
                    date = group.Key,
                    stars = group.Count()
                });

            return JObject.FromObject(new
            {
                days = result.Select(r => r.date).ToArray(),
                commits = result.Select(r => r.stars).ToArray()
            }).ToString();
        }
        public async Task<string> RateLimits()
        {
            var limits = await GitHubClientSingelton.Client.Miscellaneous.GetRateLimits();
            return String.Format(
                "Rate(Limit:{0}, Remaining:{1})<br/>"+
                "Core(Limit:{2}, Remaining:{3})<br/>" +
                "Search(Limits:{4}, Remaining:{5})<br/>",
                limits.Rate.Limit, limits.Rate.Remaining,
                limits.Resources.Core.Limit,limits.Resources.Core.Remaining,
                limits.Resources.Search.Limit, limits.Resources.Search.Remaining);
        }
    }
}