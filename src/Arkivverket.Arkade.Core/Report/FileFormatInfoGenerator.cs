using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Arkivverket.Arkade.Core.Util;
using Arkivverket.Arkade.Core.Util.FileFormatIdentification;
using CsvHelper;
using CsvHelper.Configuration;

namespace Arkivverket.Arkade.Core.Report
{
    public static class FileFormatInfoGenerator
    {
        private static readonly List<FileTypeStatisticsElement> AmountOfFilesPerFileType = new List<FileTypeStatisticsElement>();
        private static readonly List<ListElement> ListElements = new List<ListElement>();

        public static void Generate(DirectoryInfo filesDirectory, string resultFileDirectoryPath)
        {
            IEnumerable<SiegfriedFileInfo> siegfriedFileInfoSet = GetFormatInfoAllFiles(filesDirectory);

            ArrangeFileFormatStatistics(siegfriedFileInfoSet, filesDirectory.Parent?.Parent);

            WriteFileList(resultFileDirectoryPath);

            WriteFileTypeStatisticsFile(resultFileDirectoryPath);
        }

        private static IEnumerable<SiegfriedFileInfo> GetFormatInfoAllFiles(DirectoryInfo directory)
        {
            var fileFormatIdentifier = new SiegfriedFileFormatIdentifier();

            return fileFormatIdentifier.IdentifyFormat(directory);
        }

        private static void ArrangeFileFormatStatistics(IEnumerable<SiegfriedFileInfo> siegfriedFileInfoSet,
            DirectoryInfo startDirectory)
        {
            foreach (SiegfriedFileInfo siegfriedFileInfo in siegfriedFileInfoSet)
            {
                string fileName = startDirectory != null
                    ? Path.GetRelativePath(startDirectory.FullName, siegfriedFileInfo.FileName)
                    : siegfriedFileInfo.FileName;

                var documentFileListElement = new ListElement
                {
                    FileName = fileName,
                    FileExtension = siegfriedFileInfo.FileExtension,
                    FileFormatPuId = siegfriedFileInfo.Id,
                    FileFormatName = siegfriedFileInfo.Format,
                    FileFormatVersion = siegfriedFileInfo.Version,
                    FileMimeType = siegfriedFileInfo.MimeType,
                    FileScanError = siegfriedFileInfo.Errors,
                };

                ListElements.Add(documentFileListElement);

                string key = documentFileListElement.FileFormatPuId + " - " + documentFileListElement.FileFormatName;

                var fileTypeStatistic = new FileTypeStatisticsElement
                {
                    FileType = key,
                    Amount = 1,
                };

                FileTypeStatisticsElement existingStat = AmountOfFilesPerFileType.Find(t => t.FileType.Equals(key));

                if (existingStat == null)
                    AmountOfFilesPerFileType.Add(fileTypeStatistic);
                else
                    existingStat.Amount++;

            }
        }

        private static void WriteFileList(string fileLocation)
        {
            string fullFileName = Path.Combine(fileLocation, ArkadeConstants.FileFormatInfoFileName);

            using (var writer = new StreamWriter(fullFileName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<ListElementMap>();
                csv.WriteRecords(ListElements);
            }
        }

        private static void WriteFileTypeStatisticsFile(string fileLocation)
        {
            string fullFileName = Path.Combine(fileLocation, ArkadeConstants.FileFormatInfoStatisticsFileName);

            using (var writer = new StreamWriter(fullFileName))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.RegisterClassMap<FileTypeStatisticsElementMap>();
                    csv.WriteRecords(AmountOfFilesPerFileType);
                }
            }
        }

        private sealed class ListElementMap : ClassMap<ListElement>
        {
            public ListElementMap()
            {
                Map(m => m.FileName).Name(FormatAnalysisResultFileContent.HeaderFileName);
                Map(m => m.FileExtension).Name(FormatAnalysisResultFileContent.HeaderFileExtension);
                Map(m => m.FileFormatPuId).Name(FormatAnalysisResultFileContent.HeaderFormatId);
                Map(m => m.FileFormatName).Name(FormatAnalysisResultFileContent.HeaderFormatName);
                Map(m => m.FileFormatVersion).Name(FormatAnalysisResultFileContent.HeaderFormatVersion);
                Map(m => m.FileMimeType).Name(FormatAnalysisResultFileContent.HeaderMimeType);
                Map(m => m.FileScanError).Name(FormatAnalysisResultFileContent.HeaderErrors);
            }
        }

        private sealed class FileTypeStatisticsElementMap : ClassMap<FileTypeStatisticsElement>
        {
            public FileTypeStatisticsElementMap()
            {
                Map(m => m.FileType).Name(FormatAnalysisResultFileContent.StatisticsHeaderFileType);
                Map(m => m.Amount).Name(FormatAnalysisResultFileContent.StatisticsHeaderAmount);
            }
        }

        private class ListElement
        {
            public string FileName { get; set; }
            public string FileExtension { get; set; }
            public string FileFormatPuId { get; set; }
            public string FileFormatName { get; set; }
            public string FileFormatVersion { get; set; }
            public string FileMimeType { get; set; }
            public string FileScanError { get; set; }
        }

        private class FileTypeStatisticsElement
        {
            public string FileType { get; set; }
            public int Amount { get; set; }
        }
    }
}
