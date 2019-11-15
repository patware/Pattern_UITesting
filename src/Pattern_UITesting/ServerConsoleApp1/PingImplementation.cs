using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.Protos;
using Grpc.Core;

namespace ServerConsoleApp1
{
    public class PingImplementation : ClassLibrary1.Protos.Ping.PingBase
    {
        public override Task<PingResult> Ping(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            return Task.FromResult(new PingResult { Result = "Pong from a service that would be part of a classic nTier" });
        }
    }
}
