namespace Avalon
{
    using Windows.UI.Xaml.Media;
    using System.Runtime.Serialization;
    public class NotSealedPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public NotSealedPoint(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
    public class ArcData : NotSealedPoint
    {
        public bool IsLarge { get; set; }
        public double Rotation { get; set; }
        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public SweepDirection SweepDirection { get; set; }
        public ArcData(double X, double Y, double SizeX, double SizeY, double RotationAngle, SweepDirection Sweep = SweepDirection.Clockwise, bool IsLarge = false) : base(X, Y)
        {
            this.SizeX = SizeX;
            this.SizeY = SizeY;
            Rotation = RotationAngle;
            this.IsLarge = IsLarge;
            SweepDirection = Sweep;
        }
    }
}
