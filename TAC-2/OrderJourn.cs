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
    [Activity(Label = "Журнал Заказов", Theme = "@style/AppTheme.NoActionBar")]
    public class OrderJourn : AppCompatActivity, AdapterView.IOnItemClickListener
    {
        private DBHelper db;
        private EditText inputSearch;
        private ListOrderJournAdapter adapter;
        private ListView lv;
        private DateTime date;
        private Order zakaz;
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

            adapter = new ListOrderJournAdapter(this, db.GetOrderJourn(this, date), date);
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
            zakaz = adapter.GetDoc(position);
            
            PopupMenu menu = new PopupMenu(this, view);
            menu.MenuInflater.Inflate(Resource.Menu.DocMenu, menu.Menu);
            menu.MenuItemClick += (s, args) =>
            {
                if (args.Item.ItemId == Resource.Id.del)
                {
                    if (zakaz.Status == 0)
                    {
                        db.DelOrder(this, zakaz.GUID);
                        GetDocs();
                    } else
                        Toast.MakeText(this, "Документ вже відправлений", ToastLength.Long).Show();
                }
                else if (args.Item.ItemId == Resource.Id.open)
                {
                    var intent = new Intent(this, typeof(ZakazActivity));
                    intent.PutExtra("KlientCode", adapter.GetDoc(position).KlientCode);
                    intent.PutExtra("newZakaz", false);
                    intent.PutExtra("GUID", adapter.GetDoc(position).GUID);
                    StartActivityForResult(intent, 100);
                    GetDocs();
                }
            };
            menu.Show();
        }
    }
}