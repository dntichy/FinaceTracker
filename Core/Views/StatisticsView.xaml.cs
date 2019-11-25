using Core.Models;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
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

        public StatisticsView()
        {
            InitializeComponent();
            DataContext = this;

            FormatterForY = value => value + " €";



            //todo fill in the empty dates
            DateTime MinDate = new DateTime(2019, 10, 1);//todo input possibility 
            DateTime MaxDate = new DateTime(2019, 10, 31);

            var TransactionRepo = new TransactionRecordRepository();
            //var aggreg = TransactionRepo.TxRepository
            //    .Where(n => n.Date > MinDate).Where(n => n.Date < MaxDate)
            //    .GroupBy(n => n.Date.GetValueOrDefault().Day)
            //    .Select(g => g.Sum(x => x.Amount));

            var testAggr = Enumerable.Range(0, 31)
        .Select(a => MinDate.AddDays(a))
        .Where(n => n.Date >= MinDate).Where(n => n.Date <= MaxDate)
        .GroupJoin(TransactionRepo.TxRepository,
        outer => outer,
        inner => inner.Date,
        (outer, group) => new XYAxisHelper
        {
            x = outer.Day,
            y = group.Sum(n => n.Amount)
        });


            Console.WriteLine(testAggr);

            var dayConfig = Mappers.Xy<XYAxisHelper>()
                          .X(dayModel => dayModel.x)
                          .Y(dayModel => dayModel.y);

            SeriesCollection = new SeriesCollection(dayConfig) {
                new LineSeries {
                    Values = new ChartValues<XYAxisHelper>(testAggr)
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

        public SeriesCollection SeriesCollection
        {
            get;
            private set;
        }
    }
}