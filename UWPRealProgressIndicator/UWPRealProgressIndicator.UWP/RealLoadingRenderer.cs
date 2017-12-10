using MPDC.Controls.UWP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ActivityIndicator), typeof(RealLoadingRenderer))]
namespace MPDC.Controls.UWP
{
    class RealLoadingRenderer : ViewRenderer<ActivityIndicator, ProgressRing>
    {
        object _foregroundDefault;

        protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicator> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    SetNativeControl(new ProgressRing());

                    Control.Loaded += OnControlLoaded;
                }

                // UpdateColor() called when loaded to ensure we can cache dynamic default colors
                UpdateIsRunning();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ActivityIndicator.IsRunningProperty.PropertyName || e.PropertyName == VisualElement.OpacityProperty.PropertyName)
                UpdateIsRunning();
            else if (e.PropertyName == ActivityIndicator.ColorProperty.PropertyName)
                UpdateColor();
        }

        void OnControlLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _foregroundDefault = Control.Foreground;
            UpdateColor();
        }

        void UpdateColor()
        {
            Color color = Element.Color;

            if (color.IsDefault)
            {
                Control.Foreground = _foregroundDefault as Brush;
            }
            else
            {
                Control.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255)));
            }
        }

        void UpdateIsRunning()
        {
            Control.IsActive = Element.IsRunning;
        }
    }
}
