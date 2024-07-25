using AutoMapper;
using Dominio.Interfaces;
using Infraestrutura.Data;
using Infraestrutura.Profiles.ProdutoProfile;
using Infraestrutura.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteCatalogoApi.Testes
{
    public class ProdutosUnitTestController
    {
        public IUnitToWork _unitToWork;
        public IMapper _mapper;
        public static DbContextOptions<ApiDbContext> _dbContextOptions { get; }

        public static string connectionString = "Data Source=DESKTOP-13IIORA\\SQLEXPRESS;Initial Catalog=CatalogoAPI;Integrated Security=True;TrustServerCertificate=True;";

        static ProdutosUnitTestController()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApiDbContext>().UseSqlServer(connectionString).Options;
        }

        public ProdutosUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProdutoProfile());
            });

            _mapper = config.CreateMapper();
            var context = new ApiDbContext(_dbContextOptions);
            _unitToWork = new UnitToWork(context);
        }
    }
}
