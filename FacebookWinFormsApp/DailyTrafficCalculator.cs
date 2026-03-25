using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal class DailyTrafficCalculator
    {
        internal struct DayStatistics
        {
            public float m_AvarageScore;
            public int m_PostCount;
        }

        public DayOfWeek GetBestAverageDay(IEnumerable<Post> i_Posts, IRatingStrategy i_RatingStrategy)
        {
            DayStatistics[] statsPerDay = new DayStatistics[7];

            foreach (Post post in i_Posts)
            {
                int dayIndex = (int)post.CreatedTime.Value.DayOfWeek;
                int score = i_RatingStrategy.CalculateScore(post);

                DayStatistics stats = statsPerDay[dayIndex];
                stats.m_AvarageScore = (stats.m_AvarageScore * stats.m_PostCount + score) / (stats.m_PostCount + 1);
                stats.m_PostCount++;
                statsPerDay[dayIndex] = stats;
            }

            return findBestDay(statsPerDay);
        }

        private DayOfWeek findBestDay(DayStatistics[] i_StatsPerDay)
        {
            double maxAverage = -1;
            int bestDayIndex = 0;

            for (int i = 0; i < i_StatsPerDay.Length; i++)
            {
                if (i_StatsPerDay[i].m_PostCount > 0 && i_StatsPerDay[i].m_AvarageScore > maxAverage)
                {
                    maxAverage = i_StatsPerDay[i].m_AvarageScore;
                    bestDayIndex = i;
                }
            }

            return (DayOfWeek)bestDayIndex;
        }
    }
}
