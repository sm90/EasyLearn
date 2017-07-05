using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using OfficeOpenXml;

namespace Portal.Web.Models
{
    public class ImportUsers
    {
        private string _file;
        private List<ImportMessages> _resultMessages = new List<ImportMessages>();
        //private bool _error = false;
        public string _sheetname { get; set; }
        private List<string> _columnsMissing = new List<string>();
        private const int _startingLine = 2; // Linjenummeret den første artikkelen har i malen.
       // private XDocument _document;
        private int _companyId;
        private ExcelPackage _excelPackage;

        public ImportUsers(int companyId, ExcelPackage excelPackage, string sheetname)
        {
            _companyId = companyId;
            _excelPackage = excelPackage;
            _sheetname = sheetname;

        }

        private void AddResultMessage(string message, bool error)
        {
            _resultMessages.Add(new ImportMessages(message, error));
        }

        public bool Import()
        {

            ExcelWorksheet worksheet = _excelPackage.Workbook.Worksheets[_sheetname];

            int col = 1;



            for (int row = _startingLine; worksheet.Cells[row, col].Value != null; row++)
            {

            }

            return true;
        }
    }
}