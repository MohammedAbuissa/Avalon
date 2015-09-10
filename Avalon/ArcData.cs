namespace Avalon
{
    using Windows.UI.Xaml.Media;
    using System.Runtime.Serialization;
    [DataContract]
    public class NotSealedPoint
    {
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }
        public NotSealedPoint(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
    [DataContract]
    public class ArcData : NotSealedPoint
    {
        [DataMember]
        public bool IsLarge { get; set; }
        [DataMember]
        public double Rotation { get; set; }
        [DataMember]
        public double SizeX { get; set; }
        [DataMember]
        public double SizeY { get; set; }
        [DataMember]
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
