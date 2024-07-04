using AutoMapper;
using Dominio.Dtos.ProdutoDto;
using Dominio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Profiles.ProdutoProfile
{
    public class ProdutoProfile : Profile
    {
        public ProdutoProfile()
        {
            CreateMap<Produto, ProdutoRequest>().ReverseMap();
            CreateMap<Produto, ProdutoResponse>().ReverseMap();
            CreateMap<Produto, ProdutoResponsePorCategoria>().ForMember(p => p.NomeCategoria, opt => opt.MapFrom(c => c.Categoria.Nome)).ReverseMap();
            CreateMap<Produto, ProdutoUpdateRequest>().ReverseMap();
        }
    }
}
