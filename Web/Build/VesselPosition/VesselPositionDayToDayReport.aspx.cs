using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.Collections.Specialized;
using System.Web;
using System.Text;
using System.IO;
using OfficeOpenXml;
using Telerik.Web.UI;

public partial class VesselPositionDayToDayReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionDayToDayReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDayToDayReport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionDayToDayReport.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionDayToDayReport.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionDayToDayReport.aspx", "Generate Report", "<i class=\"fas fa-copy\"></i>", "GENERATE");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionDayToDayReport.aspx", "Send Mail", "<i class=\"fas fa-envelope\"></i>", "SENDMAIL");

        MenuDayToDayReportTab.AccessRights = this.ViewState;
        MenuDayToDayReportTab.MenuList = toolbar.Show();
        //MenuDayToDayReportTab.SetTrigger(pnlDayToDayReport);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        if (!IsPostBack)
        {
            try
            {
                //if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                //{
                //    ddlVessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                //    ddlVessel.Enabled = false;
                //}
                ddlVessel.bind();
                ddlVessel.DataBind();
                bindyear();
                BindMPAVEBPVessel();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
            gvDayToDayReport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();

            string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDIMONUMBER", "FLDVESSELTYPE", "FLDREGISTEREDGT", "FLDREGISTEREDNT", "FLDDWTSUMMER", "FLDEEDI", "FLDICECLASS","FLDMEPOWEROUTPUT","FLDAEPOWEROUTPUT",
            "FLDDISTANCETRAVELLED","FLDHOURSUNDERWAY","FLDMDOMGOCONS","FLDLFOCONS","FLDHFOCONS","FLDLPGPROPANE","FLDLPGBUTANE","FLDLNG","FLDMETHANOL","FLDETHANOL","FLDOTHERS","FLDFUELCONSMEASUREMETHOD","FLDREMARKS"};
            string[] alCaptions = { "Vessel", "Start Date", "End Date", "IMO number", "Ship type", "Gross tonnage (GT)", "Net tonnage (NT)", "Deadweight tonnage (DWT)", "EEDI(gCO2/t.nm)", "Ice class", "Main propulsion power",
            "Auxiliary engine(s)", "Distance travelled (nm)", "Hours underway (h)", "Diesel/Gas Oil", "LFO", "HFO", "LPG (Propane)", "LPG (Butane)", "LNG", "Methanol", "Ethanol", "Other", "Method of measure cons", "Remarks" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixVesselPositionDayToDayReport.DayToDayReportSearch(General.GetNullableInteger(ddlVessel.SelectedVessel),
               sortexpression, sortdirection,
                int.Parse(ViewState["PAGENUMBER"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount
                , General.GetNullableInteger(ddlYear.SelectedValue.ToString())
                , General.GetNullableInteger(ddlMonth.SelectedValue.ToString()));

            Response.AddHeader("Content-Disposition", "attachment; filename=MPAVEBP.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>MPA VEBP</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public string GenerateXL()
    {
        string monthName = new DateTime(int.Parse(ddlYear.SelectedValue), int.Parse(ddlMonth.SelectedValue), 1).ToString("MMM", System.Globalization.CultureInfo.InvariantCulture);
        string path = Server.MapPath("~/Attachments/TEMP/" + ddlVessel.SelectedVesselName + "_" + monthName + "_" + ddlYear.Text + "_MPAVEBP.xlsx");
        if (File.Exists(path))
            File.Delete(path);

        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet dts = new DataSet();

        string[] alColumns = {  "FLDFROMDATE", "FLDTODATE", "FLDIMONUMBER", "FLDVESSELTYPE", "FLDREGISTEREDGT", "FLDREGISTEREDNT", "FLDDWTSUMMER", "FLDEEDI", "FLDICECLASS","FLDMEPOWEROUTPUT","FLDAEPOWEROUTPUT",
            "FLDDISTANCETRAVELLED","FLDHOURSUNDERWAY","FLDMDOMGOCONS","FLDLFOCONS","FLDHFOCONS","FLDLPGPROPANE","FLDLPGBUTANE","FLDLNG","FLDMETHANOL","FLDETHANOL","FLDOTHERS","FLDFUELCONSMEASUREMETHOD","FLDREMARKS"};
        string[] alCaptions = { "Start Date", "End Date", "IMO number", "Ship type", "Gross tonnage (GT)", "Net tonnage (NT)", "Deadweight tonnage (DWT)", "EEDI(gCO2/t.nm)", "Ice class", "Main propulsion power",
            "Auxiliary engine(s)", "Distance travelled (nm)", "Hours underway (h)", "Diesel/Gas Oil", "LFO", "HFO", "LPG (Propane)", "LPG (Butane)", "LNG", "Methanol", "Ethanol", "Other", "Method of measure cons", "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        dts = PhoenixVesselPositionDayToDayReport.DayToDayReportSearch(General.GetNullableInteger(ddlVessel.SelectedVessel),
           sortexpression, sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount
            , General.GetNullableInteger(ddlYear.SelectedValue.ToString())
            , General.GetNullableInteger(ddlMonth.SelectedValue.ToString()));

        FileInfo fiTemplate = new FileInfo(path);
        using (ExcelPackage pck = new ExcelPackage(fiTemplate))
        {
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("MPA VEBP");
            ws.DefaultRowHeight = 18;
            int nRow = 22;
            int nCol = 2;
            //Data column headings
            for (int i = 0; i < alCaptions.Length; i++)
            {
                ws.Cells[nRow, (nCol + i)].Value = alCaptions[i];
                ws.Cells[nRow, (nCol + i)].Style.Font.Bold = true;
                ws.Cells[nRow, (nCol + i)].AutoFitColumns();
            }
            // Data rows
            for (int i = 0; i < dts.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < alColumns.Length; j++)
                {
                    if (dts.Tables[0].Columns[alColumns[j]].ColumnName.ToUpper().Equals("START DATE") || dts.Tables[0].Columns[alColumns[j]].ColumnName.ToUpper().Equals("END DATE"))
                        ws.Cells[nRow + (i + 1), (nCol + j)].Style.Numberformat.Format = "dd-MMM-yyyy";
                    ws.Cells[nRow + (i + 1), (nCol + j)].Value = dts.Tables[0].Rows[i][alColumns[j]];
                    ws.Cells[nRow + (i + 1), (nCol + j)].AutoFitColumns();

                }
            }

            pck.SaveAs(fiTemplate);
        }

        return path;
    }
     private void bindyear()
    {
        try
        {
            for (int i = 2005; i <= DateTime.Now.Year; i++)
            {
                ddlYear.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
            }
            ddlYear.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
            ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
        }catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindMPAVEBPVessel()
    {
        try
        {
            //DataSet ds = PhoenixVesselPositionDayToDayReport.ListMPAVEBPVsessel();
            //ddlVessel.DataSource=ds.Tables[0];
            //ddlVessel.DataValueField = "FLDVESSELID";
            //ddlVessel.DataTextField = "FLDVESSELNAME";
            //ddlVessel.DataBind();
            //ddlVessel.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //        if (ds.Tables[0].Rows[i]["FLDVESSELID"].ToString() == PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                //        {
                            ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                //        }
                //}
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void DayToDayReportTab_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Rebind();
            }

            if (CommandName.ToUpper().Equals("RESET"))
            {
                ClearFilter();
            }
            if (CommandName.ToUpper().Equals("GENERATE"))
            {
                GenerateReport();
                Rebind();

            }
            if (CommandName.ToUpper().Equals("PRINT"))
            {
                Rebind();
            }
            if (CommandName.ToUpper().Equals("SENDMAIL"))
            {
               if(General.GetNullableInteger(ddlVessel.SelectedVessel)!=null && General.GetNullableInteger(ddlYear.SelectedValue) != null && General.GetNullableInteger(ddlMonth.SelectedValue) != null)
                {
                    DataSet ds = PhoenixVesselPositionEUMRVSummaryReport.EUMRVVerifierDetail(General.GetNullableInteger(ddlVessel.SelectedVessel.ToString()) == null ? 0 : int.Parse(ddlVessel.SelectedVessel));

                    if (ds.Tables[0].Rows.Count > 0 && General.GetNullableString(ds.Tables[0].Rows[0]["FLDMPATOMAIL"].ToString())!=null)
                    {
                        DataTable dt = ds.Tables[0];
                        string tomail = dt.Rows[0]["FLDMPATOMAIL"].ToString();
                        string ccmail = dt.Rows[0]["FLDMPACCMAIL"].ToString();
                        //string dataformat = dt.Rows[0]["FLDDATAFORMAT"].ToString();
                        string subject = dt.Rows[0]["FLDIMONUMBER"].ToString()+"_"+dt.Rows[0]["FLDVESSELNAME"].ToString();
                        string monthName = new DateTime(int.Parse(ddlYear.SelectedValue), int.Parse(ddlMonth.SelectedValue), 1).ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture);

                        StringBuilder emailbody = new StringBuilder();
                        emailbody.AppendLine("Dear Sir,");
                        emailbody.AppendLine();
                        emailbody.AppendLine("Attached please find data as required by the MPA VEBP for the month of " + monthName);
                        //emailbody.AppendLine();
                        //emailbody.AppendLine("For any clarification, pls contact Sender");
                        emailbody.AppendLine();
                        //emailbody.AppendLine("Mr Yongsu Kim");
                        //emailbody.AppendLine("email: kimsu@executiveship.com");
                        //emailbody.AppendLine("Tel  :  (65)  6324 0500    EX-146");
                        emailbody.AppendLine("Thank you");

                        string Filepath = "";
                        string[] strarrfilenames = new string[1];

                        Filepath = GenerateXL();

                        strarrfilenames[0] = Filepath;

                        PhoenixMail.SendMail(tomail.ToString().Replace(";", ",").Replace(",,", ",").TrimEnd(','),
                        ccmail.Replace(";", ",").Replace(",,", ",").TrimEnd(','),
                        "",
                        subject,
                        emailbody.ToString(), false,
                        System.Net.Mail.MailPriority.Normal,
                        "",
                        strarrfilenames,
                        null);

                        ucStatus.Text = "Mail Sent.";
                    }
                    else
                    {
                        ucError.ErrorMessage = "Mail Id Not Configured.";
                        ucError.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearFilter()
    {
        ViewState["PAGENUMBER"] = 1;
        ddlVessel.SelectedVessel = "";
        ddlMonth.SelectedValue = "";
        ddlYear.SelectedValue = "";
        Rebind();
    }

    protected void ddlVessel_TextChangedEvent(object sender,EventArgs e)
    {
        Rebind();
    }
    protected void ddlMonth_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void ddlYear_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
            Rebind();
    }

    private void GenerateReport()
    {
        try
        {
            PhoenixVesselPositionDayToDayReport.InsertDayToDayReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlYear.SelectedValue.ToString()), General.GetNullableInteger(ddlMonth.SelectedValue.ToString()));
            ucStatus.Text = "Report Generated Successfully.";
        }catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDayToDayReport_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDayToDayReport_RowUpdating(object sender,GridCommandEventArgs e)
    {
        try
        {
            PhoenixVesselPositionDayToDayReport.UpdateRemarkDayToDayReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            ,General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblReportID")).Text)
                                                                            ,General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRemarks")).Text)
                                                                            );
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDayToDayReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDayToDayReport.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDIMONUMBER", "FLDVESSELTYPE", "FLDREGISTEREDGT", "FLDREGISTEREDNT", "FLDDWTSUMMER", "FLDEEDI", "FLDICECLASS","FLDMEPOWEROUTPUT","FLDAEPOWEROUTPUT",
            "FLDDISTANCETRAVELLED","FLDHOURSUNDERWAY","FLDMDOMGOCONS","FLDLFOCONS","FLDHFOCONS","FLDLPGPROPANE","FLDLPGBUTANE","FLDLNG","FLDMETHANOL","FLDETHANOL","FLDOTHERS","FLDFUELCONSMEASUREMETHOD","FLDREMARKS"};
        string[] alCaptions = { "Vessel", "Start Date", "End Date", "IMO number", "Ship type", "Gross tonnage (GT)", "Net tonnage (NT)", "Deadweight tonnage (DWT)", "EEDI(gCO2/t.nm)", "Ice class", "Main propulsion power",
            "Auxiliary engine(s)", "Distance travelled (nm)", "Hours underway (h)", "Diesel/Gas Oil", "LFO", "HFO", "LPG (Propane)", "LPG (Butane)", "LNG", "Methanol", "Ethanol", "Other", "Method of measure cons", "Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselPositionDayToDayReport.DayToDayReportSearch(General.GetNullableInteger(ddlVessel.SelectedVessel),
            sortexpression, sortdirection,
             int.Parse(ViewState["PAGENUMBER"].ToString()),
             gvDayToDayReport.PageSize,
             ref iRowCount,
             ref iTotalPageCount
             , General.GetNullableInteger(ddlYear.SelectedValue.ToString())
             , General.GetNullableInteger(ddlMonth.SelectedValue.ToString()));

        General.SetPrintOptions("gvDayToDayReport", "MPA VEBP", alCaptions, alColumns, ds);

        gvDayToDayReport.DataSource = ds;
        gvDayToDayReport.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvDayToDayReport.SelectedIndexes.Clear();
        gvDayToDayReport.EditIndexes.Clear();
        gvDayToDayReport.DataSource = null;
        gvDayToDayReport.Rebind();
    }
}
