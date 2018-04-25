using NaiveRC.ChildControl;
using NaiveRC.Models;
using NaiveRC.Utils;
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

namespace NaiveRC
{

    public class NaiveRCView : Control
    {
       
        ScrollViewer ScrollViewer;
        StackPanel StackPanel;

        List<NRCSentence> NRCSList;

        NRCSentence NowPlayNRCSentence;
        static NaiveRCView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NaiveRCView), new FrameworkPropertyMetadata(typeof(NaiveRCView)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ScrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;
            StackPanel = GetTemplateChild("StackPanel") as StackPanel;
            Load();
            //NRCSList = new List<NRCSentence>();

        }
        void Load()
        {
            NRCSList = new List<NRCSentence>();
        }



        /// <summary>
        /// 加载NRC格式歌词
        /// </summary>
        /// <param name="nrc"></param>
        public void LoadNRC(NRCModel nrc)
        {
            NRCSList.Clear();
            StackPanel.Children.Clear();

            foreach (NRCSentenceModel nrcsm in nrc.NRCS)
            {
                NRCSentence nrcs = new NRCSentence();
                nrcs.StartTime = nrcsm.StartTime;
                nrcs.LyricType =   LyricType.NRC;//测试逐字
                //nrcs.LyricType =  nrc.LyricType;//正常调用
                nrcs.Loaded += (e, c) =>
                {
                    nrcs.LoadNRCWord(nrcsm.NRCWord);
                };
                StackPanel.Children.Add(nrcs);
                NRCSList.Add(nrcs);
            }
        }

        public void UpdatePositionTime(double time)
        {
           
            var d = NRCSList.Where(m => time >= m.StartTime);//cpu <= 1%

           
            if (d.Count() > 0)
            {
                NRCSentence ns = d.Last();
                if (NowPlayNRCSentence != null && NowPlayNRCSentence != ns)
                {
                    //重置前句歌词描色
                    NowPlayNRCSentence.Reset();
                  
                }
                NowPlayNRCSentence = ns;
                ns.UpdatePlayPositionTime(time);
            }
        }


        /// <summary>
        /// 暂停（暂停的同时必须停止更新进度时间，继续播放只要重新调用更新进度时间方法即可
        /// </summary>
        public void Pause()
        {
            NowPlayNRCSentence.Pause();
        }

        /// <summary>
        /// 加载歌词
        /// </summary>
        /// <param name="str"></param>
        public void LoadNRC(string str)
        {
            Lyric lyric = new Lyric(str);
            LoadNRC(lyric.NRC);
        }

        /// <summary>
        /// 重设调整进度
        /// </summary>
        /// <param name="time"></param>
        public void ResetPositionTime(double time)
        {
            var d = NRCSList.Where(m => time >= m.StartTime);
            if (d.Count() > 0)
            {
                NRCSentence ns = d.Last();
                ns.ChangedPlayPosition(time);
            }
        }
    }
}
