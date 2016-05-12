using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Utilities
{
    public class Compiler
    {
        public string SaveFile(SolutionPostViewModel model, string userId, int problemId)
        {
            var baseFolder = ConfigurationManager.AppSettings.Get("baseDir");
            var workingFolder = baseFolder + userId + "\\" + problemId + "\\";
            
            if (!Directory.Exists(workingFolder))
            {
                Directory.CreateDirectory(workingFolder);
            }

            File.WriteAllText(workingFolder + model.fileName + ".cpp", model.code);
            return (workingFolder + model.fileName + ".cpp");
        }

        public List<string> Compile(SolutionPostViewModel model, List<Testcase> inputs, string userId, int problemId)
        {
            var savedFile = SaveFile(model, userId, problemId);

            List<string> outputs = new List<string>();

            var baseFolder = ConfigurationManager.AppSettings.Get("baseDir");
            var workingFolder = baseFolder + userId + "\\" + problemId + "\\";
            var handinFolder = workingFolder + ConfigurationManager.AppSettings.Get("handinFolder");
            var compileFolder = workingFolder + ConfigurationManager.AppSettings.Get("compiledFolder");
            

            var cppFileName = model.fileName + ".cpp";
            var exeFilePath = compileFolder + model.fileName + ".exe";

            if (!Directory.Exists(compileFolder))
            {
                Directory.CreateDirectory(compileFolder);
            }
            File.Copy(savedFile, compileFolder + cppFileName,true);

            var compilerFolder = ConfigurationManager.AppSettings.Get("compilerFolder");
            Process compiler = new Process();
            compiler.StartInfo.FileName = "cmd.exe";
            compiler.StartInfo.WorkingDirectory = compileFolder;
            compiler.StartInfo.RedirectStandardInput = true;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.UseShellExecute = false;

            compiler.Start();
            compiler.StandardInput.WriteLine("\"" + compilerFolder + "vcvars32.bat" + "\"");
            compiler.StandardInput.WriteLine("cl.exe /nologo /EHsc " + cppFileName);
            compiler.StandardInput.WriteLine("exit");
            string compilerOutput = compiler.StandardOutput.ReadToEnd();
            compiler.WaitForExit();
            compiler.Close();

            // Check if the compile succeeded, and if it did,
            // we try to execute the code:
            if (System.IO.File.Exists(exeFilePath))
            {
                var processInfoExe = new ProcessStartInfo(exeFilePath, "");
                processInfoExe.UseShellExecute = false;
                processInfoExe.RedirectStandardOutput = true;
                processInfoExe.RedirectStandardInput = true;
                processInfoExe.RedirectStandardError = true;
                processInfoExe.CreateNoWindow = true;
                using (var processExe = new Process())
                {
                    foreach (var input in inputs)
                    {
                        processExe.StartInfo = processInfoExe;
                        processExe.Start();
                        processExe.StandardInput.WriteLine(input.input);

                        string output = "";
                        // We then read the output of the program:
                        while (!processExe.StandardOutput.EndOfStream)
                        {
                            output += processExe.StandardOutput.ReadLine();
                        }
                        outputs.Add(output);
                    }
                }
            }
            else
            {
                outputs.Add(compilerOutput);
            }

            foreach (var file in Directory.GetFiles(compileFolder))
            {
                File.Delete(file);
            }

            return outputs;

        }
    }
}