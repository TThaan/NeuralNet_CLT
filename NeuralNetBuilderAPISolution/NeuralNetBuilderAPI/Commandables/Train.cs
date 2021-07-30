﻿using NeuralNetBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NeuralNetBuilderAPI.Program;   // To give this ICommandable access to Program. initializer/pathBuilder/paramBuilder. (Later: Use DI!)

namespace NeuralNetBuilderAPI.Commandables
{
    public class Train : CommandableBase
    {
        #region ICommandable

        public override async Task Execute(IEnumerable<string> parametersAndSubCommand)
        {
            await Task.Run(async () =>
            {
                TrainCommand trainCommand = GetSubCommand<TrainCommand>(parametersAndSubCommand, out var parameters);
                CheckParameters(parameters, MainCommand.train, ConsoleInputCheck.EnsureSingleParameter);
                string singleParameter = parametersAndSubCommand.ElementAt(1).GetParameterValue_String();

                switch (trainCommand)
                {
                    case TrainCommand.Undefined:
                        break;
                    case TrainCommand.start:
                        await TrainAsync(singleParameter.ToEnum<PresetValue>());
                        break;
                    case TrainCommand.example:
                        await ExampleTraining(singleParameter.ToEnum<PresetValue>());
                        break;
                    default:
                        break;
                }
            });
        }

        #endregion

        #region Sub Command methods

        /// <summary>
        /// Valid parameters: Undefined, Shuffle
        /// </summary>
        internal static async Task TrainAsync(PresetValue shuffle = PresetValue.undefined)
        {
            if (shuffle != PresetValue.undefined && shuffle != PresetValue.shuffle)
                throw new ArgumentException($"Parameter {shuffle} is not valid here. Use {PresetValue.shuffle} or no parameter.");

            stopwatch.Reset();
            stopwatch.Start();
            await initializer.TrainAsync(initializer.SampleSet, shuffle);
            stopwatch.Stop();

            await initializer.SaveTrainedNetAsync();
        }
        internal async static Task ExampleTraining(PresetValue shuffle = PresetValue.undefined)
        {
            if (shuffle != PresetValue.undefined && shuffle != PresetValue.shuffle)
                throw new ArgumentException($"Parameter {shuffle} is not valid here. Use {PresetValue.shuffle} or no parameter.");

            pathBuilder.ResetPaths();
            //pathBuilder.FileName_Prefix = @"";
            //pathBuilder.FileName_Suffix = "_test.txt";
            pathBuilder.General = @"C:\Users\Jan_PC\Documents\_NeuralNetApp\Saves\Test2\";

            // Get samples

            if (!await initializer.SampleSet.LoadSampleSetAsync(pathBuilder.SampleSet, .1f, 0))
                return;             // Always check if the loaded sample set suits the ... parameters!

            // Get net

            if (!await paramBuilder.LoadNetParametersAsync())
                return;
            if (!await initializer.CreateNetAsync())
                return;
            // if (!await initializer.LoadNetAsync())
            //     return;        // Always check if the loaded initialized net suits loaded parameters!

            // Get trainer

            if (!await paramBuilder.LoadTrainerParametersAsync())
                return;
            if (!await initializer.CreateTrainerAsync(initializer.SampleSet))
                return;
            initializer.Trainer.TrainerStatusChanged += Trainer_StatusChanged_EventHandlingMethod;

            // await initializer.Trainer.SampleSet.TrainSet.ShuffleAsync();

            // Show Initialized Net

            Show.ShowNet();

            // Activate logging
            Log.LogOn();

            // Train

            await TrainAsync(shuffle);
        }

        #endregion
    }
}
