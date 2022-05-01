using DicePokerAppV2.Dataobject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerAppV2.Controls
{
    public class PokerColumnLabel : Label
    {
        public PokerColumn Owner { get; init; }

        public string BindingValue { get; init; }

        public PokerColumnLabel(PokerColumn owner, string bindingValue, int fontSize, bool isBold, int minwidth, int minheight) : base()
        {
            Owner = owner;
            BindingValue = bindingValue;

            MinWidth = minwidth;
            MinHeight = minheight;
            HorizontalContentAlignment = HorizontalAlignment.Center;
            VerticalContentAlignment = VerticalAlignment.Center;

            BorderBrush = System.Windows.Media.Brushes.Transparent;
            FontSize = fontSize;

            if (isBold)
                FontWeight = FontWeights.Bold;

            DataContext = Owner;
            SetBinding(Label.ContentProperty, bindingValue);
        }
    }
}
