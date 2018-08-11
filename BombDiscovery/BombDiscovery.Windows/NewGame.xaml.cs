using BombDiscovery.Common;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using System.Threading;
using Windows.Media.MediaProperties;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BombDiscovery
{
    /// <summary>
    /// A basic page that provides characteri stics common to most applications.
    /// </summary>
    public sealed partial class NewGame : Page
    {
        //TimerCallback t = new TimerCallback((object)thread);
       // Timer timer = new Timer(t,null,1000,1000);
        // new System.Threading.TimerCallback(TimerTask);

     public   int raws = 10;
       public  int columns = 10;
      public  int nbomb=10;
        public int bheight;
       public int bwidth;
       public Button playagain;
       public TextBlock text;
       public TextBlock timer;
       int timeSeconds = 0;
       int timeminutes = 0;

        Control control;
        public Button menu;
        public Button ShareFace;

        public Button[][] buttons;
        DispatcherTimer dispatch = new DispatcherTimer();
        
        Grid Grid; 
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        
        
        
        
        public void thread(object t)
        { 
        
        
        }
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        void readConfiguration()
        {

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;   
           
         raws=(int)localSettings.Values["raws"];
         columns = (int)localSettings.Values["columns"];
            if(raws==10)
            nbomb = 10;
            else if (raws == 15)
                nbomb = 20;
            else if (raws == 20)
                nbomb = 30;





        }

       
       
        public NewGame()
       {
           
           
                this.InitializeComponent();
            
            readConfiguration();
                //ma();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            Grid = grid;
            buttons = new Button[raws][];
           control = new Control(this);
           ShareFace = new Button();

            for (int i = 0; i < buttons.Length; i++)
                buttons[i] = new Button[columns];
            addbuttons();

        }


        

        async void ma()
        {
                           await new Windows.UI.Popups.MessageDialog("").ShowAsync();

        }
        void button_Click(object sender, EventArgs e)
        {


        }
        void addbuttons()
        {

            int bheight = (int)Grid.Height / (2*raws  );
            int bwidth = ((int)Grid.Width-200) / (5*columns /4);
            int x = (int)Grid.Margin.Left, y = (int)Grid.Margin.Top;
            
            for (int i = 0; i < raws; i++)
            {
                x = 0;
                for (int j = 0; j < columns; j++)
                {
                    buttons[i][j] = new Button();

                    buttons[i][j].Margin = new Thickness(x, y, 0, 0);
                    x += bwidth;
                    if(raws==10)
                        buttons[i][j].FontSize = 2 * raws;
                    else if (raws==15)
                        buttons[i][j].FontSize = 3 * raws/2;
                    else
                        buttons[i][j].FontSize = 10;
                  

                    buttons[i][j].Height = bheight;
                    buttons[i][j].Width = bwidth;
                   // buttons[i][j].Background = "Assets/Logo.scale-100.png";

                   
                    // buttons[i][j].Click += new RoutedEventHandler(this.button_Click);
                    
                   
                    buttons[i][j].Click += (Sender, e) =>
                    {
                    for(int k=0;k<control.Simulation.Places.Length;k++)
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
            timer.Text = timeminutes+" : "+timeSeconds;
            timer.Margin = new Thickness(4 * Grid.Width / 5 - 50, 1 * Grid.Height / 5+50, 150, 150);

            timer.FontSize = 50;
          Grid.Children.Add(timer);


             text = new TextBlock();
            text.Text = "You Lose";
            text.FontSize = 20;

           text.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
           text.Margin = new Thickness(4* Grid.Width / 5-100, 2 * Grid.Height / 5, 150, 150);

            

            playagain = new Button();
            ShareFace = new Button();
            ShareFace.Margin = new Thickness(1200, 400, 2 * bwidth, 2 * bheight);
            ShareFace.Click += (Sender, e) =>
            {
                DataTransferManager.ShowShareUI();
                RegisterForShareImage();
            };
            ShareFace.Content = "Share On FaceBook ";
             
            playagain.Margin = new Thickness(1200, 100, 2*bwidth, 2*bheight);
            playagain.Click += (Sender, e) =>
            {
                this.Visibility = Visibility.Collapsed;
                this.Frame.Navigate(typeof(MainPage));

                this.Frame.Navigate(typeof(NewGame));

            };
            
            playagain.Content = "Play Again";
             menu = new Button();
            menu.Margin = new Thickness(1200, 300, 2*bwidth, 2*bheight);
            menu.Click += (Sender, e) =>
            {

                this.Visibility = Visibility.Collapsed;

                this.Frame.Navigate(typeof(MainPage));

            };
            menu.Content = " Menu ";
        
        }

      public  void uncoverButton(int index1, int index2)
        {
            MediaElement m = new MediaElement();
            

            control.nvisited++;
         
            if (control.gamefinish == false)
            {
                if (control.nvisited == raws * columns - nbomb)
                {
                    dispatch.Stop();

                    control.gamefinish = true;
                    text.Text = " Congratulation ";
                    Grid.Children.Add(playagain);
                    Grid.Children.Add(menu);
                 
                      Grid.Children.Add(text);
                      Grid.Children.Add(ShareFace);
                    text.Visibility = Visibility.Visible;

 
                }

                Button n = new Button();
                n = buttons[index1][index2];
                if (control.Simulation.Places[index1][index2] == -1)
                {   // this.Frame.Navigate(typeof(MainPage));
                    dispatch.Stop();
                    control.gamefinish = true;
                    Grid.Children.Add(text);
                    Grid.Children.Add(playagain);

                    Grid.Children.Add(ShareFace);
                    Grid.Children.Add(menu);
                   text.Visibility = Visibility.Visible;
  
                for(int i=0;i<buttons.Length;i++)
                    for (int j = 0; j < buttons[i].Length; j++)
                    {
                        if (control.Simulation.Places[i][j] == -1)
                        {
                            buttons[i][j].Background = new SolidColorBrush(Windows.UI.Colors.Red);
                            buttons[i][j].Content = "Bomb";
                        }

                    
                    }

                }
                
                else if (control.Simulation.Places[index1][index2] != 0)
                    n.Content = control.Simulation.Places[index1][index2];
                buttons[index1][index2].Background = new SolidColorBrush(Windows.UI.Colors.Red);
                
                    n.IsEnabled = false;
            }
         
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
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
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            

            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            {       timeSeconds = 0;
            timeminutes++;
            }
            else
            timeSeconds++;

        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {

        }




        private void RegisterForShareImage()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += ShareImageHandler;
        }

        private async void ShareImageHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = " Bomb Finder ";
            request.Data.Properties.Description = "Winning a game ";
            request.Data.SetText("winning by level "+timer.Text);
            DataRequestDeferral deferral = request.GetDeferral();
            try
            {
                StorageFile thumbnailFile = await Package.Current.InstalledLocation.GetFileAsync("Assets\\SmallLogo.scale-100.png");
                request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromFile(thumbnailFile);
                StorageFile imageFile = await Package.Current.InstalledLocation.GetFileAsync("Assets\\SmallLogo.scale-100.png");
                request.Data.SetBitmap(RandomAccessStreamReference.CreateFromFile(imageFile));
            }
            finally
            {
                deferral.Complete();
            }
        }


    }
}
