namespace Parcial.Entities;

public class Vehiculo
{
    private int id;
    private string patente = string.Empty;
    private string marca = string.Empty;
    private string tipo = string.Empty;
    private bool retirado = false;

    public int Id
    {
        get {return this.id;}
        set {this.id = value;}
    }

    public string Patente
    {
        get {return this.patente;}
        set {this.patente = value;}
    }

        public string Marca
    {
        get {return this.marca;}
        set {this.marca = value;}
    }

        public string Tipo
    {
        get {return this.tipo;}
        set {this.tipo = value;}
    }

        public bool Retirado
    {
        get {return this.retirado;}
        set {this.retirado = value;}
    }

}
