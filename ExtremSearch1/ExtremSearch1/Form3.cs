using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtremSearch
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            while(true)
            {
                progressBar1.Value++;
                if (progressBar1.Value == 100)
                {
                    progressBar1.Value = 0;
                }
            }
        }
    }
}
