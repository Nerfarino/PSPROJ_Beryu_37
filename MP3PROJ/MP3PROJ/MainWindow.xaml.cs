using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MP3PROJ
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private MediaPlayer mediaPlayer = new MediaPlayer();
        private bool userIsDraggingSlider = false;
        private double volume = 0;
        private void btnOpenAudioFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                mediaPlayer.Open(new Uri(openFileDialog.FileName));
                lblStatus.Content = openFileDialog.SafeFileName;
                lblStatus1.Content = "Loaded";
            }

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();


            volume = mediaPlayer.Volume * 100;
            lblVolume.Content = volume;

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mediaPlayer.Source != null) && (mediaPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                sliProgress.Value = mediaPlayer.Position.TotalSeconds;
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.HasAudio == false)
            {
                MessageBox.Show("No audio file loaded!");
            }
            else
            {
                mediaPlayer.Play();
                lblStatus1.Content = "Playing";
            }
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.HasAudio == false)
            {
                MessageBox.Show("No audio file loaded!");
            }
            else
            {
                mediaPlayer.Pause();
                lblStatus1.Content = "Paused";
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.HasAudio == false)
            {
                MessageBox.Show("No audio file loaded!");
            }
            else
            {
                mediaPlayer.Stop();
                lblStatus1.Content = "Stopped";
            }
        }
        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (mediaPlayer.HasAudio == false)
            {
                MessageBox.Show("No audio file loaded!");
            }
            else
                userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (mediaPlayer.HasAudio == false)
            {
                MessageBox.Show("No audio file loaded!");
            }
            else
            {
                userIsDraggingSlider = false;
                mediaPlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
            }
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaPlayer.HasAudio == false)
            {
                MessageBox.Show("No audio file loaded!");
            }
            else
                lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void btnVolumeUp_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Volume += 0.1;
            volume = mediaPlayer.Volume * 100;
            lblVolume.Content = volume.ToString();
            if (mediaPlayer.Volume == 0)
            {
                btnMute.Background = Brushes.Red;
                btnMute.Content = "Muted";
            }
            else
            {
                btnMute.Background = Brushes.Green;
                btnMute.Content = "Mute";
            }
        }

        private void btnVolumeDown_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Volume -= 0.05;
            volume = mediaPlayer.Volume * 100;
            lblVolume.Content = volume.ToString();
            if (mediaPlayer.Volume == 0)
            {
                btnMute.Background = Brushes.Red;
                btnMute.Content = "Muted";
            }
            else
            {
                btnMute.Background = Brushes.Green;
                btnMute.Content = "Mute";
            }
        }

        private void btnMute_Click(object sender, RoutedEventArgs e)
        {


            if (mediaPlayer.Volume == 0)
            {
                btnMute.Background = Brushes.Green;
                mediaPlayer.Volume = 0.5;
                btnMute.Content = "Mute";
            }
            else
            {
                mediaPlayer.Volume = 0;
                btnMute.Background = Brushes.Red;
                btnMute.Content = "Muted";
            }
            volume = mediaPlayer.Volume * 100;
            lblVolume.Content = volume.ToString();
        }
    }
}
