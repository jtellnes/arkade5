using System;
using System.IO;
using System.Linq;
using Arkivverket.Arkade.Core.Base;

namespace Arkivverket.Arkade.Core.Statistics
{
    public static class Statistics
    {
        public static void Collect(TestSession testSession)
        {
            DateTime dateOfTesting = testSession.DateOfTesting;
            long totalDuration = testSession.TestSuite.TestRuns.Sum(t => t.TestDuration);
            ArchiveType archiveType = testSession.Archive.ArchiveType;
            int numberOfErrors = testSession.TestSuite.FindNumberOfErrors();
            bool isTestableArchive = testSession.IsTestableArchive();
            
            string numberOfProcessedFiles = testSession.TestSummary.NumberOfProcessedFiles;
            string numberOfProcessedRecords = testSession.TestSummary.NumberOfProcessedRecords;
            string numberOfTestsRun = testSession.TestSummary.NumberOfTestsRun;
        }

        public static void Collect(TestSession testSession, string packageFilePath)
        {
            long packageSize = new FileInfo(packageFilePath).Length;

        }

        private static void Submit(StatisticsUnit statisticsUnit)
        {
            //  
        }
    }
}
