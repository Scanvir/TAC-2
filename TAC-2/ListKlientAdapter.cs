using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Globalization;

namespace TAC_2
{
    public class ListKlientAdapter : BaseAdapter
    {
        private Activity activity;
        private List<Klient> klients;
        private int AuthType;

        public ListKlientAdapter(Activity activity, List<Klient> klients, int AuthType)
        {
            this.activity = activity;
            this.klients = klients;
            this.AuthType = AuthType;
        }

        public override int Count
        {
            get
            {
                return klients.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return klients[position].KlientCode;
        }

        public int GetPosition(int KlientCode)
        {
            return klients.FindIndex(X => X.KlientCode == KlientCode);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.klient_item, parent, false);
            var KlientName = view.FindViewById<TextView>(Resource.Id.KlientName);
            var KlientWarn = view.FindViewById<TextView>(Resource.Id.KlientWarn);
            var KlientDebet = view.FindViewById<TextView>(Resource.Id.KlientDebet);

            KlientName.Text = klients[position].KlientName.ToString();
            KlientName.Tag = klients[position].KlientCode.ToString();

            if (AuthType == 2)
            {
                var nfi = new NumberFormatInfo();
                nfi.NumberGroupSeparator = " ";
                nfi.NumberDecimalSeparator = ".";

                string warn = klients[position].KlientWarn;
                double dolg = klients[position].KlientDebet;

                KlientWarn.Text = warn.ToString();

                if (warn == "!")
                {
                    KlientDebet.SetTextColor(Color.ParseColor("#ffff4444"));
                } else
                {
                    KlientDebet.SetTextColor(Color.ParseColor("#dfdfdf"));
                }
                    

                    
                if (dolg == 0)
                    KlientDebet.Text = "";
                else
                    KlientDebet.Text = dolg.ToString("N0", nfi);


            }
            else
            {
                KlientWarn.Text = "";
                KlientDebet.Text = "";
            }

            return view;
        }
    }
}