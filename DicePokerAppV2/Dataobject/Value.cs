using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DicePokerAppV2.Dataobject
{
    public abstract class Value : INotifyPropertyChanged
    {
        private string showenValue = string.Empty;

        public string Name { get; init; }
        public string ShortName { get; init; }

        public string LogName { get; init; }

        public int DefaultValue { get; init; }
        public bool IsThrown => ShowenValue != string.Empty;
        public PokerColumn OwnerColumn { get; set; }

        public List<string> ValidValues { get; set; } = new();

        public string ShowenValue
        {
            get => showenValue;
            set
            {
                showenValue = ControlShowenValue(value);
                OnPropertyChanged(nameof(ShowenValue));

                Logger.AppendLog(GetLogString());
            }
        }

        public abstract int Score { get; }
        public abstract int MaxPossibleScore { get; }
        public abstract int MaxRealisticScore { get; }

        public Value(PokerColumn owner, string name, string shortname, string logName, int defalutValue) : base()
        {
            Name = name;
            LogName = logName;
            DefaultValue = defalutValue;
            ShortName = shortname;
            OwnerColumn = owner;

            ValidValues.Add(string.Empty);
            ValidValues.Add("-");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));

            OwnerColumn.ScoreShown = OwnerColumn.Score;
            //MessageBox.Show(OwnerColumn.ColumnSum.ToString());
        }

        public event EventHandler<string>? Valitation;

        protected void OnValitationFaild(string msg)
        {
            Valitation?.Invoke(this, msg);
        }

        public virtual string ControlShowenValue(string incomingValue)
        {
            return incomingValue;
        }

        public static readonly string LogNameString = "value";
        public string GetLogString()
        {
            return $"{LogNameString};{OwnerColumn.Player.Id};{OwnerColumn.Player.Name};{OwnerColumn.ColumnNumber};{LogName};{ShowenValue}";
        }
    }
}
