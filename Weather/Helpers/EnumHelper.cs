﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Weather.Helpers
{
    public class EnumHelper
    {
        public static IEnumerable<KeyValuePair<string, string>> GetAllValuesAndDescriptions<TEnum>()
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an Enumeration type");
            }

            return from e in Enum.GetValues(typeof(TEnum)).Cast<Enum>()
                   select new KeyValuePair<string, string>(e.ToString(), e.ToString());
        }
    }
}