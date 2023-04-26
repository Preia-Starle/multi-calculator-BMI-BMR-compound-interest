using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Assignment3
{
    /// <summary>
    /// Class that contains Compound interest calculations and supporting methods.
    /// </summary>
    internal class SavingCalculator
    {
        //instance variables for user input
        private double monthlyDeposit = 0.0; //amount added monthly
        private double interestRate = 0.0; //interest rate yearly
        private double period = 0.0; //number of years
        private double initialDeposit = 0.0; //amount added as an initial deposit
        private double fees = 0.0; //fees yearly
        private bool depositAddedBegin = true; //value indicating whether the deposit was added at the beginning or end of the period

        /// <summary>
        /// Getters and setters
        /// get the value from the respective input field and set it - save into a variable for future use
        /// </summary>
        #region Getters and setters

        ///<summary>
        ///Get value of monthly deposit
        ///</summary>
        ///<returns>double monthly deposit</returns>
        public double GetMonDep()
        {
            return monthlyDeposit;
        }

        /// <summary>
        /// Set monthly deposit
        /// </summary>
        /// <remarks>
        /// Validate user input: make sure deposit is not negative
        /// </remarks>
        /// <param name="monthlyDeposit"></param>
        public void SetMonDep(double monthlyDeposit)
        {
            if(monthlyDeposit >= 0)
            {
                this.monthlyDeposit = monthlyDeposit;
            }

        }

        /// <summary>
        /// Get period
        /// </summary>
        /// <returns>double period</returns>
        public double GetPeriod()
        {
            return period;
        }

        /// <summary>
        /// Set period
        /// </summary>
        /// <remarks>
        /// Validate user input: make sure period is not negative
        /// </remarks>
        /// <param name="period"></param>
        public void SetPeriod(double period)
        {
            if(period >= 0)

            this.period = period;
        }

        /// <summary>
        /// Get initial deposit amount
        /// </summary>
        /// <returns>double initial deposit</returns>
        public double GetInitialDeposit()
        {
            return initialDeposit;
        }

        /// <summary>
        /// Set initial deposit
        /// </summary>
        /// <remarks>
        /// Validate user input: make sure the deposit is not negative
        /// </remarks>
        /// <param name="initialDeposit"></param>
        public void SetInitialDeposit(double initialDeposit)
        {
            if(initialDeposit >= 0)

            this.initialDeposit = initialDeposit;
        }

        /// <summary>
        /// Get interest rate
        /// </summary>
        /// <returns>double interest rate</returns>
        public double GetInterestRate()
        {
            return interestRate;
        }

        /// <summary>
        /// Set interest rate
        /// </summary>
        /// <remarks>
        /// Validate user input: make sure interest rate is not negative
        /// </remarks>
        /// <param name="interestRate"></param>
        public void SetInterestRate(double interestRate)
        {
            if(interestRate >= 0)

            this.interestRate = interestRate;
        }

        /// <summary>
        /// Get fees
        /// </summary>
        /// <returns>double fees</returns>
        public double GetFees()
        {
            return fees;
        }

        /// <summary>
        /// Set fees
        /// </summary>
        /// <remarks>
        /// Validate user input: make sure fees are not negative
        /// </remarks>
        /// <param name="fees"></param>
        public void SetFees(double fees)
        {
            if (fees >= 0)

            this.fees = fees;
        }

        /// <summary>
        /// Get when deposit was added (beginning or end of the period)
        /// </summary>
        /// <returns>true or false for if added at the beginning</returns>
        public bool GetWhenDepositAdded()
        {
            return depositAddedBegin;
        }

        /// <summary>
        /// Set when deposit added
        /// </summary>
        /// <param name="depositAddedBegin"></param>
        public void SetWhenDepositAdded(bool depositAddedBegin)
        {
            this.depositAddedBegin = depositAddedBegin;
        }
        #endregion

        /// <summary>
        /// Calculate compound interest and the future value of the investment
        /// </summary>
        /// <remarks>
        /// Formula: Future value = Initial deposit * (1 + interest rate/number of times interest is compounded per year)^number of times interest is compounded per year * time in years
        /// Number of times interest is compounded per year: for this calculation it is considered 12 times per year -> every month
        /// Initial deposit(ID):  BEGINNING OF THE PERIOD (interest applied after initial deposit and monthly deposit substracted) or END OF THE PERIOD (interest applied on the initial deposit before it is substracted to monthly deposit)
        /// Fee: for this calculation fees are considered to be added AFTER the interest rate is applied
        /// </remarks>
        /// <returns>double balance(future value of the investment)</returns>
        public double CalculateFutureValue()
        {
            double balance = 0.0; //future value
            double numOfMonths = period * 12; //period of investment converted to months
            double interestRateMonthly = interestRate / 100 / 12; //interest rate in percent yearly/100 --> decimals/12 --> interest per month
            double interestEarned = 0.0; //amount earned on interest
            double feesMonthly = fees / 100 / 12; //fees in percent yearly/100 --> decimals/12 --> fees per month
            double fee = 0.0; //amount on fees

            if (depositAddedBegin == true)
            {

                for (int i = 1; i <= numOfMonths; i++)
                {
                    if (i == 1) //consider deposit if first month 
                    {
                        interestEarned = interestRateMonthly * (monthlyDeposit + initialDeposit); //calculate interest from (monthly deposit plus initial deposit)
                        balance = initialDeposit + monthlyDeposit + interestEarned; //new balance = add calculated interest earned on top of initial deposit + monthly deposit
                        fee = balance * feesMonthly; //calculate the amount on fees (fees applied after the interest is added)
                        balance = balance - fee; //new balance = old balance - the calculated amount on fees

                    }
                    else //all other loopings happen with the new balance that has the deposit inside
                    {

                        interestEarned = interestRateMonthly * (balance + monthlyDeposit); //calculate interest from (balance created in the first iteration plus monthly deposit)
                        balance = interestEarned + monthlyDeposit + balance; //new balance  = interest earned from balance from the first iteration + monthly deposit + balance from first iteration;
                        fee = feesMonthly * balance; //calculate amount on fees(fees applied after the interest is added)
                        balance = balance - fee; //new balance = old balance - calculated amount on fees


                    }
                }
            }
            else
            {
                for (int i = 1; i <= numOfMonths; i++)
                {
                    if (i == 1) //consider deposit if first month
                    {

                        interestEarned = initialDeposit * interestRateMonthly; //calculate interest only from initial deposit
                        balance = initialDeposit + monthlyDeposit + interestEarned; //new balance = interest earned from initial deposit + monthly deposit + initial deposit
                        fee = balance * feesMonthly; //calculate the amount on fees (fees applied after the interest is added)
                        balance = balance - fee; //new balance = old balance - the calculated amount on fees

                    }
                    else //all other loopings happen with the new balance that has the deposit inside
                    {

                        interestEarned = interestRateMonthly * balance; //calculate amount earned from interest from the new balance(that has been created from both initial deposit and monthly deposit and added earned interest amound and deducted fees)
                        balance = interestEarned + monthlyDeposit + balance; //new balance  = interest earned from balance from first iteration + monthly deposit + the balance from the first iteration;
                        fee = feesMonthly * balance; //calculate amount on fees(fees applied after the interest is added)
                        balance = balance - fee; //new balance = old balance - calculated amount on fees

                    }
                }

            }

            return balance;

        }

        /// <summary>
        /// Calculate amount paid for the investment period
        /// <remarks>
        /// Formula: Amount Paid = Monthly deposit * Number of months
        /// </remarks>
        /// </summary>
        /// <returns>double amount paid for the period</returns>
        public double CalculateAmountPaid()
        {
            double amountPaid = 0.0;
            amountPaid = initialDeposit + (monthlyDeposit * (period * 12));

            return amountPaid;

        }

        /// <summary>
        /// Calculate amount earned for the investment period
        /// <remarks>
        /// Formula:
        /// Amount Earned = Final Balance - Amount Paid
        /// </remarks>
        /// </summary>
        /// <returns>double amount earned for the period</returns>

        public double CalculateAmountEarned()
        {
            double amountEarned = 0.0;
            amountEarned = CalculateFutureValue() - CalculateAmountPaid();
            return amountEarned;
        }

        /// <summary>
        /// Calculate total fees for the investment period
        /// </summary>
        /// <returns>double amount on fees for the period</returns>

        public double CalculateTotalFees()
        {
            double numOfMonths = period * 12;
            double feesMonthly = fees / 100 / 12;
            double interestEarned = 0.0;
            double fee = 0.0;
            double interestRateMonthly = interestRate / 100 / 12;
            double balance = 0.0;

            for (int i = 1; i <= numOfMonths; i++)
            {
                Console.WriteLine("Starting Step: " + i + ", Old Balance:" + balance);
                if (i == 1)
                {
                    interestEarned = (initialDeposit + monthlyDeposit) * interestRateMonthly;
                    balance = initialDeposit + monthlyDeposit + interestEarned;
                    fee = balance * feesMonthly;

                }
                else
                {

                    interestEarned = interestRateMonthly * (balance + monthlyDeposit);
                    balance = interestEarned + monthlyDeposit + balance;
                    fee = feesMonthly * balance;


                }
            }

            return fee;

        }



    }
}



