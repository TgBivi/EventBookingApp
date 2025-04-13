using Logic.Interfaces;
using Logic.Services;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Logic.Composers
{
    public class ServiceExtensionComposer:IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            //Register all your services and helpers under here
            builder.Services.AddScoped<IEventService, EventService>();
        }
    }
}
