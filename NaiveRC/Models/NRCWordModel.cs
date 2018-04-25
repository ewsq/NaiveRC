using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaiveRC.Models
{
    /// <summary>
    /// 歌词字模型
    /// </summary>
    public class NRCWordModel
    {
        /// <summary>
        /// 字
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// 吟唱时间
        /// </summary>
        public double PlayTime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public double StartTime { get; set; }


    }
}
