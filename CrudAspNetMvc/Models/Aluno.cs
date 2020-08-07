using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//para definir as propriedades de cada campo usa a Data Annotations
using System.ComponentModel.DataAnnotations; //propriedades dos campos
using System.ComponentModel.DataAnnotations.Schema; //define o nome da tabela

namespace CrudAspNetMvc.Models
{
    /*
    File > New > Project
    Lado esquerdo: Installed > Templates > Visual C# > Web
    Escolher ASP.NET MVC 4 Web Application e depois Internet Application ou Empty
    Nesse projeto usei Basic, acho que é o mais ideal

    Instalação do Entity Framework, o qual o VS irá adicionar todas as references à dlls necessárias: 
    Solution Explorer > botão direito no projeto > Manage NuGet Packages > buscar por "Entity Framework" e fazer a instalação. 
    Ou Menu > View > Other Windows > Package manager console. Default project deve ser o projeto a ser instalado, digitar Install-Package EntityFramework 

    Server Explorer > pasta Model > botão direito add class Aluno.cs 
    O que for feita nessa classe será refletido no banco de dados pelo Entity Framework
    */

    //A Data Annotation "Table" define que essa classe será uma tabela no Banco de dados com o nome "Alunos"
    [Table("Alunos")]
    public class Aluno
    {
        //Id é usada pelo Entity Framework por padrão como chave primária da tabela, então não precisa da Data Annotation que seria "key"
        //[Key]
        public int Id { get; set; }

        //"Required" campo obrigatório, not null no banco de dados
        [Required]
        public string Nome { get; set; }

        //"Display" indica para o ASP.NET MVC, não o banco de dados, aparecer na tela com esse label (o texto dentro de Display)
        [Required]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        [Required]
        public string Telefone { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        /*
        "DisplayFormat" define a data no formato brasileiro, já que o Entity Framework assume o padrão americano
        "ApplyFormatInEditMode" indica que esse formato deve ser usado também na tela de edição dessa classe
        */
        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Nascimento { get; set; }

        [Required]
        public bool Ativo { get; set; }
    }
}