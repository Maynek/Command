//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System.Collections.Generic;

namespace Maynek.Command
{
    public enum ErrorType : int
    {
        None = 0,
        NoValue = 1,
    }

    public enum WarningType : int
    {
        None = 0,
        UndefinedOption = 1,
    }

    public class ArgumentEventArgs
    {
        private List<string> list = new List<string>();
        public string[] Args { get { return this.list.ToArray(); } }

        public void Add(string arg)
        {
            this.list.Add(arg);
        }
    }

    public class ErrorEventArgs
    {
        public ErrorType Type { get; set; } = ErrorType.None;
        public string OptionName { get; set; } = string.Empty;
    }

    public class WarningEventArgs
    {
        public WarningType Type { get; set; } = WarningType.None;
        public string OptionName;
    }

    public class OptionEventArgs
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public delegate void ArgumentEventHandler(object sender, ArgumentEventArgs e);
    public delegate void ErrorEventHandler(object sender, ErrorEventArgs warning);
    public delegate void WarningEventHandler(object sender, WarningEventArgs warning);
    public delegate void OptionEventHandler(object sender, OptionEventArgs e);
}
