/*
 * Maeve Wheaton
 * Mr. T
 * May 4, 2021
 * There are three warnings, I had three textboxes but there was an issue and I almost lost all my work, 
 * when I fixed it though they were gone and although I can no longer find them anywhere in the designer 
 * or code it says they are there, I've just been ignoring them.
 */
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
using System.Media;

namespace CashRegister
{
    public partial class Form1 : Form
    {
        //variables
        double lovePotionPrice = 17.99;
        double healingPotionPrice = 13.99;
        double spellBookPrice = 25.50;

        int amountLovePotion; 
        int amountHealingPotion;
        int amountSpellBook;

        double totalOrderCost;
        double taxRate = 0.13;
        double taxAmount;
        double totalCost;

        double amountTendered;
        double changeRequired;

        int orderNumber;
                

        public Form1()
        {
            InitializeComponent();          
        }

        private void totalButton_Click(object sender, EventArgs e)
        {
            try //in case letter or decimal is entered
            {
                //clear error message if trying again
                totalErrorMessageLabel.Text = "";

                //get number of each item
                amountLovePotion = Convert.ToInt32(numberLoveInput.Text);
                amountHealingPotion = Convert.ToInt32(numberHealingInput.Text);
                amountSpellBook = Convert.ToInt32(numberBookInput.Text);

                //play bottles clinking together, but only if they bought bottles
                SoundPlayer bottleClinkSound = new SoundPlayer(Properties.Resources.bottleClink);
                if (amountLovePotion > 0 || amountHealingPotion > 0)
                {
                    bottleClinkSound.Play();
                }
                
                //calculate total before tax
                totalOrderCost = (amountLovePotion * lovePotionPrice) + (amountHealingPotion * healingPotionPrice) + (amountSpellBook * spellBookPrice);

                //calculate tax
                taxAmount = totalOrderCost * taxRate;

                //calcualte total including tax
                totalCost = totalOrderCost + taxAmount;

                //display in totalsLabel
                totalsOutput.Text = $"{totalOrderCost.ToString("C")}\n{taxAmount.ToString("C")}\n{totalCost.ToString("C")}";                             

                //pause to allow sound to play, then stop sound
                Thread.Sleep(2000);
                bottleClinkSound.Stop();

                //enable change button 
                changeButton.Enabled = true;
            }
            catch
            {
                totalErrorMessageLabel.Text = "Please enter\nwhole numbers\nonly.";
            }                       
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            try //in case letters entered in tendered
            {
                //clear error message if trying again
                changeErrorMessageLabel.Text = "";               

                //get amount tendered
                amountTendered = Convert.ToDouble(tenderedInput.Text);

                //calculate change
                changeRequired = amountTendered - totalCost;

                if (changeRequired < 0) //avoid negative change
                {
                    changeErrorMessageLabel.Text = "Please enter \na value larger \nthan total cost.";
                }
                else
                {
                    //play coin sound
                    SoundPlayer coinSound = new SoundPlayer(Properties.Resources.coins);
                    coinSound.Play();

                    //display in changeOutput
                    changeOutput.Text = $"{changeRequired.ToString("C")}";                                       

                    //pause to allow sound to play, then stop sound
                    Thread.Sleep(2000);
                    coinSound.Stop();

                    //enable print receipt
                    printReceiptButton.Enabled = true;
                }                        
            }
            catch
            {
                changeErrorMessageLabel.Text = "Please enter\nnumbers only.";
            }
            
        }

        private void printReceiptButton_Click(object sender, EventArgs e)
        {
            //play printing sound
            SoundPlayer printReceiptSound = new SoundPlayer(Properties.Resources.quill_and_parchment);
            printReceiptSound.PlayLooping(); //the sound of someone writing on a scroll but the writing doesn't start right at the begininng so it's not clear - wish I could edit the audio to just the part I want
            Thread.Sleep(5000); //give sound time to get through the beginning, hopefully mean the writing starts when the reciept prints

            //everything else visible = false
            lovePotionLabel.Visible = false;
            numberLoveInput.Visible = false;
            healingPotionLabel.Visible = false;
            numberHealingInput.Visible = false;
            spellBookLabel.Visible = false;
            numberBookInput.Visible = false;
            totalButton.Visible = false;
            totalsLabel.Visible = false;
            totalsOutput.Visible = false;
            lineLabel.Visible = false;
            tenderedLabel.Visible = false;
            tenderedInput.Visible = false;
            changeButton.Visible = false;
            changeLabel.Visible = false;
            changeOutput.Visible = false;
            printReceiptButton.Visible = false;

            //receiptOutput and newOrderButton visible
            receiptOuput.Visible = true;
            newOrderButton.Visible = true;
            exitButton.Visible = true;

            //order number calculation, equal to the number of items bought
            orderNumber = amountLovePotion + amountHealingPotion + amountSpellBook;
                       
            //print one line at a time, pause inbetween
            //heading
            int pause = 1000;
            receiptOuput.Text = "          The Little Shop of Wonders";
            Refresh();
            Thread.Sleep(pause);
            receiptOuput.Text += $"\n   Order Number {orderNumber}";
            Refresh();
            Thread.Sleep(pause);
            receiptOuput.Text += $"\n   {DateTime.Now}\n";
            Refresh();
            Thread.Sleep(pause);

            //amoung of items bought
            if(amountLovePotion > 0) //won't appear if none were bought
            {
                receiptOuput.Text += $"\n   Love Potion       {amountLovePotion.ToString("00")} @ {lovePotionPrice.ToString("0.00")}";
                Refresh();
                Thread.Sleep(pause);
            }            
            if (amountHealingPotion > 0)
            {
                receiptOuput.Text += $"\n   Healing Potion   {amountHealingPotion.ToString("00")} @ {healingPotionPrice.ToString("0.00")}";
                Refresh();
                Thread.Sleep(pause);
            }
            if (amountSpellBook > 0)
            {
                receiptOuput.Text += $"\n   Spell Book         {amountSpellBook.ToString("00")} @ {spellBookPrice.ToString("0.00")}";
                Refresh();
                Thread.Sleep(pause);
            }

            //totals
            receiptOuput.Text += $"\n\n   Subtotal                {totalOrderCost.ToString("$ .00")}";
            Refresh();
            Thread.Sleep(pause); 
            receiptOuput.Text += $"\n   Tax                     {taxAmount.ToString("$ .00")}";
            Refresh();
            Thread.Sleep(pause);
            receiptOuput.Text += $"\n   Total                    {totalCost.ToString("$ .00")}";
            Refresh(); 
            Thread.Sleep(pause);

            //change
            receiptOuput.Text += $"\n\n   Tendered               {amountTendered.ToString("$ .00")}";
            Refresh();
            Thread.Sleep(pause);
            receiptOuput.Text += $"\n   Change                 {changeRequired.ToString("$ .00")}";
            Refresh();
            Thread.Sleep(pause);

            //good day message
            receiptOuput.Text += $"\n\n Thank you for shoping! Have a good day!";
            Refresh();

            //stop sound
            printReceiptSound.Stop();

            //enable newOrderButton
            newOrderButton.Enabled = true;
            exitButton.Enabled = true;
        }

        private void newOrderButton_Click(object sender, EventArgs e)
        {
            //everything visible = true
            lovePotionLabel.Visible = true;
            numberLoveInput.Visible = true;
            healingPotionLabel.Visible = true;
            numberHealingInput.Visible = true;
            spellBookLabel.Visible = true;
            numberBookInput.Visible = true;
            totalButton.Visible = true;
            totalsLabel.Visible = true;
            totalsOutput.Visible = true;
            lineLabel.Visible = true;
            tenderedLabel.Visible = true;
            tenderedInput.Visible = true;
            changeButton.Visible = true;
            changeLabel.Visible = true;
            changeOutput.Visible = true;
            printReceiptButton.Visible = true;

            //receiptOutput and newOrderButton invisible
            receiptOuput.Visible = false;
            newOrderButton.Visible = false;
            exitButton.Visible = false;

            //clear all input boxes and output areas
            numberLoveInput.Clear();
            numberHealingInput.Clear();
            numberBookInput.Clear();
            totalsOutput.Text = "$\n$\n$";
            tenderedInput.Clear();
            changeOutput.Text = "$";

            //disable buttons exept total
            changeButton.Enabled = false;
            printReceiptButton.Enabled = false;
            newOrderButton.Enabled = false;
            exitButton.Enabled = false;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void healingPotionPicture_Click(object sender, EventArgs e)
        {
            hSecretMessage.Visible = true;
            Refresh();
            Thread.Sleep(6000);
            hSecretMessage.Visible = false;
        }

        private void lovePotionPicture_Click(object sender, EventArgs e)
        {
            lSecretMessage.Visible = true;
            Refresh();
            Thread.Sleep(6000);
            lSecretMessage.Visible = false;
        }

        private void spellBookPicture_Click(object sender, EventArgs e)
        {
            sSecretMessage.Visible = true;
            Refresh();
            Thread.Sleep(6000);
            sSecretMessage.Visible = false;
        }                
    }
}
