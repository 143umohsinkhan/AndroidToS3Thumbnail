using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using ServerlessImageToThumbnail.Model;
using System;
using System.Collections.Generic;

namespace ServerlessImageToThumbnail.Adapter
{
    internal class AddPictureViewHolder : Java.Lang.Object
    {
        public ImageView Image { get; set; }
    }

    public class AddPicture_DataRow_Adapter : CustomListAdapter<DayPicture>
    {
        public delegate void DeleteSelectedPic(long DayPictureid);
        public event DeleteSelectedPic OnDeletePic;

        public override DayPicture this[int position] => base[position];

        public override int Count => base.Count;

        public AddPicture_DataRow_Adapter(List<DayPicture> items) : base(items)
        {
            Items = items;
        }

        public override long GetItemId(int position)
        {
            return base.GetItemId(position);
        }

        public void DeletePic(int position)
        {
            OnDeletePic?.Invoke(this[position].DayPictureID);
            base.DeleteSelectedEmployee(this[position]);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.activity_main_datarow, parent, false);

                photo = view.FindViewById<ImageView>(Resource.Id.img_addPic);
                Button remove = view.FindViewById<Button>(Resource.Id.btnremove_addPic);
                remove.Click += (s, e) =>
                {
                    IViewParent parentRow = ((Button)s).Parent;
                    ListView listview = (ListView)(parentRow).Parent;
                    int pos = listview.GetPositionForView((View)parentRow);
                    DeletePic(pos);
                };

                view.Tag = new AddPictureViewHolder()
                {
                    Image = photo,
                };
            }

            AddPictureViewHolder holder = (AddPictureViewHolder)view.Tag;
            RequestBuilder thumbnailRequest = Glide.With(view).Load(Android.Net.Uri.Parse("https://vignette.wikia.nocookie.net/lego/images/b/b4/Loading_Animation.gif"));
            Glide.With(view).Load(Items[position].FilePath).Thumbnail(thumbnailRequest).Into(holder.Image);
            return view;
        }



    }

    public abstract class CustomListAdapter<T> : BaseAdapter<T>
    {
        public List<T> Items;
        public ImageView photo;
        public T SelectedItem { get; set; }
        public delegate void ItemRemoveHandler();
        public delegate void ItemHandler(T selectedItem);
        public delegate void ItemAddHandler(T selectedItem);
        public virtual event ItemRemoveHandler AfterItemRemove;
        public virtual event ItemHandler OnRemove;
        public virtual event ItemRemoveHandler BeforeItemRemove;

        public CustomListAdapter(List<T> _items)
        {
            Items = _items == null ? new List<T>() : _items;
            OriginItems.AddRange(_items);
        }

        public override T this[int position]
        {
            get
            {
                try
                {
                    return Items[position];
                }
                catch (Exception ex)
                {
                    return default(T);
                }
            }
        }

        public override int Count => Items.Count;

        public List<T> OriginItems { get; set; } = new List<T>();

        public override long GetItemId(int position)
        {
            return position;
        }

        public bool DeleteSelectedEmployee(T t)
        {
            var removed = Items.Remove(t);
            if (removed)
            {
                NotifyDataSetChanged();
                AfterItemRemove?.Invoke();
                OnRemove?.Invoke(t);
            }
            return removed;
        }
    }
}
