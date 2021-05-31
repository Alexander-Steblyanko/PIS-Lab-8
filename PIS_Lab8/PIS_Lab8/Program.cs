using System;
using System.Data.SqlClient;

namespace PIS_Lab8
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=tcp:steblyanko-server.database.windows.net,1433;" +
                "Initial Catalog=SteblyankoLab8;Persist Security Info=False;" +
                "User ID=steblyanko;Password=Alexander789;" +
                "MultipleActiveResultSets=False;Encrypt=True;" +
                "TrustServerCertificate=False;Connection Timeout=30;";

            SqlConnection cnn = new SqlConnection(connectionString);

            cnn.Open();

            /* Writing */
            String sqlWrite = "";

            // Створення таблиць
            // Протокол бригадного розподілу
            sqlWrite += "CREATE TABLE BrigadeDist" +
                "(Num int PRIMARY KEY," +
                "Date date," +
                "Type int," +
                "Earnings int," +
                "OrderNum int," +
                "BrigadeNum int);";
            // Рядок протоколу бригадного розподілу
            sqlWrite += "CREATE TABLE BrigadeDistLines" +
                "(Num int PRIMARY KEY," +
                "DocNum int," +
                "HourlyTariff int," +
                "Hours int," +
                "Coeff decimal(2,1)," +
                "WorkerName varchar(50));"; // Має бути ІД для з’єднання з табл. робітників, але для демонстрації використовуємо імена.
            // Наряд
            sqlWrite += "CREATE TABLE WorkOrder" +
                "(Num int PRIMARY KEY," +
                "Date date," +
                "Type int," +
                "WorkValue int," +
                "Salary int," +
                "BrigadeNum int," +
                "CommissionNum int);"; // Посилання на замовлення

            // Заповнення таблиць
            // 0XXXХ - Бригади, 1XXXХ - Наряди, 2ХХХХ - Прот. розп., 3XXXX - Рядки прот. розп., 4ХХХХ - Замовлення
            
            sqlWrite += "INSERT INTO WorkOrder(Num, Date, Type, WorkValue, Salary, BrigadeNum, CommissionNum)" +
                "VALUES (10000, '2021-05-30', 1, 15000, 14000, 1, 40000);";
            sqlWrite += "INSERT INTO BrigadeDist(Num, Date, Type, Earnings, OrderNum, BrigadeNum)" +
                "VALUES (20000, '2021-05-30', 2, 14000, 10000, 1);";
            sqlWrite += "INSERT INTO BrigadeDistLines(Num, DocNum, HourlyTariff, Hours, Coeff, WorkerName)" +
                "VALUES (30000, 20000, 1250, 8, 1, 'Oleg Semenovich Chevrahov')," +
                "(30001, 20000, 1250, 8, 0.5, 'Sergiy Valeriyovich Serezhkin');";

            SqlCommand writeCommand = new SqlCommand(sqlWrite, cnn);

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = writeCommand;
            adapter.InsertCommand.ExecuteNonQuery();

            writeCommand.Dispose();
            /* !Writing  */

            /* Reading */
            String output = "";

            String sqlRead = "SELECT BrigadeDist.Num, BrigadeDist.Date, BrigadeDistLines.WorkerName, " +
                "BrigadeDistLines.HourlyTariff, BrigadeDistLines.Hours, BrigadeDistLines.Coeff " +
                "FROM (BrigadeDist INNER JOIN BrigadeDistLines ON BrigadeDist.Num=BrigadeDistLines.DocNum)";
            SqlCommand readCommand = new SqlCommand(sqlRead, cnn);
            SqlDataReader dataReader = readCommand.ExecuteReader();

            while (dataReader.Read())
            {
                output += "Номер ПД: " + dataReader.GetValue(0) + " - Дата: " + dataReader.GetValue(1) + " - Ім'я робітника: " + dataReader.GetValue(2) + 
                    " - Тарифна ставка: " + dataReader.GetValue(3) + " - Години: " + dataReader.GetValue(4) + " - КТУ: " + dataReader.GetValue(5) + "\n";
            }
            Console.WriteLine(output);

            dataReader.Close();
            readCommand.Dispose();
            /* !Reading  */

            cnn.Close();
        }
    }
}
