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
        private Button btnBack;
        public override UserControl OnCreateView()
        {
            return new Views.TestActivity();
        }

        public override void OnViewCreated()
        {
            btnBack = FindViewByName<Button>("Back");
            btnBack.Click += BtnBack_Click;
        }

        private void BtnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Finish();
        }
    }
}
