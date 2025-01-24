using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;


namespace RepositoryAPI.Models.OrdersDTO
{
    public class CreateOrderWithImagesDto
    {
        public OrderDTO Order { get; set; }

        [SwaggerSchema("Upload one or more images.", Format = "binary")]
        public List<IFormFile> Images { get; set; }

    }
}
