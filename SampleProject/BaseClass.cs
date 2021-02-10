using System;

namespace SampleProject
{
    public class BaseClass
    {
        public virtual string TestProp { get; set; }

        public virtual void Test()
        {
            Console.WriteLine("Hello, from BaseClass!");
        }
    }
}
