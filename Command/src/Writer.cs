//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;

namespace Maynek.Command
{
    /// <summary>
    /// Writes strings to console.
    /// </summary>
    public class Writer
    {
        public static bool DetailMode { get; set; } = false;

        public static void WriteInfo(string value)
        {
            if (DetailMode)
            {
                Console.WriteLine(value);
            }
        }
    }
}
