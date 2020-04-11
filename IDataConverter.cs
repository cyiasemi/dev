using System;
using System.Collections.Generic;
using System.Text;

namespace JsonTester
{
    public interface IDataConverter<T> where T : new()
    {
        T Deserialize(byte[] data);
    }
}
