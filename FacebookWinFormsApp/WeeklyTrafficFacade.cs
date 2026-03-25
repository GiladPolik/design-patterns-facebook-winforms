using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class WeeklyTrafficFacade
    {
        public string Analyze(Criteria criteria, IRatingStrategy i_RatingStrategy)
        {
            User user = FacebookSession.Instance.LoggedInUser;
            PostCollector collector = new PostCollector();
            DailyTrafficCalculator traffic = new DailyTrafficCalculator();
            InsightFormater insights = new InsightFormater();
            List<Post> posts = collector.GetPostsByCriteria(criteria, user);
            DayOfWeek weeklyData = traffic.GetBestAverageDay(posts, i_RatingStrategy);
            
            return insights.Format(posts, weeklyData);
        }
    }
}
