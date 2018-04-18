using System;
using System.Windows.Forms;

namespace ZRSUBDKR
{
    public partial class Form1 : Form    {
        
        Form2 form2 = new Form2();
        Form3 form3 = new Form3();
        Form4 form4 = new Form4();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!form2.Visible)
                form2.Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!form3.Visible)
                form3.Show();
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int f = 0;
            button1.Visible = false;
            button2.Visible = false;
            button3.Text = "Ввод";
            textBox1.Visible = true;
            textBox2.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            if ((textBox1.Text == "admin") && (textBox2.Text == "admin"))
                f = 1;
            if ((!form4.Visible) && (f == 1))
            {
                form4.Show();
                Hide();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
