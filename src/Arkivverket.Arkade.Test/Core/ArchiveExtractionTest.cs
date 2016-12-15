using System.IO;
using Arkivverket.Arkade.Core;
using FluentAssertions;
using Xunit;

namespace Arkivverket.Arkade.Test.Core
{
    public class ArchiveExtractionTest
    {

        [Fact]
        public void ShouldReturnContentDescriptionFileNameForNoark5()
        {
            var workingDirectory = "c:\\dummy";
            var archiveExtraction = new ArchiveBuilder()
                .WithWorkingDirectoryExternalContent(workingDirectory)
                .Build();

            archiveExtraction.GetContentDescriptionFileName().Should().Be($"{workingDirectory}{Path.DirectorySeparatorChar}arkivstruktur.xml");
        }

        [Fact] 
        public void ShouldReturnStructureDescriptionFileNameForNoark5()
        {
            var workingDirectory = "C:\\dummy";
            var archiveExtraction = new ArchiveBuilder()
                .WithWorkingDirectoryExternalContent(workingDirectory)
                .WithArchiveType(ArchiveType.Noark5)
                .Build();

            archiveExtraction.GetStructureDescriptionFileName().Should().Be($"{workingDirectory}{Path.DirectorySeparatorChar}arkivuttrekk.xml");
        }

        [Fact(Skip = "Todo fix path - it relies on creating a proper work directory in order to function correct")]
        public void ShouldReturnStructureDescriptionFileNameForNoark4()
        {
            var workingDirectory = "c:\\dummy";
            var archiveExtraction = new ArchiveBuilder()
                .WithWorkingDirectoryExternalContent(workingDirectory)
                .WithArchiveType(ArchiveType.Noark4)
                .Build();

            archiveExtraction.GetStructureDescriptionFileName().Should().Be($"{workingDirectory}{Path.DirectorySeparatorChar}addml.xml");
        }
    }
}
