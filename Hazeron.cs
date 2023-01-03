using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace HazeronProspector
{
    public static class Hazeron
    {
        public const int AbandonmentInterval = 7;

        public const int CurrencyPadding = 16;

        private static NumberFormatInfo _numberFormat;
        public static NumberFormatInfo NumberFormat
        {
            get { return _numberFormat; }
            //set { _numberFormat = value; }
        }

        private static DateTimeFormatInfo _dateTimeFormat;
        public static DateTimeFormatInfo DateTimeFormat
        {
            get { return _dateTimeFormat; }
            //set { _dateTimeFormat = value; }
        }

        private static string[] _pirateEmpires;
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

        public static int ConvertFromBase26(string base26Number)
        { // Based on: https://stackoverflow.com/a/9559248
            if (string.IsNullOrEmpty(base26Number))
                throw new ArgumentException($"{nameof(base26Number)} cannot be null or empty.", nameof(base26Number));

            const string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (base26Number.Any(c => !charset.Contains(c)))
                throw new ArgumentException($"{nameof(base26Number)} ({base26Number}) is not valid.", nameof(base26Number));

            int result = 0;
            for (int i = base26Number.Length - 1; i >= 0; i--)
            {
                char digit = base26Number[i];
                result = result * charset.Length + charset.IndexOf(digit);
            }

            return result;
        }

        public static string ConvertToBase26(int number)
        { // Based on: https://stackoverflow.com/a/95331
            if (number < 0)
                throw new ArgumentException($"{nameof(number)} cannot be negative.", nameof(number));
            //if (number == 0)
            //    return "A"; //0 would skip while loop

            const string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string result = "";
            while (number >= 0)
            {
                result = charset[number % charset.Length] + result;
                number /= charset.Length;
            }

            return result;
        }

        public static Tuple<double, double, double> ConvertCalalogNameToCoordinates(string catalogName)
        {
            if (string.IsNullOrEmpty(catalogName))
                throw new ArgumentException($"{nameof(catalogName)} cannot be null or empty.", nameof(catalogName));
            if (catalogName.Count(c => c == '\'') != 3 || !"abcdefgh".Contains(catalogName[catalogName.Length - 1]))
                throw new ArgumentException($"{nameof(catalogName)} ({catalogName}) did not appear to be a valid catalog name.", nameof(catalogName));

            string[] catalogNameParts = catalogName.Split(new char[] { '\'' }, StringSplitOptions.RemoveEmptyEntries);
            if (catalogNameParts.Length != 4)
                throw new ArgumentException($"{nameof(catalogName)} ({catalogName}) did not appear to be a valid catalog name.", nameof(catalogName));

            double x = (double)ConvertFromBase26(catalogNameParts[0]) / 10;
            double y = (double)ConvertFromBase26(catalogNameParts[1]) / 10;
            double z = (double)ConvertFromBase26(catalogNameParts[2]) / 10;
            switch (catalogNameParts[3])
            {
                case "a":
                    break;
                case "b":
                    x *= -1;
                    break;
                case "c":
                    y *= -1;
                    break;
                case "d":
                    x *= -1;
                    y *= -1;
                    break;
                case "e":
                    z *= -1;
                    break;
                case "f":
                    x *= -1;
                    z *= -1;
                    break;
                case "g":
                    y *= -1;
                    z *= -1;
                    break;
                case "h":
                    x *= -1;
                    y *= -1;
                    z *= -1;
                    break;
            }

            return new Tuple<double, double, double>(x, y, z);
        }
    }
}
