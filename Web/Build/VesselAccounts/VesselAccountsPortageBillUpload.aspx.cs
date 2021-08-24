using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;

public partial class VesselAccountsPortageBillUpload :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPortageBillUpload.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvPBUpload')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPortageBillUpload.aspx", "Find", "search.png", "FIND");
            MenuExcelUploadItem.AccessRights = this.ViewState;
            MenuExcelUploadItem.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {               
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = 1;
      
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void MenuExcelUploadItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvPBUpload.EditIndex = -1;
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDVESSELNAME", "FLDMONTH", "FLDYEAR", "FLDCREATEDDATE", "FLDTYPENAME" };
                string[] alCaptions = { "Vessel Name", "Month", "Year", "Created Date", "Tyype" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataTable dt = PhoenixVesselAccountsPortageBill.SearchPortageBillUpload(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , null
                , null
                , null
                , sortexpression, sortdirection
                ,1, iRowCount,
               ref iRowCount,
               ref iTotalPageCount);
                General.ShowExcel("Portage Bill Upload", dt, alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            string[] alColumns = { "FLDVESSELNAME", "FLDMONTH", "FLDYEAR", "FLDCREATEDDATE", "FLDTYPENAME" };
            string[] alCaptions = { "Vessel Name", "Month", "Year", "Created Date", "Tyype"};
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixVesselAccountsPortageBill.SearchPortageBillUpload(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , null
                , null
                , null
                , sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null),
               ref iRowCount,
               ref iTotalPageCount);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvPBUpload", "Portage Bill Upload", alCaptions, alColumns, ds);
            if (dt.Rows.Count > 0)
            {
                gvPBUpload.DataSource = dt;
                gvPBUpload.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvPBUpload);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPBUpload_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                         + PhoenixModule.VESSELACCOUNTS + "'); return false;");
            }
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            string extension = Path.GetExtension(FileUpload.FileName.ToString());

            if (extension.ToUpper() == ".XLSX")
            {
                using (ExcelPackage pck = new ExcelPackage(FileUpload.PostedFile.InputStream))
                {
                    if (pck.Workbook.Worksheets.Count > 0)
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets[1];
                        string worksheetname = ws.Name.Replace(" ", "");
                        if (worksheetname.ToUpper() == "MONTHLY" || worksheetname.ToUpper() == "SIGNOFF")
                        {

                            string vesselname = ws.Cells["M1"].Value.ToString().Replace(" ", "");
                            string month = ws.Cells["M2"].Value.ToString().Replace(" ", "");
                            string year = ws.Cells["N2"].Value.ToString();

                            ExcelRange rng = ws.SelectedRange["A1:K1"];
                            if (!ValidateExcelHeaders(rng))
                            {
                                ucError.Visible = true;
                                return;
                            }

                            DateTime result;
                            if (DateTime.TryParse("01/" + month + "/" + year, out result))
                            {
                                string csvFileNo = string.Empty;
                                string csvTo = string.Empty;
                                int vesselid = 0;
                                PhoenixVesselAccountsPortageBill.CheckPortageBillUpload(vesselname, result, ref vesselid);
                                ViewState["VESSELID"] = vesselid;
                                ViewState["FILENAME"] = FileUpload.PostedFile.FileName;
                                int start = ws.Dimension.Start.Row;
                                int end = ws.Dimension.End.Row;
                                for (int i = start + 1; i <= end; i++)
                                {
                                    if (ws.Cells[i, 1].Value == null) continue;
                                    csvFileNo += ws.Cells[i, 1].Value.ToString() + ",";
                                    csvTo += ws.Cells[i, 4].Value.ToString() + ",";
                                }
                                if (csvFileNo.IndexOf(',') > -1)
                                    csvFileNo = csvFileNo.TrimEnd(',');
                                if (csvTo.IndexOf(',') > -1)
                                    csvTo = csvTo.TrimEnd(',');
                                DataSet ds = PhoenixVesselAccountsPortageBill.CheckCrewPortageBillUpload(vesselid, csvFileNo, csvTo, month, year);
                                DataTable dt1 = ds.Tables[1];
                                if (dt1.Rows.Count > 0 && worksheetname.ToUpper() == "MONTHLY")
                                {                                                                        
                                    for (int i = 0; i < dt1.Rows.Count; i++)
                                    {
                                        ucError.ErrorMessage = "TO Date for " + dt1.Rows[i][0].ToString() + " is not for the last day of the month";
                                    }
                                    ucError.Visible = true;
                                    return;
                                }
                                DataTable dt3 = ds.Tables[3];
                                if (dt3.Rows.Count > 0)
                                {
                                    ucConfirm.Visible = true;
                                    ucConfirm.HeaderMessage = "These Crew are not in the file, would you like to proceed?<br/>" +
                                    "Please ensure that all off-signers are signed off in Phoenix before finalizing Phoenix Portage Bill";
                                    for (int i = 0; i < dt3.Rows.Count; i++)
                                    {
                                        ucConfirm.ErrorMessage = dt3.Rows[i][0].ToString();
                                    }
                                }
                                DataTable dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    ucConfirm.Visible = true;                                   
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        ucConfirm.ErrorMessage = "Unable to find " + dt.Rows[i][0].ToString() + " in the Vessel Crew List. Please sign on the crew.";
                                    }
                                }
                                DataTable dt2 = ds.Tables[2];
                                if (dt2.Rows.Count > 0 && worksheetname.ToUpper() == "MONTHLY")
                                {
                                    ucConfirm.Visible = true;
                                    ucConfirm.ErrorMessage = "Monthly Data for the same vessel and month has been uploaded earlier. Old data will be overwritten.";                            
                                }

                                string path = Path.GetTempFileName();
                                ViewState["PATH"] = path;
                                FileUpload.PostedFile.SaveAs(path);
                                if (dt.Rows.Count > 0 || dt1.Rows.Count > 0 || dt3.Rows.Count > 0 || (dt2.Rows.Count > 0 && worksheetname.ToUpper() == "MONTHLY"))
                                {
                                    return;
                                }
                                ProcessPortageBillExcel(path);
                            }
                            else
                            {
                                ucError.ErrorMessage = "Invalid Month & Year. Check the Given Month & Year on L2 & M2 in upload Excel.";
                                ucError.Visible = true;
                            }
                        }
                        else
                        {
                            ucError.ErrorMessage = "Invalid worksheet name. Worksheet name should be either MONTHLY or SIGNOFF.";
                            ucError.Visible = true;
                        }
                    }
                    else
                    {
                        ucError.ErrorMessage = "Worksheet not found. Worksheet name should be either MONTHLY or SIGNOFF.";
                        ucError.Visible = true;
                    }
                }
            }
            else
            {
                ucError.ErrorMessage = "kindly upload .xlsx file only";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private bool ValidateExcelHeaders(ExcelRange range)
    {
        ExcelWorksheet ws = range.Worksheet;
        string[] headers = { "Employee Code", "Name", "Rank", "To", "TANK CLEANING", "BOND SUBSIDY", "CASH ON BD", "ALLOTMENT", "BONDED STORES", "RADIO","EXTRA OT" };
        for (int i = range.Start.Row; i <= range.End.Row; i++)
        {
            for (int j = range.Start.Column; j <= range.End.Column; j++)
            {
                if (headers[j - 1].Replace(" ", "").ToUpper() != ws.Cells[i, j].Value.ToString().Replace(" ", "").ToUpper())
                    ucError.ErrorMessage = "Couldn't find header " + headers[j - 1] + " in " + ws.Cells[i, j].Address + " Column";
            }
        }        
        return (!ucError.IsError);
    }
    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {           
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;          
            string path = ViewState["PATH"].ToString();
            if (ucCM.confirmboxvalue == 1)
            {

                ProcessPortageBillExcel(path);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message.ToString();
            ucError.Visible = true;
        }
    }
    private void ProcessPortageBillExcel(string path)
    {
        try
        {
            FileInfo fi = new FileInfo(path);
            using (ExcelPackage pck = new ExcelPackage(fi))
            {
                StringBuilder pbxml = new StringBuilder();
                XmlWriterSettings setting = new XmlWriterSettings();
                setting.OmitXmlDeclaration = true;
                setting.Encoding = Encoding.UTF8;

                XmlWriter writer = XmlWriter.Create(pbxml, setting);
                writer.WriteStartElement("pb");

                ExcelWorksheet ws = pck.Workbook.Worksheets[1];
                int start = ws.Dimension.Start.Row;
                int end = ws.Dimension.End.Row;
                for (int i = start + 1; i <= end; i++)
                {
                    if (ws.Cells[i, 1].Value == null) continue;
                    createNode(ws.Cells[i, 1].Value.ToString(), ws.Cells[i, 5].Value.ToString(), ws.Cells[i, 6].Value.ToString(), ws.Cells[i, 7].Value.ToString()
                                , ws.Cells[i, 8].Value.ToString(), ws.Cells[i, 9].Value.ToString(), ws.Cells[i, 10].Value.ToString(), ws.Cells[i, 11].Value.ToString(), ws.Cells[i, 4].Value.ToString(), writer);
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
                string month = ws.Cells["M2"].Value.ToString().Replace(" ", "");
                string year = ws.Cells["N2"].Value.ToString();
                DateTime date = DateTime.Parse("01/" + month + "/" + year);
                int type = 2; //signoff
                if (ws.Name.Replace(" ", "") == "MONTHLY")
                {
                    type = 1;
                }
                DataTable dt = PhoenixVesselAccountsPortageBill.UploadPortageBill(ViewState["FILENAME"].ToString(), int.Parse(ViewState["VESSELID"].ToString()), date.Month, date.Year, type, fi.Length, pbxml.ToString());
                if (dt.Rows.Count > 0)
                {
                    Guid uploaid = new Guid(dt.Rows[0]["FLDUPLOADID"].ToString());
                    PhoenixVesselAccountsPortageBill.UploadPortageBillItem(int.Parse(ViewState["VESSELID"].ToString()), date.Month, date.Year, pbxml.ToString(), uploaid);
                    File.Copy(path, Server.MapPath("~/attachments/VesselAccounts/" + dt.Rows[0]["FLDDTKEY"].ToString() + ".xlsx"), true);
                }                
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message.ToString();
            ucError.Visible = true;
        }
    }
    private void createNode(string fileno, string tankcleaning, string bondsubsidy, string cashonboard, string allotment, string bondedstores, string radiolog,string ExtraOT,string day, XmlWriter writer)
    {
        writer.WriteStartElement("ed");
        writer.WriteAttributeString("fileno", fileno);
        writer.WriteAttributeString("amount", tankcleaning);
        writer.WriteAttributeString("type", "1");
        writer.WriteAttributeString("earded", "1");
        writer.WriteAttributeString("day", day);
        writer.WriteEndElement();

        writer.WriteStartElement("ed");
        writer.WriteAttributeString("fileno", fileno);
        writer.WriteAttributeString("amount", bondsubsidy);
        writer.WriteAttributeString("type", "2");
        writer.WriteAttributeString("earded", "1");
        writer.WriteAttributeString("day", day);
        writer.WriteEndElement();

        writer.WriteStartElement("ed");
        writer.WriteAttributeString("fileno", fileno);
        writer.WriteAttributeString("amount", cashonboard);
        writer.WriteAttributeString("type", "3");
        writer.WriteAttributeString("earded", "-1");
        writer.WriteAttributeString("day", day);
        writer.WriteEndElement();

        writer.WriteStartElement("ed");
        writer.WriteAttributeString("fileno", fileno);
        writer.WriteAttributeString("amount", allotment);
        writer.WriteAttributeString("type", "4");
        writer.WriteAttributeString("earded", "-1");
        writer.WriteAttributeString("day", day);
        writer.WriteEndElement();

        writer.WriteStartElement("ed");
        writer.WriteAttributeString("fileno", fileno);
        writer.WriteAttributeString("amount", bondedstores);
        writer.WriteAttributeString("type", "5");
        writer.WriteAttributeString("earded", "-1");
        writer.WriteAttributeString("day", day);
        writer.WriteEndElement();

        writer.WriteStartElement("ed");
        writer.WriteAttributeString("fileno", fileno);
        writer.WriteAttributeString("amount", radiolog);
        writer.WriteAttributeString("type", "6");
        writer.WriteAttributeString("earded", "-1");
        writer.WriteAttributeString("day", day);
        writer.WriteEndElement();

        writer.WriteStartElement("ed");
        writer.WriteAttributeString("fileno", fileno);
        writer.WriteAttributeString("amount", ExtraOT);
        writer.WriteAttributeString("type", "7");
        writer.WriteAttributeString("earded", "1");
        writer.WriteAttributeString("day", day);
        writer.WriteEndElement();

    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvPBUpload.SelectedIndex = -1;
        gvPBUpload.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }  

}
