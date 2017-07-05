using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;

namespace Portal.Web.Models
{
    public interface IPdfMergeData
    {
        IDictionary<string, string> MergeFieldValues { get; }
    }

    public class PdfMergeStreamer
    {
        public void fillPDF(string templatePath,
            System.IO.MemoryStream outputStream, string username, string passedDate)
        {

            // Agggregate successive pages here:
            var pagesAll = new List<byte[]>();

            // Hold individual pages Here:
            byte[] pageBytes = null;



            // Read the form template for each item to be output:
            var templateReader = new iTextSharp.text.pdf.PdfReader(templatePath);

            using (var tempStream = new System.IO.MemoryStream())
            {
                PdfStamper stamper = new PdfStamper(templateReader, tempStream);
                stamper.FormFlattening = true;
                AcroFields fields = stamper.AcroFields;
                stamper.Writer.CloseStream = false;


                fields.SetField("Brukernavn", username);
                fields.SetField("Dato", "Dato: " + passedDate);

                // If we had not set the CloseStream property to false, 
                // this line would also kill our memory stream:
                stamper.Close();

                // Reset the stream position to the beginning before reading:
                tempStream.Position = 0;

                // Grab the byte array from the temp stream . . .
                pageBytes = tempStream.ToArray();

                // And add it to our array of all the pages:
                pagesAll.Add(pageBytes);
            }


            // Create a document container to assemble our pieces in:
            Document mainDocument = new Document(PageSize.A4);
            // Copy the contents of our document to our output stream:
            var pdfCopier = new PdfSmartCopy(mainDocument, outputStream);

            // Once again, don't close the stream when we close the document:
            pdfCopier.CloseStream = false;

            mainDocument.Open();
            foreach (var pageByteArray in pagesAll)
            {
                // Copy each page into the document:
                mainDocument.NewPage();
                pdfCopier.AddPage(pdfCopier.GetImportedPage(new PdfReader(pageByteArray), 1));
            }
            pdfCopier.Close();

            // Set stream position to the beginning before returning:
            outputStream.Position = 0;
        }
    }

}
