using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace TAC_2
{
    public class ListOborudAdapter : BaseAdapter
    {
        private Activity activity;
        private List<OborudRest> rest;

        public ListOborudAdapter(Activity activity, List<OborudRest> rest)
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
            return 1; //rest[position].OborudCode;
        }

        public OborudRest GetOborudRest(int position)
        {
            return rest[position]; 
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.oborud_item, parent, false);
            var OborudName = view.FindViewById<TextView>(Resource.Id.Name);
            var OborudRest = view.FindViewById<TextView>(Resource.Id.Rest);
            var TaraFacing = view.FindViewById<TextView>(Resource.Id.Facing);
            var SerialNumber = view.FindViewById<TextView>(Resource.Id.serialNumber);
            LinearLayout ly = view.FindViewById<LinearLayout>(Resource.Id.Item);

            OborudName.Text = rest[position].OborudName.ToString();
            OborudName.Tag = rest[position].OborudCode.ToString();
            OborudRest.Text = rest[position].Qty.ToString();
            TaraFacing.Text = rest[position].Facing.ToString();
            SerialNumber.Text = rest[position].SerialNumber.ToString();

            return view;
        }
    }
}