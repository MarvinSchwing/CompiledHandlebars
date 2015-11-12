﻿using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace CompiledHandlebars.CompilerTests.HandlebarsJsSpec.Builtins
{
  [CompiledHandlebarsTemplate]
  public static class If1
  {
    public static string Render(CompiledHandlebars.CompilerTests.HandlebarsJsSpec.Builtins.GoodbyeModel viewModel)
    {
      var sb = new StringBuilder();
      if (IsTruthy(viewModel) && IsTruthy(viewModel.StrGoodbye))
      {
        sb.Append("GOODBYE ");
      }

      sb.Append("cruel ");
      sb.Append(WebUtility.HtmlEncode(viewModel.World));
      sb.Append("!");
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

    private static bool IsTruthy<T>(IEnumerable<T> ie)
    {
      return ie != null && ie.Any();
    }

    private class CompiledHandlebarsTemplateAttribute : Attribute
    {
    }
  }
}/**/