using Ocelot.Configuration;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Net;
using System.Text;
using System.Text.Json;
using Xacte.ApiGateway.Extensions;

namespace Xacte.ApiGateway.Aggregators
{
    internal sealed class PatientAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            var sb = new StringBuilder();

            foreach (var response in responses)
            {
                if (response == null) continue;

                var downStreamRouteKey = (response.Items["DownstreamRoute"] as DownstreamRoute).Key;
                var downstreamResponse = response.Items["DownstreamResponse"] as DownstreamResponse;
                var downstreamEncoding = downstreamResponse.Content.Headers.ContentEncoding.First();
                byte[] downstreamResponseContent = await downstreamResponse!.Content.ReadAsByteArrayAsync();

                if (downStreamRouteKey == "PATIENT")
                {
                    var data = ContentExtensions.ReadBytes(downstreamResponseContent, downstreamEncoding);
                    sb.AppendLine(JsonSerializer.Deserialize<string>(data));
                }
                if (downStreamRouteKey == "PATIENT_BILLING")
                {
                    var data = ContentExtensions.ReadBytes(downstreamResponseContent, downstreamEncoding);
                    sb.AppendLine(JsonSerializer.Deserialize<string>(data));
                }
            }

            return new DownstreamResponse(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(sb.ToString())
            });
        }
    }
}
