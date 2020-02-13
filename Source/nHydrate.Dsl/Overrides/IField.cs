using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.Dsl
{
    public interface IField
    {
        string Name { get; set; }
        int Length { get; set; }
        DataTypeConstants DataType { get; set; }
        bool Nullable { get; set; }
    }
}
