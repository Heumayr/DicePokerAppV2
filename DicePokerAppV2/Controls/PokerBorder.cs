using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DicePokerAppV2.Controls
{
    public class PokerBorder : Border
    {
        public PokerBorder(int radius)
        {
            BorderThickness = new System.Windows.Thickness(1);
            BorderBrush = PokerWindow.MainFrontColor;
            CornerRadius = new CornerRadius(radius);
        }
    }
}
