using Android.Graphics.Drawables;
using AppFramework.Droid;
using AppFramework.Shared.Controls; 
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;

[assembly: ExportRenderer(typeof(BorderlessEditor), typeof(BorderlessEditorRenderer))]

namespace AppFramework.Droid
{ 
    public class BorderlessEditorRenderer : EditorRenderer
    {
        #region Constructor

        public BorderlessEditorRenderer()
            : base(Application.Context)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Used to set the transparent color for editor control background property.
        /// </summary>
        /// <param name="e">The editor</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (this.Control != null)
            {  
                //this.Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
            }
        }

        #endregion
    }
}