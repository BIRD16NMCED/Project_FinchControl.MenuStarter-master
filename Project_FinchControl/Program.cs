﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control - Menu Starter
    // Description: Starter solution with the helper methods,
    //              opening and closing screens, and the menu
    // Application Type: Console
    // Author: Spencer Bird (framework by John Velis)
    // Dated Created: 1/22/2020
    // Last Modified: 10/4/2020
    //
    // **************************************************

    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot); //methods like these have access to the finch robot like case b 
                        break;

                    case "b":
                        DisplayTalentShowMenuScreen(finchRobot);
                        break;

                    case "c":
                        DataRecorderMenuScreen(finchRobot);
                        break;

                    case "d":
                        //DisplayAlarmSystemMenuScreen(finchRobot);
                        break;

                    case "e":
                        // DisplayUserProgrammingMenuScreen(finchRobot);
                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region DATA RECORDER

        /// <summary>
        /// *****************************************************************
        /// *                  Data Recorder for Finch                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static void DataRecorderMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;  //generates menu parameter quitMenu
            string menuChoice;
            int numOfDataPoints = 0;
            double frequencyOfDataPoints = 0;       // sub menu has access to finchrobot, variables, and its going to get values and send those to methods so they can func.
            double[] temperatures = null;



            do
            {
                DisplayScreenHeader("Data Recorder Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Data Point Frequency");
                Console.WriteLine("\tb) Display Number of Data Points");
                Console.WriteLine("\tc) Record Data");
                Console.WriteLine("\td) Display Data Table");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {

                    case "a":
                        frequencyOfDataPoints = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "b":
                        numOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints(); // going to bring back an integer value
                        break;
                    case "c":
                        if (numOfDataPoints == 0 || frequencyOfDataPoints == 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Please indicate the number and frequency of data points first.");
                            Console.WriteLine();
                            DisplayContinuePrompt();
                        }
                        else
                        {
                            temperatures = DataRecorderDisplayGetData(numOfDataPoints, frequencyOfDataPoints, finchRobot);
                        }
                        break;
                    case "d":
                        DatarecorderDisplayData(temperatures);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }


        /// <summary>
        /// *****************************************************************
        /// *               Data Recorder > Display Data Points    *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            int numOfDataPoints;
            DisplayScreenHeader("Number of Data Points");

            Console.Write("\tEnter The Number of Data Points:");      // remember to include validation
            int.TryParse(Console.ReadLine(), out numOfDataPoints);  // kicks out validation to readline and pushes out number of data points 

            Console.WriteLine();
            Console.WriteLine($"\tNumber of Data Points: {numOfDataPoints}");  // allows the passthru of data points into output 

            DisplayContinuePrompt();

            return numOfDataPoints;
        }

        /// <summary>
        /// *****************************************************************
        /// *               Data Recorder > Make Data Table From Temp    *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static void DataRecorderDisplayTable(double[] temperatures)  
        {
            Console.WriteLine(
                "Recording".PadLeft(15) +       // converts table headers
                "Temperature".PadLeft(15)
                );

            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine(
                (index + 1).ToString().PadLeft(15) +
                temperatures[index].ToString("n2").PadLeft(15)  //have to convert in this way for table values 
                );
            }
        }


        /// <summary>
        /// *****************************************************************
        /// *                  Data Recorder > Display Data        *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static void DatarecorderDisplayData(double[] temperatures)
        {
            DisplayScreenHeader("Data Set:");


            DataRecorderDisplayTable(temperatures);  // single responsibility principle for function

      
            DisplayContinuePrompt();
        }



        /// <summary>
        /// *****************************************************************
        /// *               Data Recorder > Get and Echo Temp Reading    *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static double[] DataRecorderDisplayGetData(int numOfDataPoints, double frequencyOfDataPoints, Finch finchRobot)
        {
            double[] temperatures = new double[numOfDataPoints];  //always goes to the heap
            DisplayScreenHeader("Get Data Points");

            Console.WriteLine($"\tNumber Of Data Points: {numOfDataPoints}");
            Console.WriteLine($"\tFrequency Of Date Points {frequencyOfDataPoints}");
            Console.WriteLine();

            Console.WriteLine("The Finch robot is ready to record the temperatures");
            Console.ReadKey();
            Console.WriteLine();

            double temperature;
            int milliseconds;
            for (int index = 0; index < numOfDataPoints; index++)
            {
                temperature = finchRobot.getTemperature();
                Console.WriteLine($"\t\tTemperature Reading {index + 1}: {temperature}");
                temperatures[index] = temperature;
                milliseconds = (int)(frequencyOfDataPoints * 1000);



            }

            Console.WriteLine();
            Console.WriteLine("The Data Recording is Complete.");

            DisplayContinuePrompt();

            return temperatures;
        }

        //summary
        //celcius to fahrenheit conversion
        //summary
        static double ConvertCelciusToFahrenheit(double celciusTemp)
        {
            double fahrenheit;

            fahrenheit = celciusTemp * 9 / 5 + 32;

            Console.WriteLine();
            Console.WriteLine(celciusTemp);
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine(fahrenheit);
            Console.WriteLine();
           
            return fahrenheit;
        } 


        /// <summary>
        /// *****************************************************************
        /// *                  Data Recorder > Data Point Frequency         *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static double DataRecorderDisplayGetDataPointFrequency()
        {
            double frequencyOfDataPoints;
            DisplayScreenHeader("Frequency of Data Points");

            Console.Write("\tEnter the Frequency of Data Points:");      // remember to include validation
            double.TryParse(Console.ReadLine(), out frequencyOfDataPoints);  // kicks out validation to readline and pushes out number of data points 

            Console.WriteLine();
            Console.WriteLine($"\tFrequency of Data Points: {frequencyOfDataPoints}");  // allows the passthru of data points into output 

            DisplayContinuePrompt();

            return frequencyOfDataPoints;
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Alarm System for Finch                       *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static void DisplayAlarmSystemMenuScreen(Finch FinchRobot)
        {
            DisplayScreenHeader("Alarm System");
            Console.WriteLine("This Module is under Development.");
            Console.WriteLine();
            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                  User Programming for Finch                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static void DisplayUserProgrammingMenuScreen(Finch FinchRobot)
        {
            DisplayScreenHeader("User Programming");
            Console.WriteLine("This Module is under Development.");
            Console.WriteLine();
            DisplayContinuePrompt();
        }


        #endregion

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch myFinch)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Do a Dance");
                Console.WriteLine("\tc) Rave Party");
                Console.WriteLine("\td) Simple Song");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayLightAndSound(myFinch);
                        break;

                    case "b":
                        DisplayDoADance(myFinch);
                        break;

                    case "c":
                        DisplayRaveParty(myFinch);
                        break;

                    case "d":
                        DisplayQuickSong(myFinch);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }
        static void DisplayDoADance(Finch myFinch) //add to menu to appear to user, add to switch case structure, and then add the method like this 
        {
            DisplayScreenHeader("Do A Dance");
            Console.WriteLine("\tThe Finch robot will now do a dance!");
            DisplayContinuePrompt();

            for (int i = 0; i < 10; i++) //makes the finch waddle when selected 
            {
                myFinch.setMotors(255, 128);
                myFinch.wait(250);
                myFinch.setMotors(-128, -255);
                myFinch.wait(250);
                myFinch.setMotors(0, 0); //stops the motors on the finch 
            }


            DisplayContinuePrompt(); // remember to include for loopback
        }


        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light and Sound                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayLightAndSound(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will glow up and resonate a tune!");
            DisplayContinuePrompt();

            for (int lightSoundLevel = 0; lightSoundLevel < 69; lightSoundLevel++)
            {
                finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
                finchRobot.noteOn(lightSoundLevel * 523);
                finchRobot.setLED(0, 0, 0);
                finchRobot.noteOff();
            }

            DisplayMenuPrompt("Talent Show Menu");
        }


        /// <summary>
        /// *****************************************************************
        /// *               Talent Show Rave Dance                          *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayRaveParty(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Rave Party");

            Console.WriteLine("\t the finch robot will light up and dance while firing off some notes.");
            DisplayContinuePrompt();

            for (int RaveDance = 0; RaveDance < 15; RaveDance++)
            {
                finchRobot.setLED(RaveDance, RaveDance, RaveDance);
                finchRobot.noteOn(RaveDance * 587);
                finchRobot.setMotors(128, 255);
                finchRobot.wait(125);
                finchRobot.setMotors(128, 255);
                finchRobot.wait(125);
                finchRobot.setLED(0, 0, 0);
                finchRobot.noteOff();
                finchRobot.setMotors(0, 0);

            }
            DisplayMenuPrompt("Talent Show Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show Quick Jingle                        *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayQuickSong(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Quick Song");

            Console.WriteLine("\t The finch will play a quick jingle for your ears.");
            DisplayContinuePrompt();

            for (int QuickJingle = 0; QuickJingle < 12; QuickJingle++)
            {
                finchRobot.noteOn(QuickJingle * 523);
                finchRobot.wait(250);
                finchRobot.noteOff();
                finchRobot.noteOn(QuickJingle * 698);
                finchRobot.wait(250);
                finchRobot.noteOff();

            }
            DisplayMenuPrompt("Finch Talent Show");
        }

        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnect.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected = false;
            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();


            if (finchRobot.connect())
            {
                finchRobot.setLED(0, 255, 0);
                finchRobot.noteOn(261);
                finchRobot.wait(1000);
                finchRobot.noteOff();
                finchRobot.setLED(0, 0, 0);
            }

            // TODO test connection and provide user feedback - text, lights, sounds

            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion



    }
}

