﻿namespace Avalon
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Input;
    using Windows.UI;
    using path = Windows.UI.Xaml.Shapes.Path;
    public delegate void Activation(Avalon A);
    public sealed class Avalon : path
    {
        private bool isActive = false;
        private bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
                Avalon_Holding(null, null);
                Avalon_Tapped(null, null);
            }
        }
        public Avalon Real { get; private set; }
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
                    Real.Fill = value;
                }

            }
        }
        Activation Attach, Detach;
        ManipulationDeltaEventHandler Router;
        byte definition;
        public Avalon(GeometryFactory Segments, Activation Attach, Activation Detach, ManipulationDeltaEventHandler Router,byte definition , bool Real = false)
        {
            if (!Real)
            {
                this.definition = definition;
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
                this.Real = new Avalon(Segments, Attach, Detach, null, definition,true);
                this.Real.RenderTransform = RenderTransform;
                this.Real.RenderTransformOrigin = RenderTransformOrigin;
                this.Real.Data = Segments.Pattern();
                Canvas.SetZIndex(this, 2);
                Canvas.SetZIndex(this.Real, 0);
                Avalon_Holding(null, null);
            }

        }



        private void Avalon_Loaded(object sender, RoutedEventArgs e)
        {
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
        public SerializableAvalon SaveState()
        {
            return new SerializableAvalon { isActive = this.isActive, fill = (fill as SolidColorBrush).Color, Rotation = (RenderTransform as CompositeTransform).Rotation, Scale = (RenderTransform as CompositeTransform).ScaleX, TranslateX = (RenderTransform as CompositeTransform).TranslateX, TranslateY = (RenderTransform as CompositeTransform).TranslateY, Definition = this.definition };
        }
        public void RestorState(SerializableAvalon State)
        {
            isActive = State.isActive;
            fill = new SolidColorBrush(State.fill);
            CompositeTransform c = this.RenderTransform as CompositeTransform;
            c.Rotation = State.Rotation;
            c.ScaleX = State.Scale;
            c.ScaleY = State.Scale;
            c.TranslateX = State.TranslateX;
            c.TranslateY = State.TranslateY;
        }
    }
}
