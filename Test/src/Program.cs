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

            parser.SetArgumentMethod(delegate (string[] args)
            {
                for (int i = 0;  i < args.Length; i++)
                {
                    Writer.WriteInfo("  Argument[" + (i+1).ToString() + "] : " + args[i]);
                }
            });

            parser.AddOptionMethod(delegate () {
                Writer.WriteInfo("  s : No Arg.");
            }, "-s");

            parser.AddOptionMethod(delegate () {
                Writer.WriteInfo("  a : No Arg.");
            }, new string[] { "-a", "--a-longname", "-za" });

            parser.AddOptionMethod(delegate (string arg) {
                Writer.WriteInfo("  v : " + arg);
            }, "-v");

            //Test.
            var testData = new string[]
            {
                "value1",
                "-s srcpath destpath",
                "-a srcpath destpath",
                "--a-longname srcpath destpath",
                "-s",
                "-v opt path",
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
