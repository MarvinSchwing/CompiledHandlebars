﻿using System.Linq;
using System.Text;
using System.Net;
using System;

/*10/30/2015 8:55:27 AM | parsing: 0ms; init: 2; codeGeneration: 1!*/
namespace TestTemplates
{
  [CompiledHandlebarsTemplate]
  public static class RedundantTruthynessCheckTest2
  {
    public static string Render(CompiledHandlebars.CompilerTests.TestViewModels.MarsModel viewModel)
    {
      var sb = new StringBuilder();
      if (IsTruthy(viewModel) && IsTruthy(viewModel.Rovers))
      {
        foreach (var loopItem0 in viewModel.Rovers)
        {
          sb.Append(WebUtility.HtmlEncode(loopItem0.Value.Name));
        }
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