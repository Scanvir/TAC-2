using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TAC_2
{
    class ListGoodsDirectoryAdapter : BaseAdapter
    {
        private List<Model.GoodsDirectory> list;
        private Context context;

        public ListGoodsDirectoryAdapter(List<Model.GoodsDirectory> list, Context context)
        {
            this.list = list;
            this.context = context;
        }
        public override int Count => list.Count;
        public override Java.Lang.Object GetItem(int position)
        {
            return list[position].Name;
        }
        public override long GetItemId(int position)
        {
            return list[position].Code;
        }
        public int GetPosition(int dotCode)
        {
            return list.FindIndex(X => X.Code == dotCode);
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.good_group, null);
            }

            TextView Name = convertView.FindViewById<TextView>(Resource.Id.Name);
            TextView Count = convertView.FindViewById<TextView>(Resource.Id.Count);
            LinearLayout linear = convertView.FindViewById<LinearLayout>(Resource.Id.linear);

            double density = Math.Round(context.Resources.DisplayMetrics.Density, 2);

            Name.Text = list[position].Name;

            if (list[position].Level == 1)
                linear.SetPadding(int.Parse(Math.Round(density * 5, 0).ToString()), 5, 5, 5);
            else if (list[position].Level == 2)
                linear.SetPadding(int.Parse(Math.Round(density * 25, 0).ToString()), 5, 5, 5);
            else if (list[position].Level == 3)
                linear.SetPadding(int.Parse(Math.Round(density * 45, 0).ToString()), 5, 5, 5);

            // сейчас еще кое што проверим


            if (list[position].GoodCount > 0)
                Count.Text = "(" + list[position].GoodCount + ")";
            else
                Count.Text = "";

            return convertView;
        }
    }
}