using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FixerI0.Library
{
    public class FixerIoClient
    {
        public FixerIoClient(string accessKey)
        {
            _accessKey = accessKey;
            _client = new RestClient("http://data.fixer.io/api");
        }
        private string _accessKey { get; }
        private RestClient _client;

        public Currency GetLatest()
        {
            return Execute("latest");
        }

        public Currency GetLatest(string @base)
        {
            var cur = GetLatest();
            double? value = null;
            foreach (var item in cur.rates)
            {
                if (item.Key.ToLower() == @base.ToLower())
                {
                    cur.@base = @base;
                    value = item.Value;
                    break;
                }
            }
            if (value != null)
            {
                Currency currency = new Currency();
                currency.@base = cur.@base;
                currency.date = cur.date;
                currency.success = cur.success;
                currency.timestamp = cur.timestamp;
                currency.rates = new Dictionary<string, double>();
                foreach (var item in cur.rates)
                {
                    if (item.Key != currency.@base)
                    {
                        currency.rates.Add(item.Key, item.Value / (double)value);
                    }

                }
                return currency;
            }
            else
            {
                return null;
            }
        }

        public Currency GetLatest(params string[] rates)
        {
            string ratePar = string.Join(",", rates);
            var pDic = new Dictionary<string, object> { { "symbols", ratePar } };
            return Execute("latest", pDic);
        }

        public Currency Get(DateTime date)
        {
            return Execute(date.ToString("yyyy-MM-dd"));
        }

        public Currency Get(DateTime date, params string[] rates)
        {
            string ratePar = string.Join(",", rates);
            var pDic = new Dictionary<string, object> { { "symbols", ratePar } };
            return Execute(date.ToString("yyyy-MM-dd"), pDic);
        }

        private Currency Execute(string action, Dictionary<string, object> parameters = null)
        {
            RestRequest request = new RestRequest(action, Method.GET);
            request.AddParameter("access_key", _accessKey);
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }
            IRestResponse<Currency> response = _client.Execute<Currency>(request);
            return response.Data;
        }
    }
}
