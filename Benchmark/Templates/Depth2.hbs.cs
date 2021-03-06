﻿using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace CompiledHandlebars.Benchmark.ViewModels.MeasurementModels.Templates.Templates
{
  [CompiledHandlebarsTemplate]
  public static class Depth2
  {
    public static string Render(CompiledHandlebars.Benchmark.ViewModels.Depth2Model viewModel)
    {
      var sb = new StringBuilder(64);
      if (IsTruthy(viewModel) && IsTruthy(viewModel.Names))
      {
        foreach (var loopItem0 in viewModel.Names)
        {
          if (IsTruthy(loopItem0) && IsTruthy(loopItem0.Name))
          {
            foreach (var loopItem1 in loopItem0.Name)
            {
              sb.Append(WebUtility.HtmlEncode(loopItem0.Bat));
              sb.Append(WebUtility.HtmlEncode(viewModel.Foo));
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

    private static bool IsTruthy<T>(IEnumerable<T> ie)
    {
      return ie != null && ie.Any();
    }

    private static bool IsTruthy(int i)
    {
      return i != 0;
    }

    private class CompiledHandlebarsTemplateAttribute : Attribute
    {
    }

    private class CompiledHandlebarsLayoutAttribute : Attribute
    {
    }
  }
}