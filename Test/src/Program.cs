using Maynek.Command;
using System.ComponentModel.Design;

namespace Maynek.Command.Test
{
    class Program
    {
        static int Main(string[] args)
        {
            Writer.DetailMode = true;

            Writer.WriteInfo("-------- Start test. -------- ");

            //Parser setting.
            var parser = new Parser();

            parser.ArgumentEvent += delegate(object sender, ArgumentEventArgs e)
            {
                for (int i = 0; i < e.Args.Length; i++)
                {
                    Writer.WriteInfo("  Argument[" + (i + 1).ToString() + "] : " + e.Args[i]);
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

                Writer.WriteInfo(s);
            };

            parser.AddOptionDefinition(new OptionDefinition("-s")
            {
                EventHandler = delegate(object sender, OptionEventArgs e)
                {
                    Writer.WriteInfo("  " + e.Name + " (No Value).");
                }
            });

            parser.AddOptionDefinition(new OptionDefinition("-a", "--a-longname")
            {
                EventHandler = delegate(object sender, OptionEventArgs e)
                {
                    Writer.WriteInfo("  " + e.Name + " (No Value).");
                }
            });

            parser.AddOptionDefinition(new OptionDefinition("-v")
            {
                ValueType = ValueType.Require,
                EventHandler = delegate(object sender, OptionEventArgs e)
                {
                    Writer.WriteInfo("  " + e.Name + " : " + e.Value);
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
                Writer.WriteInfo("[Case" + (i+1).ToString() + "]");
                Writer.WriteInfo("Input :");
                Writer.WriteInfo("  " + testData[i]);
                Writer.WriteInfo("Output :");
                parser.Parse(testData[i].Split(' '));
                Writer.WriteInfo("");
            }

            Writer.WriteInfo("-------- End test. --------");

            return 0;
        }
    }
}
