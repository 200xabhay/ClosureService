using AutoMapper;
using ClosureServices.Application.DTO;
using ClosureServices.Domain.Entities;
using ClosureServices.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Application.Mapping
{
    public class MapperConfig :Profile
    {
        public MapperConfig()
        {
            CreateMap<ForeclosureRequestDTO, Foreclosure>()
                .ForMember(dest => dest.RequestDate,
                           opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ApprovalStatus,
                           opt => opt.MapFrom(src => Domain.Enums.ForeclosureStatus.Pending))
                .ForMember(dest => dest.CreatedAt,
                           opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Foreclosure, ForeclosureResponseDTO>();

            CreateMap<ForeclosureCalculationDTO, ForeclosureResponseDTO>();

            CreateMap<ForeclosureApprovalDTO, Foreclosure>()
                .ForMember(dest => dest.ApprovalDate,
                           opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<ForeclosureCompleteDTO, Foreclosure>()
                .ForMember(dest => dest.ForeclosureDate,
                           opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<CreateClosureDTO, LoanClosure>()
                .ForMember(dest => dest.ClosureDate,
                    opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ClosureStatus,
                    opt => opt.MapFrom(src => ClosureStatus.Completed))
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy,
                    opt => opt.MapFrom(src => src.ClosureApprovedBy.ToString()));

            CreateMap<LoanClosure, LoanClosureResponseDTO>();
        }

    }
}
