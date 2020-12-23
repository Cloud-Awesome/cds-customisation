using System;
using Microsoft.Xrm.Sdk;

namespace SamplePluginAssembly
{
    public class AccountPlugin: IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            const string message = "This is just a test plugin for now";

            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            var sdkMessage = context.MessageName;
            
            tracingService.Trace($"Account plugin type: {message}");
            tracingService.Trace($"Message being processed: {sdkMessage}");
        }
    }
}
