﻿using System;
using System.IO;
using Arkivverket.Arkade.Core;
using FluentAssertions;
using Xunit;

namespace Arkivverket.Arkade.Test.Core
{
    public class DocumentsDirectoryTest
    {
        private static DirectoryInfo _physicalWorkingDirectory;

        public DocumentsDirectoryTest()
        {
            _physicalWorkingDirectory = new DirectoryInfo(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "DocumentDirectoryTest")
            );

            if (!_physicalWorkingDirectory.Exists)
                _physicalWorkingDirectory.Create();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void ArhiveDetectsDocumentsDirectoriesWithSupportedNames()
        {
            // Plural, lowercase documents directory name:
            DirectoryInfo documentsDirectoryA = SetupArchiveWithPhysicalDocumentsDirectory("dokumenter");
            documentsDirectoryA.Name.Should().Be("dokumenter");
            documentsDirectoryA.Exists.Should().BeTrue();

            // Plural, uppercase documents directory name:
            DirectoryInfo documentsDirectoryB = SetupArchiveWithPhysicalDocumentsDirectory("DOKUMENTER");
            documentsDirectoryB.Name.Should().Be("DOKUMENTER");
            documentsDirectoryB.Exists.Should().BeTrue();

            // Singular, lowercase documents directory name:
            DirectoryInfo documentsDirectoryC = SetupArchiveWithPhysicalDocumentsDirectory("dokument");
            documentsDirectoryC.Name.Should().Be("dokument");
            documentsDirectoryC.Exists.Should().BeTrue();

            // Singular, uppercase documents directory name:
            DirectoryInfo documentsDirectoryD = SetupArchiveWithPhysicalDocumentsDirectory("DOKUMENT");
            documentsDirectoryD.Name.Should().Be("DOKUMENT");
            documentsDirectoryD.Exists.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void DocumentsDirectoriesWithUnsupportedNamesResultsInFallback()
        {
            // Mixed case documents directory name:
            DirectoryInfo documentsDirectoryA = SetupArchiveWithPhysicalDocumentsDirectory("Dokumenter");
            documentsDirectoryA.Name.Should().Be("dokumenter");

            // English documents directory name:
            DirectoryInfo documentsDirectoryB = SetupArchiveWithPhysicalDocumentsDirectory("documenter");
            documentsDirectoryB.Name.Should().Be("dokumenter");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void MissingDocumentsDirectoryResultsInFallback()
        {
            // Missing documents directory:
            DirectoryInfo documentsDirectoryA = SetupArchiveWithPhysicalDocumentsDirectory(string.Empty);
            documentsDirectoryA.Name.Should().Be("dokumenter");
            documentsDirectoryA.Exists.Should().BeFalse();
        }

        private static DirectoryInfo SetupArchiveWithPhysicalDocumentsDirectory(string documentsDirectoryName)
        {
            // Remove any existing document directories:
            RemovePhysicalContentsDirectory();

            // Make new archive to reset any existing documentsdirectory reference:
            Archive archive = SetupArchive();

            // Create an actual documentsdirectory for the archive to look for:
            CreatePhysicalDocumentsDirectory(documentsDirectoryName);

            // Return what the archive has defined as its documentsdirectory:
            return archive.GetDocumentsDirectory();
        }
        private static void RemovePhysicalContentsDirectory()
        {
            var contentDirectory = new DirectoryInfo(Path.Combine(_physicalWorkingDirectory.FullName, "content"));

            if (contentDirectory.Exists)
                contentDirectory.Delete(true);
        }

        private static Archive SetupArchive()
        {
            return new ArchiveBuilder()
                .WithArchiveType(ArchiveType.Noark5)
                .WithWorkingDirectoryRoot(_physicalWorkingDirectory.FullName)
                .Build();
        }

        private static void CreatePhysicalDocumentsDirectory(string documentsDirectoryName)
        {
            Directory.CreateDirectory(
                Path.Combine(_physicalWorkingDirectory.FullName, "content", documentsDirectoryName)
            );
        }
    }
}
