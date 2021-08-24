using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.Text;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Web;
using System.Xml;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Reports;
using Telerik.Web.UI;

public partial class VesselPositionChinaEmissionReport : PhoenixBasePage
{
    DataSet ds;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionChinaEmissionReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvConsumption')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionChinaEmissionReport.aspx", "Search", "<i class=\"fas fa-search\"></i>", "Search");
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionChinaEmissionReport.aspx", "Clear-Filter", "<i class=\"fas fa-eraser\"></i>", "Clear");
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionChinaEmissionReport.aspx", "Summary Report", "<i class=\"fas fa-file-pdf\"></i>", "PDFAll");

            MenuCrewCourseList.AccessRights = this.ViewState;
            MenuCrewCourseList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    UcVessel.Enabled = true;
                else
                    UcVessel.Enabled = false;

                txtCommencedTo.Text = DateTime.Now.ToShortDateString();
                txtCompletedTo.Text = DateTime.Now.ToShortDateString();

                if (Filter.CurrentChinaVoyageFilter == null)
                {
                    SetFilter();
                }
                else
                {
                    NameValueCollection nvc = Filter.CurrentChinaVoyageFilter;

                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? ((nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucPortMulti.SelectedValue = (nvc.Get("ddlPort") == null) ? "" : nvc.Get("ddlPort").ToString();
                    ucPortMulti.Text = (nvc.Get("ddlPortName") == null) ? "" : nvc.Get("ddlPortName").ToString();
                    txtCommencedFrom.Text = (nvc.Get("txtCommenceFrom") == null) ? "" : nvc.Get("txtCommenceFrom").ToString();
                    txtCommencedTo.Text = (nvc.Get("txtCommenceTo") == null) ? "" : nvc.Get("txtCommenceTo").ToString();
                    txtCompletedFrom.Text = (nvc.Get("txtCompletedfrom") == null) ? "" : nvc.Get("txtCompletedfrom").ToString();
                    txtCompletedTo.Text = (nvc.Get("txtCompletedTo") == null) ? "" : nvc.Get("txtCompletedTo").ToString();
                    ucFleet.SelectedFleet = ((nvc.Get("ddlFleet") == null) || General.GetNullableInteger(nvc.Get("ddlFleet")) == null) ? "" : nvc.Get("ddlFleet").ToString();
                    ucOwner.SelectedAddress = (nvc.Get("ddlOwner") == null) ? "" : nvc.Get("ddlOwner").ToString();
                    //chkShowNonEU.Checked = ((nvc.Get("chkShowNonEU")== null) || (nvc.Get("chkShowNonEU")=="0"))?false:true;

                }
                gvConsumption.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetFilter()
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();

        criteria.Add("ddlVessel", PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? UcVessel.SelectedVessel : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
        criteria.Add("ddlPort", ucPortMulti.SelectedValue);
        criteria.Add("ddlPortName", ucPortMulti.Text);
        criteria.Add("txtCommenceFrom", txtCommencedFrom.Text);
        criteria.Add("txtCommenceTo", txtCommencedTo.Text);
        criteria.Add("txtCompletedfrom", txtCompletedFrom.Text);
        criteria.Add("txtCompletedTo", txtCompletedTo.Text);
        criteria.Add("ddlFleet", ucFleet.SelectedFleet);
        criteria.Add("ddlOwner", ucOwner.SelectedAddress);
       // criteria.Add("chkShowNonEU", chkShowNonEU.Checked ? "1" : "0");
        Filter.CurrentChinaVoyageFilter = criteria;
    }
    protected void CrewCourseList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            SetFilter();
            Rebind();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtCommencedFrom.Text = "";
            txtCompletedFrom.Text = "";
            ucPortMulti.Text = "";
            ucPortMulti.SelectedValue = "";
            txtCommencedTo.Text = DateTime.Now.ToShortDateString();
            txtCompletedTo.Text = DateTime.Now.ToShortDateString();
            ucFleet.SelectedFleet = "";
            ucOwner.SelectedAddress = "";

            SetFilter();

            Rebind();
        }
       
        if (CommandName.ToUpper().Equals("PDFALL"))
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            NameValueCollection nvc = Filter.CurrentChinaVoyageFilter;
            int? vesselid = General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? ((nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
            ds = PhoenixVesselPositionChinaEmissionReport.ChinaReportingSearch(vesselid
                                                                               , null
                                                                                , null
                                                                                , 1
                                                                                , 100000
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount
                                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceFrom"] : string.Empty)
                                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceTo"] : string.Empty)
                                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedfrom"] : string.Empty)
                                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedTo"] : string.Empty)
                                                                                , General.GetNullableInteger(nvc != null ? nvc["ddlPort"] : string.Empty)
                                                                                , General.GetNullableInteger(nvc != null ? nvc["ddlOwner"] : string.Empty)
                                                                                , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
                                                                                );
            string directory = HttpContext.Current.Server.MapPath("../Attachments/TEMP/CHINAEMISSION").ToString();
            if (Directory.Exists(directory))
            {
                DirectoryInfo di = new DirectoryInfo(directory);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }
            else
                Directory.CreateDirectory(directory);

            

            string Filelist = "";
            if(ds.Tables[0].Rows.Count>0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];

                    string Tmpfilelocation = string.Empty; string[] reportfile = new string[0];
                    Guid? departureid = General.GetNullableGuid(dr["FLDCURRENTDEPID"].ToString());
                    int? vesselid1 = General.GetNullableInteger(dr["FLDVESSELID"].ToString());

                    string vessselName = dr["FLDVESSELNAME"].ToString();
                    string from = dr["FLDFROM"].ToString();
                    string to = dr["FLDTO"].ToString();

                    DataSet dsdata = PhoenixVesselPositionChinaEmissionReport.ChinaReportingSearch(vesselid1, departureid);

                    if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                    {
                        NameValueCollection nvc1 = new NameValueCollection();

                        nvc1.Add("applicationcode", "17");
                        nvc1.Add("reportcode", "CHINAEMISSION");
                        nvc1.Add("CRITERIA", "");
                        Session["PHOENIXREPORTPARAMETERS"] = nvc1;

                        Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
                        string filename = departureid.ToString()+"_"+vessselName + "_"+ from+"_"+ to + "_" + "ChinaEmissionReport.pdf";
                        Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/CHINAEMISSION/" + filename);

                        PhoenixSsrsReportsCommon.getVersion();
                        PhoenixSsrsReportsCommon.getLogo();
                        PhoenixSSRSReportClass.ExportSSRSReport(reportfile, dsdata, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);

                        if (Filelist == "")
                            Filelist = filename;
                        else
                            Filelist = Filelist + "," + filename;

                    }
                }

                string filePath = HttpContext.Current.Server.MapPath("../Attachments/TEMP/CHINAEMISSION/ChinaEmissionReports.zip");
                PhoenixVesselPositionChinaEmissionReport.ChinaReportingCreateZip(null, null, "ChinaEmissionReports");
                Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=zip");

            }
        }
        
    }
    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDFROM", "FLDTO", "FLDCOMMENCED", "FLDCURRENTSBE", "FLDDISTANCE", "FLDTIMEATSEA", "FLDTRANSPORTWORK", "FLDHFOCONS", "FLDMDOCONS", "FLDLFOCONS" };
        string[] alCaptions = { "Vessel", "From", "To", "Last port of call", "This port of call", "Distance travelled(nm)", "Hours Underway", "Transport Work (T-nm)","HFO Cons","MDO Cons","LFO Cons" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentChinaVoyageFilter;

        int? vesselid = General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? ((nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
        DataSet ds = PhoenixVesselPositionChinaEmissionReport.ChinaReportingSearch(vesselid
                                                                                , sortexpression
                                                                                 , sortdirection
                                                                                 , (int)ViewState["PAGENUMBER"]
                                                                                 , gvConsumption.PageSize
                                                                                 , ref iRowCount
                                                                                 , ref iTotalPageCount
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceFrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceTo"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedfrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedTo"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlPort"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlOwner"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
                                                                                 );


        Response.AddHeader("Content-Disposition", "attachment; filename=\"ChinaEmissionReport.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>China Emission Report</h3></td>");
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
    protected void gvConsumption_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;           
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void UcVessel_OnTextChangedEvent(object sender, EventArgs e)
    {
        SetFilter();
        Rebind();

    }
    protected void ucFleet_OnTextChangedEvent(object sender, EventArgs e)
    {
        SetFilter();
        Rebind();
    }
    protected void ucOwner_OnTextChangedEvent(object sender, EventArgs e)
    {
        SetFilter();
        Rebind();
    }
    protected void gvConsumption_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if(e.CommandName.ToUpper().Equals("PDF"))
            {
               // ConvertToPdf(PrepareHtmlDocAnnualSummary(General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblID")).Text), General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselid")).Text)), "ChinaEmissionReport");

                string Tmpfilelocation = string.Empty; string[] reportfile = new string[0];
                Guid? departureid = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblID")).Text);
                int? vesselid = General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblVesselid")).Text);

                DataSet ds = PhoenixVesselPositionChinaEmissionReport.ChinaReportingSearch(vesselid, departureid);

                if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                {
                    string vessselName = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                    string from = ds.Tables[0].Rows[0]["FLDFROM"].ToString();
                    string to = ds.Tables[0].Rows[0]["FLDTO"].ToString();

                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("applicationcode", "17");
                    nvc.Add("reportcode", "CHINAEMISSION");
                    nvc.Add("CRITERIA", "");
                    Session["PHOENIXREPORTPARAMETERS"] = nvc;

                    Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
                    string filename = departureid.ToString() + "_" + vessselName + "_" + from + "_" + to + "_" + "ChinaEmissionReport.pdf";
                    Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);

                    PhoenixSsrsReportsCommon.getVersion();
                    PhoenixSsrsReportsCommon.getLogo();
                    PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
                    Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
                }
            }
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
 
    private string PrepareHtmlDocAnnualSummary(Guid? departureid,int? vesselid)
    {
        StringBuilder DsHtmlcontent = new StringBuilder();
        try
        {
            DataSet ds = PhoenixVesselPositionChinaEmissionReport.ChinaReportingSearch(vesselid, departureid);

            DataTable dtvesselInfo = ds.Tables[0];
            DataTable dtvoyageinfo = ds.Tables[1];

            DataRow drvessel;
            DataRow drvoyage;
            if (dtvesselInfo.Rows.Count>0)
            { 
                drvessel = dtvesselInfo.Rows[0];
            }
            else
            {
                return "";
            }
            if (dtvoyageinfo.Rows.Count > 0)
            {
                drvoyage = dtvoyageinfo.Rows[0];
            }
            else
            {
                return "";
            }

            DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
            DsHtmlcontent.Append("<font size='9px' face='Helvetica'>");

            DsHtmlcontent.Append("<br />");
            DsHtmlcontent.Append("<table ID='tblheader' border ='1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
            DsHtmlcontent.Append("<font color='white' size='14px'><tr><td height='9' align='left'><b>(Energy Consumption Data Collection Report)</b></td></tr></font>");
            DsHtmlcontent.Append("</table>");
            DsHtmlcontent.Append("<br />");

            DsHtmlcontent.Append("<table ID='tbl1' border='1' cellpadding='7' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Name of Ship : </b></td><td>" + drvessel["FLDVESSELNAME"] + "</td><td bgcolor='#f1f1f1'><b>Name of Ship : </b>>Authorized Organization : </td><td></td></tr>");
            DsHtmlcontent.Append("<tr><td colspan=\"4\" bgcolor='#f1f1f1'><b>Report period : </b></td></tr>");
            DsHtmlcontent.Append("<tr><td colspan=\"4\" bgcolor='#f1f1f1'><b>Voyage report : </b></td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Last port of call : </b></td><td colspan=\"3\">" + drvoyage["FLDCOMMENCED"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>This port of call  : </b></td><td colspan=\"3\">" + drvoyage["FLDCURRENTSBE"] + "</td></tr>");

            DsHtmlcontent.Append("</table></br>");


            DsHtmlcontent.Append("<table ID='tblVerifier' border='1' cellpadding='7' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Item Number</b></td><td bgcolor='#f1f1f1' colspan=\"2\">Items</td><td bgcolor='#f1f1f1'>Contents</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' colspan=\"4\"><b>1. Ship Particulars</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>Name of ship</td><td>" + drvessel["FLDVESSELNAME"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>Name of company</td><td>" + drvessel["FLDCOMPANY"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>IMO number</td><td>" + drvessel["FLDIMONUMBER"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>Flag state</td><td>" + drvessel["FLDFLAG"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>Category of ship</td><td></td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>Ship type</td><td>" + drvessel["FLDSHIPTYPE"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>Date of built</td><td>" + drvessel["FLDBUILTDATE"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>Gross tonnage</td><td>" + drvessel["FLDGROSSTONNAGE"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>Net tonnage</td><td>" + drvessel["FLDNETTONNAGE"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>Deadweight</td><td>" + drvessel["FLDDEADWEIGHT"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>TEU</td><td>" + drvessel["FLDTEU"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>EEDI</td><td>" + drvessel["FLDEEDI"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>Ice class</td><td>" + drvessel["FLDICECLASS"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>Designed speed (kn)</td><td></td></tr>");

            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td bgcolor='#f1f1f1'>rated Power (kW)</ td><td>Main Propulsion engine</ td><td>" + drvessel["FLDKW"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b></b></td><td bgcolor='#f1f1f1'></ td><td>Auxiliary engine(s)</ td><td>" + drvessel["FLDPOWERKWAE1"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b></b></td><td bgcolor='#f1f1f1'></ td><td>Boiler(s)</ td><td>" + drvessel["FLDBOILER"] + "</td></tr>");

            //  DsHtmlcontent.Append("<tr><td><table border=\"0\"><tr><td></td></tr><tr><td>1</td></tr><tr><td></td></tr></table></td><td rowspan=\"3\" bgcolor='#f1f1f1'>rated Power (kW)</ td><td><table border=\"0\"><tr><td>Main Propulsion engine</td></tr></table><hr/><table border=\"0\"><tr><td>Auxiliary engine(s)</td></tr></table><table border=\"0\"><tr><td>Boiler(s)</td></tr></table></ td><td>" + drvessel["FLDKW"] + "</td></tr>");

            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' colspan=\"4\"><b>2. Data on transportation activities</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td bgcolor='#f1f1f1'>Turnover</ td><td>Cargoes (kt•nmile)</ td><td>" + drvoyage["FLDTRANSPORTWORK"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b></b></td><td bgcolor='#f1f1f1'></ td><td>TEUs (TEU•nmile)</ td><td></td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b></b></td><td bgcolor='#f1f1f1'></ td><td>Passengers (person•nmile)</ td><td></td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>Distance travelled(nm)</td><td>" + drvoyage["FLDDISTANCETRAVELLED"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>Hours underway (h)</td><td>" + drvoyage["FLDHOURSUNDERWAY"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"2\" bgcolor='#f1f1f1'>Operation hours (h)</td><td></td></tr>");
            DsHtmlcontent.Append("</table>");

            DsHtmlcontent.Append("<table ID='tblVerifier' border='1' cellpadding='7' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' colspan=\"5\"><b>3. Fuel consumption data</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td bgcolor='#f1f1f1'>Fuel consumption</ td><td>Fuel 1</ td><td>Fuel type</ td><td>HFO</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td bgcolor='#f1f1f1'></ td><td></ td><td>Quantity in metric tonnes</ td><td>" + drvoyage["FLDHFOCONSUMPTION"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td bgcolor='#f1f1f1'></ td><td></ td><td>Methods used for collecting data</ td><td>" + drvoyage["FLDMETHODUSED"] + "</td></tr>");

            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td bgcolor='#f1f1f1'></ td><td>Fuel 2</ td><td>Fuel type</ td><td>MDO/MGO</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td bgcolor='#f1f1f1'></ td><td></ td><td>Quantity in metric tonnes</ td><td>" + drvoyage["FLDMDOCONSUMPTION"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td bgcolor='#f1f1f1'></ td><td></ td><td>Methods used for collecting data</ td><td>" + drvoyage["FLDMETHODUSED"] + "</td></tr>");

            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td bgcolor='#f1f1f1'></ td><td>Fuel 3</ td><td>Fuel type</ td><td>LFO</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td bgcolor='#f1f1f1'></ td><td></ td><td>Quantity in metric tonnes</ td><td>" + drvoyage["FLDLFOCONSUMPTION"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td bgcolor='#f1f1f1'></ td><td></ td><td>Methods used for collecting data</ td><td>" + drvoyage["FLDMETHODUSED"] + "</td></tr>");

            DsHtmlcontent.Append("</table>");

            DsHtmlcontent.Append("<table ID='tblVerifier' border='1' cellpadding='7' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>1</b></td><td colspan=\"3\" bgcolor='#f1f1f1'>Shore power consumption(kW•h)</ td><td></td></tr>");
            DsHtmlcontent.Append("</table>");

            DsHtmlcontent.Append("<br />");

            DsHtmlcontent.Append("<table border ='0'");
            DsHtmlcontent.Append("<tr><td>Company/Vessel Representative:</td> <td>Data Collected by:</td></tr>");
            DsHtmlcontent.Append("<tr><td></td><td></td></tr>");
            DsHtmlcontent.Append("<tr><td></td><td></td></tr>");
            DsHtmlcontent.Append("<tr><td>Reported by:</td> <td>Tel:</td></tr>");
            DsHtmlcontent.Append("</table>");
            DsHtmlcontent.Append("<br /></div>");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return DsHtmlcontent.ToString();
    }
    public void ConvertToPdf(string HTMLString,string filename)
    {
        try
        {
            if (HTMLString != "")
            {
                using (var ms = new MemoryStream())
                {
                    iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(595f, 842f));
                    document.SetPageSize(PageSize.A4);
                    document.SetPageSize(PageSize.A4.Rotate());
                    document.SetMargins(36f, 36f, 36f, 0f);
                    string filefullpath = filename + ".pdf";
                    PdfWriter.GetInstance(document, ms);
                    document.Open();
                    string imageURL = "http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png";
                    StyleSheet styles = new StyleSheet();
                    styles.LoadStyle(".headertable td", "background-color", "Blue");
                    ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);
                    for (int k = 0; k < htmlarraylist.Count; k++)
                    {
                        document.Add((iTextSharp.text.IElement)htmlarraylist[k]);

                    }
                    document.Close();
                    Response.Buffer = true;
                    var bytes = ms.ToArray();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filefullpath);
                    Response.OutputStream.Write(bytes, 0, bytes.Length);
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvConsumption_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvConsumption.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDFROM", "FLDTO", "FLDCOMMENCED", "FLDCURRENTSBE", "FLDDISTANCE", "FLDTIMEATSEA", "FLDTRANSPORTWORK", "FLDHFOCONS", "FLDMDOCONS", "FLDLFOCONS" };
        string[] alCaptions = { "Vessel", "From", "To", "Last port of call", "This port of call", "Distance travelled(nm)", "Hours Underway", "Transport Work (T-nm)", "HFO Cons", "MDO Cons", "LFO Cons" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentChinaVoyageFilter;

        int? vesselid = General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? ((nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
        ds = PhoenixVesselPositionChinaEmissionReport.ChinaReportingSearch(vesselid
                                                                                , sortexpression
                                                                                 , sortdirection
                                                                                 , (int)ViewState["PAGENUMBER"]
                                                                                 , gvConsumption.PageSize
                                                                                 , ref iRowCount
                                                                                 , ref iTotalPageCount
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceFrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceTo"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedfrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedTo"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlPort"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlOwner"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
                                                                                 );
        General.SetPrintOptions("gvConsumption", "China Emision Report", alCaptions, alColumns, ds);

        gvConsumption.DataSource = ds;
        gvConsumption.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;

        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvConsumption.SelectedIndexes.Clear();
        gvConsumption.EditIndexes.Clear();
        gvConsumption.DataSource = null;
        gvConsumption.Rebind();
    }
}
