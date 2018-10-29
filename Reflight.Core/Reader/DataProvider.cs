using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflight.Core.Reader
{
    internal class DataProvider
    {
        private readonly IList<string> headers;
        private readonly IList<IEnumerable<object>> source;

        public DataProvider(IEnumerable<List<object>> idDetailsData, IList<string> idDetailsHeaders)
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