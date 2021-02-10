using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = SampleProject.Analyzers.Tests.CSharpAnalyzerVerifier<SampleProject.Analyzers.NewKeywordAnalyzer>;

namespace SampleProject.Analyzers.Tests
{
    [TestClass]
    public class AnalyzerPropertyUnitTests
    {
        [TestMethod]
        public async Task TestInheritedPropertyWithNew()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        public abstract class BaseClass
        {
            public virtual int Num { get; set; }
        }
        public class ChildClass : BaseClass
        {
            new public int Num { get; set; }
        }
    }";

            var expected = VerifyCS.Diagnostic("Demo1001").WithSpan(12, 28, 12, 31);
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task TestValidPropertyWithOverride()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        public abstract class BaseClass
        {
            public virtual int Num { get; set; }
        }
        public class ChildClass : BaseClass
        {
            public override int Num { get; set; }
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test, DiagnosticResult.EmptyDiagnosticResults);
        }

        [TestMethod]
        public async Task TestRegularClassWithSeveralProperties()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        public class BaseClass
        {
            public int Num1 { get; set; }
            public int Num2 { get; set; }
            public string Str1 { get; set; }
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test, DiagnosticResult.EmptyDiagnosticResults);
        }

        [TestMethod]
        public async Task TestAttributeWithSeveralProperties()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        public class BaseClass : System.Attribute
        {
            public int Num1 { get; set; }
            public int Num2 { get; set; }
            public string Str1 { get; set; }
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test, DiagnosticResult.EmptyDiagnosticResults);
        }
    }
}
