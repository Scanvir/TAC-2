using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TAC_2
{
    [Activity(Label = "PKO", Theme = "@style/AppTheme.NoActionBar")]
    public class ZakazActivity : AndroidX.AppCompat.App.AppCompatActivity, ListView.IOnItemClickListener
    {
        private DBHelper db;
        private int klientCode;
        private int dotCode;
        private Klient klient;
        private ListDotAdapter dotAdapter;
        private Spinner spinner;
        private Button btnZakazSave;
        private Button btnZakazDelete;
        private EditText Comment;
        private CheckBox CheckBoxA;
        private CheckBox CheckBoxF;
        private TextView OrderSumm;
        private string GUID;
        private static long back_pressed;
        private bool newZakaz;
        private AndroidX.AppCompat.Widget.Toolbar toolbar;
        private RadioGroup formGroup;
        private int Status;

        private ListZakazTabAdapter adapter;
        private ListView lv;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.zakaz);

            toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.ZakazToolbar);

            db = new DBHelper(this);

            klientCode = Intent.GetIntExtra("KlientCode", 0);
            klient = db.GetKlient(this, klientCode);

            newZakaz = Intent.GetBooleanExtra("newZakaz", false);
            if (newZakaz)
                GUID = Guid.NewGuid().ToString();
            else
                GUID = Intent.GetStringExtra("GUID");

            toolbar.Title = klient.KlientName;
            SetSupportActionBar(toolbar);

            dotAdapter = new ListDotAdapter(this, db.GetDotList(this, klientCode, ""), klientCode.ToString());

            spinner = FindViewById<Spinner>(Resource.Id.dotZak);
            spinner.Adapter = dotAdapter;
            spinner.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                dotCode = dotAdapter.GetDot(e.Position).DotCode;
            };
            
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            btnZakazSave = FindViewById<Button>(Resource.Id.btnZakazSave);
            btnZakazSave.Click += delegate { SaveOrder(); }; ;

            btnZakazDelete = FindViewById<Button>(Resource.Id.btnZakazDelete);
            btnZakazDelete.Click += delegate { DeleteOrder(); }; ;

            Comment = FindViewById<EditText>(Resource.Id.comment);
            CheckBoxA = FindViewById<CheckBox>(Resource.Id.checkBoxA);
            CheckBoxF = FindViewById<CheckBox>(Resource.Id.checkBoxF);
            OrderSumm = FindViewById<TextView>(Resource.Id.Summ);
            formGroup = FindViewById<RadioGroup>(Resource.Id.formGroup);

            lv = FindViewById<ListView>(Resource.Id.listZak);
            lv.FastScrollEnabled = true;
            lv.OnItemClickListener = this;

            if (newZakaz)
            {
                spinner.SetSelection(0);
                Status = 0;
                btnZakazSave.Visibility = ViewStates.Visible;
                btnZakazDelete.Visibility = ViewStates.Invisible;
                formGroup.Check(Resource.Id.form2);
            } else
            {
                Order order = db.GetOrder(this, GUID);
                Status = order.Status;
                if (order.Status > 0)
                {
                    btnZakazSave.Visibility = ViewStates.Invisible;
                    btnZakazDelete.Visibility = ViewStates.Invisible;
                    fab.Visibility = ViewStates.Invisible;
                }
                if (order.FlagA == 1)
                    CheckBoxA.Checked = true;
                if (order.FlagF == 1)
                    CheckBoxF.Checked = true;
                Comment.Text = order.Comment;
                if (order.Form == 1)
                    formGroup.Check(Resource.Id.form1);
                else
                    formGroup.Check(Resource.Id.form2);

                spinner.SetSelection(dotAdapter.GetPosition(order.DotCode));
            }
        }
        protected override void OnResume()
        {
            base.OnResume();
            GetDocTab();
        }
        private void GetDocTab()
        {
            var nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalSeparator = ".";

            adapter = new ListZakazTabAdapter(this, db.GetZakazTabList(this, GUID));
            lv.Adapter = adapter;
            OrderSumm.Text = adapter.GetSumm().ToString("N2", nfi);
        }
        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            var intent = new Intent(this, typeof(GoodsActivity));
            intent.PutExtra("GUID", GUID);
            StartActivityForResult(intent, 100);
        }

        private void SaveOrder()
        {
            List<OrderTab> orderTab = db.GetZakazTabList(this, GUID);
            if (orderTab.Count == 0)
            {
                Finish();
                return;
            }

            int FlagA = 0;
            int FlagF = 0;
            if (CheckBoxA.Checked)
                FlagA = 1;
            if (CheckBoxF.Checked)
                FlagF = 1;
            int Form = 2;
            if (formGroup.CheckedRadioButtonId == Resource.Id.form1)
                Form = 1;
            Order order = new Order()
            {
                DateDoc = DateTime.Now,
                GUID = GUID,
                KlientCode = klientCode,
                DotCode = dotCode,
                FlagA = FlagA,
                FlagF = FlagF,
                Comment = Comment.Text,
                Status = 0,
                Form = Form,
                OrderTab = orderTab
            };
            db.AddOrder(this, order);
            Finish();
        }
        private void DeleteOrder()
        {
            db.DelOrder(this, GUID);
            Finish();
        }
        public override void OnBackPressed()
        {
            if (back_pressed + 2000 > Java.Lang.JavaSystem.CurrentTimeMillis())
            {
                SaveOrder();
                base.OnBackPressed();
            }
            else
            {
                Snackbar.Make(this.btnZakazSave, "Нажмите еще раз для выхода!", Snackbar.LengthLong).SetAction("Action", (View.IOnClickListener)null).Show();
            }
            back_pressed = Java.Lang.JavaSystem.CurrentTimeMillis();
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            if (Status > 0)
                return;

            PopupMenu menu = new PopupMenu(this, view);
            menu.MenuInflater.Inflate(Resource.Menu.ZakazMenu, menu.Menu);
            menu.MenuItemClick += (s, args) =>
            {
                if (args.Item.ItemId == Resource.Id.delZakaz)
                {
                    db.DelZakazTab(this, GUID, adapter.GetZakazTab(position).GoodCode.ToString());
                    GetDocTab();
                    Snackbar.Make(this.btnZakazDelete, "Заказ видалено", Snackbar.LengthLong).SetAction("Action", (View.IOnClickListener)null).Show();
                } else if (args.Item.ItemId == Resource.Id.changeQuantity)
                {
                    Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
                    alert.SetTitle("Вкажіть кількість");
                    EditText input = new EditText(this);
                    input.SetRawInputType(InputTypes.ClassNumber);
                    alert.SetView(input);

                    alert.SetPositiveButton("Ок", (senderAlert, args) => {
                        try
                        {
                            int quantity = int.Parse(input.Text);
                            double price = adapter.GetZakazTab(position).PriceUAH;
                            double summa = quantity * price;
                            int goodCode = adapter.GetZakazTab(position).GoodCode;

                            OrderTab tab = new OrderTab()
                            {
                                GUID = GUID,
                                GoodCode = goodCode,
                                Quantity = quantity,
                                PriceUAH = price,
                                Summ = summa
                            };

                            db.AddOrderTab(this, tab);
                            GetDocTab();
                            Snackbar.Make(this.btnZakazDelete, "Кількість змінено", Snackbar.LengthLong).SetAction("Action", (View.IOnClickListener)null).Show();
                        }
                        catch
                        {
                            Snackbar.Make(this.btnZakazDelete, "Помилка, спробуйте іще раз...", Snackbar.LengthLong).SetAction("Action", (View.IOnClickListener)null).Show();
                        }
                    });

                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
            };
            menu.Show();
        }
    }
}