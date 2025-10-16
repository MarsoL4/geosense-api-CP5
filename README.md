# GeoSense API

GeoSense API é uma solução RESTful desenvolvida em .NET para o gerenciamento de motos, vagas, pátios e usuários em ambientes de manutenção e estacionamento. O projeto utiliza arquitetura em camadas, Entity Framework Core, Oracle como banco de dados e documentação completa via Swagger/OpenAPI.

---

## 👥 Integrantes

- **Enzo Giuseppe Marsola** – RM: 556310, Turma: 2TDSPK  
- **Rafael de Souza Pinto** – RM: 555130, Turma: 2TDSPY  
- **Luiz Paulo F. Fernandes** – RM: 555497, Turma: 2TDSPF

---

## 🏗 Justificativa do Domínio e Arquitetura

O domínio foi escolhido para atender à necessidade de controle eficiente do fluxo de motos em pátios de manutenção, oficinas ou estacionamentos. O sistema permite cadastro, alocação e histórico de motos, gestão de vagas, controle de usuários com diferentes permissões.

A arquitetura segue boas práticas REST, separação de responsabilidades (camadas Controller, Service, Repository), e utiliza recursos avançados como paginação, HATEOAS, DTOs e exemplos interativos no Swagger.

---

## 🚀 Instruções de Execução

1. **Clonar o Repositório:**
   ```bash
   git clone https://github.com/MarsoL4/geosense-api.git
   cd geosense-api
   ```

2. **Configurar o Banco de Dados:**  
   Edite o arquivo `GeoSense.API/appsettings.json` com sua string de conexão Oracle em `"ConnectionStrings:Oracle"`.

3. **Restaurar os Pacotes e Compilar:**  
   ```bash
   dotnet restore
   dotnet build
   ```

4. **Executar a API:**  
   ```bash
   dotnet run --project GeoSense.API
   ```
   Acesse a documentação Swagger em:  
   `http://localhost:5194/swagger` ou `https://localhost:7150/swagger`

5. **Rodar Testes Automatizados:**  
   ```bash
   dotnet test
   ```

---

## 🔑 Principais Entidades

- **Moto:** Controle de motos cadastradas, informações de placa, chassi, modelo e vaga alocada.
- **Vaga:** Gerenciamento de vagas disponíveis em pátios, incluindo status e tipo.
- **Usuário:** Cadastro de usuários do sistema, com controle de papéis (administrador, mecânico) e autenticação.
- **Pátio:** Cadastro e gestão dos pátios onde as vagas são distribuídas.

---

## 📑 Endpoints e Exemplos de Uso

### 🛵 MotoController

#### Listar Motos (Paginação + HATEOAS)
- **GET** `/api/moto?page=1&pageSize=10`
- **Resposta:**
    ```json
    {
      "items": [
        {
          "id": 1,
          "modelo": "Honda CG 160",
          "placa": "ABC1D23",
          "chassi": "9C2JC4110JR000001",
          "problemaIdentificado": "Motor com ruído excessivo",
          "vagaId": 1
        }
      ],
      "totalCount": 1,
      "page": 1,
      "pageSize": 10,
      "links": [
        { "rel": "self", "method": "GET", "href": "/api/moto?page=1&pageSize=10" }
      ]
    }
    ```

#### Buscar Moto por ID
- **GET** `/api/moto/{id}`
- **Resposta:**
    ```json
    {
      "id": 1,
      "modelo": "Honda CG 160",
      "placa": "ABC1D23",
      "chassi": "9C2JC4110JR000001",
      "problemaIdentificado": "Motor com ruído excessivo",
      "vagaId": 1
    }
    ```

#### Criar Moto
- **POST** `/api/moto`
- **Payload de exemplo:**
    ```json
    {
      "modelo": "Honda CG 160",
      "placa": "ABC1D23",
      "chassi": "9C2JC4110JR000001",
      "problemaIdentificado": "Motor com ruído excessivo",
      "vagaId": 1
    }
    ```
- **Resposta:**
    ```json
    {
      "mensagem": "Moto cadastrada com sucesso.",
      "dados": {
        "id": 1,
        "modelo": "Honda CG 160",
        "placa": "ABC1D23",
        "chassi": "9C2JC4110JR000001",
        "problemaIdentificado": "Motor com ruído excessivo",
        "vagaId": 1
      }
    }
    ```

#### Atualizar Moto
- **PUT** `/api/moto/{id}`
- **Payload igual ao POST**
- **Resposta:**
    ```json
    {
      "mensagem": "Moto atualizada com sucesso.",
      "dados": {
        "id": 1,
        "modelo": "Honda CG 160",
        "placa": "ABC1D23",
        "chassi": "9C2JC4110JR000001",
        "problemaIdentificado": "Motor com ruído excessivo",
        "vagaId": 1
      }
    }
    ```

#### Remover Moto
- **DELETE** `/api/moto/{id}`
- **Resposta:**
    ```json
    {
      "mensagem": "Moto deletada com sucesso."
    }
    ```

---

### 🅿️ VagaController

#### Listar Vagas (Paginação + HATEOAS)
- **GET** `/api/vaga?page=1&pageSize=10`
- **Resposta:**
    ```json
    {
      "items": [
        {
          "id": 1,
          "numero": 101,
          "tipo": 0,
          "status": 0,
          "patioId": 1,
          "motoId": 2
        }
      ],
      "totalCount": 1,
      "page": 1,
      "pageSize": 10,
      "links": [
        { "rel": "self", "method": "GET", "href": "/api/vaga?page=1&pageSize=10" }
      ]
    }
    ```

#### Buscar Vaga por ID
- **GET** `/api/vaga/{id}`
- **Resposta:**
    ```json
    {
      "id": 1,
      "numero": 101,
      "tipo": 0,
      "status": 0,
      "patioId": 1,
      "motoId": 2
    }
    ```

#### Criar Vaga
- **POST** `/api/vaga`
- **Payload de exemplo:**
    ```json
    {
      "numero": 101,
      "tipo": 0,
      "status": 0,
      "patioId": 1
    }
    ```
- **Resposta:**
    ```json
    {
      "mensagem": "Vaga cadastrada com sucesso.",
      "dados": {
        "id": 1,
        "numero": 101,
        "tipo": 0,
        "status": 0,
        "patioId": 1,
        "motoId": null
      }
    }
    ```

#### Atualizar Vaga
- **PUT** `/api/vaga/{id}`
- **Payload igual ao POST**
- **Resposta:**
    ```json
    {
      "mensagem": "Vaga atualizada com sucesso.",
      "dados": {
        "id": 1,
        "numero": 101,
        "tipo": 0,
        "status": 1,
        "patioId": 1,
        "motoId": 2
      }
    }
    ```

#### Remover Vaga
- **DELETE** `/api/vaga/{id}`
- **Resposta:**
    ```json
    {
      "mensagem": "Vaga deletada com sucesso."
    }
    ```

---

### 🏢 PatioController

#### Listar Pátios (Paginação + HATEOAS)
- **GET** `/api/patio?page=1&pageSize=10`
- **Resposta:**
    ```json
    {
      "items": [
        {
          "id": 1,
          "nome": "Pátio Central"
        }
      ],
      "totalCount": 1,
      "page": 1,
      "pageSize": 10,
      "links": [
        { "rel": "self", "method": "GET", "href": "/api/patio?page=1&pageSize=10" }
      ]
    }
    ```

#### Buscar Pátio por ID (com vagas)
- **GET** `/api/patio/{id}`
- **Resposta:**
    ```json
    {
      "id": 1,
      "nome": "Pátio Central",
      "vagas": [
        {
          "id": 1,
          "numero": 101,
          "tipo": 0,
          "status": 0,
          "patioId": 1,
          "motoId": null
        }
      ]
    }
    ```

#### Criar Pátio
- **POST** `/api/patio`
- **Payload de exemplo:**
    ```json
    {
      "nome": "Pátio Central"
    }
    ```
- **Resposta:**
    ```json
    {
      "mensagem": "Pátio cadastrado com sucesso.",
      "dados": {
        "id": 1,
        "nome": "Pátio Central"
      }
    }
    ```

#### Atualizar Pátio
- **PUT** `/api/patio/{id}`
- **Payload igual ao POST**
- **Resposta:**
    ```json
    {
      "mensagem": "Pátio atualizado com sucesso.",
      "dados": {
        "id": 1,
        "nome": "Pátio Central"
      }
    }
    ```

#### Remover Pátio
- **DELETE** `/api/patio/{id}`
- **Resposta:**
    ```json
    {
      "mensagem": "Pátio deletado com sucesso."
    }
    ```

---

### 👤 UsuarioController

#### Listar Usuários (Paginação + HATEOAS)
- **GET** `/api/usuario?page=1&pageSize=10`
- **Resposta:**
    ```json
    {
      "items": [
        {
          "id": 1,
          "nome": "Rafael de Souza Pinto",
          "email": "rafael.pinto@exemplo.com",
          "senha": "12345678",
          "tipo": 0
        }
      ],
      "totalCount": 1,
      "page": 1,
      "pageSize": 10,
      "links": [
        { "rel": "self", "method": "GET", "href": "/api/usuario?page=1&pageSize=10" }
      ]
    }
    ```

#### Buscar Usuário por ID
- **GET** `/api/usuario/{id}`
- **Resposta:**
    ```json
    {
      "id": 1,
      "nome": "Rafael de Souza Pinto",
      "email": "rafael.pinto@exemplo.com",
      "senha": "12345678",
      "tipo": 0
    }
    ```

#### Criar Usuário
- **POST** `/api/usuario`
- **Payload de exemplo:**
    ```json
    {
      "nome": "Rafael de Souza Pinto",
      "email": "rafael.pinto@exemplo.com",
      "senha": "12345678",
      "tipo": 0
    }
    ```
- **Resposta:**
    ```json
    {
      "mensagem": "Usuário cadastrado com sucesso.",
      "dados": {
        "id": 1,
        "nome": "Rafael de Souza Pinto",
        "email": "rafael.pinto@exemplo.com",
        "senha": "12345678",
        "tipo": 0
      }
    }
    ```

#### Atualizar Usuário
- **PUT** `/api/usuario/{id}`
- **Payload igual ao POST**
- **Resposta:**
    ```json
    {
      "mensagem": "Usuário atualizado com sucesso.",
      "dados": {
        "id": 1,
        "nome": "Rafael de Souza Pinto",
        "email": "rafael.pinto@exemplo.com",
        "senha": "12345678",
        "tipo": 0
      }
    }
    ```

#### Remover Usuário
- **DELETE** `/api/usuario/{id}`
- **Resposta:**
    ```json
    {
      "mensagem": "Usuário deletado com sucesso."
    }
    ```

---

### 🧩 DashboardController

#### Dados agregados do sistema para o dashboard
- **GET** `/api/dashboard`
- **Resposta:**
    ```json
    {
      "totalMotos": 10,
      "motosComProblema": 2,
      "vagasLivres": 5,
      "vagasOcupadas": 5,
      "totalVagas": 10
    }
    ```

---

## 🧩 Swagger/OpenAPI

- Todos os endpoints possuem descrição, parâmetros documentados, exemplos de payload (POST/PUT) e modelos de dados.
- Acesse `/swagger` para explorar e testar a API interativamente.
