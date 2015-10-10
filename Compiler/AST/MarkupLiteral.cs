﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledHandlebars.Compiler.Visitors;

namespace CompiledHandlebars.Compiler.AST
{
  internal class MarkupLiteral : ASTElementBase
  {
    internal readonly string Value;

    internal MarkupLiteral(string value, int line, int column) : base(line, column)
    {
      Value = value;
    }

    internal override void Accept(IASTVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}
