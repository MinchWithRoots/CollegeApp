using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Kontra
{
    public partial class Form1 : MaterialForm
    {
        private DB db = new DB();

        public Form1()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Brown700, Primary.Brown900, Primary.Brown500, Accent.Orange400, TextShade.WHITE);

            LoadData();
            InitializeComboBox();
            InitializeDatePicker();
        }

        private void InitializeComboBox()
        {
            comboBoxStatus.Items.Clear();
            comboBoxStatus.Items.Add("Incomplete");
            comboBoxStatus.Items.Add("Complete");
            comboBoxStatus.SelectedIndex = 0;

            // Подписываемся на событие изменения выбора
            comboBoxStatus.SelectedIndexChanged += ComboBoxStatus_SelectedIndexChanged;
        }

        private void InitializeDatePicker()
        {
            // Подписываемся на событие изменения даты
            DateTime.ValueChanged += DateTimePicker_ValueChanged;
        }

        private void ComboBoxStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Применяем фильтр по статусу
            string selectedStatus = comboBoxStatus.SelectedItem.ToString();
            LoadData(filterByStatus: selectedStatus);
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            // Применяем фильтр по выбранной дате
            DateTime selectedDate = DateTime.Value.Date;
            LoadData(filterByDate: selectedDate);
        }

        private void LoadData(string filterByStatus = null, string filterBySubject = null, DateTime? filterByDate = null)
        {
            db.openConnection();
            try
            {
                string query = "SELECT * FROM SubjectDebts WHERE 1=1"; // Условие "1=1" упрощает добавление фильтров
                if (!string.IsNullOrEmpty(filterByStatus))
                {
                    query += $" AND Status = '{filterByStatus}'";
                }
                if (!string.IsNullOrEmpty(filterBySubject))
                {
                    query += $" AND Subject = '{filterBySubject}'";
                }
                if (filterByDate.HasValue)
                {
                    query += $" AND CAST(DueDate AS DATE) = '{filterByDate.Value:yyyy-MM-dd}'";
                }

                SqlDataAdapter adapter = new SqlDataAdapter(query, db.GetSqlConnection());
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView.DataSource = table;

                // Проверяем просроченные задолженности
                CheckOverdueTasks();
            }
            finally
            {
                db.closeConnection();
            }
        }

        private void CheckOverdueTasks()
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells["DueDate"].Value != null && row.Cells["Status"].Value != null)
                {
                    DateTime dueDate = Convert.ToDateTime(row.Cells["DueDate"].Value);
                    string status = row.Cells["Status"].Value.ToString();
                    if (dueDate < DateTime.Value.Date && status == "Incomplete")
                    {
                        MessageBox.Show($"Просроченная задолженность: {row.Cells["StudentName"].Value} - {row.Cells["Subject"].Value}",
                            "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void AddRecord()
        {
            db.openConnection();
            try
            {
                string query = "INSERT INTO SubjectDebts (StudentName, Subject, Description, DueDate, Status) VALUES (@StudentName, @Subject, @Description, @DueDate, @Status)";
                SqlCommand cmd = new SqlCommand(query, db.GetSqlConnection());
                cmd.Parameters.AddWithValue("@StudentName", txtStudentName.Text);
                cmd.Parameters.AddWithValue("@Subject", txtSubject.Text);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                cmd.Parameters.AddWithValue("@DueDate", DateTime.Value);
                cmd.Parameters.AddWithValue("@Status", comboBoxStatus.Text);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                db.closeConnection();
            }
            LoadData();
        }

        private void DeleteRecord()
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                db.openConnection();
                try
                {
                    string query = "DELETE FROM SubjectDebts WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, db.GetSqlConnection());
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    db.closeConnection();
                }
                LoadData();
            }
        }

        private void EditRecord()
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                db.openConnection();
                try
                {
                    string query = "UPDATE SubjectDebts SET StudentName = @StudentName, Subject = @Subject, Description = @Description, DueDate = @DueDate, Status = @Status WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, db.GetSqlConnection());
                    cmd.Parameters.AddWithValue("@StudentName", txtStudentName.Text);
                    cmd.Parameters.AddWithValue("@Subject", txtSubject.Text);
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@DueDate", DateTime.Value);
                    cmd.Parameters.AddWithValue("@Status", comboBoxStatus.Text);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    db.closeConnection();
                }
                LoadData();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) => AddRecord();

        private void btnDelete_Click(object sender, EventArgs e) => DeleteRecord();

        private void btnEdit_Click(object sender, EventArgs e) => EditRecord();
    }
}
