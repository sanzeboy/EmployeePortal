using EmployeePortal.Application.Infastructures;
using EmployeePortal.Application.Models;
using Microsoft.AspNetCore.Http;
using Npoi.Mapper;
using NPOI.SS.UserModel;
using System.Data;

namespace EmployeePortal.Infrastructure.Services
{
    public class ExcelService : IExcelService
    {

        /// <summary>
        /// Common Code for the Export
        /// It creates Workbook, Sheet, Generate Header Cells and returns HttpResponseMessage
        /// </summary>
        /// <typeparam name="T">Generic Class Type</typeparam>
        /// <param name="exportData">Data to be exported</param>
        /// <param name="fileName">Export File Name</param>
        /// <param name="sheetName">First Sheet Name</param>
        /// <returns></returns>
        public FileDetail Export<T>(IList<T> exportData, string fileName,
            bool appendDateTimeInFileName = false,
            string sheetName = ExcelUtility.DEFAULT_SHEET_NAME)
        {

            var mapper = new Mapper();
            fileName = appendDateTimeInFileName
                ? $"{fileName}_{DateTime.Now.ToString(ExcelUtility.DEFAULT_FILE_DATETIME)}"
                : fileName;

            using (var memoryStream = new MemoryStream())
            {
                mapper.Save(memoryStream, exportData, sheetName, leaveOpen: false, overwrite: true);

                return new FileDetail
                {
                    ContentType = ExcelUtility.EXCEL_MEDIA_TYPE,
                    FileContents = memoryStream.ToArray(),
                    FileName = $"{fileName}.xlsx"
                };
            }
        }


        public ImportResponse<T> Import<T>(IFormFile file) where T : class
        {
                var response = new ImportResponse<T>()
                {
                    Success = false
                };


            if (file == null || file.Length == 0)
            {
                response.Success = false;
                response.ErrorMessages.Add("File is empty");
                return response;
            }

            string sFileExtension = Path.GetExtension(file.FileName).ToLower();


            using (var stream = file.OpenReadStream())
            {
                try
                {
                    var mapper = new Mapper(stream);

                    var sheet = mapper.Workbook.GetSheetAt(0);
                    IRow headerRow = sheet.GetRow(0);
                  
                    response.Data = mapper.Take<T>(0, 0).Select(x =>
                    {
                        if (x.ErrorColumnIndex > 0)
                        {
                            response.ErrorMessages.Add($"Row {x.RowNumber} - {headerRow.GetCell(x.ErrorColumnIndex)} is invalid.");
                            response.SkippedRows.Add(x.RowNumber);
                        }
                        return x.Value;
                    }).ToList();

                }
                catch (Exception)
                {
                    response.ErrorMessages.Add($"Invalid file");
                }

                finally
                {
                    stream.Close();
                }

                response.Success = response.ErrorMessages.Count == 0;
                return response;
            }
        }
    }



    public class ExcelUtility
    {
        public const string DEFAULT_SHEET_NAME = "Sheet1";
        public const string DEFAULT_FILE_DATETIME = "yyyyMMdd_HHmm";
        public const string DATETIME_FORMAT = "dd/MM/yyyy hh:mm:ss";
        public const string EXCEL_MEDIA_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string DISPOSITION_TYPE_ATTACHMENT = "attachment";


        #region DataType available for Excel Export
        public const string STRING = "string";
        public const string INT32 = "int32";
        public const string DOUBLE = "double";
        public const string DATETIME = "datetime";
        public const string BOOLEAN = "boolean";
        public const string DECIMAL = "decimal";
        #endregion
    }
}
