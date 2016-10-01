using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CSharp;

namespace Processing.Build
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var provider = new CSharpCodeProvider();
			var compilerparams = new CompilerParameters ();
			compilerparams.ReferencedAssemblies.Add (GetPath ("Processing.Core.dll"));
			compilerparams.ReferencedAssemblies.Add ("System.dll");
			compilerparams.ReferencedAssemblies.Add ("System.Drawing.dll");

			var packageDirectory = new DirectoryInfo (GetPath ("packages"));

		    if (!packageDirectory.Exists)
		        Directory.CreateDirectory(packageDirectory.FullName);
			foreach (FileInfo file in packageDirectory.EnumerateFiles().Where(file => file.Extension.Equals (".dll")))
			    compilerparams.ReferencedAssemblies.Add (file.FullName);

			compilerparams.OutputAssembly = GetPath("Sketch.dll");

			StringBuilder usings = new StringBuilder();
			StringBuilder code = new StringBuilder ();

            var lineMap = new Dictionary<int, Tuple<FileInfo, int, string>>();
		    int ammagulationLine = 1;
			foreach (FileInfo codeFile in new DirectoryInfo(Environment.CurrentDirectory).EnumerateFiles().Where(file => file.Extension.Equals(".cs")))
			{
				string[] lines = File.ReadAllText(codeFile.FullName).Split(new []{ Environment.NewLine }, StringSplitOptions.None);
				int i = 0;
				for (; i < lines.Length; i++, ammagulationLine++)
				{
					string line = lines[i];
					if (line.StartsWith("using"))
					{
						usings.AppendLine(line);
                        lineMap.Add(ammagulationLine, new Tuple<FileInfo, int, string>(codeFile, i + 1, line));
					}
					else if (!string.IsNullOrEmpty(line))
					{
						break;
					}
				}
				for (; i < lines.Length; i++, ammagulationLine++)
				{
                    lineMap.Add(ammagulationLine, new Tuple<FileInfo, int, string>(codeFile, i + 1, lines[i]));
                    code.AppendLine(lines[i]);
				}
			}

			StringBuilder contents = new StringBuilder ();
			contents.AppendLine(usings.ToString ());
			contents.AppendLine(@"public class Sketch : Canvas {");
			contents.Append(code);
			contents.AppendLine(@"}");
		    string ammagulation = contents.ToString();
			CompilerResults results = provider.CompileAssemblyFromSource (compilerparams, ammagulation);
		    if (results.Errors.HasErrors)
		    {
		        string[] ammagulation_lines = ammagulation.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
                //PrintAmmagulation(ammagulation_lines);
                foreach (CompilerError error in results.Errors)
                {
		            Console.WriteLine(error.ErrorText);
                    int index = error.Line - 1;
                    Console.WriteLine(lineMap[index].Item1.Name);
                    Console.WriteLine($"line {lineMap[index].Item2}: {lineMap[index].Item3.Trim()}");
		        }
		    }
		    else
		    {
		        Console.WriteLine("Compilation succeeded.");
		    }
		}

	    private static void PrintMap(Dictionary<int, Tuple<FileInfo, int>> lineMap)
	    {
	        foreach (var value in lineMap.Values)
	        {
	            Console.WriteLine($"{value.Item1.Name}: {value.Item2}");
	        }
	    }

	    private static void PrintAmmagulation(string[] ammagulation_lines)
	    {
            for (int i = 0; i < ammagulation_lines.Length; i++)
                Console.WriteLine($"{i + 1}: {ammagulation_lines[i]}");
        }

	    private static string GetPath(string path)
		{
			return Path.Combine (Environment.CurrentDirectory, path);
		}
	}
}
