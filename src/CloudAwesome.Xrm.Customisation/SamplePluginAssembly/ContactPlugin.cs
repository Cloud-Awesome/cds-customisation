using System;
using Microsoft.Xrm.Sdk;

namespace SamplePluginAssembly
{
    public class ContactPlugin: IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            const string message = "This is just a test plugin for now";

            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            var sdkMessage = context.MessageName;
            //var preEntityImages = context.PreEntityImages.Keys;

            tracingService.Trace($"Contact plugin type: {message}");
            tracingService.Trace($"Message being processed: {sdkMessage}");
            //tracingService.Trace($"Step registered with {preEntityImages.Count} pre-entity images:");
            //foreach (var preEntityImage in preEntityImages)
            //{
            //    tracingService.Trace($" - {preEntityImage}");
            //}
        }
    }
}
