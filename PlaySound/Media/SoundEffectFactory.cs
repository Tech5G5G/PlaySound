using CommunityToolkit.Mvvm.ComponentModel;
using PlaySound.Contracts;
using Windows.Media.Playback;

namespace PlaySound.Media;

public static partial class SoundEffectFactory
{
    public static ISoundEffect CreateSoundEffect(string fileName, SynchronizationContext context = null)
    {
        context ??= SynchronizationContext.Current;

        MediaPlayer player = new()
        {
            Volume = 1.0,
            AudioCategory = MediaPlayerAudioCategory.SoundEffects
        };
        player.SetUriSource(new(fileName));

        return new SoundEffect(fileName, context, player);
    }

    public static ISoundEffect ReadEffectFromReader(BinaryReader reader)
    {
        var effect = CreateSoundEffect(fileName: reader.ReadString());
        effect.Name = reader.ReadString();
        effect.Volume = reader.ReadDouble();
        effect.Loop = reader.ReadBoolean();

        return effect;
    }

    public static void WriteEffectToWriter(ISoundEffect effect, BinaryWriter writer)
    {
        writer.Write(effect.FileName);
        writer.Write(effect.Name);
        writer.Write(effect.Volume);
        writer.Write(effect.Loop);
    }

    private sealed partial class SoundEffect : ObservableObject, ISoundEffect
    {
        [ObservableProperty]
        public partial string Name { get; set; }

        [ObservableProperty]
        public partial double Volume { get; set; } = 1.0;

        [ObservableProperty]
        public partial TimeSpan Position { get; set; }

        [ObservableProperty]
        public partial TimeSpan Duration { get; private set; }

        [ObservableProperty]
        public partial bool Playing { get; set; }

        [ObservableProperty]
        public partial bool Loop { get; set; }

        public string FileName { get; }

        private readonly SynchronizationContext _context;

        private MediaPlayer _player;

        public SoundEffect(string fileName, SynchronizationContext context, MediaPlayer player)
        {
            FileName = fileName;
            _context = context;
            _player = player;

            _player.VolumeChanged += OnVolumeChanged;
            _player.PlaybackSession.PositionChanged += OnPositionChanged;
            _player.PlaybackSession.NaturalDurationChanged += OnNaturalDurationChanged;
            _player.PlaybackSession.PlaybackStateChanged += OnPlaybackStateChanged;
        }

        public void Play()
        {
            Playing = true;
        }

        public void Pause()
        {
            Playing = false;
        }

        public void Restart()
        {
            Position = TimeSpan.Zero;
        }

        #region Property Notifications

        partial void OnVolumeChanged(double value)
        {
            if (!IsSyncing)
            {
                _player.Volume = Math.Clamp(value, 0, 1);
            }
        }

        partial void OnPositionChanged(TimeSpan value)
        {
            if (!IsSyncing && _player.PlaybackSession is { CanSeek: true } session)
            {
                session.Position = value;
            }
        }

        partial void OnPlayingChanged(bool value)
        {
            if (IsSyncing)
            {
                return;
            }

            if (value)
            {
                _player.Play();
            }
            else
            {
                _player.Pause();
            }
        }

        partial void OnLoopChanged(bool value)
        {
            if (!IsSyncing)
            {
                _player.IsLoopingEnabled = value;
            }
        }

        #endregion

        #region Handlers

        private void OnVolumeChanged(MediaPlayer sender, object e)
        {
            Sync(() => Volume = sender.Volume);
        }

        private void OnPositionChanged(MediaPlaybackSession sender, object e)
        {
            Sync(() => Position = sender.Position);
        }

        private void OnNaturalDurationChanged(MediaPlaybackSession sender, object e)
        {
            Sync(() => Duration = sender.NaturalDuration);
        }

        private void OnPlaybackStateChanged(MediaPlaybackSession sender, object e)
        {
            Sync(() => Playing = sender.PlaybackState == MediaPlaybackState.Playing);
        }

        #endregion

        #region Synchronization

        private bool IsSyncing => _syncDepth > 0;

        private int _syncDepth;

        private void Sync(Action action)
        {
            _context.Post(action =>
            {
                ++_syncDepth;
                try
                {
                    ((Action)action)();
                }
                finally
                {
                    --_syncDepth;
                }
            }, action);
        }

        #endregion

        #region Dispose

        private bool _disposed;

        ~SoundEffect()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: Unsubscribe from events
                _player.Dispose();

                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    // TODO: set large fields to null
                    _player = null;
                }

                _disposed = true;
            }
        }

        #endregion
    }
}
