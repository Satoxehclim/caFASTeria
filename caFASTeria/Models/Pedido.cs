using System;
using System.Collections.Generic;

namespace caFASTeria.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public int Producto { get; set; }

    public int Cantidad { get; set; }

    public int Comprador { get; set; }

    public int Estado { get; set; }

    public virtual Cuentum CompradorNavigation { get; set; } = null!;

    public virtual Producto ProductoNavigation { get; set; } = null!;
}
