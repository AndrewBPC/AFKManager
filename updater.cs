using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoUpdaterDotNET;

namespace AFKManager
{
    class updater
    {
        public updater()
        {
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            AutoUpdater.RunUpdateAsAdmin = false;
            AutoUpdater.HttpUserAgent = "AutoUpdate";  
        }
        
        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args.Error == null)
            {
                if (args.IsUpdateAvailable)
                {
                    updateWindow uw = new updateWindow(args);
                    
                    uw.ShowDialog();
                    
                }
            }
        }

        public void StartUpdate()
        {
            AutoUpdater.HttpUserAgent = "Mozilla/5.0";
            AutoUpdater.Start(@"http://k96025ii.beget.tech/afkmanager/update.xml");
        }

    }
}
