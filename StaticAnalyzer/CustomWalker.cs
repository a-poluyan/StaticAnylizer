using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StaticAnalyzer
{
    public class CustomWalker : CSharpSyntaxWalker
    {
        public ListViewErrors Messages { get; private set; } = new ListViewErrors();

        private readonly SemanticModel semanticModel;
        private readonly Compilation compilation;


        public CustomWalker(SemanticModel model, Compilation compile)
        {
            semanticModel = model;
            compilation = compile;
            Messages.FilePath = model.SyntaxTree.FilePath;
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            CheckProcessStart(node);
            CheckScreenShot(node);
            CheckNewCompiler(node);
            foreach (var n in node.ChildNodes())
                Visit(n);
        }

        public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            CheckThrowKeyword(node);
            foreach (var n in node.ChildNodes())
                Visit(n);
        }

        public override void VisitArgument(ArgumentSyntax node)
        {
            CheckHttpRequests(node);
            foreach (var n in node.ChildNodes())
                Visit(n);
        }

        public override void VisitAttribute(AttributeSyntax node)
        {
            CheckDllImport(node);
            foreach (var n in node.ChildNodes())
                Visit(n);
        }


        /// <summary> Находит возможные вызовы REST сервисов </summary>
        private void CheckHttpRequests(ArgumentSyntax node)
        {
            if (node.GetText().ToString().Contains("http"))
            {
                Error(node.GetLocation(), "Возможен запрос в интернет", ListViewErrors.Criticality.Средний, $"Проверить доверенность внешнего сервиса: {node.GetText()}");
            }
        }

        /// <summary> Находит импорты сторонних DLL </summary>
        private void CheckDllImport(AttributeSyntax node)
        {
            var dllImportType = semanticModel.Compilation.GetTypeByMetadataName(typeof(DllImportAttribute).FullName);
            var nodeType = semanticModel.GetTypeInfo(node).Type;
            if (dllImportType != null && dllImportType.Equals(nodeType))
            {
                Error(node.GetLocation(), "Импорт сторонней DLL", ListViewErrors.Criticality.Высокий, $"Провести анализ DLL: {node.ArgumentList.Arguments[0]}");
            }
        }

        /// <summary> Находит вызовы стороннего программного обеспечения </summary>
        private void CheckProcessStart(InvocationExpressionSyntax node)
        {
            if (IsClassAndMethod(node, typeof(System.Diagnostics.Process), "Start"))
            {
                Error(node.GetLocation(), "Вызов стороннего процесса", ListViewErrors.Criticality.Высокий, $"Провести анализ стороннего процесса");
            }
        }

        /// <summary> Находит добавление нового компилятора </summary>
        private void CheckNewCompiler(InvocationExpressionSyntax node)
        {
            if (IsClassAndMethod(node, typeof(Microsoft.CSharp.CSharpCodeProvider), "CompileAssemblyFromSource"))
            {
                Error(node.GetLocation(), "Создание динамического компилятора кода", ListViewErrors.Criticality.Критический, "Провести ручной анализ вызываемого динамического участка кода");
            }
        }

        /// <summary> Находит вызовы стороннего программного обеспечения </summary>
        private void CheckScreenShot(InvocationExpressionSyntax node)
        {
            if (IsClassAndMethod(node, typeof(System.Drawing.Graphics), "CopyFromScreen") || IsClassAndMethod(node, typeof(Clipboard), "GetImage"))
            {
                Error(node.GetLocation(), "Создание скриншота", ListViewErrors.Criticality.Критический, "Убрать возможность ПО создавать скриншоты экрана");
            }
        }

        /// <summary> Проверяет, что при создании объекта класса Exception не потеряно ключевое слово throw </summary>
        private void CheckThrowKeyword(ObjectCreationExpressionSyntax node)
        {
            if (IsChildType(typeof(Exception), node) && !IsReferenceUsed(node.Parent))
            {
                Error(node.GetLocation(), "Пропущен оператор throw", ListViewErrors.Criticality.Низкий, "Использовать оператор throw для корректного выброса исключения");
            }
        }

        private bool IsChildType(Type parent, SyntaxNode child)
        {
            var baseType = compilation.GetTypeByMetadataName(parent.FullName);
            var childType = semanticModel.GetTypeInfo(child).Type;

            while (baseType != null && !baseType.Equals(childType))
            {
                if (childType != null)
                {
                    childType = childType.BaseType;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsReferenceUsed(SyntaxNode parentNode)
        {
            if (parentNode.IsKind(SyntaxKind.ExpressionStatement))
            {
                return false;
            }

            if (parentNode is LambdaExpressionSyntax)
            {
                var methodReturnVoid = (semanticModel.GetSymbolInfo(parentNode).Symbol as IMethodSymbol)?.ReturnsVoid == true;
                return !methodReturnVoid;
            }

            return true;
        }


        private bool IsClassAndMethod(InvocationExpressionSyntax node, Type className, string methodName)
        {
            var memberAccess = ((MemberAccessExpressionSyntax)node.ChildNodes().FirstOrDefault(n => n.IsKind(SyntaxKind.SimpleMemberAccessExpression)));

            if (memberAccess != null)
            {
                var leftIdentifier = memberAccess.Expression;
                var rightIdentifier = memberAccess.Name;

                if (IsChildType(className, leftIdentifier) && rightIdentifier.TryGetInferredMemberName() == methodName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary> Выполняет логирование ошибки в форму </summary>
        private void Error(Location location, string errorText, ListViewErrors.Criticality criticality, string suggestion)
        {
            var line = location.GetLineSpan();
            Messages.Errors.Add(new ListViewErrors.Error
            {
                Location = $"Строка: {line.StartLinePosition.Line}",
                Description = errorText,
                Criticality = criticality,
                Suggestions = suggestion,
            });
        }
    }
}
