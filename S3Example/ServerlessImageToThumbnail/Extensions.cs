using Android;
using Android.Content;
using Android.Graphics;
using Android.Locations;
using Android.Speech;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;


namespace ServerlessImageToThumbnail
{
    public static class Extensions
    {
        public const int MY_PERMISSIONS_REQUEST_READ_EXTERNAL_STORAGE = 1000;
        public const int MY_PERMISSIONS_REQUEST_Write_EXTERNAL_STORAGE = 1001;

        public static MemoryStream BmpToByteArray(this Bitmap image)
        {
            using (MemoryStream blob = new MemoryStream())
            {
                image.Compress(Bitmap.CompressFormat.Png, 0, blob);
                return blob;
            }
        }

        public static bool CheckPermissionForPicsRead(Context context)
        {
            Android.App.Activity activity = (Android.App.Activity)context;
            bool isPermit = true;

            if (ContextCompat.CheckSelfPermission(context, Manifest.Permission.ReadExternalStorage) != Android.Content.PM.Permission.Granted)
            {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(activity, Manifest.Permission.ReadExternalStorage))
                {
                    Snackbar.Make(activity.CurrentFocus, "External storage permission is necessary", Snackbar.LengthIndefinite)
                 .SetAction(Resource.String.OK,
                            delegate
                            {
                                ActivityCompat.RequestPermissions(activity, new[] { Manifest.Permission.ReadExternalStorage }, MY_PERMISSIONS_REQUEST_READ_EXTERNAL_STORAGE);
                                isPermit = true;
                            }).SetAction(Resource.String.CANCEL, delegate { isPermit = false; })
                 .Show();
                }
                else
                {
                    ActivityCompat.RequestPermissions(activity, new[] { Manifest.Permission.ReadExternalStorage }, MY_PERMISSIONS_REQUEST_READ_EXTERNAL_STORAGE);
                    isPermit = true;
                }
            }
            else
            {
                isPermit = true;
            }
            return isPermit;
        }

        public static bool CheckPermissionForPicsWrite(Context context)
        {
            Android.App.Activity activity = (Android.App.Activity)context;
            bool isPermit = true;

            if (ContextCompat.CheckSelfPermission(context, Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
            {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(activity, Manifest.Permission.WriteExternalStorage))
                {
                    Snackbar.Make(activity.CurrentFocus, "External storage permission is necessary", Snackbar.LengthIndefinite)
                 .SetAction(Resource.String.OK,
                            delegate
                            {
                                ActivityCompat.RequestPermissions(activity, new[] { Manifest.Permission.WriteExternalStorage }, MY_PERMISSIONS_REQUEST_Write_EXTERNAL_STORAGE);
                                isPermit = true;
                            }).SetAction(Resource.String.CANCEL, delegate { isPermit = false; })
                 .Show();
                }
                else
                {
                    ActivityCompat.RequestPermissions(activity, new[] { Manifest.Permission.WriteExternalStorage }, MY_PERMISSIONS_REQUEST_Write_EXTERNAL_STORAGE);
                    isPermit = true;
                }
            }
            else
            {
                isPermit = true;
            }
            return isPermit;
        }
    }
}