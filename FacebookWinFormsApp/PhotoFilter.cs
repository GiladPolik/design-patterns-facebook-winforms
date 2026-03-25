using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{
    internal class PhotoFilter
    {
        User m_LoggedInUser;
        Criteria m_Criteria;
        public List<Photo> m_PhotoList;

        public PhotoFilter(Criteria i_Criteria, User i_User)
        {
            m_Criteria = i_Criteria;
            m_LoggedInUser = i_User;
            GetBestAlbumsPhoto();
        }

        public void GetBestAlbumsPhoto()
        {
            m_PhotoList = new List<Photo>();
            foreach (Album album in m_LoggedInUser.Albums)
            {
                int maxLikes = 0;
                Photo bestPhoto = null;

                try
                {
                    foreach (Photo photo in album.Photos)
                    {
                        if(photo.CreatedTime >= m_Criteria.DateFrom && photo.CreatedTime <= m_Criteria.DateTo)
                        {
                            int currentPhotoLikes = getSafePhotoLikes(photo);

                            if (currentPhotoLikes > maxLikes)
                            {
                                maxLikes = currentPhotoLikes;
                                bestPhoto = photo;
                            }
                        }
                    }
                }
                catch
                {
                }

                if (bestPhoto != null)
                {
                    m_PhotoList.Add(bestPhoto);
                }
            }
        }

        private int getSafePhotoLikes(Photo i_photo)
        {
            try
            {
                if (i_photo.LikedBy != null)
                {
                    return i_photo.LikedBy.Count;
                }
            }
            catch
            {
            }

            return 0;
        }
    }
}
