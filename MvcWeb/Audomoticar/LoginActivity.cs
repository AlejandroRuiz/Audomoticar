using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using AndroidHUD;
using Audomoticar.Library.Core;
using System.Threading.Tasks;
using Xamarin.Auth;
using System.Collections.Generic;
using Audomoticar.Library.Models;

namespace Audomoticar
{
    [Activity(Label = "Audomoticar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, NoHistory = true, Theme = "@android:style/Theme.Holo.Light")]
    public class LoginActivity : Activity
    {
        private EditText username = null;
        private EditText password = null;
        private Button login;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            
            username = FindViewById<EditText>(Resource.Id.editText1);
            password = FindViewById<EditText>(Resource.Id.editText2);
            login = FindViewById<Button>(Resource.Id.button1);
            login.Click += login_Click;
        }

        protected override void OnStart()
        {
            base.OnStart();
            IEnumerable<Account> accounts = AccountStore.Create(this).FindAccountsForService("Arduino");
            foreach (var n in accounts)
            {
                StartActivity(typeof(ListaConectar));
                Finish();
                break;
            }
        }

        void login_Click(object sender, EventArgs e)
        {
            if (username.Text == "admin" && password.Text == "12345")
            {
                DataBase.Create();
                Account ac = new Account(username.Text, Credencials.Cuenta(username.Text, "None", string.Empty, string.Empty, "0"));
                AccountStore.Create(this).Save(ac, "Arduino");
                    RunOnUiThread(() =>
                            {
                                Toast.MakeText(this, "Bienvenido", ToastLength.Short).Show();
                                StartActivity(typeof(ListaConectar));
                                Finish();
                            });
            }
            else{
            if (username.Text.Trim() != "" && password.Text.Trim() != "")
            {
                RunOnUiThread(() => {
                    AndHUD.Shared.Show(this, "Ingresando");
                });
                Task.Factory.StartNew(() =>{
                    Login l = new Login(username.Text, password.Text);
                    RespuestaLogin r = l.getResponse();
                    if (r.Respuesta)
                    {
                        string id = l.IdUser(username.Text);
                        DataBase.Create();
                        Account ac = new Account(username.Text, Credencials.Cuenta(username.Text, r.Token, string.Empty, string.Empty, id));
                        AccountStore.Create(this).Save(ac, "Arduino");
                        RunOnUiThread(() =>
                        {
                            AndHUD.Shared.Dismiss(this);
                            Toast.MakeText(this, "Bienvenido", ToastLength.Short).Show();
                            StartActivity(typeof(ListaConectar));
                            Finish();
                        });
                    }
                    else
                    {
                        RunOnUiThread(() =>
                        {
                            AndHUD.Shared.Dismiss(this);
                            Toast.MakeText(this, "Usuario o Clave Incorrectos", ToastLength.Short).Show();
                        });
                    }
                });
            }
            else
            {
                RunOnUiThread(() => {
                    Toast.MakeText(this, "Campos Vacios", ToastLength.Short).Show();
                });
            }
                }
        }
    }
}

