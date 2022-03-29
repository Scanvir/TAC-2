using Android.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TAC_2
{
    public class ListPKO : BaseAdapter
    {
        private Activity activity;
        private List<PKO> list;
        private int ShowDate;

        public ListPKO(Activity activity, List<PKO> list, int ShowDate)
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
        public PKO GetDoc(int position)
        {
            return list[position];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.pko_item, parent, false);
            var Param1 = view.FindViewById<TextView>(Resource.Id.Param1_1);
            var Param2 = view.FindViewById<TextView>(Resource.Id.Param1_2);
            var Param3 = view.FindViewById<TextView>(Resource.Id.Param1_3);
            var Param4 = view.FindViewById<TextView>(Resource.Id.Param2_1);
            var Param5 = view.FindViewById<TextView>(Resource.Id.Param2_2);

            var nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalSeparator = ".";

            Param1.Text = list[position].NumDoc;
            Param2.Text = DateTime.ParseExact(list[position].DatePay, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd.MM.yy");
            Param3.Text = list[position].Summ.ToString("N2", nfi);
            Param4.Text = "";
            Param5.Text = "";

            Android.Graphics.Color color = Android.Graphics.Color.White;

            if(list[position].Status == 1)
                color = Android.Graphics.Color.Yellow;
            else if (list[position].Status == 2)
                color = Android.Graphics.Color.Green;

            Param1.SetTextColor(color);
            Param2.SetTextColor(color);
            Param3.SetTextColor(color);

            return view;
        }
    }
}
