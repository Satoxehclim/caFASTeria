using System;
using System.Collections.Generic;

namespace caFASTeria.Models;

public partial class TipoCuentum
{
    public int IdTipoCuenta { get; set; }

    public string? Tipo { get; set; }

    public virtual ICollection<Cuentum> Cuenta { get; set; } = new List<Cuentum>();
}
