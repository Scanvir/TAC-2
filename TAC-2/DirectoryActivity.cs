using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAC_2
{
    [Activity(Label = "Группы товаров", Theme = "@style/AppTheme.NoActionBar")]
    public class DirectoryActivity : Activity, ListView.IOnItemClickListener
    {
        private DBHelper db;
        private ListView lv;
        private ListGoodsDirectoryAdapter adapter;

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            int dirCode = int.Parse(parent.GetItemIdAtPosition(position).ToString());
            string dirName = parent.GetItemAtPosition(position).ToString();

            Intent intent = new Intent();
            intent.PutExtra("dirCode", dirCode);
            intent.PutExtra("dirName", dirName);
            SetResult(Result.Ok, intent);
            Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.goodsDirectory);

            db = new DBHelper(this);

            lv = FindViewById<ListView>(Resource.Id.myList);
            lv.FastScrollEnabled = true;
            lv.OnItemClickListener = this;
        }
        protected override void OnResume()
        {
            base.OnResume();
            adapter = new ListGoodsDirectoryAdapter(db.GetGoodsDirectory(this), this);
            lv.Adapter = adapter;
        }
    }
}