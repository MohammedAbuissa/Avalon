namespace Avalon
{
    using System;
    using System.Collections.Generic;
    using Windows.Foundation;
    using Windows.UI.Xaml.Media;
    using System.Runtime.Serialization;
    public class AvalonPolygonFactory : GeometryFactory
    {
        int sides;
        string ob;
        public AvalonPolygonFactory(int Sides, string ObjectName = "") : this(Sides, null, ObjectName) { }
        public AvalonPolygonFactory(int Sides, List<NotSealedPoint> Info, string ObjectName = "")
        {
            sides = Sides;
            ob = ObjectName;
            if (Info != null)
                this.Info = Info;
        }
        private List<NotSealedPoint> info;
        public List<NotSealedPoint> Info
        {
            get
            {
                return info;
            }
            set
            {
                if (value.Count == sides)
                    info = value;
                else
                    throw new ArgumentException("The List must contain exactly" + sides + " elements" + ob != "" ? "represent the points of a " + ob : "");
            }
        }

        public PathGeometry Pattern()
        {
            PathGeometry geo = new PathGeometry();
            PathFigure figure = new PathFigure();
            geo.Figures.Add(figure);
            if (Info == null)
                throw new ArgumentNullException("Set the value of Info property");
            figure.StartPoint = new Point(Info[0].X, info[0].Y);
            for (int i = 0; i < sides; i++)
                figure.Segments.Add(new LineSegment { Point = new Point(Info[(i + 1) % sides].X, Info[(i + 1) % sides].Y) });
            return geo;
        }
    }

    public class AvalonArcFactory : GeometryFactory
    {
        private int Arcs;
        private String Error = "";
        public AvalonArcFactory(int Arcs, string Error = "") : this(Arcs, null, Error) { }
        public AvalonArcFactory(int Arcs, List<NotSealedPoint> Data, string Error = "")
        {
            this.Arcs = Arcs;
            this.Error = Error;
            if (Data != null)
                Info = Data;
        }
        private List<NotSealedPoint> info;
        public List<NotSealedPoint> Info
        {
            get
            {
                return info;
            }

            set
            {
                if (value.Count == Arcs)
                    info = value;
                else
                    throw new ArgumentException("Number of arcs must equal to " + Arcs + Error != "" ? "To Construct " + Error : "");
            }
        }

        public PathGeometry Pattern()
        {
            PathGeometry geo = new PathGeometry();
            PathFigure figure = new PathFigure();
            geo.Figures.Add(figure);
            figure.StartPoint = new Point(Info[0].X, Info[0].Y);
            for (int i = 0; i < Arcs; i++)
            {
                ArcData a = Info[(i + 1) % Arcs] as ArcData;
                figure.Segments.Add(new ArcSegment { Point = new Point(a.X, a.Y), Size = new Size(a.SizeX, a.SizeY), IsLargeArc = a.IsLarge, RotationAngle = a.Rotation, SweepDirection = a.SweepDirection });
            }
            return geo;
        }
    }
}
