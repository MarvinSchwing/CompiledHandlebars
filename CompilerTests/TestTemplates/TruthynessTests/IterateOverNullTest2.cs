﻿using System.Linq;
using System.Text;
using System.Net;
using System;

/*10/30/2015 8:55:27 AM | parsing: 0ms; init: 3; codeGeneration: 1!*/
namespace TestTemplates
{
  [CompiledHandlebarsTemplate]
  public static class IterateOverNullTest2
  {
    public static string Render(CompiledHandlebars.CompilerTests.TestViewModels.StarModel viewModel)
    {
      var sb = new StringBuilder();
      if (IsTruthy(viewModel) && IsTruthy(viewModel.Planets))
      {
        foreach (var loopItem0 in viewModel.Planets)
        {
          if (IsTruthy(loopItem0) && IsTruthy(loopItem0.Moons))
          {
            foreach (var loopItem1 in loopItem0.Moons)
            {
              sb.Append(WebUtility.HtmlEncode(loopItem1.Name));
            }
          }
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