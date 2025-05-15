public class SgfinCartera
{
    public long Id { get; set; }

    public int? IdBanco { get; set; }

    public string Numero { get; set; } = null!;

    public double Importe { get; set; }

    public DateTime FechaEmision { get; set; }

    public DateTime FechaCobro { get; set; }

    public string? AnombreDe { get; set; }

    public string? ImagenScaneo { get; set; }

    public string? Estado { get; set; }

    public int? EstadoRechazado { get; set; }

    public string? Observaciones { get; set; }

    public int? IdProveedorIngreso { get; set; }

    public int? IdProveedorEgreso { get; set; }

    public DateTime? FechaIngreso { get; set; }

    public DateTime? FechaSalida { get; set; }

    public int? TipoHoras { get; set; }

    public int? Caja { get; set; }
}
