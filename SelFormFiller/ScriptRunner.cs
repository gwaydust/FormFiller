using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using OpenQA.Selenium;

namespace SelFormFiller
{
    public class Globals
    {
        public IWebDriver driver;
    }

    class MethodSignature
    {
        public MethodSignature()
        {
            paramsDict = new Dictionary<string, string>();
        }
        public string methodName;
        public string description;
        public string sourceFilepath;
        public Dictionary<String, String> paramsDict;
    }

    class ScriptRunner : WebAutomationBase
    {
        private readonly string scriptDir = "";
        private readonly List<MethodSignature> sigs = null;
        public ScriptRunner(string url, string scriptDir) : base(url)
        {
            sigs = new List<MethodSignature>();
            string[] filePaths = System.IO.Directory.GetFiles(scriptDir, "*.csx");
            foreach (var filePath in filePaths)
            {
                sigs = sigs.Concat(ExtractMethodSignature(System.IO.File.ReadAllText(filePath),filePath)).ToList();
            }
            this.scriptDir = scriptDir;
        }
                
        public List<MethodSignature> MethodList
        {
            get
            {
                return sigs;
            }
        }

        public async void InvokeMethod(MethodSignature sig, string [] args)
        {
            ScriptOptions options = ScriptOptions.Default
            .WithReferences(typeof(Filler).Assembly,
                typeof(System.ComponentModel.DataAnnotations.DisplayAttribute).Assembly,
                typeof(OpenQA.Selenium.DriverService).Assembly,
                typeof(OpenQA.Selenium.Support.UI.SelectElement).Assembly)
            .WithImports("System")
            .WithImports("System.IO")
            .WithImports("System.Text")
            .WithImports("System.Text.RegularExpressions")
            .WithImports("System.Collections.Generic")
            .WithImports("System.ComponentModel.DataAnnotations");
                        
            String runCmd = $"\r\n{sig.methodName}(";
            foreach (var arg in args) {
                runCmd += $"\"{arg}\",";
            }
            runCmd = Regex.Replace(runCmd, ",$", ");");
            String script = System.IO.File.ReadAllText(sig.sourceFilepath) + runCmd;
            var state = await CSharpScript.RunAsync(script, options, new Globals { driver = ScriptRunner.driver });
        }

        private List<MethodSignature> ExtractMethodSignature(string scriptContent, string filePath)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(scriptContent);

            var root = (CompilationUnitSyntax)tree.GetRoot();
            var methods = from methodDeclaration in root.DescendantNodes()
                                                    .OfType<MethodDeclarationSyntax>()
                          where methodDeclaration.Identifier.ValueText != "Main" 
                          select methodDeclaration;

            List<MethodSignature> sigs = new List<MethodSignature>();
            foreach (var method in methods)
            {
                if (!method.ReturnType.ToFullString().StartsWith("void") || !method.Modifiers[0].Text.StartsWith("public"))
                {
                    continue;
                }
                
                var sig = new MethodSignature();
                sig.methodName = method.Identifier.ValueText;
                sig.sourceFilepath = filePath;
                if (method.AttributeLists.Count > 0) {
                    var attr = method.AttributeLists.First();
                    var args = attr.Attributes[0].ArgumentList.Arguments.Where(arg => arg.NameEquals.Name.Identifier.Text == "Description");
                    sig.description = args.First().Expression.GetText().ToString().Replace("\"","");
                } else
                {
                    sig.description = "";
                }

                foreach (var param in method.ParameterList.Parameters)
                {
                    sig.paramsDict.Add(param.Identifier.Text, param.Type.ToString().Trim());                    
                }
                sigs.Add(sig);
            }
            return sigs;
        }

    }
}
