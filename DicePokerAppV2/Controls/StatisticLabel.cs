using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerAppV2.Controls
{
    public class StatisticLabel : Label
    {
        public StatisticLabel(string content, int fontSize, bool isBold, int minwidth, int minheight) : base()
        {
            MinWidth = minwidth;
            MinHeight = minheight;
            HorizontalContentAlignment = HorizontalAlignment.Center;
            VerticalContentAlignment = VerticalAlignment.Center;

            BorderBrush = Brushes.Transparent;
            FontSize = fontSize;
            Content = content;

            if (isBold)
                FontWeight = FontWeights.Bold;
        }
    }
}
