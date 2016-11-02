﻿using System.Collections.Generic;
using System.Linq;
using Arkivverket.Arkade.Core;
using Arkivverket.Arkade.Core.Addml.Processes;
using FluentAssertions;
using Xunit;

namespace Arkivverket.Arkade.Test.Integration
{
    public class Noark3IntegrationTest
    {
        [Fact]
        public void Test1()
        {
            ArchiveFile archive1 =
                ArchiveFile.Read("..\\..\\TestData\\tar\\Noark3-eksempel-1\\c3db9d4e-720c-4f75-bfb6-de90231dc44c.tar");
            //ArchiveDirectory archive2 = ArchiveDirectory.Read("..\\..\\TestData\\Noark3-eksempel-1\\");

            Arkade.Core.Arkade arkade = new Arkade.Core.Arkade();
            TestSession testSesson = arkade.RunTests(archive1);

            testSesson.Should().NotBeNull();
            TestSuite testSuite = testSesson.TestSuite;
            testSuite.Should().NotBeNull();
            testSuite.TestRuns.Should().NotBeNullOrEmpty();

            List<TestRun> analyseFindMinMaxValues = testSuite.TestRuns
                .Where(run => run.TestName == AnalyseFindMinMaxValues.Name)
                .ToList();
            analyseFindMinMaxValues.Count.Should().Be(1);
        }
    }
}