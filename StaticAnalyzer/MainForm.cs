using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StaticAnalyzer
{
    public partial class MainForm : Form
    {
        /// <summary> Путь к файлу решения объекта оценки </summary>
        private static string solutionPath = null;

        public MainForm()
        {
            InitializeComponent();
            // Attempt to set the version of MSBuild.
            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
            var instance = visualStudioInstances[0];
            MSBuildLocator.RegisterInstance(instance);
        }

        private void btnStartAnalyse_Click(object sender, EventArgs e)
        {
            var reportRows = new List<ListViewErrors>();

            using (var workspace = MSBuildWorkspace.Create())
            {
                var solution = workspace.OpenSolutionAsync(solutionPath, new ConsoleProgressReporter());
                var projects = solution.Result.Projects.ToList();

                foreach(var project in projects)
                {
                    Compilation compilation = project.GetCompilationAsync().Result;
                    foreach (var file in project.Documents)
                    {
                        SyntaxTree tree = file.GetSyntaxTreeAsync().Result;
                        SemanticModel model = compilation.GetSemanticModel(tree);

                        var cw = new CustomWalker(model, compilation);
                        cw.Visit(tree.GetRoot());

                        reportRows.Add(cw.Messages);
                    }
                }
            }

            new ReportForm(reportRows).ShowDialog();
        }

        private class ConsoleProgressReporter : IProgress<ProjectLoadProgress>
        {
            public void Report(ProjectLoadProgress loadProgress)
            {
                var projectDisplay = Path.GetFileName(loadProgress.FilePath);
                if (loadProgress.TargetFramework != null)
                {
                    projectDisplay += $" ({loadProgress.TargetFramework})";
                }

                Console.WriteLine($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
            }
        }

        private void btnChooseSolution_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Solution (*.sln)|*.sln";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = "Выберете файл решения";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                solutionPath = openFileDialog.FileName;
                tbSolutionPath.Text = solutionPath;
                btnStartAnalyse.Enabled = true;
            }
        }
    }
}
