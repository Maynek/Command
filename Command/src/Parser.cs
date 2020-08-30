//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.Collections.Generic;

namespace Maynek.Command
{
    public enum CommandParserWarningType : int
    {
        None = 0,
        UndefinedOption = 1
    }

    public class CommandParserWarning
    {
        public CommandParserWarningType Type { get; set; } = CommandParserWarningType.None;
        public string OptionName;
    }

    /// <summary>
    /// Parses command line arguments.
    /// </summary>
    public class Parser
    {
        //================================
        // Definitions 
        //================================
        public delegate void ArgumentMethod(string[] args);
        public delegate void NoArgumentOptionMethod();
        public delegate void OneArgumentOptionMethod(string arg);
        public delegate void WarningMethod(CommandParserWarning warning);

         //================================
        // Constants
        //================================
        private const uint STATE_DEFAULT =  0x80000000;
        private const uint STATE_NEUTRAL =  0x80000001;
        private const uint STATE_FINISH  =  0x80000002;
        private const uint STATE_MAX     =  STATE_DEFAULT - 1;


        //================================
        // Fields
        //================================
        private uint stateCount = 0;
        private uint currentState = STATE_DEFAULT;
        private ArgumentMethod argumentMethod = null;
        private Dictionary<uint, NoArgumentOptionMethod> noMethods =
            new Dictionary<uint, NoArgumentOptionMethod>();
        private Dictionary<uint, OneArgumentOptionMethod> oneMethods = 
            new Dictionary<uint, OneArgumentOptionMethod>();
        private Dictionary<string, uint> nextStates =
            new Dictionary<string, uint>();
        private WarningMethod warningMethod = null;


        //================================
        // Methods
        //================================
        private void updateStateCount()
        {
            this.stateCount++;

            if (this.stateCount > STATE_MAX)
            {
                throw new Exception();
            }
        }

        private bool isOptionName(string arg)
        {
            return (arg[0] == '-');
        }

        private void finishOption(string arg)
        {
            if (this.noMethods.ContainsKey(this.currentState))
            {
                this.noMethods[this.currentState]();
            }
            
            if (this.oneMethods.ContainsKey(this.currentState))
            {
                this.oneMethods[this.currentState](arg);
            }

            this.currentState = STATE_NEUTRAL;
        }

        public void SetArgumentMethod(ArgumentMethod method)
        {
            this.argumentMethod = method;
        }

        public void SetWaringMethod(WarningMethod method)
        {
            this.warningMethod = method;
        }

        public void AddOptionMethod(NoArgumentOptionMethod method, string option)
        {
            if (this.isOptionName(option))
            {
                this.updateStateCount();
                this.nextStates.Add(option, this.stateCount);
                this.noMethods.Add(this.stateCount, method);
            }
            else
            {
                throw new Exception();
            }
        }

        public void AddOptionMethod(NoArgumentOptionMethod method, string[] options)
        {
            foreach (string opt in options)
            {
                this.AddOptionMethod(method, opt);
            }
        }

        public void AddOptionMethod(OneArgumentOptionMethod method, string option)
        {
            if (this.isOptionName(option))
            {
                this.updateStateCount();
                this.nextStates.Add(option, this.stateCount);
                this.oneMethods.Add(this.stateCount, method);
            }
            else
            {
                throw new Exception();
            }
        }

        public void AddOptionMethod(OneArgumentOptionMethod method, string[] options)
        {
            foreach (string opt in options)
            {
                this.AddOptionMethod(method, opt);
            }
        }

        public void Parse(string[] args)
        {
            List<string> argList = new List<string>();

            this.currentState = STATE_NEUTRAL;
            foreach (string arg in args)
            {
                if (this.isOptionName(arg))
                {
                    if (this.currentState != STATE_NEUTRAL)
                    {
                        this.finishOption(string.Empty);
                    }

                    if (this.nextStates.ContainsKey(arg))
                    {
                        this.currentState = this.nextStates[arg];
                        if (this.noMethods.ContainsKey(this.currentState))
                        {
                            this.finishOption(string.Empty);
                        }                        
                    }
                    else
                    {
                        if (this.warningMethod != null)
                        {
                            CommandParserWarning warning = new CommandParserWarning();
                            warning.Type = CommandParserWarningType.UndefinedOption;
                            warning.OptionName = arg;
                            this.warningMethod(warning);
                        }
                    }
                }
                else
                {
                    if (this.currentState == STATE_NEUTRAL)
                    {
                        argList.Add(arg);
                    }
                    else
                    {
                        this.finishOption(arg);
                    }                    
                }
            }

            if (this.argumentMethod != null)
            {
                this.argumentMethod(argList.ToArray());
            }

            this.currentState = STATE_FINISH;
        }
    }
}
