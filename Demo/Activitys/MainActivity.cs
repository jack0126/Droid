using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Jack.Mvvm.Droid;

namespace Demo.Activitys
{
    class MainActivity : Activity
    {
        private TextBlock MessageBlock;
        public override UserControl OnCreateView()
        {
            return new Views.MainActivity();
        }

        public override void OnViewCreated()
        {
            MessageBlock = FindViewByName<TextBlock>("Message");
            MessageBlock.Text = "Hello, welcome to my world!";
        }

        private void OnGo()
        {
            StartActivity<TestActivity>();
        }

        public override void OnResume()
        {

        }

        public override void OnPause()
        {

        }

        public override void OnDestroy()
        {

        }

        public override void OnResult(string source, params object[] args)
        {

        }
    }
}
