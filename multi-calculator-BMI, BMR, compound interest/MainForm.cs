using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Assignment3
{
    /// <summary>
    /// Class that contains methods related to manipulation with the form and the form data
    /// </summary>
    public partial class MainForm : Form
    {
        private string name = string.Empty; //instance variable declaration, initialisation to an empty string

        private BMICalculator bmiCalc = new BMICalculator(); //create a new instance of BMI calculator object 

        private SavingCalculator savingCalc = new SavingCalculator(); //create a new instance of Saving Calculator object

        public MainForm()
        {
            InitializeComponent();
            InitializeGUI();
        }

        /// <summary>
        /// Set default appearance for the form when it is first displayed.
        /// </summary>
        private void InitializeGUI() 
        {
            //defaults for the BMI Calculator
            this.Text = "Super Calculator by Kate Arvay";
            lblBMI.Text = string.Empty; //initialise as an empty string
            lblWeightCat.Text = string.Empty; //initialise as an empty string
            lblNormalBMI.Text = "Normal Weight";
            rbtnMetric.Checked = true; //check metric system radio button as default
            lblNormalBMI.Text = "Normal BMI is between 18.50 and 24.9.";
            lblNormWeight.Text = string.Empty; //initialise as empty

            //defaults for the Saving Calculator
            lblAmPaid.Text = string.Empty; //initialise as an empty string
            lblFutValue.Text = string.Empty; //initialise as an empty string
            lblAmEarned.Text = string.Empty; //initialise as an empty string
            lblTotalFees.Text = string.Empty; //initialise as an empty string
            rbtnBegin.Checked = true;

            //defaults for BMR calculator
            rbtnFemale.Checked = true;
            rbtnSedentary.Checked = true;

        }

        /// <summary>
        /// Position the form in the center of the screen on load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }


        #region BMI and BMR Calculator

        /// <summary>
        /// Update the label text for Height to metric or imperial based on which radio button is checked
        /// </summary>
        private void UpdateHeightWeightText()
        {
            if (rbtnMetric.Checked)
            {
                lblHeight.Text = "Height (cm)";
                lblWeight.Text = "Weight (kg)";
                txtInch.Visible = false; //hide the second text field when Metric checked

            }
            else
            {
                lblHeight.Text = "Height (ft, in)";
                lblWeight.Text = "Weight (lbs)";
                txtInch.Visible = true; //show the second text field for inches when Imperial checked
            }

            lblWeightCat.Text = ""; //leave the text field for weight category empty on form initialisation
            lblBMI.Text = ""; // leave the BMI result text field empty on form initialisation 
        }

        /// <summary>
        /// Event handler: call UpdateHeightText method that updates text if the radio button for metric is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnMetric_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHeightWeightText();
        }

        /// <summary>
        /// Event handler: call UpdateHeightText method that updates label text if the radio button for imperial is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnImperial_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHeightWeightText();
        }

        /// <summary>
        /// Event handler: call method to read required input and a method to calculate and display BMI results on GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalcBMI_Click(object sender, EventArgs e)
        {
            //1.read input
            bool ok = ReadInputBMI();

            //2.calculate
            if (ok)
            {
                CalculateAndDisplayResults();

            }
            //3.display results
        }

        /// <summary>
        /// Call different methods to read user input related to BMI and perform validation.
        /// </summary>
        /// <returns>true or false for height and weight ok value</returns>

        private bool ReadInputBMI()
        {
            ReadName();
            bool heightOK = ReadHeight();
            bool weightOK = ReadWeight();
            ReadUnit();

            return heightOK && weightOK;
        }

        /// <summary>
        /// Read unit, set the type based on selected choice.
        /// </summary>
        private void ReadUnit()
        {
            if (rbtnMetric.Checked)
                bmiCalc.SetUnitType(UnitTypes.Metric);
            else
                bmiCalc.SetUnitType(UnitTypes.Imperial);

        }

        /// <summary>
        /// Read activity, set the type based on selected choice.
        /// </summary>
        private void ReadActivity()
        {
            if (rbtnSedentary.Checked)
            {
                bmiCalc.SetActivityLevel(ActivityLevel.Sedentary);

            }
            else if (rbtnLightAc.Checked)
            {
                bmiCalc.SetActivityLevel(ActivityLevel.LightlyActive);
            }
            else if (rbtnModAc.Checked)
            {
                bmiCalc.SetActivityLevel(ActivityLevel.ModeratelyActive);
            }
            else if (rbtnVerAc.Checked)
            {
                bmiCalc.SetActivityLevel(ActivityLevel.VeryActive);
            }
            else if (rbtnExAc.Checked)
            {
                bmiCalc.SetActivityLevel(ActivityLevel.ExtraActive);
            }

        }

        /// <summary>
        /// Read user name.
        /// </summary>
        private void ReadName()
        {
            name = txtName.Text.Trim(); //read text given by the user in the text field and trim potential spaces around
            grpResults.Text = name; //print the user name as a header of the group box

        }

        /// <summary>
        /// Read height value, perform validation, if successfull return true and set height to bmiCalc instance.
        /// </summary>
        /// <returns>true or false after height validation</returns>
        private bool ReadHeight()
        {
            double height = 0.0;

            bool ok = double.TryParse(txtCmFoot.Text, out height); // conversion from string to double

            if (!ok)
            {
                MessageBox.Show("The input value for height is invalid!", "Error");
            }

            double inch = 0.0;

            if (rbtnImperial.Checked)
            {
                ok = ok && double.TryParse(txtInch.Text, out inch); //conversion from string to double

                bmiCalc.SetHeight(height * 12 + inch); //ft --> inch

                if (!ok)
                {
                    MessageBox.Show("The inch value is invalid!", "Error");
                }
            }

            else
            {
                bmiCalc.SetHeight(height / 100); //cm --> m
            }

            return ok;


        }


        /// <summary>
        /// Read weight value, perform validation, if successfull return true and set weight to bmiCalc instance.
        /// </summary>
        /// <returns>true or false after weight validation</returns>
        private bool ReadWeight()
        {
            double weight = 0.0;

            bool ok = double.TryParse(txtKgLbs.Text, out weight); //conversion from string to double data type
            if (!ok) //if ok false
            {
                MessageBox.Show("The weight value is invalid!", "Error");
            }

            double lbs = 0.0;

            if (rbtnImperial.Checked)
            {
                ok = ok && double.TryParse(txtKgLbs.Text, out lbs);
                if (!ok) //if ok false
                {
                    MessageBox.Show("The weight value is invalid!", "Error");
                }
            }

            bmiCalc.SetWeight(weight); //call the method to set height on the instance of BMI calculator 


            return ok;
        }

        /// <summary>
        /// Read age, perform validation, if successfull return true and set age value to bmiCalc instance.
        /// </summary>
        /// <returns>true or false after age validation</returns>
        private bool ReadAge()
        {
            int age = 0;

            bool ageOk = int.TryParse(txtAge.Text, out age); //attempt to convert string to integer

            if (!ageOk)
            {
                MessageBox.Show("The age is invalid. Try again.", "Error");
            }

            bmiCalc.SetAge(age);

            return ageOk;
        }

        /// <summary>
        /// Read gender and assign true or false value based on which radio button is checked, set value to bmiCalc instance.
        /// </summary>
        /// <returns>true or false for genderFemale value</returns>
        private bool ReadGender()
        {
            bool genderFemale = true;

            if (rbtnFemale.Checked)
            {
                genderFemale = true;
            }
            else
            {
                genderFemale = false;
            }

            bmiCalc.SetGender(genderFemale);

            return genderFemale;
        }

        /// <summary>
        /// Call methods to calculate BMI and weight category and display results on the GUI
        /// </summary>
        private void CalculateAndDisplayResults()
        {
            //call the method to calculate BMI on the instance of the BMI Calculator object and save to variable type double
            double bmi = bmiCalc.CalculateBMI();

            //1.convert the value saved to double to string by calling a method ToString on the object
            //2. round and show only two deciaml (also f2 can be used)
            //3. save it to the respective label and display on the UI
            lblBMI.Text = bmi.ToString("0.00");

            //set a weight category by calling a method from the BMI calculator class
            //save into the respective label and display it on the UI
            lblWeightCat.Text = bmiCalc.BmiWeightCategory();

            //calculate and display normal weight
            lblNormWeight.Text = bmiCalc.CalculateNormalWeight();
        }

        /// <summary>
        /// Call method to calculate BMR and display result on the GUI.
        /// </summary>
        private void CalculateAndDisplayBMR()
        {
            double bmrValue = 0.0;

            bmrValue = bmiCalc.CalculateBMR();

            liboxBMR.Items.Add("BMR RESULTS FOR " + name); //items added to the list box to be displayed on GUI
            liboxBMR.Items.Add("");
            liboxBMR.Items.Add("Your BMR (calories/day)" + bmrValue.ToString("0"));

        }

        /// <summary>
        /// Call method to calculate BMR to maintain weight and display result on the GUI.
        /// </summary>
        private void CalculateAndDisplayMaintainWeightBMR()
        {
            double bmrToKeepWeight = 0.00;

            bmrToKeepWeight = bmiCalc.MaintainWeightBMR(); //set value to an instance of bmiCalc

            liboxBMR.Items.Add("Calories to maintain your weight: " + bmrToKeepWeight.ToString("0")); //item added to the list box to be displayed on GUI (display only with two deciamls)
        }

        /// <summary>
        /// Call methods to calculate BMR for loosing or gaining 0,5 and 1 kg and display results on GUI.
        /// </summary>
        private void CalculateAndDisplayLoseOrGainWeightBMR()
        {
            //set to an instance of the bmiCalc
            double looseHalfKilo = bmiCalc.LoseHalfKiloBMR();
            double loseKilo = bmiCalc.LoseKiloBMR();
            double gainHalfKilo = bmiCalc.GainHalfKiloBMR();
            double gainKilo = bmiCalc.GainKiloBMR();

            //add to listbox
            liboxBMR.Items.Add("Calories intake to lose 0,5 kg per week: " + looseHalfKilo.ToString("0"));
            liboxBMR.Items.Add("Calories intake to lose 1 kg per week: " + loseKilo.ToString("0"));
            liboxBMR.Items.Add("Calories intake to gain 0,5 kg per week: " + gainHalfKilo.ToString("0"));
            liboxBMR.Items.Add("Calories intake to gain 1 kg per week: " + gainKilo.ToString("0"));
            liboxBMR.Items.Add("");
            liboxBMR.Items.Add("Losing more than 1000 calories a day is to be avoided.");
            liboxBMR.Items.Add("");


        }


        /// <summary>
        /// Event handler: call method to read required input and a method to calculate and display BMR results on GUI
        /// 1.Read user input.
        /// 2. Calculate and display results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalcBMR_Click(object sender, EventArgs e)
        {
            //1.read input
            ReadInputBMI();
            ReadAge();
            ReadActivity();
            ReadGender();

            //2.calculate and display results
            CalculateAndDisplayBMR();
            CalculateAndDisplayMaintainWeightBMR();
            CalculateAndDisplayLoseOrGainWeightBMR();
        }

        #endregion

        #region Saving Calculator

        /// <summary>
        /// Read monthly deposit, perform validation, if successfull return true and set monthly deposit value to savingCalc instance.
        /// </summary>
        /// <returns>true or false for monthly deposit validation</returns>
        private bool ReadMonthlyDeposit()
        {
            double monthlyDeposit = 0.0;

            bool monDepOk = double.TryParse(txtMonDep.Text, out monthlyDeposit); //performs a conversion of the string input to double

            if (!monDepOk)
            {
                MessageBox.Show("Input invalid. Make sure you fill in number.", "Error");
            }

            savingCalc.SetMonDep(monthlyDeposit); //set the obtained value of monthly deposit as a property of the Sacing Calculator

            return monDepOk;
        }

        /// <summary>
        /// Read investment period, perform validation, if successfull return true and set period value to savingCalc instance.
        /// </summary>
        /// <returns>true or false for period validation</returns>
        private bool ReadPeriod()
        {
            double period = 0.0;

            bool periodOk = double.TryParse(txtPeriod.Text, out period); //performs a conversion of the string input to double

            if (!periodOk)
            {
                MessageBox.Show("Input invalid. Make sure you fill in number.", "Error");
            }

            savingCalc.SetPeriod(period); //set the obtained value of monthly deposit as a property of the Sacing Calculator

            return periodOk;
        }

        /// <summary>
        /// Read initial deposit value, perform validation, if successfull return true and set initial deposit value to savingCalc instance.
        /// </summary>
        /// <returns>true or false for initial deposit validation</returns>
        private bool ReadInitialDeposit()
        {
            double initialDeposit = 0.0;

            bool initDepOk = double.TryParse(txtInitDepo.Text, out initialDeposit);

            if (!initDepOk)
            {
                MessageBox.Show("Input invalid. Try again.", "Error");
            }

            savingCalc.SetInitialDeposit(initialDeposit);

            return initDepOk;
        }

        /// <summary>
        /// Read interest rate value, perform validation, if successfull return true and set interest value to savingCalc instance.
        /// </summary>
        /// <returns>true or false for interest rate validation</returns>
        private bool ReadInterestRate()
        {
            double interestRate = 0.0;

            bool rateOk = double.TryParse(txtIntRate.Text, out interestRate);

            if (!rateOk)
            {
                MessageBox.Show("Input invalid. Try again.", "Error");
            }

            savingCalc.SetInterestRate(interestRate);

            return rateOk;
        }

        /// <summary>
        /// Read fees, perform validation, if successfull return true and set fees value to savingCalc instance.
        /// </summary>
        /// <returns>true or false for fees validation</returns>
        private bool ReadFees()
        {
            double fees = 0.0;

            bool feesOk = double.TryParse(txtFees.Text, out fees);

            if (!feesOk)
            {
                MessageBox.Show("Invalid input.", "Error");
            }

            savingCalc.SetFees(fees);

            return feesOk;
        }

        /// <summary>
        /// Read when deposit added (end or beginning of period) and assign true or false value based on which radio button is checked, set value to savingCalc instance.
        /// </summary>
        /// <returns>true or false for if deposit added at the beginning</returns>
        private bool ReadWhenDepositAdded()
        {
            bool depositAddedBegin = true;

            if (rbtnBegin.Checked)
            {
                depositAddedBegin = true;
            }
            else
            {
                depositAddedBegin = false;
            }

            savingCalc.SetWhenDepositAdded(depositAddedBegin);

            return depositAddedBegin;
        }

        /// <summary>
        /// Call methods to calculate future value, amount paid, amount earned and total on fees and display results on GUI.
        /// </summary>
        private void CalculateAndDisplayFutureValue()
        {
            //call the method to calculate future value on the instance of the Saving Calculator object and save to variable type double
            double saving = savingCalc.CalculateFutureValue();

            //1.convert the value saved to double to string by calling a method ToString on the object
            //2. round and show only two deciaml (also f2 can be used)
            //3. save it to the respective label and display on the UI
            lblFutValue.Text = saving.ToString("0.00");

            double amountPaid = savingCalc.CalculateAmountPaid();
            lblAmPaid.Text = amountPaid.ToString("0.00");

            double amountEarned = savingCalc.CalculateAmountEarned();
            lblAmEarned.Text = amountEarned.ToString("0.00");

            double totalFees = savingCalc.CalculateTotalFees();
            lblTotalFees.Text = totalFees.ToString("0.00");

        }


        /// <summary>
        /// Event handler: call method to read required input and a method to calculate and display results for savings calculations on GUI
        /// 1.Read user input in required fields
        /// 2. Calculate and display results on GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalcSaving_Click(object sender, EventArgs e)
        {
            //1.read input
            ReadMonthlyDeposit();
            ReadPeriod();
            ReadInitialDeposit();
            ReadInterestRate();
            ReadFees();
            ReadWhenDepositAdded();
            //2.calculate
            //3.display result
            CalculateAndDisplayFutureValue();
        }

        #endregion

    }
}
