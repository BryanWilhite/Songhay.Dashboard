using Songhay.Models;

namespace Songhay.Dashboard.Activities
{
    public class DashboardActivitiesArgs : ProgramArgs
    {
        public DashboardActivitiesArgs(string[] args) : base(args)
        {
        }

        public const string ServerAssemblyFile = "--server-assembly-file";
    }
}
