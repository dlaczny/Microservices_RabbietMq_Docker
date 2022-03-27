using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Models
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<VisitCreateDto, Visit>();
        }
    }
}