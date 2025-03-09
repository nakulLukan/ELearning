using Learning.Business.Impl.Data;
using Learning.Shared.Common.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.DataCollection.ContactUsRequest;

public class DeleteContactUsRequestCommand : IRequest<ResponseDto<bool>>
{
    public long Id { get; set; }
}
public class DeleteContactUsRequestCommandHandler : IRequestHandler<DeleteContactUsRequestCommand, ResponseDto<bool>>
{
    private readonly IAppDbContext _appDbContext;

    public DeleteContactUsRequestCommandHandler(IAppDbContextFactory appDbContext)
    {
        _appDbContext = appDbContext.CreateDbContext();
    }

    public async Task<ResponseDto<bool>> Handle(DeleteContactUsRequestCommand request, CancellationToken cancellationToken)
    {
        var count = await _appDbContext.ContactUsRequests.Where(x => x.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);
        await _appDbContext.SaveAsync(cancellationToken);
        return new(count > 0);
    }
}


