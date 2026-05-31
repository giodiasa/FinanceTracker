using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FinanceTracker.Application.DTOs.Category;
using FinanceTracker.Application.DTOs.Transaction;
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
            CreateMap<CreateTransactionRequestDto, Transaction>();
            CreateMap<UpdateTransactionRequestDto, Transaction>();
            CreateMap<Transaction, TransactionResponseDto>()
                .ForMember(
                dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name)); ;
        }
    }
}
