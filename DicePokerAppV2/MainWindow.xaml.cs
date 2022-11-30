using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DicePokerAppV2.Controls;
using DicePokerAppV2.Dataobject;
using Microsoft.Win32;

namespace DicePokerAppV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string numberOfPlayers = string.Empty;
        private string numberOfColumns = string.Empty;
        private bool controlPressed = false;

        private Grid MainGrid { get; set; }
        private StackPanel InputPanel { get; set; } = new StackPanel();
        private double CurrentScale { get; set; } = 1.0;

        private List<TextBox> inputTextBoxes { get; set; } = new List<TextBox>();
        private List<TextBox> playersTextBoxes { get; set; } = new List<TextBox>();

        private List<Grid> PlayersGrids { get; set; } = new();

        public List<Player> Players { get; set; } = new();

        public string NumberOfPlayers
        {
            get => numberOfPlayers; 
            set
            {      
                numberOfPlayers = value;
                OnPropertyChanged(nameof(NumberOfPlayers));
                ConfirmPlayersAndCreateThem();
            }
        }
        public string NumberOfColumns
        {
            get => numberOfColumns;
            set
            {
                numberOfColumns = value;
                OnPropertyChanged(nameof(NumberOfColumns));
            }
        }

        public List<TextBox> AllTextboxesForStart { get; set; } = new();

        public void UpdateTextboxesForStart()
        {
            AllTextboxesForStart.Clear();

            AllTextboxesForStart.AddRange(inputTextBoxes);
            AllTextboxesForStart.AddRange(playersTextBoxes);
        }

        public MainWindow()
        {
            InitializeComponent();

            Title = "Dice-Poker-App";

            DataContext = this;

            MainGrid = SetUpContent();

            Content = MainGrid;

            SizeToContent = SizeToContent.WidthAndHeight;
            MinHeight = 150;
            MinWidth = 250;

            MaxHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            PreviewKeyDown += Window_KeyDown;
            PreviewMouseWheel += Window_MouseWheel;
            PreviewKeyUp += Window_KeyUp;
            ResizeMode = ResizeMode.NoResize;

            CurrentScale += 1;
            ScaleWindow();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.LeftCtrl)
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            UpdateTextboxesForStart();
        }

        public Grid SetUpContent()
        {
            var grid = new Grid();
            grid.Background = PokerWindow.MainBackgroundColor;

            var col1 = new ColumnDefinition();
            var col2 = new ColumnDefinition();

            var row1 = new RowDefinition();

            grid.ColumnDefinitions.Add(col1);
            grid.ColumnDefinitions.Add(col2);
            grid.RowDefinitions.Add(row1);

            InputPanel.HorizontalAlignment = HorizontalAlignment.Center;
            InputPanel.VerticalAlignment = VerticalAlignment.Center;
            InputPanel.Children.Add(CreateInputField("Players #", inputTextBoxes, true, nameof(NumberOfPlayers)));
            InputPanel.Children.Add(CreateInputField("Columns #", inputTextBoxes, true, nameof(NumberOfColumns)));
            InputPanel.CanVerticallyScroll = true;
            InputPanel.Margin = new Thickness(10);

            //Grid.SetColumn(InputPanel, 1);
            //Grid.SetRow(InputPanel, 0);

            var buttonPanel = new StackPanel();
            buttonPanel.HorizontalAlignment = HorizontalAlignment.Center;
            buttonPanel.VerticalAlignment = VerticalAlignment.Center;

            //var confirmPlayers = new PokerWindowButton("confirm players");
            //confirmPlayers.Click += ClickConfirmButton_CreatePlayerFields;
            //buttonPanel.Children.Add(confirmPlayers.SurroundingBorder);

            var startButton = new PokerWindowButton("start");
            startButton.Click += StartButton_Click;
            startButton.Command = EnableStartButton;
            startButton.CommandParameter = AllTextboxesForStart;
            buttonPanel.Children.Add(startButton.SurroundingBorder);

            var LoadAutoSaveButton = new PokerWindowButton("load auto save");
            LoadAutoSaveButton.Click += LoadAutoSaveButton_Click; ;
            buttonPanel.Children.Add(LoadAutoSaveButton.SurroundingBorder);

            var OpenPokerFileButton = new PokerWindowButton("open poker file");
            OpenPokerFileButton.Click += OpenPokerFileButton_Click;
            buttonPanel.Children.Add(OpenPokerFileButton.SurroundingBorder);

            Grid.SetColumn(buttonPanel, 0);
            Grid.SetRow(buttonPanel, 0);

            grid.Children.Add(buttonPanel);


            var scrollViewer = new ScrollViewer();
            scrollViewer.Content = InputPanel;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Grid.SetColumn(scrollViewer, 1);
            Grid.SetRow(scrollViewer, 0);

            grid.Children.Add(scrollViewer);


            grid.Margin = new Thickness(10);
            return grid;
        }

        private void OpenPokerFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "pokerapp (*.padata)|*.padata";
            
            if(dialog.ShowDialog() == true)
            {
                LoadPlayerWindowData(dialog.FileName);
            }
        }

        private void LoadAutoSaveButton_Click(object sender, RoutedEventArgs e)
        {
            var path = Logger.LoggingTempPath;

            LoadPlayerWindowData(path);
        }

        private void LoadPlayerWindowData(string path)
        {
            Logger.SaveLogTemporary();

            Logger.ResetLog();

            Players = PokerLoader.LoadPlayersFromFile(path).ToList();

            if (Players.Count < 1)
            {
                LoadingFail();
                return;
            }

            var pwin = new PokerWindow(this, Players, Players[0].NumberOfPokerColumns);

            if (!PokerLoader.LoadValuesFromFile(Players, path))
            {
                pwin.Close();
                LoadingFail();
                return;
            }

            Logger.DeleteTempLog();
        }

        private void LoadingFail()
        {
            //MessageBox.Show(PokerLoader.DebugState.ToString());
            if (Logger.ReplaceLogWithTempLog())
            {
                MessageBox.Show("Loading failed, Logfile was rebuild.");
            }
            else
            {
                MessageBox.Show("Loading failed.");
            }           
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Players = new();
            var numColumns = 0;

            int.TryParse(NumberOfColumns, out numColumns);

            if (numColumns == 0)
                return;

            var idCount = 1;

            bool resetLog = true;

            foreach (var playerGrid in PlayersGrids)
            {
                foreach (var control in playerGrid.Children)
                {
                    if(control is TextBox tb)
                    {
                        //if (tb.Text != string.Empty)
                        //{
                            if (resetLog)
                            {
                                Logger.ResetLog();
                                resetLog = false;
                            }
                            Players.Add(new Player(idCount++, string.IsNullOrWhiteSpace(tb.Text.Trim()) ? $"Player {idCount -1}" : tb.Text.Trim(), numColumns));
                        //}                          
                    }
                }
            }

            if (Players.Count > 0 && numColumns > 0)
            {    
                _ = new PokerWindow(this, Players, numColumns);
            }
        }

        private void ConfirmPlayersAndCreateThem()
        {
            foreach (var player in PlayersGrids)
            {
                InputPanel.Children.Remove(player);             
            }

            PlayersGrids = new();

            var tempNumberOfPlayers = 0;

            int.TryParse(numberOfPlayers, out tempNumberOfPlayers);

            for (int i = 0; i < tempNumberOfPlayers; i++)
            {
                var tempGrid = CreateInputField($"Player {i + 1}:", playersTextBoxes, false, string.Empty);
                PlayersGrids.Add(tempGrid);
                InputPanel.Children.Add(tempGrid);
            }   
        }


        public Grid CreateInputField(string labelContent, List<TextBox> textBoxesList, bool bindIt, string propertyName)
        {
            var inputGrid = new Grid();
            var labels = new ColumnDefinition();
            var textBoxes = new ColumnDefinition();

            var inputGridRow1 = new RowDefinition();

            inputGrid.ColumnDefinitions.Add(labels);
            inputGrid.ColumnDefinitions.Add(textBoxes);

            inputGrid.RowDefinitions.Add(inputGridRow1);

            var label = new Label();
            label.Content = labelContent;
            label.MinHeight = 20;
            label.MinWidth = 100;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.HorizontalContentAlignment = HorizontalAlignment.Right;
            label.VerticalContentAlignment = VerticalAlignment.Center;

            Grid.SetColumn(label, 0);
            Grid.SetRow(label, 0);
            inputGrid.Children.Add(label);


            var textBox = new TextBox();
            textBox.MinHeight = 20;
            textBox.MinWidth = 100;
            textBox.BorderBrush = PokerWindow.MainFrontColor;
            textBox.HorizontalAlignment = HorizontalAlignment.Center;
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.HorizontalContentAlignment = HorizontalAlignment.Left;
            textBox.VerticalContentAlignment = VerticalAlignment.Center;

            if (bindIt)
            {
                textBox.DataContext = this;
                textBox.SetBinding(TextBox.TextProperty, propertyName);
            }


            Grid.SetColumn(textBox, 1);
            Grid.SetRow(textBox, 0);
            inputGrid.Children.Add(textBox);

            textBoxesList.Add(textBox);

            return inputGrid;
        }


        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (controlPressed)
            {
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
            var scaler = MainGrid.LayoutTransform as ScaleTransform;

            if (scaler == null)
            {
                scaler = new ScaleTransform(1.0, 1.0);
                MainGrid.LayoutTransform = scaler;
            }

            DoubleAnimation animator = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(100)),
            };

            if (CurrentScale < 1)
                CurrentScale = 1;

            if (CurrentScale > 3.5)
                CurrentScale = 3.5;

            animator.To = CurrentScale;

            scaler.BeginAnimation(ScaleTransform.ScaleXProperty, animator);
            scaler.BeginAnimation(ScaleTransform.ScaleYProperty, animator);
        }


        public ICommand EnableStartButton { get; set; } = new CommandDelegate(CanExecuteEnableStartButton, ExecuteWithStartButtonClick);

        public static bool CanExecuteEnableStartButton(object? parameter)
        { 
            if(parameter != null)
            {
                var list = parameter as List<TextBox>;

                if(list == null)
                    return false;

                if (list.Count < 3)
                    return false;

                try
                {
                    //var playersCount = Convert.ToInt32(list[0].Text);
                    var columnsCount = Convert.ToInt32(list[1].Text);

                    //var countValidePlayerNames = 0;

                    //for (int i = 2; i < list.Count; i++)
                    //{
                    //    if (!string.IsNullOrWhiteSpace(list[i].Text))
                    //        countValidePlayerNames++;
                    //}

                    var countValidePlayerNames = list.Count;

                    if (countValidePlayerNames > 0 && columnsCount > 0)
                        return true;
                }
                catch (Exception)
                {

                    return false;
                }
                
            }
            return false;
        }

        public static void ExecuteWithStartButtonClick(object? parameter)
        {
            //nothing
        }
    }
}
