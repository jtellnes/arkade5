﻿using Xunit;
using System;
using System.Globalization;
using System.IO;
using Arkivverket.Arkade.Core.Resources;
using FluentAssertions;

namespace Arkivverket.Arkade.CLI.Tests
{
    public class Noark5TestSelectionFileGeneratorTests
    {
        [Fact]
        public void GenerateTest()
        {
            var culture = new CultureInfo("en");
            
            OutputFileNames.Culture = culture;

            string workingDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string testFilePath = Path.Combine(workingDirectoryPath, OutputFileNames.Noark5TestSelectionFile);

            if (File.Exists(testFilePath))
                File.Delete(testFilePath);

            Noark5TestSelectionFileGenerator.Generate(testFilePath, culture);

            File.Exists(testFilePath).Should().BeTrue();

            var testSelectionFileContent = new StringReader(File.ReadAllText(testFilePath));

            testSelectionFileContent.ReadLine().Should().Be("# " + OutputStrings.Noark5TestSelectionFileHeading);

            File.Delete(testFilePath);
        }
    }
}
