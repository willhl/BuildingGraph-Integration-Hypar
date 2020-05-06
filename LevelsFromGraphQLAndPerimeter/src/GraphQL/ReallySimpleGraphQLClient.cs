using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Dynamic;
using System.Text;

namespace LevelsFromGraphQLAndPerimeter.GraphQL
{
    public class ReallySimpleGraphQLClient
    {

        public string URI { get; private set; }
        public Dictionary<string, string> Headers { get; private set; }
        public ReallySimpleGraphQLClient(string uri, Dictionary<string, string> headers)
        {
            URI = uri;
            Headers = headers;
        }

        public dynamic SendRequest(string graphQLquery, Dictionary<string, object> variables)
        {


            //turn dictionary into dynamic object so it serializes cleanly
            dynamic queryVariablesDynamic = variables.Aggregate(new ExpandoObject() as IDictionary<string, Object>, (a, p) => { a.Add(p.Key, p.Value); return a; });

            //package up GraphQL full query for POST request.
            var fullQueryRequest = new
            {
                query = graphQLquery,
                variables = queryVariablesDynamic
            };

            var reqJson = Newtonsoft.Json.JsonConvert.SerializeObject(fullQueryRequest);
            var data = Encoding.Default.GetBytes(reqJson);

            var httpRequest = WebRequest.CreateHttp(URI);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.ContentLength = data.Length;

            foreach (var header in Headers)
            {
                httpRequest.Headers.Add(header.Key, header.Value);
            }

            var newStream = httpRequest.GetRequestStream(); 
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            var response = httpRequest.GetResponse();
            var responseReader = new System.IO.StreamReader(response.GetResponseStream());
            var responseText = responseReader.ReadToEnd();

            dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(responseText);

            return obj.data;
        }
    }
}
