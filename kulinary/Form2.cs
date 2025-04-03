using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kulinary
{
    public partial class Form2 : Form
    {
        private List<User> _users;

        public Form2(List<User> users)
        {
            InitializeComponent();
            _users = users;
            SetupDataGridView();
            LoadData();
        }
        private void SetupDataGridView()
        {
            // Настройка внешнего вида
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Добавляем колонки
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Name",
                HeaderText = "Имя",
                Width = 150
            });

            // Добавьте остальные колонки по аналогии
        }

        private void LoadData()
        {
            dataGridView1.DataSource = _users;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
