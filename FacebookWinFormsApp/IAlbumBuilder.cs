using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public interface IAlbumBuilder
    {
        void BuildAlbumName(string i_Name);
        void BuildPhotoList(List<Photo> i_Photos);
        void BuildDescription(string i_Description);
        Album GetResult();
    }
}
