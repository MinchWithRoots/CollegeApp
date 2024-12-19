using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
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
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey700, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Blue400, TextShade.WHITE);

            LoadData();
            InitializeComboBox();
            InitializeDatePicker();
            InitializeDataGridView();
        }

        private void InitializeComboBox()
        {
            comboBoxStatus.Items.Clear();
            comboBoxStatus.Items.Add("Все");
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

        private void InitializeDataGridView()
        {
            // Убедимся, что DataGridView пуст перед добавлением новых колонок
            dataGridView.Columns.Clear();

            // Добавляем колонку для Id (скрытую по умолчанию)
            var idColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Id",
                DataPropertyName = "Id", // Связь с источником данных
                Name = "IdColumn",
                Visible = false // Скрываем колонку, чтобы она не отображалась
            };
            dataGridView.Columns.Add(idColumn);

            // Добавляем выпадающий список для "Subject"
            var subjectColumn = new DataGridViewComboBoxColumn
            {
                HeaderText = "Subject",
                DataPropertyName = "Subject", // Связь с источником данных
                Name = "SubjectColumn"
            };
            LoadComboBoxItems("SELECT DISTINCT Subject FROM SubjectDebts", subjectColumn);
            dataGridView.Columns.Add(subjectColumn);

            // Добавляем выпадающий список для "Status"
            var statusColumn = new DataGridViewComboBoxColumn
            {
                HeaderText = "Status",
                DataPropertyName = "Status",
                Name = "StatusColumn"
            };
            statusColumn.Items.AddRange("Incomplete", "Complete");
            dataGridView.Columns.Add(statusColumn);

            // Прочие колонки
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "StudentName",
                DataPropertyName = "StudentName",
                Name = "StudentNameColumn"
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Description",
                DataPropertyName = "Description",
                Name = "DescriptionColumn"
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "DueDate",
                DataPropertyName = "DueDate",
                Name = "DueDateColumn"
            });

            // Настройки DataGridView
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = true;
            dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
        }


        private void LoadComboBoxItems(string query, DataGridViewComboBoxColumn comboBoxColumn)
        {
            db.openConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(query, db.GetSqlConnection());
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBoxColumn.Items.Add(reader[0].ToString());
                }
            }
            finally
            {
                db.closeConnection();
            }
        }

        private void ComboBoxStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Применяем фильтр по статусу, если выбран не "Все"
            string selectedStatus = comboBoxStatus.SelectedItem.ToString();
            if (selectedStatus == "Все")
            {
                LoadData(); // Загружаем всю таблицу
            }
            else
            {
                LoadData(filterByStatus: selectedStatus);
            }
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
                string query = "SELECT Id, StudentName, Subject, Description, DueDate, Status FROM SubjectDebts";

                // Формируем фильтр
                List<string> filters = new List<string>();
                if (!string.IsNullOrEmpty(filterByStatus))
                {
                    filters.Add($"Status = '{filterByStatus}'");
                }
                if (!string.IsNullOrEmpty(filterBySubject))
                {
                    filters.Add($"Subject = '{filterBySubject}'");
                }
                if (filterByDate.HasValue)
                {
                    filters.Add($"CAST(DueDate AS DATE) = '{filterByDate.Value:yyyy-MM-dd}'");
                }

                // Присоединяем фильтры к запросу
                if (filters.Count > 0)
                {
                    query += " WHERE " + string.Join(" AND ", filters);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(query, db.GetSqlConnection());
                DataTable table = new DataTable();
                adapter.Fill(table);

                // Привязка данных к DataGridView
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
                if (dataGridView.Columns.Contains("DueDate") &&
                    row.Cells["DueDate"].Value != null &&
                    row.Cells["Status"].Value != null)
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
                var selectedValue = dataGridView.SelectedRows[0].Cells["IdColumn"].Value;

                if (selectedValue != null && int.TryParse(selectedValue.ToString(), out int id))
                {
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
                else
                {
                    MessageBox.Show("Выберите строку с корректным идентификатором!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
