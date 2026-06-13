using System.Collections.ObjectModel;

namespace PlaySound.Models;

public sealed class Production
{
    // public string Name { get; set; }

    public IList<Scene> Scenes { get; } = new ObservableCollection<Scene>();

    public string Path { get; set; }

    public Production() { }

    public static async Task<Production> ReadFromFileAsync(string path)
    {
        await using var stream = File.OpenRead(path);
        using BinaryReader reader = new(stream);

        // Header check.
        if (reader.ReadByte() != 0x87 || reader.ReadByte() != 0x80 || reader.ReadByte() != 0x0D || reader.ReadByte() != 0x00)
        {
            return null;
        }

        var production = ReadFromReader(reader);
        production.Path = path;
        return production;
    }

    public static Production ReadFromReader(BinaryReader reader)
    {
        Production production = new();

        for (int i = reader.ReadInt32(); i > 0; --i)
        {
            production.Scenes.Add(Scene.ReadFromReader(reader));
        }

        return production;
    }

    public async Task WriteToFileAsync()
    {
        await using MemoryStream stream = new();
        await using BinaryWriter writer = new(stream);

        // Header ;)
        writer.Write([0x87, 0x80, 0x0D, 0x00]);
        WriteToWriter(writer);

        await File.WriteAllBytesAsync(Path, stream.GetBuffer());
    }

    public void WriteToWriter(BinaryWriter writer)
    {
        writer.Write(Scenes.Count);
        foreach (var scene in Scenes)
        {
            scene.WriteToWriter(writer);
        }
    }
}
