using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Net;

namespace Xacte.ApiGateway.Aggregators
{
    internal sealed class PatientAggregator : IDefinedAggregator
    {
        public Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            var content = responses.First().Response.Body;
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StreamContent(content)
            };

            return Task.FromResult(new DownstreamResponse(responseMessage));
        }
    }
}
