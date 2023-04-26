using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment3
{
    /// <summary>
    /// Class that contains BMI and BMR calculations and supporting methods.
    /// </summary>
    internal class BMICalculator
    {
        //instance variables for input
        private double height; //any unit
        private double weight; //any unit
        private int age; //age
        bool genderFemale = true; //male/female

        private ActivityLevel activity; //enum activity level to factor for maintain weight BMR

        private UnitTypes unit; //enum metric/imperial

        /// <summary>
        ///Default constructor method that is automatically called when an object is created and initiates enum instance variables
        /// </summary>
        /// <param>parameterless</param>

        public BMICalculator()
        {

            unit = UnitTypes.Metric;
            activity = ActivityLevel.Sedentary;


        }

        /// <summary>
        /// Getters and setters:
        /// get the value from the respective input field and set it - save into a variable for future use
        /// </summary>
        #region Getters and setters
        ///<summary>
        ///Get height
        ///</summary>
        ///<params>no params</params>
        /// <returns>double height</returns>
        public double GetHeight()
        {

            return height;

        }

        /// <summary>
        /// Set obtained height
        /// </summary>
        /// <remarks>
        /// Validate input: make sure height is not negative
        /// </remarks>
        /// <param name="height"></param>
        public void SetHeight(double height)
        {
            if (height >= 0.0)
            {
                this.height = height;
            }

        }

        /// <summary>
        /// Get weight
        /// </summary>
        /// <returns>double weight</returns>
        public double GetWeight()
        {

            return weight;

        }
        /// <summary>
        /// Set weight
        /// </summary>
        /// <remarks>
        /// Validate input: make sure weight is not negative
        /// </remarks>
        /// <param name="weight"></param>
        public void SetWeight(double weight)
        {

            if (weight >= 0.0)
            {
                this.weight = weight;
            }
        }

        /// <summary>
        /// Get unit type
        /// </summary>
        /// <returns>enum unit</returns>
        public UnitTypes GetUnitType()
        {
            return unit;

        }

        /// <summary>
        /// Set unit type
        /// </summary>
        /// <param name="unit"></param>
        public void SetUnitType(UnitTypes unit)
        {

            this.unit = unit;

        }

        /// <summary>
        /// Get Age
        /// </summary>
        /// <returns>double age</returns>
        public double GetAge()
        {
            return age;
        }

        /// <summary>
        /// Set age
        /// </summary>
        /// <remarks>
        /// Validate input: make sure age is not negative
        /// </remarks>
        /// <param name="age"></param>
        public void SetAge(int age)
        {
            if (age >= 0)
            {
                this.age = age;
            }

        }

        /// <summary>
        /// Get activity level
        /// </summary>
        /// <returns>enum activity</returns>
        public ActivityLevel GetActivityLevel()
        {
            return activity;

        }

        /// <summary>
        /// Set activity level
        /// </summary>
        /// <param name="activity"></param>
        public void SetActivityLevel(ActivityLevel activity)
        {

            this.activity = activity;

        }

        /// <summary>
        /// Get gender
        /// </summary>
        /// <returns>true or false for genderFemale</returns>
        public bool GetGender()
        {
            return genderFemale;
        }

        /// <summary>
        /// Set gender
        /// <remarks>
        /// value type bool - true or false
        /// </remarks>
        /// </summary>
        /// <param name="genderFemale"></param>
        public void SetGender(bool genderFemale)
        {
            this.genderFemale = genderFemale;
        }

        #endregion

        /// <summary>
        /// Calculate BMI and factor in unit type when performing the calculation
        /// </summary>
        /// <remarks>
        /// Formula Imperial: weight (lb) / (height (in) * height(in)) x 703
        /// Formula for Metric: weight(kg) / (height(cm) * height(cm))
        /// </remarks>
        /// <returns>double BMI value</returns>
        /// <example> 
        /// Weight = 150 lbs, Height = 5'5" (65")
        /// Calculation: [150 / (65)2] x 703 = 24.96
        ///  </example>
        public double CalculateBMI()
        {
            double bmiValue = 0.0;
            double factor = 703.0;

            if (unit == UnitTypes.Imperial)
            {

                bmiValue = factor * weight / (height * height);

            }
            else
            {

                bmiValue = weight / (height * height);
            }

            return bmiValue;
        }

        /// <summary>
        /// Give a BMI Category
        /// </summary>
        /// <returns>string with the value of BMI category</returns>
        public string BmiWeightCategory()
        {
            double bmi = CalculateBMI(); //always call a method to calculate BMI and save in bmi variable
            string stringout = string.Empty; //local variable (must be always initialised - in this case empty string)

            //save the respective weight category into a variable based on the result of the calculation
            if (bmi < 18.5)
            {
                stringout = "Underweight";
            }
            else if (bmi <= 24.9)
            {
                stringout = "Normal weight";
            }
            else if (bmi <= 29.9)
            {
                stringout = "Overweight (Preobesity)";
            }
            else if (bmi <= 34.9)
            {
                stringout = "Overweight (Obesity class I)";
            }
            else if (bmi <= 39.9)
            {
                stringout = "Overweight (Obesity class II)";
            }

            else if (bmi >= 40.0)
            {
                stringout = "Overweight (Obesity class III)";
            }

            return stringout;
        }


        /// <summary>
        /// Calculate the normal weight range
        /// </summary>
        /// <remarks>
        /// Formula:
        /// factor = 703 for US and 1 for metric
        /// weight = height * height / factor;
        /// low limit weightMin = weight * 18.50;
        /// high limit weightMax = weight * 24.9; 
        /// </remarks>
        /// <returns></returns>
        public string CalculateNormalWeight()
        {

            double weightMin = 0.0;
            double weightMax = 0.0;
            double weight = 0.0;
            string stringout = string.Empty;

            double factor = 703.0;

            if (unit == UnitTypes.Imperial)
            {
                weight = height * height / factor;
                weightMin = weight * 18.50;
                string weightMinStr = weightMin.ToString("0");
                weightMax = weight * 24.9;
                string weightMaxStr = weightMax.ToString("0");
                stringout = ("Normal weight should be between " + weightMinStr + " and " + weightMaxStr + " lbs.");
            }
            else
            {
                weight = height * height;
                weightMin = weight * 18.50;
                string weightMinStr = weightMin.ToString("0");
                weightMax = weight * 24.9;
                string weightMaxStr = weightMax.ToString("0");
                stringout = ("Normal weight should be between " + weightMinStr + " and " + weightMaxStr + " kg.");
            }

            return stringout;
        }



        /// <summary>
        /// Calculate BMR
        /// </summary>
        /// <remarks>
        /// Used equation: Mifflin - St Jeor equation
        /// Formulas:
        /// Metric:
        /// For men: (10 × weight in kg) + (6.25 × height in cm) - (5 × age in years) + 5
        /// For women: (10 × weight in kg) + (6.25 × height in cm) - (5 × age in years) - 161
        /// Imperial:
        /// For men: (4.536 × weight in pounds) + (15.88 × height in inches) - (5 × age) + 5
        /// For women: (4.536 × weight in pounds) + (15.88 × height in inches) - (5 × age) - 161
        /// </remarks>
        /// <returns>double BMR value = how much calories your body burns to perform necessary functions</returns>

        public double CalculateBMR()
        {
            double bmrValue = 0.00;
            bool genderFemale = GetGender();

            if (unit == UnitTypes.Metric)
            {

                if (genderFemale == true)
                {
                    bmrValue = (10 * weight) + (6.25 * (height * 100)) - (5 * age) - 161;
                }
                else
                {
                    bmrValue = (10 * weight) + (6.25 * (height * 100)) - (5 * age) + 5;
                }
            }

            else
            {
                if (genderFemale == true)
                {
                    bmrValue = Math.Round((4.536 * weight) + (15.88 * height) - (5 * age) - 161);
                }
                else
                {
                    bmrValue = Math.Round((4.536 * weight) + (15.88 * height) - (5 * age) + 5);
                }
            }

            return bmrValue;

        }


        /// <summary>
        /// Calculate BMR to maintain current weight
        /// <remarks>
        /// based on the Mifflin - St Jeor equation
        /// </remarks>
        /// </summary>
        /// <returns>double BMR value (number of calories to consume in order to maintain current weight)</returns>

        public double MaintainWeightBMR()
        {
            double maintainWeightBMR = 0.00;
            double factor = AssignFactor();

            maintainWeightBMR = CalculateBMR() * factor;

            return maintainWeightBMR;
            
        }

        /// <summary>
        /// Assign activity level factor based on the activity level
        /// </summary>
        /// <remarks>
        /// Formula:
        /// Sedentary (little or no exercise) = BMR x 1.2
        /// Lightly active(light exercise or sports 1-3 days/week) = BMR x 1.375
        /// Moderately active(moderate exercise 3-5 days/week) = BMR x 1.55
        /// Very active(hard exercise 6-7 days/week) = BMR x 1.725
        /// Extra active(very hard exercise and a physical job) = BMR x 1.9
        /// </remarks>
        /// <returns>BMR value(calories per day)</returns>

        public double AssignFactor()
        {
            double factor = 0.00;
            ActivityLevel activity = GetActivityLevel();

            switch (activity)
            {
                case ActivityLevel.Sedentary:
                    factor = 1.2;
                    break;

                case ActivityLevel.LightlyActive:
                    factor = 1.375;
                    break;

                case ActivityLevel.ModeratelyActive:
                    factor = 1.55;
                    break;

                case ActivityLevel.VeryActive:
                    factor = 1.725;
                    break;

                case ActivityLevel.ExtraActive:
                    factor = 1.9;
                    break;

                default:
                    break;

            }

            return factor;
        }


        /// <summary>
        /// Calculate how many calories daily to intake if you want to lose 0,5kg
        /// <remarks>
        /// Formula: Calories to maintain weight - 500 calories
        /// </remarks>
        /// </summary>
        /// <returns>double BMR value (number of calories to intake to lose 0,5kg</returns>
        public double LoseHalfKiloBMR()
        {
            double resultBmr = 0.0;

            resultBmr = MaintainWeightBMR() - 500;

            return resultBmr;
        }

        /// <summary>
        /// Calculate how many calories daily to intake if you want to lose 1kg
        /// <remarks>
        /// Formula: Calories to maintain weight - 1000 calories
        /// </remarks>
        /// </summary>
        /// <returns>double BMR value (number of calories to intake to lose 1kg</returns>
        public double LoseKiloBMR()
        {
            double resultBmr = 0.0;

            resultBmr = MaintainWeightBMR() - 1000;

            return resultBmr;
        }

        /// <summary>
        /// Calculate how many calories daily to intake if you want to gain 0,5kg
        /// <remarks>
        /// Formula: Calories to maintain weight + 500 calories
        /// </remarks>
        /// </summary>
        /// <returns>double BMR value (number of calories to intake to gain 0,5kg</returns>
        public double GainHalfKiloBMR()
        {
            double resultBmr = 0.0;

            resultBmr = MaintainWeightBMR() + 500;

            return resultBmr;
        }


        /// <summary>
        /// Calculate how many calories daily to intake if you want to gain 1kg
        /// <remarks>
        /// Formula: Calories to maintain weight + 1000 calories
        /// </remarks>
        /// </summary>
        /// <returns>double BMR value (number of calories to intake to gain 1kg</returns>
        public double GainKiloBMR()
        {
            double resultBmr = 0.0;

            resultBmr = MaintainWeightBMR() + 1000;

            return resultBmr;
        }


    }
}
