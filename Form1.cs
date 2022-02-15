using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    /// <summary>
    /// A basic calculator
    /// </summary>
    public partial class Calculator : Form
    {
        #region constructor

        /// <summary>
        /// Default constructor
        /// </summary>

        public Calculator()
        {
            InitializeComponent();
        }

        #endregion

        #region Clear method

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtDisplay.Text); 
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            //clears input in the text box
            txtDisplay.Clear();

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (txtDisplay.Text.Length > 0)
            {
                //Remember selection start
                var selectionStart = this.txtDisplay.SelectionStart;

                // Delete text
                txtDisplay.Text = txtDisplay.Text.Remove(txtDisplay.Text.Length - 1, 1);

                // Restore selection start
                this.txtDisplay.SelectionStart = selectionStart;

                // Set selection lenght to zero
                this.txtDisplay.SelectionLength = 0;
                
            }

            //Focuses user input text back to the display
            FocusInputText();
        }

        #endregion

        #region Operator methods

        private void btnDiv_Click(object sender, EventArgs e)
        {
            InsertTextValue(" / ");

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btnMult_Click(object sender, EventArgs e)
        {
            InsertTextValue(" * ");

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            InsertTextValue(" - ");

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            InsertTextValue(" + ");

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            CalculateEquation();

            //Focuses user input text back to the display
            FocusInputText();
        }


        #endregion

        /// <summary>
        /// Calculates the given equation and output result
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void CalculateEquation()
        {
            //The purpose of the next four block of code is to allow the cursor retain the outer position instead of highlighting the whole result and 
            //putting new inputs from the left hand side

            //Remember selection start
            var selectionStart = this.txtDisplay.SelectionStart;

            // Display the result
            txtDisplay.Text = parseOperation();

            // Restore selection start
            this.txtDisplay.SelectionStart = selectionStart;

            // Set selection lenght to zero
            this.txtDisplay.SelectionLength = 0;

            

            //Focuses user input text back to the display
            FocusInputText();

        }

        /// <summary>
        /// Parses the users equation and calculates the result
        /// </summary>
        /// <returns></returns>
        private string parseOperation()
        {
            try
            {
                //Get user's input
                var input = txtDisplay.Text;

                //Remove all spaces
                input = input.Replace(" ", "");

                //Create a new top-level
                var operation = new Operations();
                var leftside = true;

                //Looping through each character of the input starting from the left to the right
                for (int i=0; i<input.Length; i++)
                {
                    //check if current character is a number
                    if ("123456789.".Any(x => input[i] == x))
                    {
                        if (leftside)
                            operation.LeftSide = AddNumberPart(operation.LeftSide, input[i]);
                        else
                            operation.RightSide = AddNumberPart(operation.RightSide, input[i]);
                    }
                    // If it is an operator (+ - * / ) set the operator type
                    else if ("+-*/".Any(x => input[i] == x))
                    {
                        // If we are on the right side already, we now need to calculate our current operation
                        // and set the result to the left side of the next operation
                        if (!leftside)
                        {
                            //Get the operator type
                            var operatorType = GetOperationType(input[i]);

                            //Check if we actually have a right side number
                            if (operation.RightSide.Length == 0)
                            {
                                //check if the operator is not a minus (as they could be creating a negative number)
                                if (operatorType != OperationType.Minus)
                                    throw new InvalidOperationException($"Operator (+ * / or more than one - ) specified without a left side number");

                                // If we get here, the operator type is a minus, and there is no left number currently, so add minus
                                operation.RightSide += input[i];
                            }
                            else
                            {
                                // calculate previous equation and set to the left side
                                operation.LeftSide = CalculateOperaton(operation);

                                // set mew operator
                                operation.OperationType = operatorType;

                                // clear the previous right number
                                operation.RightSide = string.Empty;
                            }
                        }
                        else
                        {
                            //Get the operator type
                            var operatorType = GetOperationType(input[i]);

                            //Check if we actually have a left side number
                            if (operation.LeftSide.Length == 0)
                            {
                                //check if the operator is not a minus (as they could be creating a negative number)
                                if (operatorType != OperationType.Minus)
                                    throw new InvalidOperationException($"Operator (+ * / or more than one - ) specified without a left side number");

                                // If we get here, the operator type is a minus, and there is no left number currently, so add minus
                                operation.LeftSide += input[i];
                            }
                            else
                            {
                                // If we get here, we have a left number and now an operator, so we want to move to the right side

                                // Set the operation type
                                operation.OperationType = operatorType;

                                // Move to the right side
                                leftside = false;
                            }

                        }
                    }

                }

                // If we are done parsing and no exceptions, calulate current operation
                return CalculateOperaton(operation);
               
            }
            catch (Exception ex)
            {
                return $"invalid equation. {ex.Message}";
            }
        }

        /// <summary>
        /// Calulates an <see cref="Operation"/> and returns result
        /// </summary>
        /// <param name="operation">The operation to calculate</param>
    
        private string CalculateOperaton(Operations operation)
        {
            // store the numer value of the string representations
            double left = 0;
            double right = 0;

            // Check if we have a valid left side nubmer
            if (string.IsNullOrEmpty(operation.LeftSide) || !double.TryParse(operation.LeftSide, out left))
                throw new InvalidOperationException($"Left side of the operation is not a number. {operation.LeftSide}");

            // Check if we have a valid right side nubmer
            if (string.IsNullOrEmpty(operation.RightSide) || !double.TryParse(operation.RightSide, out right))
                throw new InvalidOperationException($"Right side of the operation is not a number. {operation.RightSide}");

            try
            {
                switch (operation.OperationType)
                {
                    case OperationType.Add:
                        return (left + right).ToString();
                    case OperationType.Minus:
                        return (left - right).ToString();
                    case OperationType.Divide:
                        return (left / right).ToString();
                    case OperationType.Multiply:
                        return (left * right).ToString();
                    default:
                        throw new InvalidOperationException($"Unknown operator type when calculating operation. {operation.OperationType}");

                }
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Failed to calculate operation {operation.LeftSide} {operation.OperationType} {operation.RightSide}. {ex.Message}");
            }

        }

        /// <summary>
        /// Accepts a character and returns a known <see cref="OperationType"/>
        /// </summary>
        /// <param name="character">The Character to parse</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private OperationType GetOperationType (char character)
        {
            switch (character)
            {
                case '+':
                    return OperationType.Add;
                case '-':
                    return OperationType.Minus;
                case '*':
                    return OperationType.Multiply;
                case '/':
                    return OperationType.Divide;
                default:
                    throw new InvalidOperationException($"Uknown Operator type { character }");
            }
        }

        /// <summary>
        /// Attempts to add a new character to the current number, checking for valid characters as it goes
        /// </summary>
        /// <param name="currentNumber">The current number string</param>
        /// <param name="newCharacter">the new character to append to the string</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string AddNumberPart(string currentNumber, char newCharacter)
        {
            // Check if there is already a . in the number
            if (newCharacter == '.' && currentNumber.Contains('.'))
                throw new InvalidOperationException($"Number {currentNumber} already contains a dot (.) and another cannot be added");

            return currentNumber + newCharacter;
        }

        #region Number methods

        private void btnDot_Click(object sender, EventArgs e)
        {
            InsertTextValue(".");

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            InsertTextValue("0");

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            InsertTextValue("1");

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            InsertTextValue("2");

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            InsertTextValue("3");

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            InsertTextValue("4");

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            InsertTextValue("5");

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            InsertTextValue("6");

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            InsertTextValue("7");

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            InsertTextValue("8");

            //Focuses user input text back to the display
            FocusInputText();
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            InsertTextValue("9");

            //Focuses user input text back to the display
            FocusInputText();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// focuses the user input text
        /// </summary>
        private void FocusInputText()
        {
            this.txtDisplay.Focus();
        }


        /// <summary>
        /// Inserts the given text into user text display
        /// </summary>
        /// <param name="value"> the value to insert</param>
        private void InsertTextValue(string value)
        {

            //Remember selection start
            var selectionStart = this.txtDisplay.SelectionStart;

            // Set new text
            this.txtDisplay.Text = this.txtDisplay.Text.Insert(this.txtDisplay.SelectionStart, value);

            // Restore selection start
            this.txtDisplay.SelectionStart = selectionStart + value.Length;

            // Set selection lenght to zero
            this.txtDisplay.SelectionLength = 0;
        }

        #endregion

        private void txtDisplay_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
