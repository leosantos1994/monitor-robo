

namespace SimuladorVotos.Models
{
    public class VotanteExportar
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string NomeMae { get; set; }
        public string DataNascimento { get; set; }
        public StatusVotante Status { get; set; }
        public int Regional { get; set; }
        public TipoVoto TipoVoto { get; set; }
    }
}