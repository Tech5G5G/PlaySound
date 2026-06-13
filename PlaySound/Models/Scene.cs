using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlaySound.Contracts;
using PlaySound.Media;

namespace PlaySound.Models;

public sealed partial class Scene(string name) : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = name;

    public IList<ISoundEffect> Effects { get; } = new ObservableCollection<ISoundEffect>();

    public static Scene ReadFromReader(BinaryReader reader)
    {
        Scene scene = new(reader.ReadString());

        for (int i = reader.ReadInt32(); i > 0; --i)
        {
            scene.Effects.Add(SoundEffectFactory.ReadEffectFromReader(reader));
        }

        return scene;
    }

    public void WriteToWriter(BinaryWriter writer)
    {
        writer.Write(Name);

        writer.Write(Effects.Count);
        foreach (var effect in Effects)
        {
            SoundEffectFactory.WriteEffectToWriter(effect, writer);
        }
    }
}   
