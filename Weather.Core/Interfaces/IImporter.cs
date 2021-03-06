﻿using System;
using System.Collections.Generic;
using Weather.Common.EventArgs;
using Weather.Common.Interfaces;

namespace Weather.Core.Interfaces
{
    public interface IImporter
    {
        void Import(string filePath, IWeatherStation station, List<Tuple<ISensor, int>> data, int excludeLines,
            params int[] timestamp);

        void Start();

        event EventHandler<ImportEventArgs> ImportChanged;

        event EventHandler ImportComplete;
    }
}