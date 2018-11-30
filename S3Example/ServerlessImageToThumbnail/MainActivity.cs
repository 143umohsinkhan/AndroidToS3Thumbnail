using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Widget;
using Java.IO;
using ServerlessImageToThumbnail.Adapter;
using System;
using System.IO;

namespace ServerlessImageToThumbnail
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IDialogInterfaceOnClickListener
    {
        private ListView lstPictures;
        private readonly int REQUEST_CAMERA = 101;
        private readonly int SELECT_FILE = 102;
        private int UserSelectedOption;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            lstPictures = FindViewById<ListView>(Resource.Id.lstPictures);
            FindViewById<Button>(Resource.Id.btnuploadpic).Click += delegate
            {
                // ShowUploadPicDialog();
                // GalleryIntent();
                bool check = Extensions.CheckPermissionForPicsRead(this) && Extensions.CheckPermissionForPicsWrite(this);
                if (check)
                {
                    CameraIntent();
                }
            };

            FillControl();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                if (requestCode == SELECT_FILE)
                {
                    AfterFileSelectFromGallery(data);
                }
                if (requestCode == REQUEST_CAMERA)
                {
                    AfterCameraSelection(data);
                }
            }
        }

        private void AfterFileSelectFromGallery(Intent data)
        {
            Bitmap bm = null;
            if (data != null)
            {
                try
                {
                    bm = MediaStore.Images.Media.GetBitmap(ApplicationContext.ContentResolver, data.Data);
                    AddPicRequest(DateTime.Now.Ticks + ".jpg", bm.BmpToByteArray());
                }
                catch (System.IO.IOException)
                {

                }
            }
        }

        private void AfterCameraSelection(Intent data)
        {
            Bitmap thumbnail = (Bitmap)data.Extras.Get("data");
            string fileName = DateTime.Now.Ticks + ".jpg";
            MemoryStream bytes = new MemoryStream();
            thumbnail.Compress(Bitmap.CompressFormat.Png, 0, bytes);
            Java.IO.File destination = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory,
                  fileName);
            FileOutputStream fo;
            try
            {
                destination.CreateNewFile();
                fo = new FileOutputStream(destination);
                fo.Write(bytes.ToArray());
                fo.Close();
                thumbnail.Recycle();

                AddPicRequest(fileName, bytes);
            }
            catch (Exception)
            {

            }
        }

        private async void AddPicRequest(string filename, Stream stream)
        {
            try
            {
                //UploadFileMPUHighLevelAPITest.stream = stream;
                await UploadFileMPUHighLevelAPITest.UploadFileAsync(stream);

                //await viewModel.AddDayPicture(new Shared.Model.WebRequestDto.AddPictureRequestDto
                //{
                //    ClipID = LoggedInUser.User.ClipSerialNumberID.Value,
                //    ContentType = "image/jpeg",
                //    CustomerID = LoggedInUser.SelectedDay.CustomerID.Value,
                //    DayID = LoggedInUser.SelectedDay.DayID,
                //    FileName = filename,
                //    FileBytes = filecontent,
                //    ShowInHistory = true
                //});

                FillControl();
            }
            catch (Exception ex)
            {
                Log.Error("Error in upload", ex.ToString());
            }
        }

        private async void FillControl()
        {
            //List<DayPicture> dayPictures = await viewModel.GetDayPictures(LoggedInUser.SelectedDay.DayID,
            //                                    LoggedInUser.User.ClipSerialNumberID.Value,
            //                                    LoggedInUser.SelectedDay.CustomerID.Value);

            //AddPicture_DataRow_Adapter adapter = new AddPicture_DataRow_Adapter(dayPictures == null ? new List<DayPicture>() : dayPictures);
            //adapter.OnDeletePic -= Adapter_AfterItemRemove;
            //adapter.OnDeletePic += Adapter_AfterItemRemove;
            //lstPictures.Adapter = adapter;

        }

        private void Adapter_AfterItemRemove(long DayPictureID)
        {
            //await viewModel.DeleteDayPicture(DayPictureID);
        }

        private void ShowUploadPicDialog()
        {
            if (Extensions.CheckPermissionForPicsRead(this) && Extensions.CheckPermissionForPicsWrite(this))
            {
                string[] items = {
                "Take Photo", "Choose from Library"};

                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                builder.SetTitle("Add Photo!");
                builder.SetItems(items, this);
                builder.Show();
            }
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            bool check = Extensions.CheckPermissionForPicsRead(this) && Extensions.CheckPermissionForPicsWrite(this);
            UserSelectedOption = which;
            if (which == 0)
            {
                if (check)
                {
                    CameraIntent();
                }
            }

            if (which == 1)
            {
                if (check)
                {
                    GalleryIntent();
                }
            }
        }

        private void CameraIntent()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, REQUEST_CAMERA);
        }

        private void GalleryIntent()
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select File"), SELECT_FILE);
        }
    }
}

