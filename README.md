# 🧱 Estrutura de Pastas — FinanblueBackend

Este documento descreve a estrutura do projeto **FinanblueBackend**, desenvolvido em **.NET**, com uso do **Dapper** para acesso a banco de dados e organização modular em camadas.

---

## 📂 Estrutura Geral

```text
FinanblueBackend/
├── Controllers/
│ ├── TesteController.cs
│ └── UserController.cs
│
├── Data/
│ └── DbContextDapper.cs
│
├── Models/
│ └── Usuario.cs
│
├── Pages/
│
├── Properties/
│ ├── launchSettings.json
│ ├── serviceDependencies.json
│ └── serviceDependencies.local.json
│
├── wwwroot/
│ ├── css/
│ ├── js/
│ ├── lib/
│ └── favicon.ico
│
├── .gitignore
├── FinanblueBackend.csproj
├── Program.cs
├── README.md
└── appsettings.json


---

## 🧩 Descrição das Pastas

### **Controllers/**
Contém os **controladores da API**, responsáveis por gerenciar as rotas e requisições HTTP.

- **TesteController.cs** – Usado para testar conexão e endpoints básicos.  
- **UserController.cs** – Controla as operações relacionadas a usuários (CRUD, autenticação, etc.).

---

### **Data/**
Camada de **acesso a dados**, responsável pela comunicação direta com o banco de dados.

- **DbContextDapper.cs** – Implementa a conexão e comandos SQL usando **Dapper**, garantindo performance e simplicidade.

---

### **Models/**
Contém as **entidades e modelos de dados** usados no sistema.

- **Usuario.cs** – Representa o modelo de usuário, com propriedades como `Id`, `Nome`, `Email`, `Senha`, `DataCriacao`, etc.

---

### **Pages/**
Pasta reservada para **páginas Razor** (caso o projeto utilize).  
No momento, pode estar vazia se o backend estiver sendo usado apenas como **API REST**.

---

### **Properties/**
Contém arquivos de configuração de execução do projeto.

- `launchSettings.json` – Define perfis de execução e portas para desenvolvimento.  
- `serviceDependencies.json` – Armazena dependências de serviços externas.  
- `serviceDependencies.local.json` – Versão local do arquivo acima.

---

### **wwwroot/**
Pasta pública do projeto — armazena arquivos **estáticos**, como:

- **css/** – Estilos front-end.  
- **js/** – Scripts JavaScript.  
- **lib/** – Bibliotecas externas.  
- **favicon.ico** – Ícone do projeto.

---

### **Arquivos de Raiz**
- **Program.cs** → Ponto de entrada da aplicação.  
- **appsettings.json** → Configurações do aplicativo (como conexão com banco, logging, etc.).  
- **FinanblueBackend.csproj** → Arquivo de projeto do .NET (metadados, dependências).  
- **.gitignore** → Define arquivos e pastas que não serão versionados.  
- **README.md** → Documentação geral do projeto.

---

## ⚙️ Fluxo Geral da Aplicação

1. O usuário faz uma requisição à API (ex: `/api/users`).
2. O **Controller** recebe a requisição e chama o serviço de dados.
3. O **DbContextDapper** executa consultas SQL diretamente via Dapper.
4. Os resultados são mapeados para os **Models** e retornados ao cliente.
5. Arquivos estáticos (JS, CSS) são servidos pela pasta **wwwroot**, se necessário.

---

## 🧠 Tecnologias Utilizadas

- **.NET 8 / C#**
- **Dapper ORM**
- **SQL Server**
- **ASP.NET Core Web API**
- **Swagger (se habilitado)**
- **Visual Studio / VS Code**

---

## 📄 Exemplo de Execução

Para rodar localmente:

```bash
dotnet restore
dotnet build
dotnet run

---

