using BombDiscovery.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;  
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BombDiscovery
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewGame : Page
    {


        public int raws = 10;
        public int columns = 10;
        public int nbomb = 10;
        public int bheight;
        public int bwidth;
        public Button playagain;
        public TextBlock text;
        public TextBlock timer;
        int timeSeconds = 0;
        int timeminutes = 0;

        Control control;
        public Button menu;
        public Button[][] buttons;
        DispatcherTimer dispatch = new DispatcherTimer();

        Grid Grid; 

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public NewGame()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            readConfiguration();
            Grid = grid;
            buttons = new Button[raws][];
            control = new Control(this);

            for (int i = 0; i < buttons.Length; i++)
                buttons[i] = new Button[columns];
            addbuttons();
        }
        void readConfiguration()
        {

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            raws = (int)localSettings.Values["raws"];
            columns = (int)localSettings.Values["columns"];
            nbomb = raws;



        }
        void addbuttons()
        {

            int bheight = ((int)Grid.Height) / ( 2*raws);
            int bwidth = ((int)Grid.Width) / (2*columns );
            int x = (int)Grid.Margin.Left, y = (int)Grid.Margin.Top+50;

            for (int i = 0; i < raws; i++)
            {
                x = 0;
                for (int j = 0; j < columns; j++)
                {
                    buttons[i][j] = new Button();

                    buttons[i][j].Margin = new Thickness(x, y, 0, 0);
                    x += 2*bwidth;
                    if (raws == 10)
                        buttons[i][j].FontSize = 2 * raws;
                    else if (raws == 15)
                        buttons[i][j].FontSize = 3 * raws / 2;
                    else
                        buttons[i][j].FontSize = 10;


                    buttons[i][j].Height = bheight;
                    buttons[i][j].Width = bwidth;
                    // buttons[i][j].Background = "Assets/Logo.scale-100.png";


                    // buttons[i][j].Click += new RoutedEventHandler(this.button_Click);


                    buttons[i][j].Click += (Sender, e) =>
                    {
                        for (int k = 0; k < control.Simulation.Places.Length; k++)
                            for (int m = 0; m < control.Simulation.Places[k].Length; m++)
                            {
                                if (Sender == buttons[k][m])
                                {
                                    control.expand(k, m);
                                    break;
                                }
                            }
                    };

                    buttons[i][j].Background = new SolidColorBrush(Windows.UI.Colors.Aqua);
                    Grid.Children.Add(buttons[i][j]);

                }
                y += 2 * bheight;

            }
            timer = new TextBlock();
            timer.Text = timeminutes + " : " + timeSeconds;
            timer.Margin = new Thickness(0, -100,  bwidth,  bheight);
            
            timer.FontSize = 10;
        //text.Grid.Children.Add(timer);
         

            text = new TextBlock();
            text.Text = "You Lose";
            text.FontSize = 40;

            text.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            text.Margin = new Thickness(Grid.Margin.Left ,  Grid.Margin.Top, 10, 10);



            playagain = new Button();
            playagain.Margin = new Thickness(100, -100, 2 * bwidth, 2 * bheight);
            playagain.Click += (Sender, e) =>
            {
                this.Visibility = Visibility.Collapsed;

                this.Frame.Navigate(typeof(NewGame));

            };

            playagain.Content = "Play Again";
            menu = new Button();
            menu.Margin = new Thickness(220, -100, 2 * bwidth, 2 * bheight);
            menu.Click += (Sender, e) =>
            {

                this.Visibility = Visibility.Collapsed;

                this.Frame.Navigate(typeof(MainPage));

            };
            menu.Content = " Menu ";

        }

        public void uncoverButton(int index1, int index2)
        {

            control.nvisited++;

            if (control.gamefinish == false)
            {
                if (control.nvisited == raws * columns - nbomb)
                {
                    dispatch.Stop();

                    control.gamefinish = true;
                    text.Text = " Congratulation You Won ";
                    Grid.Children.Add(playagain);
                    Grid.Children.Add(menu);
                    Grid.Children.Add(text);
                    text.Visibility = Visibility.Visible;

                }

                Button n = new Button();
                n = buttons[index1][index2];
                if (control.Simulation.Places[index1][index2] == -1)
                {   // this.Frame.Navigate(typeof(MainPage));
                    control.gamefinish = true;
                    Grid.Children.Add(text);
                    Grid.Children.Add(playagain);
                    Grid.Children.Add(menu);
                    text.Visibility = Visibility.Visible;

                }

                else if (control.Simulation.Places[index1][index2] != 0)
                    n.Content = control.Simulation.Places[index1][index2];
                buttons[index1][index2].Background = new SolidColorBrush(Windows.UI.Colors.Red);
                n.IsEnabled = false;
            }

        }
        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }
        private void onload(object sender, RoutedEventArgs e)
        {

            if (!dispatch.IsEnabled)
            {
                if (control.gamefinish == false)
                    dispatch.Tick += dispatch_Tick;
                else dispatch.Stop();

                dispatch.Interval = new TimeSpan(0, 0, 1);
                dispatch.Start();

            }
        }


        void dispatch_Tick(object Sender, object e)
        {

            timer.Text = timeminutes + " : " + timeSeconds;
            if (timeSeconds == 59)
            {
                timeSeconds = 0;
                timeminutes++;
            }
            else
                timeSeconds++;

        }
        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
