using System;

namespace SampleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            ChildClass childClass = new ChildClass();
            BaseClass baseClass = childClass;

            childClass.Test();
            baseClass.Test();
        }
    }
}
