using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class PhotoIterator : IIterator<Photo>
    {
        private readonly List<Photo> m_Photos;
        private int m_CurrentIndex = -1;

        public PhotoIterator(List<Photo> i_Photos)
        {
            m_Photos = i_Photos ?? new List<Photo>();
        }

        public bool HasNext() => m_CurrentIndex < m_Photos.Count - 1;

        public bool HasPrevious() => m_CurrentIndex > 0;

        public Photo Next() => HasNext() ? m_Photos[++m_CurrentIndex] : null;

        public Photo Previous() => HasPrevious() ? m_Photos[--m_CurrentIndex] : null;

        public Photo Current() => (m_CurrentIndex >= 0 && m_CurrentIndex < m_Photos.Count) ? m_Photos[m_CurrentIndex] : null;
    }
}
