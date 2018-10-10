using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightVisualizer.Core
{
    internal class DataProvider
    {
        private readonly List<string> headers;
        private readonly IList<IEnumerable<object>> source;

        public DataProvider(IEnumerable<List<object>> idDetailsData, List<string> idDetailsHeaders)
        {
            this.source = idDetailsData.Transpose().ToList();
            this.headers = idDetailsHeaders;
        }

        public IEnumerable<TOut> GetData<TIn, TOut>(string header, Func<TIn, TOut> selector)
        {
            var row = source[headers.IndexOf(header)];
            return row.Select(o => selector((TIn)o));
        }
    }
}