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
    public class PokerValueTextbox : TextBox
    {
        private bool mouseChangedValue = false;

        public PokerColumn Owner { get; init; }
        public Value BindingValue { get; init; }

        private string ActualValue { get; set; } = string.Empty;

        public PokerValueTextbox(PokerColumn owner, Value bindingValue, int fontSize, bool isBold, int minwidth, int minheight) : base()
        {
            Owner = owner;
            BindingValue = bindingValue;

            MinWidth = minwidth;
            MinHeight = minheight;
            HorizontalContentAlignment = HorizontalAlignment.Center;
            VerticalContentAlignment = VerticalAlignment.Center;

            BorderBrush = PokerWindow.MainFrontColor;
            BorderThickness = new Thickness(0.5, 0.5, 0.5, 0.5);
            FontSize = fontSize;

            if (isBold)
                FontWeight = FontWeights.Bold;

            DataContext = BindingValue;
            SetBinding(TextBox.TextProperty, nameof(BindingValue.ShowenValue));

            BindingValue.Valitation += ShowFailure;

            MouseWheel += PokerValueTextbox_MouseWheel;
            MouseLeave += PokerValueTextbox_MouseLeave;
            TextChanged += PokerValueTextbox_TextChanged;
        }

        private void PokerValueTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActualValue = Text;
        }

        private void PokerValueTextbox_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(ActualValue != BindingValue.ShowenValue && mouseChangedValue)
                BindingValue.ShowenValue = ActualValue;

            mouseChangedValue = false;
        }

        private void PokerValueTextbox_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            Focus();
            int? actualIndex = null;

            for (int i = 0; i < BindingValue.ValidValues.Count; i++)
            {
                if (BindingValue.ValidValues[i] == Text)
                {
                    actualIndex = i;
                    break;
                }
            }

            if (actualIndex == null)
                return;

            if (e.Delta < 0)
                actualIndex++;
            else
                actualIndex--;

            if (actualIndex > BindingValue.ValidValues.Count - 1)
                actualIndex = 0;

            if(actualIndex < 0)
                actualIndex = BindingValue.ValidValues.Count - 1;

            //MouseWheelValue = BindingValue.ValidValues[(int)actualIndex];
            Text = BindingValue.ValidValues[(int)actualIndex];
            mouseChangedValue = true;
        }

        public void ShowFailure(object? sender, string msg)
        {
            MessageBox.Show(msg);
        }


    }
}
