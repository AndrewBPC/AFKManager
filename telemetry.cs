//using Ets2SdkClient;
using SCSSdkClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AFKManager
{
    class telemetry
    {
        private SCSSdkTelemetry Telemetry;
        public static bool isEtsPaused { get; private set; }
        public static bool isTelemetryConnected { get; private set; }
        public static bool isProgramSDKWork { get; private set; }
        public telemetry()
        {
            
            
            Telemetry = new SCSSdkTelemetry(1000);
            
            Telemetry.Data += Telemetry_Data;
            

            if (Telemetry.Error != null)
            {
                isProgramSDKWork = false;
            }
            else
            {
                isProgramSDKWork = true;
            }
        }
        

        private void Telemetry_Data(SCSSdkClient.Object.SCSTelemetry data, bool updated)
        {
            
            _ = new TelemetryData(Telemetry_Data);
            isEtsPaused = data.Paused;
            isTelemetryConnected = data.SdkActive;
            

        }

        

    }
}
