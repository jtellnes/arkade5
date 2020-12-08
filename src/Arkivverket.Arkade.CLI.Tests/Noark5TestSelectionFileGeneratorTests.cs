using Xunit;
using System;
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
            string workingDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string testFilePath = Path.Combine(workingDirectoryPath, OutputFileNames.Noark5TestSelectionFile);

            if (File.Exists(testFilePath))
                File.Delete(testFilePath);

            Noark5TestSelectionFileGenerator.Generate(testFilePath);

            File.Exists(testFilePath).Should().BeTrue();

            var testList = new StringReader(File.ReadAllText(testFilePath));

            testList.ReadLine().Should().Be("# " + OutputStrings.Noark5TestSelectionFileHeading);

            File.Delete(testFilePath);
        }
    }
}
