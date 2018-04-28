using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace NaiveRC.NRCTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dt;

        public MainWindow()
        {
            InitializeComponent();
            string lrc = "[00:30.580]编曲 : 郑伟\n\n[00:36.580]如果有人在灯塔\n[00:39.870]拨弄她的头发\n[00:41.980]思念刻在墙和瓦\n[00:45.160]\n[00:45.900]如果感情会挣扎\n[00:48.520]没有说的儒雅\n[00:50.960]把挽回的手放下\n[00:53.910]\n[00:54.560]镜子里的人说假话\n[00:58.530]违心的样子你决定了吗\n[01:02.440]\n[01:03.650]装聋或者作哑 要不我先说话\n[01:11.340]\n[01:15.340]我们的爱情 到这刚刚好\n[01:19.810]剩不多也不少 还能忘掉\n[01:24.960]我应该可以 把自己照顾好\n[01:30.510]\n[01:32.840]我们的距离 到这刚刚好\n[01:37.140]不够我们拥抱 就挽回不了\n[01:41.640]\n[01:42.230]用力爱过的人 不该计较\n[01:48.230]\n[01:58.290]是否要逼人弃了甲\n[02:01.610]亮出一条伤疤\n[02:03.770]不堪的根源在哪\n[02:06.610]\n[02:07.520]可是感情会挣扎\n[02:09.820]没有别的办法\n[02:11.920]它劝你不如退下\n[02:15.660]如果分手太复杂\n[02:18.860]\n[02:19.420]流浪的歌手会放下吉他\n[02:24.450]故事要美必须藏着真话\n[02:29.700]\n[02:31.990]我们的爱情 到这刚刚好\n[02:36.150]剩不多也不少 还能忘掉\n[02:41.200]我应该可以 把自己照顾好\n[02:46.740]\n[02:48.750]我们的距离 到这刚刚好\n[02:52.970]不够我们拥抱 就挽回不了\n[02:57.600]\n[02:58.300]用力爱过的人 不该计较\n[03:03.590]\n[03:05.740]我们的爱情到这刚刚好\n[03:09.860]再不争也不吵 不必再煎熬\n[03:14.530]\n[03:15.100]你可以不用 记得我的好\n[03:20.510]\n[03:22.500]我们的流浪到这刚刚好\n[03:26.640]趁我们还没到 天涯海角\n[03:31.720]我也不是非要去那座城堡\n[03:37.810]\n[03:39.370]天空有些暗了暗的刚刚好\n[03:43.610]我难过的样子就没人看到\n[03:48.850]你别太在意我身上的记号\n[03:58.850]\n[04:07.580]制作 : 薛之谦\n[04:08.580]录音 : 郝宇\n[04:09.580]混音 : 郑伟\n";

            //string nrc = File.ReadAllText("test.txt");
            NRCVIEW.LoadNRC(lrc);


            dt = new DispatcherTimer();

            dt.Interval = TimeSpan.FromMilliseconds(1);

            dt.Tick += (e, c) =>
            {
                UpdatePositionTime();
            };
            me.Source = new Uri(@"薛之谦 - 刚刚好.mp3", UriKind.RelativeOrAbsolute);
            //设置为pause时才能达到启动应用后马上加载音乐以获取总时长
            me.LoadedBehavior = MediaState.Pause;
            //加载音乐后获取总时长
            me.MediaOpened += (e, c) =>
            {
                //将总时长转换为秒单位
                sd.Maximum = (me.NaturalDuration.TimeSpan.Minutes * 60) + me.NaturalDuration.TimeSpan.Seconds;
                //重新设置为手动播放模式，否则无法播放音乐
                me.LoadedBehavior = MediaState.Manual;
            };


            //监听slider的鼠标释放事件，即拖动时不调整音乐的进度，拖动后再调整
            sd.PreviewMouseLeftButtonUp += (e, c) =>
            {
                STime st = GetStime(sd.Value);
                me.Position = new TimeSpan(0, st.m, st.s);

                //NRCView.ResetPositionTime(me.Position.TotalMilliseconds);

            };
            //对slider的值变化监听，修改时可实时显示调整的时间
            sd.ValueChanged += (e, c) =>
            {
                STime st = GetStime(sd.Value);
                TimeSpan ts = new TimeSpan(0, st.m, st.s);
                metime.Text = ts.ToString("mm") + ":" + ts.ToString("ss");
            };
        }

        public void UpdatePositionTime()
        {
            metime.Text = me.Position.ToString("mm") + ":" + me.Position.ToString("ss");

            NRCVIEW.UpdateTime(me.Position.TotalMilliseconds);
        }
        public class STime
        {
            public int m { get; set; }
            public int s { get; set; }
        }
        public STime GetStime(double v)
        {
            var st = new STime();

            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(v));
            //得到多少分钟
            st.m = ts.Minutes;
            st.s = ts.Seconds;
            return st;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            dt.Start();
            me.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            dt.Stop();
            me.Pause();

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("key:" + e.Key);
            switch (e.Key)
            {
                case Key.Left:
                    NRCVIEW.BackWord();
                    break;
                case Key.Right:

                    NRCVIEW.NextWord();
                    break;
                case Key.Space:
                    me.Pause();
                    break;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "";
            //sfd.InitialDirectory = @"C:\";
            sfd.Filter = "NaiveRC歌词文件| *.nrc";
            if (sfd.ShowDialog().Value)
            {
                string path = sfd.FileName;
                NRCVIEW.ExportNRCFile(path);
            }



        }
    }
}
