namespace DIT.Test_Title_Management_Service.Application.Features.Workers.GetWorkerById;

using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Workers;
using Microsoft.EntityFrameworkCore;

public class GetWorkerByIdQueryHandler(IApplicationDbContext context)
{
    public async Task<Result<GetWorkerByIdResponse>> Handle(GetWorkerByIdQuery query, CancellationToken ct)
    {
        var worker = await context.Workers.SingleOrDefaultAsync(x => x.Id == query.Id, ct);
        if (worker is null)
        {
            return Result.Fail(new NotFoundError<Worker, Guid>(query.Id));
        }

        var response = new GetWorkerByIdResponse
        {
            Id = worker.Id,
            Username = worker.Username,
            FirstName = worker.Profile.FirstName,
            LastName = worker.Profile.LastName,
            Roles = worker.Roles,
        };

        return Result.Ok(response);
    }
}