using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace AFKManager
{
    public class afkSettings
    {
        public byte vConfig { get; set; } = 1;
        private double time = 600000;
        public double Time {
            get
            {
                return time;
            }
            set
            {
                time = value;
                if (value < 60000) time = 60000;
                if (value > 600000) time = 600000;
                
            }
        } 
        public ObservableCollection<afkTextModel> afkText { get; set; } = new ObservableCollection<afkTextModel>();
        
        public int hotkey { get; set; } = 90;

    }
}
