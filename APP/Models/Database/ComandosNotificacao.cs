using CodeData_Connection.Models.Database.Entidade;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace CodeData_Connection.Models.Database
{
    public class ComandosNotificacao
    {
        private readonly string connectionString = new ConexaoDB().GetConnectionString();
        public List<Notificacao> GetAll(string UserID) 
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM notificacao WHERE UserID = @id;", connection);
            sql.Parameters.AddWithValue("@id", UserID);

            List<Notificacao> lista = new List<Notificacao>(); //Criar uma lista para receber os produtos cadastrados no banco de dados

            connection.Open();

            MySqlDataReader reader = sql.ExecuteReader(); //Criar um objeto do MySqlDataReader para receber os dados

            while (reader.Read()) //Percorrer cada resultado do select
            {
                Notificacao notificacoes = new Notificacao(
                    reader.GetInt32("id"),
                    reader.GetString("titulo"),
                    reader.GetString("mensagem"),
                    reader.GetBoolean("visualizado"),
                    reader.GetDateTime("datahora"));

                lista.Add(notificacoes); //Adicionar o produto na lista
            }

            connection.Close();

            return lista;
        }
    }
}
