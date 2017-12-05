using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Calculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            result.Text = 0.ToString();
        }
        private void AddNumberToResut(double number)
        {
            if (char.IsNumber(result.Text.Last()))
              {
                if (result.Text.Length == 1 && result.Text == "0")
                {
                    result.Text = String.Empty;
                }
                result.Text += number;
              }
            else
            {
                if(number != 0)
                {
                    result.Text += number;
                }
            }
        }//addnumber
        enum Operation {MINUS = 1, PLUS = 2 , DIV = 3 , TIMES = 4 , NUMBER = 5 }
        private void addOperationToResult(Operation operation)
        {
            if (result.Text.Length == 1 && result.Text == "0") return;
            if (!char.IsNumber(result.Text.Last()))
                {
                result.Text = result.Text.Substring(0, result.Text.Length - 1);
                 }
            switch (operation)
            {
                case Operation.MINUS: result.Text += "-" ;break;
                case Operation.PLUS: result.Text += "+"; break;
                case Operation.DIV: result.Text += "/"; break;
                case Operation.TIMES: result.Text += "*"; break;
            }//switch
        }//addOperationToResult
        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResut(7);
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResut(8);
        }
        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResut(9);
        }
        private void btnDiv_Click(object sender, RoutedEventArgs e)
        {
            addOperationToResult(Operation.DIV);
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResut(4);
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResut(5);
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResut(6);
        }

        private void btnTimes_Click(object sender, RoutedEventArgs e)
        {
            addOperationToResult(Operation.TIMES);
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResut(1);
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResut(2);
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResut(3);
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            addOperationToResult(Operation.MINUS);
        }
        #region Equal
        private class Operand
        {
            public Operation operation = Operation.NUMBER;
            public double value = 0;
            public Operand left = null;
            public Operand right = null;
        }//class

        //Get expression from result.Text and build a tree from it
        private Operand BuildTreeOperand()
        {
            Operand tree = null;
            string expression = result.Text;
            if (!char.IsNumber(expression.Last()))
            {
                expression = expression.Substring(0, expression.Length - 1);
            }//if statement
            string numberStr = string.Empty;
            foreach (char c in expression.ToCharArray())
            {
                if(char.IsNumber(c) || c ==  '.' || numberStr == string.Empty && c == '.')
                    {
                    numberStr += c;
                    }
                else
                {
                    AddOperandTOTree(ref tree, new Operand() { value = double.Parse(numberStr)});
                    numberStr = string.Empty;
                    Operation op = Operation.MINUS;//default
                    switch(c)
                    {
                        case '-': op = Operation.MINUS; break;
                        case '+': op = Operation.PLUS; break;
                        case '/': op = Operation.DIV; break;
                        case '*': op = Operation.TIMES; break;
                    }
                    AddOperandTOTree(ref tree, new Operand() { operation = op });
                }
            }
            //Last number
            AddOperandTOTree(ref tree, new Operand() { value = double.Parse(numberStr) });
            return tree;
        }//BuildTreeOperand
        private void AddOperandTOTree(ref Operand tree, Operand elem)
        {
            if (tree == null)
            {
                tree = elem;
            }
            else
            {
                if (elem.operation < tree.operation)
                {
                    Operand auxTree = tree;
                    tree = elem;
                    elem.left = auxTree;
                }
                else
                {
                    AddOperandTOTree(ref tree.right, elem); //recursive
                }
            }
        }
        private double Calc(Operand tree)
        {
            if(tree.left == null && tree.right == null) //it's a number
            {
                return tree.value;
            }
            else
            {   //it's an operation (-,+,/,*)
                double subResult = 0;
                switch (tree.operation)
                {
                    case Operation.MINUS: subResult = Calc(tree.left) - Calc(tree.right);break;//recrusive
                    case Operation.PLUS: subResult = Calc(tree.left) + Calc(tree.right); break;
                    case Operation.DIV: subResult = Calc(tree.left) / Calc(tree.right); break;
                    case Operation.TIMES: subResult = Calc(tree.left) * Calc(tree.right); break;
                }
                return subResult;
            }
        }
        private void btnEqual_Click(object sender, RoutedEventArgs e)
        {   //GATE
            if (string.IsNullOrEmpty(result.Text)) return;
            Operand tree = BuildTreeOperand();//from string in reult.text
            double value = Calc(tree);//evaluate tree to calculate final result
            result.Text = value.ToString();
        }
        #endregion Equal
        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResut(0);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            result.Text = 0.ToString();
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            addOperationToResult(Operation.PLUS);

        }
    }
}
