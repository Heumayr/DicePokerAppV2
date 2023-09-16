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
        private List<TextBox> playersTextBoxes { get; set; } = new();

        private List<Grid> PlayersGrids { get; set; } = new();

        public List<Player> Players { get; set; } = new();

        private int maxPlayer = 30;
        private int maxColums = 10;

        public string NumberOfPlayers
        {
            get => numberOfPlayers; 
            set
            {
                if(value.Length > 3) 
                { 
                    value = value.Substring(0, 3);
                }

                if (int.TryParse(value, out int temp))
                    if (temp > maxPlayer)
                    {
                        numberOfPlayers = maxPlayer.ToString();
                        MessageBox.Show($"Max. {maxPlayer} {Translation.PlayersAllowed}.");
                    }
                    else
                        numberOfPlayers = value;
                else
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
                if (value.Length > 3)
                {
                    value = value.Substring(0, 3);
                }

                if (int.TryParse(value, out int temp))
                    if (temp > maxColums)
                    {
                        numberOfColumns = maxColums.ToString();
                        MessageBox.Show($"Max. {maxColums} {Translation.ColumnsAllowed}.");
                    }
                    else
                        numberOfColumns = value;
                else
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

            Title = Translation.MainTitle;

            DataContext = this;

            MainGrid = SetUpContent();

            Content = MainGrid;

            SizeToContent = SizeToContent.WidthAndHeight;
            MinHeight = 150;
            MinWidth = 250;

            MaxHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

            PreviewKeyDown += Window_KeyDown;
            PreviewMouseWheel += Window_MouseWheel;
            PreviewKeyUp += Window_KeyUp;
            ResizeMode = ResizeMode.NoResize;


            SizeChanged += OnWindowSizeChanged;

            CurrentScale += 1;
            ScaleWindow();
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

            if(e.Key == Key.Enter && e.IsDown)
            {
                StartButton_Click(sender, e);
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
            InputPanel.VerticalAlignment = VerticalAlignment.Top;

            var playersInput = CreateInputField($"{Translation.Players} #", inputTextBoxes, true, nameof(NumberOfPlayers));
            InputPanel.Children.Add(playersInput);

            var columnInput = CreateInputField($"{Translation.Columns} #", inputTextBoxes, true, nameof(NumberOfColumns));
            InputPanel.Children.Add(columnInput);

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

            var startButton = new PokerWindowButton(Translation.NewGame);
            startButton.Click += StartButton_Click;
            startButton.Command = EnableStartButton;
            startButton.CommandParameter = AllTextboxesForStart;
            buttonPanel.Children.Add(startButton.SurroundingBorder);

            var rotateNamesButton = new PokerWindowButton(Translation.RotateNames);
            rotateNamesButton.Click += RotateNamesButton_Click;
            buttonPanel.Children.Add(rotateNamesButton.SurroundingBorder);

            var LoadAutoSaveButton = new PokerWindowButton(Translation.LoadAutoSave);
            LoadAutoSaveButton.Click += LoadAutoSaveButton_Click; ;
            buttonPanel.Children.Add(LoadAutoSaveButton.SurroundingBorder);

            var OpenPokerFileButton = new PokerWindowButton(Translation.LoadGame);
            OpenPokerFileButton.Click += OpenPokerFileButton_Click;
            buttonPanel.Children.Add(OpenPokerFileButton.SurroundingBorder);

            var ChangeLanguageButton = new PokerWindowButton(Translation.ChangeLanguage);
            ChangeLanguageButton.Click += ChangeLanguageButton_Click;
            buttonPanel.Children.Add(ChangeLanguageButton.SurroundingBorder);

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

        private void RotateNamesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!playersTextBoxes.Any())
                return;

            var oldName = playersTextBoxes.Last().Text;

            foreach (var item in playersTextBoxes)
            {
                var temp = item.Text;

                item.Text = oldName;

                oldName = temp;
            }
        }

        int playerCountIndex = 0;
        int coulumCountIndex = 0;

        private void OnMouseWheel_PlayerInput(object sender, MouseWheelEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.Focus();

                if(tb.Name == nameof(NumberOfPlayers))
                {
                    if (e.Delta < 0)
                        playerCountIndex++;
                    else
                        playerCountIndex--;

                    if (playerCountIndex < 1)
                        playerCountIndex = 1;

                    tb.Text = playerCountIndex.ToString();
                }
                else if (tb.Name == nameof(NumberOfColumns))
                {
                    if (e.Delta < 0)
                        coulumCountIndex++;
                    else
                        coulumCountIndex--;

                    if (coulumCountIndex < 1)
                        coulumCountIndex = 1;

                    tb.Text = coulumCountIndex.ToString();
                } 
                else if (tb.Name.StartsWith("player"))
                {
                    if(StaticLiterals.DefaultNames == null)
                    {
                        e.Handled = true;
                        return;
                    }

                    var index = -1;
                    
                    for (int i = 0; i < StaticLiterals.DefaultNames.Length; i++)
                    {
                        if (StaticLiterals.DefaultNames[i] == tb.Text)
                        {
                            index = i;
                            break;
                        }
                    }

                    if (e.Delta < 0)
                        index--;
                    else
                        index++;

                    if (index < 0)
                        index = StaticLiterals.DefaultNames.Length - 1;

                    if (index >= StaticLiterals.DefaultNames.Length)
                        index = 0;

                    tb.Text = StaticLiterals.DefaultNames[index].ToString();

                }
            }

            e.Handled = true;
        }

        //private void PokerValueTextbox_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    if (ActualValue != BindingValue.ShowenValue && mouseChangedValue)
        //        BindingValue.ShowenValue = ActualValue;

        //    mouseChangedValue = false;
        //}

        private void ChangeLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            Translation.CurrentLanguage++;

            if((int)Translation.CurrentLanguage >= Enum.GetValues(typeof(Translator.Language)).Length)
            {
                Translation.CurrentLanguage = 0;
            }

            MainWindow newWindow = new MainWindow();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
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
                MessageBox.Show(Translation.LoadingFailedRebuild);
            }
            else
            {
                MessageBox.Show(Translation.LoadingFailed);
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
                            Players.Add(new Player(idCount++, string.IsNullOrWhiteSpace(tb.Text.Trim()) ? (numColumns == 1 ? $"{Translation.PlayerShort}{idCount - 1}" : $"{Translation.Player} {idCount - 1}") : tb.Text.Trim(), numColumns));
                        //}                          
                    }
                }
            }

            if (Players.Count > 0 && numColumns > 0)
            {    
                Players.First().IsCurrent = true;
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

            playersTextBoxes.Clear();

            for (int i = 0; i < tempNumberOfPlayers; i++)
            {
                var tempGrid = CreateInputField($"{Translation.Player} {i + 1}:", playersTextBoxes, false, $"player{i + 1}");
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
            textBox.Name = propertyName;
            textBox.MinHeight = 20;
            textBox.MinWidth = 100;
            textBox.BorderBrush = PokerWindow.MainFrontColor;
            textBox.HorizontalAlignment = HorizontalAlignment.Center;
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.HorizontalContentAlignment = HorizontalAlignment.Left;
            textBox.VerticalContentAlignment = VerticalAlignment.Center;

            textBox.MouseWheel += OnMouseWheel_PlayerInput;

            if (bindIt)
            {
                textBox.DataContext = this;
                textBox.SetBinding(TextBox.TextProperty, propertyName);               
            }

            textBox.TextChanged += TextBox_TextChanged;

            Grid.SetColumn(textBox, 1);
            Grid.SetRow(textBox, 0);
            inputGrid.Children.Add(textBox);

            textBoxesList.Add(textBox);

            return inputGrid;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
             if(sender is TextBox tb)
            {
                if(tb.Name == nameof(NumberOfPlayers))
                {
                    NumberOfPlayers = tb.Text;
                    tb.CaretIndex = tb.Text.Length;
                }
                else if (tb.Name == nameof(NumberOfColumns))
                {
                    NumberOfColumns = tb.Text;
                    tb.CaretIndex = tb.Text.Length;
                }
                else if (tb.Name.StartsWith("player"))
                {
                    if(int.TryParse(NumberOfColumns, out int temp))
                    {
                        if(15 < tb.Text.Length)
                        {
                            tb.Text = tb.Text.Substring(0, 15);
                            MessageBox.Show($"Max. {15} {Translation.MaxCharacters}.");
                            tb.CaretIndex = tb.Text.Length;
                        }
                    }
                    //else
                    //{
                    //    //tb.Text = string.Empty;

                    //    //MessageBox.Show(Translation.FirstEnterNumberOfColumns);
                    //}
                }
            }
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


            if (CurrentScale < 1)
                CurrentScale = 1;

            if (CurrentScale > 3.5)
                CurrentScale = 3.5;

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
