using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace TAC_2
{
    [Activity(Label = "Точки", Theme = "@style/AppTheme.NoActionBar")]
    public class DotsActivity : Activity, ListView.IOnItemClickListener
    {
        private DBHelper db;
        private int klientCode;
        private EditText inputSearch;
        private ListDotAdapter adapter;
        private ListView lv;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);

            db = new DBHelper(this);
            klientCode = Intent.GetIntExtra("KlientCode", 0);

            inputSearch = FindViewById<EditText>(Resource.Id.inputSearch);
            inputSearch.TextChanged += OnFilterTextChanged;
        }

        protected override void OnResume()
        {
            base.OnResume();
            UpdateDots();
        }

        private void UpdateDots()
        {
            adapter = new ListDotAdapter(this, db.GetDotList(this, klientCode, ""), klientCode.ToString());

            lv = FindViewById<ListView>(Resource.Id.myList);
            lv.Adapter = adapter;
            lv.FastScrollEnabled = true;
            lv.OnItemClickListener = this;
        }
        private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextUtils.IsEmpty(inputSearch.Text))
            {
                adapter = new ListDotAdapter(this, db.GetDotList(this, klientCode, ""), klientCode.ToString());
                lv.Adapter = adapter;
            }
            else
            {
                adapter = new ListDotAdapter(this, db.GetDotList(this, klientCode, inputSearch.Text), klientCode.ToString());
                lv.Adapter = adapter;
            }

        }
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            int dotCode = int.Parse(parent.GetItemIdAtPosition(position).ToString());
            string dotName = parent.GetItemAtPosition(position).ToString();

            Intent intent = new Intent();
            intent.PutExtra("dotCode", dotCode);
            intent.PutExtra("dotName", dotName);
            SetResult(Result.Ok, intent);
            Finish();
        }
    }
}