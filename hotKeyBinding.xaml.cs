using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AFKManager
{
    /// <summary>
    /// Interaction logic for hotKeyBinding.xaml
    /// </summary>
    public partial class hotKeyBinding : Window
    {
        GlobalKeyboardHook gkh;
        afkSettings settings;
        public hotKeyBinding(afkSettings settings, GlobalKeyboardHook gkh)
        {
            InitializeComponent();
            this.settings = settings;
            this.gkh = gkh;
            this.gkh.KeyboardPressed += OnKeyPressed;
            
            
        }
        
        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp)
            {
                settings.hotkey = e.KeyboardData.VirtualCode;
                Task.Delay(500);
                gkh.KeyboardPressed -= OnKeyPressed;
                this.Close();
            }
            
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
