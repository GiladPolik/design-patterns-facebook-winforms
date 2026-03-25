using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class LikeBasedStrategy : IRatingStrategy
    {
        public int CalculateScore(Post i_Post)
        {
            return getSafeLikes(i_Post);
        }

        int getSafeLikes(Post i_Post)
        {
            try
            {
                if (i_Post.LikedBy != null)
                {
                    return i_Post.LikedBy.Count;
                }
            }
            catch
            {
            }

            return 0;
        }
    }
}
