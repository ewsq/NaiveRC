using NaiveRC.Models;
using NaiveRC.Utils;
using Newtonsoft.Json;
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

namespace NaiveRC.NRCTool.Controls
{
    /// <summary>
    /// NRCView.xaml 的交互逻辑
    /// </summary>
    public partial class NRCView : UserControl
    {
        List<NRCSentence> NRCSList = new List<NRCSentence>();
        int SelectIndex = 0;
        double PositionTime { get; set; }
        public NRCView()
        {
            InitializeComponent();

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

        private void LoadNRC(NRCModel nrc)
        {
            NRCSList.Clear();
            SP.Children.Clear();

            foreach (NRCSentenceModel nrcsm in nrc.NRCS)
            {
                NRCSentence nrcs = new NRCSentence();
                nrcs.Data = nrcsm;
                nrcs.Index = NRCSList.Count;
                nrcs.Event += NRCSEvent;
                nrcs.StartTime = nrcsm.StartTime;
                SP.Children.Add(nrcs);
                NRCSList.Add(nrcs);
            }
        }

        private void NRCSEvent(NRCSentence.EventType et, int index)
        {
            if (et == NRCSentence.EventType.Top)
            {
                //切上句
                if (index > 0)
                {
                    NRCSList[index - 1].UpdateTime(PositionTime);

                    NRCSList[index - 1].Play();
                    SelectIndex = index - 1;
                }
            }
            else
            {
                //切下句
                if (index + 1 < NRCSList.Count)
                {
                    NRCSList[index + 1].UpdateTime(PositionTime);
                    NRCSList[index + 1].Play(0);
                    SelectIndex = index + 1;

                }
            }
        }

        //private void Play(int index)
        //{
        //    NRCSList[index].Play(0);
        //    SelectIndex = index;
        //}


        public void UpdateTime(double time)
        {
            PositionTime = time;
            NRCSList[SelectIndex].UpdateTime(time);

        }

        public void BackWord()
        {
            NRCSList[SelectIndex].BackWord();
        }

        public void NextWord()
        {
            NRCSList[SelectIndex].NextWord();
        }

        /// <summary>
        /// 保存NRC歌词
        /// </summary>
        /// <param name="savepath"></param>
        public void ExportNRCFile(string savepath)
        {
            NRCModel nm = new NRCModel();
            nm.LyricType = LyricType.NRC;
            nm.NRCS = new List<NRCSentenceModel>();
            foreach (NRCSentence nrcs in NRCSList)
            {
                NRCSentenceModel nrcsm = new NRCSentenceModel();
                nrcsm.StartTime = nrcs.StartTime;
                Debug.WriteLine(nrcs.ToString() + "->" + nrcs.StartTime);
                nrcsm.NRCWord = new List<NRCWordModel>();
                foreach (NRCWord nrcw in nrcs.NWList)
                {

                    nrcsm.NRCWord.Add(nrcw.Data);
                }
                nm.NRCS.Add(nrcsm);
            }

            File.WriteAllText(savepath, JsonConvert.SerializeObject(nm));
        }
    }
}
