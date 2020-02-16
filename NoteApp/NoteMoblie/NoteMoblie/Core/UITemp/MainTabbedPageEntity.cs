using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NoteMoblie.Core.TempClass
{
    public class MainTabbedPageEntity
    {
        public string Title { get; set; }

        private string icon;
        public string Icon
        {
            get { return icon; }
            set
            {
                var vl = value;
                icon = vl;
                IconImage = new FontImageSource()
                {
                    Glyph = vl,
                    FontFamily = "iconfont.ttf#",
                    Size = 30
                };
            }
        }

        public Xamarin.Forms.View Content { get; set; }

        public ImageSource IconImage { get; set; }
    }
}
