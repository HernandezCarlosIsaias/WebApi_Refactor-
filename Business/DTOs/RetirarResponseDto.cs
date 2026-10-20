public class RetirarResponseDto
{
    public int id { get; set; }
    public string patente { get; set; } = string.Empty;
    public string marca { get; set; } = string.Empty;
    public string tipo { get; set; } = string.Empty;
    public bool retirado { get; set; }
}
