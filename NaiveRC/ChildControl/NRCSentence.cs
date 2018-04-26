using NaiveRC.Models;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NaiveRC.ChildControl
{
    /// <summary>
    /// 歌词句，每一句歌词分一行
    /// </summary>
    class NRCSentence : Control
    {
        #region StartTime 开始时间（单位毫秒
        public double StartTime { get; set; }
        ///// <summary>
        ///// 开始时间（单位毫秒
        ///// </summary>
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
        #endregion

        //#region PositionTime 当前进度时间（单位毫秒
        ///// <summary>
        ///// 当前进度时间（单位毫秒
        ///// </summary>
        //public double PositionTime
        //{
        //    get { return (double)GetValue(PositionTimeProperty); }
        //    set
        //    {
        //        SetValue(PositionTimeProperty, value);
        //    }
        //}
        //public static readonly DependencyProperty PositionTimeProperty =
        //    DependencyProperty.Register("PositionTime", typeof(double), typeof(NRCWord)
        //        );
        //#endregion

        #region LyricType 歌词类型
        public LyricType LyricType { get; set; }
        ///// <summary>
        ///// 当前进度时间（单位毫秒
        ///// </summary>
        //public LyricType LyricType
        //{
        //    get { return (LyricType)GetValue(LyricTypeProperty); }
        //    set
        //    {
        //        SetValue(LyricTypeProperty, value);
        //    }
        //}
        //public static readonly DependencyProperty LyricTypeProperty =
        //    DependencyProperty.Register("LyricType", typeof(LyricType), typeof(NRCWord)
        //        );
        #endregion

        
        /// <summary>
        /// 普通歌词上色状态，false未上色，true已上色
        /// </summary>
        bool IsLRCColored = false;

        StackPanel StackPanel;

        List<NRCWord> NRCWordList = new List<NRCWord>();

        /// <summary>
        /// 当前正在描色的字
        /// </summary>
        NRCWord NowPlayNRCWord;

        string Words = "";
        static NRCSentence()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NRCSentence), new FrameworkPropertyMetadata(typeof(NRCSentence)));
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            StackPanel = GetTemplateChild("StackPanel") as StackPanel;



        }


        /// <summary>
        /// 加载歌词文字
        /// </summary>
        /// <param name="d"></param>
        public void LoadNRCWord(List<NRCWordModel> d)
        {

            NRCWordList.Clear();
            StackPanel.Children.Clear();

            foreach (NRCWordModel w in d)
            {
                NRCWord nrcword = new NRCWord();
                nrcword.Word = w.Word;
                nrcword.PlayTime = w.PlayTime;
                nrcword.StartTime = w.StartTime;
                //将文字加入控件
                StackPanel.Children.Add(nrcword);
                //保存控件集合
                NRCWordList.Add(nrcword);
                Words += w.Word;
            }
        }

        /// <summary>
        /// 正常播放更新进度时间（当进度到此句歌词时一直调用此方法保持更新文字描色
        /// </summary>
        /// <param name="PositionTime"></param>
        public void UpdatePlayPositionTime(double PositionTime)
        {
            if (LyricType == LyricType.LRC)
            {

                if (IsLRCColored == false)
                {
                    //给所有文字上色
                    foreach (NRCWord w in NRCWordList)
                    {
                        w.SetColor();
                    }
                    IsLRCColored = true;
                }
            }
            else
            {
                var w = NRCWordList.Where(m => PositionTime >= m.StartTime);
                if (w.Count() > 0)
                {
                    //取最后一个
                    NRCWord nrcword = w.Last();
                    //nrcword.Position = PositionTime;
                    nrcword.Play(PositionTime);
                    //记录当前描色的字
                    NowPlayNRCWord = nrcword;

                }
            }
        }

        /// <summary>
        /// 修改播放进度
        /// </summary>
        public void ChangedPlayPosition(double PositionTime)
        {

            if (LyricType == LyricType.LRC)
            {
                foreach (NRCWord w in NRCWordList)
                {
                    //整句描色
                    w.SetColor();
                }
            }
            else
            {

                foreach (NRCWord w in NRCWordList)
                {
                    //整句重置描色
                    w.Reset();
                    if (PositionTime >= (w.StartTime + w.PlayTime))
                    {
                        //如果播放进度超过字的进度直接上色
                        w.SetColor();
                    }
                    else
                    {
                        //Debug.WriteLine("nrcs cp:"+w.Word);

                        w.ChangedPosition(PositionTime);
                    }
                }
               

            }
        }

        /// <summary>
        /// 暂停（暂停的同时必须停止更新进度时间，继续播放只要重新调用更新进度时间方法即可
        /// </summary>
        public void Pause()
        {
            if (NowPlayNRCWord != null)
            {
                NowPlayNRCWord.Pause();
            }
        }

        public void Reset()
        {
            IsLRCColored = false;
            foreach (NRCWord w in NRCWordList)
            {
                //整句重置描色
                w.Reset();
            }
        }

        /// <summary>
        /// 获取完整的歌词
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Words;
        }
    }
}
