using NaiveRC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NaiveRC.Utils
{

    public class Lyric
    {
        public NRCModel NRC;

        public Lyric(string str)
        {
            NRC = new NRCModel();
            NRC.NRCS = new List<NRCSentenceModel>();

            LyricType lt = GetLyricType(str);
            NRC.LyricType = lt;
            if(lt== LyricType.LRC)
            {
                LoadLRC(str);
            }
        }
        private void LoadLRC(string str)
        {
            //NRC.NRCS.Clear();
           
            try
            {
              

                //Regex reg = new Regex(@"\[(?<time>([0-9]\d)\:([0-9]\d)\.([0-9]\d))\](?<lrc>.*?)(\n)", RegexOptions.IgnoreCase);
                Regex reg = new Regex(@"\[(?<time>(.*?)\:(.*?)\.(.*?))\](?<lrc>.*?)(\n)", RegexOptions.IgnoreCase);

                // 搜索匹配的字符串
                MatchCollection matches = reg.Matches(str);


                // 取得匹配项列表
                foreach (Match match in matches)
                {
                    string time = match.Groups["time"].Value;

                    string lrc = match.Groups["lrc"].Value;
                    lrc = lrc.Replace(" ", "");

                    Debug.WriteLine(time + "/" + lrc);

                    if (lrc != string.Empty)
                    {
                        NRCSentenceModel nrcsm = new NRCSentenceModel();
                        nrcsm.StartTime = ToDouble(time);
                        nrcsm.NRCWord = new List<NRCWordModel>();

                        //把歌词一个个字切出来
                        for (int i = 0; i < lrc.Length; i++)
                        {
                            nrcsm.NRCWord.Add(new NRCWordModel()
                            {
                                
                                StartTime = ToDouble(time) + (i * 1000),
                                PlayTime = 1000,
                                Word = lrc.Substring(i, 1)
                              
                            });
                        }
                        NRC.NRCS.Add(nrcsm);
                    }
                }
                Debug.WriteLine("加载网易云歌词成功!");

            }
            catch
            {
                Debug.WriteLine("加载网易云歌词失败.");
            }
        }
        public double ToDouble(string time)
        {
            int m = int.Parse(time.Split(':').First());
            int s = int.Parse(time.Split(':').Last().Split('.').First());
            int ms = int.Parse(time.Split('.').Last());

            return new TimeSpan(0, 0, m, s, ms).TotalMilliseconds;
        }

        public LyricType GetLyricType(string str)
        {
            Regex reg = new Regex(@"\[(?<time>(.*?)\:(.*?)\.(.*?))\](?<lrc>.*?)(\n)", RegexOptions.IgnoreCase);
            if (reg.Match(str).Success)
            {
                //网易云格式歌词
                return LyricType.LRC;
            }
            else
            {
                //nrc暂时不做判断
                return LyricType.NRC;
            }
        }
    }
}
