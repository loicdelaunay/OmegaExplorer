using SixLabors.ImageSharp;

namespace OmegaExplorer.Server.Services.Images.Models.Classes;

public class ImagePositioning
{
    public byte[] Data { get; set; }

    public Image Image { get; private set; }

    public int X { get; set; }
    public int Y { get; set; }

    public ImagePositioning(byte[] data, int x, int y)
    {
        Data = data;
        X = x;
        Y = y;

        Image = Image.Load(data);
    }
}