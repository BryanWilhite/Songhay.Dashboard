using Songhay.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Songhay.Dashboard.Shell.Tests")]

namespace Songhay.Dashboard.Activities
{
    public class DashboardActivitiesGetter : ActivitiesGetter
    {
        public DashboardActivitiesGetter(string[] args) : base(args)
        {
            this.LoadActivities(new Dictionary<string, Lazy<IActivity>>
            {
                {
                    nameof(AppDataActivity),
                    new Lazy<IActivity>(() => new AppDataActivity())
                }
            });
        }
    }
}
