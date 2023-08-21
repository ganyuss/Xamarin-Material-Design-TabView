using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using JetBrains.Annotations;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Material_TabView.TabView
{
    //[ContentProperty(nameof(TabView.Tabs))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabView : StackLayout
    {
        private readonly ObservableCollection<Tab> _tabs = new ObservableCollection<Tab>();

        public IList<Tab> Tabs => _tabs;

        public static readonly BindableProperty TabContainerHeightProperty = BindableProperty.Create(nameof(TabContainerHeight), typeof(double), typeof(TabView), 50.0);

        public double TabContainerHeight
        {
            get => (double)GetValue(TabContainerHeightProperty);
            set => SetValue(TabContainerHeightProperty, value);
        }
        
        public static readonly BindableProperty TabContainerBackgroundColorProperty = BindableProperty.Create(nameof(TabContainerBackgroundColor), typeof(Color), typeof(TabView), Color.Transparent);

        public Color TabContainerBackgroundColor
        {
            get => (Color)GetValue(TabContainerBackgroundColorProperty);
            set => SetValue(TabContainerBackgroundColorProperty, value);
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
                CoerceTabIndexIfNecessary();
                SetupTabs();
                UpdateMainView();
            };
            
            InitializeComponent();
        }

        private void UpdateMainView()
        {
            if (Tabs.Count <= SelectedTabIndex) 
                return;

            ShownTabContent.Content = Tabs[SelectedTabIndex].TabContent;
        }

        private void SetupTabs()
        {
            TabContainer.Children.Clear();
            TabModels.Clear();

            TabContainer.ColumnDefinitions.Clear();
            for (int i = 0; i < Tabs.Count; i++)
            {
                TabContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>" + Tabs.Count);

            for (var i = 0; i < Tabs.Count; i++)
            {
                var tab = Tabs[i];
                
                int tabIndex = TabModels.Count;
                TabModel tabModel = new TabModel(tabIndex)
                {
                    Selected = SelectedTabIndex == tabIndex
                };
                tabModel.PropertyChanged += TabModelOnPropertyChanged;

                TabModels.Add(tabModel);

                View tabView = tab.TabView;
                tabView.BindingContext = tabModel;
                tabView.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() => { tabModel.Selected = true; })
                });
                tabView.HorizontalOptions = LayoutOptions.FillAndExpand;
                tabView.VerticalOptions = LayoutOptions.FillAndExpand;

                TabContainer.Children.Add(tabView, i, 0);
            }
        }

        private void TabModelOnPropertyChanged(object tabModelObject, PropertyChangedEventArgs args)
        {
            if (!(tabModelObject is TabModel tabModel))
                return;
            
            if (args.PropertyName == nameof(TabModel.Selected) && tabModel.Selected)
            {
                int tabIndex = TabModels.IndexOf(tabModel);

                if (tabIndex != -1 && SelectedTabIndex != tabIndex)
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
            CoerceTabIndexIfNecessary();
            
            for (var i = 0; i < TabModels.Count; i++)
            {
                TabModels[i].Selected = i == newValue;
            }

            UpdateMainView();
        }

        private void CoerceTabIndexIfNecessary()
        {
            if (SelectedTabIndex < 0)
                SelectedTabIndex = 0;

            if (SelectedTabIndex >= Tabs.Count && Tabs.Count > 0)
                SelectedTabIndex = Tabs.Count - 1;
        }
    }
}