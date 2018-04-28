using NaiveRC.Models;
using System;
using System.Collections.Generic;
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
    /// NRCWord.xaml 的交互逻辑
    /// </summary>
    public partial class NRCWord : UserControl
    {
        //public string Word_;
        //public string Word
        //{
        //    get
        //    {
        //        return Word_;
        //    }
        //    set
        //    {
        //        Word_ = value;
        //        TB.Text = value;
        //    }
        //}

        public bool IsColour_;
        public bool IsColour
        {
            get
            {
                return IsColour_;
            }
            set
            {
                IsColour_ = value;
                if (value)
                {
                    Border.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    Border.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
            }
        }

        //public double StartTime { get; set; }

        //public double PlayTime { get; set; }
        private NRCWordModel Data_;
        public NRCWordModel Data
        {
            get
            {

                return Data_;
            }
            set
            {
                Data_ = value;
                TB.Text = Data_.Word;
            }
        }
        /// <summary>
        /// 标记结束吟唱
        /// </summary>
        /// <param name="PositionTime"></param>
        public void MarkEnd(double PositionTime)
        {
            Data.PlayTime = PositionTime - Data.StartTime;
        }
        /// <summary>
        /// 标记开始吟唱
        /// </summary>
        /// <param name="PositionTime"></param>
        public void MarkStart(double PositionTime)
        {
            Data.StartTime = PositionTime;
            IsColour = true;
        }

        /// <summary>
        /// 重置吟唱
        /// </summary>
        public void ReMark()
        {
            IsColour = false;

        }


        public NRCWord()
        {
            InitializeComponent();
        }
    }
}
