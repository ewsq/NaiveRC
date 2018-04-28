using System;
using System.Collections.Generic;
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

namespace NaiveRC.Sample
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

            Loaded += (e, c) =>
            {
               

                string nrc = File.ReadAllText("ggh.nrc");
                //string nrc = File.ReadAllText("test.txt");
                NRCView.LoadNRC(nrc);
            };

            dt = new DispatcherTimer();

            //dt.Interval = TimeSpan.FromMilliseconds(0.1);//0.1ms瞬间爆炸
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
               
                NRCView.ResetPositionTime(me.Position.TotalMilliseconds);
                //UpdatePositionTime();

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
            metime.Text = me.Position.ToString("mm") + ":" + me.Position.ToString("ss")+"/"+ me.Position.TotalMilliseconds;

            NRCView.UpdatePositionTime(me.Position.TotalMilliseconds);
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
            NRCView.Pause();
        }
    }
}
