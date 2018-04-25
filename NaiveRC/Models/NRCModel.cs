using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaiveRC.Models
{
    /// <summary>
    /// NRC歌词数据模型
    /// </summary>
    public class NRCModel
    {
        public LyricType LyricType { get; set; }
        public List<NRCSentenceModel> NRCS { get; set; }
    }
}
