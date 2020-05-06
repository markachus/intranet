using AutoMapper;
using IntranetCore.Data.Models;
using IntranetCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetCore.Api
{
    public class IntranetMappingProfile : Profile
    {
        public IntranetMappingProfile()
        {
            CreateMap<Etiqueta, EtiquetaModel>()
                .ReverseMap();

            CreateMap<EntradaEtiqueta, EtiquetaModel>()
                .ForMember(e => e.Nombre, m => m.MapFrom(p => p.Etiqueta.Nombre))
                .ForMember(e => e.HexColor, m => m.MapFrom(p => p.Etiqueta.HexColor))
                .ForMember(e => e.FechaUltimaModificacion, m => m.MapFrom(p => p.Etiqueta.FechaUltimaModificacion));

            CreateMap<EtiquetaForCreationModel, Etiqueta>();
            CreateMap<EtiquetaForUpdateModel, Etiqueta>();

            CreateMap<Entrada, EntradaModel>().
                ReverseMap().
                ForMember(m => m.Etiquetas, opt => opt.Ignore());

            CreateMap<Entrada, EntradaModel>().
                ReverseMap().
                ForMember(m => m.Etiquetas, opt => opt.Ignore());

            CreateMap<EntradaForCreationModel, Entrada>().
                ForMember(m => m.Etiquetas, opt => opt.Ignore());


        }
    }
}