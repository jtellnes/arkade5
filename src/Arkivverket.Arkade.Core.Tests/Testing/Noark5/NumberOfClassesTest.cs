using Arkivverket.Arkade.Core.Base;
using Arkivverket.Arkade.Core.Testing.Noark5;
using FluentAssertions;
using Xunit;

namespace Arkivverket.Arkade.Core.Tests.Testing.Noark5
{
    public class NumberOfClassesTest
    {
        [Fact]
        public void NumberOfClassesIsTwoInEachArchivePart()
        {
            XmlElementHelper helper = new XmlElementHelper().Add("arkiv",
                new XmlElementHelper()
                    // Arkivdel 1
                    .Add("arkivdel", new XmlElementHelper()
                        .Add("systemID", "someSystemId_1")
                        // Primært klassifikasjonssystem (inneholder registrering eller mappe)
                        .Add("klassifikasjonssystem", new XmlElementHelper()
                            .Add("systemID", "klassSys_1")
                            .Add("registrering", new XmlElementHelper()
                                .Add("klasse", string.Empty)
                                .Add("klasse", new XmlElementHelper()
                                    .Add("klasse", new XmlElementHelper()))))
                        // Sekundært klassifikasjonssystem
                        .Add("klassifikasjonssystem", new XmlElementHelper()
                            .Add("systemID", "klassSys_2")
                            .Add("klasse", string.Empty)
                            .Add("klasse", new XmlElementHelper()
                                .Add("klasse", new XmlElementHelper()))))
                    // Arkivdel 2
                    .Add("arkivdel", new XmlElementHelper()
                        .Add("systemID", "someSystemId_2")
                        // Primært klassifikasjonssystem (inneholder registrering eller mappe)
                        .Add("klassifikasjonssystem", new XmlElementHelper()
                            .Add("systemID", "klassSys_1")
                            .Add("mappe", new XmlElementHelper()
                                .Add("klasse", string.Empty)
                                .Add("klasse", new XmlElementHelper()
                                    .Add("klasse", new XmlElementHelper()))))
                        // Sekundært klassifikasjonssystem
                        .Add("klassifikasjonssystem", new XmlElementHelper()
                            .Add("systemID", "klassSys_2")
                            .Add("klasse", string.Empty)
                            .Add("klasse", new XmlElementHelper()
                                .Add("klasse", new XmlElementHelper())))));

            TestRun testRun = helper.RunEventsOnTest(new NumberOfClasses());

            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID): someSystemId_1 - Primært klassifikasjonssystem (systemID): klassSys_1 - Totalt antall klasser: 3"));
            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID): someSystemId_1 - Primært klassifikasjonssystem (systemID): klassSys_1 - Klasser på nivå 1: 2"));
            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID): someSystemId_1 - Primært klassifikasjonssystem (systemID): klassSys_1 - Klasser på nivå 2: 1"));

            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID): someSystemId_1 - Sekundært klassifikasjonssystem (systemID): klassSys_2 - Totalt antall klasser: 3"));
            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID): someSystemId_1 - Sekundært klassifikasjonssystem (systemID): klassSys_2 - Klasser på nivå 1: 2"));
            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID): someSystemId_1 - Sekundært klassifikasjonssystem (systemID): klassSys_2 - Klasser på nivå 2: 1"));

            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID): someSystemId_2 - Primært klassifikasjonssystem (systemID): klassSys_1 - Totalt antall klasser: 3"));
            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID): someSystemId_2 - Primært klassifikasjonssystem (systemID): klassSys_1 - Klasser på nivå 1: 2"));
            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID): someSystemId_2 - Primært klassifikasjonssystem (systemID): klassSys_1 - Klasser på nivå 2: 1"));

            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID): someSystemId_2 - Sekundært klassifikasjonssystem (systemID): klassSys_2 - Totalt antall klasser: 3"));
            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID): someSystemId_2 - Sekundært klassifikasjonssystem (systemID): klassSys_2 - Klasser på nivå 1: 2"));
            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID): someSystemId_2 - Sekundært klassifikasjonssystem (systemID): klassSys_2 - Klasser på nivå 2: 1"));
        }
    }
}
