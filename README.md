# GeoSense API - CP4

(CP4 - .NET) Refatoração da API do projeto GeoSense do Challenge de 2025

## 📝 Descrição do domínio

O **GeoSense** é um sistema para controle e gestão de motos e vagas em pátios de manutenção.  
Permite cadastrar motos, alocar/desalocar em vagas, registrar defeitos, auditar ocupação de vagas, classificar vagas por tipo/status e gerar dashboards de operação.

## 🚀 Instruções de execução

1. **Instale o .NET 8 SDK** e configure o Oracle client conforme necessário.
2. **Configure a connection string** no arquivo `GeoSense.API/appsettings.json`.
3. **Rode as migrations** (apenas uma vez, se for a primeira execução):
   ```
   dotnet ef database update --project GeoSense.API
   ```
4. **Execute a aplicação:**
   ```
   dotnet run --project GeoSense.API
   ```
5. **Acesse o Swagger:**  
   - [https://localhost:7150/swagger](https://localhost:7150/swagger)  
   - Ou conforme porta configurada no `launchSettings.json`.

## 📚 Exemplos de requisições

### Buscar todas as motos
```http
GET /api/moto
```

### Cadastrar uma moto
```http
POST /api/moto
Content-Type: application/json

{
  "modelo": "Honda CG 160",
  "placa": "ABC1234",
  "chassi": "9C2KC1670GR123456",
  "problemaIdentificado": "Vazamento de óleo",
  "vagaId": 1
}
```

### Atualizar uma moto
```http
PUT /api/moto/1
Content-Type: application/json

{
  "modelo": "Honda CG 160",
  "placa": "DEF5678",
  "chassi": "9C2KC1670GR654321",
  "problemaIdentificado": "Sem problemas",
  "vagaId": 2
}
```

### Buscar todas as vagas
```http
GET /api/vaga
```

### Cadastrar uma vaga
```http
POST /api/vaga
Content-Type: application/json

{
  "numero": 10,
  "tipo": 0,
  "status": 0,
  "patioId": 1
}
```

## 🧪 Swagger

A documentação interativa de todos os endpoints está disponível via Swagger em `/swagger` após iniciar a API.

---

## ℹ️ Observações

- Utilize DTOs nas requisições/respostas da API.
- Todas as regras de negócio estão encapsuladas no domínio, seguindo DDD e Clean Architecture.

##### 👥 Grupo Challenge:
- Enzo Giuseppe Marsola – RM: 556310, Turma: 2TDSPK
- Rafael de Souza Pinto – RM: 555130, Turma: 2TDSPY
- Luiz Paulo F. Fernandes – RM: 555497, Turma: 2TDSPF
