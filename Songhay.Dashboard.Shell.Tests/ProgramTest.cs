using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Dashboard.Activities;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace Songhay.Dashboard.Shell.Tests
{
    [TestClass]
    public class ProgramTest
    {
        public TestContext TestContext { get; set; }

        [Ignore("The build server should ignore this test because it should run locally.")]
        [TestMethod]
        [TestProperty("serverAssemblyFile", @"bin\Release\netcoreapp2.0\Songhay.Dashboard.dll")]
        public void ShouldRunAppDataActivity()
        {
            var shellDirectoryInfo = this.TestContext.ShouldGetSiblingDirectoryInfoByName(this.GetType(), typeof(Program).Namespace);
            var webDirectoryInfo = this.TestContext.ShouldGetConventionalProjectDirectoryInfo(this.GetType());
            var projectDirectoryInfo = this.TestContext.ShouldGetProjectDirectoryInfo(this.GetType());

            #region test properties:

            var serverAssemblyFile = this.TestContext.Properties["serverAssemblyFile"].ToString();
            serverAssemblyFile = webDirectoryInfo.FullName.ToCombinedPath(serverAssemblyFile);
            this.TestContext.ShouldFindFile(serverAssemblyFile);

            #endregion

            var configuration = Program.LoadConfiguration(shellDirectoryInfo.FullName);
            TraceSources.ConfiguredTraceSourceName = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];

            using (var listener = new TextWriterTraceListener(Console.Out))
            {
                Program.InitializeTraceSource(listener);

                var metaSection = configuration.GetSection("meta")?.GetChildren();
                Assert.IsTrue(metaSection.Any(), "The expected section is not here.");

                var args = new[]
                {
                    nameof(AppDataActivity),
                    ProgramArgs.BasePath, projectDirectoryInfo.FullName,
                    DashboardActivitiesArgs.ServerAssemblyFile, serverAssemblyFile
                };
                var activitiesGetter = Program.GetActivitiesGetter(args);
                var activity = activitiesGetter
                    .GetActivity()
                    .WithConfiguration(configuration) as AppDataActivity;
                Assert.IsNotNull(activity, "The expected Activity is not here.");

                activity.Start(new DashboardActivitiesArgs(args));

                listener.Flush();
            }
        }
    }
}