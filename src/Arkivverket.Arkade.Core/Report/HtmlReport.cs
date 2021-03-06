﻿using System.IO;

namespace Arkivverket.Arkade.Core.Report
{
    public class HtmlReport : IReport
    {
        private readonly string _html;

        public HtmlReport(string html)
        {
            _html = html;
        }

        public string GetHtml()
        {
            return _html;
        }

        public void Save(FileInfo fileName)
        {
            if (!fileName.Directory.Exists)
            {
                fileName.Directory.Create();
            }
            File.WriteAllText(fileName.FullName, _html);
        }
    }
}