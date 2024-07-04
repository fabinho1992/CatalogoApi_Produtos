using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Dtos.ProdutoDto
{
    public class ProdutoUpdateRequest /*: IValidatableObject*/
    {
        [Range(1, 9999, ErrorMessage = "Valor minímo 1 e máximo 9999")]
        public float Estoque { get; set; }
        //public DateTime DataCadastro { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (DataCadastro.Date < DateTime.Now.Date)
        //    {
        //        yield return new ValidationResult("A data tem que ser maior que a data atual.");
        //    }
        //}
    }
}
