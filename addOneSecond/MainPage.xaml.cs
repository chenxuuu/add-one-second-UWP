using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace addOneSecond
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer;//定义定时器

        public MainPage()
        {
            this.InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;//每秒触发这个事件，以刷新时间
            timer.Start();  //开始计时器
        }

        private async void Timer_Tick(object sender, object e)
        {
            if (isAotuAddOneSecondOpen.IsOn)
            {
                this.addedOneSecondStoryboard.Begin();  //+1s动画
                try
                {
                    await webLib.HttpPost("https://angry.im/p/life", "+1s");  //post用来+1s
                }
                catch { }
            }
            string result;
            try
            {
                string allSecondsString = await webLib.HttpGet("https://angry.im/l/life");  //获取秒数
                long allSeconds = long.Parse(allSecondsString);   //转换成long
                long dd, mm, hh, ss;     //用于存储最终数值
                dd = allSeconds / 60 / 60 / 24;
                hh = allSeconds / 60 / 60 % 24;
                mm = allSeconds / 60 % 60;
                ss = allSeconds % 60;
                result = $"{dd}d{hh.ToString().PadLeft(2)}h{mm.ToString().PadLeft(2)}m{ss.ToString().PadLeft(2)}s";
                secondsShow.Text = result;  //显示结果
            }
            catch { }
        }

        private async void secondGet_Click(object sender, RoutedEventArgs e)
        {
            this.addedOneSecondStoryboard.Begin();  //+1s动画
            try
            {
                await webLib.HttpPost("https://angry.im/p/life", "+1s");  //post用来+1s
            }
            catch { }
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;  //开关SplitView设置页
        }


        private void isfullScreen_Toggled(object sender, RoutedEventArgs e)
        {
            if (isfullScreen.IsOn)
            {
                Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryEnterFullScreenMode();  //全屏
            }
            else
            {
                Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().ExitFullScreenMode();
            }
        }

        private void ApplyBackGroungColor_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush color = new SolidColorBrush(Color.FromArgb(255, (byte)BackGroundColorRedSlider.Value, (byte)BackGroundColorGreenSlider.Value, (byte)BackGroundColorBlueSlider.Value));
            mainGrid.Background = color;    //应用背景颜色
        }

        private void ApplyFontColor_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush color = new SolidColorBrush(Color.FromArgb(255, (byte)FontColorRedSlider.Value, (byte)FontColorGreenSlider.Value, (byte)FontColorBlueSlider.Value));
            secondsShow.Foreground = color;
            secondGet.Foreground = color;
            addedOneSecondTextBlock.Foreground = color;
            settings.Foreground = color;
        }
    }
}
