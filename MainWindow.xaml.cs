using System;
using System.Windows;
using Forms = System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using Newtonsoft.Json;

using System.IO;
using System.Collections.ObjectModel;
//using Ets2SdkClient;

namespace AFKManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    
    public partial class MainWindow : Window
    {
        string[] hotkeyValueToString = { "Zero Array","LMB", "RMB", "Ctrl + Break", "MMB", "X1MB", "X2MB", "Undefined", "Backspace", "Tab", "Reserved", "Reserved", "Clear", "Enter", "Undefined", "Undefined", "Shift", "Ctrl", "Alt", "Pause", "CAPS LOCK", "KANA", "HANGUEL", "HANGUL", "Undefined", "JUNJA", "final", "Esc", "CONVERT", "NONCONVERT", "Accept", "ModeChange", "Space", "Page Up", "Page Down", "End", "Home", "Left Arrow", "Arrow Up", "Right Arrow", "Arrow Down", "Select", "Print", "Execute", "Print Screen", "Insert", "Delete", "Help", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Left Windows", "Right Windows", "App Key", "Reserved", "Sleep", "Numpad 0", "Numpad 1", "Numpad 2", "Numpad 3", "Numpad 4", "Numpad 5", "Numpad 6", "Numpad 7", "Numpad 8", "Numpad 9", "*", "+", "	Separator", "-", ".", "/", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "F16", "F17", "F18", "F19", "F20", "F21", "F22", "F23", "F24", "Unassigned", "Unassigned", "Unassigned", "Unassigned", "Unassigned", "Unassigned", "Unassigned", "Unassigned", "NumLock", "Scroll Lock", "OEM specific", "OEM specific", "OEM specific", "OEM specific", "OEM specific", "Unassigned", "Unassigned", "Unassigned", "Unassigned", "Unassigned", "Unassigned", "Unassigned", "Unassigned", "Unassigned", "Left Shift", "Right Shift", "Left Ctrl", "Right Ctrl","Left Alt", "Left Alt", "Browser Back", "Browser Forward", "Browser Refresh", "Browser Stop", "Browser Search", "Browser Favorites", "Browser Start and Home", "Volume Mute", "Volume Down", "Volume Up", "Next Track", "Previous Track", "Stop Media", "Play/Pause Media", "Start Mail", "Select Media", "Start Application 1", "Start Application 1", "Reserved", "Reserved", ";:", "+", ",", "-", ".", "/?", "~", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Reserved", "Unassigned", "Unassigned", "Unassigned", "[{", @"\|", "]}", "\'\"", "OEM_8", "Reserved", "OEM Specific", "Agle Bracket", "OEM specific", "OEM specific", "IME PROCESS", "OEM specific", "VK_PACKET", "Unassigned", "OEM specific", "OEM specific", "OEM specific", "OEM specific", "OEM specific", "OEM specific", "OEM specific", "OEM specific", "OEM specific", "OEM specific", "OEM specific", "OEM specific", "OEM specific", "Attn", "CrSel", "ExSel", "Erase", "Play", "Zoom", "Reserved for future use", "PA1 Key", "Clear Key"};
        Forms.NotifyIcon notifyIcon;
        afk AFKManager;
        public afkSettings _settings;
        GlobalKeyboardHook globalKeyboardHook;
        telemetry _tm;
        
        string pathToSave = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "AFKManager", "settings.json");
        void CloseHandler(object sender, CancelEventArgs e)
        {
            notifyIcon.Dispose();
            globalKeyboardHook?.Dispose();
            if (!File.Exists(pathToSave))
            {
                Directory.CreateDirectory(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "AFKManager"));
            }
            File.WriteAllText(pathToSave, JsonConvert.SerializeObject(_settings));
        }
        
        public MainWindow()
        {
            
            if (File.Exists(pathToSave))
            {
                afkSettings settingsFromJson = JsonConvert.DeserializeObject<afkSettings>(File.ReadAllText(pathToSave));
                _settings = new afkSettings();
                while(settingsFromJson.afkText.Count < 2)
                {
                    //settingsFromJson.afkText.Add("");
                }
                _settings.afkText = settingsFromJson.afkText;
                _settings.time = settingsFromJson.time;
                _settings.hotkey = settingsFromJson.hotkey;
                _settings.vConfig = (settingsFromJson.vConfig);
                InitializeComponent();
                TextList.ItemsSource = _settings.afkText;
                //_settings.afkText.Add(new afkTextModel { Text = "тестовая строка 1" });
                //_settings.afkText.Add(new afkTextModel { Text = "тестовая строка 2" });
                //textList.ItemsSource = _settings.afkText;
                //textBox1.Text = _settings.afkText[0];
                //textBox2.Text = _settings.afkText[1];
                interval.Text = (_settings.time/60000).ToString() + " min";
                timeSlider.Value = _settings.time/60000;
                hotkeyText(_settings.hotkey);
            }
            else
            {
                InitializeComponent();
                
                _settings = new afkSettings();
                //_settings.afkText.Add("");
                //_settings.afkText.Add("");
                

            }
            globalKeyboardHook = new GlobalKeyboardHook();
            globalKeyboardHook.KeyboardPressed += OnKeyPress;
            version.Text = (System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()).Substring(0, 5);
            updater ota = new updater();
            ota.StartUpdate();
            var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(StatusOfGame);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            _tm = new telemetry();

            var TI = new TelemetryInstaller();

            notifyIcon = new Forms.NotifyIcon();
            notifyIcon.Icon = Properties.Resources.Icon;
            notifyIcon.Text = "AFK Manager";
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            notifyIcon.Visible = true;
            notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("Open", null, trayOpenClick);
            notifyIcon.ContextMenuStrip.Items.Add("Close", null, trayCloseClick);
            //onAntiAfk();
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized) this.Hide();
        }

        private void trayCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void trayOpenClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void OnKeyPress(object sender, GlobalKeyboardHookEventArgs e)
        {

            if (e.KeyboardData.VirtualCode != _settings.hotkey)
                return;
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp)
            {
                if(e.KeyboardData.VirtualCode == _settings.hotkey && afk.isEtsForegroud && !telemetry.isEtsPaused)
                {
                    switchAFK();
                }
                e.Handled = true;
            }

        }

        private bool stateOfAFKManager;
        public void switchAFK()
        {
            if (stateOfAFKManager == false)
            {
                onAntiAfk();
            }
            else
            {
                offAntiAfk();
            }
        }
        
        public void onAntiAfk()
        {

            if (!afk.statusOfTruckersMP)
            {
                return;
            }
            if (telemetry.isEtsPaused)
            {
                return;
            }
            AFKStatus.Foreground = Brushes.Green;
            //_settings.afkText[0] = textBox1.Text;
            //_settings.afkText[1] = textBox2.Text;
            AFKManager = new afk(_settings);
            stateOfAFKManager = true;
            
        }
        private void offAntiAfk()
        {
            AFKManager.DestroyTimer();
            stateOfAFKManager = false;
            AFKStatus.Foreground = Brushes.Red;

        }

        byte isFirstStart = 0;
        private void afkTextChanged(object sender, TextChangedEventArgs e)
        {

            if (isFirstStart <2)
            {
                isFirstStart++;
                return;
            }
            
            
            //_settings.afkText[0] = textBox1.Text;
            //_settings.afkText[1] = textBox2.Text;
        }

        

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isFirstStart < 3)
            {
                isFirstStart++;
                return;
            }
            
            interval.Text = e.NewValue.ToString() + " min";
            _settings.time = e.NewValue * 60000;
        }
          public void StatusOfGame(object sender, EventArgs e)
          {
            
            if (afk.CheckETS())
            {
                truckersmpStatus.Foreground = Brushes.Green;
                afk.statusOfTruckersMP = true;
            }
            else
            {
                truckersmpStatus.Foreground = Brushes.Red;
                afk.statusOfTruckersMP = false;
                if(stateOfAFKManager) offAntiAfk();
            }
            
            if (afk.GetActiveWindow() == true)
            {
                
                afk.isEtsForegroud = true;
            }
            else
            {
                
                afk.isEtsForegroud = false;
            }
            if (telemetry.isTelemetryConnected)
            {
                isTelemetry.Foreground = Brushes.Green;
                GameSDKStatus.Foreground = Brushes.Green;
            }
            else
            {
                isTelemetry.Foreground = Brushes.Red;
                GameSDKStatus.Foreground = Brushes.Red;
            }
            if (telemetry.isProgramSDKWork)
            {
                SDKStatus.Foreground = Brushes.Green;
            }
            else
            {
                SDKStatus.Foreground = Brushes.Red;
            }

          }
        public void hotkeyText(int hotkeyValue)
        {
            hotkey.Text = hotkeyValueToString[hotkeyValue];
        }

        private void HotKey_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
            hotKeyBinding hk = new hotKeyBinding(_settings, globalKeyboardHook);
            hk.Owner = this;
            hk.ShowDialog();
            hotkeyText(_settings.hotkey);
            

        }
        
        private void About_Button_Click(object sender, RoutedEventArgs e)
        {

            var window = new AboutPage();
            window.Owner = this; window.Show();
        }

        private void DeleteStroke(object sender, RoutedEventArgs e)
        {
            
            
            Button btn = sender as Button;
            if (btn != null)
            {
                object item = btn.DataContext;


                if (item != null)
                {
                    int index = this.TextList.Items.IndexOf(item);
                    _settings.afkText.RemoveAt(index);
                    
                }
            }
            
            
        }

        private void AddStroke(object sender, RoutedEventArgs e)
        {
            _settings.afkText.Add(new afkTextModel { Text = ""});
        }
    }
}
