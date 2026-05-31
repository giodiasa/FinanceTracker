using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FinanceTracker.Application.DTOs.Category;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCategoryRequestDto, Category>();
            CreateMap<UpdateCategoryRequestDto, Category>();
            CreateMap<Category, CategoryResponseDto>();
        }
    }
}
