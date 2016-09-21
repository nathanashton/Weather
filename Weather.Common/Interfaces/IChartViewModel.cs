using System;

namespace Weather.Common.Interfaces
{
    public interface IChartViewModel
    {
        string Header { get; }

        ISelectedStation SelectedStation { get; set; }

        bool OptionsOpened { get; set; }

        void ChangesMadeToSelectedStation(object sender, System.EventArgs e);

        void RecordsUpdatedForSelectedStation(object sender, System.EventArgs e);

        void SelectedStation_GetRecordsCompleted(object sender, System.EventArgs e);

        void SelectedStation_SelectedStationChanged(object sender, System.EventArgs e);

        void DrawChart();

        event EventHandler ChartDone;

        void OnChartDone();

        void SavePosition();

        void LoadPosition();
    }
}