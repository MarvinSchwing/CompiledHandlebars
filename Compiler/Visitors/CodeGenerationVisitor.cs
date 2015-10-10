﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledHandlebars.Compiler.AST;
using CompiledHandlebars.Compiler.Introspection;
using CompiledHandlebars.Compiler.CodeGeneration;

namespace CompiledHandlebars.Compiler.Visitors
{
  internal class CodeGenerationVisitor : IASTVisitor
  {
    private CompilationState state { get; set; }
    public CodeGenerationVisitor(RoslynIntrospector introspector, HandlebarsTemplate template)
    {
      state = new CompilationState(introspector, template);
      state.Introspector = introspector;
    }

    public void Visit(MarkupLiteral astLeaf)
    {
      state.SetCursor(astLeaf);
      state.PushStatement(SyntaxHelper.AppendStringLiteral(astLeaf.Value));
    }

    public void Visit(YieldStatement astLeaf)
    {
      state.SetCursor(astLeaf);
      if (astLeaf._type == TokenType.Encoded)
        state.PushStatement(SyntaxHelper.AppendMemberEncoded(astLeaf.Member.Evaluate(state)));
      else
        state.PushStatement(SyntaxHelper.AppendMember(astLeaf.Member.Evaluate(state)));
    }

    public void VisitEnter(HandlebarsTemplate template)
    {
      state.PushStatement(SyntaxHelper.DeclareAndCreateStringBuilder);
    }

    public void VisitLeave(HandlebarsTemplate template)
    {
      state.PushStatement(SyntaxHelper.ReturnSBToString);
    }

    internal CompilationState GenerateCode()
    {
      state.Template.Accept(this);
      return state;
    }

  }
}
