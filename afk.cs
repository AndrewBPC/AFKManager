using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using WindowsInput;
using WindowsInput.Native;
using System.Media;
using System.Collections.Generic;

using System.Diagnostics;

namespace AFKManager
{
    class afk
    {

        
        Timer AFKTimer;

        private ObservableCollection<afkTextModel> _afkText =  new ObservableCollection<afkTextModel>();
        private bool isOn { get; set; } = true;
        public afk(afkSettings settings)
        {

            _afkText = settings.afkText;
            AFKTimer = new Timer(Convert.ToInt32(settings.time));
            AFKTimer.Elapsed += WriteKey;
            AFKTimer.Start();
            
            WriteKey(null, null);


        }
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("USER32.DLL")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private static IntPtr GetHandle()
        {
            return GetForegroundWindow();
        }

        public static bool GetActiveWindow()
        {
            const int nChars = 256;

            StringBuilder Buff = new StringBuilder(nChars);


            if (GetWindowText(GetHandle(), Buff, nChars) > 0)
            {
                if (Buff.ToString() == "Euro Truck Simulator 2 Multiplayer")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            return false;
        }

        public static bool CheckETS()
        {

            IntPtr etsHandle = FindWindow("prism3d", "Euro Truck Simulator 2 Multiplayer");
            
            if (etsHandle == IntPtr.Zero)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }

        public void DestroyTimer()
        {
            playSound(Properties.Resources.afkStop);
            AFKTimer.Dispose();
            
        }
        async void WriteKey(object sender, ElapsedEventArgs e)
        {
            if (!statusOfTruckersMP) return;
            if (telemetry.isEtsPaused) return;
            if (!isEtsForegroud)
            {
                playSound(Properties.Resources.afkError);
                
                return;
            }

            if (isOn)
            {
                playSound(Properties.Resources.afkStart);
                isOn = false;
            } else
            {
                playSound(Properties.Resources.afk);
            }
            
            
            var inputSim = new InputSimulator();
            
            for(int i = 0; i < _afkText.LongCount(); i++)
            {
                if (!isEtsForegroud) return;
                inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_Y);
                inputSim.Keyboard.TextEntry(_afkText[i].Text);
                inputSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                await Task.Delay(530);
            }
            
        }
        private void playSound(System.IO.UnmanagedMemoryStream sound)
        {
            var player = new SoundPlayer(sound);
            player.Load();
            player.Play();
            
        }
        

        public static bool statusOfTruckersMP { get; set; }
        public static bool isEtsForegroud { get; set; }


    }
}
