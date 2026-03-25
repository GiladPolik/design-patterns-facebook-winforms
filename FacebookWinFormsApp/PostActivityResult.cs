using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{
    internal class PostActivityResult
    {
        public int m_NumberOfLikes { get; set; }
        public int m_NumberOfComments { get; set; }
        public int m_TotalScore { get; set; } // likes + comments + tags

        public PostActivityResult(int i_NumberOfLikes = 0, int i_NumberOfComments = 0, int i_TotalScore = 0)
        {
            m_NumberOfLikes = i_NumberOfLikes;
            m_NumberOfComments = i_NumberOfComments;
            m_TotalScore = i_TotalScore;
        }
    }
}
