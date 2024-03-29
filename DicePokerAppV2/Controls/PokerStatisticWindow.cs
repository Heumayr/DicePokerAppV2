﻿using DicePokerAppV2.Dataobject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace DicePokerAppV2.Controls
{
    public class PokerStatisticWindow : Window
    {
        private bool controlPressed;
        private bool buttonCallBackToGame;

        public StackPanel MainPanel { get; init; } = new();

        public PokerWindow CallingPokerWindow { get; }
        public List<Player> Players { get; }
        public double CurrentScale { get; private set; } = 3;

        public StackPanel ButtonPanel { get; init; } = new();
        public StackPanel StatisticPanel { get; init; } = new();

        public List<StackPanel> StatisticGridContent { get; init; } = new();

        public Grid StatisticGrid { get; init; } = new();

        public StackPanel Placing { get; init; } = new();

        public bool ButtonCallBackToGame { get => buttonCallBackToGame; set => buttonCallBackToGame = value; }

        public PokerStatisticWindow(PokerWindow pokerWindow, List<Player> players)
        {
            CallingPokerWindow = pokerWindow;
            Players = players;
            Title = Translation.StatisticTitle;
            IsClosed = false;

            MaxHeight = System.Windows.SystemParameters.VirtualScreenHeight;

            pokerWindow.IsEnabled = false;

            MainPanel.Background = PokerWindow.MainBackgroundColor;
            MainPanel.Orientation = Orientation.Horizontal;
            MainPanel.Margin = new Thickness(5);

            CreateButtons();
            SetUpStatisticGrid();
            SetUpPlacingStackPanel();

            StatisticPanel.Children.Add(StatisticGrid);
            StatisticPanel.Margin = new Thickness(2, 4, 2, 2);

            StatisticPanel.Children.Add(Placing);

            MainPanel.Children.Add(ButtonPanel);
            MainPanel.Children.Add(StatisticPanel);

            Content = MainPanel;

            //MinHeight = 100;
            //MinWidth = 200;
            SizeToContent = SizeToContent.WidthAndHeight;
            ResizeMode = ResizeMode.NoResize;

            Closing += PokerStatisticWindow_Closing;
            PreviewKeyDown += Window_KeyDown;
            PreviewMouseWheel += PokerDataWindow_MouseWheel;
            PreviewKeyUp += Window_KeyUp;

            Topmost = true;

            SizeChanged += OnWindowSizeChanged;

            Show();

            ScaleWindow();
        }

        private void SetUpPlacingStackPanel()
        {
            var orderedPlayers = Players.OrderByDescending(p => p.FinalPoints);

            var count = 1;
            var oldPoints = orderedPlayers.First().FinalPoints;

            Placing.Orientation = Orientation.Vertical;

            Placing.Children.Add(CreateHeadersLabels(Translation.Rankings));

            foreach (var player in orderedPlayers)
            {
                if(player != orderedPlayers.First())
                {
                    if (player.FinalPoints != oldPoints)
                        count++;

                    oldPoints = player.FinalPoints;
                }

                var tempLabel = CreateContentLabels($"{count}. {player.Name} - {Translation.Points}: {Math.Round(player.FinalPoints, 2)}");

                Placing.Children.Add(tempLabel);
            }
        }

        private int countSizeChangeInit = 0;
        protected void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            countSizeChangeInit++;

            if (e.NewSize.Width != e.PreviousSize.Width)
            {
                Top = (SystemParameters.VirtualScreenHeight - ActualHeight) / 2; 
                Left = (SystemParameters.VirtualScreenWidth - ActualWidth) / 2;
            }

            if (countSizeChangeInit >= 2)
                SizeChanged -= OnWindowSizeChanged;
        }

        private void SetUpStatisticGrid()
        {
            StatisticGrid.Background = Brushes.White;

            StatisticGrid.ColumnDefinitions.Add(new ColumnDefinition() { Name = "PokerColumns" });
            StatisticGrid.ColumnDefinitions.Add(new ColumnDefinition() { Name = "First" });
            StatisticGrid.ColumnDefinitions.Add(new ColumnDefinition() { Name = "Last" });

            StatisticGrid.RowDefinitions.Add(new RowDefinition() { Name = "Headers" });

            var firstLabel = CreateHeadersLabels(Translation.First);
            Grid.SetColumn(firstLabel, 1);
            var lastLabel = CreateHeadersLabels(Translation.Last);
            Grid.SetColumn(lastLabel, 2);

            StatisticGrid.Children.Add(firstLabel);
            StatisticGrid.Children.Add(lastLabel);

            for (int i = 0; i < CallingPokerWindow.ColumnsPerPlayer; i++)
            {
                StatisticGrid.RowDefinitions.Add(new RowDefinition() { Name = $"Column_{i + 1}" });

                var tempLabel = CreateHeadersLabels($"{Translation.Column} {i + 1}");
                Grid.SetRow(tempLabel, i + 1);
                StatisticGrid.Children.Add(tempLabel);
            }

            CreateBordersForGrid(3, CallingPokerWindow.ColumnsPerPlayer+1);
        }

        private void CreateBordersForGrid(int columns, int rows)
        {
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    var tempBorder = new PokerBorder(0);
                    Grid.SetColumn(tempBorder, i);
                    Grid.SetRow(tempBorder, j);
                    StatisticGrid.Children.Add(tempBorder);
                }
            }
        }

        private StatisticLabel CreateHeadersLabels(string content)
        {
            return new StatisticLabel(content, PokerWindow.NormalFontSize + 3, true, 0, 0);
        }

        private StatisticLabel CreateContentLabels(string content)
        {
            return new StatisticLabel(content, PokerWindow.NormalFontSize + 3, false, 0, 0);
        }

        public void ReloadData()
        {
            foreach (var con in StatisticGridContent)
            {
                StatisticGrid.Children.Remove(con);
            }
            StatisticGridContent.Clear();

            for (int i = 1; i <= CallingPokerWindow.ColumnsPerPlayer; i++)
            {
                StatisticGrid.Children.Add(CreateStatisticContent(i, 1, PlacementEnumeration.First, PlacementEnumeration.FirstShared));
                StatisticGrid.Children.Add(CreateStatisticContent(i, 2, PlacementEnumeration.Last, PlacementEnumeration.LastShared));
            }

            Placing.Children.Clear();
            SetUpPlacingStackPanel();
        }

        private StackPanel CreateStatisticContent(int columnNumber, int statisticGridColumn, params PlacementEnumeration[] placements)
        {
            var result = new StackPanel();
            result.VerticalAlignment = VerticalAlignment.Center;
            result.HorizontalAlignment = HorizontalAlignment.Center;

            foreach (var player in Players)
            {
                var column = player.PokerColumns.FirstOrDefault(c => c.ColumnNumber == columnNumber);

                if (column == null) 
                    break;

                if (placements.Contains(column.PokerPlacement))
                {
                    result.Children.Add(CreateContentLabels($"{player.Name}: {column.Score} ({Math.Round(player.PokerColumns.First(c => c.ColumnNumber == columnNumber).GetFinalPoints(), 2)})"));
                }
            }

            Grid.SetColumn(result, statisticGridColumn);
            Grid.SetRow(result, columnNumber);

            StatisticGridContent.Add(result);

            return result;
        }

        private void CreateButtons()
        {
            var saveButton = new PokerWindowButton(Translation.Save);
            saveButton.Click += SaveButton_Click;
            ButtonPanel.Children.Add(saveButton.SurroundingBorder);

            var backButton = new PokerWindowButton(Translation.BackToGame);
            backButton.Click += BackButton_Click;
            ButtonPanel.Children.Add(backButton.SurroundingBorder);

            var closeButton = new PokerWindowButton(Translation.Close);
            closeButton.Click += CloseButton_Click; 
            ButtonPanel.Children.Add(closeButton.SurroundingBorder);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonCallBackToGame = true;
           
            CallingPokerWindow.IsEnabled = true;

            Hide();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "pokerapp (*.padata)|*.padata";
            var result = dialog.ShowDialog();
            if (result != null && (bool)result)
            {
                Logger.CopyLogToPath(dialog.FileName);
            }
        }

        public bool IsClosed { get; private set; }
        private void PokerStatisticWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ButtonCallBackToGame)
                return;

            //var result = MessageBox.Show("Are you sure to leave?", "Cancelation", MessageBoxButton.OKCancel);

            //if (result == MessageBoxResult.OK)
            //{
            //    IsClosed = true;
            //    CallingPokerWindow.Close();
            //}
            //else
            //{
            //    e.Cancel = true;
            //}

            IsClosed = true;
            CallingPokerWindow.Close();
        }

        
        private void PokerDataWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (controlPressed)
            {
                var scaler = MainPanel.LayoutTransform as ScaleTransform;

                if (scaler == null)
                {
                    scaler = new ScaleTransform(1.0, 1.0);
                    MainPanel.LayoutTransform = scaler;
                }

                DoubleAnimation animator = new DoubleAnimation()
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                };

                if (e.Delta < 0)
                {
                    CurrentScale += 0.1;
                }
                else
                {
                    CurrentScale -= 0.1;
                }       

                ScaleWindow();
            }
        }

        private void ScaleWindow()
        {
            var scaler = MainPanel.LayoutTransform as ScaleTransform;

            if (scaler == null)
            {
                scaler = new ScaleTransform(1.0, 1.0);
                MainPanel.LayoutTransform = scaler;
            }
     

            if (CurrentScale < 1)
                CurrentScale = 1;

            if (CurrentScale > 3.5)
                CurrentScale = 3.5;


            if (firstLoad)
            {
                //animator.Completed += OnAnimationCompleted;
                //animator.Duration = new Duration(TimeSpan.FromMilliseconds(0));

                scaler.ScaleX = CurrentScale;
                scaler.ScaleY = CurrentScale;

                //Top = (SystemParameters.VirtualScreenHeight - Height) / 2;
                //Left = (SystemParameters.VirtualScreenWidth - Width) / 2;

                firstLoad = false;
            }
            else
            {
                DoubleAnimation animator = new DoubleAnimation()
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                };

                animator.To = CurrentScale;

                scaler.BeginAnimation(ScaleTransform.ScaleXProperty, animator);
                scaler.BeginAnimation(ScaleTransform.ScaleYProperty, animator);
            }
        }

        private bool firstLoad = true;

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                controlPressed = false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl && e.IsDown)
            {
                controlPressed = true;
            }
        }
    }
}
