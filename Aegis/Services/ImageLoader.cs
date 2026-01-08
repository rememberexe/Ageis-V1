using System.IO;
using System.Windows.Media.Imaging;

public static class ImageLoader
{
    public static BitmapImage LoadUnlocked(string path)
    {
        if (!File.Exists(path))
            return null;

        using var fs = new FileStream(
            path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite);

        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.StreamSource = fs;
        bitmap.EndInit();
        bitmap.Freeze();

        return bitmap;
    }
}
