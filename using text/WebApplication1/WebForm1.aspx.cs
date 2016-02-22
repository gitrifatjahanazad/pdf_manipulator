using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string WatermarkLocation = @"C:\users\Rifat\Desktop\work\nar.png";
            string FileLocation = @"C:\users\Rifat\Desktop\task\x.pdf";
            Document document = new Document();
            PdfReader pdfReader = new PdfReader(FileLocation);
            PdfStamper stamp = new PdfStamper(pdfReader, new FileStream(FileLocation.Replace(".pdf", "[temp][file].pdf"), FileMode.Create));

            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(WatermarkLocation);
            img.SetAbsolutePosition(125, 300); // set the position in the document where you want the watermark to appear (0,0 = bottom left corner of the page)

            // Create New Layer for Watermark
            PdfLayer layer = new PdfLayer("WatermarkLayer", stamp.Writer);

            string waterMark="test";
            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                PdfContentByte cb = stamp.GetUnderContent(page);
                // Getting the Page Size
                Rectangle rect = pdfReader.GetPageSize(page);
                // Tell the cb that the next commands should be "bound" to this new layer
                cb.BeginLayer(layer);
                cb.SetFontAndSize(BaseFont.CreateFont(
                  BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 50);

                PdfGState gState = new PdfGState();
                gState.FillOpacity = 0.25f;
                cb.SetGState(gState);

                //cb.SetColorFill(BaseColor.BLACK);
                cb.BeginText();
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, waterMark, rect.Width / 2, rect.Height / 2, 45f);
                cb.EndText();

                // Close the layer
                cb.EndLayer();
            }
            stamp.FormFlattening = true;
            stamp.Close();

            // now delete the original file and rename the temp file to the original file
            File.Delete(FileLocation);
            File.Move(FileLocation.Replace(".pdf", "[temp][file].pdf"), FileLocation);
        }


    }
}