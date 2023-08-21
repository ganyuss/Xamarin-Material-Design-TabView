using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Xamarin.Forms;

namespace MaterialTabView.TabView
{
    public class TabModel : INotifyPropertyChanged
    {
        private bool _selected;

        [PublicAPI]
        public bool Selected
        {
            get => _selected;
            set => SetField(ref _selected, value);
        }

        [PublicAPI]
        public int Index { get; }

        public TabModel(int index)
        {
            Index = index;
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