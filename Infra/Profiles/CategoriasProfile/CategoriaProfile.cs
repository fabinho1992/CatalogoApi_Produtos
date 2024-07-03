using AutoMapper;
using Dominio.Dtos.CategoriaDto;
using Dominio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Profiles.CategoriasProfile
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile() 
        {
            CreateMap<Categoria, CategoriaRequest>().ReverseMap();
            CreateMap<Categoria, CategoriaResponse>().ForMember(c => c.Produtos, opt => opt.MapFrom(c => c.Produtos)).ReverseMap();
        }
    }
}
