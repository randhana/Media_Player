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
        string currentFilename;

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
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
                ResizeMode = ResizeMode.CanResize;
                Topmost = false;
            }

            isFullScreen = !isFullScreen;
        }

        public void playCurrentMedia(string filename)
        {
            currentFilename = filename;
            if (filename != "")
            {
                


                // now write code for the media play 
                Uri u = new Uri(filename);
                // set this URI object to Media Element
                FullScreenMediaPlayer.Source = u;
                // set the volume (optional)
                FullScreenMediaPlayer.Volume = 100.5;
                // start the video using LoadedBehiour Property
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
