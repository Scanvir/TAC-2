using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TAC_2
{
    public class ListZakazTabAdapter : BaseAdapter
    {
        private Activity activity;
        private List<OrderTab> list;

        public ListZakazTabAdapter(Activity activity, List<OrderTab> list)
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
        public OrderTab GetZakazTab(int position)
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
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.zakaz_tab_item, parent, false);
            
            var GoodName = view.FindViewById<TextView>(Resource.Id.GoodName);
            var Quantity = view.FindViewById<TextView>(Resource.Id.Quantity);
            var Summa = view.FindViewById<TextView>(Resource.Id.Summa);

            var nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalSeparator = ".";

            GoodName.Text = list[position].GoodName;
            Quantity.Text = list[position].Quantity.ToString("N0", nfi);
            Summa.Text = list[position].Summ.ToString("N2", nfi);

            return view;
        }
    }
}