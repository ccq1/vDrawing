
namespace vPlusDrawing.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private ImageSource? imgPro;

    [ObservableProperty]
    private ImageUtils? imageUtils;

    [ObservableProperty]
    private Image? imgView;

    [ObservableProperty]
    public PointCollection drawPointCollection = new();

    [ObservableProperty]
    private bool brush;

    [ObservableProperty]
    private bool line;
    [ObservableProperty]
    private bool deleteDrawing;

    private bool drawLine = false;

    [RelayCommand]
    private void inputImg(Image img)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Title = "请打开图片";
        dialog.Filter = "图像文件 | *.jpg; *.png; *.jpeg; *.bmp; *.gif | 所有文件 | *.* ";
        if (dialog.ShowDialog() == true)
        {
            BitmapImage bitmap = new BitmapImage(new Uri(dialog.FileName));
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.DecodePixelWidth = 800;
            ImgPro = bitmap;
            ImgView = img;
            ImageUtils = new ImageUtils(bitmap);
        }
    }

    [RelayCommand]
    private void SaveImg()
    {
        ImgView.Source = ImgPro;
        ImageUtils?.SaveImg(ImgView);
    }

    [RelayCommand]
    public void MouseClick(MouseButtonEventArgs e)
    {
        Point p = e.GetPosition(relativeTo: ImgView);
        Debug.WriteLine(p.ToString());
        if (Brush && !Line)
        {
            ImgPro = ImageUtils?.DrawDot(p);
        }
        else if (Brush && Line)
        {
            drawLine = true;
        }
        if (DeleteDrawing)
        {
            Brush = false;
            Line = false;
          //  ImgPro = ImageUtils.DeleteDrawing(p);
        }
    }

    [RelayCommand]
    public void MouseMove(MouseEventArgs e)
    {
        if (drawLine)
        {
            Point p = e.GetPosition(ImgView);
            DrawPointCollection.Add(p);
        }
    }

    [RelayCommand]
    public void MouseLeftButtonUp(Polyline line)
    {
        drawLine = false;
        if (DrawPointCollection.Count != 0)
        {
            ImgPro = ImageUtils?.DrawLine(DrawPointCollection);
            DrawPointCollection.Clear();        
            line.Visibility = Visibility.Hidden;
        }
    }

    [RelayCommand]
    public void RefreshPolyline(Polyline line)
    {
        if (drawLine)
        {   
            line.Visibility = Visibility.Visible;
            if (line.StrokeThickness == 1.9)
                line.StrokeThickness = 2;
            else line.StrokeThickness = 1.9;
        }
    }
    [RelayCommand]
    public void Undo()
    {
        ImgPro = ImageUtils.Undo();

    }
    [RelayCommand]
    public void Redo()
    {
        ImgPro = ImageUtils.Redo();
    }
    
}