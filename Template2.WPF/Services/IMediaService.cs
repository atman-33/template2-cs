
namespace Template2.WPF.Services
{
    public interface IMediaService
    {
        void Play();
        void Pause();
        void Stop();
        void Rewind();
        void FastForward();
        bool IsPlaying();
    }
}
