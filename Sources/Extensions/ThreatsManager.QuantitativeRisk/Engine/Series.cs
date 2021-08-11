using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    public class Series
    {
        private readonly List<double> _raw = new List<double>();

        public List<double> Samples
        {
            get => _raw;
            set
            {
                _raw.Clear();
                _raw.AddRange(value);
            }
        }

        public IEnumerable<KeyValuePair<double, int>> GetColumns([StrictlyPositive] int count)
        {
            List<KeyValuePair<double, int>> result = new List<KeyValuePair<double, int>>();

            Histogram histogram = new Histogram(_raw, count);
            
            for (int i = 0; i < count; i++)
            {
                var bucket = histogram[i];
                result.Add(new KeyValuePair<double, int>(bucket.LowerBound + ((bucket.UpperBound - bucket.LowerBound) / 2.0), (int)bucket.Count));
            }

            return result;
        }

        public int Count => _raw.Count;
        public double Min => _raw.Minimum();
        public double Max => _raw.Maximum();
        public double Mean => _raw.Mean();
        public double Median => _raw.Median();

        public double GetMode([StrictlyPositive] int count)
        {
            var columns = GetColumns(count);
            return columns.OrderByDescending(x => x.Value).Select(x => x.Key).FirstOrDefault();
        }

        public double GetPercentile(int percentile)
        {
            return _raw.Percentile(percentile);
        }
    }
}