﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions;

public class DuplicateItemException : Exception
{
    public DuplicateItemException(){}

    public DuplicateItemException(string message) : base(message) { }
}
