using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace TAC_2
{
    public class ListTaraAdapter : BaseAdapter
    {
        private Activity activity;
        private List<TaraRest> rest;

        public ListTaraAdapter(Activity activity, List<TaraRest> rest)
        {
            this.activity = activity;
            this.rest = rest;
        }

        public override int Count
        {
            get
            {
                return rest.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return rest[position].TaraCode;
        }

        public TaraRest GetTaraRest(int position)
        {
            return rest[position];
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.tara_item, parent, false);
            var TaraName = view.FindViewById<TextView>(Resource.Id.Name);
            var TaraRest = view.FindViewById<TextView>(Resource.Id.Rest);
            var TaraFacing = view.FindViewById<TextView>(Resource.Id.Facing);
            var Delay = view.FindViewById<TextView>(Resource.Id.Delay);
            LinearLayout ly = view.FindViewById<LinearLayout>(Resource.Id.Item);

            TaraName.Text = rest[position].TaraName.ToString();
            TaraName.Tag = rest[position].TaraCode.ToString();
            TaraRest.Text = rest[position].Qty.ToString();
            TaraFacing.Text = rest[position].Facing.ToString();
            Delay.Text = rest[position].Dalay.ToString();

            if (rest[position].Dalay == 0)
                Delay.SetTextColor(Android.Graphics.Color.ParseColor("#2f2f2f"));

            return view;
        }
    }
}