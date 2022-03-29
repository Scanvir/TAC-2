using System;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Widget;
using Google.Android.Material.Tabs;
using Xamarin.Forms;

namespace TAC_2
{
    [Activity(Label = "Тара", Theme = "@style/AppTheme.NoActionBar")]
    class Detailing : AndroidX.AppCompat.App.AppCompatActivity, Android.Views.View.IOnClickListener, AdapterView.IOnItemClickListener
    {
        private DBHelper db;
        private Google.Android.Material.Tabs.TabLayout tabLayout1;

        private int Type = 0;

        private int klientCode;
        private int dotCode = 0;

        private Klient klient;

        private TextView KlientDotName;
        private TextView notKlientDotName;

        private Android.Widget.ListView lv;

        private ListTaraAdapter adapterT;
        private ListOborudAdapter adapterO;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.detailing);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            
            db = new DBHelper(this);
            
            klientCode = Intent.GetIntExtra("KlientCode", 0);
            klient = db.GetKlient(this, klientCode);

            /*
            KlientName = FindViewById<TextView>(Resource.Id.KlientName);
            KlientName.Text = klient.KlientName;
            */

            toolbar.Title = klient.KlientName;
            SetSupportActionBar(toolbar);

            KlientDotName = FindViewById<TextView>(Resource.Id.KlientDotName);
            KlientDotName.Text = "Оберіть адресу...";
            KlientDotName.SetOnClickListener(this);

            notKlientDotName = FindViewById<TextView>(Resource.Id.notKlientDotName);
            notKlientDotName.SetOnClickListener(this);

            lv = FindViewById<Android.Widget.ListView>(Resource.Id.listDetailing);
            lv.FastScrollEnabled = true;
            lv.OnItemClickListener = this;

            tabLayout1 = FindViewById<Google.Android.Material.Tabs.TabLayout>(Resource.Id.tabLayout1);
            tabLayout1.TabSelected += (object sender, Google.Android.Material.Tabs.TabLayout.TabSelectedEventArgs e) =>
            {
                var tab = e.Tab;
                if (tab.Text == "Тара")
                    Type = 0;
                else
                    Type = 1;
                GetDetailing(klientCode, dotCode, Type);
            };
            
        }
        protected override void OnResume()
        {
            base.OnResume();
            GetDetailing(klientCode, dotCode, Type);
        }
        private void GetDetailing(int klientCode, int dotCode, int type)
        {
            lv.FastScrollEnabled = true;
            if (type == 0)
            {
                adapterT = new ListTaraAdapter(this, db.GetTaraList(this, klientCode, dotCode));
                lv.Adapter = adapterT;
            }
            else if (type == 1)
            {
                adapterO = new ListOborudAdapter(this, db.GetOborudList(this, klientCode, dotCode));
                lv.Adapter = adapterO;
            }
        }
        public void OnClick(Android.Views.View v)
        {
            if (v.Id == Resource.Id.KlientDotName)
            {
                var intent = new Intent(this, type: typeof(DotsActivity));
                intent.PutExtra("KlientCode", klient.KlientCode);
                StartActivityForResult(intent, 100);
            } else if (v.Id == Resource.Id.notKlientDotName)
            {
                dotCode = 0;
                KlientDotName.Text = "Оберіть адресу...";
                GetDetailing(klientCode, dotCode, Type);
            }
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (data == null) { return; }
            dotCode = data.GetIntExtra("dotCode", 0);
            KlientDotName.Text = data.GetStringExtra("dotName");
        }
        public void OnItemClick(AdapterView parent, Android.Views.View view, int position, long id)
        {
            if (Type == 0)
                adapterT.NotifyDataSetChanged();
            else if (Type == 1)
                adapterO.NotifyDataSetChanged();
            if (dotCode == 0)
                return;

            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle("Вкажіть кількість");
            EditText input = new EditText(this);
            input.SetRawInputType(InputTypes.ClassNumber);
            alert.SetView(input);

            alert.SetPositiveButton("Ок", (senderAlert, args) => {
                try
                {
                    int rest = int.Parse(input.Text);


                    if (Type == 0)
                    {
                        int code = adapterT.GetTaraRest(position).TaraCode;
                        db.AddOrUpdateTaraRest(this, klientCode, dotCode, code, rest);
                    }
                    else if (Type == 1)
                    {
                        string code = adapterO.GetOborudRest(position).OborudCode;
                        db.AddOrUpdateOborudRest(this, klientCode, dotCode, code, rest);
                    }

                    GetDetailing(klientCode, dotCode, Type);
                    Snackbar.Make(this.KlientDotName, "Кількість змінено", Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }
                catch
                {
                    Snackbar.Make(this.KlientDotName, "Помилка, спробуйте іще раз...", Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }
            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}