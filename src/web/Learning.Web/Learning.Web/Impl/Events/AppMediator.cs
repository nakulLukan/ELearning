using FluentValidation;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using Learning.Web.Contracts.Events;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace Learning.Web.Impl.Events;

public class AppMediator : IAppMediator
{
    private readonly IMediator mediator;
    private readonly ILogger<AppMediator> logger;
    private readonly NavigationManager _navigationManager;

    public AppMediator(IMediator mediator, ILogger<AppMediator> logger, NavigationManager navigationManager)
    {
        this.mediator = mediator;
        this.logger = logger;
        _navigationManager = navigationManager;
    }

    public async Task<ResponseDto<TData>> Send<TData>(IRequest<ResponseDto<TData>> request, bool showLoader = false)
    {
        try
        {
            return await mediator.Send(request);
        }
        catch (ValidationException ex)
        {
            return new ResponseDto<TData>(new FormError(ex.Errors));
        }
        catch (AppException ex)
        {
            logger.LogError(ex, "Mediator failed for request {request}", request.GetType().Name);
            NavigateIfInAccessable(ex);
            return new ResponseDto<TData>(new ErrorDto(ex.ErrorMessage));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{0} mediator request failed.", request.GetType().Name);
            return new ResponseDto<TData>(new ErrorDto("Oops, something went wrong."));
        }
    }

    private void NavigateIfInAccessable(AppException ex)
    {
        if (ex.IsInAccessible)
        {
            _navigationManager.NavigateTo("/unauthorized");
        }
    }

    public async Task<ResponseDto<TResponse>> Send<TResponse>(IRequest<TResponse> request, bool showLoader = false)
    {
        try
        {
            return new ResponseDto<TResponse>(await mediator.Send(request));
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, "Validation exception");
            return new ResponseDto<TResponse>(new FormError(ex.Errors));
        }
        catch (AppException ex)
        {
            logger.LogError(ex, "Mediator failed for request {request}", request.GetType().Name);
            NavigateIfInAccessable(ex);
            return new ResponseDto<TResponse>(new ErrorDto(ex.ErrorMessage));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Validation exception");
            return new ResponseDto<TResponse>(new ErrorDto("Oops, something went wrong."));
        }
    }
}