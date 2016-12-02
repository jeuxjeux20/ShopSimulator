using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopSimulator
{
    /// <summary>
    ///     Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class ShopInfo
    {
        public ShopInfo()
        {
            InitializeComponent();
        }

        private ShopItem ToDisplay { get; set; } = new ShopItem("PLACEHOLDER", 6.9);

        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is ShopItem)
                ToDisplay = e.Parameter as ShopItem;
            else
                ToDisplay = new ShopItem("PLACEHOLDER", 6.9);
        }


        private async void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
           var kek =  await ToDisplay.ShowBuyConfirmation();
            if (kek == ContentDialogResult.Primary)
            {
                this.Frame.Navigate(typeof(ShopInfo), ToDisplay);
            }
        }
    }
}