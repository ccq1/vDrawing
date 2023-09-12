namespace vPlusDrawing.Utils;

public class ImageUtils /*: IRecipient<myImage>*/
{
    private double MyImageX, MyImageY;
    private double MyImagepixelX, MyImagePixely;
    private double MyImageDpiX, MyImagedpiY;//位图图像每英寸点数
    private DrawingGroup baseImage = new DrawingGroup(); // 底图，以及最后返图实时展示，addRange(drawingGroup2). 将覆盖层填充进去，然后Drawing，return。
    private DrawingGroup drawingGroup = new DrawingGroup(); // 绘制层，add(<shape>)，维护填充的图像，cirle，line，rect之类的。进行撤销操作等一系列的修改工作。
    private DrawingGroup tempList = null;
    private Stack<Drawing> drawings = new Stack<Drawing>();
    private Stack<DrawingWithCommand> commands = new Stack<DrawingWithCommand>();
    private BitmapImage bitmap;

    public ImageUtils(BitmapImage bitmap)
    {
        baseImage.Children.Clear();
        this.bitmap = bitmap;
        this.MyImagepixelX = bitmap.PixelWidth;
        this.MyImagePixely = bitmap.PixelHeight;
        this.MyImageDpiX = bitmap.DpiX;
        this.MyImagedpiY = bitmap.DpiY;
        baseImage.Children.Add(new ImageDrawing(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight)));

    }
    public ImageUtils() { }

    public ImageSource DrawDot(Point p)
    {
        Point center = new Point(p.X, p.Y);
        EllipseGeometry ellipseGeometry = new EllipseGeometry();
        ellipseGeometry.Center = center;
        ellipseGeometry.RadiusX = 10;
        ellipseGeometry.RadiusY = 10;
        GeometryDrawing g = new GeometryDrawing();
        g.Geometry = ellipseGeometry;
        g.Pen = new Pen(Brushes.Black, 1);
        drawingGroup.Children.Add(g);
        tempList = baseImage;
        baseImage.Children.Add(drawingGroup);
        return DrawImage(ref baseImage);
    }

    public ImageSource DrawLine(PointCollection DrawPointCollection)
    {

        GeometryGroup lineGroup = new GeometryGroup();
        for (int i = 0; i < DrawPointCollection.Count - 1; i++)
        {
            LineGeometry line = new LineGeometry();
            line.StartPoint = DrawPointCollection[i];
            line.EndPoint = DrawPointCollection[i + 1];
            lineGroup.Children.Add(line);
        }
        GeometryDrawing g = new GeometryDrawing();
        g.Geometry = lineGroup;
        g.Pen = new Pen(Brushes.Black, 2);
        drawingGroup.Children.Add(g);
        tempList = baseImage;
        baseImage.Children.Add(drawingGroup);
        return DrawImage(ref baseImage);
    }

    /// <summary>
    /// 将图像集合绘制到控件中
    /// </summary>
    /// <param name="g"></param>
    public ImageSource DrawImage(ref DrawingGroup drawing)
    {
        //在控件中展示
        DrawingImage drawImage = new DrawingImage(drawing);
        drawing = tempList;
        tempList = null;
        return drawImage;
    }

    public ImageSource DeleteDrawing(Point point)
    {
        RectangleGeometry rect = new RectangleGeometry();
        var location = new Point(point.X - 5, point.Y - 5);
        rect.Rect = new Rect(location, new Size(10, 10)); //所以他是以location为起点，然后向右向下画10个像素的矩形。矩形以point为中心。
        Drawing drawing = drawingGroup.Children.FirstOrDefault(d => d.Bounds.IntersectsWith(rect.Bounds));
        if (drawing != null)
        {
            drawingGroup.Children.Remove(drawing);
            drawings.Push(drawing);
        }
        tempList = baseImage;
        baseImage.Children.Add(drawingGroup);
        return DrawImage(ref baseImage);
    }


    /// <summary>
    /// 撤销
    /// </summary>
    /// <returns></returns>
    public ImageSource Undo()
    {
        int lastIndex = drawingGroup.Children.Count - 1;
        tempList = baseImage;
        if (lastIndex >= 0)
        {
            drawings.Push(drawingGroup.Children[lastIndex]); // 进栈，栈结构维护撤销的控件
            drawingGroup.Children.RemoveAt(lastIndex);
            baseImage.Children.Add(drawingGroup);
        }
        //合成图像
        return DrawImage(ref baseImage);
    }
    /// <summary>
    /// 退回撤销
    /// </summary>
    /// <returns></returns>
    public ImageSource Redo()
    {
        tempList = baseImage;
        if (drawings.Count > 0)
        {
            drawingGroup.Children.Add(drawings.Pop()); // 出栈，栈结构维护撤销的控件
            baseImage.Children.Add(drawingGroup);
        }
        //合成图像
        return DrawImage(ref baseImage);
    }



    /// <summary>
    /// 保存图像，将控件转为Png图像
    /// </summary>
    public void SaveImg(Visual img)
    {
        DrawingImage drawingImage = new(baseImage); // 获取 DrawingImage 实例
        RenderTargetBitmap rtb = new RenderTargetBitmap((int)drawingImage.Width, (int)drawingImage.Height, 96, 96, PixelFormats.Pbgra32);
        DrawingVisual drawingVisual = new DrawingVisual();
        using (DrawingContext context = drawingVisual.RenderOpen())
        {
            context.DrawImage(drawingImage, new Rect(0, 0, drawingImage.Width, drawingImage.Height));
        }
        rtb.Render(drawingVisual);

        PngBitmapEncoder png = new PngBitmapEncoder();
        png.Frames.Add(BitmapFrame.Create(rtb));
        using (Stream stream = File.Create("D:\\test.png"))
        {
            png.Save(stream);
        }
    }
}