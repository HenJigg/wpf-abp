using System;
using System.Collections.Generic;
using AppFramework.Tenants.Dashboard.Dto;

namespace AppFramework.Tenants.Dashboard
{
    public static class DashboardRandomDataGenerator
    {
        private const string DateFormat = "yyyy-MM-dd";
        private static readonly Random Random;
        public static string[] CountryNames = { "Argentina", "China", "France", "Italy", "Japan", "Netherlands", "Russia", "Spain", "Turkey", "United States"};

        static DashboardRandomDataGenerator()
        {
            Random = new Random();
        }

        public static int GetRandomInt(int min, int max)
        {
            return Random.Next(min, max);
        }

        public static int[] GetRandomArray(int size, int min, int max)
        {
            var array = new int[size];
            for (var i = 0; i < size; i++)
            {
                array[i] = GetRandomInt(min, max);
            }

            return array;
        }

        public static int[] GetRandomPercentageArray(int size)
        {
            if (size == 1)
            {
                return new int[100];
            }

            var array = new int[size];
            var total = 0;
            for (var i = 0; i < size - 1; i++)
            {
                array[i] = GetRandomInt(0, 100 - total);
                total += array[i];
            }

            array[size - 1] = 100 - total;

            return array;
        }

        public static List<SalesSummaryData> GenerateSalesSummaryData(SalesSummaryDatePeriod inputSalesSummaryDatePeriod)
        {
            List<SalesSummaryData> data = null;


            switch (inputSalesSummaryDatePeriod)
            {
                case SalesSummaryDatePeriod.Daily:
                    data = new List<SalesSummaryData>
                    {
                        new SalesSummaryData(DateTime.Now.AddDays(-5).ToString(DateFormat), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(DateTime.Now.AddDays(-4).ToString(DateFormat), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(DateTime.Now.AddDays(-3).ToString(DateFormat), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(DateTime.Now.AddDays(-2).ToString(DateFormat), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(DateTime.Now.AddDays(-1).ToString(DateFormat), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                    };

                    break;
                case SalesSummaryDatePeriod.Weekly:
                    var lastYear = DateTime.Now.AddYears(-1).Year;
                    data = new List<SalesSummaryData>
                    {
                        new SalesSummaryData(lastYear + " W4", Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(lastYear + " W3", Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(lastYear + " W2", Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(lastYear + " W1", Random.Next(1000, 2000),
                            Random.Next(100, 999))
                    };

                    break;
                case SalesSummaryDatePeriod.Monthly:
                    data = new List<SalesSummaryData>
                    {
                        new SalesSummaryData(DateTime.Now.AddMonths(-4).ToString("yyyy-MM"), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(DateTime.Now.AddMonths(-3).ToString("yyyy-MM"), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(DateTime.Now.AddMonths(-2).ToString("yyyy-MM"), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"), Random.Next(1000, 2000),
                            Random.Next(100, 999))
                    };

                    break;
            }

            return data;
        }

        public static List<MemberActivity> GenerateMemberActivities()
        {
            return new List<MemberActivity>
            {
                new MemberActivity("Brain", AppFrameworkConsts.CurrencySign + GetRandomInt(100, 500), GetRandomInt(10, 100), GetRandomInt(10, 150),
                    GetRandomInt(10, 99) + "%"),

                new MemberActivity("Jane", AppFrameworkConsts.CurrencySign + GetRandomInt(100, 500), GetRandomInt(10, 100), GetRandomInt(10, 150),
                    GetRandomInt(10, 99) + "%"),

                new MemberActivity("Tim", AppFrameworkConsts.CurrencySign + GetRandomInt(100, 500), GetRandomInt(10, 100), GetRandomInt(10, 150),
                    GetRandomInt(10, 99) + "%"),

                new MemberActivity("Kate", AppFrameworkConsts.CurrencySign + GetRandomInt(100, 500), GetRandomInt(10, 100), GetRandomInt(10, 150),
                    GetRandomInt(10, 99) + "%")
            };
        }

        public static List<RegionalStatCountry> GenerateRegionalStat()
        {
            var stats = new List<RegionalStatCountry>();
            for (var i = 0; i < 4; i++)
            {
                var countryIndex = GetRandomInt(0, CountryNames.Length);
                stats.Add(new RegionalStatCountry
                {
                    CountryName = CountryNames[countryIndex],
                    AveragePrice = GetRandomInt(10, 100),
                    Sales = GetRandomInt(10000, 100000),
                    TotalPrice = GetRandomInt(10000, 50000),
                    Change = new List<int>
                    {
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20)
                    }
                });
            }

            return stats;
        }
    }
}