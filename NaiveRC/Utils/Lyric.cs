using NaiveRC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            if (lt == LyricType.LRC)
            {
                LoadLRC(str);
                Export();
            }
            else
            {
                LoadNRC(str);
            }
        }

        #region 加载LRC歌词
        private void LoadLRC(string str)
        {
            //NRC.NRCS.Clear();

            try
            {


                //Regex reg = new Regex(@"\[(?<time>([0-9]\d)\:([0-9]\d)\.([0-9]\d))\](?<lrc>.*?)(\n)", RegexOptions.IgnoreCase);
                Regex reg = new Regex(@"\[(?<time>(.*?)\:(.*?)\.(.*?))\](?<lrc>.*?)(\r|\n)", RegexOptions.IgnoreCase);

                // 搜索匹配的字符串
                MatchCollection matches = reg.Matches(str);


                // 取得匹配项列表
                foreach (Match match in matches)
                {
                    string time = match.Groups["time"].Value;

                    string lrc = match.Groups["lrc"].Value;
                    //lrc = lrc.Replace(" ", "");

                    Debug.WriteLine(time + "/" + lrc);

                    if (lrc != string.Empty)
                    {
                        NRCSentenceModel nrcsm = new NRCSentenceModel();
                        nrcsm.StartTime = ToDouble(time);
                        nrcsm.NRCWord = new List<NRCWordModel>();


                        //把歌词一个个字切出来

                        for (int i = 0; i < lrc.Length; i++)
                        {
                            //nrcsm.NRCWord.Add();
                            string c = lrc.Substring(i, 1);
                            CharType ct = GetCharType(c);

                            NRCWordModel nrcwm = new NRCWordModel();
                            nrcwm.PlayTime = 1000;//测试数据，默认吟唱时间1秒
                            nrcwm.StartTime = nrcsm.StartTime + (i * 1000);//测试数据，字开始时间
                            nrcwm.Word = c;

                            //什么鸡掰代码 写完自己都看晕了
                            if (i == 0 || ct == CharType.Chinese)
                            {

                                //创建新字组

                                nrcsm.NRCWord.Add(nrcwm);
                            }
                            else if (ct == CharType.English)
                            {


                                CharType ctlast = GetCharType(lrc.Substring((i - 1), 1));
                                if (ctlast != CharType.Chinese && ctlast != CharType.Space)
                                {


                                    nrcsm.NRCWord[nrcsm.NRCWord.Count - 1].Word += c;
                                    Debug.WriteLine("char:" + c + ",last char:" + lrc.Substring((i - 1), 1) + ",laststr:" + nrcsm.NRCWord[nrcsm.NRCWord.Count - 1].Word);
                                }
                                else
                                {
                                    //创建新字组
                                    nrcsm.NRCWord.Add(nrcwm);
                                }
                            }
                            else if (ct == CharType.Space || ct == CharType.Symbol)
                            {

                                nrcsm.NRCWord[nrcsm.NRCWord.Count - 1].Word += c;
                            }

                            //Debug.WriteLine(c + "->" + ct);

                        }
                        //加入歌词
                        NRC.NRCS.Add(nrcsm);
                    }
                }
                Debug.WriteLine("加载网易云歌词成功!");

            }
            catch (Exception ec)
            {
                Debug.WriteLine("加载网易云歌词失败." + ec);
            }
        }
        #endregion


        #region 判断字符类型
        public enum CharType
        {
            /// <summary>
            /// 中文
            /// </summary>
            Chinese,
            /// <summary>
            /// 英文
            /// </summary>
            English,
            /// <summary>
            /// 符号
            /// </summary>
            Symbol,
            /// <summary>
            /// 空格
            /// </summary>
            Space,
            /// <summary>
            /// 未知
            /// </summary>
            Unknown


        }
        /// <summary>
        /// 获得字符的类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public CharType GetCharType(string str)
        {
            Regex regChinese = new Regex(@"([\u4e00-\u9fa5])", RegexOptions.IgnoreCase);
            Regex regEnglish = new Regex(@"([a-zA-Z0-9])", RegexOptions.IgnoreCase);
            Regex regSymbol = new Regex(@"([_\-’'])", RegexOptions.IgnoreCase);
            Regex regSpace = new Regex(@"([\s+])", RegexOptions.IgnoreCase);

            if (regChinese.Match(str).Success)
            {
                return CharType.Chinese;
            }
            if (regEnglish.Match(str).Success)
            {
                return CharType.English;
            }
            if (regSymbol.Match(str).Success)
            {
                return CharType.Symbol;
            }
            if (regSpace.Match(str).Success)
            {
                return CharType.Space;
            }
            return CharType.Unknown;
        }
        #endregion

        #region 将歌词时间格式转为总毫秒数
        /// <summary>
        /// 将歌词时间格式转为总毫秒数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public double ToDouble(string time)
        {
            int m = int.Parse(time.Split(':').First());
            int s = int.Parse(time.Split(':').Last().Split('.').First());
            int ms = int.Parse(time.Split('.').Last());

            return new TimeSpan(0, 0, m, s, ms).TotalMilliseconds;
        }
        #endregion

        #region 获取歌词格式
        /// <summary>
        /// 获取歌词格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public LyricType GetLyricType(string str)
        {
            //Regex reg = new Regex(@"\[(?<time>(.*?)\:(.*?)\.(.*?))\](?<lrc>.*?)(\n)", RegexOptions.IgnoreCase);
            //if (reg.Match(str).Success)
            //{
            //    //网易云格式歌词
            //    return LyricType.LRC;
            //}
            //else
            //{
            //    //nrc暂时不做判断
            //    return LyricType.NRC;
            //}
            if (str.IndexOf("NRCS") != -1)
            {
                return LyricType.NRC;
            }
            else
            {
                return LyricType.LRC;
            }
        }
        #endregion


        #region 加载NRC格式
        private void LoadNRC(string nrc)
        {
            NRC = JsonConvert.DeserializeObject<NRCModel>(nrc);
        }
        #endregion

        public void Export()
        {
            File.WriteAllText("test.txt", JsonConvert.SerializeObject(NRC));
        }

        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //public static string Compress(string input)
        //{
        //    byte[] inputBytes = Encoding.Default.GetBytes(input);
        //    byte[] result = Compress(inputBytes);
        //    return Convert.ToBase64String(result);
        //}
    }
}
