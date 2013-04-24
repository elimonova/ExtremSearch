using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ExtremSearch
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            textBox1.Text = saveFileDialog1.FileName;
        }
        public void addBeeData(string[] args)
        {
            dataGridView1.Rows.Add(args);
        }
        public void addFireflyData(string[] args)
        {
            dataGridView2.Rows.Add(args);
        }
        public void addSimAnnealingData(string[] args)
        {
            dataGridView3.Rows.Add(args);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                StreamWriter dataOut = new StreamWriter(textBox1.Text);

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        dataOut.Write(dataGridView1.Rows[i].Cells[j].Value.ToString());
                        dataOut.Write(" ");
                    }
                    dataOut.WriteLine();
                }
                dataOut.WriteLine();
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView2.Columns.Count; j++)
                    {
                        dataOut.Write(dataGridView2.Rows[i].Cells[j].Value.ToString());
                        dataOut.Write(" ");
                    }
                    dataOut.WriteLine();
                }
                dataOut.WriteLine();
                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView3.Columns.Count; j++)
                    {
                        dataOut.Write(dataGridView3.Rows[i].Cells[j].Value.ToString());
                        dataOut.Write(" ");
                    }
                    dataOut.WriteLine();
                }
                dataOut.Close();
            }
            catch
            {
                textBox1.Text = "Invalid file";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.Remove(row);
            }
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                dataGridView2.Rows.Remove(row);
            }
            return;
        }
    }
}
