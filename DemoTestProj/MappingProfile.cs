using AutoMapper;
using Survey.Contract.Model;
using Survey.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Survey.Contract.Constent.Enums;

namespace DemoTestProj
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Survey.DAL.DBModel.Survey, SurveyDTO>().ReverseMap();
            CreateMap<Survey.DAL.DBModel.Question, QuestionDTO>().ReverseMap();

            CreateMap<Survey.DAL.DBModel.Choice, ChoiceDTO>().ReverseMap();
            CreateMap<ChoiceDTO, Survey.DAL.DBModel.Choice>().ReverseMap();

            CreateMap<SurveyDTO, Survey.DAL.DBModel.Survey>().ReverseMap();
            CreateMap<QuestionDTO, Survey.DAL.DBModel.Question>().ReverseMap();

            CreateMap<SurveyResponsesDTO, SurveyResponses>();
            CreateMap<SurveyResponses, SurveyResponsesDTO>();

            CreateMap<QuestionResponse, QuestionResponseDTO>();
            CreateMap<QuestionResponseDTO, QuestionResponse>();

            CreateMap<SurveyAssignmentDTO, SurveyAssignment>()
            .ForMember(dest => dest.ID, opt => opt.Ignore()) 
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())); 

            // Reverse mapping from Entity to DTO
            CreateMap<SurveyAssignment, SurveyAssignmentDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<AssigneStatusType>(src.Status))); 
        }
    }
}   
