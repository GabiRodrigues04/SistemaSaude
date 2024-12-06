namespace SistemaSaude.Models
{
    public class Consulta
    {
        public int IdConsulta { get; set; }
        public int IdPaciente { get; set; }
        public DateTime DataConsulta { get; set; }
        public string Observacoes { get; set; }
        public string NomePaciente { get; set; }
    }
}
