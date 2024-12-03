using infinitysky.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace infinitysky.Repository
{
    public class PaisRepositorio : IPaisRepositorio
    {
        private readonly string _conexaoMySQL;

        public PaisRepositorio(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }

        public Pais ObterPais(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM pais_tbl WHERE id_pais=@IdPais", conexao);
                cmd.Parameters.Add("@IdPais", MySqlDbType.Int64).Value = Id;

                MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                Pais pais = new Pais();

                if (dr.Read())
                {
                    pais.IdPais = Convert.ToInt32(dr["id_pais"]);

                    pais.NomePais = (string)dr["nome_pais"];
                    pais.ClimaPais = (string)dr["clima_pais"];
                    pais.ComidasTip = (string)dr["comidas_tip"];
                    pais.MoedaPais = (string)dr["moeda_pais"];
                    pais.DescricaoPais = (string)dr["descricao_pais"];
                    pais.image_pais = (string)dr["image_pais"];
                    pais.image_clima = (string)dr["image_clima"];
                    pais.image_comida = (string)dr["image_comida"];
                    pais.image_moeda = (string)dr["image_moeda"];

                }
                return pais;
            }
        }

        public IEnumerable<Pais> ObterTodosPaises()
        {
            List<Pais> paises = new List<Pais>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Pais_tbl;", conexao);

                MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                sd.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    paises.Add(
                        new Pais
                        {
                            IdPais = Convert.ToInt32(dr["id_pais"]),
                            NomePais = dr["nome_pais"].ToString()
                        });
                }
                return paises;
            }
        }
    }
}
