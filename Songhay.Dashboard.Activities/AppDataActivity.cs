using Microsoft.Extensions.Configuration;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using System.Diagnostics;

namespace Songhay.Dashboard.Activities
{
    public class AppDataActivity : IActivity, IActivityConfigurationSupport
    {
        static AppDataActivity() => traceSource = TraceSources
            .Instance
            .GetTraceSourceFromConfiguredName()
            .WithAllSourceLevels()
            .EnsureTraceSource();

        public void AddConfiguration(IConfigurationRoot configuration)
        {
            this.Configuration = configuration;
        }

        public string DisplayHelp(ProgramArgs args) => "Updates the conventional static JSON file in Azure Storage for the Web App in this Solution.";

        public void Start(ProgramArgs args)
        {
        }

        internal IConfigurationRoot Configuration { get; private set; }

        static readonly TraceSource traceSource;
    }
}
