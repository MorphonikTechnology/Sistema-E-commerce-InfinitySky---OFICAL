using infinitysky.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Numerics;

namespace infinitysky.Repository
{
    public class PlanosRepositorio : IPlanosRepositorio
    {
        private readonly string _conexaoMySQL;

        public PlanosRepositorio(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }

        // Método para adicionar (área adm)
        public void Adicionar(Planos plano)
        {

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();


                if (plano.ImagemFile != null && plano.ImagemFile.Length > 0)
                {
                    //Adicionando o caminho correto na variável 
                    var pastaImagens = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Imagens");

                    if (!Directory.Exists(pastaImagens))
                    {
                        Directory.CreateDirectory(pastaImagens);
                    }

                    // Gerar um nome único para cada imagem
                    var nomeArquivo = $"{Guid.NewGuid()}_{plano.ImagemFile.FileName}";
                    var caminhoDaImagem = Path.Combine(pastaImagens, nomeArquivo);

                    // Salvar a imagem na pasta
                    using (var stream = new FileStream(caminhoDaImagem, FileMode.Create))
                    {
                        plano.ImagemFile.CopyTo(stream);
                    }

                    // Quando for inserir colocar /Imagens na frente ai vai para a pasta imagens + nome aleatório 
                    plano.image_plano = Path.Combine("/Imagens/", nomeArquivo).Replace("\\", "/");
                }

                MySqlCommand cmd = new MySqlCommand("INSERT INTO Plano_tbl (nome_plano, hospedagem_plano, curso_plano, instituicao_plano, periodo_plano, parcela_plano, qtd_plano, valor, image_plano, descricao_plano, id_pais) VALUES (@NomePlano, @HospedagemPlano, @CursoPlano, @InstituicaoPlano, @PeriodoPlano, @ParcelaPlano, @qtd_plano, @ValorPlano, @ImagemPlano, @DescricaoPlano, @IdPais)", conexao);

                cmd.Parameters.Add("@NomePlano", MySqlDbType.VarChar).Value = plano.Nome;
                cmd.Parameters.Add("@HospedagemPlano", MySqlDbType.VarChar).Value = plano.HospedagemPlano;
                cmd.Parameters.Add("@CursoPlano", MySqlDbType.VarChar).Value = plano.CursoPlano;
                cmd.Parameters.Add("@InstituicaoPlano", MySqlDbType.VarChar).Value = plano.InstituicaoPlano;
                cmd.Parameters.Add("@PeriodoPlano", MySqlDbType.VarChar).Value = plano.PeriodoPlano;
                cmd.Parameters.Add("@ParcelaPlano", MySqlDbType.VarChar).Value = plano.ParcelaPlano;
                cmd.Parameters.Add("@qtd_plano", MySqlDbType.VarChar).Value = plano.qtd_plano;
                cmd.Parameters.Add("@ValorPlano", MySqlDbType.Decimal).Value = plano.Valor;
                cmd.Parameters.Add("@ImagemPlano", MySqlDbType.VarChar).Value = plano.image_plano;
                cmd.Parameters.Add("@DescricaoPlano", MySqlDbType.VarChar).Value = plano.DescricaoPlano;
                cmd.Parameters.Add("@IdPais", MySqlDbType.Int32).Value = plano.IdPais;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        //Método que Atualiza o plano (área adm)
        public void Atualizar(Planos plano, string image_plano_atual)
        {
            // Lógica de atualização da imagem
            if (plano.ImagemFile != null && plano.ImagemFile.Length > 0)
            {
                // Salvar a nova imagem
                var pastaImagens = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Imagens");
                var nomeArquivo = $"{Guid.NewGuid()}_{plano.ImagemFile.FileName}";
                var caminhoDaImagem = Path.Combine(pastaImagens, nomeArquivo);

                using (var stream = new FileStream(caminhoDaImagem, FileMode.Create))
                {
                    plano.ImagemFile.CopyTo(stream);
                }

                plano.image_plano = Path.Combine("/Imagens/", nomeArquivo).Replace("\\", "/");
            }
            else
            {
                // Manter a imagem atual se nenhuma nova imagem foi enviada
                plano.image_plano = image_plano_atual;
            }

            // Atualização dos dados do plano no banco
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                string query = @"
       UPDATE Plano_tbl 
       SET nome_plano=@NomePlano, 
           hospedagem_plano=@HospedagemPlano, 
           curso_plano=@CursoPlano, 
           instituicao_plano=@InstituicaoPlano, 
           periodo_plano=@PeriodoPlano, 
           parcela_plano=@ParcelaPlano, 
           qtd_plano=@qtd_plano, 
           valor=@ValorPlano, 
           descricao_plano=@DescricaoPlano, 
           id_pais=@IdPais";

                // Se a imagem foi alterada, atualiza também a imagem
                if (plano.ImagemFile != null && plano.ImagemFile.Length > 0)
                {
                    query += ", image_plano=@ImagemPlano";
                }

                query += " WHERE id_plano=@IdPlano";

                var cmd = new MySqlCommand(query, conexao);
                cmd.Parameters.AddWithValue("@IdPlano", plano.IdPlano);
                cmd.Parameters.AddWithValue("@NomePlano", plano.Nome);
                cmd.Parameters.AddWithValue("@HospedagemPlano", plano.HospedagemPlano);
                cmd.Parameters.AddWithValue("@CursoPlano", plano.CursoPlano);
                cmd.Parameters.AddWithValue("@InstituicaoPlano", plano.InstituicaoPlano);
                cmd.Parameters.AddWithValue("@PeriodoPlano", plano.PeriodoPlano);
                cmd.Parameters.AddWithValue("@ParcelaPlano", plano.ParcelaPlano);
                cmd.Parameters.AddWithValue("@qtd_plano", plano.qtd_plano);
                cmd.Parameters.AddWithValue("@ValorPlano", plano.Valor);
                cmd.Parameters.AddWithValue("@DescricaoPlano", plano.DescricaoPlano);
                cmd.Parameters.AddWithValue("@IdPais", plano.IdPais);

                // Somente adicionar o parâmetro de imagem se houve uma nova imagem
                if (plano.ImagemFile != null && plano.ImagemFile.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@ImagemPlano", plano.image_plano);
                }

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        //Método que obtem todos os planos, um select
        public IEnumerable<Planos> ObterTodosPlanos()
        {
            List<Planos> planoList = new List<Planos>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM plano_tbl;", conexao);

                MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                sd.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    planoList.Add(
                        new Planos
                        {
                            IdPlano = Convert.ToInt64(dr["id_plano"]),
                            Nome = (string)dr["nome_plano"],
                            HospedagemPlano = (string)dr["hospedagem_plano"],
                            CursoPlano = (string)dr["curso_plano"],
                            InstituicaoPlano = (string)dr["instituicao_plano"],
                            PeriodoPlano = (string)dr["periodo_plano"],
                            DescricaoPlano = (string)dr["descricao_plano"],
                            image_plano = (string)dr["image_plano"],
                            Valor = Convert.ToDecimal(dr["valor"]),
                            IdPais = Convert.ToInt32(dr["id_pais"]),
                            ParcelaPlano = (string)dr["parcela_plano"],
                            qtd_plano = Convert.ToInt16(dr["qtd_plano"])
                        });
                }
                return planoList;
            }
        }

        //Método que puxa os 3 primeiros planos na index
        public List<Planos> ObterPlanosAleatorios(int IdPlano)
        {
            List<Planos> planoList = new List<Planos>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                // Chama a procedure
                MySqlCommand cmd = new MySqlCommand("SelecionarTresPrimeirosPlanos", conexao);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPlano", IdPlano);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    planoList.Add(new Planos
                    {
                        IdPlano = Convert.ToInt64(dr["id_plano"]),
                        Nome = dr["nome_plano"].ToString(),
                        HospedagemPlano = dr["hospedagem_plano"].ToString(),
                        CursoPlano = dr["curso_plano"].ToString(),
                        InstituicaoPlano = dr["instituicao_plano"].ToString(),
                        PeriodoPlano = dr["periodo_plano"].ToString(),
                        DescricaoPlano = dr["descricao_plano"].ToString(),
                        image_plano = dr["image_plano"].ToString(),
                        Valor = Convert.ToDecimal(dr["valor"]),
                        IdPais = Convert.ToInt32(dr["id_pais"]),
                        ParcelaPlano = dr["parcela_plano"].ToString()
                    });
                }
                conexao.Close();
            }
            return planoList;
        }

        //Método que puxa os 6 planos depois dos 3 primeiros na index
        public List<Planos> ObterRestante(int IdPlano)
        {
            List<Planos> planoList = new List<Planos>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                // Chama a procedure
                MySqlCommand cmd = new MySqlCommand("SelecionarSeisPlanos", conexao);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPlano", IdPlano);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    planoList.Add(new Planos
                    {
                        IdPlano = Convert.ToInt64(dr["id_plano"]),
                        Nome = dr["nome_plano"].ToString(),
                        HospedagemPlano = dr["hospedagem_plano"].ToString(),
                        CursoPlano = dr["curso_plano"].ToString(),
                        InstituicaoPlano = dr["instituicao_plano"].ToString(),
                        PeriodoPlano = dr["periodo_plano"].ToString(),
                        DescricaoPlano = dr["descricao_plano"].ToString(),
                        image_plano = dr["image_plano"].ToString(),
                        Valor = Convert.ToDecimal(dr["valor"]),
                        IdPais = Convert.ToInt32(dr["id_pais"]),
                        ParcelaPlano = dr["parcela_plano"].ToString()
                    });
                }
                conexao.Close();
            }
            return planoList;
        }

       
        //Método que obtem o plano através do id
        public Planos ObterPlano(long Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM plano_tbl WHERE id_plano=@IdPlano", conexao);
                cmd.Parameters.Add("@IdPlano", MySqlDbType.Int64).Value = Id;

                MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                Planos plano = new Planos();

                if (dr.Read())
                {
                    plano.IdPlano = Convert.ToInt64(dr["id_plano"]);

                    plano.Nome = (string)dr["nome_plano"];
                    plano.HospedagemPlano = (string)dr["hospedagem_plano"];
                    plano.CursoPlano = (string)dr["curso_plano"];
                    plano.InstituicaoPlano = (string)dr["instituicao_plano"];
                    plano.PeriodoPlano = (string)dr["periodo_plano"];
                    plano.DescricaoPlano = (string)dr["descricao_plano"];
                    plano.image_plano = (string)dr["image_plano"];
                    plano.Valor = Convert.ToDecimal(dr["valor"]);
                    plano.IdPais = Convert.ToInt32(dr["id_pais"]);
                    plano.ParcelaPlano = (string)dr["parcela_plano"];
                    plano.qtd_plano = Convert.ToInt16(dr["qtd_plano"]);


                }
                return plano;
            }
        }

        //Método de pesquisa de planos pelo nome 
        public IEnumerable<Planos> PesquisaPlanos(string nome)
        {
            List<Planos> planoList = new List<Planos>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM plano_tbl WHERE curso_plano LIKE @Nome;", conexao);
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = "%" + nome + "%"; // Para busca parcial

                MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                sd.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    planoList.Add(
                        new Planos
                        {
                            IdPlano = Convert.ToInt64(dr["id_plano"]),
                            Nome = dr["nome_plano"].ToString(),
                            HospedagemPlano = dr["hospedagem_plano"].ToString(),
                            CursoPlano = dr["curso_plano"].ToString(),
                            InstituicaoPlano = dr["instituicao_plano"].ToString(),
                            PeriodoPlano = dr["periodo_plano"].ToString(),
                            DescricaoPlano = dr["descricao_plano"].ToString(),
                            image_plano = dr["image_plano"].ToString(),
                            Valor = Convert.ToDecimal(dr["valor"]),
                            IdPais = Convert.ToInt32(dr["id_pais"]),
                            ParcelaPlano = dr["parcela_plano"].ToString()

                        });
                }
                return planoList;
            }
        }

        // Pesquisar planos por nome do país ou nome do plano
        public IEnumerable<Planos> PesquisaPlanosPorNome(string searchTerm)
        {
            List<Planos> planoList = new List<Planos>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                // Consulta com JOIN para buscar planos pelo nome do país ou nome do plano
                MySqlCommand cmd = new MySqlCommand(@"
            SELECT p.* FROM Plano_tbl p
            JOIN Pais_tbl pa ON p.id_pais = pa.id_pais
            WHERE LOWER(pa.nome_pais) LIKE LOWER(@SearchTerm)
            OR LOWER(p.nome_plano) LIKE LOWER(@SearchTerm);", conexao);

                cmd.Parameters.Add("@SearchTerm", MySqlDbType.VarChar).Value = "%" + searchTerm + "%"; // Busca parcial

                MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                sd.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    planoList.Add(
                        new Planos
                        {
                            IdPlano = Convert.ToInt64(dr["id_plano"]),
                            Nome = dr["nome_plano"].ToString(),
                            HospedagemPlano = dr["hospedagem_plano"].ToString(),
                            CursoPlano = dr["curso_plano"].ToString(),
                            InstituicaoPlano = dr["instituicao_plano"].ToString(),
                            PeriodoPlano = dr["periodo_plano"].ToString(),
                            DescricaoPlano = dr["descricao_plano"].ToString(),
                            image_plano = dr["image_plano"].ToString(),
                            Valor = Convert.ToDecimal(dr["valor"]),
                            IdPais = Convert.ToInt32(dr["id_pais"]),
                            ParcelaPlano = dr["parcela_plano"].ToString()
                        });
                }
            }
            return planoList;
        }

        //Método que pega os planos pelo id do país
        public IEnumerable<Planos> ObterPlanosPorPaisId(int paisId)
        {
            List<Planos> planoList = new List<Planos>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                // Chama a procedure
                MySqlCommand cmd = new MySqlCommand("SelecionarPlanosPorPaisIdsO", conexao);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PaisId", paisId);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    planoList.Add(new Planos
                    {
                        IdPlano = Convert.ToInt64(dr["id_plano"]),
                        Nome = dr["nome_plano"].ToString(),
                        HospedagemPlano = dr["hospedagem_plano"].ToString(),
                        CursoPlano = dr["curso_plano"].ToString(),
                        InstituicaoPlano = dr["instituicao_plano"].ToString(),
                        PeriodoPlano = dr["periodo_plano"].ToString(),
                        DescricaoPlano = dr["descricao_plano"].ToString(),
                        image_plano = dr["image_plano"].ToString(),
                        Valor = Convert.ToDecimal(dr["valor"]),
                        IdPais = Convert.ToInt32(dr["id_pais"]),
                        ParcelaPlano = dr["parcela_plano"].ToString()
                    });
                }
                conexao.Close();
            }
            return planoList;
        }


        //Método que apaga os planos do banco (área adm)
        public void Apagar(long Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM plano_tbl WHERE id_plano = @IdPlano;", conexao);
                cmd.Parameters.Add("@IdPlano", MySqlDbType.Int64).Value = Id;

                cmd.ExecuteNonQuery();
            }
        }

        public Planos ObterPlanoPorIdCarrinho(int carrinhoId)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var query = @"SELECT p.* 
                  FROM Plano_tbl p
                  INNER JOIN Carrinho_tbl c ON p.id_plano = c.id_plano
                  WHERE c.id_carrinho = @CarrinhoId";

            using var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@CarrinhoId", carrinhoId);

            using var reader = comando.ExecuteReader();

            if (reader.Read())
            {
                return new Planos
                {
                    IdPlano = reader.GetInt32("id_plano"),
                    Nome = reader.GetString("nome_plano"),
                    DescricaoPlano = reader.GetString("descricao_plano"),
                    Valor = reader.GetDecimal("valor")
                };
            }

            return null;
        }

        public Planos ObterPlanoPorPagamento(int pagamentoId)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var query = @"
            SELECT *
            FROM Plano_tbl p
            INNER JOIN Carrinho_tbl c ON p.id_plano = c.id_plano
            INNER JOIN Pagamento_tbl pg ON c.id_carrinho = pg.id_carrinho
            WHERE pg.id_pagamento = @PagamentoId";

            using var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@PagamentoId", pagamentoId);

            using var reader = comando.ExecuteReader();

            if (reader.Read())
            {
                return new Planos
                {
                    IdPlano = reader.GetInt32("id_plano"),
                    Nome = reader.GetString("nome_plano"),
                    DescricaoPlano = reader.GetString("descricao_plano"),
                    Valor = reader.GetDecimal("valor"),
                    image_plano = reader.GetString("image_plano"),

                };
            }

            return null;
        }

    }
}