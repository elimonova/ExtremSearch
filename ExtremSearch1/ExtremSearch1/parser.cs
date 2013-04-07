using System;
using System.Collections;
using System.Collections.Generic;

namespace Parser
{
	public class PostfixNotationExpression
    {
        public PostfixNotationExpression()
        {
            operators = new List<string>(standart_operators);
			operators.AddRange(additional_operators);
        }
        public PostfixNotationExpression(string input)
        {
            operators = new List<string>(standart_operators);
            operators.AddRange(additional_operators);
            functionQueue = new Queue<string>(ConvertToPostfixNotation(input));
        }
        private Queue<string> functionQueue;
        private List<string> operators;
        private List<string> additional_operators =
            new List<string>(new string[] { "n", "p", "abs", "sin", "cos", "tan", "asin", "acos", "atan", "sqrt", "ceil", "E", 
			                                "sign", "exp", "PI", "floor", "ln", "log10" });
 
		private List<string> standart_operators =
            new List<string>(new string[] { "(", ")", "+", "-", "*", "/", "^"});
 
        private IEnumerable<string> Separate(string input)
        {
	        int pos = 0;
			string prev = string.Empty;
            while (pos < input.Length)
            {
                if (Char.IsSeparator(input[pos]))
				{
					pos++;
					continue;
				}
				if (input[pos].Equals('-') && ((prev == string.Empty) || (standart_operators.Contains(prev) && prev != ")")))
				{
					input = input.Remove(pos, 1);
					input = input.Insert(pos, "n");
				}
				if (input[pos].Equals('+') && ((prev == string.Empty) || (standart_operators.Contains(prev) && prev != ")")))
				{
					input = input.Remove(pos, 1);
					input = input.Insert(pos, "p");
				}
				
				string s = string.Empty + input[pos];
                if (!operators.Contains(input[pos].ToString()))
                {
                    if (Char.IsDigit(input[pos]))
					{
						for (int i = pos + 1; i < input.Length &&
                            (Char.IsDigit(input[i]) || input[i] == ',' || input[i] == '.'); i++)
						{
							s += input[i];
						}
					}
                    else if (Char.IsLetter(input[pos]))
					{
						for (int i = pos + 1; i < input.Length &&
                            (Char.IsLetter(input[i]) || Char.IsDigit(input[i])); i++)
						{
							s += input[i];
						}
					}
                }
				yield return s;
                prev = s;
				pos += s.Length;
            }
        }
        private byte GetPriority(string s)
        {
            switch (s)
            {
                case "(":
                    return 0;
				case ")":
                    return 1;
                case "+":
                    return 2;
                case "-":
                    return 3;
                case "*":
                case "/":
                    return 4;
                case "^":
                    return 5;
                default:
                    return 6;
            }
        }
 
        public string[] ConvertToPostfixNotation(string input)
        {
            List<string> outputSeparated = new List<string>();
            Stack<string> stack = new Stack<string>();
            foreach (string c in Separate(input))
            {
                if (operators.Contains(c))
                {
                    if (stack.Count > 0 && !c.Equals("("))
                    {
                        if (c.Equals(")"))
                        {
                            string s = stack.Pop();
                            while (s != "(")
                            {
                                outputSeparated.Add(s);
                                s = stack.Pop();
                            }
                        }
                        else if (GetPriority(c) > GetPriority(stack.Peek()))
						{
							stack.Push(c);
						}
						else
                        {
                            while (stack.Count > 0 && GetPriority(c) <= GetPriority(stack.Peek()))
							{
								outputSeparated.Add(stack.Pop());
							}
							stack.Push(c);
                        }
                    }
                    else
					{
                        stack.Push(c);
					}
                }
                else
				{
                    outputSeparated.Add(c);
				}
			}
            if (stack.Count > 0)
			{
                foreach (string c in stack)
				{ 
					outputSeparated.Add(c);
				}
			}
            return outputSeparated.ToArray();
        }
        public double result(int argCnt, double[] x)
        {
            Stack<string> stack = new Stack<string>();
            Queue<string> queue = new Queue<string>(functionQueue);
            string str = queue.Dequeue();
            while (queue.Count >= 0)
            {
				Console.WriteLine(str);
				if (!operators.Contains(str))
                {
                    if (str[0].Equals('x'))
					{
						int index = Convert.ToInt32(str.Substring (1, str.Length - 1));
						stack.Push(x[index - 1].ToString());
                    }
					else
					{
						stack.Push (str);
					}
					str = queue.Dequeue();
                }
                else
                {
                    double summ = 0;
         //           try
         //           {

                        switch (str)
                        {
 
                            case "+":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    double b = Convert.ToDouble(stack.Pop());
                                    summ = a + b;
                                    break;
                                }
                            case "-":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    double b = Convert.ToDouble(stack.Pop());
                                    summ = b - a;
                                    break;
                                }
                            case "*":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    double b = Convert.ToDouble(stack.Pop());
                                    summ = b * a;
                                    break;
                                }
                            case "/":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    double b = Convert.ToDouble(stack.Pop());
                                    summ = b / a;
                                    break;
                                }
                            case "^":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    double b = Convert.ToDouble(stack.Pop());
                                    summ = Math.Pow(b, a);
                                    break;
                                }
							case "abs":
								{
									double a = Convert.ToDouble(stack.Pop ());
									summ = Math.Abs(a);
									break;
								}
							case "n":
								{
									summ = - Convert.ToDouble(stack.Pop ());
									break;
                       			}
							case "p":
							{
								summ = Convert.ToDouble(stack.Pop ());
								break;
							}
							case "sin":
							{
								double a = Convert.ToDouble(stack.Pop ());
								summ = Math.Sin(a);
								break;
							}
							case "cos":
							{
								double a = Convert.ToDouble(stack.Pop ());
								summ = Math.Cos(a);
								break;
							}
							case "tan":
							{
								double a = Convert.ToDouble(stack.Pop ());
								summ = Math.Tan(a);
								break;
							}
							case "asin":
							{
								double a = Convert.ToDouble(stack.Pop ());
								summ = Math.Asin(a);
								break;
							}
							case "acos":
							{
								double a = Convert.ToDouble(stack.Pop ());
								summ = Math.Acos(a);
								break;
							}
							case "atan":
							{
								double a = Convert.ToDouble(stack.Pop ());
								summ = Math.Atan(a);
								break;
							}
							case "sqrt":
							{
								double a = Convert.ToDouble(stack.Pop ());
								summ = Math.Sqrt(a);
								break;
							}
							case "ceil":
							{
								double a = Convert.ToDouble(stack.Pop ());
								summ = Math.Ceiling(a);
								break;
							}
							case "E":
							{
								summ = Math.E;
								break;
							}
							case "sign":
							{
								double a = Convert.ToDouble(stack.Pop ());
								summ = Math.Sign(a);
								break;
							}
							case "exp":
							{
								double a = Convert.ToDouble(stack.Pop ());
								summ = Math.Exp(a);
								break;
							}
							case "PI":
							{
								summ = Math.PI;
								break;
							}
							case "floor":
							{
								double a = Convert.ToDouble(stack.Pop ());
								summ = Math.Floor(a);
								break;
							}
							case "ln":
							{
								double a = Convert.ToDouble(stack.Pop ());
								summ = Math.Log(a);
								break;
							}
							case "log10":
							{
								double a = Convert.ToDouble(stack.Pop ());
								summ = Math.Log10(a);
								break;
							}
							default:
							{
								throw new System.FormatException();
							}
						}
       //             }
        //            catch (Exception ex)
       //             {
       //                 ExtremSearch.Form1.throwError("Функция некорректна");   
       //             }
                    stack.Push(summ.ToString());
                    if (queue.Count > 0)
					{
                        str = queue.Dequeue();
					}
					else
					{
						break;
					}
                }
 
            }
            return Convert.ToDouble(stack.Pop());
        }
    }

    /*
	class MainClass
	{
		public static void Main (string[] args)
		{
			string inputExpr = Console.ReadLine();
			PostfixNotationExpression expr = new PostfixNotationExpression();
			string[] outData = expr.ConvertToPostfixNotation(inputExpr);
			foreach(string s in outData)
			{
				Console.WriteLine(s);
			}
			Console.WriteLine("All");
			int[] x = new int [2];
			x[0] = 1;
			x[1] = 1;
			decimal res = expr.result(inputExpr, x);
			Console.WriteLine (res);
		}
	}*/
}
