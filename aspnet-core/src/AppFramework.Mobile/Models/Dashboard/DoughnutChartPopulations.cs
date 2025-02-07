using System;

namespace AppFramework.Shared.Models
{
    public class DoughnutChartPopulations
    {
        public string Continent { get; set; }

        public string Countries { get; set; }

        public string States { get; set; }

        public double PopulationinStates { get; set; }

        public double PopulationinCountries { get; set; }

        public double PopulationinContinents { get; set; }

        public string Category { get; set; }

        public double Expenditure { get; set; }

        public Uri Image { get; set; }
    }
}