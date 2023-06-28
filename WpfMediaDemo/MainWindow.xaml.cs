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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Window = System.Windows.Window;
using System.IO.Compression;
using System.IO;
using System.Diagnostics;
using System.Windows.Input;

using System.Runtime.InteropServices;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using KeyEventHandler = System.Windows.Forms.KeyEventHandler;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WpfMediaDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    

    public partial class MainWindow : Window
    {
       
        private bool isPlaying = false;
        private bool isStarted = false;

        private DateTime lastClickTime = DateTime.MinValue;   //for check double click

        public MainWindow()
        {
            InitializeComponent();

            me.LoadedBehavior = MediaState.Manual;
            me.Volume = VolumeSlider.Value / 100.0; // Set initial volume based on the slider value
            
        }
       

        // Button Click Event Handler
        private void b1_Click(object sender, RoutedEventArgs e)
        {

            if (!isStarted) // check if a media file has been selected
            {
                System.Windows.Forms.MessageBox.Show("No media file selected", "Warning");
            }
            else
            {
                PauseButton();
            }
        }

        // Stop Button Code
        private void b3_Click(object sender, RoutedEventArgs e)
        {
            // stop the running media element using same LoadedBehiour property
            me.LoadedBehavior = MediaState.Stop;
            me.LoadedBehavior =MediaState.Close;

            // Show the logo image and hide the MediaElement
            logoImage.Visibility = Visibility.Visible;
            me.Visibility = Visibility.Collapsed;

            b1.Content = "Play";
            
        }
       


        // Broswe Button Code
        private void b5_Click(object sender, RoutedEventArgs e)
        {

            // this will play any video / audio files dynamically
            // use openfile file dialog. WPF does not support openfile dialog. So we should add the WinForms reference
            // here. I ll show now. watch plz.
            // create open file dialog
            try
            {
                OpenFileDialog fd = new OpenFileDialog();
                // set the filters
                fd.Filter = "MP4 File (*.mp4)|*.mp4|3GP File (*.3gp)|*.3gp|Audio File (*.wma)|*.wma|MOV File (*.mov)|*.mov|AVI File (*.avi)|*.avi|Flash Video(*.flv)|*.flv|Video File (*.wmv)|*.wmv|MPEG-2 File (*.mpeg)|*.mpeg|WebM Video (*.webm)|*.webm|All files (*.*)|*.*";
                // set the initial directory optional
                fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                // display the dialog
                fd.ShowDialog();
                // get the currently selected video / audio file path and store it in string variable
                string filename = fd.FileName;

                if (filename != "")
                {
                    // display this path of selected video / audio path to the text box called "tb"
                    tb.Text = filename;


                    // now write code for the media play 
                    Uri u = new Uri(filename);
                    // set this URI object to Media Element
                    me.Source = u;
                    // set the volume (optional)
                    me.Volume = 100.5;
                    // start the video using LoadedBehiour Property
                    MediaState opt = MediaState.Play;
                    me.LoadedBehavior = opt;

                    //Set Play/Pause button status to default value
                    isPlaying = true;
                    isStarted = true;
                    b1.Content = "Pause";

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("No Selection", "Empty");
                }
            }
            catch (Exception e1)
            {
                System.Console.WriteLine("Error Text: " + e1.Message);
            }
        }

        // window_loaded event: called automatically when you run application first
        // this will be useful for preprocessing part. here video will be automatically played when run the application
        // thats y i created this event instead of button event
        string videoURL = @"C:\Users\morning1.mp4";
        private void window_loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // create URI object because MediaElement does not directly support the Video URL
                Uri obj = new Uri(videoURL);
                // set the local video URL via URI then attach this Uri object to Media Element using its
                // Source property
                me.Source = obj;
                // now url is successfully set to MediaElement. Next step is to play the Media Element
                // this is done by MediaState built-in class
                MediaState opt = MediaState.Play;
                // now attach this play state to MediaElement using its important property LoadedBehaviour
                me.LoadedBehavior = opt;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error Message: " + ex.Message);
            }
        }

        //Backward Button Code
        private void b6_Click(object sender, RoutedEventArgs e)
        {
            // Check if a media file has been selected
            if (!isStarted)
            {
                System.Windows.Forms.MessageBox.Show("No media file selected", "Warning");
            }
            else
            {
                SeekBackward();
            }
        }

        //Forward Buttion Code
        private void b7_Click(object sender, RoutedEventArgs e)
        {
            // Check if a media file has been selected
            if (!isStarted)
            {
                System.Windows.Forms.MessageBox.Show("No media file selected", "Warning");
            }
            else
            {
                SeekForward();
            }
        }

        private void PauseButton()
        {
            // toggle between play and pause based on the current state
            if (isPlaying)
            {
                MediaState uc = MediaState.Pause;
                me.LoadedBehavior = uc;
                isPlaying = false;
                b1.Content = "Play";
            }
            else
            {
                MediaState ms = MediaState.Play;
                // start the video using LoadedBehiour
                me.LoadedBehavior = ms;
                isPlaying = true;
                b1.Content = "Pause";
            }
        }
        
        // Seek backward
        private void SeekBackward()
        {
            TimeSpan currentPosition = me.Position;
            TimeSpan backwardPosition = currentPosition.Subtract(TimeSpan.FromSeconds(10)); // 1 minute backward
            if (backwardPosition < TimeSpan.Zero)
            {
                backwardPosition = TimeSpan.Zero;
            }
            me.Position = backwardPosition;
        }

        // Seek forward
        private void SeekForward()
        {
            TimeSpan currentPosition = me.Position;
            TimeSpan forwardPosition = currentPosition.Add(TimeSpan.FromSeconds(10)); // 1 minute forward
            if (forwardPosition > me.NaturalDuration.TimeSpan)
            {
                forwardPosition = me.NaturalDuration.TimeSpan;
            }
            me.Position = forwardPosition;
        }

        private void keyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }
        // Handle key presses for arrow keys
        private void keyPress(object sender, System.Windows.Input.KeyEventArgs e)

        {
            if (e.Key == Key.Left)

            {
                SeekBackward();
            }
            else if (e.Key == Key.Right)
            {
                SeekForward();
            }
            else if (e.Key == Key.P)
            {
                PauseButton();
                
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            double volume = VolumeSlider.Value;
            double volumePercentage = volume / VolumeSlider.Maximum; // Calculate the volume percentage

            double adjustedVolume = volumePercentage * 100.0; // Scale the volume to a range of 0 to 100

            me.Volume = adjustedVolume / 100.0; // Scale the adjusted volume to a range of 0.0 to 1.0
           

            lblVolumePrecentage.Content = ((int) adjustedVolume).ToString() +"%";   //show current volume as a precentage
        }

        //Full screen
        private void me_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DateTime clickTime = DateTime.Now;
            TimeSpan elapsedTime = clickTime - lastClickTime;
            lastClickTime = clickTime;

            if (elapsedTime.TotalMilliseconds < 300) // Check if the elapsed time is less than 300 milliseconds (indicating a double-click)
            {
                ToggleFullScreen();
            }
        }

        private void ToggleFullScreen()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.SingleBorderWindow;
            }
            else
            {
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
            }
        }

    }
}
