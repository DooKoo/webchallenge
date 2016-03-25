using Newtonsoft.Json.Linq;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using website.Utils;

namespace website.Controllers
{
    public class HomeController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            
            /*
            try
            {
                // The following requests retrieves all of the user's repositories and
                // requires that the user be logged in to work.
                var repositories = GitHubClientSingelton.Client.Repository.GetAllForCurrent().Result;
                //var model = new IndexViewModel(repositories);
                
            }
            catch (AuthorizationException)
            {
                // Either the accessToken is null or it's invalid. This redirects
                // to the GitHub OAuth login page. That page will redirect back to the
                // Authorize action.
                //Redirect(GetOauthLoginUrl());
            }
             * */
        }
        
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            /*
            var searchRequest = new SearchRepositoriesRequest();
            searchRequest.Updated = new DateRange(DateTime.Now.AddMonths(-1), DateTime.Now);
            searchRequest.PerPage = 500;
            var result = await GitHubClientSingelton.Client.Search.SearchRepo(searchRequest);
            */
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Repositories()
        {                     
            var limits = await GitHubClientSingelton.Client.Miscellaneous.GetRateLimits();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Repository(string name)
        {
            if (name == null)
                return View("Error");

            var repository = await CacheService.GetRepositoryDetails(name);
            repository.Add("chartType", 0);
            return View(repository);
        }

        [HttpGet]
        public ActionResult Organizations()
        {
            return View();
        }

        public async Task<ActionResult> Authorize(string code, string state)
        {
            if (!String.IsNullOrEmpty(code))
            {
                var expectedState = Session["CSRF:State"] as string;
                if (state != expectedState) throw new InvalidOperationException("SECURITY FAIL!");
                Session["CSRF:State"] = null;

                var token = await GitHubClientSingelton.Client.Oauth.CreateAccessToken(
                    new OauthTokenRequest(GitHubClientSingelton.ClientId, GitHubClientSingelton.ClientSecret, code)
                    {
                        RedirectUri = new Uri("http://webchallengeapp.azurewebsites.net/Home/Authorize")//TODO:Change url
                    });
                Session["OAuthToken"] = token.AccessToken;
                GitHubClientSingelton.Client.Credentials = new Credentials(token.AccessToken);
                return RedirectToAction("Index");
            }

            return Redirect(GetOauthLoginUrl());
        }

        private string GetOauthLoginUrl()
        {
            string csrf = Membership.GeneratePassword(24, 1);
            Session["CSRF:State"] = csrf;

            var request = new OauthLoginRequest(GitHubClientSingelton.ClientId)
            {
                Scopes = { "public_repo"},
                State = csrf
            };
            var oauthLoginUrl = GitHubClientSingelton.Client.Oauth.GetGitHubLoginUrl(request);
            return oauthLoginUrl.ToString();
        }
    }
}