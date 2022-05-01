using DicePokerAppV2.Dataobject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace DicePokerAppV2.Controls
{
    public class PlayerLabel : Label
    {
        public Player Owner { get; init; }

        public PlayerLabel(Player owner, string bindingValue, int fontSize, bool isBold, int minwidth, int minheight) : base()
        {
            Owner = owner;

            MinWidth = minwidth;
            MinHeight = minheight;
            HorizontalContentAlignment = HorizontalAlignment.Center;
            VerticalContentAlignment = VerticalAlignment.Center;

            BorderBrush = System.Windows.Media.Brushes.Transparent;
            FontSize = fontSize;
            
            if(isBold)
                FontWeight = FontWeights.Bold;

            DataContext = Owner;
            SetBinding(Label.ContentProperty, bindingValue);
        }
    }
}
