using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfMediaDemo
{
    /// <summary>
    /// Interaction logic for FullScreen.xaml
    /// </summary>
    public partial class FullScreen : Window
    {
        private bool isFullScreen;
        private DateTime lastClickTime = DateTime.MinValue;   //for check double click
        public string currentFilename;
        private TimeSpan playbackPosition;


        public FullScreen()
        {
            InitializeComponent();
                    }

        private void me_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            DateTime clickTime = DateTime.Now;
            TimeSpan elapsedTime = clickTime - lastClickTime;
            lastClickTime = clickTime;

            if (elapsedTime.TotalMilliseconds < 500) // Check if the elapsed time is less than 500 milliseconds (indicating a double-click)
            {

                ToggleFullScreen();
                //ToggleFullScreenWithPlaybackPosition();
            }
            


            
        }
        private void ToggleFullScreen()

        {
            
           
            if (!isFullScreen)
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                ResizeMode = ResizeMode.NoResize;
                Topmost = true;
                FullScreenMediaPlayer.Focus();
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.UpdateCurrentFilename(currentFilename);


                playbackPosition = FullScreenMediaPlayer.Position;

                // Stop the media playback
                FullScreenMediaPlayer.Source = null;
                FullScreenMediaPlayer.LoadedBehavior = MediaState.Manual;
                FullScreenMediaPlayer.Position = TimeSpan.Zero;
                FullScreenMediaPlayer.Stop();
                FullScreenMediaPlayer.Close();
                this.Hide();

                mainWindow.Show();
                mainWindow.playCurrentMedia(currentFilename, playbackPosition);


               /* mainWindow.WindowStyle = WindowStyle.None;
                mainWindow.WindowState = WindowState.Maximized;
                mainWindow.ResizeMode = ResizeMode.NoResize;
                mainWindow.Topmost = true;
                mainWindow.Focus();*/

            

            /*WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Normal;
            ResizeMode = ResizeMode.CanResize;
            Topmost = false;*/
        }

            isFullScreen = !isFullScreen;
        }

        
        public void playCurrentMedia(string filename, TimeSpan position)
        {
            currentFilename = filename;
            playbackPosition = position; // Store the playback position
            currentFilename = filename;
            if (filename != "")
            {
                


                // now write code for the media play 
                Uri u = new Uri(filename);
                // set this URI object to Media Element
                FullScreenMediaPlayer.Source = u;
                // set the volume (optional)
                FullScreenMediaPlayer.Volume = 100.5;
                
                // Attach a handler for the MediaOpened event to seek to the stored playback position
                FullScreenMediaPlayer.MediaOpened += (sender, e) =>
                {
                    // Check if the stored playback position is within the video's duration
                    if (playbackPosition < FullScreenMediaPlayer.NaturalDuration.TimeSpan)
                    {
                        // Seek to the stored playback position
                        FullScreenMediaPlayer.Position = playbackPosition;
                    }
                };

                // Start the video using the LoadedBehavior property
                MediaState opt = MediaState.Play;
                FullScreenMediaPlayer.LoadedBehavior = opt;



            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No Selection", "Empty");
            }

        }




    }
}
