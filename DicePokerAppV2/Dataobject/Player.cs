using DicePokerAppV2.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerAppV2.Dataobject
{
    public class Player : INotifyPropertyChanged, IComparable<Player>
    {
        private int numberOfThrowsShown;
        private bool isFinished;

        public string Name { get; init; }

        public string DisplayName { get; set; }

        public int Id { get; init; }

        public List<PokerColumn> PokerColumns { get; set; } = new();

        public int NumberOfPokerColumns { get; init; }

        public bool IsDone => PokerColumns.All(c => c.IsDone);

        public event EventHandler<bool>? GameFinished;

        protected void OnIsDoneChanged()
        {
            if (IsDone && opponents.All(o => o.IsDone))
                GameFinished?.Invoke(this, true);
        }

        public int NumberOfThrows => PokerColumns.Sum(x => x.NumberOfThrownValues);
        public int NumberOfThrowsShown
        {
            get => numberOfThrowsShown;
            set
            {
                numberOfThrowsShown = value;
                OnPropertyChanged(nameof(NumberOfThrowsShown));
                OnThrowsValitationHappend();

                OnIsDoneChanged();

                foreach (var op in opponents)
                {
                    op.OnThrowsValitationHappend();
                }
            }
        }

        public event EventHandler<bool>? ThrowsValitation;

        protected void OnThrowsValitationHappend()
        {
            ThrowsValitation?.Invoke(this, CheckValitaionOfThrows());
        }

        private bool CheckValitaionOfThrows()
        {
            if (opponents.Count < 1)
                return true;

            if (opponents.Count == 1)
            {
                var control = NumberOfThrows - opponents[0].NumberOfThrows;

                if (Id == 2)
                {
                    control *= -1;
                }

                if (control < 0 || control > 1)
                    return false;

                return true;
            }

            Player? playerInFront = null;
            Player? playerBehinde = null;

            if (Id == opponents.Count + 1)
                playerInFront = opponents.First(x => x.Id == 1);
            else
                playerInFront = opponents.First(x => x.Id == Id + 1);

            if (Id == 1)
                playerBehinde = opponents.First(x => x.Id == opponents.Max(m => m.Id));
            else
                playerBehinde = opponents.First(x => x.Id == Id - 1);

            var behinde = playerBehinde.NumberOfThrows - NumberOfThrows;
            var inFront = NumberOfThrows - playerInFront.NumberOfThrows;

            if (Id == 1)
                behinde += 1;

            if (Id == opponents.Max(x => x.Id) + 1)
                inFront += 1;

            if (behinde > 1 || behinde < 0 || inFront > 1 || inFront < 0)
                return false;

            return true;
        }

        public List<Player> opponents { get; private set; } = new();

        public Player(int id, string name, int numberOfPokerColumns)
        {
            Name = name;
            DisplayName = name;
            NumberOfPokerColumns = numberOfPokerColumns;
            Id = id;

            for (int i = 0; i < NumberOfPokerColumns; i++)
            {
                PokerColumns.Add(new(this, i + 1));
            }

            Logger.AppendLog(GetLogString());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateData()
        {
            NumberOfThrowsShown = NumberOfThrows;
        }

        public void SetUpOpponents(IEnumerable<Player> allPlayers)
        {
            opponents.AddRange(allPlayers.Where(x => x.Id != Id));
            opponents.Sort();

            foreach (var pokerColumn in PokerColumns)
            {
                pokerColumn.SetOpponentsColumns(opponents);
            }
        }

        public int GetCurrentScorePerColumn(int columnNumber)
        {
            var currentColumn = PokerColumns.FirstOrDefault(x => x.ColumnNumber == columnNumber);

            if (currentColumn == null)
                throw new PokerException($"Column: {columnNumber} does not exist");

            return currentColumn.Score;
        }

        public int CompareTo(Player? other)
        {
            return Id.CompareTo(other?.Id);
        }

        public static readonly string LogNameString = "player";

        public string GetLogString()
        {
            return $"{LogNameString};{Id};{Name};{NumberOfPokerColumns}";
        }
    }
}
