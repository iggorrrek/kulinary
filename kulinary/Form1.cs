using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Globalization;

namespace kulinary
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dbPath = "C:\\Users\\Asus\\source\\repos\\kulinary\\kulinary\\Data\\MyDatabase.db";


            // Теперь можно подключаться
            string connectionString = $"Data Source={dbPath};Version=3;";


            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string createTableSql = @"
        CREATE TABLE IF NOT EXISTS Users (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Age INTEGER
        )";

                    using (SQLiteCommand command = new SQLiteCommand(createTableSql, connection))
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Таблица Users создана или уже существует!");
                    }
                    string sql = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age)";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", "Иван");
                        command.Parameters.AddWithValue("@Age", 30);

                        int result = command.ExecuteNonQuery();
                        MessageBox.Show($"Добавлено записей: {result}");
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show($"Ошибка БД: {ex.Message}\nКод ошибки: {ex.ErrorCode}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string userText = txtUserInput.Text.Trim();
                string textValue = textBox2.Text.Trim();
                DateTime selectedDate = dateTimePicker1.Value;

                // Проверка обязательных полей
                if (string.IsNullOrEmpty(userText))
                {
                    MessageBox.Show("Введите имя!");
                    return;
                }

                string dbPath = "C:\\Users\\Asus\\source\\repos\\kulinary\\kulinary\\Data\\MyDatabase.db";
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO Users (Name, Date, Text, Photo) VALUES (@Name, @Date, @Text, @Photo)";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        // Правильная передача значений
                        command.Parameters.AddWithValue("@Name", userText);
                        command.Parameters.AddWithValue("@Date", selectedDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Text", textValue);

                        // Если Photo - это путь к изображению
                        string photoPath = string.IsNullOrEmpty(textBox2.Text) ? null : textBox2.Text;
                        command.Parameters.AddWithValue("@Photo", photoPath ?? (object)DBNull.Value);

                        // Выполнение команды
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Запись успешно добавлена!");
                            // Очистка полей после успешного сохранения
                            txtUserInput.Clear();
                            textBox2.Clear();
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Ошибка базы данных: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(GetUsersFromDatabase());
            form2.Show();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public List<User> GetUsersFromDatabase()
        {
            List<User> users = new List<User>();
            string dbPath = "C:\\Users\\Asus\\source\\repos\\kulinary\\kulinary\\Data\\MyDatabase.db";

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();
                string sql = "SELECT * FROM Users";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Date = reader["Date"] != DBNull.Value ?
       DateTime.ParseExact(reader["Date"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture) :
       DateTime.MinValue,
                                Text = reader["Text"].ToString(),
                                Photo = reader["Photo"].ToString()
                            });
                        }
                    }
                }
            }
            return users;
        }
    }
}
