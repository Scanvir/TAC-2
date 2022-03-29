using Android.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TAC_2
{
    public class ListDebet : BaseAdapter
    {
        private Activity activity;
        private List<Debet> list;
        private int ShowDate;

        public ListDebet(Activity activity, List<Debet> list, int ShowDate)
        {
            this.activity = activity;
            this.list = list;
            this.ShowDate = ShowDate;
        }

        public override int Count
        {
            get
            {
                return list.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }
        public Debet GetDoc(int position)
        {
            return list[position];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.debet_item, parent, false);
            var NumDoc = view.FindViewById<TextView>(Resource.Id.NumDoc);
            var DateDoc = view.FindViewById<TextView>(Resource.Id.DateDoc);
            var Dolg = view.FindViewById<TextView>(Resource.Id.Dolg);

            NumDoc.Text = list[position].NumDoc;
            if (ShowDate == 1)
                DateDoc.Text = DateTime.ParseExact(list[position].DateDoc, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd.MM.yy");
            else
                DateDoc.Text = "";
            Dolg.Text = list[position].Dolg.ToString();
            
            return view;
        }
    }
}