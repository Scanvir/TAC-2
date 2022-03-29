using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Globalization;

namespace TAC_2
{
    public class ListZakazAdapter : BaseAdapter
    {
        private Activity activity;
        private List<Order> list;

        Color color;

        public ListZakazAdapter(Activity activity, List<Order> list)
        {
            this.activity = activity;
            this.list = list;
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
        public Order GetZakaz(int position)
        {
            return list[position];
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

            Param1_1.Text = list[position].DateDoc.ToString("dd.MM.yy");
            
            int status = list[position].Status;

            if (status == 0)
            {
                Param1_2.Text = "новий";
                color = Color.White;
            }
            else if (status == 1)
            {
                Param1_2.Text = "відправлено";
                color = Color.Yellow;
            }
            else if (status == 2)
            {
                Param1_2.Text = "отриманий";
                color = Color.Green;
            }

            Param1_3.Text = list[position].Summ.ToString("N2", nfi);

            Param1_1.SetTextColor(color);
            Param1_2.SetTextColor(color);
            Param1_3.SetTextColor(color);

            return view;
        }
    }
}