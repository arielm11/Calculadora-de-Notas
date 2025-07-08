using Microsoft.Data.SqlClient;

internal class Program
{
    private static void Main(string[] args)
    {

        // Códigos de cores para o terminal
        const string RED = "\u001b[31m";
        const string GREEN = "\u001b[32m";
        const string YELLOW = "\u001b[33m";
        const string BLUE = "\u001b[34m";
        const string CYAN = "\u001b[36m";
        const string RESET = "\u001b[0m";
        const string BOLD = "\u001b[1m";

        // Teste de conexão ao banco de dados
        //SqlConnection? connection = ConnectToDatabase();

        RunProgram();

        // Função para rodar o programa até o usuário decidir sair
        static void RunProgram()
        {
            while (true)
            {
                PrintHeader();
                printMenu();

                Console.Write("\n" + YELLOW + "Digite o número da operação: " + RESET);
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int operacao) || operacao < 1 || operacao > 11)
                {
                    Console.WriteLine(YELLOW + "Entrada inválida. Por favor, insira um número entre 1 e 10.\n" + RESET);
                    continue;
                }
                switch (operacao)
                {
                    case 1:
                        CadastrarMateria();
                        break;
                    case 2:
                        ConsultarMaterias();
                        break;
                    case 3:
                        EditarMateria();
                        break;
                    case 4:
                        DeletarMateria();
                        break;
                    case 5:
                        CadastrarNotas();
                        break;
                    case 6:
                        ConsultarNotas();
                        break;
                    case 7:
                        EditarNotas();
                        break;
                    case 8:
                        SaberNotaB2();
                        break;
                    case 9:
                        ExameFinal();
                        break;
                    case 10:
                        Media();
                        break;
                    case 11:
                        Console.WriteLine("Saindo do Programa");
                        return;
                }
            }
        }

        // Função para imprimir o cabeçalho
        static void PrintHeader()
        {
            Console.WriteLine(BLUE + new string('=', 60) + RESET);
            Console.WriteLine(BOLD + "SISTEMA DE CLASSIFICAÇÃO DE ALUNOS".PadLeft(30 + "SISTEMA DE CLASSIFICAÇÃO DE ALUNOS".Length / 2).PadRight(60) + RESET);
            Console.WriteLine(BLUE + new string('=', 60) + RESET);
        }

        // Função para imprimir o menu
        static void printMenu()
        {
            Console.WriteLine("\n" + CYAN + "Menu Principal".PadLeft(30 + "Menu Principal".Length / 2).PadRight(60) + RESET);
            Console.WriteLine(new string('-', 60));
            Console.WriteLine("1 - Cadastrar Matéria");
            Console.WriteLine("2 - Consultar Matérias Cadastradas");
            Console.WriteLine("3 - Editar Matéria");
            Console.WriteLine("4 - Deletar Matéria");
            Console.WriteLine("5 - Cadastrar Notas");
            Console.WriteLine("6 - Consultar Notas");
            Console.WriteLine("7 - Editar Notas");
            Console.WriteLine("8 - Saber quanto precisa tirar no 2° Bimestre");
            Console.WriteLine("9 - Saber quanto precisa tirar no Exame Final");
            Console.WriteLine("10 - Ver se foi aprovado");
            Console.WriteLine("11 - Sair");
            Console.WriteLine(new string('-', 60));
        }

        //Função para testar conexão ao banco de dados
        static SqlConnection? ConnectToDatabase()
        {
            string connectionString = "Server=.\\SQLEXPRESS;Database=MeuBancoTeste;Trusted_Connection=True;TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine(RED + "Erro ao conectar ao banco de dados: " + e.Message + RESET);
                return null;
            }
        }

        // Função para cadastrar uma nova matéria
        static void CadastrarMateria()
        {
            Console.WriteLine("\n" + BLUE + "Cadastrar Matéria".PadLeft(30 + "Cadastrar Matéria".Length / 2).PadRight(60) + RESET);

            Console.WriteLine(YELLOW + "Digite o nome da matéria: " + RESET);
            string? nomeMateria = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nomeMateria))
            {
                Console.WriteLine(YELLOW + "Entrada inválida. Por favor, insira um número.\n" + RESET);
            }

            Console.WriteLine(YELLOW + "Digite o nome do Professor: " + RESET);
            string? nomeProfessor = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nomeProfessor))
            {
                Console.WriteLine(RED + "Nome do professor não pode ser vazio ou nulo." + RESET);
                return;
            }

            Console.WriteLine(YELLOW + "Digite o periodo da matéria: " + RESET);
            string? periodoMateria = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(periodoMateria))
            {
                Console.WriteLine(RED + "Período da matéria não pode ser vazio ou nulo." + RESET);
                return;
            }

            string connectionString = "Server=.\\SQLEXPRESS;Database=MeuBancoTeste;Trusted_Connection=True;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Materias(NOM_MATERIA, NOM_PROFESSOR, PERIODO) VALUES (@NomeMateria, @NomeProfessor, @PeriodoMateria)";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@NomeMateria", nomeMateria);
                    cmd.Parameters.AddWithValue("@NomeProfessor", nomeProfessor);
                    cmd.Parameters.AddWithValue("@PeriodoMateria", periodoMateria);

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    Console.WriteLine(GREEN + $"Matéria '{nomeMateria}' cadastrada com sucesso! Linhas afetadas: {linhasAfetadas}" + RESET);
                }
            }
        }

        // Função para editar matérias cadastradas
        static void EditarMateria()
        {
            Console.WriteLine("\n" + BLUE + "Editar Matéria".PadLeft(30 + "Editar Matéria".Length / 2).PadRight(60) + RESET);
            ConsultarMaterias();
            List<int> materiasIds = ListarMateriasIds();
            if (materiasIds.Count == 0)
            {
                Console.WriteLine(RED + "Nenhuma matéria cadastrada. Por favor, cadastre uma matéria primeiro." + RESET);
                return;
            }
            Console.WriteLine(YELLOW + "Digite o código da matéria que deseja editar: " + RESET);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int codMateria) || !materiasIds.Contains(codMateria))
            {
                Console.WriteLine(RED + "Código inválido. Por favor, insira um código de matéria válido.\n" + RESET);
                return;
            }

            Console.WriteLine(YELLOW + "Digite o novo nome da matéria: " + RESET);
            string? novoNomeMateria = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(novoNomeMateria))
            {
                Console.WriteLine(RED + "Nome da matéria não pode ser vazio ou nulo." + RESET);
                return;
            }

            Console.WriteLine(YELLOW + "Digite o novo nome do Professor: " + RESET);
            string? novoNomeProfessor = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(novoNomeProfessor))
            {
                Console.WriteLine(RED + "Nome do professor não pode ser vazio ou nulo." + RESET);
                return;
            }

            Console.WriteLine(YELLOW + "Digite o novo período da matéria: " + RESET);
            string? novoPeriodoMateria = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(novoPeriodoMateria))
            {
                Console.WriteLine(RED + "Período da matéria não pode ser vazio ou nulo." + RESET);
                return;
            }

            try
            {
                string connectionString = "Server=.\\SQLEXPRESS;Database=MeuBancoTeste;Trusted_Connection=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Materias SET NOM_MATERIA= @NovoNomeMateria, NOM_PROFESSOR = @NovoNomeProfessor, PERIODO = @NovoPeriodo WHERE COD_MATERIA = @CodMateria ";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.AddWithValue("@NovoNomeMateria", novoNomeMateria);
                        cmd.Parameters.AddWithValue("@NovoNomeProfessor", novoNomeProfessor);
                        cmd.Parameters.AddWithValue("@NovoPeriodo", novoPeriodoMateria);
                        cmd.Parameters.AddWithValue("@Codmateria", codMateria);

                        int linhasAfetadas = cmd.ExecuteNonQuery();
                        Console.WriteLine(GREEN + $"Matéria atualizada!" + RESET);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + "Erro ao editar a matéria: " + ex.Message + "\n" + RESET);
            }
        }

        //Função para deletar uma matéria já cadastrada
        static void DeletarMateria()
        {
            Console.WriteLine("\n" + BLUE + "Deletar Matéria".PadLeft(30 + "Deletar Matéria".Length / 2).PadRight(60) + RESET);
            ConsultarMaterias();
            List<int> materiasIds = ListarMateriasIds();
            if (materiasIds.Count == 0)
            {
                Console.WriteLine(RED + "Nenhuma matéria cadastrada. Por favor, cadastre uma matéria primeiro." + RESET);
                return;
            }
            Console.WriteLine(YELLOW + "Digite o código da matéria que deseja deletar: " + RESET);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int codMateria) || !materiasIds.Contains(codMateria))
            {
                Console.WriteLine(RED + "Código inválido. Por favor, insira um código de matéria válido.\n" + RESET);
                return;
            }
            try
            {
                string connectionString = "Server=.\\SQLEXPRESS;Database=MeuBancoTeste;Trusted_Connection=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Materias WHERE COD_MATERIA = @CodMateria";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CodMateria", codMateria);
                        int linhasAfetadas = cmd.ExecuteNonQuery();
                        Console.WriteLine(GREEN + $"Matéria deletada!" + RESET);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + "Erro ao deletar a matéria: " + ex.Message + "\n" + RESET);
            }
        }

        // Função para Listar as matérias cadastradas
        static List<int> ListarMateriasIds()
        {
            var ids = new List<int>();
            string connectionString = "Server=.\\SQLEXPRESS;Database=MeuBancoTeste;Trusted_Connection=True;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COD_MATERIA FROM Materias";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            int codMateria = reader.GetInt32(0);
                            ids.Add(codMateria);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(RED + "Erro ao ler os dados da matéria: " + ex.Message + RESET);
                        }
                    }
                }
            }

            return ids;
        }

        // Função para consultar as matérias cadastradas
        static void ConsultarMaterias()
        {
            Console.WriteLine("\n" + BLUE + "Lista de Matérias Cadastradas".PadLeft(30 + "Lista de Matérias Cadastradas".Length / 2).PadRight(60) + RESET);

            string connectionString = "Server=.\\SQLEXPRESS;Database=MeuBancoTeste;Trusted_Connection=True;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COD_MATERIA, NOM_MATERIA, NOM_PROFESSOR, PERIODO FROM Materias";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    bool headerPrinted = false;
                    while (reader.Read())
                    {
                        try
                        {
                            if (!headerPrinted)
                            {
                                Console.WriteLine("+---------+----------------------+----------------------+----------------------+");
                                Console.WriteLine("| Código  | Matéria              | Professor            | Período              |");
                                Console.WriteLine("+---------+----------------------+----------------------+----------------------+");
                                headerPrinted = true;
                            }

                            int codMateria = reader.GetInt32(0);

                            string nomeMateria = reader.GetValue(1).ToString() ?? "";
                            string nomeProfessor = reader.GetValue(2).ToString() ?? "";
                            string periodoMateria = reader.GetValue(3).ToString() ?? "";

                            Console.WriteLine($"| {codMateria,-7} | {nomeMateria,-20} | {nomeProfessor,-20} | {periodoMateria,-20} |");
                            if (headerPrinted)
                            {
                                Console.WriteLine("+---------+----------------------+----------------------+----------------------+");
                            }
                            else
                            {
                                Console.WriteLine("Nenhuma matéria cadastrada.\n");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(RED + "Erro ao ler os dados da matéria: " + ex.Message + RESET);
                        }
                    }
                }
            }
        }

        // Função para Cadastrar Notas em Matérias já cadastradas
        static void CadastrarNotas()
        {
            Console.WriteLine("\n" + BLUE + "Cadastrar Notas".PadLeft(30 + "Cadastrar Notas".Length / 2).PadRight(60) + RESET);

            ConsultarMaterias();

            List<int> materiasIds = ListarMateriasIds();

            if (materiasIds.Count == 0)
            {
                Console.WriteLine(RED + "Nenhuma matéria cadastrada. Por favor, cadastre uma matéria primeiro." + RESET);
                return;
            }

            Console.WriteLine(YELLOW + "Digite o código da matéria: " + RESET);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int codMateria) || !materiasIds.Contains(codMateria))
            {
                Console.WriteLine(RED + "Código inválido. Por favor, insira um código de matéria válido.\n" + RESET);
                return;
            }

            Console.WriteLine(YELLOW + "Digite a nota do 1° Bimestre: " + RESET);
            string? nota1 = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nota1) || !float.TryParse(nota1, out float notaB1) || notaB1 < 0 || notaB1 > 100)
            {
                Console.WriteLine(RED + "Nota inválida! A nota deve estar entre 0 e 100.\n" + RESET);
                return;
            }

            Console.WriteLine(YELLOW + "Digite a nota do 2° Bimestre: " + RESET);
            string? nota2 = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nota2) || !float.TryParse(nota2, out float notaB2) || notaB2 < 0 || notaB2 > 100)
            {
                Console.WriteLine(RED + "Nota inválida! A nota deve estar entre 0 e 100.\n" + RESET);
                return;
            }


            float media = (notaB1 * 2 + notaB2 * 3) / 5;

            if (media < 70)
            {
                Console.WriteLine(YELLOW + "Digite a nota do Exame Final: " + RESET);
                string? nota3 = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nota3) || !float.TryParse(nota3, out float notaExame) || notaExame < 0 || notaExame > 100)
                {
                    Console.WriteLine(RED + "Nota inválida! A nota deve estar entre 0 e 100.\n" + RESET);
                    return;
                }

                float mediaFinal = (media * 3 + notaExame * 2) / 5;


                try
                {
                    string connectionString = "Server=.\\SQLEXPRESS;Database=MeuBancoTeste;Trusted_Connection=True;TrustServerCertificate=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "INSERT INTO Notas (COD_MATERIA, PRIMEIRA_NOTA, SEGUNDA_NOTA, EXAME_FINAL, NOTA_FINAL) VALUES (@CodMateria,@PrimeiraNota,@SegundaNota,@ExameFinal,@NotaFinal)";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@CodMateria", codMateria);
                            cmd.Parameters.AddWithValue("@PrimeiraNota", notaB1);
                            cmd.Parameters.AddWithValue("@SegundaNota", notaB2);
                            cmd.Parameters.AddWithValue("@ExameFinal", notaExame);
                            cmd.Parameters.AddWithValue("@NotaFinal", mediaFinal);

                            int linhasAfetadas = cmd.ExecuteNonQuery();
                            Console.WriteLine(GREEN + $"Notas Cadastradas!" + RESET);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(RED + "Erro ao cadastrar as notas: " + ex.Message + "\n" + RESET);
                }
            }
            else
            {
                try
                {
                    string connectionString = "Server=.\\SQLEXPRESS;Database=MeuBancoTeste;Trusted_Connection=True;TrustServerCertificate=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "INSERT INTO Notas (COD_MATERIA, PRIMEIRA_NOTA, SEGUNDA_NOTA, NOTA_FINAL) VALUES (@CodMateria,@PrimeiraNota,@SegundaNota,@NotaFinal)";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@CodMateria", codMateria);
                            cmd.Parameters.AddWithValue("@PrimeiraNota", notaB1);
                            cmd.Parameters.AddWithValue("@SegundaNota", notaB2);
                            cmd.Parameters.AddWithValue("@NotaFinal", media);

                            int linhasAfetadas = cmd.ExecuteNonQuery();
                            Console.WriteLine(GREEN + $"Notas Cadastradas!" + RESET);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(RED + "Erro ao cadastrar as notas: " + ex.Message + "\n" + RESET);
                }
            }
        }

        //Função para consultar as notas cadastradas
        static void ConsultarNotas()
        {
            Console.WriteLine("\n" + BLUE + "Consultar Notas".PadLeft(30 + "Consultar Notas".Length / 2).PadRight(60) + RESET);
            string connectionString = "Server=.\\SQLEXPRESS;Database=MeuBancoTeste;Trusted_Connection=True;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT N.COD_MATERIA, M.NOM_MATERIA, N.PRIMEIRA_NOTA, N.SEGUNDA_NOTA, N.EXAME_FINAL, N.NOTA_FINAL FROM Notas N JOIN Materias M ON N.COD_MATERIA = M.COD_MATERIA";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    bool headerPrinted = false;
                    while (reader.Read())
                    {
                        try
                        {
                            if (!headerPrinted)
                            {
                                Console.WriteLine("+---------+----------------------+----------------------+----------------------+----------------------+----------------------+");
                                Console.WriteLine("| Código  | Matéria              | 1° Bimestre          | 2° Bimestre          | Exame Final          | Nota Final           |");
                                Console.WriteLine("+---------+----------------------+----------------------+----------------------+----------------------+----------------------+");
                                headerPrinted = true;
                            }

                            int codMateria = reader.GetInt32(0);
                            string nomeMateria = reader.GetValue(1).ToString() ?? "";
                            float notaB1 = (float)reader.GetDecimal(2);
                            float notaB2 = (float)reader.GetDecimal(3);
                            float exameFinal = reader.IsDBNull(4) ? 0 : (float)reader.GetDecimal(4);
                            float notaFinal = (float)reader.GetDecimal(5);

                            Console.WriteLine($"| {codMateria,-7} | {nomeMateria,-20} | {notaB1,-20} | {notaB2,-20} | {exameFinal,-20} | {notaFinal,-20} |");
                            if (headerPrinted)
                            {
                                Console.WriteLine("+---------+----------------------+----------------------+----------------------+----------------------+----------------------+");
                            }
                            else
                            {
                                Console.WriteLine("Nenhuma matéria cadastrada.\n");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(RED + "Erro ao ler os dados das notas: " + ex.Message + RESET);
                        }
                    }
                }
            }
        }

        //Função para editar notas já cadastradas
        static void EditarNotas()
        {
            Console.WriteLine("\n" + BLUE + "Editar Notas".PadLeft(30 + "Editar Notas".Length / 2).PadRight(60) + RESET);

            ConsultarNotas();

            List<int> materiasIds = ListarMateriasIds();

            if (materiasIds.Count == 0)
            {
                Console.WriteLine(RED + "Nenhuma matéria cadastrada. Por favor, cadastre uma matéria primeiro." + RESET);
                return;
            }

            Console.WriteLine(YELLOW + "Digite o código da matéria que deseja editar as notas: " + RESET);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int codMateria) || !materiasIds.Contains(codMateria))
            {
                Console.WriteLine(RED + "Código inválido. Por favor, insira um código de matéria válido.\n" + RESET);
                return;
            }

            Console.WriteLine(YELLOW + "Digite a nova nota do 1° Bimestre: " + RESET);
            string? nota1 = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nota1) || !float.TryParse(nota1, out float notaB1) || notaB1 < 0 || notaB1 > 100)
            {
                Console.WriteLine(RED + "Nota inválida! A nota deve estar entre 0 e 100.\n" + RESET);
                return;
            }

            Console.WriteLine(YELLOW + "Digite a nova nota do 2° Bimestre: " + RESET);
            string? nota2 = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nota2) || !float.TryParse(nota2, out float notaB2) || notaB2 < 0 || notaB2 > 100)
            {
                Console.WriteLine(RED + "Nota inválida! A nota deve estar entre 0 e 100.\n" + RESET);
                return;
            }

            float media = (notaB1 * 2 + notaB2 * 3) / 5;

            if (media < 70)
            {
                Console.WriteLine(YELLOW + "Digite a nova nota do Exame Final: " + RESET);
                string? nota3 = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nota3) || !float.TryParse(nota3, out float notaExame) || notaExame < 0 || notaExame > 100)
                {
                    Console.WriteLine(RED + "Nota inválida! A nota deve estar entre 0 e 100.\n" + RESET);
                    return;
                }

                float mediaFinal = (media * 3 + notaExame * 2) / 5;

                try
                {
                    string connectionString = "Server=.\\SQLEXPRESS;Database=MeuBancoTeste;Trusted_Connection=True;TrustServerCertificate=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "UPDATE Notas SET PRIMEIRA_NOTA= @PrimeiraNota, SEGUNDA_NOTA = @SegundaNota, EXAME_FINAL = @ExameFinal, NOTA_FINAL = @NotaFinal WHERE COD_MATERIA = @CodMateria";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@CodMateria", codMateria);
                            cmd.Parameters.AddWithValue("@PrimeiraNota", notaB1);
                            cmd.Parameters.AddWithValue("@SegundaNota", notaB2);
                            cmd.Parameters.AddWithValue("@ExameFinal", notaExame);
                            cmd.Parameters.AddWithValue("@NotaFinal", mediaFinal);

                            int linhasAfetadas = cmd.ExecuteNonQuery();
                            Console.WriteLine(GREEN + $"Notas atualizadas!" + RESET);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(RED + "Erro ao atualizar as notas: " + ex.Message + "\n" + RESET);
                }
            }
            else
            {
                try
                {
                    string connectionString = "Server=.\\SQLEXPRESS;Database=MeuBancoTeste;Trusted_Connection=True;TrustServerCertificate=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "UPDATE Notas SET PRIMEIRA_NOTA= @PrimeiraNota, SEGUNDA_NOTA = @SegundaNota, EXAME_FINAL = null, NOTA_FINAL = @NotaFinal WHERE COD_MATERIA = @CodMateria";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@CodMateria", codMateria);
                            cmd.Parameters.AddWithValue("@PrimeiraNota", notaB1);
                            cmd.Parameters.AddWithValue("@SegundaNota", notaB2);
                            cmd.Parameters.AddWithValue("@NotaFinal", media);

                            int linhasAfetadas = cmd.ExecuteNonQuery();
                            Console.WriteLine(GREEN + $"Notas atualizadas!" + RESET);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(RED + "Erro ao atualizar as notas: " + ex.Message + "\n" + RESET);
                }
            }

        }

        //Função para saber a nota do 2° Bimestre
        static float SaberNotaB2()
        {
            Console.WriteLine("\n" + BLUE + "Descobrir nota do 2° Bimestre".PadLeft(30 + "Descobrir nota do 2° Bimestre".Length / 2).PadRight(60) + RESET);

            try
            {
                Console.WriteLine(YELLOW + "Digite qual foi a sua nota no 1° Bimestre: " + RESET);
                float nota1 = Convert.ToSingle(Console.ReadLine());

                if (nota1 < 0 || nota1 > 100)
                {
                    Console.WriteLine(RED + "Nota inválida! A nota deve estar entre 0 e 100.\n" + RESET);
                    return 0;
                }

                float numerador = 350 - nota1 * 2;
                float notaMin2 = numerador / 3;

                Console.WriteLine($"Para ser aprovado, você precisa tirar no minímo {notaMin2} no 2° Bimestre.\n");
                return notaMin2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + "Erro ao calcular a nota do 2° Bimestre: " + ex.Message + "\n" + RESET);
                return 0;
            }
        }

        // Função para saber a nota do Exame Final 
        static float ExameFinal()
        {
            Console.WriteLine("\n" + BLUE + "Descobrir quanto precisa tirar no exame final".PadLeft(30 + "Descobrir quanto precisa tirar no exame final".Length / 2).PadRight(60) + RESET);

            try
            {
                Console.WriteLine(YELLOW + "Digite a nota do 1° Bimestre: " + RESET);
                float nota1 = Convert.ToSingle(Console.ReadLine());
                Console.WriteLine(YELLOW + "Digite a nota do 2° Bimestre: " + RESET);
                float nota2 = Convert.ToSingle(Console.ReadLine());

                if (nota1 < 0 || nota1 > 100 || nota2 < 0 || nota2 > 100)
                {
                    Console.WriteLine(RED + "Notas inválidas! As notas devem estar entre 0 e 100.\n" + RESET);
                    return 0;
                }

                float mediaFinal = (nota1 * 2 + nota2 * 3) / 5;
                float numerador = 250 - mediaFinal * 3;
                float notaExameMin = numerador / 2;

                Console.WriteLine($"Para ser aprovado, você precisa tirar no minímo {notaExameMin} no Exame Final.\n");
                return notaExameMin;
            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + "Erro ao calcular a nota do Exame Final: " + ex.Message + "\n" + RESET);
                return 0;
            }
        }

        // Função para calcular Média
        static float Media()
        {
            Console.WriteLine("\n" + BLUE + "Descobrir se foi aprovado!".PadLeft(30 + "Descobrir se foi aprovado!".Length / 2).PadRight(60) + RESET);

            try
            {
                Console.WriteLine(YELLOW + "Digite a nota do 1° Bimestre: " + RESET);
                float num1 = Convert.ToSingle(Console.ReadLine());
                Console.WriteLine(YELLOW + "Digite a nota do 2° Bimestre: " + RESET);
                float num2 = Convert.ToSingle(Console.ReadLine());

                if (num1 < 0 || num1 > 100 || num2 < 0 || num2 > 100)
                {
                    Console.WriteLine(RED + "Notas inválidas! As notas devem estar entre 0 e 100.\n" + RESET);
                    return 0;
                }

                float media = (num1 * 2 + num2 * 3) / 5;

                if (media < 70)
                {
                    Console.WriteLine(YELLOW + "Digite a nota do Exame Final: " + RESET);
                    float notaExame = Convert.ToSingle(Console.ReadLine());

                    if (notaExame < 0 || notaExame > 100)
                    {
                        Console.WriteLine(RED + "Nota inválida! A nota do Exame Final deve estar entre 0 e 100.\n" + RESET);
                        return 0;
                    }

                    float mediaFinal = (media * 3 + notaExame * 2) / 5;

                    if (mediaFinal >= 50)
                    {
                        Console.WriteLine(GREEN + $"Você foi aprovado com média final de {mediaFinal}!\n" + RESET);
                    }
                    else
                    {
                        Console.WriteLine(RED + $"Você foi reprovado com média final de {mediaFinal}.\n" + RESET);
                    }
                }
                else
                {
                    Console.WriteLine(GREEN + $"Você foi aprovado com média de {media}!\n" + RESET);
                }

                return media;
            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + "Erro ao calcular a média: " + ex.Message + "\n" + RESET);
                return 0;
            }
        }
    }
}