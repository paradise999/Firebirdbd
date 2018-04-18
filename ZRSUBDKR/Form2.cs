using System;
using System.IO;
using System.Windows.Forms;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace ZRSUBDKR
{
    public partial class Form2 : Form
    {
        FbConnection fb;
        int f = 0;
        string city1, city2, city1id, city2id, clas, clasid, plane, planeid, day, time, place;

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f--;
            switch (f)
            {
                case 0:
                    button2.Visible = false;
                    label1.Text = "Откуда: ";
                    label2.Visible = false;
                    dataGridView1.Rows.Clear();
                    if (fb.State == ConnectionState.Closed)
                        fb.Open();
                    FbTransaction fbt = fb.BeginTransaction();

                    FbCommand SelectSQL = new FbCommand(@"SELECT ID AS ""Номер"", NAME AS ""Откуда"" FROM CITYS", fb); //задаем запрос на выборку

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
                    break;
                case 1:
                    label2.Text = "Куда: ";
                    label3.Visible = false;
                    dataGridView1.Rows.Clear();
                    if (fb.State == ConnectionState.Closed)
                        fb.Open();
                    fbt = fb.BeginTransaction();

                    SelectSQL = new FbCommand(String.Format(@"SELECT c.ID AS ""Номер"", c.NAME AS ""Куда"" FROM FLIGHTS f, CITYS c WHERE f.CITY_FROMM = '{0}' AND c.id = f.CITY_WHEREE;", city1id), fb); //задаем запрос на выборку

                    SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
                    reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()

                    select_result = ""; //в эту переменную будем складывать результат запроса Select                       

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
                    break;
                case 2:
                    label3.Text = "Самолёт: ";
                    label4.Visible = false;
                    dataGridView1.Rows.Clear();
                    if (fb.State == ConnectionState.Closed)
                        fb.Open();
                    fbt = fb.BeginTransaction();

                    SelectSQL = new FbCommand(String.Format(@"SELECT f.PLANE AS ""Номер"", p.NANE AS ""Самолёты"", f.DAYY AS ""День"", f.TIMEE AS ""Время"" FROM FLIGHTS f, PLANES p WHERE f.CITY_FROMM = '{0}' AND f.CITY_WHEREE = '{1}' AND p.ID = f.PLANE;", city1id, city2id), fb); //задаем запрос на выборку

                    SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
                    reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()

                    select_result = ""; //в эту переменную будем складывать результат запроса Select                       

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
                                if (i == 2)
                                    SQLM[i] = reader.GetString(i).Remove(reader.GetString(i).Length - 8, 8);
                                else
                                    if (i == 3)
                                    SQLM[i] = reader.GetString(i).Remove(reader.GetString(i).Length - 3, 3);
                                else
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
                    break;
                case 3:
                    label4.Text = "Класс: ";
                    label5.Visible = false;
                    dataGridView1.Rows.Clear();
                    if (fb.State == ConnectionState.Closed)
                        fb.Open();
                    fbt = fb.BeginTransaction();

                    SelectSQL = new FbCommand(String.Format(@"SELECT ID AS ""Номер"", NAME AS ""Название"" FROM CLASSES"), fb); //задаем запрос на выборку

                    SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
                    reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()

                    select_result = ""; //в эту переменную будем складывать результат запроса Select                       

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

                    break;
                case 4:
                    label5.Text = "Место: ";
                    label6.Visible = false;
                    textBox1.Visible = false;
                    label7.Visible = false;
                    textBox2.Visible = false;
                    label8.Visible = false;
                    textBox3.Visible = false;
                    button1.Visible = false;
                    dataGridView1.Rows.Clear();
                    if (fb.State == ConnectionState.Closed)
                        fb.Open();
                    fbt = fb.BeginTransaction();
                    switch (clas)
                    {
                        case "Бизнес":
                            SelectSQL = new FbCommand(String.Format(@"SELECT PLACE AS ""Место"" FROM PASSENGERS WHERE CLASS = 1"), fb); //задаем запрос на выборку

                            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
                            reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()

                            select_result = ""; //в эту переменную будем складывать результат запроса Select                      
                            string[] free = new string[10];
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
                                    for (int i = 0; i < 10; i++)
                                        for (int j = 0; j < SQLM.Length; j++)
                                            if (i + 1 == Int32.Parse(SQLM[j]))
                                                free[i] = "Занято";
                                }
                                for (int i = 0; i < 10; i++)
                                    if (free[i] == null)
                                        free[i] = (i + 1).ToString();
                                for (int i = 0; i < free.Length; i++)
                                    dataGridView1.Rows.Add(free[i]);
                            }
                            finally
                            {
                                //всегда необходимо вызывать метод Close(), когда чтение данных завершено
                                reader.Close();
                                fb.Close();
                            }
                            SelectSQL.Dispose();
                            break;
                        case "Обычный":
                            SelectSQL = new FbCommand(String.Format(@"SELECT PLACE AS ""Место"" FROM PASSENGERS WHERE CLASS = 2"), fb); //задаем запрос на выборку

                            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
                            reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()

                            select_result = ""; //в эту переменную будем складывать результат запроса Select                      
                            free = new string[10];
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
                                    for (int i = 0; i < 10; i++)
                                        for (int j = 0; j < SQLM.Length; j++)
                                            if (i + 1 == Int32.Parse(SQLM[j]))
                                                free[i] = "Занято";
                                }
                                for (int i = 0; i < 10; i++)
                                    if (free[i] == null)
                                        free[i] = (i + 1).ToString();
                                for (int i = 0; i < free.Length; i++)
                                    dataGridView1.Rows.Add(free[i]);
                            }
                            finally
                            {
                                //всегда необходимо вызывать метод Close(), когда чтение данных завершено
                                reader.Close();
                                fb.Close();
                            }
                            SelectSQL.Dispose();

                            break;
                        case "Эконом":
                            SelectSQL = new FbCommand(String.Format(@"SELECT PLACE AS ""Место"" FROM PASSENGERS WHERE CLASS = 3"), fb); //задаем запрос на выборку

                            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
                            reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()

                            select_result = ""; //в эту переменную будем складывать результат запроса Select                      
                            free = new string[10];
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
                                    for (int i = 0; i < 10; i++)
                                        for (int j = 0; j < SQLM.Length; j++)
                                            if (i + 1 == Int32.Parse(SQLM[j]))
                                                free[i] = "Занято";
                                }
                                for (int i = 0; i < 10; i++)
                                    if (free[i] == null)
                                        free[i] = (i + 1).ToString();
                                for (int i = 0; i < free.Length; i++)
                                    dataGridView1.Rows.Add(free[i]);
                            }
                            finally
                            {
                                //всегда необходимо вызывать метод Close(), когда чтение данных завершено
                                reader.Close();
                                fb.Close();
                            }
                            SelectSQL.Dispose();

                            break;
                    }
                    break;

            }
        }

        string[] text = new string[5];

        private void button1_Click(object sender, EventArgs e)
        {
            string Path = Application.StartupPath.ToString() + "\\Check.txt";
            FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("ФИО: " + textBox1.Text);
            text[0] = textBox1.Text;
            sw.WriteLine("Номер паспорта: " + textBox2.Text);
            sw.WriteLine("Вес груза: " + textBox3.Text);
            text[2] = textBox3.Text;
            sw.WriteLine("Откуда: " + city1);
            sw.WriteLine("Куда: " + city2);
            sw.WriteLine("Самолёт: " + plane);
            text[1] = planeid;
            sw.WriteLine("День: " + day);
            sw.WriteLine("Час: " + time);
            sw.WriteLine("Класс: " + clas);
            text[3] = clasid;
            sw.WriteLine("Место: " + place);
            text[4] = place;
            sw.WriteLine("Спасибо, что пользуетесь услугами нашего Аэропорта");
            sw.Close();
            string execute = "execute procedure ADD_PASSENGERS(";
            for (int i = 0; i < 5; i++)
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
            fb.Close();        
          ExecuteSQL.Dispose();
        }

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
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

            if (fb.State == ConnectionState.Closed)
                fb.Open();
            FbTransaction fbt = fb.BeginTransaction();

            FbCommand SelectSQL = new FbCommand(@"SELECT ID AS ""Номер"", NAME AS ""Откуда"" FROM CITYS", fb); //задаем запрос на выборку

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

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            f++;            
            switch (f)
            {
                case 1:
                    button2.Visible = true;
                    label1.Text += dataGridView1[1, e.RowIndex].Value;
                    city1 = dataGridView1[1, e.RowIndex].Value.ToString();
                    city1id = dataGridView1[0, e.RowIndex].Value.ToString();
                    label2.Visible = true;
                    dataGridView1.Rows.Clear();
                    if (fb.State == ConnectionState.Closed)
                        fb.Open();
                    FbTransaction fbt = fb.BeginTransaction();

                    FbCommand SelectSQL = new FbCommand(String.Format(@"SELECT c.ID AS ""Номер"", c.NAME AS ""Куда"" FROM FLIGHTS f, CITYS c WHERE f.CITY_FROMM = '{0}' AND c.id = f.CITY_WHEREE;", city1id), fb); //задаем запрос на выборку

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
                    break;
                case 2:                    
                    label2.Text += dataGridView1[1, e.RowIndex].Value;
                    city2 = dataGridView1[1, e.RowIndex].Value.ToString();
                    city2id = dataGridView1[0, e.RowIndex].Value.ToString();
                    label3.Visible = true;
                    dataGridView1.Rows.Clear();
                    if (fb.State == ConnectionState.Closed)
                        fb.Open();
                    fbt = fb.BeginTransaction();

                    SelectSQL = new FbCommand(String.Format(@"SELECT f.PLANE AS ""Номер"", p.NANE AS ""Самолёты"", f.DAYY AS ""День"", f.TIMEE AS ""Время"" FROM FLIGHTS f, PLANES p WHERE f.CITY_FROMM = '{0}' AND f.CITY_WHEREE = '{1}' AND p.ID = f.PLANE;", city1id, city2id), fb); //задаем запрос на выборку

                    SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
                    reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()

                    select_result = ""; //в эту переменную будем складывать результат запроса Select                       

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
                                if (i == 2)
                                    SQLM[i] = reader.GetString(i).Remove(reader.GetString(i).Length - 8, 8);
                                else
                                    if (i == 3)
                                    SQLM[i] = reader.GetString(i).Remove(reader.GetString(i).Length - 3, 3);
                                else
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
                    break;
                case 3:
                    label3.Text += dataGridView1[1, e.RowIndex].Value;
                    planeid = dataGridView1[0, e.RowIndex].Value.ToString();
                    plane = dataGridView1[1, e.RowIndex].Value.ToString();
                    day = dataGridView1[2, e.RowIndex].Value.ToString();
                    time = dataGridView1[3, e.RowIndex].Value.ToString();
                    label4.Visible = true;
                    dataGridView1.Rows.Clear();
                    if (fb.State == ConnectionState.Closed)
                        fb.Open();
                    fbt = fb.BeginTransaction();

                    SelectSQL = new FbCommand(String.Format(@"SELECT ID AS ""Номер"", NAME AS ""Название"" FROM CLASSES"), fb); //задаем запрос на выборку

                    SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
                    reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()

                    select_result = ""; //в эту переменную будем складывать результат запроса Select                       

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

                    break;
                case 4:
                    label4.Text += dataGridView1[1, e.RowIndex].Value;
                    clas = dataGridView1[1, e.RowIndex].Value.ToString();
                    clasid = dataGridView1[0, e.RowIndex].Value.ToString();
                    label5.Visible = true;                    
                    dataGridView1.Rows.Clear();
                    if (fb.State == ConnectionState.Closed)
                        fb.Open();
                    fbt = fb.BeginTransaction();
                    switch (clas)
                    {
                        case "Бизнес":
                            SelectSQL = new FbCommand(String.Format(@"SELECT PLACE AS ""Место"" FROM PASSENGERS WHERE CLASS = 1"), fb); //задаем запрос на выборку

                            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
                            reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()

                            select_result = ""; //в эту переменную будем складывать результат запроса Select                      
                            string[] free = new string[10];
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
                                    for (int i = 0; i < 10; i++)
                                        for (int j = 0; j < SQLM.Length; j++)
                                            if (i + 1 == Int32.Parse(SQLM[j]))
                                                free[i] = "Занято";
                                }
                                for (int i = 0; i < 10; i++)
                                    if (free[i] == null)
                                        free[i] = (i + 1).ToString();
                                for (int i = 0; i < free.Length; i++)
                                    dataGridView1.Rows.Add(free[i]);
                            }
                            finally
                            {
                                //всегда необходимо вызывать метод Close(), когда чтение данных завершено
                                reader.Close();
                                fb.Close();
                            }
                            SelectSQL.Dispose();
                            break;
                        case "Обычный":
                            SelectSQL = new FbCommand(String.Format(@"SELECT PLACE AS ""Место"" FROM PASSENGERS WHERE CLASS = 2"), fb); //задаем запрос на выборку

                            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
                            reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()

                            select_result = ""; //в эту переменную будем складывать результат запроса Select                      
                            free = new string[10];
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
                                    for (int i = 0; i < 10; i++)
                                        for (int j = 0; j < SQLM.Length; j++)
                                            if (i + 1 == Int32.Parse(SQLM[j]))
                                                free[i] = "Занято";
                                }
                                for (int i = 0; i < 10; i++)
                                    if (free[i] == null)
                                        free[i] = (i + 1).ToString();
                                for (int i = 0; i < free.Length; i++)
                                    dataGridView1.Rows.Add(free[i]);
                            }
                            finally
                            {
                                //всегда необходимо вызывать метод Close(), когда чтение данных завершено
                                reader.Close();
                                fb.Close();
                            }
                            SelectSQL.Dispose();

                            break;
                        case "Эконом":
                            SelectSQL = new FbCommand(String.Format(@"SELECT PLACE AS ""Место"" FROM PASSENGERS WHERE CLASS = 3"), fb); //задаем запрос на выборку

                            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
                            reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()

                            select_result = ""; //в эту переменную будем складывать результат запроса Select                      
                            free = new string[10];
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
                                    for (int i = 0; i < 10; i++)
                                        for (int j = 0; j < SQLM.Length; j++)
                                            if (i + 1 == Int32.Parse(SQLM[j]))
                                                free[i] = "Занято";
                                }
                                for (int i = 0; i < 10; i++)
                                    if (free[i] == null)
                                        free[i] = (i + 1).ToString();
                                for (int i = 0; i < free.Length; i++)
                                    dataGridView1.Rows.Add(free[i]);
                            }
                            finally
                            {
                                //всегда необходимо вызывать метод Close(), когда чтение данных завершено
                                reader.Close();
                                fb.Close();
                            }
                            SelectSQL.Dispose();

                            break;
                    }                    
                    break;
                case 5:
                    place = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    label5.Text += dataGridView1[e.ColumnIndex, e.RowIndex].Value;
                    label6.Visible = true;
                    textBox1.Visible = true;
                    label7.Visible = true;
                    textBox2.Visible = true;
                    label8.Visible = true;
                    textBox3.Visible = true;
                    button1.Visible = true;
                    break;
            }
        }
    }
}
