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
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Notifications;
using System.Threading.Tasks;

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
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)  //页面加载完毕
        {
            GetSettings();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;//每秒触发这个事件，以刷新时间
            timer.Start();  //开始计时器
        }

        private async void Timer_Tick(object sender, object e)
        {
            if (isAutoAddOneSecondOpen.IsOn)
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
            SaveSettings();
        }

        private void ApplyBackGroungColor_Click(object sender, RoutedEventArgs e)
        {
            //应用背景颜色
            SaveSettings();
        }

        private void ApplyFontColor_Click(object sender, RoutedEventArgs e)
        {
            //应用字体颜色
            SaveSettings();
        }

        private async void SaveSettings()    //保存设置
        {
            StorageFolder folder;
            folder = ApplicationData.Current.RoamingFolder; //获取应用目录的文件夹
            try
            {
                var file_demonstration = await folder.CreateFileAsync("settings", CreationCollisionOption.ReplaceExisting);
                //创建文件


                using (Stream file = await file_demonstration.OpenStreamForWriteAsync())
                {
                    using (StreamWriter write = new StreamWriter(file))
                    {
                        write.Write(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}",
                                                    isfullScreen.IsOn,
                                                    isAutoAddOneSecondOpen.IsOn,
                                                    BackGroundColorRedSlider.Value,
                                                    BackGroundColorGreenSlider.Value,
                                                    BackGroundColorBlueSlider.Value,
                                                    FontColorRedSlider.Value,
                                                    FontColorGreenSlider.Value,
                                                    FontColorBlueSlider.Value,
                                                    isTileFresh.IsOn
                                                   ));
                    }
                }
            }
            catch { }

        }


        private async void GetSettings()
        {
            BackGroundColorRedSlider.Value = 255;
            BackGroundColorGreenSlider.Value = 255;
            BackGroundColorBlueSlider.Value = 255;


            StorageFolder folder;
            folder = ApplicationData.Current.RoamingFolder; //获取应用目录的文件夹

            var file_demonstration = await folder.CreateFileAsync("settings", CreationCollisionOption.OpenIfExists);
            //创建文件

            string s;

            using (Stream file = await file_demonstration.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(file))
                {
                    s = read.ReadToEnd();
                }
            }
            
            
            if (s.IndexOf(";") >= 1 && s.IndexOf(";") != s.Length - 1)
            {
                string[] str2;
                int count_temp = 0;
                str2 = s.Split(';');
                foreach (string i in str2)
                {
                    if (count_temp == 0)
                    {
                        if (i.ToString() == "True")
                        {
                            isfullScreen.IsOn = true;
                        }
                        else
                        {
                            isfullScreen.IsOn = false;
                        }
                        count_temp++;
                    }
                    else if (count_temp == 1)
                    {
                        if (i.ToString() == "True")
                        {
                            isAutoAddOneSecondOpen.IsOn = true;
                        }
                        else
                        {
                            isAutoAddOneSecondOpen.IsOn = false;
                        }
                        count_temp++;
                    }
                    else if (count_temp == 2)
                    {
                        BackGroundColorRedSlider.Value = double.Parse(i.ToString());
                        count_temp++;
                    }
                    else if (count_temp == 3)
                    {
                        BackGroundColorGreenSlider.Value = double.Parse(i.ToString());
                        count_temp++;
                    }
                    else if (count_temp == 4)
                    {
                        BackGroundColorBlueSlider.Value = double.Parse(i.ToString());
                        count_temp++;
                    }
                    else if (count_temp == 5)
                    {
                        FontColorRedSlider.Value = double.Parse(i.ToString());
                        count_temp++;
                    }
                    else if (count_temp == 6)
                    {
                        FontColorGreenSlider.Value = double.Parse(i.ToString());
                        count_temp++;
                    }
                    else if (count_temp == 7)
                    {
                        FontColorBlueSlider.Value = double.Parse(i.ToString());
                        count_temp++;
                    }
                    else if (count_temp == 8)
                    {
                        if (i.ToString() == "True")
                        {
                            isTileFresh.IsOn = true;
                        }
                        else
                        {
                            isTileFresh.IsOn = false;
                        }
                        count_temp++;
                    }
                }
            }
            SolidColorBrush color = new SolidColorBrush(Color.FromArgb(255, (byte)FontColorRedSlider.Value, (byte)FontColorGreenSlider.Value, (byte)FontColorBlueSlider.Value));
            secondsShow.Foreground = color;
            secondGet.Foreground = color;
            addedOneSecondTextBlock.Foreground = color;
            settings.Foreground = color;
            SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(255, (byte)BackGroundColorRedSlider.Value, (byte)BackGroundColorGreenSlider.Value, (byte)BackGroundColorBlueSlider.Value));
            mainGrid.Background = color2;    //应用背景颜色
        }

        private void isAutoAddOneSecondOpen_Toggled(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        private async void BackGroundColorRedSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SolidColorBrush color = new SolidColorBrush(Color.FromArgb(255, (byte)BackGroundColorRedSlider.Value, (byte)BackGroundColorGreenSlider.Value, (byte)BackGroundColorBlueSlider.Value));
            mainGrid.Background = color;    //应用背景颜色
            await Task.Delay(1);
        }

        private void BackGroundColorGreenSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SolidColorBrush color = new SolidColorBrush(Color.FromArgb(255, (byte)BackGroundColorRedSlider.Value, (byte)BackGroundColorGreenSlider.Value, (byte)BackGroundColorBlueSlider.Value));
            mainGrid.Background = color;    //应用背景颜色
        }

        private void BackGroundColorBlueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SolidColorBrush color = new SolidColorBrush(Color.FromArgb(255, (byte)BackGroundColorRedSlider.Value, (byte)BackGroundColorGreenSlider.Value, (byte)BackGroundColorBlueSlider.Value));
            mainGrid.Background = color;    //应用背景颜色
        }

        private void FontColorRedSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SolidColorBrush color = new SolidColorBrush(Color.FromArgb(255, (byte)FontColorRedSlider.Value, (byte)FontColorGreenSlider.Value, (byte)FontColorBlueSlider.Value));
            secondsShow.Foreground = color;    //应用字体颜色
            secondGet.Foreground = color;
            addedOneSecondTextBlock.Foreground = color;
            settings.Foreground = color;
        }

        private void FontColorGreenSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SolidColorBrush color = new SolidColorBrush(Color.FromArgb(255, (byte)FontColorRedSlider.Value, (byte)FontColorGreenSlider.Value, (byte)FontColorBlueSlider.Value));
            secondsShow.Foreground = color;    //应用字体颜色
            secondGet.Foreground = color;
            addedOneSecondTextBlock.Foreground = color;
            settings.Foreground = color;
        }

        private void FontColorBlueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SolidColorBrush color = new SolidColorBrush(Color.FromArgb(255, (byte)FontColorRedSlider.Value, (byte)FontColorGreenSlider.Value, (byte)FontColorBlueSlider.Value));
            secondsShow.Foreground = color;    //应用字体颜色
            secondGet.Foreground = color;
            addedOneSecondTextBlock.Foreground = color;
            settings.Foreground = color;
        }


        private async void isTileFresh_Toggled(object sender, RoutedEventArgs e)
        {
            if (isTileFresh.IsOn)
            {
                await setLiveTile();
            }
            else
            {
                setLiveTileStop();
            }
            SaveSettings();
        }

        private void setLiveTileStop()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.StopPeriodicUpdate();
        }


        private async Task setLiveTile()
        {
            try
            {
                string allSecondsString = await webLib.HttpGet("https://angry.im/l/life");  //获取秒数
                long allSeconds = long.Parse(allSecondsString);   //转换成long
                long dd, mm, hh, ss;     //用于存储最终数值
                dd = allSeconds / 60 / 60 / 24;
                hh = allSeconds / 60 / 60 % 24;
                mm = allSeconds / 60 % 60;
                ss = allSeconds % 60;

                var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text01);

                var tileAttributes = tileXml.GetElementsByTagName("text");
                tileAttributes[0].AppendChild(tileXml.CreateTextNode("Total time"));
                tileAttributes[1].AppendChild(tileXml.CreateTextNode(dd + "d"));
                tileAttributes[2].AppendChild(tileXml.CreateTextNode(hh + "h"));
                tileAttributes[3].AppendChild(tileXml.CreateTextNode(mm + "m"));
                var tileNotification = new TileNotification(tileXml);
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
            catch { }

            var tileContent = new Uri("https://www.chenxublog.com/getsecond.php");  //自建网站
            var requestedInterval = PeriodicUpdateRecurrence.HalfHour;   //半小时一次

            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.StartPeriodicUpdate(tileContent, requestedInterval);
            updater.StopPeriodicUpdate();
        }
    }
}
