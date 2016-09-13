using System;
using Weather.Common.Interfaces;

namespace Weather.Interfaces
{
    public interface IChartViewModel
    {
        string Header { get; }
        ISelectedStation SelectedStation { get; set; }

        void SelectedStation_RecordsUpdated(object sender, EventArgs e);

        void SelectedStation_GetRecordsCompleted(object sender, EventArgs e);

        void SelectedStation_SelectedStationChanged(object sender, EventArgs e);

        void DrawChart();
    }
}