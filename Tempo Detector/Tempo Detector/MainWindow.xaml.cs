using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;


namespace Tempo_Detector
{

    public partial class MainWindow : Window
    {
        private Stopwatch stopwatch = new Stopwatch();
        private List<Double> tapIntervals = new List<double>();
        private DispatcherTimer metronomeTimer;
        private SoundPlayer metronomeSoundPlayer;
        public MainWindow()
        {
            InitializeComponent();
            stopwatch.Start();

            metronomeTimer = new DispatcherTimer();
            metronomeTimer.Tick += MetronomeTick;
            //add sound file
            try
            {
                metronomeSoundPlayer = new SoundPlayer("click.wav");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading metronome sound: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

            int startIndex = Math.Max(0, tapIntervals.Count - 8);
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
            {
                if (averageIntervalMs > 0)
                {
                    metronomeTimer.Interval = TimeSpan.FromMilliseconds(averageIntervalMs);
                }
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
            metronomeSoundPlayer?.Play();
        }
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //temp empty
        }
    }
}
