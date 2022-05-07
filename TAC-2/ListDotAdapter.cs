using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace TAC_2
{
    public class ListDotAdapter : BaseAdapter
    {
        private Activity activity;
        private List<Dot> dots;
        private string klientCode;

        public ListDotAdapter(Activity activity, List<Dot> dots, string klientCode)
        {
            this.activity = activity;
            this.dots = dots;
            this.klientCode = klientCode;
        }


        public override int Count
        {
            get
            {
                return dots.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return dots[position].DotAddress;
        }
        public int GetPosition(int code)
        {
            return dots.FindIndex(a => a.DotCode == code);
        }
        public Dot GetDot(int position)
        {
            return dots[position];
        }
        public override long GetItemId(int position)
        {
            return dots[position].DotCode;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.dot_item, parent, false);
            var DotName = view.FindViewById<TextView>(Resource.Id.DotName);
            var DotAddress = view.FindViewById<TextView>(Resource.Id.DotAddress);
            var DotCode = view.FindViewById<TextView>(Resource.Id.DotCode);

            DotName.Text = dots[position].DotName.ToString();
            DotName.Tag = dots[position].DotCode.ToString();
            DotAddress.Text = dots[position].DotAddress.ToString();
            DotCode.Text = klientCode + "\\" + dots[position].DotCode.ToString();

            return view;
        }
    }
}