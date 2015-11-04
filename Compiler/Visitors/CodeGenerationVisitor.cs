﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledHandlebars.Compiler.AST;
using CompiledHandlebars.Compiler.Introspection;
using CompiledHandlebars.Compiler.CodeGeneration;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CompiledHandlebars.Compiler.Visitors
{
  internal class CodeGenerationVisitor : IASTVisitor
  {
    private CompilationState state { get; set; }

    public List<HandlebarsException> ErrorList
    {
      get
      {
        return state.Errors;
      }
    }
    public CodeGenerationVisitor(RoslynIntrospector introspector, HandlebarsTemplate template)
    {
      state = new CompilationState(introspector, template);
      state.Introspector = introspector;
    }
    public void GenerateCode()
    {
      try
      {
        state.Template.Accept(this);
      } catch(HandlebarsTypeError e)
      {
        state.AddTypeError(e);
      } catch(Exception e)
      {
        state.AddTypeError($"Compilation failed: {e.Message}", HandlebarsTypeErrorKind.CompilationFailed);
      }
    }
    public CompilationUnitSyntax CompilationUnit(string templateComment)
    {
      return state.GetCompilationUnit(templateComment);
    }

    public void Visit(MarkupLiteral astLeaf)
    {
      state.SetCursor(astLeaf);
      if (!string.IsNullOrEmpty(astLeaf.Value))
      {//Do not append empty strings (possible through whitespace control)
        state.PushStatement(SyntaxHelper.AppendStringLiteral(astLeaf.Value));
      }
    }

    public void Visit(YieldStatement astLeaf)
    {
      state.SetCursor(astLeaf);
      Context yieldContext = astLeaf.Expr.Evaluate(state);

      if (astLeaf.Type == TokenType.Encoded)
        state.PushStatement(SyntaxHelper.AppendMemberEncoded(yieldContext.FullPath, yieldContext.Symbol?.IsString()??false));
      else
        state.PushStatement(SyntaxHelper.AppendMember(yieldContext.FullPath, yieldContext.Symbol?.IsString()??false));
    }

    public void VisitEnter(HandlebarsTemplate template)
    {
      state.PushStatement(SyntaxHelper.DeclareAndCreateStringBuilder);
    }

    public void VisitLeave(HandlebarsTemplate template)
    {
      state.PushStatement(SyntaxHelper.ReturnSBToString);
    }

    public void VisitEnter(WithBlock astNode)
    {
      state.SetCursor(astNode);
      state.PushNewBlock();
      //Enter new Context and promise to check its truthyness
      state.PromiseTruthyCheck(astNode.Expr.Evaluate(state));
      state.ContextStack.Push(astNode.Expr.Evaluate(state));
    }

    public void VisitLeave(WithBlock astNode)
    {
      //Leave Context
      state.ContextStack.Pop();
      state.DoTruthyCheck(state.PopBlock());
    }

    public void VisitEnter(IfBlock astNode)
    {
      state.SetCursor(astNode);
      state.PushNewBlock();
      state.PromiseTruthyCheck(astNode.Expr.Evaluate(state), astNode.QueryType);
    }

    public void VisitLeave(IfBlock astNode)
    {
      var latestBlock = state.PopBlock();
      if (astNode.HasElseBlock)
        state.DoTruthyCheck(state.PopBlock(), latestBlock, astNode.QueryType);
      else
        state.DoTruthyCheck(latestBlock, ifType: astNode.QueryType);          
    }

    public void VisitElse(IfBlock astNode)
    {
      var truthyContext = state.TruthyStack.Pop();
      truthyContext.Truthy = !truthyContext.Truthy;
      state.TruthyStack.Push(truthyContext);
      state.PushNewBlock();
    }

    public void Visit(CommentLiteral astLeaf)
    {
      state.AddComment(astLeaf.Value);
    }

    public void VisitEnter(EachBlock astNode)
    {
      state.SetCursor(astNode);
      var loopedVariable = astNode.Member.Evaluate(state);
      state.PromiseTruthyCheck(loopedVariable);
      state.ContextStack.Push(astNode.Member.EvaluateLoop(state));   
      state.PushNewBlock();
      state.LoopLevel++;
      if (astNode.Flags.HasFlag(EachBlock.ForLoopFlags.Last))
        state.SetLastVariable(loopedVariable.FullPath);
    }

    public void VisitLeave(EachBlock astNode)
    {
      //Leave loop context
      if (astNode.Flags.HasFlag(EachBlock.ForLoopFlags.Index)|| astNode.Flags.HasFlag(EachBlock.ForLoopFlags.Last))
        state.IncrementIndexVariable();
      if (astNode.Flags.HasFlag(EachBlock.ForLoopFlags.First))
        state.SetFirstVariable();
      state.ContextStack.Pop();
      state.LoopLevel--;
      var prepareStatements = SyntaxHelper.PrepareForLoop(astNode.Flags, state.LoopLevel + 1);
      prepareStatements.Add(SyntaxHelper.ForLoop(astNode.Member.EvaluateLoop(state).FullPath, astNode.Member.Evaluate(state).FullPath, state.PopBlock()));
      state.DoTruthyCheck(
          prepareStatements                        
      );
    }

    public void Visit(PartialCall astLeaf)
    {
      state.SetCursor(astLeaf);
      string memberName = astLeaf.HasExpr ?
                            astLeaf.Expr.Evaluate(state).FullPath :
                            state.ContextStack.Peek().FullPath;
      if (astLeaf.TemplateName.Equals(state.Template.Name))
      {//Self referencing Template
        state.PushStatement(SyntaxHelper.SelfReferencingPartialCall(memberName));
      }
      else
      {
        var partial = state.Introspector.GetPartialHbsTemplate(astLeaf.TemplateName);
        if (partial == null)
        {        
          state.AddTypeError($"Could not find partial '{astLeaf.TemplateName}'", HandlebarsTypeErrorKind.UnknownPartial);
          return;
        }        
        state.PushStatement(
          SyntaxHelper.PartialTemplateCall(
            partial.Name, 
            memberName));        
      }
    }

    public void Visit(HelperCall astLeaf)
    {
      state.SetCursor(astLeaf);
      var helperMethod = state.Introspector.GetHelperMethod(astLeaf.FunctionName, astLeaf.Parameters.Count);
      if (helperMethod != null)
      {
        state.RegisterUsing(helperMethod.ContainingNamespace.ToDisplayString());
        state.PushStatement(
          SyntaxHelper.AppendFuntionCallResult(
            string.Concat(helperMethod.ContainingType.Name,".",helperMethod.Name),
            astLeaf.Parameters.Select(x => x.Evaluate(state).FullPath).ToList()));
      } 
      else
      {//HelperMethod not found
        state.AddTypeError($"Could not find Helper Method '{astLeaf.FunctionName}'", HandlebarsTypeErrorKind.UnknownHelper);
        return;
      }
    }
  }
}
