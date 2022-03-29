using Android.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TAC_2
{
    public class ListOrderJournAdapter : BaseAdapter
    {
        private Activity activity;
        private List<Order> list;
        private DateTime ShowDate;

        public ListOrderJournAdapter(Activity activity, List<Order> list, DateTime ShowDate)
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
        public Order GetDoc(int position)
        {
            return list[position];
        }
        public double GetSumm()
        {
            return list.Sum(x => x.Summ);
        }
        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.zakaz_item, parent, false);
            var Param1_1 = view.FindViewById<TextView>(Resource.Id.Param1_1);
            var Param1_2 = view.FindViewById<TextView>(Resource.Id.Param1_2);
            var Param1_3 = view.FindViewById<TextView>(Resource.Id.Param1_3);

            var nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalSeparator = ".";


            Param1_1.Text = list[position].KlientName;
            Param1_2.Text = ""; 
            Param1_3.Text = list[position].Summ.ToString("N2", nfi);

            Android.Graphics.Color color = Android.Graphics.Color.White;

            if (list[position].Status == 1)
                color = Android.Graphics.Color.Yellow;
            else if (list[position].Status == 2)
                color = Android.Graphics.Color.Green;

            Param1_1.SetTextColor(color);
            Param1_2.SetTextColor(color);
            Param1_3.SetTextColor(color);

            return view;
        }
    }
}