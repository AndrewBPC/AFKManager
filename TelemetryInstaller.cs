using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using Gameloop.Vdf;
using Gameloop.Vdf.Linq;

namespace AFKManager
{
    class TelemetryInstaller
    {
        public string SteamPath { get; private set; }
        public string GamePath { get; private set; }
        public List<string> SteamLibraryPaths { get; private set; } = new List<string>();
        
        public TelemetryInstaller()
        {
            
            try 
	        {
                SearchSteamInstallPath();
                SearchSteamLibraryFolders();
                CheckTelemetry();
            }
	        catch (Exception e){
                MessageBox.Show("Erorr while install/check sdk\n" + e.Message,"AFKManager Error", MessageBoxButton.OK ,MessageBoxImage.Error);
            }
            
        }
        private void CheckTelemetry()
        {
            
            if (!File.Exists(Path.Combine(GamePath, @"bin\win_x64\plugins\scs-telemetry.dll")))
            {
                try
                {
                    DirectoryInfo dI = new DirectoryInfo(Path.Combine(GamePath, @"bin\win_x64\plugins\"));
                    if (!dI.Exists) dI.Create();
                    File.Copy("./files/scs-telemetry.dll", Path.Combine(GamePath, @"bin\win_x64\plugins\scs-telemetry.dll"));
                }
                catch (DirectoryNotFoundException dirEx)
                {
                    MessageBox.Show("Check Telemetry failed\n" + dirEx.Message);
                }
                
            }
        }
        
        private void SearchSteamInstallPath()
        {
            RegistryKey SteamPath1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Valve\\Steam\\");
            SteamPath = SteamPath1.GetValue("SteamPath") as string;
            SteamLibraryPaths.Add(SteamPath);
        }
        
     
      
        private void SearchSteamLibraryFolders()
        {
            VToken vdf = VdfConvert.Deserialize(File.ReadAllText(Path.Combine(SteamPath, @"/steamapps/libraryfolders.vdf"))).Value;
            string SteamLibraryWithGame = String.Empty;
            int i = 0;
            var tempObj = vdf[i.ToString()];
            while (tempObj != null)
            {
                VToken tempToken = tempObj["apps"]["227300"];
                if (tempToken !=null)
                {
                    SteamLibraryWithGame = vdf[i.ToString()]["path"].ToString();
                    GamePath = Path.Combine(SteamLibraryWithGame, @"SteamApps\common\Euro Truck Simulator 2\");
                    if(File.Exists(GamePath)) Debug.WriteLine(GamePath);
                    return;
                }
                i++;
                tempObj = vdf[i.ToString()];
            }
        }
    }
}
