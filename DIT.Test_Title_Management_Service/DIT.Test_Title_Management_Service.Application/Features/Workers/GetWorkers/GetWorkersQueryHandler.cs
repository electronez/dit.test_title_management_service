namespace DIT.Test_Title_Management_Service.Application.Features.Workers.GetWorkers;

using Microsoft.EntityFrameworkCore;

public class GetWorkersQueryHandler(IApplicationDbContext context)
{
    public async Task<Result<GetWorkersResponse>> Handle(GetWorkersQuery request, CancellationToken ct)
    {
        var result = await context.Workers
            .Select(x => new WorkerResponse
            {
                Id = x.Id,
                Username = x.Username,
                FirstName = x.Profile.FirstName,
                LastName = x.Profile.LastName,
                Roles = x.Roles,
            })
            .ToArrayAsync(ct);

        return Result.Ok(new GetWorkersResponse(result));
    }
}