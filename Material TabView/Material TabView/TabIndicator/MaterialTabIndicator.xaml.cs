using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MaterialTabView.TabView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MaterialTabIndicator : StackLayout
    {
        public static readonly BindableProperty LabelProperty = BindableProperty.Create(
            nameof(Label),
            typeof(string),
            typeof(MaterialTabIndicator),
            string.Empty);

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
        
        public static readonly BindableProperty IconImageSourceProperty = BindableProperty.Create(
            nameof(IconImageSource),
            typeof(ImageSource),
            typeof(MaterialTabIndicator),
            propertyChanged: IconChanged);

        public ImageSource IconImageSource
        {
            get => (ImageSource)GetValue(IconImageSourceProperty);
            set => SetValue(IconImageSourceProperty, value);
        }
        
        public static readonly BindableProperty SelectedIconImageSourceProperty = BindableProperty.Create(
            nameof(SelectedIconImageSource),
            typeof(ImageSource),
            typeof(MaterialTabIndicator),
            propertyChanged: IconChanged);

        public ImageSource SelectedIconImageSource
        {
            get => (ImageSource)GetValue(SelectedIconImageSourceProperty);
            set => SetValue(SelectedIconImageSourceProperty, value);
        }
        
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(MaterialTabIndicator),
            propertyChanged: TextColorChanged);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        
        public static readonly BindableProperty SelectedTextColorProperty = BindableProperty.Create(
            nameof(SelectedTextColor),
            typeof(Color),
            typeof(MaterialTabIndicator),
            propertyChanged: TextColorChanged);

        public Color SelectedTextColor
        {
            get => (Color)GetValue(SelectedTextColorProperty);
            set => SetValue(SelectedTextColorProperty, value);
        }
        
        public static readonly BindableProperty SelectionIndicatorColorProperty = BindableProperty.Create(
            nameof(SelectionIndicatorColor),
            typeof(Color),
            typeof(MaterialTabIndicator));

        public Color SelectionIndicatorColor
        {
            get => (Color)GetValue(SelectionIndicatorColorProperty);
            set => SetValue(SelectionIndicatorColorProperty, value);
        }

        public static readonly BindableProperty SelectedProperty = BindableProperty.Create(
            nameof(Selected),
            typeof(bool),
            typeof(MaterialTabIndicator),
            propertyChanged: SelectedPropertyChanged);

        public bool Selected
        {
            get => (bool)GetValue(SelectedProperty);
            set => SetValue(SelectedProperty, value);
        }

        public Color FinalTextColor => Selected ? SelectedTextColor : TextColor;

        public ImageSource FinalIconImageSource => Selected ? SelectedIconImageSource : IconImageSource;
        
        public MaterialTabIndicator()
        {
            InitializeComponent();
        }

        private static void SelectedPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is MaterialTabIndicator materialTabIndicator)
                materialTabIndicator.SelectedPropertyChanged();
        }

        private void SelectedPropertyChanged()
        {
            TextColorChanged();
            IconChanged();

            AnimateSelectionIndicator();
        }

        private static void TextColorChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is MaterialTabIndicator materialTabIndicator)
                materialTabIndicator.TextColorChanged();
        }

        private void TextColorChanged()
        {
            OnPropertyChanged(nameof(FinalTextColor));
        }
        
        private static void IconChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is MaterialTabIndicator materialTabIndicator)
                materialTabIndicator.IconChanged();
        }

        private void IconChanged()
        {
            OnPropertyChanged(nameof(FinalIconImageSource));
        }

        private void AnimateSelectionIndicator()
        {
            if (Selected)
            {
                SelectionIndicator.Opacity = 1;

                SelectionIndicator.ScaleX = 0.5;
                SelectionIndicator.ScaleXTo(1, 150);
            }
            else
            {
                SelectionIndicator.IsVisible = true;

                SelectionIndicator.FadeTo(0, 150);
            }
        }
    }
}