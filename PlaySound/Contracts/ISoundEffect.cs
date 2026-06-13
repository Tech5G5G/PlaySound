using System.ComponentModel;

namespace PlaySound.Contracts;

public interface ISoundEffect : INotifyPropertyChanged, IDisposable
{
    string Name { get; set; }

    double Volume { get; set; }

    TimeSpan Position { get; set; }
    TimeSpan Duration { get; }

    bool Playing { get; }
    bool Loop { get; set; }

    string FileName { get; }

    void Play();
    void Pause();
    void Restart();
}