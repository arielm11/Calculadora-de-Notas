# 🎓 Sistema de Notas da FEPI – Gerenciador Acadêmico em C#

Este projeto é um sistema de gerenciamento de notas para matérias do curso de Sistemas de Informação da FEPI. Ele permite cadastrar matérias, lançar notas de provas bimestrais e calcular automaticamente a média final, inclusive considerando o exame final quando necessário.

> 📌 Projeto pessoal com foco em práticas reais de CRUD, SQL Server, lógica de negócios e estruturação de código em C#.

---

## 🚀 Funcionalidades

- ✅ Cadastro, edição, consulta e exclusão de matérias
- ✅ Cadastro, edição e consulta de notas por matéria
- ✅ Cálculo da média ponderada dos bimestres
- ✅ Identificação automática da necessidade de exame final
- ✅ Cálculo da média final com exame
- ✅ Descoberta de quanto o aluno precisa tirar:
  - no **2° Bimestre** para aprovação direta
  - no **Exame Final** para recuperação

---

## 🧠 Regras Acadêmicas Utilizadas

- **Peso das provas**:
  - 1º Bimestre: peso 2
  - 2º Bimestre: peso 3

- **Média para aprovação direta**: ≥ 70  
- **Se média < 70**: aluno faz exame final

- **Cálculo da média final com exame**:
```
Média Final = (Média Bimestral * 3 + Exame Final * 2) / 5
```

- **Aprovação com exame**: Média Final ≥ 50

---

## 🛠 Tecnologias Utilizadas

- 💻 C# (.NET Console App)
- 🗃️ SQL Server
- 🔌 ADO.NET (`SqlConnection`)
- 🧮 Lógica de programação com foco em regras reais
- 🎨 Formatação com cores no terminal para melhor visualização

---

## 🗂️ Estrutura do Projeto

```
├── Program.cs         // Toda a lógica do sistema
├── README.md          // Este arquivo
└── ScriptsSQL/        // Scripts para criar e popular o banco de dados
```

---

## 🧪 Como Executar

1. **Pré-requisitos:**
   - .NET SDK instalado
   - SQL Server instalado e rodando (pode ser o SQL Server Express)

2. **Crie o banco de dados:**
   - Use o script `ScriptsSQL/create_database.sql` (fornecido por você)
   - Ele irá criar as tabelas `Materias` e `Notas`

3. **Ajuste a string de conexão no código se necessário:**
   ```csharp
   string connectionString = "Server=.\SQLEXPRESS;Database=MeuBancoTeste;Trusted_Connection=True;TrustServerCertificate=True";
   ```

4. **Compile e execute o projeto:**
   ```bash
   dotnet run
   ```

---

## 📁 Scripts SQL

Você pode criar os scripts a partir do seu banco. Aqui vai um exemplo para começar:

```sql
CREATE TABLE Materias (
    COD_MATERIA INT PRIMARY KEY IDENTITY(1,1),
    NOM_MATERIA VARCHAR(100) NOT NULL,
    NOM_PROFESSOR VARCHAR(100) NOT NULL,
    PERIODO VARCHAR(50) NOT NULL
);

CREATE TABLE Notas (
    COD_NOTA INT PRIMARY KEY IDENTITY(1,1),
    COD_MATERIA INT FOREIGN KEY REFERENCES Materias(COD_MATERIA),
    PRIMEIRA_NOTA DECIMAL(5,2),
    SEGUNDA_NOTA DECIMAL(5,2),
    EXAME_FINAL DECIMAL(5,2),
    NOTA_FINAL DECIMAL(5,2)
);
```

---

## 📌 Possíveis Melhorias Futuras

- Interface Web com Blazor ou ASP.NET
- Login de usuário
- Histórico de notas por semestre
- Exportação de boletins para PDF

---

## 👨‍💻 Autor

**Ariel Morais**  
Estagiário de Sistemas de Informação na Helibras | Airbus  
GitHub: [@arielm11](https://github.com/arielm11)  
LinkedIn: [linkedin.com/in/ariel-morais](https://www.linkedin.com/in/ariel-morais/)