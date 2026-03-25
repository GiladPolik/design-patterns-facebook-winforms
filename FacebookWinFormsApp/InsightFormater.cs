using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{
    internal class InsightFormater
    {
        public string Format(List<Post> i_Posts, DayOfWeek i_Day)
        {
            return (i_Posts.Count > 0 ? $"Your posts recieved more traffic on {i_Day}s" : "No posts were found in those dates.");
        }
    }
}
