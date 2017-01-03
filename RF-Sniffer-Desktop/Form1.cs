using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;


namespace List_RCMDS
{
    public partial class Form1 : Form
    {
        SerialPort port;
        Thread thSerial;
        Dictionary<int, int> codes = new Dictionary<int, int>();
        bool newData = true;
        public Form1()
        {
            InitializeComponent();

            dataGridView1.Columns.Add("Code", "Code");
            dataGridView1.Columns.Add("CNT", "CNT");

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                port = new SerialPort("COM" + numericUpDown1.Value, 9600);
                port.Open();
                thSerial = new Thread(new ThreadStart(delegate
                {
                    while (true)
                    {
                        string input = "";
                        try
                        {
                            while (port.BytesToRead == 0) ;
                            while (port.BytesToRead > 0)
                            {
                                input += port.ReadExisting();
                                Thread.Sleep(50);
                            }
                        }
                        catch (Exception ex)
                        { }
                        if (input != "")
                        {
                            int offset = 4;
                            List<object> ints = Tools.Strings.encode<int>(input, ';', ref offset);
                            foreach(int c in ints)
                            {
                                if (codes.ContainsKey(c))
                                    codes[c]++;
                                else
                                    codes.Add(c, 1);
                            }
                            newData = true;

                        }

                    }
                }));
                thSerial.IsBackground = true;
                thSerial.Start();

            }
            catch
            { }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if(port!=null)
            port.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (newData)
            {
                newData = false;
                while (dataGridView1.Rows[0].IsNewRow == false)
                    dataGridView1.Rows.RemoveAt(0);

                for (int i = 0; i < codes.Count; i++)
                {
                    dataGridView1.Rows.Add(codes.Keys.ElementAt(i), codes.Values.ElementAt(i));
                }
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
    }
}