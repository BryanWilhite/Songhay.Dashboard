using Microsoft.Extensions.Configuration;
using Songhay.Dashboard.Activities;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Songhay.Dashboard.Shell.Tests")]

namespace Songhay.Dashboard.Shell
{
    public class Program
    {
        internal static ActivitiesGetter GetActivitiesGetter(string[] args)
        {
            var getter = new DashboardActivitiesGetter(args);
            return getter;
        }

        internal static void InitializeTraceSource(TraceListener listener)
        {
            var traceSource = TraceSources
                .Instance
                .GetTraceSourceFromConfiguredName()
                .WithAllSourceLevels()
                .EnsureTraceSource();
            traceSource.Listeners.Add(listener);
        }

        internal static IConfigurationRoot LoadConfiguration(string basePath)
        {

            Console.WriteLine("Loading configuration...");
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("./appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile("./app-settings.songhay-system.json", optional: false, reloadOnChange: false);

            Console.WriteLine("Building configuration...");
            var configuration = builder.Build();

            return configuration;
        }

        static void DisplayAssemblyInfo()
        {
            Console.Write(FrameworkAssemblyUtility.GetAssemblyInfo(Assembly.GetExecutingAssembly(), true));
            Console.WriteLine(string.Empty);
            Console.WriteLine("Activities Assembly:");
            Console.Write(FrameworkAssemblyUtility.GetAssemblyInfo(typeof(DashboardActivitiesGetter).Assembly, true));
        }

        static void HandleDebug()
        {
#if DEBUG
            Console.WriteLine(string.Format("{0}Press any key to continue...", Environment.NewLine));
            Console.ReadKey(false);
#endif
        }

        static void Main(string[] args)
        {
            DisplayAssemblyInfo();

            var configuration = LoadConfiguration(Directory.GetCurrentDirectory());
            TraceSources.ConfiguredTraceSourceName = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];

            using (var listener = new TextWriterTraceListener(Console.Out))
            {
                InitializeTraceSource(listener);

                var getter = GetActivitiesGetter(args);
                var activity = getter.GetActivity();

                if (getter.Args.IsHelpRequest())
                {
                    Console.WriteLine(activity.DisplayHelp(getter.Args));
                    return;
                }

                activity
                    .WithConfiguration(configuration)
                    .Start(getter.Args);

                listener.Flush();
            }

            HandleDebug();

            Environment.Exit(0);
        }
    }
}
