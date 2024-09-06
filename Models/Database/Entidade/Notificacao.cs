namespace CodeData_Connection.Models.Database.Entidade
{
    public class Notificacao
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataHora { get; set; }

        public Notificacao(int id, string titulo, string mensagem, DateTime dataHora)
        {
            Id = id;
            Titulo = titulo;
            Mensagem = mensagem;
            DataHora = dataHora;
        }
    }
}