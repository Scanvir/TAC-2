using Android.Animation;
using Android.App;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.SimpleAudioPlayer;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace TAC_2
{
    [Activity(Label = "Протокол", Theme = "@style/AppTheme.NoActionBar")]
    public class LogActivity : Activity
    {
        private string type;
        private TextView log;
        private ISimpleAudioPlayer beep;
        private ISimpleAudioPlayer error;
        private ProgressBar progressBar;
        private ObjectAnimator animator;
        private DBHelper db;
        private Auth auth;
        private string serverIP;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.log);

            type = Intent.GetStringExtra("type");

            log = FindViewById<TextView>(Resource.Id.log);
            log.Text = "Оновлення розпочато\n";

            beep = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            beep.Load(GetType().Assembly.GetManifestResourceStream("TAC_2.beep.mp3"));

            error = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            error.Load(GetType().Assembly.GetManifestResourceStream("TAC_2.error.mp3"));

            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            progressBar.Max = 100;
            progressBar.Progress = 0;

            animator = ObjectAnimator.OfInt(progressBar, "progress", 0, 100);
            animator.SetInterpolator(null);
            animator.RepeatCount = 100;
            animator.RepeatMode = ValueAnimatorRepeatMode.Restart;
            animator.SetDuration(1000);

            db = new DBHelper(this);
            auth = db.GetAuth(this);

            if (type == "update")
            {
                await Update();
                await Export();
            } else if (type == "export")
                await Export();
            else if (type == "import")
                await Import ();

            log.Text += "==================================\n";
            log.Text += "Оновлення завершено\n";
            log.Text += "Можете закрити це вікно\n";
        }
        private async Task<int> Update()
        {
            log.Text += "==================================\n";
            await DetectIP();

            if (string.IsNullOrEmpty(serverIP))
                return 0;

            animator.Start();

            try
            {
                log.Text += "Запит на повне оновлення...\n";

                var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync("http://" + serverIP + ":1221/?GetUpdate=" + auth.Code + "=" + AppInfo.Version.ToString());

                log.Text += "Відповідь отримано...\n";

                Update update = JsonConvert.DeserializeObject<Update>(response);

                if (update.Auth.TAC == 1 && update.Auth.Type >= 1)
                {
                    log.Text += "Ви маєте доступ до програми\n";
                    db.UpdateAuth(this, update.Auth);
                    log.Text += "Розпочато оновлення даних\n";
                    db.UpdateDatabase(this, update, log);
                    log.Text += "Завершено оновлення даних\n";
                    beep.Play();
                }
                else
                {
                    log.Text += "У вас немає доступа до програми!\n";
                    db.UpdateAuth(this, update.Auth);
                    db.Delete(this);
                    error.Play();
                }
                animator.Cancel();
                progressBar.Progress = 0;
                return 1;
            }
            catch (Exception ex)
            {
                log.Text += "Виникла помилка: " + ex.Message + "\n";
                error.Play();
                animator.Cancel();
                progressBar.Progress = 0;
                return 0;
            }
        }
        private async Task<int> Import()
        {
            log.Text += "==================================\n";
            await DetectIP();

            if (string.IsNullOrEmpty(serverIP))
                return 0;

            animator.Start();

            try
            {
                log.Text += "Запит на завантаження документів з архіву...\n";

                var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync("http://" + serverIP + ":1221/?GetDocs=" + auth.Code + "=" + AppInfo.Version.ToString());

                Update update = JsonConvert.DeserializeObject<Update>(response);

                if (update.Auth.TAC == 1 && update.Auth.Type >= 1)
                {
                    log.Text += "Ви маєте доступ до програми\n";
                    db.UpdateAuth(this, update.Auth);
                    log.Text += "Розпочато оновлення даних\n";
                    db.UpdateDatabase(this, update, log);
                    log.Text += "Завершено оновлення даних\n";
                    beep.Play();
                }
                else
                {
                    log.Text += "У вас немає доступа до програми!\n";
                    db.UpdateAuth(this, update.Auth);
                    db.Delete(this);
                    error.Play();
                }
                animator.Cancel();
                progressBar.Progress = 0;
                return 1;
            }
            catch (Exception ex)
            {
                log.Text += "Виникла помилка: " + ex.Message + "\n";
                error.Play();
                animator.Cancel();
                progressBar.Progress = 0;
                return 0;
            }
        }
        private async Task<int> Export()
        {
            log.Text += "==================================\n";
            await DetectIP ();

            if (string.IsNullOrEmpty(serverIP))
                return 0;

            animator.Start();

            try
            {
                log.Text += "Запит на відвантаження документів...\n";

                Update update = db.GetDocForExport(this);
                update.Auth = auth;
                update.Version = AppInfo.Version.ToString();

                string json = JsonConvert.SerializeObject(update);

                json = await PostCallAPI("http://" + serverIP + ":1221/ExportDocs", json);

                log.Text += "Відповідь отримано...\n";

                update = JsonConvert.DeserializeObject<Update>(json);

                log.Text += "Розпочато оновлення даних\n";
                db.UpdateDatabase(this, update, log);
                log.Text += "Завершено оновлення даних\n";

                beep.Play();
                animator.Cancel();
                progressBar.Progress = 0;
                return 1;
            }
            catch (Exception ex)
            {
                log.Text += "Виникла помилка: " + ex.Message + "\n";
                error.Play();
                animator.Cancel();
                progressBar.Progress = 0;
                return 0;
            }
        }
        private async Task<int> DetectIP()
        {
            serverIP = "";
            if (!CrossConnectivity.Current.IsConnected)
            {
                log.Text += "Інтернет відсутній!\n";
                error.Play();
                return 0;
            }

            if (await CrossConnectivity.Current.IsRemoteReachable("79.143.40.187"))
                serverIP = "79.143.40.187";
            else if (await CrossConnectivity.Current.IsRemoteReachable("212.113.44.30"))
                serverIP = "212.113.44.30";
            else
            {
                log.Text += "IP адреса сервера не визначена!\n";
                error.Play();
                return 0;
            }
            log.Text += "IP адреса сервера: " + serverIP + "\n";
            return 1;
        }
        public static async Task<string> PostCallAPI(string url, string json)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(content: json, encoding: Encoding.UTF8, mediaType: "application/json");
                    var response = await client.PostAsync(url, content);
                    if (response != null)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        }
}