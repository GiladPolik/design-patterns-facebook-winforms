using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal class PostCollector
    {
        public List<Post> GetPostsByCriteria(Criteria i_Criteria, User i_User)
        {
            List<Post> filteredPosts = new List<Post>();
            foreach (Post post in i_User.Posts)
            {
                if (post.CreatedTime.HasValue &&
                    post.CreatedTime >= i_Criteria.DateFrom &&
                    post.CreatedTime <= i_Criteria.DateTo)
                {
                    filteredPosts.Add(post);
                }
            }

            return filteredPosts;
        }
    }
}
