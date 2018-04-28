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

namespace NaiveRC.NRCTool.Controls
{
    /// <summary>
    /// NRCSentence.xaml 的交互逻辑
    /// </summary>
    public partial class NRCSentence : UserControl
    {
        public int Index { get; set; }
        public enum EventType
        {
            Top,
            Bottom
        }
        #region 事件
        public delegate void Events(EventType et,int index);
        /// <summary>
        /// 状态通知事件（当切到上一句/下一句时触发
        /// </summary>
        public event Events Event;
        private void OnEvent_(EventType et)
        {

            Event?.Invoke(et,Index);

        }
        #endregion
        public double time { get; set; }
        public double StartTime { get; set; }

        private string str = "";
        public List<NRCWord> NWList = new List<NRCWord>();
        private NRCSentenceModel Data_;
        public NRCSentenceModel Data
        {

            get
            {
                return Data_;
            }
            set
            {
                Data_ = value;
                SP.Children.Clear();
                foreach (NRCWordModel nm in Data_.NRCWord)
                {
                    NRCWord nw = new NRCWord();
                    nw.Data = nm;
                    str += nm.Word;
                    SP.Children.Add(nw);
                    NWList.Add(nw);
                }
            }
        }

        /// <summary>
        /// 当前播放字
        /// </summary>
        private int SelectNRCWordIndex { get; set; } = -1;

        /// <summary>
        /// 更新时间（执行到此句时必须实时调用
        /// </summary>
        /// <param name="time"></param>
        public void UpdateTime(double time)
        {
            this.time = time;
            Debug.WriteLine(ToString()+"->"+this.time);
        }

        public void MarkStart()
        {
            Data.StartTime = time;
        }

        /// <summary>
        /// 上一个字
        /// </summary>
        public void BackWord()
        {
            if (SelectNRCWordIndex >= 0)
            {
                //重置当前字
                NWList[SelectNRCWordIndex].ReMark();

            }
            if (SelectNRCWordIndex <= 0)
            {
                //已经是第一个字的时候通知切到前一句歌词

                OnEvent_(EventType.Top);
            }
            else
            {
               

                //切字
                Play(SelectNRCWordIndex - 1);
            }
        }

        /// <summary>
        /// 下一个字
        /// </summary>
        public void NextWord()
        {

         

            if (SelectNRCWordIndex >= 0)
            {
                //结束标记当前字
                NWList[SelectNRCWordIndex].MarkEnd(time);

            }
           

            if (SelectNRCWordIndex + 1 > (NWList.Count - 1))
            {
                //已经是最后一个字通知切到下一句
                OnEvent_(EventType.Bottom);

            }
            else
            {
                Play(SelectNRCWordIndex + 1);
            }
        }
        public void Play()
        {
            NWList[SelectNRCWordIndex].MarkStart(time);
        }
        /// <summary>
        /// 选择播放字组
        /// </summary>
        /// <param name="index"></param>
        public void Play(int index)
        {
            if (index == 0)
            {
                StartTime = time;
                Debug.WriteLine("标记开始->" + time+"/"+StartTime);
            }
            NWList[index].MarkStart(time);
            SelectNRCWordIndex = index;
        }

        public NRCSentence()
        {
            InitializeComponent();
            
        }

        public override string ToString()
        {
            return str;
        }
    }
}
