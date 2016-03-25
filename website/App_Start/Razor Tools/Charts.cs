using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Mvc.RazorTools.Charts.App_Start), "Initialize")]

// ***
// *** Add @Mvc.RazorTools.BundleManager.Styles.Render() to the top of your pages
// *** or to your layout view.
// ***
// *** Add @Mvc.RazorTools.BundleManager.Scripts.Render() to the bottom of your pages
// *** or to your layout view.
// ***

namespace Mvc.RazorTools.Charts
{
	/// <summary>
	/// Provides an initialization mechanism when the web application starts.
	/// </summary>
	public class App_Start
	{
		/// <summary>
		/// Adds the necessary style sheets and JavScript libraries to the
		/// bundle table.
		/// </summary>
		public static void Initialize()
		{
			// ***
			// *** Add styles to the Razor Tools bundle
			// ***
			Mvc.RazorTools.BundleManager.Styles.Include("~/Content/morris.css");

			// ***
			// *** Add styles to the Razor Tools bundle
			// ***
			Mvc.RazorTools.BundleManager.Scripts.Include("~/Scripts/jquery.ba-throttle-debounce.js");
			Mvc.RazorTools.BundleManager.Scripts.Include("~/Scripts/raphael.js");
			Mvc.RazorTools.BundleManager.Scripts.Include("~/Scripts/morris.js");
			Mvc.RazorTools.BundleManager.Scripts.Include("~/Scripts/moment-with-langs.js");
			Mvc.RazorTools.BundleManager.Scripts.Include("~/Scripts/razortools.charts.unobtrusive.js");
		}
	}
}