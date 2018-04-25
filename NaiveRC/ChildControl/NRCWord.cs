using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NaiveRC.ChildControl
{
    /// <summary>
    /// 歌词文字控件
    /// </summary>
    class NRCWord : Control
    {
        #region WordColor 默认字体颜色
        /// <summary>
        /// 默认字体颜色
        /// </summary>
        public SolidColorBrush WordColor
        {
            get { return (SolidColorBrush)GetValue(WordColorProperty); }
            set
            {
                SetValue(WordColorProperty, value);
            }
        }
        public static readonly DependencyProperty WordColorProperty =
            DependencyProperty.Register("WordColor", typeof(SolidColorBrush), typeof(NRCWord),
                new PropertyMetadata(
                    defaultValue: new SolidColorBrush(Colors.Black),
                    propertyChangedCallback: null,
                    coerceValueCallback: null
                    )
                );
        #endregion

        #region PlayColor 吟唱字体颜色
        /// <summary>
        /// 吟唱字体颜色
        /// </summary>
        public SolidColorBrush PlayColor
        {
            get { return (SolidColorBrush)GetValue(PlayColorProperty); }
            set
            {
                SetValue(PlayColorProperty, value);
            }
        }
        public static readonly DependencyProperty PlayColorProperty =
            DependencyProperty.Register("PlayColor", typeof(SolidColorBrush), typeof(NRCWord),
                new PropertyMetadata(
                    defaultValue: new SolidColorBrush(Colors.Green),
                    propertyChangedCallback: null,
                    coerceValueCallback: null
                    )
                );
        #endregion

        #region Word 文字
        /// <summary>
        /// 文字
        /// </summary>
        public string Word
        {
            get { return (string)GetValue(WordProperty); }
            set
            {
                SetValue(WordProperty, value);
            }
        }
        public static readonly DependencyProperty WordProperty =
            DependencyProperty.Register("Word", typeof(string), typeof(NRCWord),
                new PropertyMetadata(
                    defaultValue: "正",
                    propertyChangedCallback: null,
                    coerceValueCallback: null
                    )
                );
        #endregion

        #region PositionTime 当前进度时间（单位毫秒
        /// <summary>
        /// 当前进度时间（单位毫秒
        /// </summary>
        public double PositionTime
        {
            get { return (double)GetValue(PositionTimeProperty); }
            set
            {
                SetValue(PositionTimeProperty, value);
            }
        }
        public static readonly DependencyProperty PositionTimeProperty =
            DependencyProperty.Register("PositionTime", typeof(double), typeof(NRCWord)
                );
        #endregion

        #region PlayTime 吟唱时间（单位毫秒
        /// <summary>
        /// 吟唱时间（单位毫秒
        /// </summary>
        public double PlayTime
        {
            get { return (double)GetValue(PlayTimeProperty); }
            set
            {
                SetValue(PlayTimeProperty, value);
            }
        }
        public static readonly DependencyProperty PlayTimeProperty =
            DependencyProperty.Register("PlayTime", typeof(double), typeof(NRCWord)
                );
        #endregion

        #region StartTime 开始时间（单位毫秒
        /// <summary>
        /// 开始时间（单位毫秒
        /// </summary>
        //public double StartTime
        //{
        //    get { return (double)GetValue(StartTimeProperty); }
        //    set
        //    {
        //        SetValue(StartTimeProperty, value);
        //    }
        //}
        //public static readonly DependencyProperty StartTimeProperty =
        //    DependencyProperty.Register("StartTime", typeof(double), typeof(NRCWord)
        //        );
        public double StartTime { get; set; }
        #endregion

        /// <summary>
        /// 是否正在进行描色，false否，true是
        /// </summary>
        public bool IsPlay { get; set; }


        TextBlock ColorTextBlock, WordTextBlock;
        DoubleAnimation widthAnimation;
        Storyboard storyboard;

        static NRCWord()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NRCWord), new FrameworkPropertyMetadata(typeof(NRCWord)));

        }
        /// <summary>
        /// 初始化
        /// </summary>
        void Load()
        {
            widthAnimation = new DoubleAnimation();
            widthAnimation.Completed += widthAnimation_Completed;
           
            storyboard = new Storyboard();

            //如果描色动画不流畅可提高帧率，随之cpu占用也会提高，qq音乐能做到<1%真的牛逼
            Timeline.SetDesiredFrameRate(storyboard, 10);//cpu<=1.1%

            //Timeline.SetDesiredFrameRate(storyboard, 20);//cpu<=1.5%

            //Timeline.SetDesiredFrameRate(storyboard, 60);//默认60帧时 cpu<=5% 非常夸张

            storyboard.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, ColorTextBlock);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath("(TextBlock.Width)"));
           
         
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ColorTextBlock = GetTemplateChild("ColorTextBlock") as TextBlock;
            WordTextBlock = GetTemplateChild("WordTextBlock") as TextBlock;
            Load();
        }

        /// <summary>
        /// 重新开始描色
        /// </summary>
        public void RePlay()
        {
            if (IsPlay == false && ColorTextBlock != null)
            {
                IsPlay = true;
                widthAnimation.From = 0;
                widthAnimation.To = WordTextBlock.ActualWidth;
                widthAnimation.Duration = TimeSpan.FromMilliseconds(PlayTime);

                storyboard.Begin();
                //ColorTextBlock.BeginAnimation(WidthProperty, widthAnimation);
            }
        }

        private void widthAnimation_Completed(object sender, EventArgs e)
        {
            //播放完毕重置状态
            IsPlay = false;
        }

        /// <summary>
        /// 从当前进度开始播放动画
        /// </summary>
        public void Play()
        {
           
            if (IsPlay == false && ColorTextBlock != null)
            {
                IsPlay = true;
                //动画所需时间（描色时间
                double antime = 0;
                //已播放的时间
                double playedtime = PositionTime - StartTime;
                double hastime = PlayTime - playedtime;
                if (hastime > 0)
                {
                    antime = hastime;
                }
                Debug.WriteLine("[NRCWord->Play] word:"+Word+",antime:" + antime + ",playedtime:" + playedtime + ",hastime:" + hastime);
                widthAnimation.To = WordTextBlock.ActualWidth;
                widthAnimation.Duration = TimeSpan.FromMilliseconds(antime);

                storyboard.Begin();
            }
        }

        public void Stop()
        {
            storyboard.Stop();
        }

        /// <summary>
        /// 进度调整定位
        /// </summary>
        public void ChangedPosition()
        {
            IsPlay = false;
            //获得已播放时间
            double playedtime = PositionTime - StartTime;
            if (playedtime > 0)
            {
                //如果当前字已经播放的话就计算进度设置描色的位置
                ColorTextBlock.Width = (WordTextBlock.ActualWidth / PlayTime) * playedtime;
            }
            Debug.WriteLine("[NRCWord->ChangedPosition] playedtime:" + playedtime);

        }
        /// <summary>
        /// 重置描色
        /// </summary>
        public void Reset()
        {
            IsPlay = false;
            Stop();
            ColorTextBlock.Width = 0;
        }

        /// <summary>
        /// 普通歌词直接上色
        /// </summary>
        public void SetColor()
        {
            ColorTextBlock.Width = WordTextBlock.ActualWidth;
        }
    }
}
