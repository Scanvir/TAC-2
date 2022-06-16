using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace TAC_2
{
    [Activity(Label = "Товари", Theme = "@style/AppTheme.NoActionBar")]
    public class GoodsActivity : Activity, ListView.IOnItemClickListener, Android.Views.View.IOnClickListener
    {
        private EditText inputSearch;
        private ListGoodAdapter adapter;
        private TextView directory;
        private ListView lv;
        private DBHelper db;
        private string GUID;
        private static long back_pressed;
        private int dirCode;
        private string fillial;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.goods);

            db = new DBHelper(this);

            lv = FindViewById<ListView>(Resource.Id.listGoods);
            lv.FastScrollEnabled = true;
            lv.OnItemClickListener = this;

            inputSearch = FindViewById<EditText>(Resource.Id.inputSearch);
            inputSearch.TextChanged += OnFilterTextChanged;

            directory = FindViewById<TextView>(Resource.Id.Directory);
            directory.SetOnClickListener(this);

            GUID = Intent.GetStringExtra("GUID");
            fillial = Intent.GetStringExtra("fillial");
        }
        protected override void OnResume()
        {
            base.OnResume();
            adapter = new ListGoodAdapter(this, db.GetPriceList(this, "", dirCode, fillial), GUID);
            lv.Adapter = adapter;
        }
        private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextUtils.IsEmpty(inputSearch.Text))
            {
                adapter = new ListGoodAdapter(this, db.GetPriceList(this, "", dirCode, fillial), GUID);
                lv.Adapter = adapter;
            }
            else
            {
                adapter = new ListGoodAdapter(this, db.GetPriceList(this, inputSearch.Text, dirCode, fillial), GUID);
                lv.Adapter = adapter;
            }
        }
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            Price price = adapter.GetPrice(position);
            if (price.Quantity > 0) {
                Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
                alert.SetTitle("Вкажіть кількість");
                EditText input = new EditText(this);
                input.SetRawInputType(InputTypes.ClassNumber);
                alert.SetView(input);

                alert.SetPositiveButton("Ок", (senderAlert, args) => {
                    try
                    {
                        int quantity = int.Parse(input.Text);
                        double summa = quantity * price.PriceUAH;
                        OrderTab tab = new OrderTab()
                        {
                            GUID = GUID,
                            GoodCode = adapter.GetPrice(position).GoodCode,
                            Quantity = quantity,
                            PriceUAH = price.PriceUAH,
                            Summ = summa
                        };
                        db.AddOrderTab(this, tab);
                        Snackbar.Make(this.inputSearch, "Товар додано в заказ", Snackbar.LengthLong).SetAction("Action", (View.IOnClickListener)null).Show();
                    } catch
                    {
                        Snackbar.Make(this.inputSearch, "Помилка, спробуйте іще раз...", Snackbar.LengthLong).SetAction("Action", (View.IOnClickListener)null).Show();
                    }
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            } else {
                return;
            }
        }
        
        public override void OnBackPressed()
        {
            if (back_pressed + 2000 > Java.Lang.JavaSystem.CurrentTimeMillis())
            {
                base.OnBackPressed();
            }
            else
            {
                Snackbar.Make(this.inputSearch, "Нажмите еще раз для выхода!", Snackbar.LengthLong).SetAction("Action", (View.IOnClickListener)null).Show();
            }
            back_pressed = Java.Lang.JavaSystem.CurrentTimeMillis();
        }

        public void OnClick(View v)
        {
            var intent = new Intent(this, type: typeof(DirectoryActivity));
            intent.PutExtra("dirCode", dirCode);
            StartActivityForResult(intent, 100);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (data == null) { return; }
            dirCode = data.GetIntExtra("dirCode", 0);
            directory.Text = data.GetStringExtra("dirName");
        }
    }
}