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
using System.Runtime.InteropServices.ComTypes;

namespace WpfMediaDemo
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        //Full screen
        private bool isFullScreen;
        private TimeSpan playbackPosition;

        private string currentFilename;

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
            me.LoadedBehavior = MediaState.Close;


            // Show the logo image and hide the MediaElement
            logoImage.Visibility = Visibility.Visible;
            me.Visibility = Visibility.Collapsed;

            ChangeIcon("play.png");


        }


        // Broswe Button Code
        private void b5_Click(object sender, RoutedEventArgs e)
        {

            playMedia(openMediaFile());
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

        private string openMediaFile()
        {
            try
            {
                OpenFileDialog fd = new OpenFileDialog();
                fd.Filter = "MP4 File (*.mp4)|*.mp4|3GP File (*.3gp)|*.3gp|Audio File (*.wma)|*.wma|MOV File (*.mov)|*.mov|AVI File (*.avi)|*.avi|Flash Video(*.flv)|*.flv|Video File (*.wmv)|*.wmv|MPEG-2 File (*.mpeg)|*.mpeg|WebM Video (*.webm)|*.webm|All files (*.*)|*.*";
                fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                fd.ShowDialog();

                // Check if a file was selected
                if (!string.IsNullOrEmpty(fd.FileName))
                {
                    currentFilename = fd.FileName;
                    return currentFilename;
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
            return null;
        }


        public void playMedia(string filename)
        {
            currentFilename = filename;
            if (!string.IsNullOrEmpty(currentFilename))
            {
                // Set the visibility of the MediaElement to Visible
                me.Visibility = Visibility.Visible;

                // Hide the logo image
                logoImage.Visibility = Visibility.Collapsed;

                // now write code for the media play 
                Uri u = new Uri(filename);
                // set this URI object to Media Element
                me.Source = u;
                // set the volume (optional)
                me.Volume = 100.5;
                // start the video using LoadedBehiour Property
                MediaState opt = MediaState.Play;
                me.LoadedBehavior = opt;

                // Set Play/Pause button status to default value
                isPlaying = true;
                isStarted = true;
                ChangeIcon("pause.png");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No Selection", "Empty");
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
        private void ChangeIcon(string icon)
        {
            // Load the new icon image using the provided icon filename
            Uri newIconUri = new Uri($"Resources\\{icon}", UriKind.Relative);
            BitmapImage newIconImage = new BitmapImage(newIconUri);

            // Set the new icon image as the source for the Image element
            btnImage.Source = newIconImage;
        }

        private void PauseButton()
        {
            // toggle between play and pause based on the current state
            if (isPlaying)
            {
                MediaState uc = MediaState.Pause;
                me.LoadedBehavior = uc;
                isPlaying = false;
                ChangeIcon("play.png");

            }
            else
            {
                MediaState ms = MediaState.Play;
                // start the video using LoadedBehiour
                me.LoadedBehavior = ms;
                isPlaying = true;
                ChangeIcon("pause.png");

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


            lblVolumePrecentage.Content = ((int)adjustedVolume).ToString() + "%";   //show current volume as a precentage
        }

        //Full screen
        private void me_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            DateTime clickTime = DateTime.Now;
            TimeSpan elapsedTime = clickTime - lastClickTime;
            lastClickTime = clickTime;

            if (elapsedTime.TotalMilliseconds < 500) // Check if the elapsed time is less than 500 milliseconds (indicating a double-click)
            {

                ToggleFullScreen();



            }
        }

        private void ToggleFullScreen()
        {
            var fullScreen = new FullScreen();


            if (WindowState == WindowState.Maximized)
            {

            }
            else
            {

                fullScreen.playCurrentMedia(currentFilename, me.Position);

                // Stop the media playback
                me.Source = null;
                me.LoadedBehavior = MediaState.Manual;
                me.Position = TimeSpan.Zero;
                me.Stop();
                me.Close();
                this.Hide();

                fullScreen.Show();


                fullScreen.WindowStyle = WindowStyle.None;
                fullScreen.WindowState = WindowState.Maximized;
                fullScreen.ResizeMode = ResizeMode.NoResize;
                fullScreen.Topmost = true;
                fullScreen.FullScreenMediaPlayer.Focus();
            }
        }

        public void UpdateCurrentFilename(string filename)
        {
            currentFilename = filename;
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
                me.Source = u;
                // set the volume (optional)
                me.Volume = 100.5;

                // Attach a handler for the MediaOpened event to seek to the stored playback position
                me.MediaOpened += (sender, e) =>
                {
                    // Check if the stored playback position is within the video's duration
                    if (playbackPosition < me.NaturalDuration.TimeSpan)
                    {
                        // Seek to the stored playback position
                        me.Position = playbackPosition;
                    }
                };

                // Start the video using the LoadedBehavior property
                MediaState opt = MediaState.Play;
                me.LoadedBehavior = opt;

                isStarted = true;
                ChangeIcon("pause.png");




            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No Selection", "Empty");
            }

        }


        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void me_Drop(object sender, System.Windows.DragEventArgs e)
        {
            MessageBox.Show("Drag");
        }

        private void me_DragOver(object sender, System.Windows.DragEventArgs e)
        {
            MessageBox.Show("Drag");
        }
    }
}
