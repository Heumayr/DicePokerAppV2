using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerAppV2.Dataobject
{
    public sealed class SubValue : Value
    {
        public bool IsServed { get; set; } = false;
        public int ServedValue { get; init; }

        public string[] ServedValitationValues { get; set; }

        public override int Score => GetScore();

        public override int MaxPossibleScore => IsThrown ? GetScore() : ServedValue;

        public override int MaxRealisticScore => IsThrown ? GetScore() : DefaultValue;

        public SubValue(PokerColumn owner, string name, string shortname, string logName, int defalutValue, int servedValue, params string[] valitationValues) 
            : base(owner, name, shortname, logName, defalutValue)
        {
            ServedValue = servedValue;
            ServedValitationValues = valitationValues;

            ValidValues.Add("x");
            ValidValues.AddRange(valitationValues);
        }


        private int GetScore()
        {
            if (ShowenValue == "x")
                return DefaultValue;

            foreach (var valitaion in ServedValitationValues)
            {
                if(valitaion == ShowenValue)
                {
                    return ServedValue;
                }
            }

            return 0;
        }

        public override string ControlShowenValue(string incomingValue)
        {
            if (string.IsNullOrWhiteSpace(incomingValue))
                return string.Empty;
            else
                incomingValue = incomingValue.Trim();

            if (incomingValue == "-" || incomingValue == string.Empty || incomingValue == "x")
                return incomingValue;

            foreach (var valitation in ServedValitationValues)
            {
                if (incomingValue == valitation)
                    return incomingValue;
            }

            OnValitationFaild($"{incomingValue} {Translation.IsNotValideFor} \"{Name}\"");
          
            return string.Empty;
        }
    }
}
