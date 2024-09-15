using Songhay.Models;
using System;
using System.Collections.Generic;
using Songhay.Abstractions;

namespace Songhay.Dashboard.Activities;

public sealed class DashboardActivitiesGetter : ActivitiesGetter
{
    public DashboardActivitiesGetter(string[] args) : base(args)
    {
        LoadActivities(new Dictionary<string, Lazy<IActivity?>>
        {
            {
                nameof(AppDataActivity),
                new Lazy<IActivity?>(() => new AppDataActivity())
            }
        });
    }
}