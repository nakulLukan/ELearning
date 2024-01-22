using Learning.Business.Dto.Master.Lookup;
using Learning.Business.Impl.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Master.Lookup;

public class LookupSelectQuery : IRequest<List<LookupSelectDto>>
{
    public string LookupMasterInternalName { get; set; }
}

public class LookupSelectQueryHandler : IRequestHandler<LookupSelectQuery, List<LookupSelectDto>>
{
    readonly IAppDbContext _dbContext;

    public LookupSelectQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<LookupSelectDto>> Handle(LookupSelectQuery request, CancellationToken cancellationToken)
    {
        var lookupValues = await _dbContext.LookupValues
            .Where(x => x.LookupMaster.InternalName == request.LookupMasterInternalName)
            .Select(x => new LookupSelectDto
            {
                Id = x.Id,
                Value = x.DisplayValue,
            })
            .ToListAsync(cancellationToken);

        return lookupValues;
    }
}
