using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.PluginTelemetry;

namespace SamplePluginAssembly
{
    public class ContactPlugin: IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var logger = (ILogger)serviceProvider.GetService(typeof(ILogger));
            using (logger.BeginScope(new Dictionary<string, object> { ["CorrelationKey"] = Guid.NewGuid() }))
            {
                const string message = "This is just a test plugin for now";

                logger.LogDebug("Setting up stuff...");
                
                var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                var sdkMessage = context.MessageName;
                //var preEntityImages = context.PreEntityImages.Keys;

                tracingService.Trace($"Contact plugin type: {message}");
                tracingService.Trace($"Message being processed: {sdkMessage}");
                
                logger.LogInformation("Contact plugin type: {message}", message);
                logger.LogInformation("Message being processed: {sdkMessage}", sdkMessage);
                
                //tracingService.Trace($"Step registered with {preEntityImages.Count} pre-entity images:");
                //foreach (var preEntityImage in preEntityImages)
                //{
                //    tracingService.Trace($" - {preEntityImage}");
                //}   
            }
        }
    }
}
