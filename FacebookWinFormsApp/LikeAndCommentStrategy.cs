using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class LikeAndCommentStrategy : IRatingStrategy
    {
        public int CalculateScore(Post i_Post)
        {
            int likes = getSafeLikes(i_Post);
            int comments = getSafeComment(i_Post);
            return (likes + comments);
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

        int getSafeComment(Post i_Post)
        {
            try
            {
                if (i_Post.Comments != null)
                {
                    return i_Post.Comments.Count;
                }
            }
            catch
            {
            }

            return 0;
        }
    }
}
