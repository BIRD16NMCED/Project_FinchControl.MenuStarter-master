using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
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

    public enum Command
    {
        NONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        WAIT,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF,
        NOTEON,
        NOTEOFF,
        GETTEMPERATURE,
        DONE

    }
    
    
    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();
            DataDisplaySetTheme();
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
                Console.WriteLine("\tf) Change Color Theme");
                Console.WriteLine("\tg) Username and Password");
                Console.WriteLine("\th) Disconnect Finch Robot");
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
                        AlarmSystemDisplayMenu(finchRobot);
                        break;

                    case "e":
                         DisplayUserProgrammingMenuScreen(finchRobot);
                        break;

                    case "f":
                        DisplayChangeThemeMenu();
                        break;

                    case "g":
                        DiplsayUserNamePasswordMenu();
                        break;

                    case "h":
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

        /// <summary>
        /// Celsius to Fahrenheit converison
        /// </summary>
        /// <returns></returns>
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
        #endregion

        #region ALARM SYSTEM
        /// <summary>
        /// *****************************************************************
        /// *                  Alarm System for Finch                       *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static void AlarmSystemDisplayMenu(Finch finchRobot)  // finchrobot lives in the HEAP
        {
            Console.CursorVisible = true;

            bool quitMenu = false;  //generates menu parameter quitMenu
            string menuChoice;

            string sensorToMonitor= "";
            string rangeType = ""; // initilizes with an empty string 
            int minMaxThresholdValue = 0;
            int timeToMonitor = 0;


            do
            {
                DisplayScreenHeader("Alarm System Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Sensors To monitor");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) Set Min/Max ThreshHolds");
                Console.WriteLine("\td) Set Time to Monitor");
                Console.WriteLine("\te) Set Alarm");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {

                    case "a":
                        sensorToMonitor = AlarmSystemDisplaySensorToMonitor();  //add alarmsys
                        break;

                    case "b":
                        rangeType = AlarmSystemDisplayRangeType();
                        break;
                    case "c":
                        minMaxThresholdValue = AlarmSystemDisplaySetMinMaxThreshold(finchRobot, rangeType);
                        break;
                    case "d":
                        timeToMonitor = AlarmSystemTimeToMonitor();
                        break;

                    case "e":
                        if (sensorToMonitor == "" || rangeType == "" || minMaxThresholdValue == 0 || timeToMonitor == 0)
                        {
                            Console.WriteLine("Please enter all required values.");
                            DisplayContinuePrompt();
                        }
                        else
                        {
                            AlarmSystemSetAlarm(finchRobot, sensorToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);
                        }
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

        static void AlarmSystemSetAlarm(Finch finchRobot, string sensorToMonitor, string rangeType, int minMaxThresholdValue, int timeToMonitor)  //use automatic build exceptions saves time
        {
            DisplayScreenHeader("Set Alarm");

            Console.WriteLine("\tAlarm Settings");
            Console.WriteLine($"\tSensor to Monitor: {sensorToMonitor}");
            Console.WriteLine($"\tRange Type: {rangeType}");
            Console.WriteLine($"\tMin/Max Threshold Value: {minMaxThresholdValue}");
            Console.WriteLine($"\tTime To Monitor: {timeToMonitor}");

            Console.WriteLine();
            Console.WriteLine("Press any Key to Start the Alarm System");
            Console.CursorVisible = false;
            Console.ReadKey();
            Console.CursorVisible = true;

            //
            // get current light values by switch statement from userInput 
            //
            //for (int second = 1; second <= timeToMonitor; second++)
            //{

            //    Console.WriteLine($"\t\tTime: {second}");

            //    if (AlarmSystemThresholdExceeded(finchRobot, sensorToMonitor, rangeType, minMaxThresholdValue))
            //    {
            //        Console.WriteLine("Threshold  Exceeded");
            //        break; //stops loop when threshold is exceeded 
            //    }
            //    finchRobot.wait(1000);
            //}

            //
            // loop for continous monitoring (YOU HAVE TO CHECK BOTH TEMP AND LIGHT LEVEL OR SOUND HERE) 
            //
            int second = 1;
            bool thresholdExceeded = AlarmSystemThresholdExceeded(finchRobot, sensorToMonitor, rangeType, minMaxThresholdValue);
            while (!thresholdExceeded && second <= timeToMonitor) // havent exceeded threshold and time to monitor it will continue to loop logic is cleaner 
            {
                Console.SetCursorPosition(10, 12);
                Console.WriteLine($"\tTime: {second++}");
                finchRobot.wait(1000);
                thresholdExceeded = AlarmSystemThresholdExceeded(finchRobot, sensorToMonitor, rangeType, minMaxThresholdValue);
            }
            //
            // Display Status 
            //
            if (second > timeToMonitor) // boundary condition (example of one)
            {
                Console.WriteLine();
                Console.WriteLine("\tThreshold Not Exceeded"); 
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("\tThreshold Exceeded");
                for (int SoundLevel = 0; SoundLevel < 69; SoundLevel++)
                {
                    
                    finchRobot.noteOn(SoundLevel * 523);
                    finchRobot.noteOff();
                }
            }
            DisplayContinuePrompt();
        }


            //if (var1 > var 2 || var 3 > var4)
            //{
            //thresholdExceeded = true;
            //}
            // want this for exceeded threshold 
       

        static bool AlarmSystemThresholdExceeded(Finch finchRobot, string sensorToMonitor, string rangeType, int minMaxThresholdValue)
        {
            // becomes simpler because we moved logic from above if statement to below for checking
            //
            //get the current light value
            //
            int currentLeftLightSensorValue = finchRobot.getLeftLightSensor();
            int currentRightLightSensorValue = finchRobot.getRightLightSensor();

            bool thresholdExceeded = false;
            switch (sensorToMonitor)
            {
                case "left":
                    if (rangeType == "minimum")
                    {
                        thresholdExceeded = currentLeftLightSensorValue > minMaxThresholdValue;
                        thresholdExceeded = true;
                    }
                    else
                    {
                        thresholdExceeded = currentLeftLightSensorValue < minMaxThresholdValue;
                        thresholdExceeded = false;
                        
                    }
                    if (rangeType == "maximum") 
                    {
                        thresholdExceeded = currentLeftLightSensorValue > minMaxThresholdValue;
                        thresholdExceeded = true; 
                    }
                    else
                    {
                        thresholdExceeded = currentLeftLightSensorValue < minMaxThresholdValue;
                        thresholdExceeded = false;
                    }
                    break;

                case "right":
                    if (rangeType == "minimum")
                    {
                        thresholdExceeded = currentRightLightSensorValue > minMaxThresholdValue;
                        thresholdExceeded = true;
                    }
                    else
                    {
                        thresholdExceeded = currentRightLightSensorValue < minMaxThresholdValue;
                        thresholdExceeded = false;
                    }
                    if (rangeType == "maximum")
                    {
                        thresholdExceeded = currentRightLightSensorValue > minMaxThresholdValue;
                        thresholdExceeded = true;
                    }
                    else
                    {
                        thresholdExceeded = currentRightLightSensorValue < minMaxThresholdValue;
                        thresholdExceeded = false;
                    }
                    break;

                case "both":
                    if (rangeType == "minimum")
                    {
                        thresholdExceeded = (currentLeftLightSensorValue > minMaxThresholdValue) || (currentRightLightSensorValue > minMaxThresholdValue);
                        thresholdExceeded = true;
                    }
                    else
                    {
                        thresholdExceeded = (currentLeftLightSensorValue < minMaxThresholdValue) || (currentRightLightSensorValue < minMaxThresholdValue);
                        thresholdExceeded = false;
                    }
                    if (rangeType == "maximum")
                    {
                        thresholdExceeded = (currentLeftLightSensorValue > minMaxThresholdValue) || (currentRightLightSensorValue > minMaxThresholdValue);
                        thresholdExceeded = true; 
                    }
                    else
                    {
                        thresholdExceeded = (currentLeftLightSensorValue < minMaxThresholdValue) || (currentRightLightSensorValue < minMaxThresholdValue);
                        thresholdExceeded = false;
                    }
                    break;

                    default:
                        Console.WriteLine("Sensor Value incorrect, please enter a correct value.");
                        DisplayContinuePrompt();
                    break;
            }
            return thresholdExceeded;
            
        }

        static int AlarmSystemTimeToMonitor()
        {
            int timeToMonitor = 0;
            DisplayScreenHeader("Time to monitor");

            Console.WriteLine("Enter the time to monitor (seconds).");
           
            int.TryParse(Console.ReadLine(), out timeToMonitor);
           
            DisplayContinuePrompt();
            
            Console.WriteLine($"Time To monitor: {timeToMonitor} seconds");

            DisplayContinuePrompt();

            return timeToMonitor;
        }
        /// <summary>
        /// Get Min/Max Threshold Value From User
        /// </summary>
        /// <returns></MinMax threshold value>
        private static int AlarmSystemDisplaySetMinMaxThreshold(Finch finchRobot, string rangeType)
        {
            int minMaxThresholdValue = 0;  // since int not string set intial value to 0 (we also have to give them the ambient value) (they also need the FinchRobot)

            DisplayScreenHeader("Min/Max Threshold Value");

            Console.WriteLine($"Ambient Left Light Level: {finchRobot.getLeftLightSensor()}");
            Console.WriteLine($"Ambient Right Light Level: {finchRobot.getRightLightSensor()}");

            Console.Write($"Enter the {rangeType} Threshold Value");  // makes so you enter either minimum or maximum threshold value MAKE SURE TO VALIDATE THIS 
            minMaxThresholdValue = Convert.ToInt32(Console.ReadLine());  // conveerts string to int 
            
            DisplayContinuePrompt();

            return minMaxThresholdValue;
        }

        /// <summary>
        /// Get Range Type From User
        /// </summary>
        /// <returns></getRange>
        static string AlarmSystemDisplayRangeType()  // MAKE SURE TO VALIDATE THE USERINPUT FOR ALL NEW METHODS THIS WAS NOT BUILT W/ VELIS
        {
            string rangeType = "";

            DisplayScreenHeader("Range Type");

            Console.Write("Enter Range Type [minimum, maximum]: ");
            
            rangeType = Console.ReadLine();

            Console.WriteLine($"Range Type: {rangeType}.");

            DisplayContinuePrompt();
            
            return rangeType;
        }

        static string AlarmSystemDisplaySensorToMonitor()
        {
            string sensorsToMonitor = "";  // during evaluation you can initialize it as an empty string 

            DisplayScreenHeader("Sensors to Monitor");
           
            Console.Write("Enter Sensors to Monitor [left, right, Both]");
            
            sensorsToMonitor = Console.ReadLine();

            Console.WriteLine($"Sensors to monitor: {sensorsToMonitor}.");
           
            DisplayContinuePrompt();

           return sensorsToMonitor;
        }
        //int LightAlarmDisplaySetMaximumTimeToMonitor()
        //{
        //    int MaxTimeToMonitor = 0;

        //    DisplayScreenHeader("Maximum Time To Monitor");

        //    Console.WriteLine("Enter the maximum time in seconds to monitor in integer form.");

        //    MaxTimeToMonitor = Convert.ToInt32(Console.ReadLine());

        //    Console.WriteLine($"so you will take {MaxTimeToMonitor} seconds to monitor the light values.");

        //    DisplayContinuePrompt();

        //    return MaxTimeToMonitor;
        //}

        //void LightAlarmSystemDisplaySetAlarm(Finch finchRobot, string sensorsToMonitor, string rangeType, double minMaxThresholdValue, int timeToMonitor)
        //{

        //    DisplayScreenHeader("Set Alarm System");
        //    Console.WriteLine($"Sensor(s) to Montitor: {sensorsToMonitor}");
        //    Console.WriteLine($"Range Type for Sensor(s): {rangeType}");
        //    Console.WriteLine($"");

        //}
        #endregion

        #region USER PROGRAMMING
        /// <summary>
        /// *****************************************************************
        /// *                  User Programming for Finch                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static void DisplayUserProgrammingMenuScreen(Finch FinchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;  //generates menu parameter quitMenu
            string menuChoice;

            (int motorSpeed, int LedBrightness, double waitSeconds) commandParameters = (0, 0, 0);
            List<Command> commands = new List<Command>(); //menu

            do
            {
                DisplayScreenHeader("\tUser Programming Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Parameters");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execute Commands");
                Console.WriteLine("\te) Main Menu");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {

                    case "a":
                        commandParameters = UserProgrammingDisplayGetCommandParameters();
                        break;

                    case "b":
                        UserProgrammingDisplayGetCommands(commands);
                        break;
                    case "c":
                        UserProgrammingDisplayCommands(commands);
                        break;
                    case "d":
                        UserProgrammingDisplayGetCommandParametersDisplayExecuteCommands(FinchRobot, commands, commandParameters);
                        break;
                    case "q":
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

        static void UserProgrammingDisplayGetCommandParametersDisplayExecuteCommands(Finch finchRobot, List<Command> commands, (int motorSpeed, int ledBrightness, 
            double waitSeconds) commandParameters)
        {
            DisplayScreenHeader("\tExecute Commands");

            Console.WriteLine("\t The Finch robot is ready to execute the list of commands.");
            DisplayContinuePrompt();

            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:
                        Console.WriteLine("\t\t Invalid Command.");
                        break;
                   
                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(commandParameters.motorSpeed, commandParameters.motorSpeed);
                        break;
                    
                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-commandParameters.motorSpeed, -commandParameters.motorSpeed);
                        break;
                   
                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        break;
                    
                    case Command.WAIT:
                        int waitMilliseconds = (int)(commandParameters.waitSeconds * 1000);
                        finchRobot.wait(waitMilliseconds);
                        break;
                    
                    case Command.TURNRIGHT:
                        finchRobot.setMotors(255, 128);
                        break;
                    
                    case Command.TURNLEFT:
                        finchRobot.setMotors(-128, 255);
                        break;
                   
                    case Command.LEDON:
                        finchRobot.setLED(commandParameters.ledBrightness, commandParameters.ledBrightness, commandParameters.ledBrightness);
                        break;
                   
                    case Command.LEDOFF:
                        finchRobot.setLED(0, 0, 0);
                        break;
                   
                    case Command.NOTEON:
                        finchRobot.noteOn(523);
                        break;
                   
                    case Command.NOTEOFF:
                        finchRobot.noteOff();
                        break;
                   
                    case Command.GETTEMPERATURE:  // want to get & DISPLAY TEMPERATURE
                        finchRobot.getTemperature();
                        Console.WriteLine($"\t\tTemperature: {finchRobot.getTemperature()} °C");
                        break;
                    
                    case Command.DONE:
                        break;
                    default:
                        break;
                }

                Console.WriteLine("\t\t" + command);
            }

            DisplayMenuPrompt("User Programming");
        }

        static void UserProgrammingDisplayCommands(List<Command> commands) //displays user programming commands 
        {
            DisplayScreenHeader("Commands");
            foreach (Command command in commands)
            {
                Console.WriteLine("\t" + command);
                DisplayContinuePrompt();
            }
        }

        static void UserProgrammingDisplayGetCommands(List<Command> commands)
        {
            bool validResponse;
            bool isDoneEditing = false;
            string userResponse;
            Command command;

            DisplayScreenHeader("Commands");

            foreach (var commandName in Enum.GetValues(typeof(Command)))
            {
                //Console.WriteLine($"\t{commandName}");

                if (commandName.ToString() != "NONE") Console.WriteLine("\t" + commandName);
                {
                    //Console.WriteLine("\t" + commandName);
                }
            }
            
            do
            {
                validResponse = true;

                Console.Write("Enter Command:");
                userResponse = Console.ReadLine();

                if (userResponse != "DONE")
                {
                    if (!Enum.TryParse(userResponse, out command))
                    {
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a proper command");
                        DisplayContinuePrompt();
                    }
                    else
                    {
                        commands.Add(command); // logic for processing a command 
                    }
                }
                else
                {
                    isDoneEditing = true;
                }

            } while (!validResponse || !isDoneEditing);

            DisplayMenuPrompt("User Programming");
        }

        static (int motorSpeed, int LedBrightness, double waitSeconds) UserProgrammingDisplayGetCommandParameters()
        {
            (int motorSpeed, int LedBrightness, double waitSeconds) commandParameters;
            commandParameters = (0, 0, 0);

            DisplayScreenHeader("Command Parameters");

            Console.Write("Enter the motor speed [1 - 255]");
            int.TryParse(Console.ReadLine(), out commandParameters.motorSpeed);
           
            Console.WriteLine($"User Motor Speed: {commandParameters.motorSpeed}");
            DisplayContinuePrompt();

            Console.Write("Enter the LED Brightness [1 - 255]");
            int.TryParse(Console.ReadLine(), out commandParameters.LedBrightness);

            Console.WriteLine($"User LED Brightness: {commandParameters.LedBrightness}");
            DisplayContinuePrompt();
            
            Console.Write("Enter the Wait [seconds] [1 - 255]");
            double.TryParse(Console.ReadLine(), out commandParameters.waitSeconds);

            Console.WriteLine($"User Wait Time: {commandParameters.waitSeconds} Seconds");
            DisplayContinuePrompt();
            
            DisplayMenuPrompt("User Programming");


            return commandParameters;
        }


        #endregion

        #region FILEIO
        static void DisplayChangeThemeMenu()
        {
            bool quitMenu = false;
            string menuChoice;
            do
            {
                DisplayScreenHeader("\t\tChange Theme Color");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Change Theme");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                switch (menuChoice)
                {
                    case "a":
                        DataDisplaySetTheme();
                        break;
                    
                    case "q":
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
        /// *                FileIO File Reading for Theme                  *
        /// *****************************************************************
        /// </summary>
        static(ConsoleColor foregroundColor, ConsoleColor backgroundColor) ReadThemeData() 
        {
            string dataPath = @"DataFolder/ColorFile.txt";
            string[] themeColors;

            ConsoleColor foregroundColor;
            ConsoleColor backgroundColor;

            themeColors = File.ReadAllLines(dataPath);

            Enum.TryParse(themeColors[0], true, out foregroundColor);
            Enum.TryParse(themeColors[1], true, out backgroundColor);

            return (foregroundColor, backgroundColor);

        }

       
        
        /// <summary>
        /// *****************************************************************
        /// *               FileIO Write Theme Data to File                 *
        /// *****************************************************************
        /// </summary>
        static void WriteThemeData(ConsoleColor foreground, ConsoleColor background)
        {
            string dataPath = @"DataFolder/ColorFile.txt";

            File.WriteAllText(dataPath, foreground.ToString() + "\n");
            File.AppendAllText(dataPath, background.ToString());
        }
        
        
        
        /// <summary>
        /// *****************************************************************
        /// *                    Enter ConsoleColor Input                   *
        /// *****************************************************************
        /// </summary>
        static ConsoleColor GetConsoleColorFromUser(string property)
        {
            ConsoleColor consoleColor;
            bool validConsoleColor;

            do
            {
                Console.Write($"\tEnter a value for the {property}:");
                validConsoleColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);

                if (!validConsoleColor)
                {
                    Console.WriteLine("\n\t***** It appears you did not provide a valid console color. Please try again *****\n");
                }
                else
                {
                    validConsoleColor = true;
                }
            } while (!validConsoleColor);

            return consoleColor;
        }


        /// <summary>
        /// *****************************************************************
        /// *                    Set and Display FileIO                     *
        /// ***************************************************************** 
        /// </summary>
        static void DataDisplaySetTheme()
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            bool themeChosen = false;

            //
            // Set current theme from data
            //
            themeColors = ReadThemeData();
            Console.ForegroundColor = themeColors.foregroundColor;
            Console.BackgroundColor = themeColors.backgroundColor;
            Console.Clear();
            DisplayScreenHeader("Set Application Theme");

            Console.WriteLine($"\tCurrent foreground color: {Console.ForegroundColor}");
            Console.WriteLine($"\tCurrent background color: {Console.BackgroundColor}");
            Console.WriteLine();
            

            Console.Write("\tWould you like to change the current theme [ yes | no ]?");
            if (Console.ReadLine().ToLower() == "yes")
            {
                do
                {
                    themeColors.foregroundColor = GetConsoleColorFromUser("foreground");
                    themeColors.backgroundColor = GetConsoleColorFromUser("background");
                    
                    //
                    // Set New Theme
                    //
                    Console.ForegroundColor = themeColors.foregroundColor;
                    Console.BackgroundColor = themeColors.backgroundColor;
                    Console.Clear();
                    DisplayScreenHeader("Set Application Theme");
                    Console.WriteLine($"\tNew Foreground Color: {Console.ForegroundColor}");
                    Console.WriteLine($"\tNew Background Color: {Console.BackgroundColor}");

                    Console.WriteLine();
                    Console.WriteLine("\tIs this the theme you would like? [ yes | no ]");
                    if (Console.ReadLine().ToLower() == "yes")
                    {
                        themeChosen = true;
                        WriteThemeData(themeColors.foregroundColor, themeColors.backgroundColor);
                    }
                } while (!themeChosen);
            }
            DisplayContinuePrompt();
        }
        #endregion
        #region USER PASSWORD AND USERNAME
         static void DiplsayUserNamePasswordMenu()
         {
            bool quitMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Username & Password File Menu");
                //
                // User Menu Choice Files
                //
                Console.WriteLine("\ta) Read & Write Single Username");
                Console.WriteLine("\tb) Read and Write Single Password");
                Console.WriteLine("\tc) Login Validation of Username & Password");
                Console.WriteLine("\tq) Quit Username & Password Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();
                switch (menuChoice)
                {
                    case "a":
                        DisplayNewUsername();
                        break;
                    case "b":
                        DisplayNewPassword();
                        break;
                    case "c":
                        LoginRegister();
                        break;
                    case "q":
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
            //
            // Read UserNamePassword File
            //
            static void WriteUsernameToDataFile()
            {
                string dataPath = @"DataFolder\UserNamePassword.txt";
                string usernameText;
                Console.Clear();


            }
            static void DisplayNewUsername()
            {
                Console.Clear();
                Console.WriteLine("Enter a Username: ");
                string usernameText = Console.ReadLine();
                File.AppendAllText(@"DataFolder\UserNamePassword.txt", usernameText + "\n");
                Console.WriteLine($"New Username {usernameText}");
                DisplayContinuePrompt();
            }
            static void DisplayNewPassword()
            {
                bool correctLogin = false;
                Console.Clear();
                Console.WriteLine("Enter a Password: ");
                string passwordText = Console.ReadLine();
                File.AppendAllText(@"DataFolder\UserNamePassword.txt", passwordText + "\n");
                DisplayContinuePrompt();
            }
            static void LoginRegister()
            {
                bool correctLogin = false; 
                
                Console.WriteLine("Are you an existing user? [yes / no]"); //validate do/while loop for username and password inside if statement for yes 
                string LoginInput = Console.ReadLine();
                if (LoginInput == "yes")
                {
                    do
                    {
                        
                        Console.WriteLine("Please enter your Username.");

                        string userName = Console.ReadLine();

                        Console.WriteLine("Please enter your password.");

                        string passWord = Console.ReadLine();

                        string[] userNamePasswordValidation = File.ReadAllLines(@"DataFolder\UserNamePassword.txt");

                        if (userName == userNamePasswordValidation[0] && passWord == userNamePasswordValidation[1]) //process used for validation of lines 
                        {
                            correctLogin = true; 
                            Console.WriteLine("Username & Password Correct, you may continue.");
                            DisplayContinuePrompt();

                        }
                        else
                        {
                            correctLogin = false; 
                            Console.WriteLine("The UserName and Password that you entered does not match the credentials needed, please try re-entering you Username & Password");
                            DisplayContinuePrompt();
                        }

                    } while (!correctLogin);

                }
            }


        // INCOMPLETE COULD NOT IMPLEMENT USERNAME PASSWORD PROPERLY WILL TRY TO CONTINUE PROGRESS SOON 

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