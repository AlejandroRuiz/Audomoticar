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
using Xamarin.Auth;
using Audomoticar.Library.Core;

namespace Audomoticar
{
    [Activity(Label = "Audomoticar", NoHistory=true,Theme = "@android:style/Theme.Holo.Light", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ListaConectar : Activity
    {
        private static List<BtDevice> ListaDispositivos = new List<BtDevice>();
        // Debugging
        private const string TAG = "DeviceListActivity";
        private const bool Debug = true;

        // Return Intent extra
        public const string EXTRA_DEVICE_ADDRESS = "device_address";

        // Member fields
        private BluetoothAdapter btAdapter;
        private static ArrayAdapter<string> pairedDevicesArrayAdapter;
        private Receiver receiver;

        protected override void OnStart()
        {
            base.OnStart();
            try
            {
                IEnumerable<Account> accounts = AccountStore.Create(this).FindAccountsForService("Arduino");
                if (accounts.ToList().Count == 0)
                {
                    StartActivity(typeof(LoginActivity));
                    Finish();
                }
                foreach (var n in accounts)
                {
                    if (n.Properties["Mac"] != string.Empty)
                    {
                        StartActivity(typeof(Principall));
                        Finish();
                        break;
                    }
                }

                
                
                ListaDispositivos.Clear();
                btAdapter = BluetoothAdapter.DefaultAdapter;
                if (!btAdapter.IsEnabled)
                {
                    btAdapter.Enable();
                }
                var pairedDevices = btAdapter.BondedDevices;
                if (pairedDevices.Count > 0)
                {
                    foreach (var device in pairedDevices)
                    {
                        BtDevice dis = new BtDevice();
                        dis.Nombre = device.Name;
                        dis.Mac = device.Address;
                        ListaDispositivos.Add(dis);
                        pairedDevicesArrayAdapter.Add(device.Name + "\n" + device.Address);
                    }
                }
                else
                {
                }
                DoDiscovery();
            }
            catch (Exception)
            {

            }
        }

        void DeviceListClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            btAdapter.CancelDiscovery();
            IEnumerable<Account> accounts = AccountStore.Create(this).FindAccountsForService("Arduino");
            foreach (var n in accounts)
            {
                Account ac = new Account(n.Username, Credencials.Cuenta(n.Username, n.Properties["Token"], ListaConectar.ListaDispositivos[e.Position].Mac, n.Properties["Cam"], n.Properties["IdUser"]));
                AccountStore.Create(this).Save(ac, "Arduino");
                StartActivity(typeof(Principall));
                Finish();
                break;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.IndeterminateProgress);
            SetContentView(Resource.Layout.Encontrar);
            SetResult(Result.Canceled);
            pairedDevicesArrayAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1);
            var pairedListView = FindViewById<ListView>(Resource.Id.listadescubrir);
            pairedListView.Adapter = pairedDevicesArrayAdapter;
            pairedListView.ItemClick += DeviceListClick;
            receiver = new Receiver(this);
            var filter = new IntentFilter(BluetoothDevice.ActionFound);
            RegisterReceiver(receiver, filter);
            filter = new IntentFilter(BluetoothAdapter.ActionDiscoveryFinished);
            RegisterReceiver(receiver, filter);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (btAdapter != null)
            {
                btAdapter.CancelDiscovery();
            }

            UnregisterReceiver(receiver);
        }

        private void DoDiscovery()
        {
            if (Debug)
                Console.WriteLine("doDiscovery()");
            SetProgressBarIndeterminateVisibility(true);
            if (btAdapter.IsDiscovering)
            {
                btAdapter.CancelDiscovery();
            }
            btAdapter.StartDiscovery();
        }

        public class Receiver : BroadcastReceiver
        {
            Activity _clase;

            public Receiver(Activity clase)
            {
                _clase = clase;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                string action = intent.Action;
                if (action == BluetoothDevice.ActionFound)
                {
                    BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                    if (device.BondState != Bond.Bonded)
                    {
                        BtDevice dis = new BtDevice();
                        dis.Nombre = device.Name;
                        dis.Mac = device.Address;
                        ListaConectar.ListaDispositivos.Add(dis);
                        pairedDevicesArrayAdapter.Add(device.Name + "\n" + device.Address);
                    }
                }
                else if (action == BluetoothAdapter.ActionDiscoveryFinished)
                {
                    _clase.SetProgressBarIndeterminateVisibility(false);
                }
            }


        }
    }

    public class BtDevice
    {
        public string Nombre { get; set; }
        public string Mac { get; set; }
    }

}