using System;
using SkiaSharp;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace VAR.Toolbox.Code;

public static class BitmapConverter
{
    public static Bitmap? ConvertToAvalonia(SKBitmap? skiaBitmap)
    {
        if (skiaBitmap == null) return null;

        PixelFormat pixelFormat = skiaBitmap.ColorType switch
        {
            SKColorType.Bgra8888 => PixelFormat.Bgra8888,
            SKColorType.Rgba8888 => PixelFormat.Rgba8888,
            _ => PixelFormat.Rgba8888
        };

        AlphaFormat alphaFormat = skiaBitmap.AlphaType switch
        {
            SKAlphaType.Premul => AlphaFormat.Premul,
            SKAlphaType.Opaque => AlphaFormat.Opaque,
            _ => AlphaFormat.Unpremul
        };

        try
        {
            return new Bitmap(
                pixelFormat,
                alphaFormat,
                skiaBitmap.GetPixels(),
                new PixelSize(skiaBitmap.Width, skiaBitmap.Height),
                new Vector(96, 96),
                skiaBitmap.RowBytes);
        }
        catch
        {
            // Fallback for safety, though direct copy is preferred
            using var ms = new System.IO.MemoryStream();
            skiaBitmap.Encode(ms, SKEncodedImageFormat.Bmp, 100);
            ms.Position = 0;
            return new Bitmap(ms);
        }
    }
}
