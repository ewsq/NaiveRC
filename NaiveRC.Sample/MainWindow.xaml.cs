using System;
using System.Collections.Generic;
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
                NRCView.LoadNRC("[00:12.750] 我唱得不够动人你别皱眉\n[00:18.350] 我愿意和你约定至死\n[00:24.400] 我只想嬉戏唱游到下世纪\n[00:30.608] 请你别嫌我将这煽情奉献给你\n[00:37.859] 还能凭甚么\n[00:40.529] 拥抱若未能令你兴奋\n[00:43.949] 便宜地唱出写在情歌的性感\n[00:50.799] 还能凭甚么要是爱不可感动人\n[00:57.290] 俗套的歌词煽动你恻忍\n[01:03.349] 谁人又相信一世一生这肤浅对白\n[01:10.790] 来吧送给你叫几百万人流泪过的歌\n[01:17.490] 如从未听过誓言如幸福摩天轮\n[01:23.499] 才令我因你要呼天叫地爱爱爱爱那么多\n[01:30.099] 将我漫天心血一一抛到银河\n[01:38.678] 谁是垃圾谁不舍我难过分一丁目赠我\n[01:49.519]\n[01:58.990] 我唱出心里话时眼泪会流\n[02:09.290] 要是怕难过抱住我手\n[02:13.600] 我只得千语万言放在你心\n[02:20.410] 比渴望地老天荒更简单未算罕有\n[02:26.900] 谁人又相信一世一生这肤浅对白\n[02:33.390] 来吧送给你叫几百万人流泪过的歌\n[02:39.000] 如从未听过誓言如幸福摩天轮\n[02:44.710] 才令我因你要呼天叫地爱爱爱爱那么多\n[02:51.200] 给你用力作二十首不舍不弃\n[02:55.520] 还附送你爱得过火\n[02:58.400] 给你卖力唱二十首真心真意\n[03:01.100] 米高峰都因我动容\n[03:04.330] 无人及我\n[03:06.290] 你怎么竟然说k歌之王是你\n[03:10.400]\n[03:17.750] 我只想跟你未来浸在爱河\n[03:25.270] 而你那呵欠绝得不能绝\n[03:28.960] 绝到溶掉我\n");
            };

            dt = new DispatcherTimer();

            dt.Interval = TimeSpan.FromSeconds(0.1);

            dt.Tick += (e, c) =>
            {
                UpdatePositionTime();
            };
            me.Source = new Uri(@"C:\360安全浏览器下载\x.mp3", UriKind.RelativeOrAbsolute);
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
            metime.Text = me.Position.ToString("mm") + ":" + me.Position.ToString("ss");

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
        }
    }
}
