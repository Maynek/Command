using Maynek.Command;
using System.ComponentModel.Design;

namespace Maynek.Command.Test
{
    class Program
    {
        static int Main(string[] args)
        {
            Writer writer = new Writer() { EnabledWrite = true };


            writer.Write("-------- Start test. -------- ");

            //Parser setting.
            var parser = new Parser();

            parser.ArgumentEvent += delegate(object sender, ArgumentEventArgs e)
            {
                for (int i = 0; i < e.Args.Length; i++)
                {
                    writer.Write("  Argument[" + (i + 1).ToString() + "] : " + e.Args[i]);
                }
            };

            parser.ErrorEvent += delegate (object sender, ErrorEventArgs e)
            {
                var s = "Parse Error : ";

                switch(e.Type)
                {
                    case ErrorType.NoValue:
                        s += "Option '" + e.OptionName + "' needs value.";
                        break;
                }

                writer.Write(s);
            };

            parser.AddOptionDefinition(new OptionDefinition("-s")
            {
                EventHandler = delegate(object sender, OptionEventArgs e)
                {
                    writer.Write("  " + e.Name + " (No Value).");
                }
            });

            parser.AddOptionDefinition(new OptionDefinition("-a", "--a-longname")
            {
                EventHandler = delegate(object sender, OptionEventArgs e)
                {
                    writer.Write("  " + e.Name + " (No Value).");
                }
            });

            parser.AddOptionDefinition(new OptionDefinition("-v")
            {
                Type = OptionType.RequireValue,
                EventHandler = delegate(object sender, OptionEventArgs e)
                {
                    writer.Write("  " + e.Name + " : " + e.Value);
                }
            });


            //Test.
            var testData = new string[]
            {
                "value1",
                "-s srcpath destpath",
                "-a srcpath destpath",
                "--a-longname srcpath destpath",
                "-s",
                "-v opt path",
                "-v opt1 -v opt2",
                "-v -s srcpath destpath"
            };

            for (int i = 0; i < testData.Length; i++)
            {
                writer.Write("[Case" + (i+1).ToString() + "]");
                writer.Write("Input :");
                writer.Write("  " + testData[i]);
                writer.Write("Output :");
                parser.Parse(testData[i].Split(' '));
                writer.Write("");
            }

            writer.Write("-------- End test. --------");

            return 0;
        }
    }
}
