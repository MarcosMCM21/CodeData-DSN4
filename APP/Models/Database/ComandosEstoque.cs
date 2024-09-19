using CodeData_Connection.Models.Database.Entidade;
using MySql.Data.MySqlClient;

namespace CodeData_Connection.Models.Database
{
    public class ComandosEstoque
    {
        private readonly string connectionString = new ConexaoDB().GetConnectionString();
        public List<Equipamento> GetAll() 
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM equipamentos;", connection);

            List<Equipamento> lista = new List<Equipamento>(); //Criar uma lista para receber os produtos cadastrados no banco de dados

            connection.Open();

            MySqlDataReader reader = sql.ExecuteReader(); //Criar um objeto do MySqlDataReader para receber os dados

            while (reader.Read()) //Percorrer cada resultado do select
            {
                Equipamento equipamento = new Equipamento(
                    reader.GetInt32("id"),
                    reader.GetString("codigo"),
                    reader.GetString("modelo"),
                    reader.GetString("descricao"),
                    reader.GetString("marca"),
                    reader.GetString("serialNumber"),
                    reader.GetString("partNumber"),
                    reader.GetBoolean("condicao"),
                    reader.GetInt32("estoqueID"),
                    reader.GetInt32("documentoID"));

                lista.Add(equipamento); //Adicionar o produto na lista
            }

            connection.Close();

            return lista;
        }
    }
}
