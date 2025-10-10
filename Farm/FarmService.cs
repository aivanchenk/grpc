namespace Servers;

using System.Threading.Tasks;
using Grpc.Core;

public class FarmService : Services.Farm.FarmBase
{
    // NOTE: replace this stub logic with your real FarmLogic calls
    private readonly FarmLogic mLogic = new FarmLogic();

    public override Task<Services.SubmissionResult> SubmitFood(Services.SubmitRequest request, ServerCallContext context)
    {
        var logicResult = mLogic.SubmitFood(request.Amount);
        var response = new Services.SubmissionResult
        {
            IsAccepted = logicResult.IsAccepted,
            FailReason = logicResult.FailReason ?? string.Empty
        };
        return Task.FromResult(response);
    }

     public override Task<Services.SubmissionResult> SubmitWater(Services.SubmitRequest request, ServerCallContext context)
    {
        var logicResult = mLogic.SubmitWater(request.Amount);
        var response = new Services.SubmissionResult
        {
            IsAccepted = logicResult.IsAccepted,
            FailReason = logicResult.FailReason ?? string.Empty
        };
        return Task.FromResult(response);
    }
}
