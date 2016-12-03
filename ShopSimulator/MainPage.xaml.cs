using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using ShopSimulator.Annotations;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ShopSimulator
{
    /// <summary>
    ///     Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : INotifyPropertyChanged
    {
        public static Frame MainFrame;
        public static MainPage Myself;
        private readonly ObservableCollection<ShopItem> _boughtItems = new ObservableCollection<ShopItem>();
        private readonly ObservableCollection<ShopItem> _shopItems = new ObservableCollection<ShopItem>();
        private double _balance = 100;

        public MainPage()
        {
            InitializeComponent();
            _shopItems.Add(new ShopItem("FireC", 1.50));
            _shopItems.Add(new ShopItem("MDMCK10", 0.85, "Wanted to ragequit discord"));
            _shopItems.Add(new ShopItem("cth103", 1.25));
            _shopItems.Add(new ShopItem("Visual studio 2019 future edition", 175.55));
            _shopItems.Add(new ShopItem("topkek strawberry", 0.25));
            _shopItems.Add(new ShopItem("Shit", 0.01));
            _shopItems.Add(new ShopItem("jeuxjeux20", 0.20));
            _shopItems.Add(new ShopItem("Chaten", 7.5, "Likes to FUCK OFF hahahaha"));
            _shopItems.Add(new ShopItem("darkshadow97056", 3.1, "Likes dogecoins", "Have school wowowowow"));
            //  _shopItems.Add(new ShopItem("LONG NAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAME",2.5,"test name ww"));
            Main.ItemsSource = _shopItems;
            MainFrame = Frame;
            Myself = this;
            DataContext = this;
            Bought.ItemsSource = _boughtItems;
            Main.SelectionChanged += Main_SelectionChanged;
        }

        public double Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                // ReSharper disable once RedundantArgumentDefaultValue
                OnPropertyChanged("Balance");
            }
        }

        public bool IsThereAnyChecked => Main.SelectedItems.Count > 0;

        public event PropertyChangedEventHandler PropertyChanged;

        private void Main_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            SAppBarButton.IsEnabled = Main.SelectedItems.Count > 0;
        }

        public void RequestBought(ShopItem s)
        {
            _shopItems.RemoveAt(_shopItems.IndexOf(s));
            _boughtItems.Add(s);
        }

        private void Uncheck(object sender, RoutedEventArgs e) => Main.SelectionMode = ListViewSelectionMode.Extended;

        private void Checkd(object sender, RoutedEventArgs e) => Main.SelectionMode = ListViewSelectionMode.Multiple;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Balance += 10;
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var thingsShopItems = new List<ShopItem>();
            foreach (var item in Main.SelectedItems)
                thingsShopItems.Add(item as ShopItem ?? new ShopItem("owo what's dis", 7));
            var k = "";
            double totalPrice = 0;
            foreach (var textie in thingsShopItems)
            {
                k += $"● {textie.Name} for {textie.Price}$ {Environment.NewLine}";
                totalPrice += textie.Price;
            }
            var canAfford = Balance >= totalPrice;
            var d = new ContentDialog
            {
                PrimaryButtonText = canAfford ? "Yes" : "Not enough money",
                SecondaryButtonText = "No!",
                Title = "Buy confirmation",
                IsPrimaryButtonEnabled = canAfford,
                Content = new Grid
                {
                    Children =
                    {
                        new TextBlock
                        {
                            Text = k
                        }
                    }
                }
            };
            var ok = await d.ShowAsync();
            if (ok == ContentDialogResult.Primary)
                foreach (var item in thingsShopItems)
                    item.Buy();
        }

        private void Ono(object sender, TextChangedEventArgs e)
        {
            var tokekxt = sender as TextBox;
            double okm8;
            if (tokekxt != null && double.TryParse(tokekxt.Text, out okm8))
            {
                if (okm8 > Balance)
                    tokekxt.Text = Balance.ToString(CultureInfo.CurrentCulture);
                if (okm8 < 0)
                    tokekxt.Text = "0.01";
                Okdammit.IsEnabled = true;
            }
            else
            {
                Okdammit.IsEnabled = false;
            }
        }


        private void Okdammit_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var kek = new ShopItem(NameBoxNew.Text, double.Parse(PriceBoxNew.Text), DescNew.Text, VendDescNew.Text);
            _shopItems.Add(kek);
            MehFlyout.Hide();
        }
    }

    public class ShopItem
    {
        private string _description;
        private double _price;
        private string _vendorDesc;

        public ShopItem()
        {
        }

        public ShopItem(string n)
        {
            Name = n;
        }

        public ShopItem(string n, double p, string desc = null, string vendordesc = null)
        {
            Name = n;
            _price = p;
            Description = desc;
            VendorDesc = vendordesc;
        }

        public string Name { get; set; }

        public string Description
        {
            get { return string.IsNullOrEmpty(_description) ? "No description provided" : _description; }
            set { _description = value; }
        }

        public string VendorDesc
        {
            get { return string.IsNullOrEmpty(_vendorDesc) ? "No vendor description provided" : _vendorDesc; }
            set { _vendorDesc = value; }
        }

        public double Price
        {
            get { return Math.Round(_price, 2); }
            set { _price = value; }
        }

        public string UpperName => Name.ToUpper();
        public bool CanAfford => MainPage.Myself.Balance >= Price;
        public bool IsBought { get; protected set; }
        public bool IsBoughtInversed => !IsBought;

        public void FlyoutInfoRequest()
        {
            MainPage.MainFrame.Navigate(typeof(ShopInfo), this);
            // await new MessageDialog("omg").ShowAsync();           
        }

        public void TapFlyoutInfoRequest(object sender, TappedRoutedEventArgs e)
        {
            FlyoutInfoRequest();
        }

        public void DoubleTapFlyoutInfoRequest(object sender, DoubleTappedRoutedEventArgs e)
        {
            FlyoutInfoRequest();
        }

        public async void Buy()
        {
            if ((MainPage.Myself.Balance >= Price) && !IsBought)
            {
                MainPage.Myself.Balance -= Price;
                IsBought = true;

                MainPage.Myself.RequestBought(this);
            }
            else if (IsBought)
            {
                await new MessageDialog("Sorry, but this item has alerady been bought").ShowAsync();
            }
        }

        public async Task<ContentDialogResult> ShowBuyConfirmation()
        {
            var d = new ContentDialog
            {
                PrimaryButtonText = CanAfford ? "Yes !" : "Not enough money",
                SecondaryButtonText = "No!",
                Title = "Buy confirmation",
                IsPrimaryButtonEnabled = CanAfford,
                Content = new Grid
                {
                    Children =
                    {
                        new TextBlock
                        {
                            Text = $"Are you sure you want to buy {Name} for {Price}$ ?"
                        }
                    }
                }
            };
            var results = await d.ShowAsync();
            if (results == ContentDialogResult.Primary)
                Buy();
            return results;
        }
    }
}