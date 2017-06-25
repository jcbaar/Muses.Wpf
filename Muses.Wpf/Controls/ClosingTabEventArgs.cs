using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Muses.Wpf.Controls
{
    public class ClosingTabEventArgs : RoutedEventArgs
    {
        public ClosingTabEventArgs() { }
        public ClosingTabEventArgs(RoutedEvent e) : base(e) { }
        public ClosingTabEventArgs(RoutedEvent e, object source) : base(e, source) { }

        public bool Cancel { get; set; }
    }
}
