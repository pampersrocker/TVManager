using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CecSharp;
using System.Diagnostics;
using System.Windows.Forms;

namespace TVManager
{
    class CECUtility : CecSharp.CecCallbackMethods
    {
        private static CECUtility s_Instance;
        public static TVManager.CECUtility Instance
        {
            get { return s_Instance; }
            set { s_Instance = value; }
        }

        private CECUtility()
        {
            Instance = this;
        }

        public LibCECConfiguration Config;
        public LibCecSharp Lib;
        public int LogLevel;

        public static void CreateUtility()
        {
            CECUtility Utility = new CECUtility();
            Utility.Config = new LibCECConfiguration();
            Utility.Config.DeviceTypes.Types[0] = CecDeviceType.RecordingDevice;
            Utility.Config.DeviceName = "TV Manager";
            Utility.Config.ClientVersion = LibCECConfiguration.CurrentVersion;
            Utility.Config.SetCallbacks(Utility);
            Utility.Config.WakeDevices.Clear();
            Utility.Config.ActivateSource = false;

            Utility.Lib = new LibCecSharp(Utility.Config);
            Utility.Lib.InitVideoStandalone();

            Utility.LogLevel = (int)CecLogLevel.All;

            var Adapters = Utility.Lib.FindAdapters(string.Empty);


            if (Adapters.Length > 0)
            {
                int RetryCount = 0;
                while(!Utility.Lib.Open(Adapters[0].ComPort, 10000))
                {
                    System.Threading.Thread.Sleep(1000);
                    RetryCount++;
                    if (RetryCount > 10)
                    {
                        MessageBox.Show("Could not connect after 10 retries");
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Could not find Adapters");

            }
        }

        public override int ReceiveLogMessage(CecLogMessage message)
        {
            if (((int)message.Level & LogLevel) == (int)message.Level)
            {
                string strLevel = "";
                switch (message.Level)
                {
                    case CecLogLevel.Error:
                        strLevel = "ERROR:   ";
                        break;
                    case CecLogLevel.Warning:
                        strLevel = "WARNING: ";
                        break;
                    case CecLogLevel.Notice:
                        strLevel = "NOTICE:  ";
                        break;
                    case CecLogLevel.Traffic:
                        strLevel = "TRAFFIC: ";
                        break;
                    case CecLogLevel.Debug:
                        strLevel = "DEBUG:   ";
                        break;
                    default:
                        break;
                }
                string strLog = string.Format("{0} {1,16} {2}", strLevel, message.Time, message.Message);
                Debug.WriteLine(strLog);
            }
            return 1;
        }
    }
}
