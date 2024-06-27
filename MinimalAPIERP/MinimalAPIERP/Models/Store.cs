using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ERP
{
    public partial class Store
    {
        [Key]
        public int StoreId { get; set; }

        public string? Name { get; set; }

        public string Code { get; set; } // Agregar propiedad Code para almacenar el código aleatorio

        [InverseProperty("Store")]
        public virtual ICollection<Raincheck> Rainchecks { get; set; } = new List<Raincheck>();
    }
}
