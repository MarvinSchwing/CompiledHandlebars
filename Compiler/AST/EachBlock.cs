﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledHandlebars.Compiler.Visitors;
using CompiledHandlebars.Compiler.AST.Expressions;

namespace CompiledHandlebars.Compiler.AST
{
  internal class EachBlock : ASTNode
  {
    internal readonly MemberExpression Member;
    internal EachBlock(MemberExpression member, IList<ASTElementBase> children, int line, int column) : base(children, line, column)
    {
      Member = member;
    }
    internal override void Accept(IASTVisitor visitor)
    {
      visitor.VisitEnter(this);
      foreach (var child in _children)
        child.Accept(visitor);
      visitor.VisitLeave(this);
    }

    /// <summary>
    /// Returns true if any of the child elements have an LastExpression
    /// </summary>
    /// <returns></returns>
    internal bool BodyContainsFirstExpression()
    {
      foreach (var child in _children)
        if (child is IfBlock)
          return ((child as IfBlock).Expr is FirstExpression);
      return false;
    }

    /// <summary>
    /// Returns true if any of the child elements have a LastExpression
    /// </summary>
    /// <returns></returns>
    internal bool BodyContainsLastExpression()
    {
      foreach (var child in _children)
        if (child is IfBlock)
          return ((child as IfBlock).Expr is LastExpression);
      return false;
    }

    internal override bool HasExpression<T>(bool includeChildren = false)
    {
      if (Member is T)
        return true;
      if (includeChildren)
      {
        return _children.Any(x => x.HasExpression<T>(includeChildren));
      }
      return false;
    }
  }
}
