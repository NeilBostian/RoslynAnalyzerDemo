using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = SampleProject.Analyzers.Tests.CSharpAnalyzerVerifier<SampleProject.Analyzers.NewKeywordAnalyzer>;

namespace SampleProject.Analyzers.Tests
{
    [TestClass]
    public class AnalyzerMethodUnitTests
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestNoResult()
        {
            var test = @"";
            await VerifyCS.VerifyAnalyzerAsync(test, DiagnosticResult.EmptyDiagnosticResults);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestInheritedMethodWithNew()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        public abstract class BaseClass
        {
            public virtual void Print()
            {
                Console.WriteLine(""Hello world, from BaseClass!"");
            }
        }
        public class ChildClass : BaseClass
        {
            new public void Print()
            {
                Console.WriteLine(""Hello world, from ChildClass!"");
            }
        }
    }";

            var expected = VerifyCS.Diagnostic("Demo1001").WithSpan(15, 29, 15, 34);
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task TestValidMethodWithOverride()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        public abstract class BaseClass
        {
            public virtual void Print()
            {
                Console.WriteLine(""Hello world, from BaseClass!"");
            }
        }
        public class ChildClass : BaseClass
        {
            public override void Print()
            {
                Console.WriteLine(""Hello world, from ChildClass!"");
            }
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test, DiagnosticResult.EmptyDiagnosticResults);
        }
    }
}
