using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.Protos;
using Grpc.Core;

namespace UnitTestProject1.Mocking
{
    public class PingMock : ClassLibrary1.Protos.Ping.PingBase
    {
        public string PingReturn { get; set; }

        public override Task<PingResult> Ping(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            return Task.FromResult(new PingResult { Result = PingReturn });
        }
    }
}
