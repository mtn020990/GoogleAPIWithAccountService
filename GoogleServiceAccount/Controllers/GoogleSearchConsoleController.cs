using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.SearchConsole.v1;
using Google.Apis.SearchConsole.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GoogleServiceAccount.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoogleSearchConsoleController : ControllerBase
    {
        public GoogleSearchConsoleController()
        {

        }

        /// <summary>
        /// Zenfolio URL e.g: https://your_domain.zenfoliosite.com
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet(Name = "GoogleSerchConsole")]
        public ActionResult Get(string url)
        {
            string siteUrl = "https://yoursite.com/";
            using (var stream =
              new FileStream("thinking-pagoda-447206-p8-5b2ac55ac4ca.json", FileMode.Open, FileAccess.Read))
            {
                var credentials = GoogleCredential.FromStream(stream);
                if (credentials.IsCreateScopedRequired)
                {
                    credentials = credentials.CreateScoped(new string[]
                    {
                        SearchConsoleService.Scope.Webmasters,
                        SearchConsoleService.Scope.WebmastersReadonly
                    });
                }

                var service = new SearchConsoleService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credentials
                });

                //var response = service.UrlInspection.Index.Inspect(
                //    new InspectUrlIndexRequest
                //    {
                //        SiteUrl = siteUrl,
                //        InspectionUrl = url
                //    })
                //    .Execute();

                // var result = service.Sitemaps.List(siteUrl).Execute();

                var sitemap = service.Sitemaps.Get(siteUrl, url).Execute();
            }

            return Ok();
        }
    }
}
