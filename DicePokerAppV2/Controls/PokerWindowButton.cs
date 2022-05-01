using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerAppV2.Controls
{
    public class PokerWindowButton : Button
    {
        public Border SurroundingBorder { get; set; }
        public PokerWindowButton(string contentName)
        {
            SurroundingBorder = new Border();
            //SurroundingBorder.BorderThickness = new Thickness(1);
            SurroundingBorder.Margin = new Thickness(2);
            //SurroundingBorder.CornerRadius = new CornerRadius(PokerWindow.GlobalCornerRadius);
            //SurroundingBorder.BorderBrush = Brushes.Turquoise;
            SurroundingBorder.Background = Brushes.White;
            SurroundingBorder.Child = this;

            MinHeight = 30;
            MinWidth = 100;

            Content = contentName;
            Background = Brushes.White;
            Margin = new Thickness(2);
            BorderBrush = PokerWindow.MainFrontColor;
            ClipToBounds = true;

            //MouseEnter += PokerWindowButton_MouseEnter;
            //MouseLeave += PokerWindowButton_MouseLeave;

            //Trigger trigger = new Trigger();
            //trigger.Property = IsMouseOverProperty;
            //trigger.Value = true;
            //trigger.Setters.Add(new Setter(Border.BorderBrushProperty, Brushes.Red));

            //Style style = new Style(typeof(Border));
            //style.Setters.Add(new Setter(Border.BorderBrushProperty, Brushes.Blue));
            //style.Triggers.Add(trigger);

            //SurroundingBorder.Style = style;            
           
        }

        //private void PokerWindowButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    SurroundingBorder.Background = Brushes.White;
        //    e.Handled = true;
        //}

        //private void PokerWindowButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    SurroundingBorder.Background = Brushes.AliceBlue;
        //    e.Handled = true;
        //}
    }
}
