using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerAppV2.Dataobject
{
    public static class PokerLoader
    {
        public static IEnumerable<Player> LoadPlayersFromFile (string path)
        {
            var result = new List<Player>();

            if (!path.EndsWith(".padata"))
                return result;

            var log = File.ReadAllLines(path);

            foreach (var line in log)
            {
                
                if (line.StartsWith(Player.LogNameString))
                {
                    var playerData = line.Split(";");

                    try
                    {
                        result.Add(new Player(Convert.ToInt32(playerData[1]), playerData[2], Convert.ToInt32(playerData[3])));
                    }
                    catch
                    {
                        result = new List<Player>();
                        return result;
                    }         
                }
            }

            return result;
        }
        
        public static bool LoadValuesFromFile(IEnumerable<Player> players, string path)
        {
            if (!path.EndsWith(".padata"))
                return false;

            var log = File.ReadAllLines(path).Where(s => s.StartsWith(Value.LogNameString));
            
            foreach (var line in log)
            {
                var valueData = line.Split(";");
                
                try
                {
                    DebugState = 0;
                    if (valueData.Length < 5 || valueData.Length > 6)
                        return false;

                    DebugState = 1;
                    var player = players.FirstOrDefault(x => x.Id == Convert.ToInt32(valueData[1]) &&
                                                             x.Name == valueData[2]);
                    if (player == null)
                        return false;

                    DebugState = 2;
                    var column = player.PokerColumns.FirstOrDefault(x => x.ColumnNumber == Convert.ToInt32(valueData[3]));

                    if (column == null)
                        return false;

                    DebugState = 3;
                    var value = column.Values.FirstOrDefault(x => x.Name == valueData[4]);

                    if (value == null)
                        return false;

                    DebugState = 4;
                    if (valueData.Length == 6)
                    {
                        value.ShowenValue = valueData[5];
                    }
                    else
                    {
                        value.ShowenValue = string.Empty;
                    }
                    DebugState = 5;
                }
                catch
                {
                    DebugState = 6;
                    return false;
                }
            }
            return true;
        }

        public static int DebugState { get; set; }
    }
}
