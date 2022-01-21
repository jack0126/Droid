using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Jack.Mvvm.Droid;

namespace Demo.Activitys
{
    class TestActivity : Activity
    {
        public override UserControl OnCreateView()
        {
            return new Views.TestActivity();
        }
        
        private void OnBack()
        {
            Finish();
        }
    }
}
