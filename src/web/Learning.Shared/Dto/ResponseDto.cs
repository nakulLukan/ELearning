﻿using Learning.Shared.Enums;
using System.Text.Json.Serialization;

namespace Learning.Shared.Common.Dto;
public class ResponseDto<TData>
{
    public TData? Data { get; set; }

    public List<FieldErrorDto> Errors { get; set; }

    public ErrorDto? Error { get; set; }

    public FormError? FormError { get; set; }

    [JsonConstructor]
    public ResponseDto(TData data)
    {
        Data = data;
    }

    public ResponseDto(IEnumerable<FieldErrorDto> errors)
    {
        Errors = errors.ToList();
    }


    public ResponseDto(ErrorDto error)
    {
        Error = error;
    }

    public ResponseDto(FormError formError)
    {
        FormError = formError;
    }

    public bool HasData()
    {
        return Data is not null;
    }

    public bool HasErrors => (Errors is not null && Errors.Any());
    public bool HasError => Error is not null;
    public bool HasFormError => FormError is not null;

    public static ResponseDto<TData> ShowOopsError(string message)
    {
        return new ResponseDto<TData>(new ErrorDto($"Error: {message}", ErrorType.Error));
    }
}