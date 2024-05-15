using Cafe_management_system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Web;
using System.IO;
using System.Net.Http.Headers;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cafe_management_system.Controllers
{

    [RoutePrefix("api/product")]
    public class BillController : ApiController
    {
        CafeEntities db = new CafeEntities();
        Response response = new Response();
        private string pdfPath = "E:\\";


        [HttpPost, Route("generateReoport")]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GenerateReport([FromBody] Bill bill)
        {

            try
            {
                var token = Request.Headers.GetValues("authorization").First();
                TokenClaim tokenClaim = TokenManager.ValidateToken(token);

                var ticks = DateTime.Now.Ticks;
                var guid = Guid.NewGuid().ToString();
                var uniqueId = ticks.ToString() + '-' + guid;
                bill.createdBy = tokenClaim.Email;
                bill.uuid = uniqueId;
                db.Bills.Add(bill);
                db.SaveChanges();
                Get(bill);

            

            }


            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

        private void Get(Bill bill)
        {
            try
            {
                dynamic productdetails = JsonConvert.DeserializeObject(bill.productDetails);
                var todayDate = "Date: " + Convert.ToDateTime(DateTime.Today).ToString("MM/dd/yyyy");
                PdfWriter writer = new PdfWriter(pdfPath + bill.uuid + ".pdf");
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);


                Paragraph header = new Paragraph("Management System")
                    .SetBold()
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(25);
                document.Add(header);


                Paragraph newline = new Paragraph(new Text("\n"));



                LineSeparator ls = new LineSeparator(new SolidLine());
                document.Add(ls);



                Paragraph customerDetails = new Paragraph("Name: " + bill.name + "\nEmail: " + bill.email + "\nContact Number: " + bill.contactnumber+"\nPayment Method: "+bill.paymentMethod);

                document.Add(customerDetails);


                Table table = new Table(5, false);
                table.SetWidth(new UnitValue(UnitValue.PERCENT, 100));



                Cell headerName = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold()
                    .Add(new Paragraph ("Name"));


                Cell headerName = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetBold()
                   .Add(new Paragraph("Name"));


                Cell headerCategory = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetBold()
                   .Add(new Paragraph("Category"));

                Cell headerPrice = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetBold()
                   .Add(new Paragraph("Price"));

                table.AddCell(headerName);
                table.AddCell(headerName);
                table.AddCell(headerName);
                table.AddCell(headerName);
                table.AddCell(headerName);
                table.AddCell(headerName);








            }




            catch (Exception ex)
            {

            }

        }
    }
}