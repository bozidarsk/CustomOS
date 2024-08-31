using System.Collections;
using System.Collections.Generic;

namespace System;

public sealed class CharEnumerator : IEnumerator, IEnumerator<char>, IDisposable, ICloneable
{
    private string str;
    private int index = -1;

    object? IEnumerator.Current => Current;
    public char Current 
    {
        get 
        {
            if (index < 0 || index >= str.Length)
                throw new InvalidOperationException();

            return str[index];
        }
    }

    public void Dispose() => str = null;
    public object Clone() => MemberwiseClone();

    public void Reset() => index = -1;
    public bool MoveNext() 
    {
        int next = index + 1;

        if (next < str.Length) 
        {
            index = next;
            return true;
        }

        index = str.Length;
        return false;
    }

    internal CharEnumerator(string str) => this.str = str;
}


