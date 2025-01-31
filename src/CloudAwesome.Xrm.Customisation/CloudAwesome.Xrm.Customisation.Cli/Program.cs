using System;
using System.Linq;
using System.Reflection;
using CloudAwesome.Xrm.Customisation.Cli.CommandLineVerbs;
using CommandLine;

namespace CloudAwesome.Xrm.Customisation.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var verbs = LoadVerbs();

            Parser.Default.ParseArguments(args, verbs)
                .WithParsed<BaseCliVerb>(RunFeature);
        }

        private static void RunFeature(BaseCliVerb options)
        {
            if (options.OverrideManifestConnectionDetails)
            {
                options.Run(options.Manifest, options.CdsConnectionDetails);    
            }
            else
            {
                options.Run(options.Manifest);
            }
        }
        
        private static Type[] LoadVerbs()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();
        }
    }
}
