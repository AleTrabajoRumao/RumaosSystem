public class ChequeDestino
{

    public int Id { get; set; }

    public string Uen { get; set; } = null!;

    public int? Ptovta { get; set; }

    public int? Nrorecibo { get; set; }

    public string Banco { get; set; } = null!;

    public DateTime? Fechavto { get; set; }

    public int? Nrocheque { get; set; }

    public double? Importe { get; set; }

}
