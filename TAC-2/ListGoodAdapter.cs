using Android.App;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TAC_2
{
    public class ListGoodAdapter: BaseAdapter
    {
        private Activity activity;
        private List<Price> list;
        private string GUID;
        private DBHelper db;
        private Android.Graphics.Color color;

        public ListGoodAdapter(Activity activity, List<Price> list, string GUID)
        {
            this.activity = activity;
            this.list = list;
            this.GUID = GUID;

            db = new DBHelper(activity);
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
        public Price GetPrice(int position)
        {
            return list[position];
        }
        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.good_item, parent, false);
            var Param1_1 = view.FindViewById<TextView>(Resource.Id.Param1_1);
            var Param2_1 = view.FindViewById<TextView>(Resource.Id.Param2_1);
            var Param3_1 = view.FindViewById<TextView>(Resource.Id.Param3_1);
            var Param3_2 = view.FindViewById<TextView>(Resource.Id.Param3_2);
            var myImage = view.FindViewById<ImageView>(Resource.Id.imageView1);

            myImage.SetImageBitmap(null);

            ImageService.Instance
                .LoadUrl("http://79.143.40.187:1221/?GetImg=" + list[position].GoodCode)
                .DownSample()
                .BitmapOptimizations(true)
                .WithCache(FFImageLoading.Cache.CacheType.All)
                .Into(myImage);

            var nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalSeparator = ".";

            Param1_1.Text = list[position].GoodName;
            Param1_1.Tag = list[position].GoodCode;

            

            if (list[position].Quantity > 0)
                Param2_1.Text = list[position].Quantity.ToString("N2", nfi);
            else
                Param2_1.Text = "0.00";

            Param3_2.Text = list[position].PriceUAH.ToString("N2", nfi) + " грн.";

            if (list[position].Quantity == 0)
                color = Android.Graphics.Color.LightGray;    
            else
                color = Android.Graphics.Color.DarkGray;

            Param1_1.SetTextColor(color);
            Param2_1.SetTextColor(color);
            Param3_1.SetTextColor(color);
            Param3_2.SetTextColor(color);

            List<OrderTab> orderTab = db.GetZakazTabList(activity, GUID);
            OrderTab tab = orderTab.Find(X => X.GoodCode == list[position].GoodCode);
            if (tab != null)
                Param3_1.Text = tab.Quantity.ToString("N2", nfi);
            else
                Param3_1.Text = "";

            return view;
        }
    }
}