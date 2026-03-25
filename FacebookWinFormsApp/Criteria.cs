using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BasicFacebookFeatures
{
    public class Criteria : INotifyPropertyChanged
    {
        private DateTime dateFrom;
        private DateTime dateTo;
        private string m_AnalysisResult;
        private string m_AlbumPhotoUrl;
        public event PropertyChangedEventHandler PropertyChanged;

        public Criteria(DateTime i_DateFrom, DateTime i_DateTo)
        {
            dateFrom = i_DateFrom;
            dateTo = i_DateTo;
        }

        public DateTime DateFrom
        {
            get { return dateFrom; }
            set
            {
                if (dateFrom != value)
                {
                    dateFrom = value;
                    OnPropertyChanged(); // Notify UI
                }
            }
        }

        public DateTime DateTo
        {
            get { return dateTo; }
            set
            {
                if (dateTo != value)
                {
                    dateTo = value;
                    OnPropertyChanged(); // Notify UI
                }
            }
        }

        public string AnalysisResult
        {
            get { return m_AnalysisResult; }
            set
            {
                if (m_AnalysisResult != value)
                {
                    m_AnalysisResult = value;
                    OnPropertyChanged();
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
