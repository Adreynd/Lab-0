using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory
{
    public partial class boardForm : Form
    {
        public boardForm()
        {
            InitializeComponent();
        }

        #region Instance Variables
        const int NOT_PICKED_YET = -1;
        const int CARDS = 20;

        int firstCardNumber = NOT_PICKED_YET;
        int secondCardNumber = NOT_PICKED_YET;
        int matches = 0;
        #endregion

        #region Methods

        // This method finds a picture box on the board based on it's number (1 - 20)
        // It takes an integer as it's parameter and returns the picture box controls
        // that's name contains that number
        private PictureBox GetCard(int i)
        {
            PictureBox card = (PictureBox)this.Controls["card" + i];
            return card;
        }

        // This method gets the filename for the image displayed in a picture box given it's number
        // It takes an integer as it's parameter and returns a string containing the 
        // filename for the image in the corresponding picture box
        private string GetCardFilename(int i)
        {
            return GetCard(i).Tag.ToString();
        }

        // This method changes the filename for a given picture box
        // It takes an integer and a string that represents a filename as it's parameters
        // It doesn't return a value but stores the filename for the image to be displayed
        // in the picture box.  It doesn't actually display the new image
        private void SetCardFilename(int i, string filename)
        {
            GetCard(i).Tag = filename;
        }

        // These 2 methods get the value (and suit) of the card in a given picturebox
        // Both methods take an integer as the parameter and return a string
        private string GetCardValue(int index)
        {
            return GetCardFilename(index).Substring(4, 1);
        }

        private string GetCardSuit(int index)
        {
            return GetCardFilename(index).Substring(5, 1);
        }

        private bool IsMatch(int index1, int index2)        // Checks if the second card has been picked, and whether or not the value of card 1 matches card 2
        {
            return index2!= NOT_PICKED_YET && GetCardValue(index1) == GetCardValue(index2);
        }

        // This method fills each picture box with a filename
        private void FillCardFilenames()
        {
            string[] values = { "a", "2", "j", "q", "k" };
            string[] suits = { "c", "d", "h", "s" };
            int i = 1;

            for (int suit = 0; suit < suits.Length; suit++)
            {
                for (int value = 0; value < values.Length; value++)
                {
                    SetCardFilename(i, "card" + values[value] + suits[suit] + ".jpg");
                    i++;
                }
            }
        }

        private void ShuffleCards()                     // Goes through every card index, and swaps the image with the image from a random index
        {
            Random rnd = new Random();
            int index;                          // Will hold random index values
            string filename;                    // Placeholder used to swap the images of two cards
            for (int i = 1; i <= CARDS; i++)    // Go through every card index
            {
                index = rnd.Next(1, CARDS + 1);             // Randomize index to a valid index
                while (index == i)                          // If the random index is equal to i, rerandomize it until it isn't
                    index = rnd.Next(1, CARDS + 1);
                filename = GetCardFilename(i);              // Store the picture location for the card at index i
                SetCardFilename(i, GetCardFilename(index));     // Give the card at index i, the picture from the random index
                SetCardFilename(index, filename);               // Complete the swap, give the card at the random index, the picture that was at index i
            }
        }

        // This method loads (shows) an image in a picture box.  Assumes that filenames
        // have been filled in an earlier call to FillCardFilenames
        private void LoadCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\Cards\\" + GetCardFilename(i));
        }

        // This method loads the image for the back of a card in a picture box
        private void LoadCardBack(int i)
        {
            PictureBox card = GetCard(i);
            card.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\Cards\\black_back.jpg");
        }

        // TODO:  students should write all of these
        // shows (loads) the backs of all of the cards
        private void LoadAllCardBacks()
        {
            for (int i = 1; i <= CARDS; i++)
                LoadCardBack(i);
        }

        // Hides a picture box
        private void HideCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Hide();
        }

        private void HideAllCards()
        {
            for (int i = 1; i <= CARDS; i++)
                HideCard(i);
        }

        // shows a picture box
        private void ShowCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Show();
        }

        private void ShowAllCards()
        {
            for (int i = 1; i <= CARDS; i++)
                ShowCard(i);
        }

        // disables a picture box
        private void DisableCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Enabled = false;
        }

        private void DisableAllCards()
        {
            for (int i = 1; i <= CARDS; i++)
                DisableCard(i);
        }

        private void EnableCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Enabled = true;
        }

        private void EnableAllCards()
        {
            for (int i = 1; i <= CARDS; i++)
                EnableCard(i);
        }
    
        private void EnableAllVisibleCards()
        {
            for (int i = 1; i <= CARDS; i++)
                if (GetCard(i).Visible == true)
                    EnableCard(i);
        }

        #endregion

        #region EventHandlers
        private void boardForm_Load(object sender, EventArgs e)
        {
            FillCardFilenames();    // Fill the board with unique cards
            ShuffleCards();         // Randomize the postion of those cards
            LoadAllCardBacks();     // Flip all the cards over
        }

        private void card_Click(object sender, EventArgs e)
        {
            PictureBox card = (PictureBox)sender;       // Get the card at the location the user clicked...
            int cardNumber = int.Parse(card.Name.Substring(4));     // Then the index derived from that card

            if (firstCardNumber == NOT_PICKED_YET)      // When the first card has been clicked, track its index, flip it over, and prevent it from being reclicked
            {
                firstCardNumber = cardNumber;
                LoadCard(firstCardNumber);
                DisableCard(firstCardNumber);
            }
            else                                        // When the second card has been clicked, track index, flip it, prevent it from being reclicked, start the timer
            {
                secondCardNumber = cardNumber;
                LoadCard(secondCardNumber);
                DisableAllCards();
                flipTimer.Start();
            }
        }

        private void flipTimer_Tick(object sender, EventArgs e)
        {
            flipTimer.Stop();
            if (IsMatch(firstCardNumber, secondCardNumber))
            {
                matches++;

                HideCard(firstCardNumber);      // Remove the visiblity of matched cards
                HideCard(secondCardNumber);

                firstCardNumber = NOT_PICKED_YET;       // After getting a match, reset the variables tracking the indexs of picked cards
                secondCardNumber = NOT_PICKED_YET;

                if (matches == 10)
                    MessageBox.Show("You have matched every card.");
                else
                    EnableAllVisibleCards();            // Renable any cards that are visible so that they can be clicked
            }
            else
            {
                LoadCardBack(firstCardNumber);          // Upon failing to get a match, flip the cards back over
                LoadCardBack(secondCardNumber);

                firstCardNumber = NOT_PICKED_YET;       // After flipping card back over, reset the variables tracking the indexs of picked cards
                secondCardNumber = NOT_PICKED_YET;

                EnableAllVisibleCards();                // Renable any cards that are visible so that they can be clicked
            }
        }
        #endregion
    }
}
