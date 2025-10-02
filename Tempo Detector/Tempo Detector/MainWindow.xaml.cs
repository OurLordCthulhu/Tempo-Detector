using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Tempo_Detector
{
    public partial class MainWindow : Window
    {
        private Stopwatch stopwatch = new Stopwatch();
        private List<double> tapIntervals = new List<double>();
        private DispatcherTimer metronomeTimer;

        // NAudio specific variables for audio playback
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        public MainWindow()
        {
            InitializeComponent();
            stopwatch.Start();

            metronomeTimer = new DispatcherTimer();
            metronomeTimer.Tick += MetronomeTick;

            // Initialize NAudio and load the sound file
            try
            {
                audioFile = new AudioFileReader("click.wav");
                outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading audio file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Hook up the button and slider events
            PlayPauseButton.Click += PlayPauseButton_Click;
            VolumeSlider.ValueChanged += VolumeSlider_ValueChanged;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                ProcessTap();
            }
        }

        private void TapButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessTap();
        }

        private void ProcessTap()
        {
            double intervalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Restart();
            tapIntervals.Add(intervalMilliseconds);
            if (tapIntervals.Count >= 2)
            {
                CalculateBpm();
            }
        }

        private void CalculateBpm()
        {
            double sumOfIntervals = 0;
            int count = 0;

            int startIndex = Math.Max(0, tapIntervals.Count - 4);
            for (int i = startIndex; i < tapIntervals.Count; i++)
            {
                sumOfIntervals += tapIntervals[i];
                count++;
            }

            if (count > 0)
            {
                double averageIntervalMs = sumOfIntervals / count;
                if (averageIntervalMs > 0)
                {
                    double bpm = 60000 / averageIntervalMs;
                    BpmTextBlock.Text = $"BPM: {bpm:F2}";
                    UpdateMetronomeInterval(averageIntervalMs);
                }
            }
        }

        private void UpdateMetronomeInterval(double averageIntervalMs)
        {
            if (averageIntervalMs > 0)
            {
                metronomeTimer.Interval = TimeSpan.FromMilliseconds(averageIntervalMs);
            }
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (metronomeTimer.IsEnabled)
            {
                metronomeTimer.Stop();
                PlayPauseButton.Content = "Play";
            }
            else
            {
                if (tapIntervals.Count >= 2)
                {
                    CalculateBpm();
                    metronomeTimer.Start();
                    PlayPauseButton.Content = "Pause";
                }
            }
        }

        private void MetronomeTick(object sender, EventArgs e)
        {
            if (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                outputDevice.Stop();
            }
            audioFile.Position = 0; // Rewind the audio file to the beginning
            outputDevice.Play();
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (outputDevice != null)
            {
                outputDevice.Volume = (float)VolumeSlider.Value;
            }
        }
    }
}