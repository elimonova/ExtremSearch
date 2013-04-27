using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace ExtremSearch
{
    public partial class Form1 : Form
    {
        Form3 myForm3 = new Form3();
        Form4 progressBar = new Form4();
              
        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == 0)
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;
            }
            if (listBox1.SelectedIndex == 1)
            {
                panel1.Visible = false;
                panel2.Visible = true;
                panel3.Visible = false;
            }
            if (listBox1.SelectedIndex == 2)
            {
                panel1.Visible = false;
                panel2.Visible = false;
                panel3.Visible = true;
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            radioButton2.Checked = false;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
        }
        private void StartBeeMethod()
        {
            int Bs = Convert.ToInt32(textBox1.Text);
            if (Bs <= 0)
            {
                throwError("Bs должно быть положительным целым числом");
                return;
            }
            double alpha = Convert.ToDouble(textBox2.Text);
            if (alpha <= 0)
            {
                throwError("alpha должно быть положительным числом");
                return;
            }
            double TInit = Convert.ToDouble(textBox3.Text);
            if (TInit <= 0)
            {
                throwError("TInit должно быть положительным числом");
                return;
            }
            double TFinal = Convert.ToDouble(textBox4.Text);
            if (TFinal <= 0)
            {
                throwError("TFinal должно быть положительным числом");
                return;
            }
            int maxIter = Convert.ToInt32(textBox5.Text);
            if (maxIter <= 0)
            {
                throwError("Max Iteration должно быть положительным целым числом");
                return;
            }
            double w = Convert.ToDouble(textBox6.Text);
            if (w < 0)
            {
                throwError("w должно быть неотрицательным числом");
                return;
            }
            double eps = Convert.ToDouble(textBox7.Text);
            if (eps < 0)
            {
                throwError("eps должно быть неотрицательным числом");
                return;
            }
            double eta = Convert.ToDouble(textBox8.Text);
            if (eta < 0)
            {
                throwError("eta должно быть неотрицательным числом");
                return;
            }
            double beta = Convert.ToDouble(textBox9.Text);
            if (beta < 0)
            {
                throwError("beta должно быть неотрицательным числом");
                return;
            }
            double gamma = Convert.ToDouble(textBox10.Text);
            if (gamma < 0 || gamma > 1)
            {
                throwError("gamma должно быть в промежутке [0, 1]");
                return;
            }
            double range = Convert.ToDouble(textBox11.Text);
            if (range <= 0)
            {
                throwError("range должно быть неотрицательным числом");
                return;
            }
            int argCnt = Convert.ToInt32(textBox12.Text);
            if (argCnt <= 0)
            {
                throwError("n должно быть положительным числом");
                return;
            }
            double[] rangeMin = getPointFromString(textBox14.Text, argCnt).ToArray();
            double[] rangeMax = getPointFromString(textBox15.Text, argCnt).ToArray();
            Parser.PostfixNotationExpression myFunc = new Parser.PostfixNotationExpression(textBox13.Text);
            int optOrient;
            if (radioButton1.Checked)
            {
                optOrient = 1;
            }
            else if (radioButton2.Checked)
            {
                optOrient = -1;
            }
            else
            {
                throwError("Выберите Max или Min");
                return;
            }
            BeeMethod.BeeMethod myBeeMethod = new BeeMethod.BeeMethod(Bs,         alpha, TInit, TFinal, argCnt, maxIter, rangeMin,
                                                                      rangeMax,       w,   eps,    eta,   beta,   gamma,    range,
                                                                      optOrient, myFunc);
            double[] res = myBeeMethod.search();
            addResultToLabel20(argCnt, res, myFunc);
            return;
        }
        private void StartFireflyMethod()
        {
            int B = Convert.ToInt32(textBox16.Text);
            if (B <= 0)
            {
                throwError("B должно быть положительным целым числом");
                return;
            }
            double alpha = Convert.ToDouble(textBox19.Text);
            if (alpha <= 0)
            {
                throwError("alpha должно быть положительным числом");
                return;
            }
            int maxIter = Convert.ToInt32(textBox18.Text);
            if (maxIter <= 0)
            {
                throwError("Max Iteration должно быть положительным целым числом");
                return;
            }
            double beta = Convert.ToDouble(textBox20.Text);
            if (beta < 0)
            {
                throwError("beta должно быть неотрицательным числом");
                return;
            }
            double gamma = Convert.ToDouble(textBox17.Text);
            if (gamma < 0 || gamma > 1)
            {
                throwError("gamma должно быть в промежутке [0, 1]");
                return;
            }
            int argCnt = Convert.ToInt32(textBox12.Text);
            if (argCnt <= 0)
            {
                throwError("n должно быть положительным числом");
                return;
            }
            double[] rangeMin = getPointFromString(textBox14.Text, argCnt).ToArray();
            double[] rangeMax = getPointFromString(textBox15.Text, argCnt).ToArray();
            Parser.PostfixNotationExpression myFunc = new Parser.PostfixNotationExpression(textBox13.Text);
            int optOrient;
            if (radioButton1.Checked)
            {
                optOrient = 1;
            }
            else if (radioButton2.Checked)
            {
                optOrient = -1;
            }
            else
            {
                throwError("Выберите Max или Min");
                return;
            }
      
            FireflyMethod.FireflyMethod myFireflyMethod = new FireflyMethod.FireflyMethod(B, argCnt, rangeMin, rangeMax, gamma, 
                                                                                          maxIter, optOrient, alpha, beta, myFunc);
            double[] res = myFireflyMethod.search();
            addResultToLabel20(argCnt, res, myFunc);
            return;
        }
        private void addResultToLabel20(int argCnt, double[] res, Parser.PostfixNotationExpression myFunc)
        {
            string result = "f(";
            for (int i = 0; i < res.Length - 1; i++)
            {
                result += res[i].ToString() + ", ";
            }
            result += res[res.Length - 1].ToString() + ") = ";
            result += myFunc.result(argCnt, res).ToString();
            if (Double.IsNaN(myFunc.result(argCnt, res)))
            {
                throwError("Функция не определена на заданном промежутке");
                return;
            }
            label20.Text = result;
            progressBar.Hide();
        }
        private void StartSimAnnealing()
        {
            double T = Convert.ToDouble(textBox21.Text);
            if (T <= 0)
            {
                throwError("T должно быть больше 0");
                return;
            }
            double alpha = Convert.ToDouble(textBox23.Text);
            if (alpha <= 0 || alpha >= 1)
            {
                throwError("alpha должно быть в пределах (0, 1)");
                return;
            }
            int maxIter = Convert.ToInt32(textBox22.Text);
            if (maxIter <= 0)
            {
                throwError("Max Iteration должно быть положительным целым числом");
                return;
            }
            int argCnt = Convert.ToInt32(textBox12.Text);
            if (argCnt <= 0)
            {
                throwError("n должно быть положительным числом");
                return;
            }
            double[] rangeMin = getPointFromString(textBox14.Text, argCnt).ToArray();
            double[] rangeMax = getPointFromString(textBox15.Text, argCnt).ToArray();
            Parser.PostfixNotationExpression myFunc = new Parser.PostfixNotationExpression(textBox13.Text);
            int optOrient;
            if (radioButton1.Checked)
            {
                optOrient = 1;
            }
            else if (radioButton2.Checked)
            {
                optOrient = -1;
            }
            else
            {
                throwError("Выберите Max или Min");
                return;
            }
            SimAnnealing.SimAnnealing mySimAnnealingMethod = new SimAnnealing.SimAnnealing(T, argCnt, rangeMin, rangeMax, 
                                                                                           maxIter, optOrient, alpha, myFunc);
            double[] res = mySimAnnealingMethod.search();
            addResultToLabel20(argCnt, res, myFunc);
            return;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                if (listBox1.SelectedIndex == 0) // Bee Method
                {
                    Thread progress = new Thread(StartBeeMethod);
                    progress.Start();
                    progressBar.ShowDialog();
                }
                if (listBox1.SelectedIndex == 1) // Firefly Method
                {
                    Thread progress = new Thread(StartFireflyMethod);
                    progress.Start();
                    progressBar.ShowDialog();      
                }
                if (listBox1.SelectedIndex == 2) // Sim Annealing
                {
                    Thread progress = new Thread(StartSimAnnealing);
                    progress.Start();
                    progressBar.ShowDialog();      
                }
                    
            }
            catch (System.FormatException)
            {
                throwError("Некорректные параметры");
            }
            catch (System.OverflowException)
            {
                throwError("Параметры слишком большие");
            }
            catch (System.DivideByZeroException)
            {
                throwError("Функция не определена на заданном промежутке");
            }
            catch (System.OutOfMemoryException)
            {
                throwError("Не хватает памяти для завершения операции");
            }
            return;
        }
        private IEnumerable<double> getPointFromString(string input, int argCnt)
        {
            string[] result = input.Split(',');
            if (result.Length != argCnt)
            {
                throw new System.FormatException();
            }
            for (int i = 0; i < result.Length; i++)
            {
                yield return Convert.ToDouble(result[i]);
            }
        }
        public void throwError(string output)
        {
            Form2 errorWindow = new Form2(output);
            errorWindow.ShowInTaskbar = false;
            errorWindow.StartPosition = FormStartPosition.CenterScreen;
            errorWindow.ShowDialog(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            myForm3.ShowDialog(this);
            this.Visible = true;
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            string orient;
            if (radioButton1.Checked)
            {
                orient = "Max";
            }
            else if (radioButton2.Checked)
            {
                orient = "Min";
            }
            else
            {
                throwError("Выберите Min или Max");
                return;
            }
            if (listBox1.SelectedIndex == 0)
            {
                if (label20.Text != string.Empty)
                {
                    string[] args = { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text, textBox11.Text, 
                                      textBox12.Text, textBox13.Text, textBox14.Text, textBox15.Text, orient, label20.Text };
                    myForm3.addBeeData(args);
                }
            }
            if (listBox1.SelectedIndex == 1)
            {
                if (label20.Text != string.Empty)
                {
                    string[] args = { textBox16.Text, textBox17.Text, textBox18.Text, textBox19.Text, textBox20.Text, textBox12.Text, textBox13.Text, textBox14.Text, textBox15.Text, orient, label20.Text};
                    myForm3.addFireflyData(args);
                }
            }
            if (listBox1.SelectedIndex == 2)
            {
                if (label20.Text != string.Empty)
                {
                    string[] args = { textBox21.Text, textBox22.Text, textBox23.Text, textBox12.Text, textBox13.Text, textBox14.Text, textBox15.Text, orient, label20.Text };
                    myForm3.addSimAnnealingData(args);
                }           
            }
            if (listBox1.SelectedIndex == 3)
            {

            }
        }
        private string getSearchString(string func)
        {
            string res = string.Empty;
            byte[] currSymbol = Encoding.ASCII.GetBytes(func); 
            for (int i = 0; i < func.Length; i++)
            {
                if (Char.IsLetterOrDigit(func[i]))
                {
                    res += func[i];
                }
                else
                {
                    res += "%" + String.Format("{0:X}", currSymbol[i]);
                }
            }
            res = res.Replace("x1", "x");
            res = res.Replace("x2", "y");
            return res;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox13.Text != String.Empty)
                {
                    int n = Convert.ToInt32(textBox12.Text);
                    if (n != 1 && n != 2)
                    {
                        throwError("Можно построить функцию только 1 или 2 переменных");
                        return;
                    }
                    Process.Start("firefox.exe", "google.ru/search?q="+getSearchString(textBox13.Text));
           
                }
                else
                {
                    throwError("Введите функцию");
                    return;
                }
            }
            catch
            {
                throwError("Ошибка при открытии Firefox");
            }
        }

    }
}
