using Foundation;
using System;
using UIKit;
using IVSPlayer;
using System.Diagnostics;
using System.Timers;

namespace IVSPlayer_iOS_app
{
    public partial class ViewController : UIViewController, IIVSPlayerWrapperCallback
    {
        IVSPlayerWrapper player;
        Timer aTimer;
        UITextView textView;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            player = new IVSPlayerWrapper(this);
            player.View.Frame = View.Frame;

            View.AddSubview(player.View);

            player.LoadWithPathStr("https://fcc3ddae59ed.us-west-2.playback.live-video.net/api/video/v1/us-west-2.893648527354.channel.YtnrVcQbttF0.m3u8");

            // sample purposes
            textView = new UITextView(new CoreGraphics.CGRect(0, 40, View.Frame.Width, 80));
            textView.TextColor = UIColor.Orange;
            textView.TextAlignment = UITextAlignment.Right;
            textView.Font = UIFont.PreferredTitle2;
            textView.BackgroundColor = UIColor.Clear;
            View.AddSubview(textView);

            SetTimer();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
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
            BeginInvokeOnMainThread(() =>
            {
                Debug.WriteLine(e.SignalTime.ToString() + "> latency: " + player.LiveLatency.ToString());
                textView.Text = "latency: " + player.LiveLatency.ToString();
            });
        }

        public void DidChangeStateWithState(nint state)
        {
            switch (state)
            {
                case 0:
                    Debug.WriteLine("idle state");
                    break;
                case 1:
                    Debug.WriteLine("ready state");
                    player.Play();
                    break;
                case 2:
                    Debug.WriteLine("buffering state");
                    break;
                case 3:
                    Debug.WriteLine("playing state");
                    Debug.Write("Latency: ");
                    Debug.WriteLine(player.LiveLatency);
                    break;
                case 4:
                    Debug.WriteLine("ended state");
                    Process.GetCurrentProcess().Kill();
                    break;
                default:
                    Debug.WriteLine("unknown state");
                    break;
            }
        }

        public void didFailWithErrorWithError(NSError err)
        {
            Debug.WriteLine(err);
        }

        public void willRebuffer()
        {

        }

    }
}
