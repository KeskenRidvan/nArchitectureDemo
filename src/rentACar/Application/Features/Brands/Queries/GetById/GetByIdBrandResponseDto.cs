﻿namespace Application.Features.Brands.Queries.GetById;
public class GetByIdBrandResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
}