﻿using System.Linq;
using System.Text;
using System.Net;
using System;

/*10/30/2015 8:55:28 AM | parsing: 3ms; init: 2; codeGeneration: 2!*/
namespace TestTemplates
{
  [CompiledHandlebarsTemplate]
  public static class CommentTest1
  {
    public static string Render(CompiledHandlebars.CompilerTests.TestViewModels.MarsModel viewModel)
    {
      var sb = new StringBuilder();
      ; /*Name*/
      return sb.ToString();
    }

    private static bool IsTruthy(bool b)
    {
      return b;
    }

    private static bool IsTruthy(string s)
    {
      return !string.IsNullOrEmpty(s);
    }

    private static bool IsTruthy(object o)
    {
      return o != null;
    }

    private class CompiledHandlebarsTemplateAttribute : Attribute
    {
    }
  }
}