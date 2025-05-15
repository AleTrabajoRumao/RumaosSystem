public class SgfinIngresoCaja
{
    public long Id { get; set; }

    public int IdArqueo { get; set; }

    public DateTime Fecha { get; set; }

    public string? Descripcion { get; set; }

    public double? Importe { get; set; }

    public int? IdDepartamento { get; set; }

    public int? IdReferencia { get; set; }

    public int? IdProveedor { get; set; }

    public int? IdRendicion { get; set; }

    public int? CentroCosto { get; set; }

    public bool? Activo { get; set; }
}
