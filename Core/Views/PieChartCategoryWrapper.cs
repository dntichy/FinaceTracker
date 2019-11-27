using System.Collections.Generic;

namespace Core.Views
{
    internal class PieChartCategoryWrapper
    {
        public string category { get; set; }
        public float sumAmounts { get; set; }
        public IEnumerable<string> color { get; set; }
    }
}