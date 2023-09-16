using DicePokerAppV2.Dataobject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DicePokerAppV2.Controls
{
    public class PlayerStackPanel : StackPanel
    {
        public Player Owner { get; set; }

        public StackPanel ColumnsPanel { get; set; } = new();

        public List<PokerColumnLabel> StatisticLabels { get; set; } = new();
        public List<PlayerLabel> StatisticPlayerLabels { get; set; } = new();

        public PlayerStackPanel(Player owner)
        {
            Owner = owner;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            //Margin = new Thickness(5, 0, 5, 0);

            AddPlayerLabelsBefore();

            CreateColumns();

            AddPlayerLabelsAfter();

            Owner.ThrowsValitation += ChangeColorOnThrowValitation;

            if (Owner.IsCurrent)
                SetValueTextboxes(Children, true, true);
            else
                SetValueTextboxes(Children, false, true);
        }

        private void ChangeColorOnThrowValitation(object? sender, bool e)
        {
            if (e && Owner.IsCurrent)
                SetValueTextboxes(Children, true, e);
            else

                SetValueTextboxes(Children, false, e);

            if (!e)
                SetValueTextboxes(Children, true, e);   
        }

        private void SetValueTextboxes(UIElementCollection children, bool isFocused, bool noError)
        {
            foreach (var item in children)
            {
                if (item is PokerValueTextbox pvt)
                {
                    if(isFocused)
                    {
                        pvt.Background = PokerWindow.HighlightColor;
                    }
                    else
                    {
                        pvt.Background = PokerWindow.OverlayColor;
                    }

                    if (!noError)
                    {
                        pvt.Background = PokerWindow.LightErrorColor;
                    }
                }
                else if (item is PlayerLabel pl)
                {
                    if(isFocused)
                    {
                        pl.Background = PokerWindow.HighlightColor;
                    }
                    else
                    {
                        pl.Background = PokerWindow.MainBackgroundColor;
                    }

                    if (!noError)
                    {
                        pl.Background = PokerWindow.ErrorColor;
                    }
                }
                else if (item is PokerColumnLabel pcl)
                {
                    if (isFocused)
                    {
                        pcl.Background = PokerWindow.HighlightColor;
                    }
                    else
                    {
                        pcl.Background = PokerWindow.MainBackgroundColor;
                    }

                    if (!noError)
                    {
                        pcl.Background = PokerWindow.ErrorColor;
                    }
                }

                if (item is StackPanel sp)
                {
                    SetValueTextboxes(sp.Children, isFocused, noError);
                }
            }
        }

        private void AddPlayerLabelsAfter()
        {
            var playerLabel1 = new PlayerLabel(Owner, nameof(Owner.NumberOfThrowsShown), PokerWindow.SmallFontSize, false, 0, 0);

            StatisticPlayerLabels.Add(playerLabel1);
            Children.Add(playerLabel1);
        }

        private void AddPlayerLabelsBefore()
        {
            var numOfCol = Owner.NumberOfPokerColumns;


            if (Owner is Player pl)
            {
                if (numOfCol == 1 && pl.DisplayName.Length > 4)
                    pl.DisplayName = $"{pl.DisplayName.Substring(0, 4)}.";
                else if (numOfCol == 2 && Owner.Name.Length > 12)
                    pl.DisplayName = $"{pl.DisplayName.Substring(0, 12)}.";

                Children.Add(new PlayerLabel(Owner, nameof(pl.DisplayName), PokerWindow.NormalFontSize, true, 0, 0));
            }
            else
            {
                Children.Add(new PlayerLabel(Owner, nameof(Owner.Name), PokerWindow.NormalFontSize, true, 0, 0));
            }


            Children.Add(new PokerBorder(0));
        }

        private void CreateColumns()
        {
            ColumnsPanel.Orientation = Orientation.Horizontal;
            ColumnsPanel.HorizontalAlignment = HorizontalAlignment.Center;
            foreach (var column in Owner.PokerColumns)
            {
                var tempColumnPanel = new StackPanel();

                tempColumnPanel.Children.Add(new PokerColumnLabel(column, nameof(column.ColumnNumber), PokerWindow.SmallFontSize, false, 0, 0));

                bool betweenLine = true;

                foreach (var value in column.Values)
                {
                    var tempTextBox = new PokerValueTextbox(column, value, PokerWindow.NormalFontSize, false, 50, 30);

                    if (value is SubValue && betweenLine)
                    {
                        tempColumnPanel.Children.Add(new PokerBorder(0));
                        betweenLine = false;
                    }

                    tempColumnPanel.Children.Add(tempTextBox);
                }

                tempColumnPanel.Children.Add(new PokerBorder(0));
                tempColumnPanel.Children.Add(new PokerColumnLabel(column, nameof(column.ScoreShown), PokerWindow.NormalFontSize, true, 0, 0));

                //Statistics
                tempColumnPanel.Children.Add(new PokerBorder(0));

                var statlabel3 = new PokerColumnLabel(column, nameof(column.DifferenceToMaxOpponentScoreShown), PokerWindow.SmallFontSize, false, 0, 0);
                StatisticLabels.Add(statlabel3);
                tempColumnPanel.Children.Add(statlabel3);

                var statlabel7 = new PokerColumnLabel(column, nameof(column.PokerPlacementShown), PokerWindow.SmallFontSize, false, 0, 0);
                StatisticLabels.Add(statlabel7);
                tempColumnPanel.Children.Add(statlabel7);

                var statlabel8 = new PokerColumnLabel(column, nameof(column.MaxPossibleScoreOpenShown), PokerWindow.SmallFontSize, false, 0, 0);
                StatisticLabels.Add(statlabel8);
                tempColumnPanel.Children.Add(statlabel8);

                var statlabel9 = new PokerColumnLabel(column, nameof(column.MaxRealisticScoreOpenShown), PokerWindow.SmallFontSize, false, 0, 0);
                StatisticLabels.Add(statlabel9);
                tempColumnPanel.Children.Add(statlabel9);

                var statlabel4 = new PokerColumnLabel(column, nameof(column.MaxPossibleScoreShown), PokerWindow.SmallFontSize, false, 0, 0);
                StatisticLabels.Add(statlabel4);
                tempColumnPanel.Children.Add(statlabel4);

                var statlabel5 = new PokerColumnLabel(column, nameof(column.MaxRealisticScoreShown), PokerWindow.SmallFontSize, false, 0, 0);
                StatisticLabels.Add(statlabel5);
                tempColumnPanel.Children.Add(statlabel5);

                var statlabel1 = new PokerColumnLabel(column, nameof(column.CurrentMaxOpponentScoreShown), PokerWindow.SmallFontSize, false, 0, 0);
                StatisticLabels.Add(statlabel1);
                tempColumnPanel.Children.Add(statlabel1);

                var statlabel2 = new PokerColumnLabel(column, nameof(column.CurrentMinOpponentScoreShown), PokerWindow.SmallFontSize, false, 0, 0);
                StatisticLabels.Add(statlabel2);
                tempColumnPanel.Children.Add(statlabel2);

                var statlabel6 = new PokerColumnLabel(column, nameof(column.NumberOfNotThrownValuesShown), PokerWindow.SmallFontSize, false, 0, 0);
                StatisticLabels.Add(statlabel6);
                tempColumnPanel.Children.Add(statlabel6);



                column.PlacementChanged += CheckColorOfPacement;

                ColumnsPanel.Children.Add(tempColumnPanel);
                column.UpdateData();
            }
            Children.Add(ColumnsPanel);
        }

        private void CheckColorOfPacement(object? sender, PlacementEnumeration e)
        {
            if (sender is PokerColumn pc)
            {
                PokerColumnLabel? label = null;

                foreach (var child in ColumnsPanel.Children)
                {
                    if (child is StackPanel column)
                    {
                        foreach (var child2 in column.Children)
                        {
                            if (child2 is PokerColumnLabel pcl)
                            {
                                if (pc.ColumnNumber == pcl.Owner.ColumnNumber &&
                                    pcl.BindingValue == nameof(pcl.Owner.PokerPlacementShown))
                                {
                                    label = pcl;
                                    break;
                                }
                            }
                        }
                    }

                    if (label != null)
                        break;
                }

                if (label != null)
                {
                    switch (e)
                    {
                        case PlacementEnumeration.First:
                            label.Background = Brushes.ForestGreen;
                            break;
                        case PlacementEnumeration.FirstShared:
                            label.Background = Brushes.LightGreen;
                            break;
                        case PlacementEnumeration.Middle:
                            label.Background = PokerWindow.MainBackgroundColor;
                            break;
                        case PlacementEnumeration.LastShared:
                            label.Background = Brushes.LightCoral;
                            break;
                        case PlacementEnumeration.Last:
                            label.Background = Brushes.IndianRed;
                            break;
                        default:
                            label.Background = PokerWindow.MainBackgroundColor;
                            break;
                    }
                }
            }
        }
    }
}
