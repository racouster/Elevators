﻿using System.Diagnostics;
using System.Text;

namespace ElevatorLib
{
    class DebugWriter : TextWriter
    {
        public override void WriteLine(string value)
        {
            Debug.WriteLine(value);
            //base.WriteLine(value);
        }

        public override void Write(string value)
        {
            Debug.Write(value);
            //base.Write(value);
        }

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }
    }
}