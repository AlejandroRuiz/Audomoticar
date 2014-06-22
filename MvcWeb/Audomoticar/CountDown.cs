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

namespace Audomoticar
{
    class CountDown : CountDownTimer
    {
        private Activity _act;
        private System.Type _actLaunch;

        public CountDown(long millisInFuture, long countDown, Activity act, System.Type actLaunch) :
            base(millisInFuture, countDown)
        {
            _act = act;
            _actLaunch = actLaunch;
        }

        public override void OnFinish()
        {
            _act.StartActivity(_actLaunch);
            _act.Finish();
        }

        public override void OnTick(long millisUntilFinished)
        {

        }


    }
}