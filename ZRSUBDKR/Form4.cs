using System;
using System.Windows.Forms;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace ZRSUBDKR
{
    public partial class Form4 : Form
    {
        FbConnection fb;
        int flag = 0;
        string table;
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            FbConnectionStringBuilder fb_con = new FbConnectionStringBuilder();
            fb_con.Charset = "WIN1251"; //используемая кодировка
            fb_con.UserID = "SYSDBA"; //логин
            fb_con.Password = "masterkey"; //пароль
            fb_con.Database = @"C:\Users\igych\Desktop\ZRSUBDKR\LAB31.FDB"; //путь к файлу базы данных
            fb_con.ServerType = 0;
            fb = new FbConnection(fb_con.ToString());
            fb.Open();
            FbDatabaseInfo fb_inf = new FbDatabaseInfo(fb);
            MessageBox.Show("Info: " + fb_inf.ServerClass + "; " + fb_inf.ServerVersion);
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Form form = Application.OpenForms[0];
                Hide();
                e.Cancel = true;
                form.Close();
                if (fb.State == ConnectionState.Open)
                    fb.Close(); //закрываем соединение, т.к. оно нам больше не нужно
            }
        }
        private void clear()
        {           
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            textBox7.Visible = false;
            textBox8.Visible = false;
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBoxid.Text = "";
            dataGridView1.Rows.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clear();
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            textBoxid.Visible = true;
            labelid.Visible = true;
            int c;
            if (fb.State == ConnectionState.Closed)
                fb.Open();                        
            table = comboBox1.Text;
            switch (table)
                {
                case "CITYS":
                    textBox2.Visible = true;
                    flag = 1;
                    break;
                case "CLASSES":
                    textBox2.Visible = true;
                    textBox3.Visible = true;
                    textBox4.Visible = true;
                    textBox5.Visible = true;
                    flag = 4;
                    break;
                case "EATING":
                    textBox2.Visible = true;
                    textBox3.Visible = true;
                    flag = 2;
                    break;
                case "FLIGHTS":
                    textBox2.Visible = true;
                    textBox3.Visible = true;
                    textBox4.Visible = true;
                    textBox5.Visible = true;
                    textBox6.Visible = true;
                    textBox7.Visible = true;
                    textBox8.Visible = true;
                    flag = 7;
                    break;
                case "PLANES":
                    textBox2.Visible = true;
                    textBox3.Visible = true;
                    textBox4.Visible = true;
                    textBox5.Visible = true;
                    flag = 4;
                    break;
                case "TRAKES":
                    textBox2.Visible = true;
                    textBox3.Visible = true;
                    flag = 2;
                    break;
                case "PASSENGERS":
                    textBox2.Visible = true;
                    textBox3.Visible = true;
                    textBox4.Visible = true;
                    textBox5.Visible = true;
                    textBox6.Visible = true;
                    flag = 5;
                    break;
            }

            FbTransaction fbt = fb.BeginTransaction(); //стартуем транзакцию; стартовать транзакцию можно только для открытой базы (т.е. мутод Open() уже был вызван ранее, иначе ошибка)
            FbCommand SelectSQL;
            if (table == "PASSENGERS")
                SelectSQL = new FbCommand(String.Format(@"SELECT * FROM PASSENGERR"), fb);
            else
                SelectSQL = new FbCommand(String.Format(@"SELECT * FROM {0}", table), fb); //задаем запрос на выборку

            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
            FbDataReader reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()
            
            string select_result = ""; //в эту переменную будем складывать результат запроса Select                       

            try
            {
                while (reader.Read()) //пока не прочли все данные выполняем...
                {
                    c = reader.FieldCount;
                    string[] SQLM = new string[c];
                    dataGridView1.ColumnCount = c;                    
                        for (int i = 0; i < c; i++)
                        {
                            select_result = reader.GetName(i);
                            dataGridView1.Columns[i].HeaderText = select_result;
                        }
                    select_result = "";
                    for (int i = 0; i < c; i++) { 
                        SQLM[i] = reader.GetString(i);                        
                        //dataGridView1[i, 0].Value = select_result;                        
                    }
                    dataGridView1.Rows.Add(SQLM);

                }
            }
            finally
            {
                //всегда необходимо вызывать метод Close(), когда чтение данных завершено
                reader.Close();
                fb.Close();
            }
            SelectSQL.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string[] text = { textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text};
            string execute = "execute procedure ADD_" + table + "(";
            for (int i = 0; i < flag; i++)
            {
                execute += "'" + text[i] + "',";
            }
            execute = execute.Remove(execute.Length - 1, 1);
            execute += ");";
            if (fb.State == ConnectionState.Closed)
                fb.Open();
            FbTransaction fbt = fb.BeginTransaction(); //стартуем транзакцию; стартовать транзакцию можно только для открытой базы (т.е. мутод Open() уже был вызван ранее, иначе ошибка)

            FbCommand ExecuteSQL = new FbCommand(execute, fb);
            ExecuteSQL.Transaction = fbt;
            try
            {
                int res = ExecuteSQL.ExecuteNonQuery(); //для запросов, не возвращающих набор данных (insert, update, delete) надо вызывать этот метод
                MessageBox.Show("SUCCESS: " + res.ToString());
                fbt.Commit(); //если вставка прошла успешно - комитим транзакцию
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ExecuteSQL.Dispose();

            FbCommand SelectSQL = new FbCommand(String.Format(@"SELECT * FROM {0} f", table), fb); //задаем запрос на выборку

            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
            FbDataReader reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()

            string select_result = ""; //в эту переменную будем складывать результат запроса Select                       

            try
            {
                while (reader.Read()) //пока не прочли все данные выполняем...
                {
                    int c = reader.FieldCount;
                    string[] SQLM = new string[c];
                    dataGridView1.ColumnCount = c;
                    for (int i = 0; i < c; i++)
                    {
                        select_result = reader.GetName(i);
                        dataGridView1.Columns[i].HeaderText = select_result;
                    }
                    select_result = "";
                    for (int i = 0; i < c; i++)
                    {
                        SQLM[i] = reader.GetString(i);                      
                    }
                    dataGridView1.Rows.Add(SQLM);

                }
            }
            finally
            {
                //всегда необходимо вызывать метод Close(), когда чтение данных завершено
                reader.Close();
                fb.Close();
            }
            SelectSQL.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string[] text = { textBoxid.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text };
            string execute = "execute procedure UPDATE_" + table + "(";
            for (int i = 0; i < flag + 1; i++)
            {
                execute += "'" + text[i] + "',";
            }
            execute = execute.Remove(execute.Length - 1, 1);
            execute += ");";
            if (fb.State == ConnectionState.Closed)
                fb.Open();
            FbTransaction fbt = fb.BeginTransaction(); //стартуем транзакцию; стартовать транзакцию можно только для открытой базы (т.е. мутод Open() уже был вызван ранее, иначе ошибка)

            FbCommand ExecuteSQL = new FbCommand(execute, fb);
            ExecuteSQL.Transaction = fbt;
            try
            {
                int res = ExecuteSQL.ExecuteNonQuery(); //для запросов, не возвращающих набор данных (insert, update, delete) надо вызывать этот метод
                MessageBox.Show("SUCCESS: " + res.ToString());
                fbt.Commit(); //если вставка прошла успешно - комитим транзакцию
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            FbCommand SelectSQL = new FbCommand(String.Format(@"SELECT * FROM {0} f", table), fb); //задаем запрос на выборку

            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
            FbDataReader reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()

            string select_result = ""; //в эту переменную будем складывать результат запроса Select                       

            try
            {
                while (reader.Read()) //пока не прочли все данные выполняем...
                {
                    int c = reader.FieldCount;
                    string[] SQLM = new string[c];
                    dataGridView1.ColumnCount = c;
                    for (int i = 0; i < c; i++)
                    {
                        select_result = reader.GetName(i);
                        dataGridView1.Columns[i].HeaderText = select_result;
                    }
                    select_result = "";
                    for (int i = 0; i < c; i++)
                    {
                        SQLM[i] = reader.GetString(i);
                    }
                    dataGridView1.Rows.Add(SQLM);

                }
            }
            finally
            {
                //всегда необходимо вызывать метод Close(), когда чтение данных завершено
                reader.Close();
                fb.Close();
            }
            SelectSQL.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string[] text = { textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text };
            string execute = "execute procedure DELETE_" + table + "('" + textBoxid.Text + "');";
            if (fb.State == ConnectionState.Closed)
                fb.Open();
            FbTransaction fbt = fb.BeginTransaction(); //стартуем транзакцию; стартовать транзакцию можно только для открытой базы (т.е. мутод Open() уже был вызван ранее, иначе ошибка)

            FbCommand ExecuteSQL = new FbCommand(execute, fb);
            ExecuteSQL.Transaction = fbt;
            try
            {
                int res = ExecuteSQL.ExecuteNonQuery(); //для запросов, не возвращающих набор данных (insert, update, delete) надо вызывать этот метод
                MessageBox.Show("SUCCESS: " + res.ToString());
                fbt.Commit(); //если вставка прошла успешно - комитим транзакцию
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            FbCommand SelectSQL = new FbCommand(String.Format(@"SELECT * FROM {0} f", table), fb); //задаем запрос на выборку

            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
            FbDataReader reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()

            string select_result = ""; //в эту переменную будем складывать результат запроса Select                       

            try
            {
                while (reader.Read()) //пока не прочли все данные выполняем...
                {
                    int c = reader.FieldCount;
                    string[] SQLM = new string[c];
                    dataGridView1.ColumnCount = c;
                    for (int i = 0; i < c; i++)
                    {
                        select_result = reader.GetName(i);
                        dataGridView1.Columns[i].HeaderText = select_result;
                    }
                    select_result = "";
                    for (int i = 0; i < c; i++)
                    {
                        SQLM[i] = reader.GetString(i);
                    }
                    dataGridView1.Rows.Add(SQLM);

                }
            }
            finally
            {
                //всегда необходимо вызывать метод Close(), когда чтение данных завершено
                reader.Close();
                fb.Close();
            }
            SelectSQL.Dispose();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxid.Text = dataGridView1[0, e.RowIndex].Value.ToString();

        }
    }
}
