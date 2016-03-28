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
    /// Data updating every 3 hour
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
            ExpirationTimesDictionary[key] = DateTime.Now.AddHours(3);
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
        /// <returns>Repository</returns>
        public static async Task<Repository> GetRepositoryDetails(string name)
        {
            if (!IsDataValid(name, typeof(Repository)))
            {
                RepositoryDetailsDictionary[name] = await GitHubWrapper.GetRepository(name);
                AddExpirationTime(name, typeof(Repository));
            }

            return RepositoryDetailsDictionary[name];
        }        

        /// <summary>
        /// Return chached stargazers
        /// </summary>
        /// <param name="name">Name of repository</param>
        /// <param name="from">Minimal date of stargazing</param>
        /// <param name="to">Maximal date of stargazing</param>
        /// <returns>Stargazers of repository on json format</returns>
        public static async Task<IEnumerable<UserStar>> GetStargazers(string name, int type)
        {
            if (!IsDataValid(name, typeof(UserStar)))
            {                
                StargazersDictionary[name] = await GitHubWrapper.GetStargazers(name);
                AddExpirationTime(name, typeof(UserStar));
            }

            var stargazers = StargazersDictionary[name];
            IEnumerable<UserStar> stargazersFiltered;
            if(type == 0)
                return stargazers.Where(stargazer =>
                stargazer.StarredAt >= DateTime.Now.AddDays(-7));
            else if(type == 1)
                return stargazersFiltered = stargazers.Where(stargazer =>
                stargazer.StarredAt >= DateTime.Now.AddMonths(-1));

            return new List<UserStar>();
        }

        /// <summary>
        /// Return cached commits
        /// </summary>
        /// <param name="name">Name of the repository</param>
        /// <param name="type">Describe period: 0 - weekly commits, 1 - monthly commits</param>
        /// <returns>IReadOnlyList that contains commits of repository</returns>
        public static async Task<IReadOnlyList<GitHubCommit>> GetCommits(string name, int type)
        {
            Dictionary<string, IReadOnlyList<GitHubCommit>> commitsDictionary;

            if(type == 0)
                commitsDictionary = WeekCommitsDictionary;
            else if(type == 1)
                commitsDictionary = MonthCommitsDictionary;
            else
                return new List<GitHubCommit>();
            
            if (!IsDataValid(name, typeof(GitHubCommit), type.ToString()))
            {
                commitsDictionary[name] = await GitHubWrapper.GetCommits(name, type);
                AddExpirationTime(name, typeof(GitHubCommit), type.ToString());
            }

            return commitsDictionary[name];
        }

        /// <summary>
        /// Get all cached repositories from GitHubApiWrapper
        /// </summary>
        /// <returns>IReadOnlyList of repositories</returns>
        public static async Task<IReadOnlyList<Repository>> GetRepositories()
        {
            var repositories = await GitHubWrapper.GetRepositories();
            repositories.ToList().ForEach(repository =>
            {
                if (!IsDataValid(repository.FullName, typeof(Repository)))
                {
                    RepositoryDetailsDictionary[repository.FullName] = repository;
                    AddExpirationTime(repository.FullName, typeof(Repository));
                }
            });
            var stars = repositories.Select(r => r.StargazersCount).Sum();
            return repositories;
        }
    }
}