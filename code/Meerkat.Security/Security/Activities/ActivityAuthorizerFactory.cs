using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using Meerkat.Security.Activities.Configuration;

namespace Meerkat.Security.Activities
{
    public class ActivityAuthorizerFactory
    {
        public IActivityAuthorizer Create(string name = "activityAuthorization", string commonServiceUrl = null)
        {
            var section = ActivitySection(name);
            var activities = section.ToActivitites();

            if (commonServiceUrl != null)
            {
                //var commonActivites = GetCommonActivities(commonServiceUrl);
                //MergeActivities(activities, commonActivites);
            }

            return new ActivityAuthorizer(activities, section.Default, section.DefaultActivity);
        }

        //private IEnumerable<Activity> GetCommonActivities(string commonServiceUrl)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
        //        {
        //            client.BaseAddress = new Uri(commonServiceUrl);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //            // New code:
        //            var response = client.GetAsync("api/activity");
        //            response.Result.EnsureSuccessStatusCode();
        //            var result = response.Result.Content.ReadAsAsync<IEnumerable<Activity>>().Result;
        //            return result;
        //        }
        //    }
        //    // TODO: Do we want some sort of retry mechansim?
        //    catch (Exception ex)
        //    {
        //        Tracing.Error(ex);
        //        return new List<Activity>
        //        {
        //            new Activity
        //            {
        //                Resource = "Home",
        //                Action = "Index",
        //                Allow = new Permission { Roles = new [] { "ANGLO\\Domain Users" } }
        //            }
        //        };
        //    }
        //}

        private ActivityAuthorizationSection ActivitySection(string name)
        {
            try
            {
                var section = (ActivityAuthorizationSection)ConfigurationManager.GetSection(name);

                return section;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void MergeActivities(IList<Activity> existingActivities, IEnumerable<Activity> newActivities )
        {
            foreach (var activity in newActivities)
            {
                var current = activity;
                var exists = (from x in existingActivities where x.Name == current.Name select x).FirstOrDefault();
                if (exists == null)
                {
                    existingActivities.Add(activity);
                }
            }
        }
    }
}