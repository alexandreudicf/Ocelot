using Ocelot.Middleware;
using Ocelot.Responses;
using Ocelot.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ocelot.LoadBalancer.LoadBalancers
{
    public class NoLoadBalancer : ILoadBalancer
    {
        private readonly Func<Task<List<Service>>> _services;

        public NoLoadBalancer(Func<Task<List<Service>>> services)
        {
            _services = services;
        }

        public async Task<Response<ServiceHostAndPort>> Lease(DownstreamContext downstreamContext)
        {
            var services = await _services();

            if (services == null || services.Count == 0)
            {
                return new ErrorResponse<ServiceHostAndPort>(new ServicesAreEmptyError("There were no services in NoLoadBalancer"));
            }

            var service = await Task.FromResult(services.FirstOrDefault());
            var my_service = new Service("", new ServiceHostAndPort(downstreamContext.DownstreamReRoute.DownstreamAddresses.FirstOrDefault().Host, downstreamContext.DownstreamReRoute.DownstreamAddresses.FirstOrDefault().Port), "", "", Enumerable.Empty<string>());
            if (service.HostAndPort.DownstreamHost == my_service.HostAndPort.DownstreamHost && service.HostAndPort.DownstreamPort == my_service.HostAndPort.DownstreamPort)
            {
                return new OkResponse<ServiceHostAndPort>(service.HostAndPort);
            }
            else
            {
                return new OkResponse<ServiceHostAndPort>(my_service.HostAndPort);
            }
        }

        public void Release(ServiceHostAndPort hostAndPort)
        {
        }
    }
}
