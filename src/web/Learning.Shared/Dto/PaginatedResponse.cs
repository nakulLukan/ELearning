﻿namespace Learning.Shared.Dto;

public class PaginatedResponse<T>
{
    public IEnumerable<T> Data { get; set; }
    public int TotalRecords { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }

    public PaginatedResponse(IEnumerable<T> data, int totalRecords)
    {
        TotalRecords = totalRecords;
        Data = data;
    }
}
