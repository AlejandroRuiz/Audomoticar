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
using Android.Bluetooth;
using Android.Webkit;
using System.IO;
using Java.Util;
using System.Threading.Tasks;
using Xamarin.Auth;
using Audomoticar.Library.Core;
using Com.Camera.Simplemjpeg;
using Android.Net;
using Audomoticar.Library.Models;

namespace Audomoticar
{
    [Activity(Label = "Audomoticar", NoHistory = true, Theme = "@android:style/Theme.Holo.Light", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Principall : Activity
    {
        ToggleButton OnOff;
        TextView Result;
        private Java.Lang.String dataToSend;

        private static string TAG = "Jon";
        private BluetoothAdapter mBluetoothAdapter = null;
        private BluetoothSocket btSocket = null;
        private Stream outStream = null;
        private string address = "";
        private static UUID MY_UUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
        private Stream inStream = null;

        Button btnAdelante;
        Button btnApagado;
        Button btnAtras;
        Button btnQuemon;
        Button btnQuemon1;

        private static bool DEBUG = false;

        private MjpegView mv = null;
        String URL = "";

        private int width = 640;
        private int height = 480;
        public int IdUSER = 0;
        private bool suspending = false;
        private static int TEXT_ID = 0;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
             IEnumerable<Account> accounts = AccountStore.Create(this).FindAccountsForService("Arduino");
             foreach (var n in accounts)
             {
                 address = n.Properties["Mac"] ?? "";
                 URL = n.Properties["Cam"] ?? "";
                 IdUSER = int.Parse(n.Properties["IdUser"]);
             }
            // Create your application here
            SetContentView(Resource.Layout.Prueba);


            OnOff = FindViewById<ToggleButton>(Resource.Id.toggleButtonUsar);
            btnAdelante = FindViewById<Button>(Resource.Id.btnAdelante);
            btnApagado = FindViewById<Button>(Resource.Id.btnParar);
            btnAtras = FindViewById<Button>(Resource.Id.btnAtras);
            btnQuemon = FindViewById<Button>(Resource.Id.btnQuemon);
            btnQuemon1 = FindViewById<Button>(Resource.Id.btnDer);

            OnOff.CheckedChange += OnOff_HandleCheckedChange;
            btnAdelante.Click += (a, e) =>
            {
                TrySaveData("Señal de Avance", "Lanzado desde Aplicacion", IdUSER);
                dataToSend = new Java.Lang.String("a");
                writeData(dataToSend);
            };
            btnAtras.Click += (a, e) =>
            {
                TrySaveData("Señal de Retroceder", "Lanzado desde Aplicacion", IdUSER);
                dataToSend = new Java.Lang.String("b");
                writeData(dataToSend);
            };
            btnApagado.Click += (a, e) =>
            {
                TrySaveData("Señal de paro", "Lanzado desde Aplicacion", IdUSER);
                dataToSend = new Java.Lang.String("c");
                writeData(dataToSend);
            };
            btnQuemon.Click += (a, e) =>
            {
                TrySaveData("Señal de Giro a la Derecha", "Lanzado desde Aplicacion", IdUSER);
                dataToSend = new Java.Lang.String("q");
                writeData(dataToSend);
            };
            btnQuemon1.Click += (a, e) =>
            {
                TrySaveData("Señal de Giro a la Izquieda", "Lanzado desde Aplicacion", IdUSER);
                dataToSend = new Java.Lang.String("l");
                writeData(dataToSend);
            };

            mv = FindViewById<MjpegView>(Resource.Id.mv);
            if (mv != null)
            {
                mv.SetResolution(width, height);
            }
            taskTask(URL);
            SaveAllOnCloud();
            ((ImageView)FindViewById(Resource.Id.imageView1)).SetBackgroundColor(Android.Graphics.Color.Green);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 0, 0, "Salir");
            menu.Add(0, 1, 1, "Seleccionar Bluetooth");
            menu.Add(0, 2, 2, "Configurar Camara");
            menu.Add(0, 3, 3, "Cerrar Sesion");
            return true;
        }

        public void TrySaveData(string Descripcion, string Evento, int IdUsuario)
        {
            tblEventoL entity = new tblEventoL();
            entity.Descripcion = Descripcion;
            entity.Evento = Evento;
            entity.Fecha = DateTime.Now.ToShortDateString();
            entity.fkIdUsuario = IdUsuario;
            entity.Hora = DateTime.Now.ToShortTimeString();
            entity.IdEvento = 0;
            entity.IdEventoS = 0;
            entity.Sync = 0;
            EventosD manejador = new EventosD();
            manejador.entity = entity;
            if (IsConectad())
            {
                Task.Factory.StartNew(()=>{
                    if(manejador.UploadData())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }).ContinueWith((t)=>{
                    if (!t.Result)
                    {
                        if (manejador.Create())
                        {
                            /*RunOnUiThread(() =>
                            {
                                Toast.MakeText(this, "Registro Guardado Local", ToastLength.Short).Show();
                            });*/
                        }
                        else
                        {
                            /*RunOnUiThread(() =>
                            {
                                Toast.MakeText(this, "Error al Guardar Registro Local", ToastLength.Short).Show();
                            });*/
                        }
                    }
                    else
                    {
                        //Se guardo correctamente en la nube
                    }
                });
            }
            else
            {
                if (manejador.Create())
                    Toast.MakeText(this, "Registro Guardado Local", ToastLength.Short).Show();
                else
                    Toast.MakeText(this, "Error al Guardar Registro Local", ToastLength.Short).Show();
            }
        }

        public void SaveAllOnCloud()
        {
            if (IsConectad())
            {
                Task.Factory.StartNew(() =>
                {
                    var PorSubir = from p in EventosD.arrayList() where p.Sync == 0 select p;
                    foreach (var en in PorSubir)
                    {
                        EventosD manejador = new EventosD();
                        manejador.entity = en;
                        if (manejador.UploadData())
                        {
                            en.Sync = 1;
                            manejador.entity = en;
                            manejador.Update();
                        }
                    }
                });
            }
            else
            {
                Toast.MakeText(this, "No hay conexion de red", ToastLength.Short).Show();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 0: //Do stuff for button 0
                    Finish();
                    return true;
                case 1:
                    IEnumerable<Account> accounts = AccountStore.Create(this).FindAccountsForService("Arduino");
                    foreach (var n in accounts)
                    {
                        Account ac = new Account(n.Username, Credencials.Cuenta(n.Username, n.Properties["Token"], string.Empty, n.Properties["Cam"], n.Properties["IdUser"]));
                        AccountStore.Create(this).Save(ac, "Arduino");
                    }
                    StartActivity(typeof(ListaConectar));
                    Finish();
                    return true;
                case 2:
                    createDialog().Show();
                    return true;
                case 3: //Do stuff for button 1
                    IEnumerable<Account> accounts1 = AccountStore.Create(this).FindAccountsForService("Arduino");
                    foreach (var n in accounts1)
                    {
                       AccountStore.Create(this).Delete(n, "Arduino");
                    }
                    DataBase.Delete();
                    StartActivity(typeof(LoginActivity));
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            CheckBt();
            Connect();
            OnOff.Checked = true;

            if (IsConectad())
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {

                        IEnumerable<Account> accounts = AccountStore.Create(this).FindAccountsForService("Arduino");
                        Account c = new Account();
                        foreach(var acc in accounts)
                        {
                            c = acc;
                        }
                        return Login.TIsValid(c.Properties["Token"]);
                    }
                    catch (Exception e)
                    {
                        return true;
                    }
                }).ContinueWith( (t)=>
                {
                    if (!t.Result)
                    {
                        if (mv != null)
                        {
                            mv.FreeCameraMemory();
                        }
                        try
                        {
                            btSocket.Close();
                        }
                        catch (System.Exception e)
                        {
                        }
                        IEnumerable<Account> accounts1 = AccountStore.Create(this).FindAccountsForService("Arduino");
                        foreach (var n in accounts1)
                        {
                            AccountStore.Create(this).Delete(n, "Arduino");
                        }
                        DataBase.Delete();
                        RunOnUiThread(() =>
                            {
                                Toast.MakeText(this, "Sesion Terminada, Token Invalido", ToastLength.Short).Show();
                                StartActivity(typeof(LoginActivity));
                                Finish();
                            });
                    }
                });
            }
        }

        private void CheckBt()
        {
            mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            if (!mBluetoothAdapter.Enable())
            {
                Toast.MakeText(this, "Bluetooth Desactivado",
                    ToastLength.Short).Show();
            }

            if (mBluetoothAdapter == null)
            {
                Toast.MakeText(this,
                    "Bluetooth No Existe o esta Ocupado", ToastLength.Short)
                    .Show();
            }
        }

        void OnOff_HandleCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                TrySaveData("Señal de Conexion Bluetooth", "Lanzado desde Aplicacion", IdUSER);
                dataToSend = new Java.Lang.String("1");
                writeData(dataToSend);
                dataToSend = new Java.Lang.String("c");
                writeData(dataToSend);
            }
            else
            {
                TrySaveData("Señal de Desconexion Bluetooth", "Lanzado desde Aplicacion", IdUSER);
                dataToSend = new Java.Lang.String("2");
                writeData(dataToSend);
            }
        }

        public async void Connect()
        {
            //Iniciamos la conexion con el arduino
            BluetoothDevice device = mBluetoothAdapter.GetRemoteDevice(address);
            System.Console.WriteLine("Conexion en curso" + device);

            //Indicamos al adaptador que ya no sea visible
            mBluetoothAdapter.CancelDiscovery();
            try
            {
                //Inicamos el socket de comunicacion con el arduino
                btSocket = device.CreateRfcommSocketToServiceRecord(MY_UUID);
                //Conectamos el socket
                btSocket.Connect();
                Toast.MakeText(this, "Conexion Correcta", ToastLength.Short);
            }
            catch (System.Exception e)
            {
                //en caso de generarnos error cerramos el socket
                System.Console.WriteLine(e.Message);
                try
                {
                    btSocket.Close();
                }
                catch (System.Exception)
                {
                    Toast.MakeText(this, "Imposible conectar", ToastLength.Short);
                }
                System.Console.WriteLine("Socket Creado");
            }

            //Una vez conectados al bluetooth mandamos llamar el metodo que generara el hilo
            //que recibira los datos del arduino
            beginListenForData();
        }

        private void writeData(Java.Lang.String data)
        {
            try
            {
                outStream = btSocket.OutputStream;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error al enviar" + e.Message);
            }
            Java.Lang.String message = data;
            byte[] msgBuffer = message.GetBytes();
            try
            {
                outStream.Write(msgBuffer, 0, msgBuffer.Length);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error al enviar" + e.Message);
            }
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (mv != null)
            {
                mv.FreeCameraMemory();
            }
            try
            {
                dataToSend = new Java.Lang.String("2");
                writeData(dataToSend);
                btSocket.Close();
            }
            catch (System.Exception e)
            {
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (mv != null)
            {
                if (mv.IsStreaming)
                {
                    mv.StopPlayback();
                    suspending = true;
                }
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (mv != null)
            {
                if (suspending)
                {
                    taskTask(URL);
                    suspending = false;
                }
            }
        }

        public void beginListenForData()
        {
            try
            {
                inStream = btSocket.InputStream;
            }
            catch (System.IO.IOException e)
            {
            }
            Task.Factory.StartNew(() =>
            {
                byte[] buffer = new byte[1024];
                int bytes;
                while (true)
                {
                    try
                    {
                        bytes = inStream.Read(buffer, 0, buffer.Length);
                        if (bytes > 0)
                        {
                            RunOnUiThread(() =>
                            {
                                string valor = System.Text.Encoding.UTF8.GetString(buffer);
                                if (valor.Contains('1'))
                                {
                                    TrySaveData("Comando de encendido procesado correctamente", "Lanzado desde Robot", IdUSER);
                                }
                                else if (valor.Contains('2'))
                                {
                                    TrySaveData("Comando de apagado procesado correctamente", "Lanzado desde Robot", IdUSER);
                                }
                                else if (valor.Contains('a'))
                                {
                                    TrySaveData("Comando de avance procesado correctamente", "Lanzado desde Robot", IdUSER);
                                }
                                else if (valor.Contains('b'))
                                {
                                    TrySaveData("Comando de retroceso procesado correctamente", "Lanzado desde Robot", IdUSER);
                                }
                                else if (valor.Contains('c'))
                                {
                                    TrySaveData("Comando de paro procesado correctamente", "Lanzado desde Robot", IdUSER);
                                }
                                else if (valor.Contains('q'))
                                {
                                    TrySaveData("Comando de giro a la derecha procesado correctamente", "Lanzado desde Robot", IdUSER);
                                }
                                else if (valor.Contains('l'))
                                {
                                    TrySaveData("Comando de giro a la izquierda procesado correctamente", "Lanzado desde Robot", IdUSER);
                                }
                                else if(valor.Contains('x'))
                                {
                                    TrySaveData("Objeto localizado en ruta modificando direccion", "Lanzado desde Robot", IdUSER);
                                }
                                else if (valor.Contains('g'))
                                {
                                    ((ImageView)FindViewById(Resource.Id.imageView1)).SetBackgroundColor(Android.Graphics.Color.Red);
                                    TrySaveData("Alerta de gas detectado", "Lanzado desde Robot", IdUSER);
                                }
                                else if (valor.Contains('3'))
                                {
                                    ((ImageView)FindViewById(Resource.Id.imageView1)).SetBackgroundColor(Android.Graphics.Color.Green);
                                }
                                else
                                {
                                    TrySaveData("Comando no identificado", "Lanzado desde Robot", IdUSER);
                                }
                            });
                        }
                    }
                    catch (Java.IO.IOException e)
                    {
                        break;
                    }
                }
            });
        }

        public void setImageError()
        {
            RunOnUiThread(() =>
            {
                this.Title = "Error de imagen";
            });
        }

        public void taskTask(string url)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    return MjpegInputStream.Read(url);
                }
                catch (Exception e)
                {
                    if (DEBUG)
                    {
                    }
                    //Error connecting to camera
                }
                return null;
            }).ContinueWith((t) =>
            {
                mv.SetSource(t.Result);
                if (t.Result != null)
                {
                    t.Result.SetSkip(1);
                    //Title = "Prueba";
                }
                else
                {
                    //Title = "Desconectado";
                }
                mv.SetDisplayMode(MjpegView.SizeFullscreen);
                mv.ShowFps(false);
            });
        }

        public bool IsConectad()
        {
            var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

            var activeConnection = connectivityManager.ActiveNetworkInfo;

            if ((activeConnection != null) && activeConnection.IsConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    private Dialog createDialog() {
 
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.SetTitle("Camara");
        builder.SetMessage("Ingresa la Direccion");
 
         // Use an EditText view to get user input.
         EditText input = new EditText(this);
         input.Id = TEXT_ID;
         input.Text = URL;
         builder.SetView(input);
 
        builder.SetPositiveButton("Aceptar", (e,a)=>{
                string value = input.Text;
                URL = value;
                IEnumerable<Account> accounts = AccountStore.Create(this).FindAccountsForService("Arduino");
                foreach (var n in accounts)
                {
                    Account ac = new Account(n.Username, Credencials.Cuenta(n.Username, n.Properties["Token"], n.Properties["Mac"], value, n.Properties["IdUser"]));
                    AccountStore.Create(this).Save(ac, "Arduino");
                }
                taskTask(URL);
                return;
        });

        builder.SetNegativeButton("Cancelar", (a,e)=>{});

 
        return builder.Create();
    }
    }
}