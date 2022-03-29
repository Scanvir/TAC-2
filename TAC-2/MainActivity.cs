using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace TAC_2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener, ListView.IOnItemClickListener
    {
        private ISimpleAudioPlayer beep;
        private ISimpleAudioPlayer error;

        private DBHelper db;
        private TextView userName;
        private EditText inputSearch;
        private ListView lv;
        private ListKlientAdapter adapter;
        private TextView updateInfo;

        private Auth auth;
        public string tag = "TAC_2";
        private int activeClientCode = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            db = new DBHelper(this);
            auth = db.GetAuth(this);

            beep = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            beep.Load(GetType().Assembly.GetManifestResourceStream("TAC_2.beep.mp3"));

            error = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            error.Load(GetType().Assembly.GetManifestResourceStream("TAC_2.error.mp3"));

            inputSearch = FindViewById<EditText>(Resource.Id.inputSearch);
            inputSearch.TextChanged += OnFilterTextChanged;

            updateInfo = FindViewById<TextView>(Resource.Id.UpdateInfo);
            //updateInfo.MovementMethod = LinkMovementMethod.Instance;


            lv = FindViewById<ListView>(Resource.Id.myList);
            lv.TextFilterEnabled = true;
            /*
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
            */

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();
        }

        protected override void OnResume()
        {
            base.OnResume();
            auth = db.GetAuth(this);

            UpdateKlients();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            View v = navigationView.GetHeaderView(0);
            userName = v.FindViewById<TextView>(Resource.Id.UserName);
            userName.Text = auth.Name;

            string version = AppInfo.Version.ToString();
            if (auth.Version == version)
            {
                updateInfo.Text = "";
                updateInfo.SetBackgroundColor(Android.Graphics.Color.ParseColor("#7b1fa2"));
            }
            else
            {
                updateInfo.Text = @"Оновіть програму https://1drv.ms/u/s!Apsk8mmpNEKlgZdke15ZrF2vSG5Vyg?e=430VEL";
                updateInfo.SetBackgroundColor(Android.Graphics.Color.ParseColor("#ff8000"));
            }
        }
        private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextUtils.IsEmpty(inputSearch.Text))
            {
                adapter = new ListKlientAdapter(this, db.GetKlientDebetList(this, ""), auth.Type);
                lv.Adapter = adapter;
            }
            else
            {
                adapter = new ListKlientAdapter(this, db.GetKlientDebetList(this, inputSearch.Text), auth.Type);
                lv.Adapter = adapter;
            }

        }
        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }
        private void UpdateKlients()
        {
            if (auth.Type == 1)
            {
                adapter = new ListKlientAdapter(this, db.GetKlientList(this, ""), auth.Type);
            }
            else if (auth.Type == 2)
            {
                adapter = new ListKlientAdapter(this, db.GetKlientDebetList(this, ""), auth.Type);
            }
            lv.Adapter = adapter;
            lv.FastScrollEnabled = true;
            lv.OnItemClickListener = this;
            if (activeClientCode != 0)
            {
                lv.SetSelection(adapter.GetPosition(activeClientCode));
            }
        }
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            activeClientCode = int.Parse(parent.GetItemIdAtPosition(position).ToString());
            if (auth.Type == 1)
            {
                var intent = new Intent(this, typeof(Detailing));
                intent.PutExtra("KlientCode", activeClientCode);
                StartActivityForResult(intent, 100);
            }
            if (auth.Type == 2)
            {
                PopupMenu menu = new PopupMenu(this, view);
                menu.MenuInflater.Inflate(Resource.Menu.AgentMenu, menu.Menu);
                menu.MenuItemClick += (s, args) =>
                {
                    if (args.Item.ItemId == Resource.Id.agent)
                    {
                        var intent = new Intent(this, typeof(Agent));
                        intent.PutExtra("KlientCode", activeClientCode);
                        StartActivityForResult(intent, 100);
                    }
                    else if (args.Item.ItemId == Resource.Id.detailing)
                    {
                        var intent = new Intent(this, typeof(Detailing));
                        intent.PutExtra("KlientCode", activeClientCode);
                        StartActivityForResult(intent, 100);

                    }

                };
                menu.Show();
            }

        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                var intent = new Intent(this, typeof(Authorization));
                StartActivityForResult(intent, 100);
                return true;
            }
            else if (id == Resource.Id.action_exit)
            {
                this.Finish();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_update)
            {
                var intent = new Intent(this, typeof(LogActivity));
                intent.PutExtra("type", "update");
                StartActivityForResult(intent, 100);
            }
            else if (id == Resource.Id.nav_export)
            {
                var intent = new Intent(this, typeof(LogActivity));
                intent.PutExtra("type", "export");
                StartActivityForResult(intent, 100);
            }
            else if (id == Resource.Id.nav_import)
            {
                var intent = new Intent(this, typeof(LogActivity));
                intent.PutExtra("type", "import");
                StartActivityForResult(intent, 100);
            }
            else if (id == Resource.Id.nav_PKOJourn)
            {
                var intent = new Intent(this, typeof(PKOJourn));
                StartActivityForResult(intent, 100);
                return true;
            }
            else if (id == Resource.Id.nav_OrderJourn)
            {
                var intent = new Intent(this, typeof(OrderJourn));
                StartActivityForResult(intent, 100);
                return true;
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public interface IDatabaseHandler
        {
            void UpdateAuth(Context context, Auth auth);
            Auth GetAuth(Context context);
            List<Klient> GetKlientList(Context context, string searchText);
            List<Klient> GetKlientDebetList(Context context, string searchText);
        }
    }
}

