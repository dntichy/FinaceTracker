using Core.Models;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Core.Views
{
    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : UserControl
    {
        public Func<double, string> FormatterForY { get; set; }
        TransactionRecordRepository TransactionRepo = new TransactionRecordRepository();
       

        public StatisticsView()
        {
            InitializeComponent();
            DataContext = this;

            PrepareMonthCombobox();
            PrepareYearsCombobox();

            FormatterForY = value => value + " €";


            var month = monthsCombobox.Text;
            var year = int.Parse(yearComboBox.Text);
            Enum.TryParse(month, true, out Months mnth);

            DateTime MinDate = new DateTime(year, (int)mnth, 1);

            var totalSpentInGivenInterval = Enumerable.Range(0, DateTime.DaysInMonth(year, (int)mnth)-1)
            .Select(a => MinDate.AddDays(a))
            .Where(n => GetMonthNameFromMonth(n.Date.Month) == month).Where(n => n.Date.Year == year)
            .GroupJoin(TransactionRepo.TxRepository,
            outer => outer,
            inner => inner.Date,
            (outer, group) => new XYAxisHelper
            {
                x = outer.Day,
                y = group.Sum(n => n.Amount)
            });


            var amountPerCategories = TransactionRepo.TxRepository
             .Where(n => n.Date >= MinDate)
             .GroupBy(n => n.Category.Name)
             .Select(group => new PieChartCategoryWrapper
             {
                 category = group.Key,
                 sumAmounts = group.Sum(x => x.Amount),
                 color = group.Select(n => n.Category.Color)
             });


            CreateLineChart(totalSpentInGivenInterval);
            CreateCollumnChart(totalSpentInGivenInterval);
            CreatePieChart(amountPerCategories);
        }


        private string GetMonthNameFromMonth(int month)
        {
            return CultureInfo.InvariantCulture.DateTimeFormat.MonthNames[month - 1];
        }

        private void CreatePieChart(IEnumerable<PieChartCategoryWrapper> amountPerCategories)
        {
            Func<ChartPoint, string> labelPoint = chartPoint =>
             string.Format("{0:n0} ({1:P})", chartPoint.Y, chartPoint.Participation);


            PieChart.Series = new SeriesCollection(amountPerCategories);

            foreach (var item in amountPerCategories)
            {
                PieSeries pie = new PieSeries();
                pie.Title = item.category;
                pie.Values = new ChartValues<float> { item.sumAmounts };
                pie.LabelPoint = labelPoint;
                pie.DataLabels = true;
                PieChart.Series.Add(pie);
            }
            PieChart.LegendLocation = LegendLocation.Right;
        }

        private void CreateCollumnChart(IEnumerable<XYAxisHelper> totalSpentInGivenInterval)
        {
            CartesianMapper<XYAxisHelper> dayConfig = Mappers
              .Xy<XYAxisHelper>()
                        .X(model => model.x)
                        .Y(model => model.y);
            SeriesCollection2 = new SeriesCollection(dayConfig) {
                new ColumnSeries {
                    Values = new ChartValues<XYAxisHelper>(totalSpentInGivenInterval)
                }
            };
        }

        private void CreateLineChart(IEnumerable<XYAxisHelper> totalSpentInGivenInterval)
        {
            CartesianMapper<XYAxisHelper> dayConfig = Mappers
              .Xy<XYAxisHelper>()
                        .X(model => model.x)
                        .Y(model => model.y);
            SeriesCollection = new SeriesCollection(dayConfig) {
                new LineSeries {
                    Values = new ChartValues<XYAxisHelper>(totalSpentInGivenInterval)
                }
            };

            TestChart.AxisX = new AxesCollection()
{
                new Axis()
                {
                    Title = "Day of month",
                    Separator = new LiveCharts.Wpf.Separator()
                    {
                        Step = 1.0,
                        IsEnabled = false
                    }

                }
};
        }

        private void PrepareYearsCombobox()
        {
            yearComboBox.ItemsSource = Enumerable.Range(2018, DateTime.Now.Year - 2018 + 1).ToList();
            yearComboBox.SelectedItem = DateTime.Now.Year;
        }

        private void RefreshChartsAccordingCombBoxes()
        {

        }

        private void PrepareMonthCombobox()
        {
            monthsCombobox.ItemsSource = CultureInfo.InvariantCulture.DateTimeFormat
                                                     .MonthNames.Take(12).ToList();
            monthsCombobox.SelectedItem = CultureInfo.InvariantCulture.DateTimeFormat
                                                    .MonthNames[DateTime.Now.AddMonths(-1).Month - 1];
        }

        public SeriesCollection SeriesCollection
        {
            get;
            private set;
        }
        public SeriesCollection SeriesCollection2
        {
            get;
            private set;
        }

    }
}