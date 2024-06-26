using System;
using System.Collections.Generic;

namespace caFASTeria.Models;

public partial class Foto
{
    public int Idfoto { get; set; }

    public string? Direcion { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
