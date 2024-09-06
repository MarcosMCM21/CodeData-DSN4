using CodeData_Connection.Models.Database.Entidade;
using MySql.Data.MySqlClient;

namespace CodeData_Connection.Models.Database.CRUD
{
    public class ComandosNotificacao
    {
        private readonly string connectionString = "Server=localhost;Database=codedata;Uid=root;Pwd=R00tAce$$";

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
                    reader.GetDateTime("datahora"));

                lista.Add(notificacoes); //Adicionar o produto na lista
            }

            connection.Close();

            Console.WriteLine(lista);

            return lista;
        }
    }
}
