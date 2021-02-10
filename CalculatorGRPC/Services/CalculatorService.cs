using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcService1.Services
{
    public class CalculatorService : Calculator.CalculatorBase
    {
        private readonly ILogger<CalculatorService> _logger;
        private readonly IDictionary<string, Func<double[], double>> funcs;

        public CalculatorService(ILogger<CalculatorService> logger, IDictionary<string, Func<double[],double>> funcs)
        {
            _logger = logger;
            this.funcs = funcs;
        }

        public override Task<MathResponse> Calculate(MathRequest request, ServerCallContext context)
        {
            Thread.Sleep(TimeSpan.FromSeconds(10));
            return Task.FromResult(new MathResponse() { Result = funcs[request.Func](request.Param.ToArray()) });
        }
    }
}
