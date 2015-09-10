namespace Avalon
{
    using System.Collections.Generic;
    using Windows.UI.Xaml.Media;
    public interface GeometryFactory
    {
        List<NotSealedPoint> Info { get; set; }
        PathGeometry Pattern();
    }
}
