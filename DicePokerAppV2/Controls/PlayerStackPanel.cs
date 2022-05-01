﻿using DicePokerAppV2.Dataobject;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        private void ChangeColorOnThrowValitation(object? sender, bool e)
        {
            if (e)
                Background = PokerWindow.MainBackgroundColor;
            else
                Background = Brushes.IndianRed;
        }

        private void AddPlayerLabelsAfter()
        {
            var playerLabel1 = new PlayerLabel(Owner, nameof(Owner.NumberOfThrowsShown), PokerWindow.SmallFontSize, false, 0, 0);
            StatisticPlayerLabels.Add(playerLabel1);
            Children.Add(playerLabel1);
        }

        private void AddPlayerLabelsBefore()
        {
            Children.Add(new PlayerLabel(Owner, nameof(Owner.Name), PokerWindow.NormalFontSize, true, 0, 0));
            Children.Add(new PokerBorder(0));
        }

        private void CreateColumns()
        {
            ColumnsPanel.Orientation = Orientation.Horizontal;
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
            if(sender is PokerColumn pc)
            {
                PokerColumnLabel? label = null;

                foreach (var child in ColumnsPanel.Children)
                {
                    if(child is StackPanel column)
                    {
                        foreach (var child2 in column.Children)
                        {
                            if(child2 is PokerColumnLabel pcl)
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

                if(label != null)
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