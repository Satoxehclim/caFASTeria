using System;
using System.Collections.Generic;

namespace caFASTeria.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public double Precio { get; set; }

    public int Vendedor { get; set; }

    public int Foto { get; set; }

    public int Calificacion { get; set; }

    public virtual Foto FotoNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual Cuentum VendedorNavigation { get; set; } = null!;
}
