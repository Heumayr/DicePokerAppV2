using DicePokerAppV2.Dataobject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DicePokerAppV2.Controls
{
    public class PokerWindow : Window
    {
        public static readonly int NormalFontSize = 12;
        public static readonly int SmallFontSize = NormalFontSize - 2;
        public static readonly int GlobalCornerRadius = 8;
        public static readonly Brush MainBackgroundColor = Brushes.Azure;
        public static readonly Brush MainFrontColor = Brushes.Turquoise;
        public static readonly Brush OverlayColor = new SolidColorBrush(Color.FromRgb(220, 255, 255));
        public static readonly Brush ErrorColor = Brushes.IndianRed;
        public static readonly Brush LightErrorColor = new SolidColorBrush(Color.FromRgb(255, 230, 230));
        public static readonly Brush HighlightColor = Brushes.White;
        public static readonly Brush HasValueColer = new SolidColorBrush(Color.FromRgb(240, 255, 255));

        private readonly int numberOfAllColums = 0;
        private bool controlPressed;

        public StackPanel MainPanel { get; init; } = new();

        public Window MainWindow { get; init; }
        public PokerStatisticWindow? StatisticWindow { get; set; } = null;

        private double CurrentScale { get; set; } = 1;
        private double MaximumScale { get; set; } = 5;

        private List<PokerValueTextbox> PokerValueTextboxes { get; set; } = new();

        public List<Player> Players { get; init; }

        public List<PlayerStackPanel> PlayerStackPanels { get; init; } = new();

        public List<PokerValueLabel> StatisticLabels { get; set; } = new();

        public int ColumnsPerPlayer { get; }

        public PokerWindow(Window mainWindow, IEnumerable<Player> players, int numberOfColumns)
        {
            MainWindow = mainWindow;
            MainWindow.Hide();
            Title = MainWindow.Title;

            MaxHeight = System.Windows.SystemParameters.VirtualScreenHeight;

            ColumnsPerPlayer = numberOfColumns;
            numberOfAllColums = numberOfColumns * players.Count();
            Players = players.ToList();

            foreach (var player in players)
            {
                player.SetUpOpponents(players);
            }

            MainPanel.CanHorizontallyScroll = true;
            MainPanel.CanVerticallyScroll = true;
            MainPanel.HorizontalAlignment = HorizontalAlignment.Left;
            MainPanel.VerticalAlignment = VerticalAlignment.Top;
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.Content = MainPanel;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            Content = scrollViewer;
            Background = new SolidColorBrush(Colors.AliceBlue);
            MainPanel.Orientation = Orientation.Horizontal;
            MainPanel.Margin = new Thickness(5);
            MainPanel.Background = MainBackgroundColor;

            CreateButtons();

            CreateLabels(players.FirstOrDefault());

            foreach (var player in players)
            {
                Border tempBorder = new PokerBorder(0);
                var tempPsp = new PlayerStackPanel(player);
                PlayerStackPanels.Add(tempPsp);
                tempBorder.Child = tempPsp;
                tempBorder.Margin = new Thickness(0, 4, 0, 2);

                MainPanel.Children.Add(tempBorder);
            }

            SetTabIndex(MainPanel.Children);

            SizeToContent = SizeToContent.WidthAndHeight;
            ResizeMode = ResizeMode.NoResize;
            Closing += CloseActions;
            PreviewKeyDown += Window_KeyDown;
            PreviewMouseWheel += PokerDataWindow_MouseWheel;
            PreviewKeyUp += Window_KeyUp;

            foreach (var p in Players)
            {
                p.GameFinished += ChangeToFinishMode;
            }

            SetUpStatsState();

            SizeChanged += OnWindowSizeChanged;

            Show();

            ScaleWindow();
        }

        private int countSizeChangeInit = 0;

        protected void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            countSizeChangeInit++;

            if (e.NewSize.Width != e.PreviousSize.Width)
            {
                Top = 0;
                Left = (SystemParameters.VirtualScreenWidth - ActualWidth) / 2;
            }

            if (countSizeChangeInit >= 2)
                SizeChanged -= OnWindowSizeChanged;
        }

        private void ChangeToFinishMode(object? sender, bool e)
        {
            if (e)
            {
                if (StatisticWindow == null)
                {
                    StatisticWindow = new(this, Players);
                }

                StatisticWindow.ButtonCallBackToGame = false;
                StatisticWindow.ReloadData();
                StatisticWindow.Show();
            }
        }

        private void CreateButtons()
        {
            var panel = new StackPanel();

            var buttonSave = new PokerWindowButton(Translation.Save);
            buttonSave.Click += ButtonSave_Click;
            panel.Children.Add(buttonSave.SurroundingBorder);

            var buttonHideStatistics = new PokerWindowButton(Translation.HideStats);
            buttonHideStatistics.Click += ButtonHideStatistics_Click;
            panel.Children.Add(buttonHideStatistics.SurroundingBorder);

            var buttonCancel = new PokerWindowButton(Translation.BackToMenu);
            buttonCancel.Click += ButtonCancel_Click;
            panel.Children.Add(buttonCancel.SurroundingBorder);


            MainPanel.Children.Add(panel);
        }

        private Visibility saveStatVis = Visibility.Visible;
        private Visibility saveMinStatsVis = Visibility.Visible;
        private int countClicksVis = 1;
        private int showMinstats = 4;
        private int showMinstatsMax = 8;
        private void ButtonHideStatistics_Click(object sender, RoutedEventArgs e)
        {
            if (sender is PokerWindowButton button)
            {
                SetUpStatsState();

                if (countClicksVis == 1)
                    button.Content = Translation.MinStats;
                else if (countClicksVis == 2)
                    button.Content = Translation.HideStats;
                else
                    button.Content = Translation.ShowStats;

            }
        }

        private void SetUpStatsState()
        {
            if (countClicksVis == 0)
            {
                saveStatVis = Visibility.Visible;
                saveMinStatsVis = Visibility.Visible;
            }
            else if (countClicksVis == 1)
            {
                saveStatVis = Visibility.Collapsed;
                saveMinStatsVis = Visibility.Visible;
            }
            else
            {
                saveStatVis = Visibility.Collapsed;
                saveMinStatsVis = Visibility.Collapsed;
            }

            var i = 0;

            foreach (var stat in StatisticLabels)
            {
                if (i < showMinstats || i > showMinstatsMax)
                {
                    stat.Visibility = saveMinStatsVis;
                }
                else
                {
                    stat.Visibility = saveStatVis;
                }
                i++;
            }

            foreach (var psp in PlayerStackPanels)
            {
                i = 0;
                foreach (var pspstatl in psp.StatisticLabels)
                {
                    if (i < showMinstats)
                    {
                        pspstatl.Visibility = saveMinStatsVis;
                    }
                    else
                    {
                        pspstatl.Visibility = saveStatVis;
                    }
                    i++;
                    if (i > showMinstatsMax)
                        i = 0;
                }

                foreach (var pspstatpl in psp.StatisticPlayerLabels)
                {
                    pspstatpl.Visibility = saveMinStatsVis;
                }
            }

            countClicksVis++;
            if (countClicksVis > 2)
                countClicksVis = 0;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "pokerapp (*.padata)|*.padata";
            var result = dialog.ShowDialog();
            if (result != null && (bool)result)
            {
                //if (File.Exists(dialog.FileName))
                //{
                //    var resultMb = MessageBox.Show("Do you want to override?","Override existing file", MessageBoxButton.YesNo);

                //    if(resultMb == MessageBoxResult.Yes)
                //    {
                //        Logger.CopyLogToPath(dialog.FileName);
                //    }
                //}
                //else
                //{
                //    Logger.CopyLogToPath(dialog.FileName);
                //}

                Logger.CopyLogToPath(dialog.FileName);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

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

        private int countColumn = 0;
        private int countRow = 0;
        private void SetTabIndex(UIElementCollection uiec)
        {
            foreach (var uie in uiec)
            {
                if (uie is PokerValueTextbox tb)
                {
                    tb.PreviewKeyDown += PokerValueKeyDown_Pressed;
                    tb.TabIndex = numberOfAllColums * countRow + countColumn + 1;

                    countRow++;

                    if (countRow >= PokerColumn.NumberOfValues)
                    {
                        countRow = 0;
                        countColumn++;
                    }
                    PokerValueTextboxes.Add(tb);
                }
                else if (uie is StackPanel panel)
                {
                    SetTabIndex(panel.Children);
                }
                else if (uie is Border border)
                {
                    if (border.Child is StackPanel panel1)
                        SetTabIndex(panel1.Children);
                }
            }
        }

        /*private void SetTabIndex()
        {
            int countColumn = 0;
            int countRow = 0;

            foreach (var control in MainPanel.Children)
            {
                if (control is PlayerStackPanel sp)
                {
                    foreach (var control2 in sp.Children)
                    {
                        if (control2 is StackPanel sp2)
                        {
                            foreach (var control3 in sp2.Children)
                            {
                                if (control3 is StackPanel sp3)
                                {
                                    foreach (var control4 in sp3.Children)
                                    {
                                        if (control4 is PokerValueTextbox tb)
                                        {
                                            tb.PreviewKeyDown += PokerValueKeyDown_Pressed;
                                            tb.TabIndex = numberOfAllColums * countRow + countColumn + 1;

                                            countRow++;

                                            if (countRow >= PokerColumn.NumberOfValues)
                                            {
                                                countRow = 0;
                                                countColumn++;
                                            }
                                            PokerValueTextboxes.Add(tb);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }*/

        private void PokerValueKeyDown_Pressed(object sender, KeyEventArgs e)
        {
            if (sender is PokerValueTextbox pvtb)
            {
                //e.Handled = true;
                var tabIndex = pvtb.TabIndex;

                if (e.Key == Key.Enter)
                {
                    tabIndex += numberOfAllColums;

                    if (tabIndex == PokerColumn.NumberOfValues * numberOfAllColums + numberOfAllColums)
                        tabIndex = 1;

                    if (tabIndex > PokerColumn.NumberOfValues * numberOfAllColums)
                        tabIndex -= PokerColumn.NumberOfValues * numberOfAllColums - 1;
                }
                else if (e.Key == Key.Up)
                {
                    tabIndex -= numberOfAllColums;

                    if (tabIndex < 1)
                        tabIndex += PokerColumn.NumberOfValues * numberOfAllColums;
                }
                else if (e.Key == Key.Down)
                {
                    tabIndex += numberOfAllColums;

                    if (tabIndex > PokerColumn.NumberOfValues * numberOfAllColums)
                        tabIndex -= PokerColumn.NumberOfValues * numberOfAllColums;
                }
                else if (e.Key == Key.Left)
                {
                    tabIndex -= 1;

                    if (tabIndex % numberOfAllColums == 0)
                        tabIndex += numberOfAllColums;
                }
                else if (e.Key == Key.Right)
                {
                    if (tabIndex % numberOfAllColums == 0)
                        tabIndex -= numberOfAllColums - 1;
                    else
                        tabIndex += 1;
                }

                var nextTexbox = PokerValueTextboxes.FirstOrDefault(t => t.TabIndex == tabIndex);

                if (nextTexbox != null)
                    nextTexbox.Focus();
            }
        }

        private void CloseActions(object? sender, CancelEventArgs eventArgs)
        {
            if (StatisticWindow != null && !StatisticWindow.IsClosed)
            {
                StatisticWindow.Close();
            }

            MainWindow.Show();
        }

        private void PokerDataWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (controlPressed)
            {
                e.Handled = true;

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
            if (firstLoad)
            {
                MaximumScale = SystemParameters.VirtualScreenHeight / ActualHeight;

                CurrentScale = MaximumScale;
            }

            var scaler = MainPanel.LayoutTransform as ScaleTransform;

            if (scaler == null)
            {
                scaler = new ScaleTransform(1.0, 1.0);
                MainPanel.LayoutTransform = scaler;
            }

            if (CurrentScale < 0.5)
                CurrentScale = 0.5;

            if (CurrentScale > MaximumScale)
                CurrentScale = MaximumScale;

            if (firstLoad)
            {
                //animator.Completed += OnAnimationCompleted;
                //animator.Duration = new Duration(TimeSpan.FromMilliseconds(0));

                scaler.ScaleY = CurrentScale;

                scaler.ScaleX = CurrentScale;

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

        //private void OnAnimationCompleted(object? sender, EventArgs e)
        //{
        //    if(firstLoad) 
        //    {
        //        Top = 0;
        //        Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
        //        firstLoad = false;
        //    }        
        //}

        private void CreateLabels(Player? player)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));

            var column = player.PokerColumns.FirstOrDefault();

            ArgumentNullException.ThrowIfNull(column, nameof(column));

            var tempColumnPanel = new StackPanel();
            tempColumnPanel.HorizontalAlignment = HorizontalAlignment.Center;
            tempColumnPanel.VerticalAlignment = VerticalAlignment.Center;

            var dummyPlayer = new PlayerLabel(player, nameof(Owner.Name), NormalFontSize, true, 0, 0);
            dummyPlayer.Content = "";
            tempColumnPanel.Children.Add(dummyPlayer);
            tempColumnPanel.Children.Add(new PokerBorder(0));

            var dummy = new PokerColumnLabel(column, nameof(column.ColumnNumber), SmallFontSize, false, 0, 0);
            dummy.Content = "";

            tempColumnPanel.Children.Add(dummy);

            bool betweenLine = true;

            foreach (var value in column.Values)
            {
                var tempLabel = new PokerValueLabel(value, nameof(value.ShortName), NormalFontSize, false, 50, 30);

                if (value is SubValue && betweenLine)
                {
                    tempColumnPanel.Children.Add(new PokerBorder(0));
                    betweenLine = false;
                }

                tempColumnPanel.Children.Add(tempLabel);
            }

            tempColumnPanel.Children.Add(new PokerBorder(0));
            var tempLabel2 = new PokerValueLabel(NormalFontSize, true, 0, 0);
            tempLabel2.Content = Translation.Sum;
            tempColumnPanel.Children.Add(tempLabel2);

            //Statistics
            SetUpStatisticLables(tempColumnPanel);

            Border tempBorder = new PokerBorder(0);
            tempBorder.Child = tempColumnPanel;
            tempBorder.Margin = new Thickness(4, 4, 0, 2);
            MainPanel.Children.Add(tempBorder);
        }

        public void SetUpStatisticLables(StackPanel panel)
        {
            panel.Children.Add(new PokerBorder(0));

            StatisticLabels.Add(GeneratePokerValueLabelStatisitics(Translation.Dtm));
            StatisticLabels.Add(GeneratePokerValueLabelStatisitics(Translation.Place));

            StatisticLabels.Add(GeneratePokerValueLabelStatisitics(Translation.PossibleOpen));
            StatisticLabels.Add(GeneratePokerValueLabelStatisitics(Translation.RealisticOpen));

            StatisticLabels.Add(GeneratePokerValueLabelStatisitics(Translation.Possible));
            StatisticLabels.Add(GeneratePokerValueLabelStatisitics(Translation.Realistic));


            StatisticLabels.Add(GeneratePokerValueLabelStatisitics(Translation.MaxOpponent));
            StatisticLabels.Add(GeneratePokerValueLabelStatisitics(Translation.MinOpponent));

            StatisticLabels.Add(GeneratePokerValueLabelStatisitics(Translation.ThrowsOpen));
            StatisticLabels.Add(GeneratePokerValueLabelStatisitics(Translation.Throws));

            StatisticLabels.ForEach(p => panel.Children.Add(p));
        }

        public PokerValueLabel GeneratePokerValueLabelStatisitics(string labelname)
        {
            return new PokerValueLabel(SmallFontSize, false, 0, 0)
            {
                Content = labelname,
            };
        }
    }
}
