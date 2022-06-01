using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Windows;

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
                GamePath = SearchEtsInSteamLibrary();
                CheckTelemetry();
            }
	        catch (Exception e){
                MessageBox.Show("Erorr while install/check sdk","AFKManager Error", MessageBoxButton.OK ,MessageBoxImage.Error);
            }
            
            
        }
        private void CheckTelemetry()
        {
            if (File.Exists(GamePath + "win_x64/plugins/scs-telemetry.dll"))
            {
                
            }
            else
            {
                    DirectoryInfo dI = new DirectoryInfo(GamePath + "win_x64/plugins/");
                    
                    if (!dI.Exists)
                    {
                        Debug.WriteLine("plugin folder not found!");
                        dI.Create();
                    }
                    File.Copy("./files/scs-telemetry.dll", GamePath + "win_x64/plugins/scs-telemetry.dll");
                
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


            string[] ConfigSteamLibrary = File.ReadAllLines(SteamPath + "/steamapps/libraryfolders.vdf"); //  or libraryfolders.vdf

            for (int i = 5; i < ConfigSteamLibrary.Length; i += 7)
            {
                StringBuilder sb = new StringBuilder(ConfigSteamLibrary[i]);
                if (sb.Length < 5) return;
                sb.Remove(0, 11);
                sb.Remove(sb.Length - 1, 1);

                SteamLibraryPaths.Add(sb.ToString());
            }

        }
                
               
        
        private string SearchEtsInSteamLibrary()
        {
            for(int i = 0; i < SteamLibraryPaths.Count; i++)
            {
                DirectoryInfo directoryInfoBin = new DirectoryInfo(SteamLibraryPaths[i] + "/steamapps/common/Euro Truck Simulator 2/bin/");
                if (directoryInfoBin.Exists) return directoryInfoBin.FullName;
                
            }
            return "0";
            

        }
    }
}
