using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Reports;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Media;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Flow.FormatProviders.Html;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;

public partial class Log_ElectricLogTankList : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    Guid AttachmentId = new Guid();
    bool isAttached = false;
    bool isChanged = true;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        ShowRevisionToolBar();

        if (IsPostBack == false)
        {
            ViewState["PAGENUMBER"] = 1;
            gvTankList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //gvTankList.Rebind();
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        ShowToolBar();
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        GetAttachmentId();
        if (IsDraft())
        {
            toolbarmain.AddFontAwesomeButton(Session["sitepath"] + "/Log/ElectricLogTankList.aspx", "Select Tank", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbarmain.AddLinkButton(Session["sitepath"] + "/Log/ElectricLogTankList.aspx", "Submit", "SUBMIT", ToolBarDirection.Right);
            toolbarmain.AddLinkButton(Session["sitepath"] + "/Log/ElectricLogTankList.aspx", "Save", "SAVE", ToolBarDirection.Right);
        } else
        {
            toolbarmain.AddLinkButton(Session["sitepath"] + "/Log/ElectricLogTankList.aspx", "Revise", "REVISE", ToolBarDirection.Right);
        }

        toolbarmain.AddFontAwesomeButton(Session["sitepath"] + "/Log/ElectricLogTankList.aspx", "Download pdf", "<img src='../css/Theme1/images/pdf.png' title='Download pdf'>", "PDF");
        //toolbarmain.AddFontAwesomeButton(Session["sitepath"] + "/Log/ElectricLogTankList.aspx", "Download excel", "<img src='../css/Theme1/images/icon_xls.png' title='Download excel'>", "EXCEL");

        if (isAttached)
        {
            toolbarmain.AddFontAwesomeButton("javascript: openNewWindow('MoreInfo', '', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + AttachmentId + "&MOD=LOG'); return false;", "Attachment", "<img src='../css/Theme1/images/attachment.png' title='Attachment'>", "ATTACHMENT");
        }
        else
        {
            toolbarmain.AddFontAwesomeButton("javascript: openNewWindow('MoreInfo', '', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + AttachmentId + "&MOD=LOG'); return false;", "Attachment", "<img src='../css/Theme1/images/no-attachment.png' title='Attachment'>", "ATTACHMENT");
        }

        toolbarmain.AddFontAwesomeButton("javascript: openNewWindow('TankRevision', '', '" + Session["sitepath"] + "/Log/ElectricLogRevision.aspx'); return false;", "Revision", "<img src='../css/Theme1/images/requisition.png' title='Revision'>", "REVISION");
        gvTabStrip.AccessRights = this.ViewState;
        gvTabStrip.MenuList = toolbarmain.Show();
    }

    private void ShowRevisionToolBar()
    {
        DataSet ds = PhoenixElog.GetCurrentRevisionNumber(usercode, vesselId);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (row["FLDREVISIONNO"] != DBNull.Value && row["FLDREVISIONDATE"] != DBNull.Value)
            {
                string template = string.Format("Active Revision  - {0} ({1})", row["FLDREVISIONNO"].ToString(), Convert.ToDateTime(row["FLDREVISIONDATE"]).ToString("dd-MM-yyyy"));
                gvRevisionTabStrip.Title = template;
            }
            gvRevisionTabStrip.AccessRights = this.ViewState;
            gvRevisionTabStrip.MenuList = toolbarmain.Show();
        }
    }


    public bool IsDraft()
    {
        bool isDraft = true;

        DataSet ds = PhoenixElog.LogLocationIsDraft(usercode, vesselId);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            isDraft = Convert.ToBoolean(row["FLDISDRAFT"]);
        }
        return isDraft;
    }

    public void GetAttachmentId()
    {
        DataSet ds = PhoenixElog.LogLocationGetAttachment(usercode, vesselId);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            if (row["FLDATTACHMENTID"] != DBNull.Value)
            {
                AttachmentId = (Guid)row["FLDATTACHMENTID"];
            }

            if (row["FLDATTACHMENTCODE"] != DBNull.Value)
            {
                isAttached = String.IsNullOrWhiteSpace(row["FLDATTACHMENTCODE"].ToString()) == true ? false : true;
            }

            if (row["FLDISCHANGED"] != DBNull.Value)
            {
                isChanged = Convert.ToInt32(row["FLDISCHANGED"]) == 1 ? true : false;
            }
        }
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //gvTankList.Rebind();
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

     #region Grid Events
    protected void gvTankList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int TotalPageCount = 0;
        DataSet ds = new DataSet();
        ds = GetGridData(ref TotalPageCount);
        gvTankList.DataSource = ds;
        gvTankList.VirtualItemCount = TotalPageCount;

    }

    private DataSet GetGridData(ref int TotalPageCount)
    {
        int iRowCount = 0;
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTankList.CurrentPageIndex + 1;
        return PhoenixElog.ListLocation(usercode, vesselId, (int)ViewState["PAGENUMBER"], 100, ref iRowCount, ref TotalPageCount);
    }

    protected void gvTankList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
            {
                LinkButton editBtn = (LinkButton)e.Item.FindControl("cmdEdit");
                LinkButton deleteBtn = (LinkButton)e.Item.FindControl("cmdDelete");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (editBtn != null) editBtn.Visible = SessionUtil.CanAccess(this.ViewState, editBtn.CommandName);
                
                if (drv["FLDACTIVEREVISIONID"] != DBNull.Value)
                {
                    editBtn.Visible = false;
                    deleteBtn.Visible = false;
                }
            }

            if (e.Item is GridGroupHeaderItem)
            {
                GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
                DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
                item.DataCell.Text = string.Format("<span class='tankType'>{0}</span>", groupDataRow["TankType"].ToString());
            }

            if (e.Item is GridEditableItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void Rebind()
    {
        gvTankList.SelectedIndexes.Clear();
        gvTankList.EditIndexes.Clear();
        gvTankList.DataSource = null;
        gvTankList.Rebind();
    }


    protected void gvTankList_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblLocation = (RadLabel)e.Item.FindControl("lblLocationId");

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Dictionary<string, bool> accessRight = GetAccessRights();
                if (accessRight["REVISE"] == false)
                {
                    throw new Exception("You don't have access rights");
                }

                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdDelete");
                Guid locationId = new Guid(lblLocation.Text);
                PhoenixElog.LocationDelete(usercode, locationId, vesselId);
                Rebind();
            }

            if (e.CommandName.ToUpper().Equals("TRANSCATION"))
            {
                Response.Redirect("../Log/ElectronicLogOperationReport.aspx");
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                Dictionary<string, bool> accessRight = GetAccessRights();
                if (accessRight["REVISE"] == false)
                {
                    throw new Exception("You don't have access rights");
                }

                LinkButton editBtn = (LinkButton)e.Item.FindControl("cmdEdit");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadLabel LocationId = (RadLabel)e.Item.FindControl("lblLocationId");
                if (LocationId != null && editBtn != null)
                {
                    String script = string.Format("javascript:openNewWindow('tankEdit','','{0}/LOG/ElectricLogTankEdit.aspx?LocationId={1}', 'false', '600', '400');",  Session["sitepath"], LocationId.Text );
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
                }

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    #endregion

    private Dictionary<string, bool> GetAccessRights()
    {
        // get it from db and form a dictionary
        Dictionary<string, bool> accessrights = new Dictionary<string, bool>();
        string rankShortCode = PhoenixElog.GetRankName(usercode);
        string designaionShortCode = null;
        bool isVessel = vesselId > 0 ? true : false;
        DataSet ds = PhoenixElog.GetMarpolLogAccessRights(usercode, vesselId, rankShortCode, designaionShortCode, isVessel);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            accessrights.Add("REVISE", Convert.ToBoolean(row["FLDTANKREVISE"]));
            accessrights.Add("ADD", Convert.ToBoolean(row["FLDTANKREVISE"]));
        }
        else
        {
            accessrights.Add("REVISE", false);
            accessrights.Add("ADD", false);
        }
        accessrights.Add("SAVE", true);
        accessrights.Add("SUBMIT", true);
        accessrights.Add("PDF", true);
        accessrights.Add("EXCEL", true);



        return accessrights;
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
	try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Dictionary<string, bool> accessrights = GetAccessRights();
            if (accessrights[CommandName.ToUpper()] == false)
            {
                string errorMessage = string.Format("You don't have access rights", CommandName.ToLower());
                throw new Exception(errorMessage);
            }

            if (CommandName.ToUpper().Equals("ADD"))
            {
                string script = string.Format("javascript:parent.openNewWindow('tankAdd','','{0}/Log/ElectricLogTankAdd.aspx', 'false', 600, 400);", Session["sitepath"]);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
            }

            if (CommandName.ToUpper().Equals("PDF"))
            {
                string Tmpfilelocation = string.Empty;
                string[] reportfile = new string[0];
                int TotalPageCount = 0;
                DataSet ds = new DataSet();
                ds = GetGridData(ref TotalPageCount);

                if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                {
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("applicationcode", "24");
                    nvc.Add("reportcode", "LOGTANKLIST");
                    nvc.Add("CRITERIA", "");
                    Session["PHOENIXREPORTPARAMETERS"] = nvc;

                    Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
                    string filename = string.Format("{0}.pdf", "TankList");
                    Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);
                    PhoenixReportsCommon.LoadLogo(ds);
                    PhoenixSsrsReportsCommon.getLogo();
                    PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
                    Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
                }
            }

            if (CommandName.ToUpper().Equals("SAVE")) // NOT NEEDED
            {
                GetAttachmentId();
                if (isChanged == false)
                {
                    ucError.ErrorMessage = "No Changes Done";
                    ucError.Visible = true;
                    return;
                }

            }

            if (CommandName.ToUpper().Equals("EXCEL")) 
            {
                //DownloadExcel();
                //gvTankList.ClientSettings.Scrolling.AllowScroll = false;
                //gvTankList.ClientSettings.Scrolling.UseStaticHeaders = false;
                //gvTankList.MasterTableView.ExportToExcel();
                int TotalPageCount = 0;
                DataSet ds = new DataSet();
                ds = GetGridData(ref TotalPageCount);
                DataTable dt = ds.Tables[0];
                ShowExcel("TankList", dt, getAlColumns(), getAlCaptions(), 0, null);

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "exportRadSpreadsheet();", true);
            }


            if (CommandName.ToUpper().Equals("SUBMIT"))
            {

                GetAttachmentId();
                if (isAttached == false && isChanged)
                {
                    ucError.ErrorMessage = "Attachment is mandatory before submit";
                    ucError.Visible = true;
                    return;
                }

                PhoenixElog.LogLocationAddRevision(usercode, vesselId);
                ucMessage.ErrorMessage = "Revision added successfully";
                ucMessage.Visible = true;
                Rebind();
            }

            if (CommandName.ToUpper().Equals("REVISE"))
            {
                PhoenixElog.LogLocationRevisie(usercode, vesselId);
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private string[] getAlColumns()
    {
        string[] alColumns = { "FLDNAME", "FLDIOPPNAME", "FLDTANKTYPE", "FLDFRAMESFROMTO", "FLDLATERALPOSITION", "FLDCAPACITY", "FLD85PERCENT" };
        return alColumns;
    }
    private string[] getAlCaptions()
    {
        string[] alCaptions = { "Tank Name", "Tank Identification (IOPP NAME)", "Tank Type", "Frames From - To", "Lateral Position", "100%", "85%"};
        return alCaptions;
    }

    public void ShowExcel(string strHeading, DataTable dt, string[] alColumns, string[] alCaptions, int? strSortDirection, string strSortExpression)
    {

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + (string.IsNullOrEmpty(strHeading) ? "Attachment" : strHeading.Replace(" ", "_")) + ".xls");
        HttpContext.Current.Response.ContentType = "application/vnd.msexcel";
        HttpContext.Current.Response.Write("<style>.text{ mso-number-format:\"\\@\";}</style>");
        HttpContext.Current.Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        HttpContext.Current.Response.Write("<tr>");
        HttpContext.Current.Response.Write("<td ><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        HttpContext.Current.Response.Write("<td colspan='" + (alColumns.Length - 1).ToString() + "'><h3>&nbsp&nbsp&nbsp&nbsp " + strHeading + "</h3></td>");
        HttpContext.Current.Response.Write("</tr>");
        HttpContext.Current.Response.Write("</TABLE>");
        HttpContext.Current.Response.Write("<br />");
        

        HttpContext.Current.Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        HttpContext.Current.Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            HttpContext.Current.Response.Write("<td width='20%'>");
            HttpContext.Current.Response.Write("<b>" + alCaptions[i] + "</b>");
            HttpContext.Current.Response.Write("</td>");
        }
        HttpContext.Current.Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            HttpContext.Current.Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                HttpContext.Current.Response.Write(dr[alColumns[i]].GetType().Equals(typeof(string)) ? "<td  class='text'>" : "<td>");
                HttpContext.Current.Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                HttpContext.Current.Response.Write("</td>");
            }
            HttpContext.Current.Response.Write("</tr>");
        }
        HttpContext.Current.Response.Write("</TABLE>");

        HttpContext.Current.Response.Write("<img src='" + HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/esmlogo4_small.png" + "   ' height='600px' style='height:600px;Width:200px;margin:0 auto;' />");
        HttpContext.Current.Response.Write("<br/>");
        HttpContext.Current.Response.Write("<br/>");
        HttpContext.Current.Response.End();
    }

    private void DownloadExcel(RadFlowDocument document)
    {
        Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider pdfProvider = new Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider();

        string filename = "TankList";

        TextWatermarkSettings settings = new TextWatermarkSettings()
        {
            Angle = -45,
            Width = 600,
            Height = 150,
            Opacity = 0.3,
            ForegroundColor = Colors.Red,
            Text = "Uncontrolled"
        };

        Watermark textWatermark = new Watermark(settings);
        Header header = document.Sections[0].Headers.Add();
        string newwater = textWatermark.TextSettings.Text;
        header.Watermarks.Add(textWatermark);
        //header.Blocks.AddParagraph().TextAlignment = Telerik.Windows.Documents.Flow.Model.Styles.Alignment.Right;
        //header.Blocks.AddParagraph().Inlines.AddRun(newwater);
        document.Sections.First().PageOrientation = PageOrientation.Portrait;
        document.Sections.First().PageMargins = new Telerik.Windows.Documents.Primitives.Padding(30, 5, 30, 5);
        document.Sections.First().PageSize = PaperTypeConverter.ToSize(PaperTypes.A4);
        byte[] pdfBytes = null;

        //XlsxFormatProvider provider = new XlsxFormatProvider();
        //provider.Export();


        pdfProvider.ExportSettings.ImageQuality = Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ImageQuality.High;
        pdfBytes = pdfProvider.Export(document);

        Response.Clear();
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + ".pdf");
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(pdfBytes);

        //gvTankList.MasterTableView.ExportToExcel();
    }

    protected void gvTankList_GridExporting(object sender, GridExportingArgs e)
    {
        //byte[] output = Encoding.GetEncoding("utf-8").GetBytes(e.ExportOutput);
        //byte[] output = Encoding.GetEncoding("utf-8").GetBytes(e.ExportOutput);
        //string utfString = Encoding.UTF8.GetString(output, 0, output.Length);
        //HtmlFormatProvider provider = new HtmlFormatProvider();
        //RadFlowDocument document = provider.Import(utfString);
        //DownloadExcel(document);
    }
}