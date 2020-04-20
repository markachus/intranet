using AutoMapper;
using Intranet.API.Models;
using Intranet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.API
{
    public class IntranetMappingProfile : Profile
    {
        public IntranetMappingProfile()
        {
            CreateMap<Etiqueta, EtiquetaModel>()
                .ReverseMap();
        }
    }
}