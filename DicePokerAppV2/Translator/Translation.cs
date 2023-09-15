using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerAppV2.Translator
{
    public static class Translation
    {
        public static Language CurrentLanguage { get; set; } = Translator.Language.Deutsch;

        public static string StatisticTitle => CurrentLanguage == Language.Deutsch ? "Glückwunsch" : "Congratulations";
        public static string PokerColumns => CurrentLanguage == Language.Deutsch ? "Poker Spalten" : "PokerColumns";

        public static string First => CurrentLanguage == Language.Deutsch ? "Erster" : "First";
        public static string Last => CurrentLanguage == Language.Deutsch ? "Letzter" : "Last";
        public static string Column => CurrentLanguage == Language.Deutsch ? "Spalte" : "Column";
        public static string Columns => CurrentLanguage == Language.Deutsch ? "Spalten" : "Columns";

        public static string PlayerShort => CurrentLanguage == Language.Deutsch ? "Sp" : "P";
        public static string Player => CurrentLanguage == Language.Deutsch ? "Spieler" : "player";
        public static string Players => CurrentLanguage == Language.Deutsch ? "Spieler" : "players";

        public static string Save => CurrentLanguage == Language.Deutsch ? "speichern" : "save";

        public static string BackToGame => CurrentLanguage == Language.Deutsch ? "zurück zum Spiel" : "back to game";

        public static string Close => CurrentLanguage == Language.Deutsch ? "Schließen" : "close";
        public static string BackToMenu => CurrentLanguage == Language.Deutsch ? "zurück zum Menü" : "back to menu";

        public static string HideStats => CurrentLanguage == Language.Deutsch ? "Stat. ausblenden" : "hide stats";
        public static string MinStats => CurrentLanguage == Language.Deutsch ? "min. Statistik" : "show min stats";
        public static string ShowStats => CurrentLanguage == Language.Deutsch ? "Stat. einblenden" : "show stats";

        public static string Sum => CurrentLanguage == Language.Deutsch ? "Summe" : "sum";
        public static string Dtm => CurrentLanguage == Language.Deutsch ? "Diff. zum 1." : "diff. to 1.";
        public static string Place => CurrentLanguage == Language.Deutsch ? "Platz" : "place";
        public static string Possible => CurrentLanguage == Language.Deutsch ? "mögl. Max." : "pos. max.";
        public static string Realistic => CurrentLanguage == Language.Deutsch ? "real. Max." : "real. max.";
        public static string PossibleOpen => CurrentLanguage == Language.Deutsch ? "mögl. offen" : "pos. open ";
        public static string RealisticOpen => CurrentLanguage == Language.Deutsch ? "real. offen" : "real. open";
        public static string MaxOpponent => CurrentLanguage == Language.Deutsch ? "Max. Gegener" : "max. opp.";
        public static string MinOpponent => CurrentLanguage == Language.Deutsch ? "Min. Gegner" : "min. opp.";
        public static string ThrowsOpen => CurrentLanguage == Language.Deutsch ? "offene Würfe" : "open throws";
        public static string Throws => CurrentLanguage == Language.Deutsch ? "Würfe" : "throws";

        public static string Shared => CurrentLanguage == Language.Deutsch ? "gt." : "sh.";

        public static string IsNotValideFor => CurrentLanguage == Language.Deutsch ? "ist nicht gültig für" : "is no valide value for";

        public static string Nine => CurrentLanguage == Language.Deutsch ? "Neun" : "Nine";
        public static string NineShort => CurrentLanguage == Language.Deutsch ? "9" : "9";

        public static string Ten => CurrentLanguage == Language.Deutsch ? "Zehn" : "Ten";
        public static string TenShort => CurrentLanguage == Language.Deutsch ? "10" : "10";

        public static string Boy => CurrentLanguage == Language.Deutsch ? "Bube" : "Boy";
        public static string BoyShort => CurrentLanguage == Language.Deutsch ? "B" : "B";

        public static string Lady => CurrentLanguage == Language.Deutsch ? "Dame" : "Lady";
        public static string LadyShort => CurrentLanguage == Language.Deutsch ? "D" : "L";

        public static string King => CurrentLanguage == Language.Deutsch ? "König" : "King";
        public static string KingShort => CurrentLanguage == Language.Deutsch ? "K" : "K";

        public static string Ace => CurrentLanguage == Language.Deutsch ? "Ass" : "Ace";
        public static string AceShort => CurrentLanguage == Language.Deutsch ? "A" : "A";

        public static string Street => CurrentLanguage == Language.Deutsch ? "Straße" : "Street";
        public static string StreetShort => CurrentLanguage == Language.Deutsch ? "Str" : "Str";

        public static string Full => CurrentLanguage == Language.Deutsch ? "Volles Haus" : "Full House";
        public static string FullShort => CurrentLanguage == Language.Deutsch ? "V" : "F";

        public static string Poker => CurrentLanguage == Language.Deutsch ? "Poker" : "Poker";
        public static string PokerShort => CurrentLanguage == Language.Deutsch ? "P" : "P";

        public static string Grande => CurrentLanguage == Language.Deutsch ? "Grande" : "Grande";
        public static string GrandeShort => CurrentLanguage == Language.Deutsch ? "G" : "G";


        public static string PlayersAllowed => CurrentLanguage == Language.Deutsch ? "Spieler erlaubt" : "player allowed";
        public static string ColumnsAllowed => CurrentLanguage == Language.Deutsch ? "Spalten erlaubt" : "columns allowed";

        public static string MainTitle => CurrentLanguage == Language.Deutsch ? "Würfel-Poker-App" : "Dice-Poker-App";

        public static string NewGame => CurrentLanguage == Language.Deutsch ? "Neues Spiel" : "new game";
        public static string LoadAutoSave => CurrentLanguage == Language.Deutsch ? "Letzen Spielstand laden" : "load auto save";
        public static string LoadGame => CurrentLanguage == Language.Deutsch ? "Spiel laden" : "load game";


        public static string LoadingFailedRebuild => CurrentLanguage == Language.Deutsch ? "Laden fehlgeschlagen, letzter Spielstand wurde überschrieben." : "Loading failed, Logfile was rebuild.";
        public static string LoadingFailed => CurrentLanguage == Language.Deutsch ? "Spielstand laden fehlgeschlagen." : "Loading failed";

        public static string MaxCharacters => CurrentLanguage == Language.Deutsch ? "Zeichen erlaubt" : "characters allowed";
        public static string FirstEnterNumberOfColumns => CurrentLanguage == Language.Deutsch ? "Bitte zuerst die Spaltenanzahl eingeben." : "At first, please ente a valid number of colums.";

        public static string ChangeLanguage => CurrentLanguage == Language.Deutsch ? "Sprache ändern" : "change language";

        public static string Points => CurrentLanguage == Language.Deutsch ? "Punkte" : "Points";

        public static string Rankings => CurrentLanguage == Language.Deutsch ? "Punktestand" : "Ranking";
    }
}
