# Droid
Jack.Droid  for C#
# WPF桌面应访安卓（Activity）开发模式框架，主要适用于自助机类应用开发
# Demo

<Window x:Class="Sany.Scrap.MainWindow"
        
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        
        xmlns:a="https://github.com/jack0126/Droid"
        
        xmlns:local="clr-namespace:Sany.Scrap"
        
        xmlns:activity="clr-namespace:Sany.Scrap.Activitys"
        
        mc:Ignorable="d"
        
        Icon="/Sany.Scrap;component/Res/ic_company_logo.png"
        
        WindowStartupLocation="CenterScreen"
        
        Title="" Height="720" Width="1024">
        
    <a:ActivityViewGroup Launcher="{x:Type activity:MainActivity}"/>
        
</Window>


namespace Sany.Scrap.Activitys

{
    using System;

    using System.Collections.Generic;

    using System.Linq;

    using System.Text;

    using System.Threading.Tasks;

    using Jack.Mvvm.Droid;

    class MainActivity : Activity
    
    {
    
        public override UserControl OnCreateView()
        {
            return new Views.MainActivity();//Views.MainActivity.xaml
        }

        public override void OnViewCreated()
        {
        
        }

        public override void OnDestroy()
        {
        
        }

        public override void OnResume()
        {
        
        }
    }
}
