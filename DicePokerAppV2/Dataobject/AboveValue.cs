using DicePokerAppV2.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerAppV2.Dataobject
{ 
    public sealed class AboveValue : Value
    {
        public AboveValue(PokerColumn owner, string name, string shortname, string logName, int defalutValue) : base(owner, name, shortname, logName, defalutValue)
        {
            for(int i = 1; i <= defalutValue * 5; i++)
            {
                if (i % defalutValue == 0)
                    ValidValues.Add(i.ToString());
            }
        }

        //public int Number { get; set; }

        public override int Score => GetScore();

        public override int MaxPossibleScore => IsThrown ? GetScore() : DefaultValue * 5;

        public override int MaxRealisticScore => IsThrown ? GetScore() : DefaultValue * 4;

        private int GetScore()
        {
            var result = 0;

            Int32.TryParse(ShowenValue, out result);

            return result;

        }

        public override string ControlShowenValue(string incomingValue)
        {
            if(string.IsNullOrWhiteSpace(incomingValue))
                return string.Empty;
            else
                incomingValue = incomingValue.Trim();

            if (incomingValue == "-" || incomingValue == string.Empty)
                return incomingValue;

            var val = 0;
            bool failure = false;

            try
            {
               val = Convert.ToInt32(incomingValue);
            }
            catch (Exception)
            {
                //throw new PokerValueException($"{incomingValue} is no proper Value.");
                failure = true;                
            }

            if (val % DefaultValue != 0 || val > DefaultValue * 5 || val < 0) 
            {
                failure = true;
            }

            if (failure)
            {
                //throw new PokerValueException($"{incomingValue} is no proper Value.");
                OnValitationFaild($"{incomingValue} {Translation.IsNotValideFor} \"{Name}\"");
                return string.Empty;
            }

            return incomingValue;
        }
    }
}

