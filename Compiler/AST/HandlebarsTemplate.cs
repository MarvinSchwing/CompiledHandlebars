﻿using CompiledHandlebars.Compiler.AST.Expressions;
using CompiledHandlebars.Compiler.Visitors;
using System.Collections.Generic;

namespace CompiledHandlebars.Compiler.AST
{

 
  internal class HandlebarsLayout : HandlebarsTemplate
  {
    private IList<ASTElementBase> _preItems;
    private IList<ASTElementBase> _postItems;

    internal HandlebarsLayout(IList<ASTElementBase> preItems, IList<ASTElementBase> postItems, MemberExpression model, IList<HandlebarsSyntaxError> parseErrors)
      :base(null, model, parseErrors)
    {
      _preItems = preItems;
      _postItems = postItems;
    }

    internal override void Accept(IASTVisitor visitor)
    {
      visitor.VisitEnter(this);
      foreach (var item in _preItems)
        item.Accept(visitor);
      visitor.VisitRenderBody(this);
      foreach (var item in _postItems)
        item.Accept(visitor);
      visitor.VisitLeave(this);
    }
  }

  internal class LayoutedHandlebarsTemplate : HandlebarsTemplate
  {
    internal readonly string LayoutName;

    internal LayoutedHandlebarsTemplate(string layout, IList<ASTElementBase> items, MemberExpression/*Not clean. This is no memberExpression but a typename. TODO:FIX*/ model, IList<HandlebarsSyntaxError> parseErrors)
      :base(items, model, parseErrors)
    {
      LayoutName = layout;
    }

    internal override void Accept(IASTVisitor visitor)
    {
      visitor.VisitEnter(this);
      foreach (var item in _items)
        item.Accept(visitor);
      visitor.VisitLeave(this);
    }
  }

  internal class HandlebarsTemplate
  {
    internal readonly IList<HandlebarsSyntaxError> ParseErrors;
    internal MemberExpression Model { get; set; }
    internal string Name { get; set; }
    internal string Namespace { get; set; }
    protected IList<ASTElementBase> _items { get; set; }

    internal HandlebarsTemplate(IList<ASTElementBase> items, MemberExpression model, IList<HandlebarsSyntaxError> parseErrors)
    {
      Model = model;
      ParseErrors = parseErrors;
      _items = items;
    }

    internal virtual void Accept(IASTVisitor visitor)
    {
      visitor.VisitEnter(this);
      foreach (var item in _items)
        item.Accept(visitor);
      visitor.VisitLeave(this);
    }

  }
}
