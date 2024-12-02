using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace EmployeePortal.Application
{
    public static class FormFileExtension
    {
        public const int ImageMinimumBytes = 512;

        public static bool IsCsvFile(this IFormFile postedFile)
        {
            if (postedFile == null || postedFile.Length == 0)
                return false;

            var allowedContentTypes = new[] { "text/csv", "application/vnd.ms-excel" }; // text/csv is the standard MIME type for CSV
            var allowedExtensions = new[] { ".csv" };

            var contentType = postedFile.ContentType.ToLower();
            var fileExtension = Path.GetExtension(postedFile.FileName).ToLower();

            return allowedContentTypes.Contains(contentType) && allowedExtensions.Contains(fileExtension);
        }

        public static bool IsExcelFile(this IFormFile postedFile)
        {
            if (postedFile == null || postedFile.Length == 0)
                return false;

            var allowedContentTypes = new[] {
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", // .xlsx
            "application/vnd.ms-excel" // .xls
        };

            var allowedExtensions = new[] { ".xlsx", ".xls" };

            var contentType = postedFile.ContentType.ToLower();
            var fileExtension = Path.GetExtension(postedFile.FileName).ToLower();

            return allowedContentTypes.Contains(contentType) && allowedExtensions.Contains(fileExtension);
        }
        public static bool IsImage(this IFormFile postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (postedFile.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            ////-------------------------------------------
            ////  Try to instantiate new Bitmap, if .NET will throw exception
            ////  we can assume that it's not a valid image
            ////-------------------------------------------

            //try
            //{
            //    using (var bitmap = new Bitmap(postedFile.OpenReadStream()))
            //    {
            //    }
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
            //finally
            //{
            //    postedFile.OpenReadStream().Position = 0;
            //}

            return true;
        }

        public static bool IsWordFile(this IFormFile postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "application/msword" &&
                        postedFile.ContentType.ToLower() != "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                       )
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".doc"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".docx"
                )
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (postedFile.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }

        public static bool IsPdfFile(this IFormFile postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "application/pdf")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".pdf"

                )
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (postedFile.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }
    }
}
