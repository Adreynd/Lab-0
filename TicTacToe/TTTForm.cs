using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class TTTForm : Form
    {
        public TTTForm()
        {
            InitializeComponent();
        }

        const string USER_SYMBOL = "X";
        const string COMPUTER_SYMBOL = "O";
        const string EMPTY = "";

        const int SIZE = 5;

        // constants for the 2 diagonals
        const int TOP_LEFT_TO_BOTTOM_RIGHT = 1;
        const int TOP_RIGHT_TO_BOTTOM_LEFT = 2;

        // constants for IsWinner
        const int NONE = -1;
        const int ROW = 1;
        const int COLUMN = 2;
        const int DIAGONAL = 3;

        // This method takes a row and column as parameters and 
        // returns a reference to a label on the form in that position
        private Label GetSquare(int row, int column)
        {
            int labelNumber = row * SIZE + column + 1;
            return (Label)(this.Controls["label" + labelNumber.ToString()]);
        }

        // This method does the "reverse" process of GetSquare
        // It takes a label on the form as it's parameter and
        // returns the row and column of that square as output parameters
        private void GetRowAndColumn(Label l, out int row, out int column)
        {
            int position = int.Parse(l.Name.Substring(5));
            row = (position - 1) / SIZE;
            column = (position - 1) % SIZE;
        }

        // This method takes a row (in the range of 0 - 4) and returns true if 
        // the row on the form contains 5 Xs or 5 Os.
        // Use it as a model for writing IsColumnWinner
        private bool IsRowWinner(int row)
        {
            Label square = GetSquare(row, 0);
            string symbol = square.Text;
            for (int col = 1; col < SIZE; col++)
            {
                square = GetSquare(row, col);
                if (symbol == EMPTY || square.Text != symbol)
                    return false;
            }
            return true;
        }

        //* TODO:  finish all of these that return true
        private bool IsAnyRowWinner()
        {
            for (int i = 0; i < SIZE; i++) // Call IsRowWinner to test each individual row
                if (IsRowWinner(i))
                    return true;           // Return true as soon as one of them are
            return false;
        }

        private bool IsColumnWinner(int col)
        {
            Label square = GetSquare(0, col);       // Start at the top for any given column
            string symbol = square.Text;            // Create a string, give it the value at the same location
            for (int row = 1; row < SIZE; row++)
            {
                square = GetSquare(row, col);   // Test a different square each loop
                if (symbol == EMPTY || square.Text != symbol)
                    return false;               // Return false as soon as we find a square that is either empty, or has the wrong symbol
            }
            return true;                            // Otherwise, return true
        }

        private bool IsAnyColumnWinner()
        {
            for (int i = 0; i < SIZE; i++) // Call IsColumnWinner to test each individual column
                if (IsColumnWinner(i))
                    return true;           // Return true as soon as one of them are
            return false;
        }

        private bool IsDiagonal1Winner()
        {
            Label square = GetSquare(0, 0);         // Start at the top left box
            string symbol = square.Text;            // Get the symbol from that location
            for (int i = 1; i < SIZE; i++)          // Go diagonally to the bottom right
            {
                square = GetSquare(i, i);           // Test a different square each loop
                if (symbol == EMPTY || square.Text != symbol)
                    return false;                   // Return false as soon as we find an empty square or a square with a different symbol
            }
            return true;
        }

        private bool IsDiagonal2Winner()
        {
            Label square = GetSquare(0, (SIZE - 1));
            string symbol = square.Text;
            for (int row = 1, col = SIZE - 2; row < SIZE; row++, col--)
            {
                square = GetSquare(row, col);
                if (symbol == EMPTY || square.Text != symbol)
                    return false;
            }
            return true;
        }

        private bool IsAnyDiagonalWinner()
        {
            return IsDiagonal1Winner() || IsDiagonal2Winner();  // Test both diagonals, return true if either of them return true
        }

        private bool IsFull()
        {
            Label square;
            for (int col = 0; col < SIZE; col++)            // Look at each square from each row and column
                for (int row = 0; row < SIZE; row++)
                {
                    square = GetSquare(row, col);           // Set the location to the next square to test
                    if (square.Text == EMPTY)
                        return false;                       // Return false if any of them are empty
                }
            return true;
        }

        // This method determines if any row, column or diagonal on the board is a winner.
        // It returns true or false and the output parameters will contain appropriate values
        // when the method returns true.  See constant definitions at top of form.
        private bool IsWinner(out int whichDimension, out int whichOne)
        {
            // rows
            for (int row = 0; row < SIZE; row++)
            {
                if (IsRowWinner(row))
                {
                    whichDimension = ROW;
                    whichOne = row;
                    return true;
                }
            }
            // columns
            for (int column = 0; column < SIZE; column++)
            {
                if (IsColumnWinner(column))
                {
                    whichDimension = COLUMN;
                    whichOne = column;
                    return true;
                }
            }
            // diagonals
            if (IsDiagonal1Winner())
            {
                whichDimension = DIAGONAL;
                whichOne = TOP_LEFT_TO_BOTTOM_RIGHT;
                return true;
            }
            if (IsDiagonal2Winner())
            {
                whichDimension = DIAGONAL;
                whichOne = TOP_RIGHT_TO_BOTTOM_LEFT;
                return true;
            }
            whichDimension = NONE;
            whichOne = NONE;
            return false;
        }

        // I wrote this method to show you how to call IsWinner
        private bool IsTie()
        {
            int winningDimension, winningValue;
            return (IsFull() && !IsWinner(out winningDimension, out winningValue));
        }

        // This method takes an integer in the range 0 - 4 that represents a column
        // as it's parameter and changes the font color of that cell to red.
        private void HighlightColumn(int col)
        {
            for (int row = 0; row < SIZE; row++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        // This method changes the font color of the top right to bottom left diagonal to red
        // I did this diagonal because it's harder than the other one
        private void HighlightDiagonal2()
        {
            for (int row = 0, col = SIZE - 1; row < SIZE; row++, col--)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        // This method will highlight either diagonal, depending on the parameter that you pass
        private void HighlightDiagonal(int whichDiagonal)
        {
            if (whichDiagonal == TOP_LEFT_TO_BOTTOM_RIGHT)
                HighlightDiagonal1();
            else
                HighlightDiagonal2();

        }

        //* TODO:  finish these 2
        private void HighlightRow(int row)
        {
            for (int col = 0; col < SIZE; col++)        // Go through each square of a row
            {
                Label square = GetSquare(row, col);     // Highlight a new square
                square.Enabled = true;                  // Enable it
                square.ForeColor = Color.Red;           // Change the square's color
            }
        }

        private void HighlightDiagonal1()
        {
            for (int i = 0; i < SIZE; i++)          // Go through each square
            {
                Label square = GetSquare(i, i);     // Highlight a new square
                square.Enabled = true;              // Enable it
                square.ForeColor = Color.Red;       // Change the square's color
            }
        }

        //* TODO:  finish this
        private void HighlightWinner(string player, int winningDimension, int winningValue)
        {
            switch (winningDimension)
            {
                case ROW:
                    HighlightRow(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
                case COLUMN:
                    HighlightColumn(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
                case DIAGONAL:
                    HighlightDiagonal(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
            }
            DisableAllSquares();
        }

        //* TODO:  finish these 2
        private void ResetSquares()
        {
            Label square;
            for (int col = 0; col < SIZE; col++)
                for (int row = 0; row < SIZE; row++)
                {
                    square = GetSquare(row, col);
                    square.Text = EMPTY;
                    square.ForeColor = Color.Black;
                }
        }

        private void MakeComputerMove()
        {
            Random rnd = new Random();
            int row = rnd.Next(0, SIZE);
            int col = rnd.Next(0, SIZE);
            Label square = GetSquare(row, col); // Find a random square

            while (square.Text != EMPTY)    // While the random square is occupied...
            {
                row = rnd.Next(0, SIZE);    // Rerandom the square
                col = rnd.Next(0, SIZE);
                square = GetSquare(row, col);
            }

            int dim, one;
            square.Text = COMPUTER_SYMBOL;              // Give the square the computer's symbol
            if (IsWinner(out dim, out one))             // If there is a winner, highlight the winning squares
                HighlightWinner("The computer", dim, one);
            IsTie();                                    // Test if there is a tie...
        }

        // Setting the enabled property changes the look and feel of the cell.
        // Instead, this code removes the event handler from each square.
        // Use it when someone wins or the board is full to prevent clicking a square.
        private void DisableAllSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    DisableSquare(square);
                }
            }
        }

        // Inside the click event handler you have a reference to the label that was clicked
        // Use this method (and pass that label as a parameter) to disable just that one square
        private void DisableSquare(Label square)
        {
            square.Click -= new System.EventHandler(this.label_Click);
        }

        // You'll need this method to allow the user to start a new game
        private void EnableAllSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    square.Click += new System.EventHandler(this.label_Click);
                    square.ForeColor = Color.Black;
                }
            }
        }

        //* TODO:  finish the event handlers
        private void label_Click(object sender, EventArgs e)
        {
            int winningDimension = NONE;
            int winningValue = NONE;

            Label clickedLabel = (Label)sender;

        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            ResetSquares();
            EnableAllSquares();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
