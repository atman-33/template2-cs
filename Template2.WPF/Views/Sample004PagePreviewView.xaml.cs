using System;
using System.Numerics;
using System.Windows.Controls;
using Template2.WPF.Services;

namespace Template2.WPF.Views
{
    /// <summary>
    /// Interaction logic for Sample004PagePreviewView
    /// </summary>
    public partial class Sample004PagePreviewView : UserControl, IMediaService
    {
        public Sample004PagePreviewView()
        {
            InitializeComponent();
        }

        void IMediaService.FastForward()
        {
            this.MediaPlayer.Position += TimeSpan.FromSeconds(10);
        }

        void IMediaService.Pause()
        {
            this.MediaPlayer.Pause();
        }

        void IMediaService.Play()
        {
            this.MediaPlayer.Position = TimeSpan.Zero;
            this.MediaPlayer.Play();
        }

        void IMediaService.Rewind()
        {
            this.MediaPlayer.Position -= TimeSpan.FromSeconds(10);
        }

        void IMediaService.Stop()
        {
            this.MediaPlayer.Stop();
        }

        bool IMediaService.IsPlaying()
        {
            return this.MediaPlayer.Position < this.MediaPlayer.NaturalDuration;
        }
    }
}
