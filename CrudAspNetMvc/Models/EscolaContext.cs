using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//para poder herdar da classe DbContext
using System.Data.Entity; 

namespace CrudAspNetMvc.Models
{
    /*
    A classe "Aluno" representa a entidade, o que será persistido em uma tabela no banco de dados
    A classe "DbContext", que é padrão do Entity Framework, que vai dar o acesso ao Banco de dados, irá representar o banco de dados dentro da aplicação.
    No banco de dados tem o conjunto de tabelas, no DbContext tem coleções de classes de entidades 
     
    Server Explorer > pasta Model > botão direito add class EscolaContext.cs. Essa classe herda da DbContext
    */
    public class EscolaContext : DbContext
    {
        //construtor da classe EscolaContext chamando o constutor padrão da DbContext, passando como parâmetro o nome da string de conexão
        public EscolaContext() : base("Escola")
        {
            //cria a base caso não exista
            Database.CreateIfNotExists();
        }

        /*
        coleção de objetos que irá representar a tabela no banco de dados, DbSet tipada com a classe "Aluno"
        DbSet é uma classe do Entity Framework representa uma coleção de entidades que serão persistidas no banco de dados na forma de uma tabela
        */
        public DbSet<Aluno> Alunos { get; set; }
    }
}