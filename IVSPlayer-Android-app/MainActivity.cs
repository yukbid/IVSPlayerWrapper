using System.Timers;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.App;
using Com.Soemarko.Ivsplayerwrapper;
using Java.Lang;

namespace IVSPlayer_Android_app
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IVSPlayerWrapperCallback
    {
        IVSPlayerWrapper player = null;
        private static Timer aTimer;
        TextView textView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            //SetContentView(Resource.Layout.activity_main);

            player = new IVSPlayerWrapper(this, this);
            player.OnCreate(savedInstanceState);

            player.Load("https://fcc3ddae59ed.us-west-2.playback.live-video.net/api/video/v1/us-west-2.893648527354.channel.YtnrVcQbttF0.m3u8");

            RelativeLayout relativeLayout = new RelativeLayout(this);

            relativeLayout.AddView(player.View);

            SetContentView(relativeLayout);

            // optionals
            RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(Resources.DisplayMetrics.WidthPixels, 200);
            layoutParams.AddRule(LayoutRules.AlignParentTop);
            textView = new TextView(this);
            textView.LayoutParameters = layoutParams;
            textView.SetTextColor(Android.Graphics.Color.Orange);
            textView.SetTextSize(ComplexUnitType.Pt, 10);
            textView.TextAlignment = Android.Views.TextAlignment.ViewEnd;

            relativeLayout.AddView(textView);

            SetTimer();
        }

        private void SetTimer()
        {
            aTimer = new Timer(5000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                Log.Info("ivsplayer", e.SignalTime.ToString() + "> latency: " + player.LiveLatency.ToString());
                textView.Text = "latency: " + player.LiveLatency.ToString();
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void IVSPlayerWrapperCallback.OnError(RuntimeException p0)
        {
            throw p0;
        }

        void IVSPlayerWrapperCallback.OnRebuffering()
        {

        }

        void IVSPlayerWrapperCallback.OnStateChanged(string p0)
        {
            Log.Info("ivsplayer", p0);
            if (p0 == "READY") player.Play();
        }
    }
}
