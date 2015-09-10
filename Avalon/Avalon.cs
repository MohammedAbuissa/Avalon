namespace Avalon
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Input;
    using Windows.UI;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using path = Windows.UI.Xaml.Shapes.Path;
    public delegate void Activation(Avalon A);
    [DataContract]
    public sealed class Avalon : path
    {
        [DataMember]
        private bool isActive = false;
        [DataMember]
        public Avalon Real { get; private set; }
        [DataMember]
        public Brush fill
        {
            get
            {
                return Real.Fill;
            }
            set
            {

                if (Real != null)
                {
                    Debug.WriteLine("I'm filling now");
                    Real.Fill = value;
                }

            }
        }
        [DataMember]
        Activation Attach, Detach;
        [DataMember]
        ManipulationDeltaEventHandler Router;
        public Avalon(GeometryFactory Segments, Activation Attach, Activation Detach, ManipulationDeltaEventHandler Router, bool Real = false)
        {
            if (!Real)
            {
                Data = Segments.Pattern();
                ManipulationMode = Windows.UI.Xaml.Input.ManipulationModes.All;
                RenderTransform = new CompositeTransform();
                RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
                Fill = new SolidColorBrush(new Color { A = 0, R = 0, G = 0, B = 0 });
                this.Attach = Attach;
                this.Detach = Detach;
                this.Router = Router;
                Tapped += Avalon_Tapped;
                Holding += Avalon_Holding;
                Loaded += Avalon_Loaded;
                //ManipulationDelta += Avalon_ManipulationDelta;
                this.Real = new Avalon(Segments, Attach, Detach, null, true);
                this.Real.RenderTransform = RenderTransform;
                this.Real.RenderTransformOrigin = RenderTransformOrigin;
                this.Real.Data = Segments.Pattern();
                Canvas.SetZIndex(this, 2);
                Canvas.SetZIndex(this.Real, 0);
                Avalon_Holding(null, null);
            }

        }

        void Avalon_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Router(sender, e);
        }

        private void Avalon_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("I have been loaded");
            (this.Parent as Panel).Children.Add(Real);
        }

        private void Avalon_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (!isActive)
            {
                Attach(this);
                Real.Opacity = 0.5;
                isActive = true;
            }
        }
        public void Replaced()
        {
            this.Avalon_Tapped(null, null);
        }
        private void Avalon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (isActive)
            {
                Detach(this);
                Real.Opacity = 1;
                isActive = false;
            }
        }
        public void Manipulation(ManipulationDeltaRoutedEventArgs e)
        {
            if (!e.IsInertial)
            {
                (RenderTransform as CompositeTransform).TranslateX += e.Delta.Translation.X;
                (RenderTransform as CompositeTransform).TranslateY += e.Delta.Translation.Y;
                (RenderTransform as CompositeTransform).Rotation += e.Delta.Rotation;
                if (e.Delta.Scale > 0)
                {
                    (RenderTransform as CompositeTransform).ScaleX *= e.Delta.Scale;
                    (RenderTransform as CompositeTransform).ScaleY *= e.Delta.Scale;
                }
            }

        }
    }
}
