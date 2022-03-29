using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TAC_2
{
    [Activity(Label = "Журнал ПКО", Theme = "@style/AppTheme.NoActionBar")]
    public class PKOJourn : AppCompatActivity, AdapterView.IOnItemClickListener
    {
        private DBHelper db;
        private EditText inputSearch;
        private ListPKOjournAdapter adapter;
        private ListView lv;
        private DateTime date;
        private PKO pko;
        private Android.Support.V7.Widget.Toolbar toolbar;
        private NumberFormatInfo nfi = new NumberFormatInfo();
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.pkoJourn);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarPKOjourn);

            db = new DBHelper(this);

            date = DateTime.Now;

            inputSearch = FindViewById<EditText>(Resource.Id.datePKO);
            inputSearch.Text = date.ToString("yyyy-MM-dd");
            inputSearch.TextChanged += OnFilterTextChanged;

            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalSeparator = ".";

            SetSupportActionBar(toolbar);
        }
        protected override void OnResume()
        {
            base.OnResume();
            GetDocs();
        }
        private void GetDocs()
        {
            lv = FindViewById<ListView>(Resource.Id.listPKOjourn);
            lv.FastScrollEnabled = true;
            lv.OnItemClickListener = this;

            adapter = new ListPKOjournAdapter(this, db.GetPKOJourn(this, date), date);
            lv.Adapter = adapter;

            toolbar.Title = string.Format("Сумма: {0}", adapter.GetSumm().ToString("N2", nfi));
        }
        private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                date = DateTime.ParseExact(inputSearch.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                GetDocs();
            }
            catch {
                
            }
        }
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            pko = adapter.GetDoc(position);
            if (pko.Status == 0)
            {
                PopupMenu menu = new PopupMenu(this, view);
                menu.MenuInflater.Inflate(Resource.Menu.DocMenu, menu.Menu);
                menu.MenuItemClick += (s, args) =>
                {
                    if (args.Item.ItemId == Resource.Id.del)
                    {
                        db.DelPKO(this, pko.GUID);
                        GetDocs();
                    }
                };
                menu.Show();
            } else
            {
                Toast.MakeText(this, "Документ вже відправлений", ToastLength.Long).Show();
            }
        }
    }
}