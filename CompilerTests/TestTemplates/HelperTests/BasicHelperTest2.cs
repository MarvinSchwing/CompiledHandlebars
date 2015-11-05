﻿using System;
using System.Linq;
using System.Net;
using System.Text;
using CompiledHandlebars.CompilerTests;

/*05.11.2015 10:55:34 | parsing: 0ms; init: 8; codeGeneration: 5!*/
namespace TestTemplates
{
  [CompiledHandlebarsTemplate]
  public static class BasicHelperTest2
  {
    public static string Render(CompiledHandlebars.CompilerTests.TestViewModels.MarsModel viewModel)
    {
      var sb = new StringBuilder();
      sb.Append(HelperTests.ToUpper(viewModel.Name));
      sb.Append(HelperTests.ToLower(viewModel.Name));
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
}/**/