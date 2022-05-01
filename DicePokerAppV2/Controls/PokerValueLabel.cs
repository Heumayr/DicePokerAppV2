using DicePokerAppV2.Dataobject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerAppV2.Controls
{
    public class PokerValueLabel : Label
    {
        public Value? Owner { get; init; }

        public PokerValueLabel(int fontSize, bool isBold, int minwidth, int minheight) : base()
        {
            MinWidth = minwidth;
            MinHeight = minheight;
            HorizontalContentAlignment = HorizontalAlignment.Center;
            VerticalContentAlignment = VerticalAlignment.Center;

            BorderBrush = Brushes.Transparent;
            FontSize = fontSize;

            if (isBold)
                FontWeight = FontWeights.Bold;
        }

        public PokerValueLabel(Value owner, string bindingValue, int fontSize, bool isBold, int minwidth, int minheight) : base()
        {
            Owner = owner;

            MinWidth = minwidth;
            MinHeight = minheight;
            HorizontalContentAlignment = HorizontalAlignment.Center;
            VerticalContentAlignment = VerticalAlignment.Center;

            BorderBrush = Brushes.Transparent;
            FontSize = fontSize;

            if (isBold)
                FontWeight = FontWeights.Bold;

            DataContext = Owner;
            SetBinding(Label.ContentProperty, bindingValue);
        }
    }
}
