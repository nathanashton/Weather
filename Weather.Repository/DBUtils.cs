﻿using System;

namespace Weather.Repository
{
    internal static class DbUtils
    {




        internal static int? ParseIntNull(string input)
        {
            int value;
            if (int.TryParse(input, out value))
            {
                return (int?) value;
            }
            else return null;
        }


        internal static int ParseIntZero(string input)
        {
            int value;
            return int.TryParse(input, out value) ? value : 0;
        }

        internal static double? ParseDoubleNull(string input)
        {
            if (input == null)
            {
                return null;
            }
            double value;
            if (double.TryParse(input, out value))
            {
                return (double?) value;
            }
            else return null;
        }


        internal static DateTime? ParseDateTimeNull(string input)
        {
            if (input == null)
            {
                return null;
            }
            DateTime value;
            if (DateTime.TryParse(input, out value))
            {
                return value;
            }
            else return null;
        }
    }
}