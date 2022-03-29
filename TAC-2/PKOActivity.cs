using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace TAC_2
{
    [Activity(Label = "PKO", Theme = "@style/AppTheme.NoActionBar")]
    class PKOActivity : AndroidX.AppCompat.App.AppCompatActivity
    {
        private DBHelper db;
        private int klientCode;
        private Klient klient;
        private ListDebet adapter;
        private Spinner spinner;
        private TextView summ;
        private Debet debet;
        private Android.Widget.Button btnPKOSave;
        
        private NumberStyles style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
        private CultureInfo culture = CultureInfo.InvariantCulture;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.pko);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.PKOtoolbar);

            db = new DBHelper(this);

            klientCode = Intent.GetIntExtra("KlientCode", 0);
            klient = db.GetKlient(this, klientCode);

            toolbar.Title = klient.KlientName;
            SetSupportActionBar(toolbar);

            summ = FindViewById<TextView>(Resource.Id.summPKO);

            adapter = new ListDebet(this, db.GetDebetList(this, klientCode, 0), 0);

            spinner = FindViewById<Spinner>(Resource.Id.docPKO);
            spinner.Adapter = adapter;
            spinner.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                summ.Text = adapter.GetDoc(e.Position).Dolg.ToString();
                debet = adapter.GetDoc(e.Position);
            };
            spinner.SetSelection(0);

            btnPKOSave = FindViewById<Android.Widget.Button>(Resource.Id.btnPKOSave);
            btnPKOSave.Click += delegate { SavePKO(); }; ;
        }
        private void SavePKO()
        {
            if (double.TryParse(summ.Text.Replace(",", "."), style, culture, out double Summ))
            {
                Summ = Math.Abs(Summ);

                PKO pko = new PKO()
                {
                    GUID = Guid.NewGuid().ToString(),
                    DateDoc = debet.DateDoc,
                    NumDoc = debet.NumDoc,
                    KlientCode = debet.KlientCode,
                    DotCode = debet.DotCode,
                    Summ = Summ,
                    DatePay = DateTime.Now.ToString("yyyy-MM-dd"),
                    Status = 0
                };
                db.AddPKO(this, pko);
                Finish();
            }
        }
    }
}