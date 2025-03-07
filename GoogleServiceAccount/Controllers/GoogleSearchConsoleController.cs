using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.SearchConsole.v1;
using Google.Apis.SearchConsole.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Indexing.v3;
using System;
using Google.Apis.Requests;
using Google.Apis.Indexing.v3.Data;
using Google.Apis.Discovery;
using static Google.Apis.Indexing.v3.UrlNotificationsResource;

namespace GoogleServiceAccount.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoogleSearchConsoleController : ControllerBase
    {
        private static SearchConsoleService searchConsoleService;
        private static IndexingService indexingService;
        public GoogleSearchConsoleController()
        {

        }

        /// <summary>
        /// Zenfolio URL e.g: https://your_domain.com
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet(Name = "GoogleSerchConsole")]
        public async Task<ActionResult> Get(string url)
        {
            await GoogleIndexing();
            return Ok();

            string siteUrl = "sc-domain:zenfoliosite.com";
            using (var stream =
              new FileStream("thinking-pagoda-447206-p8-0b562cdd5548.json", FileMode.Open, FileAccess.Read))
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

                searchConsoleService = new SearchConsoleService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credentials
                });

                var response = await searchConsoleService.UrlInspection.Index.Inspect(
                    new InspectUrlIndexRequest
                    {
                        SiteUrl = siteUrl,
                        InspectionUrl = url
                    })
                    .ExecuteAsync();

                var result = searchConsoleService.Sitemaps.List(siteUrl).Execute();

                // var sitemap = searchConsoleService.Sitemaps.Get(siteUrl, url).Execute();
            }

            return Ok();
        }

        private async Task GoogleIndexing()
        {
            using (var stream =
              new FileStream("thinking-pagoda-447206-p8-0b562cdd5548.json", FileMode.Open, FileAccess.Read))
            {
                var credentials = GoogleCredential.FromStream(stream);
                if (credentials.IsCreateScopedRequired)
                {
                    credentials = credentials.CreateScoped(new string[]
                    {
                       IndexingService.Scope.Indexing
                    });
                }

                indexingService = new IndexingService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credentials
                });

                // var request = new BatchRequest(indexingService);

                //var response = await indexingService.UrlNotifications.Publish(new Google.Apis.Indexing.v3.Data.UrlNotification
                //{
                //    Url = "",
                //    Type = "URL_UPDATED"
                //}).ExecuteAsync();

                var urlToIndex = "";

                var request = indexingService.UrlNotifications;

                //var pub_response = await request.Publish(new UrlNotification
                //{
                //    Url = urlToIndex,
                //    Type = "URL_UPDATED"
                //}).ExecuteAsync();



                GetMetadataRequest meta = request.GetMetadata();
                meta.Url = urlToIndex;
                UrlNotificationMetadata m = await meta.ExecuteAsync();
            }
        }

        private void Scope()
        {
            //foreach (FeatureProjectEnum project in Enum.GetValues<FeatureProjectEnum>())
            //{
            //    serviceBuilder.ContainerBuilder.Register(ctx =>
            //    {
            //        FlagsmithSettings flagsmithSettings = ctx.Resolve<IOptions<FlagsmithSettings>>().Value.Validate();
            //        string key = flagsmithSettings.GetKey(project);

            //        if (string.IsNullOrEmpty(key))
            //            throw new InvalidOperationException($"Flagsmith key is not found for \"{project}\"");

            //        return new FlagsmithClient(
            //            key,
            //            retries: flagsmithSettings.FlagsmithRetry,
            //            requestTimeout: flagsmithSettings.FlagsmithTimeout);
            //    }).Keyed<FlagsmithClient>(project).SingleInstance();
            //}

            // FlagsmithClient flagsmithClient = _lifetimeScope.ResolveKeyed<FlagsmithClient>(project);
        }
    }
}
