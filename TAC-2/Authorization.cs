﻿using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Widget;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.SimpleAudioPlayer;
using System;
using System.Net.Http;
using Xamarin.Essentials;

namespace TAC_2
{
    [Activity(Label = "Авторизація", Theme = "@style/AppTheme")]
    public class Authorization : AppCompatActivity
    {
        private ISimpleAudioPlayer beep;
        private ISimpleAudioPlayer error;

        private Button btnOk;
        private EditText authCode;
        private string oldAuthCode;
        private TextView info;

        private DBHelper db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.authorization);
            
            db = new DBHelper(this);
            Auth auth = db.GetAuth(this);
            
            oldAuthCode = auth.Code;

            authCode = FindViewById<EditText>(Resource.Id.authCode);
            info = FindViewById<TextView>(Resource.Id.info);

            authCode.Text = auth.Code;
            info.Text = auth.Name;

            btnOk = FindViewById<Button>(Resource.Id.btnOk);
            btnOk.Click += delegate {
                BtnOkClick();
            };
            
            var names = this.GetType().Assembly.GetManifestResourceNames();

            beep = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            beep.Load(GetType().Assembly.GetManifestResourceStream("TAC_2.beep.mp3"));

            error = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            error.Load(GetType().Assembly.GetManifestResourceStream("TAC_2.error.mp3"));

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Internet) != (int)Permission.Granted)
               ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Internet }, 1);

        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        private void BtnOkClick()
        {
            Auth();
        }
        private async void Auth()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                info.Text = "немає інтернета";
                error.Play();
                return;
            }

            string serverIP;

            if (await CrossConnectivity.Current.IsRemoteReachable("79.143.40.187"))
                serverIP = "79.143.40.187";
            else if (await CrossConnectivity.Current.IsRemoteReachable("212.113.44.30"))
                serverIP = "212.113.44.30";
            else
            {
                info.Text = "немає зв`язку з сервером Psheni4niy";
                error.Play();
                return;
            }
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync("http://" + serverIP + ":1221/?GetAuth=" + authCode.Text + "=" + AppInfo.Version.ToString());

                Auth auth = JsonConvert.DeserializeObject<Auth>(response);

                if (auth.TAC == 1 && auth.Type >= 1)
                {
                    db.UpdateAuth(this, auth);
                    beep.Play();
                    if (oldAuthCode != auth.Code)
                    {
                        db.Delete(this);
                        oldAuthCode = auth.Code;
                    }
                    Finish();
                } else {
                    db.UpdateAuth(this, auth);
                    info.Text = "У вас немає права працювати з програмою ТАС!";
                    error.Play();
                    db.Delete(this);
                    return;
                }
            } catch (Exception ex)
            {
                error.Play();
                info.Text = "Помилка: " + ex.Message;
                return;
            }
        }
    }
}