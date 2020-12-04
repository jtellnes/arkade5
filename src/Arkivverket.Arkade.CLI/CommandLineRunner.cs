using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Arkivverket.Arkade.Core.Base;
using Arkivverket.Arkade.Core.Metadata;
using Arkivverket.Arkade.Core.Testing.Noark5;
using Arkivverket.Arkade.Core.Util;
using Serilog;

namespace Arkivverket.Arkade.CLI
{
    internal static class CommandLineRunner
    {
        private static readonly ILogger Log = Serilog.Log.ForContext(MethodBase.GetCurrentMethod()?.DeclaringType);
        private static readonly Core.Base.Arkade Arkade;

        static CommandLineRunner()
        {
            Arkade = new Core.Base.Arkade();

            Log.Information($"\n" +
                            $"***********************\n" +
                            $"* ARKADE 5 CLI v{ArkadeVersion.Current} *\n" +
                            $"***********************\n");

            Log.Information(GetBundledSoftwareInfo());

            if (Arkade.Version().UpdateIsAvailable())
            {
                Log.Warning("    The current Arkade 5 CLI version is outdated!");
                Log.Information($"Arkade 5 CLI v{Arkade.Version().GetLatest().ToString(3)} is available.");
            }

            Log.Information(
                "Download new releases, see release notes and version history at: " + ArkadeConstants.ArkadeWebSiteUrl + "\n");
        }

        private static string GetBundledSoftwareInfo()
        {
            var info = new StringBuilder();

            info.AppendLine("\n-----------------------BUNDLED SOFTWARE-----------------------\n");
            info.AppendLine("-- Siegfried --");
            info.AppendLine("PURPOSE: identify document file format.");
            info.AppendLine("Copyright © 2019 Richard Lehane");
            info.AppendLine("Available from: https://www.itforarchivists.com/siegfried/");
            info.AppendLine("Licensed under the Apache License, Version 2.0");
            info.AppendLine("\n--------------------------------------------------------------\n");

            return info.ToString();
        }

        public static void Run(ProcessOptions options)
        {
            try
            {
                string command = GetRunningCommand(options.GetType().Name);

                TestSession testSession = CreateTestSession(options.Archive, options.ArchiveType, command,
                    options.LanguageForOutputFiles, options.TestListFile, options.DocumentFileFormatCheck);

                Test(options.OutputDirectory, testSession);

                Pack(options.MetadataFile, options.InformationPackageType, options.OutputDirectory, testSession);

                LogFinishedStatus(command, RanWithoutErrors(testSession));
            }
            catch (ArgumentException e)
            {
                Log.Error(e.Message);
            }
            finally
            {
                ArkadeProcessingArea.CleanUp();
            }
        }

        public static void Run(TestOptions options)
        {
            try
            {
                string command = GetRunningCommand(options.GetType().Name);

                TestSession testSession = CreateTestSession(options.Archive, options.ArchiveType, command,
                    options.LanguageForOutputFiles, options.TestListFile);

                Test(options.OutputDirectory, testSession);

                LogFinishedStatus(command, RanWithoutErrors(testSession));
            }
            catch (ArgumentException e)
            {
                Log.Error(e.Message);
            }
            finally
            {
                ArkadeProcessingArea.CleanUp();
            }
        }

        public static void Run(PackOptions options)
        {
            try
            {
                string command = GetRunningCommand(options.GetType().Name);

                TestSession testSession = CreateTestSession(options.Archive, options.ArchiveType, command,
                    options.LanguageForOutputFiles, checkDocumentFileFormat: options.DocumentFileFormatCheck);

                Pack(options.MetadataFile, options.InformationPackageType, options.OutputDirectory, testSession);

                LogFinishedStatus(command);
            }
            finally
            {
                ArkadeProcessingArea.CleanUp();
            }
        }

        public static void Run(GenerateOptions options)
        {
            string command = GetRunningCommand(options.GetType().Name);

            if (options.GenerateMetadataExample)
            {
                string metadataFileName = Path.Combine(options.OutputDirectory, ArkadeConstants.MetadataFileName);
                new MetadataExampleGenerator().Generate(metadataFileName);
                Log.Information(metadataFileName + " was created");
            }

            if (options.GenerateNoark5TestList)
            {
                string noark5TestListFileName = Path.Combine(options.OutputDirectory, ArkadeConstants.Noark5TestListFileName);
                Noark5TestListGenerator.Generate(noark5TestListFileName);
                Log.Information(noark5TestListFileName + " was created");
            }

            LogFinishedStatus(command);
        }

        public static void Run(AnalyseOptions options)
        {
            string command = GetRunningCommand(options.GetType().Name);

            Log.Information($"{{{command.TrimEnd('e')}ing}} format of all content in {options.FormatCheckTarget}");
            Arkade.GenerateFileFormatInfoFiles(new DirectoryInfo(options.FormatCheckTarget), options.OutputDirectory);

            LogFinishedStatus(command);
        }

        private static void Test(string outputDirectory, TestSession testSession)
        {
            if (!testSession.IsTestableArchive())
            {
                Log.Error("Archive is not testable: Valid specification file not found");
                return;
            }
            Arkade.RunTests(testSession);
            SaveTestReport(testSession, outputDirectory);
        }

        private static void Pack(string metadataFile, string packageType, string outputDirectory,
            TestSession testSession)
        {
            ArchiveMetadata archiveMetadata = MetadataLoader.Load(metadataFile);

            archiveMetadata.PackageType = InformationPackageCreator.ParsePackageType(packageType);

            testSession.ArchiveMetadata = archiveMetadata;
            testSession.ArchiveMetadata.Id = $"UUID:{testSession.Archive.Uuid}";

            Arkade.CreatePackage(testSession, outputDirectory);
        }

        private static ArchiveType GetArchiveType(string archiveTypeString, string archive)
        {
            if (string.IsNullOrWhiteSpace(archiveTypeString))
            {
                ArchiveType? detectedArchiveType = Arkade.DetectArchiveType(archive);

                if (detectedArchiveType == null)
                {
                    string errorMessage =
                        $"Arkade could not detect archive type of {archive}. " +
                        "Please check the structure- or info-file. To attempt a forced run, explicitly specify type with parameter \"-t\"";
                    throw new ArgumentException(errorMessage);
                }
                Log.Information($"Arkade determined {archive} to be of type {detectedArchiveType}");
                return (ArchiveType) detectedArchiveType;
            }

            if (!Enum.TryParse(archiveTypeString, true, out ArchiveType selectedArchiveType))
            {
                Log.Error("Unknown archive type");
                throw new ArgumentException("unknown archive type");
            }

            return selectedArchiveType;
        }

        private static TestSession CreateTestSession(string archive, string archiveTypeString,
            string command, string languageForOutputFiles, string testListFilePath = null,
            bool checkDocumentFileFormat = false)
        {
            var fileInfo = new FileInfo(archive);
            Log.Information($"{{{command}ing}} archive: {fileInfo.FullName}");

            ArchiveType archiveType = GetArchiveType(archiveTypeString, archive);

            TestSession testSession;

            if (File.Exists(archive))
            {
                Log.Debug("File exists");
                testSession = Arkade.CreateTestSession(ArchiveFile.Read(archive, archiveType));
            }
            else if (Directory.Exists(archive))
            {
                Log.Debug("Directory exists");
                testSession = Arkade.CreateTestSession(ArchiveDirectory.Read(archive, archiveType));
            }
            else
            {
                throw new ArgumentException("Invalid archive path: " + archive);
            }

            if (archiveType == ArchiveType.Noark5)
            {
                testSession.TestsToRun = File.Exists(testListFilePath)
                    ? Noark5TestListReader.GetUserSelectedTestIds(testListFilePath)
                    : Noark5TestProvider.GetAllTestIds();

                if (testSession.TestsToRun.Count == 0)
                    throw new ArgumentException($"No tests selected in {testListFilePath}");
            }

            languageForOutputFiles ??= "nb-NO";
            testSession.CultureInfo = CultureInfo.CreateSpecificCulture(languageForOutputFiles);
            testSession.GenerateDocumentFileInfo = checkDocumentFileFormat;

            return testSession;
        }

        private static void SaveTestReport(TestSession testSession, string outputDirectory)
        {
            var packageTestReport = new FileInfo(Path.Combine(
                testSession.GetReportDirectory().FullName, "report.html"
            ));
            Arkade.SaveReport(testSession, packageTestReport);

            var standaloneTestReport = new FileInfo(Path.Combine(
                outputDirectory, string.Format(OutputStrings.TestReportFileName, testSession.Archive.Uuid)
            ));
            Arkade.SaveReport(testSession, standaloneTestReport);
            Log.Information($"Test report generated at: {standaloneTestReport.FullName}");
        }

        private static void LogFinishedStatus(string command, bool withoutErrors = true)
        {
            if (withoutErrors)
                Log.Information($"Arkade 5 CLI {ArkadeVersion.Current} {{{command}}} successfully finished.");
            else
                Log.Warning($"Arkade 5 CLI {ArkadeVersion.Current} {{{command}}} finished with errors.");
        }

        private static string GetRunningCommand(string optionType)
        {
            int optionsStartIndex = optionType.Length - "options".Length;
            return optionType.Remove(optionsStartIndex).ToLower();
        }

        private static bool RanWithoutErrors(TestSession testSession)
        {
            if (!testSession.IsTestableArchive())
                return false;
            return true;
        }
    }
}
