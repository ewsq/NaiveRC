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



        #region PlayTime 吟唱时间（单位毫秒
        ///// <summary>
        ///// 吟唱时间（单位毫秒
        ///// </summary>
        //public double PlayTime
        //{
        //    get { return (double)GetValue(PlayTimeProperty); }
        //    set
        //    {
        //        SetValue(PlayTimeProperty, value);
        //    }
        //}
        //public static readonly DependencyProperty PlayTimeProperty =
        //    DependencyProperty.Register("PlayTime", typeof(double), typeof(NRCWord)
        //        );
        public double PlayTime { get; set; }
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
        //public bool IsPlay { get; set; }


        TextBlock ColorTextBlock, WordTextBlock;
        DoubleAnimation widthAnimation;
        Storyboard storyboard;
        AnimationState animationState = AnimationState.Stop;

        /// <summary>
        /// 描色动画状态
        /// </summary>
        enum AnimationState
        {
            /// <summary>
            /// 执行中
            /// </summary>
            Play,
            /// <summary>
            /// 暂停中
            /// </summary>
            Pause,
            /// <summary>
            /// 已停止
            /// </summary>
            Stop
        }
        static NRCWord()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NRCWord), new FrameworkPropertyMetadata(typeof(NRCWord)));

        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ColorTextBlock = GetTemplateChild("ColorTextBlock") as TextBlock;
            WordTextBlock = GetTemplateChild("WordTextBlock") as TextBlock;
            Load();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        void Load()
        {
            widthAnimation = new DoubleAnimation();

            //widthAnimation.FillBehavior = FillBehavior.HoldEnd;
            storyboard = new Storyboard();
            storyboard.Completed += storyboard_Completed;

            //如果描色动画不流畅可提高帧率，随之cpu占用也会提高，默认帧率在描色时cpu<=8%，丧心病狂。而调整帧率可能会出现的bug是暂停时视觉无法暂停，不能准确停止，导致描色会描完字

            //Timeline.SetDesiredFrameRate(storyboard, 40);//cpu<=1.5%

            //Timeline.SetDesiredFrameRate(storyboard, 10);//cpu<=1.1%，

            //Timeline.SetDesiredFrameRate(storyboard, 20);//cpu<=1.5%

            //Timeline.SetDesiredFrameRate(storyboard, 60);//默认60帧时 cpu<=5% 非常夸张

            storyboard.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, ColorTextBlock);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath("(TextBlock.Width)"));

        }
        /// <summary>
        /// 设置描色状态
        /// </summary>
        /// <param name="as_"></param>
        private void SetState(AnimationState as_)
        {



            switch (as_)
            {
                case AnimationState.Pause:
                    if (animationState != AnimationState.Stop)
                    {
                        storyboard.Pause();
                    }

                    break;
                case AnimationState.Play:

                    if (animationState == AnimationState.Pause)
                    {
                        storyboard.Resume();
                    }
                    else
                    {
                        storyboard.Begin();
                    }
                    break;
                case AnimationState.Stop:

                    
                        storyboard.Stop();
                    

                    break;
            }
            animationState = as_;
        }




        private void storyboard_Completed(object sender, EventArgs e)
        {


            animationState = AnimationState.Stop;
            Debug.WriteLine("[NRCWord->storyboard_Completed] word:" + Word);
        }

        /// <summary>
        /// 从当前进度开始播放动画
        /// </summary>
        public void Play(double PositionTime)
        {


            ////获得已播放时间
            //double hastime = (PositionTime - StartTime);
            //if (hastime > 0)
            //{
            //    ////如果当前字已经播放的话就计算进度设置描色的位置
            //    ColorTextBlock.Width = (WordTextBlock.ActualWidth / PlayTime) * hastime;



            //}


            if (animationState != AnimationState.Play && ColorTextBlock != null)
            {

                //动画所需时间（描色时间
                double antime = 0;
                //已播放的时间
                double playedtime = PositionTime - StartTime;
                double hastime = PlayTime - playedtime;
                if (hastime > 0)
                {
                    //IsPlay = true;
                    antime = hastime;
                    Debug.WriteLine("[NRCWord->Play] word:" + Word + ",antime:" + antime + ",playedtime:" + playedtime + ",hastime:" + hastime);
                    widthAnimation.To = WordTextBlock.ActualWidth;
                    widthAnimation.Duration = TimeSpan.FromMilliseconds(antime);
                    //animationState = AnimationState.Play;
                    //storyboard.Begin();
                    SetState(AnimationState.Play);
                }



            }
        }

        public void Pause()
        {
            //IsPlay = false;

            //animationState = AnimationState.Pause;
            //storyboard.Pause();
            SetState(AnimationState.Pause);

            Debug.WriteLine("[NRCWord->Pause] word:" + Word);
        }

        /// <summary>
        /// 进度调整定位
        /// </summary>
        public void ChangedPosition(double positiontime)
        {

            //IsPlay = false;
            //storyboard.Stop();

            //SetState(AnimationState.Stop);

           
            if (animationState != AnimationState.Stop)
            {
                double hastime = PlayTime - (positiontime - StartTime);
                if (hastime > 0)
                {

                    storyboard.Seek(TimeSpan.FromMilliseconds(hastime));
                }
            }
            else
            {
                //获得已播放时间
                double playedtime = (positiontime - StartTime);
                if (playedtime > 0)
                {
                    //如果当前字已经播放的话就计算进度设置描色的位置
                    ColorTextBlock.Width = (WordTextBlock.ActualWidth / PlayTime) * playedtime;
                    Debug.WriteLine("[NRCWord->ChangedPosition] playedtime:" + playedtime + ",word:" + Word);

                }
            }

            //Debug.WriteLine("[NRCWord->ChangedPosition] hastime:" + hastime);

        }
        /// <summary>
        /// 重置描色
        /// </summary>
        public void Reset()
        {
            //IsPlay = false;
            //storyboard.Stop();
            SetState(AnimationState.Stop);
            widthAnimation.From = 0;
            ColorTextBlock.Width = 0;
            //Debug.WriteLine("重置描色:word:" + Word + ",colorw:" + ColorTextBlock.ActualWidth);

        }

        /// <summary>
        /// 普通歌词直接上色
        /// </summary>
        public void SetColor()
        {
            if (animationState == AnimationState.Stop)
            {
                ColorTextBlock.Width = WordTextBlock.ActualWidth;
                Debug.Write("stop");
            }
            else
            {
                storyboard.Seek(TimeSpan.FromMilliseconds(0));
                //SetState(AnimationState.Stop);
                //Reset();
            }
        }
    }
}
