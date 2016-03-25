using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace website.Utils
{
    public static class GitHubClientSingelton
    {
        public const string ClientId = "aeb8473ffe84d2facfed";
        public const string ClientSecret = "913abc9c9f8dd76c9ecbc3fa59a4d9ef8016c5d7";
        private static readonly GitHubClient _client =
            new GitHubClient(new ProductHeaderValue("WebChallengeApp"), new Uri("https://github.com/"));
        public static GitHubClient Client
        {
            get
            {
                return _client;
            }
        }
    }
}