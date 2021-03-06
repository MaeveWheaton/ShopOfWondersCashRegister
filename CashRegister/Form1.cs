/*
 * Maeve Wheaton
 * Mr. T
 * May 4, 2021
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

        double discountAmount;
        int discountBlocker = 0; //use so only one code per order

        int orderNumber = 1;
                

        public Form1()
        {
            InitializeComponent();          
        }

        private void totalButton_Click(object sender, EventArgs e) //total cost of items
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
                totalsOutput.Text = $"{totalOrderCost.ToString("C")}\n{taxAmount.ToString("C")}\n\n{totalCost.ToString("C")}";                             

                //pause to allow sound to play, then stop sound
                Thread.Sleep(2000);
                bottleClinkSound.Stop();

                //enable next buttons, disable total
                totalButton.Enabled = false;
                changeButton.Enabled = true;
                checkCodeButton.Enabled = true;
            }
            catch
            {
                totalErrorMessageLabel.Text = "Please enter\nwhole numbers\nonly.";
            }                       
        }
        private void checkCodeButton_Click(object sender, EventArgs e) //check to apply discount code
        {
            try
            {
                //clear error message label
                notValidMessageLabel.Text = "";

                //dicount variable, unique to this function
                string discountCodeOne = "HP+10";
                string discountCodeTwo = "HappilyEA";
                string discountCodeThree = "Pg364";
                double discountOneValue = 10; //10 dollars off
                double discountTwoValue = .10; //10% off
                double discountThreeValue = .20; //20% off                               

                if (discountCodeInput.Text == discountCodeOne && discountBlocker == 0)
                {
                    //calculate discount and new total
                    discountAmount = discountOneValue;
                    totalCost = totalCost - discountAmount;

                    //change total display
                    totalsLabel.Text = "Subtotal\nTax\nDiscount\nTotal";
                    totalsOutput.Text = $"{totalOrderCost.ToString("C")}\n{taxAmount.ToString("C")}\n{discountAmount.ToString("C")}\n{totalCost.ToString("C")}";

                    //prevent another code from being applied
                    discountBlocker = 1;
                }
                else if (discountCodeInput.Text == discountCodeTwo && discountBlocker == 0)
                {
                    discountAmount = totalCost * discountTwoValue;
                    totalCost = totalCost - discountAmount;
                    totalsLabel.Text = "Subtotal\nTax\nDiscount\nTotal";
                    totalsOutput.Text = $"{totalOrderCost.ToString("C")}\n{taxAmount.ToString("C")}\n{discountAmount.ToString("C")}\n{totalCost.ToString("C")}";
                    discountBlocker = 1;
                }
                else if (discountCodeInput.Text == discountCodeThree && discountBlocker == 0)
                {
                    discountAmount = totalCost * discountThreeValue;
                    totalCost = totalCost - discountAmount;
                    totalsLabel.Text = "Subtotal\nTax\nDiscount\nTotal";
                    totalsOutput.Text = $"{totalOrderCost.ToString("C")}\n{taxAmount.ToString("C")}\n{discountAmount.ToString("C")}\n{totalCost.ToString("C")}";
                    discountBlocker = 1;
                }
                else
                {
                    if (discountBlocker == 1)
                    {
                        notValidMessageLabel.Text = "Sorry, one \ncode per \norder";
                    }
                    else
                    {
                        notValidMessageLabel.Text = "Code not valid";
                    }                    
                }            
            }
            catch
            {
                notValidMessageLabel.Text = "Code not valid";
            }                         
        }

        private void changeButton_Click(object sender, EventArgs e) //calculate change
        {
            //clear message as process is continueing
            notValidMessageLabel.Text = "";

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
                    changeButton.Enabled = false;
                    checkCodeButton.Enabled = false;
                }                        
            }
            catch
            {
                changeErrorMessageLabel.Text = "Please enter\nnumbers only.";
            }
            
        }

        private void firstNewOrderButton_Click(object sender, EventArgs e) //clear order before receipt
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
            firstNewOrderButton.Visible = true;
            firstExitButton.Visible = true;
            discountCodeInput.Visible = true;
            checkCodeButton.Visible = true;

            //receiptOutput and newOrderButton invisible
            receiptOuput.Visible = false;
            newOrderButton.Visible = false;
            exitButton.Visible = false;

            //clear all input boxes and output areas
            numberLoveInput.Clear();
            numberHealingInput.Clear();
            numberBookInput.Clear();
            totalsOutput.Text = "$\n$\n\n$";
            tenderedInput.Clear();
            changeOutput.Text = "$";
            discountCodeInput.Clear();
            discountBlocker = 0;

            //disable buttons exept total
            changeButton.Enabled = false;
            printReceiptButton.Enabled = false;
            newOrderButton.Enabled = false;
            exitButton.Enabled = false;
            checkCodeButton.Enabled = false;
            totalButton.Enabled = true;

            //increase order number
            orderNumber = orderNumber + 1;
        }

        private void firstExitButton_Click(object sender, EventArgs e) //close during the purchase stage
        {
            Close();
        }

        private void printReceiptButton_Click(object sender, EventArgs e) //print receipt
        {
            //play printing sound
            SoundPlayer printReceiptSound = new SoundPlayer(Properties.Resources.quill_and_parchment);
            printReceiptSound.PlayLooping(); //sound of handwritten parchment receipt
            Thread.Sleep(5000); //give sound time to get through the beginning, so the writing starts when the reciept prints, rustling actually kind fits

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
            firstNewOrderButton.Visible = false;
            firstExitButton.Visible = false;
            discountCodeInput.Visible = false;
            checkCodeButton.Visible = false;

            //receiptOutput and newOrderButton visible
            receiptOuput.Visible = true;
            newOrderButton.Visible = true;
            exitButton.Visible = true;            

            //order number calculation, equal to the number of items bought
            //orderNumber = amountLovePotion + amountHealingPotion + amountSpellBook;
                       
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
            receiptOuput.Text += $"\n   Discount              {discountAmount.ToString("$ .00")}";
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

        private void newOrderButton_Click(object sender, EventArgs e) //start new order after printing receipt
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
            firstNewOrderButton.Visible = true;
            firstExitButton.Visible = true;
            discountCodeInput.Visible = true;
            checkCodeButton.Visible = true;

            //receiptOutput and newOrderButton invisible
            receiptOuput.Visible = false;
            newOrderButton.Visible = false;
            exitButton.Visible = false;

            //clear all input boxes and output areas
            numberLoveInput.Clear();
            numberHealingInput.Clear();
            numberBookInput.Clear();
            totalsOutput.Text = "$\n$\n\n$";
            tenderedInput.Clear();
            changeOutput.Text = "$";
            discountCodeInput.Clear();
            discountBlocker = 0;

            //disable buttons exept total
            changeButton.Enabled = false;
            printReceiptButton.Enabled = false;
            newOrderButton.Enabled = false;
            exitButton.Enabled = false;
            checkCodeButton.Enabled = false;
            totalButton.Enabled = true;

            //increase order number
            orderNumber = orderNumber + 1;
        }

        private void exitButton_Click(object sender, EventArgs e) //close after printing receipt
        {
            Close();
        }

        private void healingPotionPicture_Click(object sender, EventArgs e)
        {
            hSecretMessage.Visible = true;
            Refresh();
            Thread.Sleep(5000);
            hSecretMessage.Visible = false;
        }

        private void lovePotionPicture_Click(object sender, EventArgs e)
        {
            lSecretMessage.Visible = true;
            Refresh();
            Thread.Sleep(5000);
            lSecretMessage.Visible = false;
        }

        private void spellBookPicture_Click(object sender, EventArgs e)
        {
            sSecretMessage.Visible = true;
            Refresh();
            Thread.Sleep(5000);
            sSecretMessage.Visible = false;
        }        
    }
}
