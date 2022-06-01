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
        public double time { get; set; } = 600000;
        public ObservableCollection<afkTextModel> afkText { get; set; } = new ObservableCollection<afkTextModel>();
        public int hotkey { get; set; } = 90;

    }
}
