﻿using System.Text;
using System.Net;
using System;

/*10/19/2015 11:10:17 PM | parsing: 1ms; init: 2; codeGeneration: 39!*/
namespace TestTemplates
{
  [CompiledHandlebarsTemplate]
  public static class BasicPartialTest2
  {
    public static string Render(System.String viewModel)
    {
      var sb = new StringBuilder();
      sb.Append(BasicPartialTest1.Render(viewModel));
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