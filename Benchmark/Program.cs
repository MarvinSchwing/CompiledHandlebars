﻿using CompiledHandlebars.Benchmark.MeasurementModels;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CompiledHandlebars.Benchmark
{
  class Program
  {
    static void Main(string[] args)
    {      
      Console.WriteLine("Started Benchmark...");
      var benchCase = Benchmarker.CreateFullBenchmark();
      PrintSummary(benchCase.Summary);
      if (args.Length == 3 && args[0].Equals("-s"))
      {//if save-flag is set -> write benchmark result to blobstorage as json
        var commitHash = args[1];
        benchCase.CommitHash = commitHash;
        var cloudStorageConnectionString = args[2];
        var storageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
        var blobClient = storageAccount.CreateCloudBlobClient();
        var container = blobClient.GetContainerReference("results");
        var blockBlob = container.GetBlockBlobReference($"{commitHash}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.json");
        var json = JsonConvert.SerializeObject(benchCase, Formatting.Indented);
        blockBlob.UploadText(json);
      }
      else if (args.Length == 2 && args[0].Equals("-l"))
      {
        var dirPath = args[2];
        DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
        if (!dirInfo.Exists)
          dirInfo.Create();
        var json = JsonConvert.SerializeObject(benchCase, Formatting.Indented);
        File.WriteAllText(Path.Combine(dirInfo.FullName, $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.json"), json);
      }
    }


    static void PrintSummary(BenchmarkSummary summary)
    {
      Console.WriteLine("Summary: ");
      Console.WriteLine($"Average Standard Deviation: {summary.AverageStandardDeviation*100}%");
      Console.WriteLine($"{"Name",20}{"Average Throughput", 22}{"Standard Deviation",22}{"Sample Size",15}");
      foreach (var item in summary.Items.Select(x => x.Value).ToList())
        Console.WriteLine($"{item.Name,20}{item.AverageThroughput,22}{item.StandardDeviation,22}{item.SampleSize,15}");
    }

   
  }
}
