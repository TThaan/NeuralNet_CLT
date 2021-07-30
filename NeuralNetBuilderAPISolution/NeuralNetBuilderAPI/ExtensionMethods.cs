﻿using NeuralNetBuilder;
using NeuralNetBuilderAPI.Commandables;
using System;
using System.Linq;
using static NeuralNetBuilderAPI.GlobalConstants;

namespace NeuralNetBuilderAPI
{
    public static class ExtensionMethods
    {
        public static ParameterName GetParameterName(this string parameter, char separator = Separator_Parameter)
        {
            return parameter.Split(separator).First().ToEnum<ParameterName>();
        }
        public static string GetParameterValue_String(this string parameter, char separator = Separator_Parameter)
        {
            return parameter.Split(separator).Last();
        }
        public static int GetParameterValue_Int(this string parameter, char separator = Separator_Parameter)
        {
            var parameterValue_String = parameter.Split(separator).Last();
            if (!int.TryParse(parameterValue_String, out int result))
                throw new ArgumentException($"Cannot parse {parameterValue_String} into an integer.");

            return result;
        }
        public static ICommandable ToCommandableBase(this MainCommand mainCommand)
        {
            throw new NotImplementedException();
            //ICommandable result;

            //// Get the ICommandable (in the entry assembly) with the name in 'mainCommand_String'.

            //var ass = System.Reflection.Assembly.GetEntryAssembly();
            //Type commType = typeof(ICommandable);
            //result = ass.DefinedTypes
            //    .Where(x => x.ImplementedInterfaces.Contains(commType))
            //    .SingleOrDefault(x => Equals(x.Name.ToLower(), mainCommand.ToString().ToLower()))
            //    .AsType()
            //    as ICommandable;

            //if (result == null)
            //    throw new ArgumentException($"Cannot find a type {mainCommand.ToString()} in assemby {ass} (even when ignoring case sensitivity).");

            //return result;
        }
        //public static ICommandable ToICommandable(this MainCommand mainCommand)
        //{
        //    ICommandable result;

        //    // Get the ICommandable (in the entry assembly) with the name in 'mainCommand_String'.

        //    var ass = System.Reflection.Assembly.GetEntryAssembly();
        //    Type commType = typeof(ICommandable);
        //    result = ass.DefinedTypes
        //        .Where(x => x.ImplementedInterfaces.Contains(commType))
        //        .SingleOrDefault(x => Equals(x.Name.ToLower(), mainCommand.ToString().ToLower()))
        //        .AsType()
        //        as ICommandable;

        //    if (result == null)
        //        throw new ArgumentException($"Cannot find a type {mainCommand.ToString()} in assemby {ass} (even when ignoring case sensitivity).");

        //    return result;
        //}
        //public static ICommandable ToICommandable(this string mainCommand_String)
        //{
        //    ICommandable result;

        //    // Check if 'mainCommand_String' is a MainCommand

        //    var mainCommand = mainCommand_String.ToEnum<MainCommand>();

        //    // Get the ICommandable (in the entry assembly) with the name in 'mainCommand_String'.

        //    var ass = System.Reflection.Assembly.GetEntryAssembly();
        //    Type commType = typeof(ICommandable);
        //    result = ass.DefinedTypes
        //        .Where(x => x.ImplementedInterfaces.Contains(commType))
        //        .SingleOrDefault(x => Equals(x.Name.ToLower(), mainCommand_String.ToLower()))
        //        .AsType()
        //        as ICommandable;

        //    if (result == null)
        //        throw new ArgumentException($"Cannot find a type {mainCommand_String} in assemby {ass} (even when ignoring case sensitivity).");

        //    return result;
        //}
    }
}