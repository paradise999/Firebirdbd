using System;
using System.Windows.Forms;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Globalization;

namespace ZRSUBDKR
{
    public partial class Form3 : Form
    {
        FbConnection fb;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
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

        private void Form3_Load(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (fb.State == ConnectionState.Closed)
                fb.Open();

            FbTransaction fbt = fb.BeginTransaction(); //стартуем транзакцию; стартовать транзакцию можно только для открытой базы (т.е. мутод Open() уже был вызван ранее, иначе ошибка)

            FbCommand SelectSQL = new FbCommand(@"SELECT * FROM FLIGHTS_V", fb); //задаем запрос на выборку

            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
            FbDataReader reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()
            dataGridView1.ColumnCount = 6;
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
    }
}
