﻿using System.Text;
using System.Net;
using System;

/*10/19/2015 11:10:21 PM | parsing: 0ms; init: 2; codeGeneration: 0!*/
namespace TestTemplates
{
  [CompiledHandlebarsTemplate]
  public static class UnlessElseTest
  {
    public static string Render(CompiledHandlebars.CompilerTests.TestViewModels.MarsModel viewModel)
    {
      var sb = new StringBuilder();
      if (!IsTruthy(viewModel) || !IsTruthy(viewModel.Name))
      {
        sb.Append("HasNoName");
      }
      else
      {
        sb.Append("HasName");
      }

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