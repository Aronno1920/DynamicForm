using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace CoreLibrary
{
    public class GenerateWatermark
    {
        Cookie _user = new Cookie();

        public void AsDraft(String sFileId)
        {
            try
            {
                String sQuery = "SELECT FilePath FROM dbo.DocumentData WHERE TransID=" + sFileId;
                DataManager dataManager = new DataManager();
                DataTable dtFileInfo = dataManager.GetDataTable(sQuery.ToString());

                if (File.Exists(dtFileInfo.Rows[0]["FilePath"].ToString()))
                {
                    FileInfo fileInfo = new FileInfo(dtFileInfo.Rows[0]["FilePath"].ToString());
                    if (fileInfo.Extension == ".pdf")
                    {
                        String FileLocation = dtFileInfo.Rows[0]["FilePath"].ToString();
                        string WatermarkLocation = ConfigurationManager.AppSettings["DraftImage"].ToString();

                        Document document = new Document();
                        PdfReader pdfReader = new PdfReader(FileLocation);
                        PdfStamper stamp = new PdfStamper(pdfReader, new FileStream(FileLocation.Replace(".pdf", "[temp][file].pdf"), FileMode.Create));

                        Image img = Image.GetInstance(WatermarkLocation);
                        var gstate = new PdfGState { FillOpacity = 0.3f, StrokeOpacity = 0.3f };

                        PdfContentByte waterMark;
                        for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                        {
                            Rectangle pageRectangle = pdfReader.GetPageSizeWithRotation(page);
                            float width = pageRectangle.Width;
                            float height = pageRectangle.Height;
                            img.ScaleToFit(width / 2, height / 2);    //scale image
                            img.SetAbsolutePosition(width / 2 - img.ScaledWidth / 2, height / 2 - img.ScaledHeight / 2);   //center image

                            waterMark = stamp.GetOverContent(page);
                            waterMark.SetGState(gstate);
                            waterMark.AddImage(img);
                        }
                        stamp.FormFlattening = true;
                        stamp.Close();

                        // now delete the original file and rename the temp file to the original file
                        File.Delete(FileLocation);
                        File.Move(FileLocation.Replace(".pdf", "[temp][file].pdf"), FileLocation);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTracking.SaveError(_user.EmployeeId, "GenerateWatermark", System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public void AsApprove(String sDocumentId)
        {
            try
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("SELECT ApproveFilePath,A.IsRequired FROM dbo.DocumentData D   ");
                sQuery.Append("LEFT JOIN SysLookup A ON LookupTypeId=7 AND D.DocumentTypeID=A.LookupValue   ");
                sQuery.Append("WHERE D.DocumentID=" + sDocumentId);

                DataManager dataManager = new DataManager();
                DataTable dtFileInfo = dataManager.GetDataTable(sQuery.ToString());

                foreach (DataRow drFile in dtFileInfo.Rows)
                {
                    if (Convert.ToBoolean(drFile["IsRequired"].ToString()) == true)
                    {
                        if (File.Exists(drFile["ApproveFilePath"].ToString()))
                        {
                            FileInfo fileInfo = new FileInfo(drFile["ApproveFilePath"].ToString());
                            if (fileInfo.Extension == ".pdf")
                            {
                                String FileLocation = drFile["ApproveFilePath"].ToString();
                                string WatermarkLocation = ConfigurationManager.AppSettings["ApproveImage"].ToString();

                                Document document = new Document();
                                PdfReader pdfReader = new PdfReader(FileLocation);
                                PdfStamper stamp = new PdfStamper(pdfReader, new FileStream(FileLocation.Replace(".pdf", "[temp][file].pdf"), FileMode.Create));

                                Image img = Image.GetInstance(WatermarkLocation);
                                var gstate = new PdfGState { FillOpacity = 0.3f, StrokeOpacity = 0.3f };

                                PdfContentByte waterMark;
                                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                                {
                                    Rectangle pageRectangle = pdfReader.GetPageSizeWithRotation(page);
                                    float width = pageRectangle.Width;
                                    float height = pageRectangle.Height;
                                    img.ScaleToFit(width / 2, height / 2);    //scale image
                                    img.SetAbsolutePosition(width / 2 - img.ScaledWidth / 2, height / 2 - img.ScaledHeight / 2);   //center image

                                    waterMark = stamp.GetOverContent(page);
                                    waterMark.SetGState(gstate);
                                    waterMark.AddImage(img);
                                }
                                stamp.FormFlattening = true;
                                stamp.Close();

                                // now delete the original file and rename the temp file to the original file
                                File.Delete(FileLocation);
                                File.Move(FileLocation.Replace(".pdf", "[temp][file].pdf"), FileLocation);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTracking.SaveError(_user.EmployeeId, "GenerateWatermark", System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
    }
}
