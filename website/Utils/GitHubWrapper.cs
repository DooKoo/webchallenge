﻿using Newtonsoft.Json.Linq;
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

        /// <summary>
        /// Return all stargazers of repository
        /// </summary>
        /// <param name="name">Name of repository</param>
        /// <returns>List of stargazers</returns>
        public static async Task<IReadOnlyList<UserStar>> GetStargazers(string name)
        {
            var splitted = name.Split('/');

            if (splitted.Length != 2)
                return new List<UserStar>();

            var stargazers = await GitHubClientSingelton.Client.Activity.Starring
                .GetAllStargazersWithTimestamps(splitted[0], splitted[1]);

            return stargazers;
        }

        /// <summary>
        /// Return commits by period
        /// </summary>
        /// <param name="name">Name of the repository</param>
        /// <param name="type">Describe period: 0 - weekly commits, 1 - monthly commits</param>
        /// <returns>List of commits</returns>
        public static async Task<IReadOnlyList<GitHubCommit>> GetCommits(string name, int type)
        {
            var splitted = name.Split('/');

            if (splitted.Length != 2)
                return new List<GitHubCommit>();

            var request = new CommitRequest()
            {
                Since = type == 0 ? DateTime.Now.AddDays(-7) : DateTime.Now.AddMonths(-1)
            };
            var commits = await GitHubClientSingelton.Client.Repository.Commit.GetAll(splitted[0], splitted[1],request);

            return commits;
        }
    }
}