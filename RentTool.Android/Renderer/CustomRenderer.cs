using Android.Content;
using RentTool.Controls;
using RentTool.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Entry), typeof(CustomRenderer), new[] { typeof(CustomVisual) })]

namespace RentTool.Droid.Renderer
{
    public class CustomRenderer : EntryRenderer
    {
        public CustomRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.SetBackground(null);
            }
        }
    }
}