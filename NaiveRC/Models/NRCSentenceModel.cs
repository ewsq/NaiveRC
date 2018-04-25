using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaiveRC.Models
{
    /// <summary>
    /// 歌词句数据模型
    /// </summary>
    public class NRCSentenceModel
    {
        /// <summary>
        /// 歌词开始时间
        /// </summary>
        public double StartTime { get; set; }

        /// <summary>
        /// 歌词
        /// </summary>
        public List<NRCWordModel> NRCWord { get; set; }
    }
}
