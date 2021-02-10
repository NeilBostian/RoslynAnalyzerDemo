using System;

namespace SampleProject
{
    public class ChildClass : BaseClass
    {
        public new string TestProp { get; set; }

        public new void Test()
        {
            Console.WriteLine("Hello, from ChildClass!");
        }
    }
}
