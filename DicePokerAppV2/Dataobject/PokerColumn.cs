using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DicePokerAppV2.Dataobject
{
    public class PokerColumn : INotifyPropertyChanged
    {
        public static readonly int NumberOfValues = 10;
        private int scoreShown;
        private int currentMaxOpponentScoreShown;
        private int currentMinOpponentScoreShown;
        private int differenceToMaxOpponentScore;
        private int maxPossibleScoreShown;
        private int maxRealisticScoreShown;
        private string pokerPlacementShown = string.Empty;
        private int numberOfNotThrownValuesShown;
        private int maxPossibleScoreOpenShown;
        private int maxRealisticScoreOpenShown;

        public int ColumnNumber { get; init; }

        public List<PokerColumn> OpponentsColumns { get; private set; } = new();

        public Player Player { get; init; }
        //public StackPanel Column { get; init; }

        public AboveValue Nine { get; init; }
        public AboveValue Ten { get; init; }
        public AboveValue Boy { get; init; }
        public AboveValue Lady { get; init; }
        public AboveValue King { get; init; }
        public AboveValue Ace { get; init; }

        public SubValue Street { get; init; }
        public SubValue Full { get; init; }
        public SubValue Poker { get; init; }
        public SubValue Grande { get; init; }

        public List<Value> Values { get; init; } = new();

        public bool IsDone => NumberOfThrownValues == Values.Count();

        public int NumberOfThrownValues => Values.Count(x => x.IsThrown);
        public int NumberOfNotThrownValuesShown
        {
            get => numberOfNotThrownValuesShown; 
            set
            {
                numberOfNotThrownValuesShown = value;
                OnPropertyChanged(nameof(NumberOfNotThrownValuesShown));
            }
        }
        public int Score => Values.Sum(x => x.Score);
        public int ScoreShown
        {
            get => scoreShown;
            set
            {
                scoreShown = value;
                OnPropertyChanged(nameof(ScoreShown));
                UpdateData();
            }
        }
        public int MaxPossibleScore => Values.Sum(x => x.MaxPossibleScore);
        public int MaxPossibleScoreShown
        {
            get => maxPossibleScoreShown;
            set
            {
                maxPossibleScoreShown = value;
                OnPropertyChanged(nameof(MaxPossibleScoreShown));
            }
        }

        public int MaxPossibleScoreOpen => Values.Sum(x => x.MaxPossibleScore - x.Score);
        public int MaxPossibleScoreOpenShown
        {
            get => maxPossibleScoreOpenShown;
            set
            {
                maxPossibleScoreOpenShown = value;
                OnPropertyChanged(nameof(MaxPossibleScoreOpenShown));
            }
        }

        public int MaxRealisticScore => Values.Sum(x => x.MaxRealisticScore);
        public int MaxRealisticScoreShown
        {
            get => maxRealisticScoreShown;
            set
            {
                maxRealisticScoreShown = value;
                OnPropertyChanged(nameof(MaxRealisticScoreShown));
            }
        }
        public int MaxRealisticScoreOpen => Values.Sum(x => x.MaxRealisticScore - x.Score);
        public int MaxRealisticScoreOpenShown
        {
            get => maxRealisticScoreOpenShown;
            set
            {
                maxRealisticScoreOpenShown = value;
                OnPropertyChanged(nameof(MaxRealisticScoreOpenShown));
            }
        }


        public PlacementEnumeration PokerPlacement => GetPlacement();
        public string PokerPlacementShown
        {
            get => pokerPlacementShown;
            set
            {
                pokerPlacementShown = value;
                OnPropertyChanged(nameof(PokerPlacementShown));
            }
        }
        public int CurrentMaxOpponentScore => OpponentsColumns.Count() > 0 ? OpponentsColumns.Max(x => x.Score) : 0;
        public int CurrentMaxOpponentScoreShown
        {
            get => currentMaxOpponentScoreShown;
            set
            {
                currentMaxOpponentScoreShown = value;
                OnPropertyChanged(nameof(CurrentMaxOpponentScoreShown));
            }
        }
        public int CurrentMinOpponentScore => OpponentsColumns.Count() > 0 ? OpponentsColumns.Min(x => x.Score) : 0;
        public int CurrentMinOpponentScoreShown
        {
            get => currentMinOpponentScoreShown;
            set
            {
                currentMinOpponentScoreShown = value;
                OnPropertyChanged(nameof(CurrentMinOpponentScoreShown));
            }
        }
        public int DifferenceToMaxOpponentScore => ScoreShown - CurrentMaxOpponentScore;
        public int DifferenceToMaxOpponentScoreShown
        {
            get => differenceToMaxOpponentScore;
            set
            {
                differenceToMaxOpponentScore = value;
                OnPropertyChanged(nameof(DifferenceToMaxOpponentScoreShown));
            }
        }

        public void UpdateData()
        {
            if(OpponentsColumns.Count > 0)
            {
                foreach (var opponentColumn in OpponentsColumns)
                {
                    opponentColumn.CalcDataOpponent();
                }

                DifferenceToMaxOpponentScoreShown = DifferenceToMaxOpponentScore;
                PokerPlacementShown = GetPlacementName();
                OnPlacemantChanged();
            }
            
            MaxPossibleScoreShown = MaxPossibleScore;
            MaxPossibleScoreOpenShown = MaxPossibleScoreOpen;
            MaxRealisticScoreShown = MaxRealisticScore;
            MaxRealisticScoreOpenShown = MaxRealisticScoreOpen;
            NumberOfNotThrownValuesShown = Values.Count - NumberOfThrownValues;

            Player.UpdateData();
        }

        public void CalcDataOpponent()
        {
            CurrentMaxOpponentScoreShown = CurrentMaxOpponentScore;
            CurrentMinOpponentScoreShown = CurrentMinOpponentScore;
            DifferenceToMaxOpponentScoreShown = DifferenceToMaxOpponentScore;
            PokerPlacementShown = GetPlacementName();
            OnPlacemantChanged();
        }

        public string GetPlacementName()
        {
            switch (PokerPlacement)
            {
                case PlacementEnumeration.First:
                    return "1.";
                case PlacementEnumeration.FirstShared:
                    return "1. s.";
                case PlacementEnumeration.Middle:
                    return "m";
                case PlacementEnumeration.LastShared:
                    return $"{OpponentsColumns.Count + 1}. s.";
                case PlacementEnumeration.Last:
                    return $"{OpponentsColumns.Count + 1}.";
                default:
                    return "don't happen";
            }
        }

        public PokerColumn(Player player, int columnNumber)
        {
            Player = player;
            ColumnNumber = columnNumber;

            Nine = new(this, "Nine", "9", 1);
            Ten = new(this, "Ten", "10", 2);
            Boy = new(this, "Boy", "B", 3);
            Lady = new(this, "Lady", "L", 4);
            King = new(this, "King", "K", 5);
            Ace = new(this, "Ace", "A", 6);

            Street = new(this, "Street", "Str", 20, 25, "5");
            Full = new(this, "Full House", "F", 30, 35, "5");
            Poker = new(this, "Poker", "P", 40, 45, "5");
            Grande = new(this, "Grande", "G", 50, 80, "30");


            Values.Add(Nine);
            Values.Add(Ten);
            Values.Add(Boy);
            Values.Add(Lady);
            Values.Add(King);
            Values.Add(Ace);
            Values.Add(Street);
            Values.Add(Full);
            Values.Add(Poker);
            Values.Add(Grande);
        }
        private PlacementEnumeration GetPlacement()
        {
            if (Score > CurrentMaxOpponentScore)
                return PlacementEnumeration.First;
            else if (Score == CurrentMaxOpponentScore)
                return PlacementEnumeration.FirstShared;
            else if (Score == CurrentMinOpponentScore)
                return PlacementEnumeration.LastShared;
            if (Score < CurrentMinOpponentScore)
                return PlacementEnumeration.Last;
            else
                return PlacementEnumeration.Middle;
        }

        public void SetOpponentsColumns(IEnumerable<Player> opponents)
        {
            foreach (var opponent in opponents)
            {
                OpponentsColumns.Add(opponent.PokerColumns.First(p => p.ColumnNumber == ColumnNumber));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<PlacementEnumeration>? PlacementChanged;

        protected void OnPlacemantChanged()
        {
            PlacementChanged?.Invoke(this, PokerPlacement);
        }
    }
}
