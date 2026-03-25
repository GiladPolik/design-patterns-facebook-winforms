using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public interface IIterator<T>
    {
        bool HasNext();
        T Next();
        bool HasPrevious();
        T Previous();
        T Current();
    }
}
