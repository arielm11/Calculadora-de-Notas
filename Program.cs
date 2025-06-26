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

                if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int operacao) || operacao < 1 || operacao > 5)
                {
                    Console.WriteLine(YELLOW + "Entrada inválida. Por favor, insira um número entre 1 e 4.\n" + RESET);
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
                        SaberNotaB2();
                        break;
                    case 4:
                        ExameFinal();
                        break;
                    case 5:
                        Media();
                        break;
                    case 6:
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
            Console.WriteLine("3 - Saber quanto precisa tirar no 2° Bimestre");
            Console.WriteLine("4 - Saber quanto precisa tirar no Exame Final");
            Console.WriteLine("5 - Ver se foi aprovado");
            Console.WriteLine("6 - Sair");
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

        static void CadastrarNotas()
        {
            Console.WriteLine("\n" + BLUE + "Cadastrar Notas".PadLeft(30 + "Cadastrar Notas".Length / 2).PadRight(60) + RESET);

            string connectionString = "Server=.\\SQLEXPRESS;Database=MeuBancoTeste;Trusted_Connection=True;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COD_MATERIA FROM Materias";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    
                }

            }

            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + "Erro ao cadastrar as notas: " + ex.Message + "\n" + RESET);
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