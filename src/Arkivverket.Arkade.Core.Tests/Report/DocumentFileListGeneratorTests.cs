using System;
using System.IO;
using Xunit;
using Arkivverket.Arkade.Core.Base;
using Arkivverket.Arkade.Core.Report;
using Arkivverket.Arkade.Core.Util;
using FluentAssertions;

namespace Arkivverket.Arkade.Core.Tests.Report
{
    public class DocumentFileListGeneratorTests
    {
        [Fact]
        public void GenerateTest()
        {
            string workingDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "Report", "FilesToBeListed");
            string testFilePath = Path.Combine(workingDirectoryPath, ArkadeConstants.DocumentFileListFileName);

            var testArchive = new Archive(ArchiveType.Noark5, null,
                new WorkingDirectory(new DirectoryInfo(workingDirectoryPath)));

            if (File.Exists(testFilePath))
                File.Delete(testFilePath);

            DocumentFileListGenerator.Generate(workingDirectoryPath, testArchive);

            File.Exists(testFilePath).Should().BeTrue();

            File.Delete(testFilePath);
        }
    }
}
