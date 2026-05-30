using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace VAR.Toolbox.Controls;

public class CtrImageViewer : Border
{
    private readonly Image _image;

    public Bitmap? ImageShow
    {
        get;
        set
        {
            field = value;
            _image.Source = value;
        }
    }

    public CtrImageViewer()
    {
        Background = Brushes.Black;
        BorderBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80));
        BorderThickness = new Avalonia.Thickness(1);
        _image = new Image { Stretch = Stretch.Uniform, };
        Child = _image;
    }
}