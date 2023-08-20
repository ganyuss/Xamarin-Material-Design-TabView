using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using JetBrains.Annotations;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Material_TabView.TabView
{
    [ContentProperty(nameof(Tabs))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabView : StackLayout
    {
        private readonly ObservableCollection<Tab> _tabs = new ObservableCollection<Tab>();

        public IList<Tab> Tabs => _tabs;

        public static readonly BindableProperty TabContainerHeightProperty = BindableProperty.Create(nameof(TabContainerHeight), typeof(double), typeof(TabView));

        public double TabContainerHeight
        {
            get => (double)GetValue(TabContainerHeightProperty);
            set => SetValue(TabContainerHeightProperty, value);
        }
        
        public static readonly BindableProperty SelectedTabIndexProperty = BindableProperty.Create(nameof(SelectedTabIndex), typeof(int), typeof(TabView),
            propertyChanged: SelectedTabIndexChanged);

        public int SelectedTabIndex
        {
            get => (int)GetValue(SelectedTabIndexProperty);
            set => SetValue(SelectedTabIndexProperty, value);
        }

        private readonly List<TabModel> TabModels = new List<TabModel>();

        public TabView()
        {
            _tabs.CollectionChanged += (_, __) =>
            {
                SetupTabs();
                UpdateMainView();
            };
            
            InitializeComponent();

            SetupTabs();
            UpdateMainView();
        }

        private void UpdateMainView()
        {
            ShownTabContent.Content = Tabs[SelectedTabIndex].TabContent;
        }

        private void SetupTabs()
        {
            TabContainer.ColumnDefinitions.Clear();
            
            TabContainer.Children.Clear();
            TabModels.Clear();

            foreach (var tab in _tabs)
            {
                TabContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                
                int tabIndex = TabModels.Count;
                TabModel tabModel = new TabModel(tabIndex)
                {
                    Selected = SelectedTabIndex == tabIndex
                };
                tabModel.PropertyChanged += TabModelOnPropertyChanged;

                TabModels.Add(tabModel);
                
                View contentView = tab.TabView;
                contentView.BindingContext = tabModel;
                
                TabContainer.Children.Add(contentView);
            }
        }

        private void TabModelOnPropertyChanged(object tabModelObject, PropertyChangedEventArgs args)
        {
            if (!(tabModelObject is TabModel tabModel))
                return;
            
            if (args.PropertyName == nameof(TabModel.Selected))
            {
                int tabIndex = TabModels.IndexOf(tabModel);

                if (tabIndex != -1)
                    SelectedTabIndex = tabIndex;
            } 
        }
        
        private static void SelectedTabIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TabView tabView
                && oldValue is int oldIntValue
                && newValue is int newIntValue)
                tabView.SelectedTabIndexChanged(oldIntValue, newIntValue);
        }

        private void SelectedTabIndexChanged(int oldValue, int newValue)
        {
            for (var i = 0; i < TabModels.Count; i++)
            {
                TabModels[i].Selected = i == newValue;
            }

            UpdateMainView();
        }
    }
}