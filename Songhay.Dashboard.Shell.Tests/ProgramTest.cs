using Songhay.Dashboard.Activities;
using Songhay.Extensions;
using Songhay.Models;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Dashboard.Shell.Tests
{
    public class ProgramTest
    {
        public ProgramTest(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;

            this._basePath = ProgramAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, @"..\..\..\");
        }

        [Fact]
        public void ShouldRunAppDataActivity()
        {
            var projectDirectoryInfo = new DirectoryInfo(this._basePath);

            var shellDirectoryInfo = projectDirectoryInfo
                .Parent.ToReferenceTypeValueOrThrow().GetDirectories("*.Shell").Single();

            var configuration = Program.LoadConfiguration(shellDirectoryInfo.FullName);

            using (var writer = new StringWriter())
            using (var listener = new TextWriterTraceListener(writer))
            {
                Program.InitializeTraceSource(listener, configuration);

                var metaSection = configuration.GetSection("meta")?.GetChildren();
                Assert.True(metaSection?.Any(), "The expected section is not here.");

                var args = new[]
                {
                    nameof(AppDataActivity),
                    ProgramArgs.BasePath, projectDirectoryInfo.FullName
                };
                var activitiesGetter = Program.GetActivitiesGetter(args);
                var activity = activitiesGetter
                    .GetActivity()
                    .WithConfiguration(configuration) as AppDataActivity;
                Assert.NotNull(activity);

                activity.Start(new ProgramArgs(args));

                listener.Flush();

                this._testOutputHelper.WriteLine(writer.ToString());
            }
        }

        readonly string _basePath;
        readonly ITestOutputHelper _testOutputHelper;
    }
}