using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace HazeronProspector
{
    public class Hazeron
    {
        public const int AbandonmentInterval = 7;

        public const int CurrencyPadding = 16;

        protected static NumberFormatInfo _numberFormat;
        public static NumberFormatInfo NumberFormat
        {
            get { return _numberFormat; }
            //set { _numberFormat = value; }
        }

        protected static DateTimeFormatInfo _dateTimeFormat;
        public static DateTimeFormatInfo DateTimeFormat
        {
            get { return _dateTimeFormat; }
            //set { _dateTimeFormat = value; }
        }

        protected static string[] _pirateEmpires;
        public static string[] PirateEmpires
        {
            get { return _pirateEmpires; }
            //set { _pirateEmpires = value; }
        }

        static Hazeron()
        {
            System.Globalization.CultureInfo tempCulture = new System.Globalization.CultureInfo("en-US");
            _numberFormat = tempCulture.NumberFormat;
            _numberFormat.NumberDecimalDigits = 0;
            _numberFormat.NumberDecimalSeparator = ".";
            _numberFormat.NumberGroupSeparator = "'";
            _numberFormat.CurrencyDecimalDigits = 0;
            _numberFormat.CurrencyDecimalSeparator = ".";
            _numberFormat.CurrencyGroupSeparator = "'";
            _numberFormat.CurrencySymbol = "¢";
            _numberFormat.CurrencyPositivePattern = 1; // https://msdn.microsoft.com/en-us/library/system.globalization.numberformatinfo.currencypositivepattern.aspx
            _numberFormat.CurrencyNegativePattern = 5; // https://msdn.microsoft.com/en-us/library/system.globalization.numberformatinfo.currencynegativepattern.aspx
            _numberFormat.NegativeInfinitySymbol = "-∞";
            _numberFormat.PositiveInfinitySymbol = "∞";
            _dateTimeFormat = tempCulture.DateTimeFormat;
            _dateTimeFormat.FullDateTimePattern = "yyyy-MM-dd HH:mm"; // TimeDate format information: http://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx

            _pirateEmpires = new string[] { "Akson"
                                          , "Balorite"
                                          , "Dendrae"
                                          , "Haxu"
                                          , "Kla'tra"
                                          , "Malaco"
                                          , "Myntaka"
                                          , "Ogar"
                                          , "Otari"
                                          , "Seledon"
                                          , "Syth"
                                          , "Tassad"
                                          , "Vilmorti"
                                          , "Vreen"
                                          , "Zuul"
                                          };
        }

        public static bool ValidID(string id)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[A-Z]+$");
            System.Text.RegularExpressions.Match regexMatch = regex.Match(id);
            return (id == null || id == "" || !regexMatch.Success);
        }
    }
}
