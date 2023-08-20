using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Material_TabView.TabView
{

    [ContentProperty(nameof(TabContent))]
    public class Tab : INotifyPropertyChanged
    {
        private View _content;
        private View _tabView;

        public View TabContent
        {
            get => _content;
            set => SetField(ref _content, value);
        }

        public View TabView
        {
            get => _tabView;
            set => SetField(ref _tabView, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}