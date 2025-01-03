﻿using System.ComponentModel.DataAnnotations;

namespace infinitysky.Models
{
    public class Cliente
    {
        //CRIANDO O ENCAPSULAMENTO DO OBJETO COM GET E SET
        // encapsulamento = realiza a passsagem de dados de forma mais simples 
        public int Codigo { get; set; }

        public string? Nome { get; set; }

        public string? Telefone { get; set; }

        public string? Data_Nascimento { get; set; }

        

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public DateTime? DataNascimento { get; set; }


        public string? Cpf_Cliente { get; set; }

        public string? Email { get; set; }

        public string? Senha { get; set; }

        // array para listar clientes 
        public List<Cliente>? ListaCliente { get; set; }
    }
}
