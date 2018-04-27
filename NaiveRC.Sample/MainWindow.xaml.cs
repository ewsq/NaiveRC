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
                NRCView.LoadNRC("[00:22.330]听着节奏 就知道\r\n[00:24.400]我来自黄金大街90号\r\n[00:25.500]一所经典 纯粹的老学校\r\n[00:27.290]把你们拉回到那个特别躁的年代\r\n[00:29.860]让你们耳边环绕着 和平 尊重与爱\r\n[00:32.350]说唱的街拍\r\n[00:33.310]在大街小巷里尽兴的摇摆\r\n[00:35.060]根本停不下来的freestyle\r\n[00:36.650]尽管无人喝彩\r\n[00:37.600]无所谓\r\n[00:38.560]我24个小时都在飞\r\n[00:40.180]夜里开个演唱会\r\n[00:41.440]白天挣着生活费\r\n[00:42.560]从不知疲惫\r\n[00:43.550]把音乐开到最高分贝 又是宿醉\r\n[00:45.910]昨晚听着BEATS又喝了太多杯\r\n[00:48.000]我穿着纯色的T 白色的AF1\r\n[00:50.410]NewEra的帽子歪戴着 手表特别闪\r\n[00:52.820]晃瞎了装*者的眼 做事儿的态度很明显\r\n[00:55.530]麦克风在扫射花了那些评论家的脸\r\n[00:58.070]听得懂的往前站 那听不懂的向后转\r\n[01:01.600]我说我想说的 其他的我不管\r\n[01:03.140]YES YES YO YOU DON‘T stop\r\n[01:05.600]你知道 我们就是不服 什么都不怕\r\n[01:08.240]YES YES YO YOU DON’T stop\r\n[01:11.000]你知道 我们就是COOL 我们就是HIPHOP\r\n[01:13.360]YES YES YO YOU DON‘T stop\r\n[01:15.850]你知道 我们就是不服 什么都不怕\r\n[01:18.460]YES YES YO YOU DON’T stop\r\n[01:20.930]你知道 我们就是COOL 我们就是HIPHOP\r\n[01:23.610]当我打开了麦克风嘴巴就开始闯祸\r\n[01:26.120]习惯在大白天睡觉 夜深人静来创作\r\n[01:28.670]生活就是歌词 所以从不担心断货\r\n[01:31.230]十年前起飞到死那天 我才会降落\r\n[01:34.600]老时间老地点老学校在广播\r\n[01:36.350]欢迎收听黄金年代调频90兆赫\r\n[01:38.900]随心所欲的演绎黑色的幽默\r\n[01:41.440]来自街头的角色去**被诱惑\r\n[01:44.050]本就身陷于沼泽又何惧后果\r\n[01:46.590]是个小丑还是野兽我选择后者\r\n[01:49.130]就是说我想说的 要做我想做\r\n[01:51.610]只学会看自己的心情从不看人脸色\r\n[01:54.050]在循规蹈矩之中我是为不速之客\r\n[01:56.600]不需要别人来教我对错或善恶\r\n[01:59.180]与其来给我上课 还不如听我唱歌\r\n[02:01.910]就用这段VERSE帮你们彻底解脱\r\n[02:14.700]让那些所谓的天才们先睡\r\n[02:16.900]我是一只 笨鸟 所以要学会 先飞\r\n[02:19.410]弱肉强食的宴会 不相信眼泪\r\n[02:21.810]这天下没有任何一顿午餐会免费\r\n[02:24.510]哪里还有时间拿来去浪费\r\n[02:27.040]在身后有太多人不择手段想上位\r\n[02:29.560]所以需要誓死守护热爱的岗位\r\n[02:32.150]不管他们付出多少 我都是他们双倍\r\n[02:35.080]没有必要跟我比较 我比较低调\r\n[02:37.650]为人处世有些蹊跷 想法出乎预料\r\n[02:40.160]有着娴熟的技巧 刚刚好的力道\r\n[02:42.900]音乐被我烹调 这世界多么奇妙\r\n[02:45.120]我是个匠人 别叫我领导\r\n[02:47.700]不参加比赛 没想过领跑\r\n[02:50.410]偶尔逞个英雄 来替天行道\r\n[02:52.960]说唱界的小学生 老师你好\r\n[03:05.800]YES YES YO YOU DON‘T stop\r\n[03:08.210]你知道 我们就是不服 什么都不怕\r\n[03:10.810]YES YES YO YOU DON’T stop\r\n[03:13.300]你知道 我们就是COOL 我们就是HIPHOP\r\n[03:15.920]YES YES YO YOU DON‘T stop\r\n[03:18.370]你知道 我们就是不服 什么都不怕\r\n[03:20.990]YES YES YO YOU DON’T stop\r\n[03:23.510]你知道 我们就是COOL 我们就是HIPHOP\r\n[03:26.100]YES YES YO YOU DON‘T stop\r\n[03:28.590]你知道 我们就是不服 什么都不怕\r\n[03:31.190]YES YES YO YOU DON’T stop\r\n[03:33.670]你知道 我们就是COOL 我们就是HIPHOP\r\n[03:36.300]YES YES YO YOU DON‘T stop\r\n[03:38.800]你知道 我们就是不服 什么都不怕\r\n[03:41.420]YES YES YO YOU DON’_-'T stop\r\n[03:43.870]你知道 我们就是COOL 我们就是HIPHOP\r\n[03:46.540]YES YES YO YOU DON‘T stop\r\n[03:49.050]你知道 我们就是不服 什么都不怕\r\n[03:51.640]YES YES YO YOU DON’T stop\r\n[03:54.180]你知道 我们就是COOL 我们就是HIPHOP\n");
            };

            dt = new DispatcherTimer();

            dt.Interval = TimeSpan.FromSeconds(0.1);

            dt.Tick += (e, c) =>
            {
                UpdatePositionTime();
            };
            me.Source = new Uri(@"C:\CloudMusic\龙井说唱 - 回到未来.mp3", UriKind.RelativeOrAbsolute);
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
            NRCView.Pause();
        }
    }
}
