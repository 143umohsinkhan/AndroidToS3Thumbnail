using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ServerlessImageToThumbnail.Model
{
    public class DayPicture
    {
        public long DayPictureID { get; set; }

        public long DayID { get; set; }

        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool ShowInHistory { get; set; }
        public string FilePath { get; set; }

    }
}