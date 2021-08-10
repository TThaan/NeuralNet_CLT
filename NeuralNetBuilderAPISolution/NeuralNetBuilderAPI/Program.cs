﻿using ConsoleWindowChanger;
using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.Builders;
using NeuralNetBuilderAPI.Commandables;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static NeuralNetBuilderAPI.GlobalConstants;

namespace NeuralNetBuilderAPI
{
    public class Program
    {
        #region fields

        // Later: use DI to let ICommandables access those fields instead of making them internal static?
        internal static Initializer initializer;
        internal static PathBuilder pathBuilder;
        internal static ParameterBuilder paramBuilder;
        internal static Stopwatch stopwatch = new Stopwatch();
        internal static bool isInitializerStatusChangedEventActive = true;
        internal static bool isDataProviderChangedEventActive = true;

        #endregion

        #region methods

        static async Task Main(string[] args)
        {
            #region Fit Console Window

            Change.BorderPositions(.5m, 0, 1, 1);

            #endregion

            initializer = new Initializer();
            initializer.InitializerStatusChanged += NetbuilderChanged_EventHandlingMethod;
            pathBuilder = initializer.Paths;            
            paramBuilder = initializer.ParameterBuilder;

            initializer.SampleSet = new SampleSet();
            initializer.SampleSet.DataProviderChanged += DataProviderChanged_EventHandlingMethod;

            Show.ShowHelp();
            Show.ShowSettings();

            await ExecuteConsoleCommands();
        }

        private static async Task ExecuteConsoleCommands()
        {
            Console.WriteLine();
            string consoleInput = Console.ReadLine();

            try
            {
                var splitInput = consoleInput.Split(Separator_ConsoleInput, StringSplitOptions.RemoveEmptyEntries);
                var mainCommand_String = splitInput.First();
                var mainCommand = mainCommand_String.ToEnum<MainCommand>();
                var parameters = splitInput.Skip(1);

                var comm = mainCommand.ToCommandableBase();
                await comm.Execute(parameters);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            await ExecuteConsoleCommands();
        }

        #endregion

        #region event handling methods

        private static void NetbuilderChanged_EventHandlingMethod(object initializer, InitializerStatusChangedEventArgs e)
        {
            if(isInitializerStatusChangedEventActive)
                Console.WriteLine($"{e.Info}");
        }
        private static void DataProviderChanged_EventHandlingMethod(object initializer, DataProviderChangedEventArgs e)
        {
            if(isDataProviderChangedEventActive)
                Console.WriteLine($"{e.Info}");
        }
        // public to give access to Create class. (Later: Use DI.)
        public static void Trainer_StatusChanged_EventHandlingMethod(object trainer, TrainerStatusChangedEventArgs e)
        {
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds,10}: {e.Info}");
        }

        #endregion
    }
}