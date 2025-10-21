# GeoSense API

GeoSense API é uma solução RESTful em .NET 8 para gerenciamento de motos, vagas, pátios e usuários. A API está organizada em camadas por pastas e inclui integração com MongoDB, Health Check e Swagger com versionamento (v1 / v2).

---

## Integrantes

- Enzo Giuseppe Marsola – RM: 556310  
- Rafael de Souza Pinto – RM: 555130  
- Luiz Paulo F. Fernandes – RM: 555497

---

## Requisitos (ambiente)

- .NET 8 SDK
- Docker (opcional, recomendado para Mongo local)
- Oracle DB caso deseje usar persistência relacional via EF Core como no projeto (se não houver, os endpoints v2 usam MongoDB)

Portas padrão (conforme launchSettings.json):
- HTTP: 5194
- HTTPS: 7150

---

## Configuração

Editar `GeoSense.API/appsettings.json` conforme seu ambiente:

- ConnectionStrings:Oracle — string de conexão Oracle (se usar EF/Oracle).
- MongoSettings:
  - ConnectionString (ex.: `mongodb://localhost:27017`)
  - DatabaseName (ex.: `geosense`)

Exemplo mínimo (já presente no projeto):
```json
"ConnectionStrings": {
  "Oracle": "Data Source=xxx.xxx.xxx:xxxx/xxxx;User ID=xxxx;Password=xxx;",
  "Mongo": "mongodb://localhost:27017"
},
"MongoSettings": {
  "ConnectionString": "mongodb://localhost:27017",
  "DatabaseName": "geosense"
}
```

---

## Rodando um MongoDB local (opcional)

Com Docker (comando rápido):
```bash
docker run -d --name geosense-mongo -p 27017:27017 mongo:6
```

Após subir o Mongo local, o valor padrão em `appsettings.json` (`mongodb://localhost:27017`) funciona sem alterações.

---

## Build e execução

1. Restaurar pacotes e compilar:
```bash
dotnet restore
dotnet build
```

2. Executar a API:
```bash
dotnet run --project GeoSense.API
```

- Swagger UI: http://localhost:5194/swagger (ou pela porta HTTPS definida)
- Health Check: http://localhost:5194/health

---

## Health Check

Endpoint: `/health`

- Verifica conectividade com:
  - DbContext (EF Core / Oracle) — se configurado.
  - MongoDB (HealthCheck custom para Mongo).
- A resposta é JSON com status geral, duração e detalhes das entradas (Database, MongoDB).

---

## Swagger / Versionamento

- A API utiliza versionamento via URL segment (ApiVersioning).
- Swagger expõe documentação separada para cada versão (v1 e v2).
  - v1 -> endpoints que usam repositórios EF/Oracle (controllers com ApiVersion "1.0")
  - v2 -> endpoints que usam repositórios Mongo (controllers com ApiVersion "2.0")
- No Swagger UI você verá entradas para:
  - GeoSense API v1
  - GeoSense API v2

---

## Endpoints (resumo com versionamento)

- Versão via URL: `/api/v{version}/...`  
  (ex.: `/api/v1/moto`, `/api/v2/moto`)

Moto
- GET  /api/v1/moto?page=1&pageSize=10
- GET  /api/v1/moto/{id}
- POST /api/v1/moto
- PUT  /api/v1/moto/{id}
- DELETE /api/v1/moto/{id}

- GET  /api/v2/moto
- GET  /api/v2/moto/{id}
- POST /api/v2/moto
- PUT  /api/v2/moto/{id}
- DELETE /api/v2/moto/{id}

Vaga
- GET  /api/v1/vaga?page=1&pageSize=10
- GET  /api/v1/vaga/{id}
- POST /api/v1/vaga
- PUT  /api/v1/vaga/{id}
- DELETE /api/v1/vaga/{id}

- GET  /api/v2/vaga
- GET  /api/v2/vaga/{id}
- POST /api/v2/vaga
- PUT  /api/v2/vaga/{id}
- DELETE /api/v2/vaga/{id}

Pátio
- GET  /api/v1/patio?page=1&pageSize=10
- GET  /api/v1/patio/{id}
- POST /api/v1/patio
- PUT  /api/v1/patio/{id}
- DELETE /api/v1/patio/{id}

- GET  /api/v2/patio
- GET  /api/v2/patio/{id}
- POST /api/v2/patio
- PUT  /api/v2/patio/{id}
- DELETE /api/v2/patio/{id}

Usuário
- GET  /api/v1/usuario?page=1&pageSize=10
- GET  /api/v1/usuario/{id}
- GET  /api/v1/usuario/{email}
- POST /api/v1/usuario
- PUT  /api/v1/usuario/{id}
- DELETE /api/v1/usuario/{id}

- GET  /api/v2/usuario
- GET  /api/v2/usuario/{id}
- GET  /api/v2/usuario/by-email/{email}
- POST /api/v2/usuario
- PUT  /api/v2/usuario/{id}
- DELETE /api/v2/usuario/{id}

Aggregate (VagaAggregate v2)
- POST /api/v2/vaga-aggregate/{id}/alocar   (body: { "motoId": long })
- POST /api/v2/vaga-aggregate/{id}/liberar

HATEOAS:
- List endpoints retornam links `self`, `prev`, `next` (gerados pelo helper HATEOAS).

---

## Testes

Executar todos os testes:
```bash
dotnet test
```

- Os testes de integração com Mongo utilizam Mongo2Go (in-memory/ephemeral) — não é necessário um Mongo externo para esses testes.
- Os testes unitários usam in-memory provider do EF Core quando aplicável.

---

## Observações de configuração rápida

- Para usar endpoints v2 (Mongo) assegure que `MongoSettings:ConnectionString` aponte para um Mongo acessível.
- Para que `/health` reporte sucesso para todos checks, assegure que tanto Oracle (DbContext) quanto Mongo estejam acessíveis, ou ajuste `appsettings.json` conforme seu ambiente de avaliação.
Para dúvidas sobre execução ou configuração, abra uma issue no repositório.

```
