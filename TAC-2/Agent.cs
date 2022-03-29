using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Tabs;

namespace TAC_2
{
    [Activity(Label = "Тара", Theme = "@style/AppTheme.NoActionBar")]
    class Agent : AndroidX.AppCompat.App.AppCompatActivity, Android.Views.View.IOnClickListener, ListView.IOnItemClickListener
    {
        private DBHelper db;
        private TabLayout tabLayout1;

        private ListView lv;
        private ListZakazAdapter zakazAdapter;

        private int Type = 0;

        private int klientCode;
        private int dotCode = 0;

        private Klient klient;

        private TextView KlientDotName;
        private TextView notKlientDotName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.agent);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            
            db = new DBHelper(this);
            
            klientCode = Intent.GetIntExtra("KlientCode", 0);
            klient = db.GetKlient(this, klientCode);

            toolbar.Title = klient.KlientName;
            SetSupportActionBar(toolbar);

            KlientDotName = FindViewById<TextView>(Resource.Id.KlientDotName);
            KlientDotName.Text = "Оберіть адресу...";
            KlientDotName.SetOnClickListener(this);

            notKlientDotName = FindViewById<TextView>(Resource.Id.notKlientDotName);
            notKlientDotName.SetOnClickListener(this);

            lv = FindViewById<ListView>(Resource.Id.listAgent);
            lv.FastScrollEnabled = true;
            lv.OnItemClickListener = this;

            tabLayout1 = FindViewById<TabLayout>(Resource.Id.tabLayout1);
            tabLayout1.TabSelected += (object sender, TabLayout.TabSelectedEventArgs e) =>
            {
                var tab = e.Tab;
                if (tab.Text == "Борги")
                    Type = 0;
                else if (tab.Text == "ПКО")
                    Type = 1;
                else
                    Type = 2;
                Refresh();
            };
            
        }
        protected override void OnResume()
        {
            base.OnResume();
            Refresh();
        }
        private void Refresh()
        {
            if (Type == 0)
                GetDebet();
            else if (Type == 1)
                GetPKO();
            else if (Type == 2)
                GetZakaz();
        }
        
        private void GetPKO()
        {
            ListPKO adapter = new ListPKO(this, db.GetPKOList(this, klientCode, dotCode), 1);
            lv.Adapter = adapter;
        }
        private void GetDebet()
        {
            ListDebet adapter = new ListDebet(this, db.GetDebetList(this, klientCode, dotCode), 1);
            lv.Adapter = adapter;
        }
        private void GetZakaz()
        {
            zakazAdapter = new ListZakazAdapter(this, db.GetZakazList(this, klientCode, dotCode));
            lv.Adapter = zakazAdapter;
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_agent, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.addPKO)
            {
                var intent = new Intent(this, typeof(PKOActivity));
                intent.PutExtra("KlientCode", klientCode);
                StartActivityForResult(intent, 100);
                return true;
            }
            else if (id == Resource.Id.addZakaz)
            {
                var intent = new Intent(this, typeof(ZakazActivity));
                intent.PutExtra("KlientCode", klientCode);
                intent.PutExtra("newZakaz", true);
                StartActivityForResult(intent, 100);
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
        public void OnClick(View v)
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
                Refresh();
            }
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (data == null) { return; }
            dotCode = data.GetIntExtra("dotCode", 0);
            KlientDotName.Text = data.GetStringExtra("dotName");
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            try
            {
                if (Type == 2)
                {
                    var intent = new Intent(this, typeof(ZakazActivity));
                    intent.PutExtra("KlientCode", klientCode);
                    intent.PutExtra("newZakaz", false);
                    intent.PutExtra("GUID", zakazAdapter.GetZakaz(position).GUID);
                    StartActivityForResult(intent, 100);
                }
            }
            catch { }
        }
    }
}