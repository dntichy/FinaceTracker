using Core.Models;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class StatisticsView : UserControl, INotifyPropertyChanged
    {
        public double TotalMonthlySpent { get; set; }
        public Func<double, string> FormatterForY { get; set; }
        TransactionRecordRepository TransactionRepo = new TransactionRecordRepository();


        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler(this, new PropertyChangedEventArgs(name));
        }

        public StatisticsView()
        {
            InitializeComponent();
            DataContext = this;

            PrepareMonthCombobox();
            PrepareYearsCombobox();

            FormatterForY = value => value + " €";

            RegenerateCharts();
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
            ColumnChart.Series = new SeriesCollection(dayConfig) {
                new ColumnSeries {
                    Values = new ChartValues<XYAxisHelper>(totalSpentInGivenInterval)
                }
            };

            ColumnChart.AxisX = new AxesCollection()
{
                new Axis()
                {
                    Title = "Day of month",
                    Separator = new LiveCharts.Wpf.Separator()
                    {
                        Step = 1.0,
                        IsEnabled = true
                    },
                    MaxValue = 32

                }
};
        }

        private void CreateLineChart(IEnumerable<XYAxisHelper> totalSpentInGivenInterval)
        {
            CartesianMapper<XYAxisHelper> dayConfig = Mappers
              .Xy<XYAxisHelper>()
                        .X(model => model.x)
                        .Y(model => model.y);
            LineChart.Series = new SeriesCollection(dayConfig) {
                new LineSeries {
                    Values = new ChartValues<XYAxisHelper>(totalSpentInGivenInterval)
                }
            };
            LineChart.AxisX = new AxesCollection()
{
                new Axis()
                {
                    Title = "Day of month",
                    Separator = new LiveCharts.Wpf.Separator()
                    {
                        Step = 1.0,
                        IsEnabled = true
                    }
                }
};
        }

        private void PrepareYearsCombobox()
        {
            yearComboBox.ItemsSource = Enumerable.Range(2018, DateTime.Now.Year - 2018 + 1).ToList();
            yearComboBox.SelectedItem = DateTime.Now.Year;
        }



        private void PrepareMonthCombobox()
        {
            monthsCombobox.ItemsSource = CultureInfo.InvariantCulture.DateTimeFormat
                                                     .MonthNames.Take(12).ToList();
            monthsCombobox.SelectedItem = CultureInfo.InvariantCulture.DateTimeFormat
                                                    .MonthNames[DateTime.Now.AddMonths(-1).Month];
        }

        private void MonthsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RegenerateCharts();

        }

        private void RegenerateCharts()
        {
            if (monthsCombobox.SelectedItem == null || yearComboBox.SelectedItem == null) return;

            var monthTxt = monthsCombobox.SelectedItem.ToString();
            var yearTxt = yearComboBox.SelectedItem.ToString();

            var year = int.Parse(yearTxt);
            Enum.TryParse(monthTxt, true, out Months monthEnum);

            DateTime MinDate = new DateTime(year, (int)monthEnum, 1);


            var filtered = TransactionRepo.TxRepository
                 .Where(n => GetMonthNameFromMonth(n.Date.Value.Month) == monthTxt)
                 .Where(n => n.Date.Value.Year == year);

            var totalSpentInGivenInterval = Enumerable.Range(0, DateTime.DaysInMonth(year, (int)monthEnum))
            .Select(a => MinDate.AddDays(a).Day)
            .GroupJoin(filtered,
            outer => outer,
            inner => inner.Date.Value.Day,
            (outer, group) => new XYAxisHelper
            {
                x = outer,
                y = group.Sum(n => n.Amount)
            }).ToList();

            TotalMonthlySpent = totalSpentInGivenInterval.Sum(n => n.y);
            OnPropertyChanged("TotalMonthlySpent");

            var amountPerCategories = TransactionRepo.TxRepository
             .Where(n => GetMonthNameFromMonth(n.Date.Value.Month) == monthTxt).Where(n => n.Date.Value.Year == year)
             .GroupBy(n => n.Category.Name)
             .Select(group => new PieChartCategoryWrapper
             {
                 category = group.Key,
                 sumAmounts = group.Sum(x => x.Amount),
                 color = group.Select(n => n.Category.Color)
             });


            CreateCollumnChart(totalSpentInGivenInterval);
            CreateLineChart(totalSpentInGivenInterval);

            CreatePieChart(amountPerCategories);
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RegenerateCharts();
        }
    }
}