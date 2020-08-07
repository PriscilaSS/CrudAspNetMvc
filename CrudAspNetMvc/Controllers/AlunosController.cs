using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrudAspNetMvc.Models;

//para usar HttpStatusCode
using System.Net;

namespace CrudAspNetMvc.Controllers
{
    /*
    Server Explorer > pasta Controller > botão direito > Add Controller
    Escolher o template: MVC controller with read/write actions and views, using Entity Framework
    Definir a Model Class, no caso Aluno.cs (precisa dar um build no projeto para que possa encontrar a model class)
    Definir a Data context class, no caso EscolaContext.cs (a classe que herda da DbContext)
    Definir o Controller name para AlunosController (por padrão deixar o nome Controller)     
     
    O VS cria as actions, instancia a classe EscolaContext e cria a base no SQL Server
    No RouteConfig.cs está definido o caminho então ao executar colocar o nome do Controller sem a palavra Controller e acessará a página Index
    */
    public class AlunosController : Controller
    {
        private EscolaContext db = new EscolaContext();

        /*
        return View exibe a view com o mesmo nome do método ActionResult
        O VS retira o sufixo Controller e pega o nome Alunos (de AlunosController) e busca na pasta Views\Alunos\ o arquivo cshtml como o nome da ActionResult
        caso queira retornar uma View de nome diferente passar o nome dela como parâmetro return View("NovaView")
        */

        // GET: /Alunos/

        //public ActionResult Index()
        public ActionResult Index(string busca = null) //valor padrão null
        {
            if (busca != null)
            {
                /*
                usando empressão lambda, retorna da coleção DbSet Alunos aqueles que contem o texto buscado no nome e converte para lista (ToList())
                se a busca for null não entra no if e cai no próximo returb View que traz todos os Alunos da base de dados
                Para que a busca seja case-insensitive basta converter os dois lados da comparação para maiúsculo (ToUpper) ou minúsculo (ToLower)
                */
                return View(db.Alunos.Where(a => a.Nome.ToUpper().Contains(busca.ToUpper())).ToList());
            }

            //passa para a view Index uma lista de objetos para exibir, o DbSet definido dentro de db (instância de EscolaContext)
            return View(db.Alunos.ToList());
        }

        // POST: /Alunos/
        /*
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult IndexBusca(string busca = null)
        {
            if (busca != null)
            {
                return View(db.Alunos.Where(a => a.Nome.Contains(busca)).ToList());
            }
            return View(db.Alunos.ToList());
        }
        */


        // GET: /Alunos/Details/5

        //(int id = 0) ou (int? id), com ? é nullable type para aceitar valores nulos (caso não seja passado na URL)
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //se nulo, retorna um erro usando o código BadRequest do HTTP (requisição inválida)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Find filtra a coleção pela chave primária
            Aluno aluno = db.Alunos.Find(id);
            if (aluno == null)
            {
                //erro 404 not found se não encontrou algum registro com aquele id
                return HttpNotFound();
            }

            //passa para a view Details o objeto filtrado 
            return View(aluno);
        }


        /*
        A CRIAÇÃO / EDIÇÃO / REMOÇÃO de novos registros divide-se em duas partes. 
        Primeiro: o método Create / Edit / Delete do controller Alunos, que é invocado quando acessamos a URL /Alunos/(nome do método ActionResult) e retorna a view de mesmo nome
        */

        // GET: /Alunos/Create

        public ActionResult Create()
        {
            return View();
        }

        /*
        HttpPost faz com que esse método seja acionado quando for recebida uma requisição HTTP POST à URL /Alunos/Create. Ou seja, ele será acionado quando o form de inserção for submetido
        ValidateAntiForgeryToken valida o token criado no form dentro da view create 
        
        Bind define quais campos devem ser repassados para o objeto recebido (tipo Aluno), se tiver mais campos na tela que não forem listados no Bind serão desconsiderados
        Os valores separados por vírgula são o atributo "name" dos inputs do form, e o "name" é o mesmo nome da propriedade correspondente aquele input
        Isso impede que campos adicionais sejam incluídos no form de forma maliciosa para enviar dados extras para o back-end
          
        ModelState.IsValid é uma propriedade do controller que verifica se todas as validações da classe aluno, definidas por Data Annotations, foram atendidas (required, formato de dado, etc) 
        se IsValid for false o ModelState já contém automaticamente as mensagens de validação que podem ser passadas para o front-end
        */

        // POST: /Alunos/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(Aluno aluno)
        public ActionResult Create([Bind(Include = "Id,Nome,Endereco,Telefone,Email,Nascimento,Ativo")] Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                db.Alunos.Add(aluno); //adiciona o objeto Aluno recebido por parâmetro no DbSet do EscolaContext.cs
                db.SaveChanges();     //persiste os dados no banco
                return RedirectToAction("Index"); //redireciona para a action index
            }

            //se validação for false retorna para a tela de criação mas com os dados preenchidos, por isso passa o objeto aluno recebido por parâmetro para a view
            return View(aluno); 
        }


        // GET: /Alunos/Edit/5

        //(int id = 0) ou (int? id), com ? é nullable type para aceitar valores nulos (caso não seja passado na URL)
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Aluno aluno = db.Alunos.Find(id);
            if (aluno == null)
            {
                return HttpNotFound();
            }
            return View(aluno);
        }

        // POST: /Alunos/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit(Aluno aluno)
        public ActionResult Edit([Bind(Include = "Id,Nome,Endereco,Telefone,Email,Nascimento,Ativo")] Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                /*
                Método Entry do EntityFramework localiza o objeto aluno recebido na lista DbSet do EscolaContext.cs e muda o estado dele para alterado
                O objeto aluno recebido, tem seus campos alterados e já possui o Id, o método Entry aponta para aquele objeto na lista
                */
                db.Entry(aluno).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aluno);
        }


        // GET: /Alunos/Delete/5

        //(int id = 0) ou (int? id), com ? é nullable type para aceitar valores nulos (caso não seja passado na URL)
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);          
            }

            Aluno aluno = db.Alunos.Find(id);
            if (aluno == null)
            {
                return HttpNotFound();
            }
            return View(aluno);
        }

        // POST: /Alunos/Delete/5

        /*
        Não pode existir 2 métodos com o mesmo nome e recebimento de parâmetros, então no método acionado via POST mudamos o nome de Delete para DeleteConfirmed
        O atributo HttpPost faz com que esse método seja acionado quando for recebida uma requisição HTTP POST à URL /Alunos/Delete/id. Ou seja, ele será acionado quando o form de inserção for submetido. 
        O atributo ActionName faz com que esse método seja acessado a partir da URL Alunos/Delete/id, pois como seu nome é diferente de Delete, por padrão ele seria acessado pela URL Alunos/DeleteConfirmed/id
        */
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Aluno aluno = db.Alunos.Find(id); //busca o aluno no DbSet do EscolaContext.cs
            db.Alunos.Remove(aluno);          //remove o aluno
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        /*
        método Dispose que foi sobreescrito para quando o método Dispose do controller for chamado para liberar o controller da memória, também elimina a instância do DbContext
        o controller é removido da memória no final de uma requisição, ou seja, após chamar o form submeter e retornar
        ao fazer uma nova requisição o controller é instanciado novamente
        */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //elimina da memória a instância do DbContext
                db.Dispose();    
            }            
            base.Dispose(disposing);
        }
    }
}