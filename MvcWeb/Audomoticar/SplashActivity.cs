using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.Animations;

namespace Audomoticar
{
    [Activity(Label = "Audomoticar", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {
        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            Window window = this.Window;
            window.SetFormat(Android.Graphics.Format.Rgb888);
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            RequestWindowFeature(WindowFeatures.NoTitle);
            this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            SetContentView(Resource.Layout.Splash);

            CountDown _tik;
            _tik = new CountDown(4000, 1000, this, typeof(LoginActivity));// It delay the screen for 1 second and after that switch to YourNextActivity
            _tik.Start();

            StartAnimations();
        }

        private void StartAnimations()
        {
            Animation anim = AnimationUtils.LoadAnimation(this, Resource.Animation.alpha);
            anim.Reset();
            LinearLayout l = FindViewById<LinearLayout>(Resource.Id.lin_lay);
            l.ClearAnimation();
            l.StartAnimation(anim);

            anim = AnimationUtils.LoadAnimation(this, Resource.Animation.translate);
            anim.Reset();
            ImageView iv = FindViewById<ImageView>(Resource.Id.logo);
            iv.ClearAnimation();
            iv.StartAnimation(anim);

        }
    }
}