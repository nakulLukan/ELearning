﻿using Learning.Shared.Enums;

namespace Learning.Shared.Common.Dto;
public class FieldErrorDto
{
    public string PropertyName { get; set; }
    public string Message { get; set; }
    public ErrorType Type { get; set; }
    public FieldErrorDto(string propertyName, string message, ErrorType type = ErrorType.Error)
    {
        PropertyName = propertyName;
        Message = message;
        Type = type;
    }
}