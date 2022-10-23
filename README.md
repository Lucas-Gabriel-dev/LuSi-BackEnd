# LuSi-BackEnd
![version]( https://img.shields.io/badge/version-1.0.0-Green)

## Sobre
LuSi é uma plataforma online ToDo, que te auxilia no gerenciamento de tarefas! É uma aplicação leve e bastante intuitiva, facilitando no uso de qualquer
que seja sua função
<br>

# Ferramentas Utilizadas
Logo abaixo, irei descrever quais foram as ferramentas utilizadas para o desenvolvimento do BackEnd.<br>

## .NET 6
Para todo o desenvolivmento foi o utilizado o .NET 6, que oferece diversos recursos e pacotes para o desenvolvimento. Os pacotes utilizados no desenvolvimento
foram: <br> 

<b> Microsoft.AspNet.WebApi.Cors: </b> Foi o principal recurso utilizado no desenvolvimento, uma vez que o BackEnd se comunica com o Front em API's. <br>

<b> Microsoft.EntityFrameworkCore.Design: </b> Foi o ORM utilizado para realizar consultar, atualizar, apagar e enviar dados para o banco de dados. <br>  

<b> BCrypt.Net-Core: </b> Utilizado para criptografia de senhas, para armazená-las com maior segurança <br>

<b> Microsoft.AspNetCore.Authentication </b> e <b> Microsoft.AspNetCore.Authentication.JwtBearer </b>: Esses dois pacotes servem de complemento um para
o outro, com os recursos oferecido por ambos foi possível realizar a autenticação de um usuário na aplicação.<br>

<b> Npgsql.EntityFrameworkCore.PostgreSQL: </b> Pacote que foi utilizado para o EntityFramework poder se comunicar com o banco de dados PostgreSQL.
<br>
<br>

## C#
Linguagem de programação que foi utilizada para o desenvimento. O C# oferece diversos recursos que facilitam na hora do desenvolimento.
<br>
<br>

## PostgreSQL
Banco de dados que foi utilizado para armazenar os dados da aplicação.
<br>
<br>

# Rotas da aplicação
A aplicação oferece diversas rotas, onde na maioria é necessário estar autenticado para utilizar.<br>

## GET

### User Notifications - https://localhost:7288/lusi/UserNotifications
Com essa rota você pode consultar quais foram as últimas noticações do usuário autenticado.
<br>
<br>

### Get Details Task - https://localhost:7288/Lusi/{id}
Com essa rota você pode consultar os datalhes de uma tarefa específica.
<br>
<br>

### Get All Tasks User - https://localhost:7288/Lusi/AllTask
Com essa rota você pode consultar todas as tarefas que o usuário autenticado possui.
<br>
<br>

## POST

### Add Option Task - https://localhost:7288/Lusi/addtaskoption?idTask={id}
Nesta rota você adiciona novas opções em uma tarefa. A tarefa é identificada pelo ID passado pelo parametro.<br>
JSON:<br>
[<br>
   "opção 1",<br>
   "opção 2"<br>
]
<br>
<br>

### Add Task  - https://localhost:7288/lusi/addtask
Nesta rota você pode adicionar uma nova tarefa. <br>
JSON: <br>
{ <br>
  "title": "TesteAgora", <br>
  "description": "Livros", <br>
	"deadLine": "2022-10-30", // Prazo para concluir tarefa <br>
  "taskOptions": [ // Opções da tarefa <br>
    { <br>
      "name": "aa" <br>
    }, <br>
    { <br>
      "name": "Teste2" <br>
    } <br>
  ] <br>
} 
<br>
<br>

### Create User - https://localhost:7288/user/CreateUser
Com essa rota você pode criar um novo usuário.<br>
JSON: <br>
{<br>
    "name": "nome", <br>
    "email": "lucas@teste.com", <br>
    "password": "senha123" <br>
}
<br>
<br>

## PATCH
### Edit Info Task - https://localhost:7288/lusi/edittask
Como essa rota você pode atualizar informações sobre akguma tarefa, basta passa o ID da tarefa que desejar e o que deseja mudar. <br>
JSON: <br>
{ <br>
  "id": 3, <br>
  "title": "Novo Titulo", <br>
  "description": "Nova descrição" <br>
}
<br>
<br>

### Update Option Task - https://localhost:7288/lusi/EditOptionsTask
Com essa rota você pode atualizar as opções de uma tarefa. <br>
JSON: <br>
[ <br>
  { <br>
    "id": 37, <br>
    "name": "teste", <br>
    "currentTaskId": 3, //Id da tarefa que deseja atualizar as opções <br>
    "complete": false <br>
  } <br>
 ]
 <br>
<br>
 
 ## DELETE
 ### Delete Task - https://localhost:7288/Lusi/DeleteTask/{id}
 Com essa rota você pode deletar alguma tarefa, basta passar o ID como parâmetro.
 <br>
 <br>
 
<br>
<h4 align="center">
✅  ToDo LuSi 🚀 Concluído!!!  ✅
</h4>










