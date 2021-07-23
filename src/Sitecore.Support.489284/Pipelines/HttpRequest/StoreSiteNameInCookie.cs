using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.XA.Foundation.Abstractions;
using Sitecore.XA.Foundation.Multisite.Extensions;

namespace Sitecore.XA.Foundation.Multisite.Pipelines.HttpRequest
{
    public class StoreSiteNameInCookie : HttpRequestProcessor
    {
        protected IContext Context { get; } = ServiceLocator.ServiceProvider.GetService<IContext>();

        public override void Process(HttpRequestArgs args)
        {
            if (args.Context.Response.HeadersWritten)
            {
                return;
            }
            if (Context.Site != null && Context.Site.Name != "shell" && Context.Item != null)
            {
                var siteInfo = ServiceLocator.ServiceProvider.GetService<ISiteInfoResolver>().GetSiteInfo(Context.Item);
                if (siteInfo != null)
                {
                    args.Context.Response.Cookies.Add(new HttpCookie("sxa_site", siteInfo.Name));
                }
            }
        }
    }
}