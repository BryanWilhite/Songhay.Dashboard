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

        internal static void InitializeTraceSource(TraceListener listener, IConfigurationRoot configuration)
        {
            var traceSource = TraceSources
                .Instance
                .GetConfiguredTraceSource(configuration);
            traceSource?.Listeners.Add(listener);
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

        internal static void WriteException(Exception ex)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine($@"
EXCEPTION:
{nameof(ex.Source)}: {ex.Source}
{nameof(ex.HResult)}: {ex.HResult}
{nameof(ex.Message)}: {ex.Message}
{nameof(ex.StackTrace)}: {ex.StackTrace}
");
            Console.ResetColor();
        }

        static void DisplayAssemblyInfo()
        {
            Console.Write(ProgramAssemblyUtility.GetAssemblyInfo(Assembly.GetExecutingAssembly(), true));
            Console.WriteLine(string.Empty);
            Console.WriteLine("Activities Assembly:");
            Console.Write(ProgramAssemblyUtility.GetAssemblyInfo(typeof(DashboardActivitiesGetter).Assembly, true));
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
            var listener = new TextWriterTraceListener(Console.Out);
            try
            {
                InitializeTraceSource(listener, configuration);

                var getter = GetActivitiesGetter(args);
                var activity = getter.GetActivity();

                if (getter.Args.IsHelpRequest())
                {
                    Console.WriteLine(activity.DisplayHelp(getter.Args));
                    Environment.Exit(0);
                }

                activity
                    .WithConfiguration(configuration)
                    .Start(getter.Args);
            }
            catch (Exception ex)
            {
                WriteException(ex);

                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception:");
                    WriteException(ex.InnerException);
                }

                Environment.ExitCode = (ex.HResult != 0) ? ex.HResult : -1;
                HandleDebug();

                Environment.Exit(Environment.ExitCode);
            }
            finally
            {
                listener.Flush();
            }

            HandleDebug();

            Environment.Exit(0);
        }
    }
}
