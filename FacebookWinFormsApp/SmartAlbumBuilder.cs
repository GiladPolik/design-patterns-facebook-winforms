using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class SmartAlbumBuilder : IAlbumBuilder
    {
        private User m_LoggedInUser;
        private string m_AlbumName;
        private string m_Description;
        private List<Photo> m_PhotosToUpload;
        private Album m_CreatedAlbum;

        public SmartAlbumBuilder(User i_User)
        {
            m_LoggedInUser = i_User;
            m_PhotosToUpload = new List<Photo>();
        }

        public void BuildAlbumName(string i_Name)
        {
            m_AlbumName = i_Name;
        }

        public void BuildPhotoList(List<Photo> i_Photos)
        {
            m_PhotosToUpload = i_Photos;  
        }

        public void BuildDescription(string i_Description)
        {
            m_Description = i_Description;
        }

        public Album GetResult()
        {
            if (m_CreatedAlbum == null)
            {
                createAlbumProcess();
            }

            return m_CreatedAlbum;
        }

        private void createAlbumProcess()
        {
            if(string.IsNullOrEmpty(m_AlbumName))
            {
                throw new Exception("Cannot create album without a name");
            }

            try
            {
                m_CreatedAlbum = m_LoggedInUser.CreateAlbum(m_AlbumName, m_Description);
                if(m_PhotosToUpload != null)
                {
                    foreach(Photo photo in m_PhotosToUpload)
                    {
                        m_CreatedAlbum.Photos.Add(photo);
                    }
                }
            }
            catch
            {
                m_CreatedAlbum = null;
            }
        }
    }
}
