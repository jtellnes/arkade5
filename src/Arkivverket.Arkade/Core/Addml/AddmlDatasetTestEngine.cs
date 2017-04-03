using System;
using System.Collections.Generic;
using Arkivverket.Arkade.Core.Addml.Definitions;
using Arkivverket.Arkade.Logging;
using System.Linq;
using Arkivverket.Arkade.Tests;

namespace Arkivverket.Arkade.Core.Addml
{
    public class AddmlDatasetTestEngine : ITestEngine
    {
        private readonly AddmlProcessRunner _addmlProcessRunner;
        private readonly IStatusEventHandler _statusEventHandler;
        private readonly FlatFileReaderFactory _flatFileReaderFactory;
        private readonly List<TestResult> _testResultsFailedRecordsList = new List<TestResult>();
        private int _totalFoundFieldDelimErrors = 0;

        public AddmlDatasetTestEngine(FlatFileReaderFactory flatFileReaderFactory, AddmlProcessRunner addmlProcessRunner, IStatusEventHandler statusEventHandler)
        {
            _flatFileReaderFactory = flatFileReaderFactory;
            _addmlProcessRunner = addmlProcessRunner;
            _statusEventHandler = statusEventHandler;
        }
        
        public TestSuite RunTestsOnArchive(TestSession testSession)
        {
            AddmlDefinition addmlDefinition = testSession.AddmlDefinition;

            _addmlProcessRunner.Init(addmlDefinition);

            List<FlatFile> flatFiles = addmlDefinition.GetFlatFiles();

            foreach (FlatFile file in flatFiles)
            {
                string testName = string.Format(Resources.Messages.RunningAddmlProcessesOnFile, file.GetName());
                int recordIdx = 0;
                _statusEventHandler.RaiseEventFileProcessingStarted(new FileProcessingStatusEventArgs(testName, file.GetName()));

                _addmlProcessRunner.RunProcesses(file);

                IRecordEnumerator recordEnumerator = _flatFileReaderFactory.GetRecordEnumerator(testSession.Archive, file);

                while (recordEnumerator != null && recordEnumerator.MoveNext())
                {
                    try
                    {
                        _statusEventHandler.RaiseEventRecordProcessingStart();
                        Record record = recordEnumerator.Current;
                        _addmlProcessRunner.RunProcesses(file, record);

                        foreach (Field field in record.Fields)
                        {
                            _addmlProcessRunner.RunProcesses(file, field);
                        }

                    }
                    catch (ArkadeAddmlDelimiterException afed)
                    {
                        _totalFoundFieldDelimErrors++;
                        _testResultsFailedRecordsList.Add(new TestResult(ResultType.Error, new AddmlLocation(file.GetName(), afed.RecordName,""), afed.Message + " Felt tekst: " + afed.RecordData));
                        new EventReportingHelper(_statusEventHandler).RaiseEventOperationMessageErrorAddmlFieldDelim($"{Resources.AddmlMessages.RecordLengthErrorTestName} i fil {file.GetName()}, post nummer {recordIdx}, feil nummer {_testResultsFailedRecordsList.Count}" , 
                            afed.Message + " Felt tekst: " + afed.RecordData, OperationMessageStatus.Error, _totalFoundFieldDelimErrors);
                       
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        _statusEventHandler.RaiseEventRecordProcessingStopped();
                    }

                    recordIdx++;
                }
                _addmlProcessRunner.EndOfFile(file);

                _statusEventHandler.RaiseEventFileProcessingFinished(new FileProcessingStatusEventArgs(testName, file.GetName(), true));
            }

            TestSuite testSuite = _addmlProcessRunner.GetTestSuite();

            HardcodedProcessRunner p = new HardcodedProcessRunner(addmlDefinition, testSession.Archive);
            p.Run().ForEach(t => testSuite.AddTestRun(t));


            var failedRecoredTestRun = new TestRun(Resources.AddmlMessages.RecordLengthErrorTestName, TestType.Structure)
            {
                Results = _testResultsFailedRecordsList,
            };

            testSuite.AddTestRun(failedRecoredTestRun);

            return testSuite;
        }
    }
}