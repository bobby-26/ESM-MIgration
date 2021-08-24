using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportAddressLabel : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportAddressLabel.aspx", "Export to Word", "<i class=\"fas fa-file-word\"></i>", "Word");
            
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportAddressLabel.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["SHOWREPORT"] = null;
                ViewState["SHOWFLAG"] = 1;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("WORD"))
            {
                ShowWord();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Response.Redirect("../Crew/CrewReportAddressLabel.aspx");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SHOWFLAG"] = 1;
                ShowReport();

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDNAME", "FLDRANKPOSTEDNAME", "FLDEMPLOYEECODE", "FLDADDRESS1", "FLDADDRESS2", "FLDADDRESS3", "FLDADDRESS4", "FLDCITY", "FLDSTATE", "FLDPOSTALCODE" };
        string[] alCaptions = { "Name", "Rank", "File No", "Address1", "Address2", "Address3", "Address4", "City", "State", "Pin Code" };
        string[] filtercolumns = { "FLDNAME", "FLDRANKPOSTEDNAME", "FLDEMPLOYEECODE", "FLDADDRESS1", "FLDADDRESS2", "FLDADDRESS3", "FLDADDRESS4", "FLDCITY", "FLDSTATE", "FLDPOSTALCODE" };
        string[] filtercaptions = { "Name", "Rank", "File No", "Address1", "Address2", "Address3", "Address4", "City", "State", "Pin Code" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewAddressLabel.CrewAddressLabelSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                    , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                                    , (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist)
                                                    , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                    , General.GetNullableInteger(ucPrinicipal.SelectedAddress)
                                                    , General.GetNullableInteger(ucManager.SelectedAddress)
                                                    , General.GetNullableInteger(ViewState["SHOWFLAG"].ToString())
                                                    , sortexpression, sortdirection
                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                    , gvCrew.PageSize
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                    );

        Response.AddHeader("Content-Disposition", "attachment; filename=AddressLables.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void ShowReport()
    {
        ViewState["SHOWREPORT"] = 1;
        
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDNAME", "FLDRANKPOSTEDNAME", "FLDEMPLOYEECODE", "FLDADDRESS1", "FLDADDRESS2", "FLDADDRESS3", "FLDADDRESS4", "FLDCITY", "FLDSTATE", "FLDPOSTALCODE" };
        string[] alCaptions = { "Name", "Rank", "File No", "Address1", "Address2", "Address3", "Address4", "City", "State", "Pin Code" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewAddressLabel.CrewAddressLabelSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                    , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                                    , (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist)
                                                    , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                    , General.GetNullableInteger(ucPrinicipal.SelectedAddress)
                                                    , General.GetNullableInteger(ucManager.SelectedAddress)
                                                    , General.GetNullableInteger(ViewState["SHOWFLAG"].ToString())
                                                    , sortexpression, sortdirection
                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                    , gvCrew.PageSize
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                    );

        General.SetPrintOptions("gvCrew", "Address Lables", alCaptions, alColumns, ds);


        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

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
    protected void ShowWord()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewAddressLabel.CrewAddressLabelSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                    , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                                    , (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist)
                                                    , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                    , General.GetNullableInteger(ucPrinicipal.SelectedAddress)
                                                    , General.GetNullableInteger(ucManager.SelectedAddress)
                                                    , General.GetNullableInteger(ViewState["SHOWFLAG"].ToString())
                                                    , sortexpression, sortdirection
                                                    , 1
                                                    , iRowCount, ref iRowCount, ref iTotalPageCount
                                                    );

        Response.AddHeader("Content-Disposition", "attachment; filename=AddressLables.doc");
        Response.ContentType = "application/vnd.ms-word";


        string template = @"<table CELLPADDING='2' CELLSPACING='2'   width='100%'> 
                                <tr>
                                    <td style='font-family:Arial; font-size:12px;' align='left'>Name:</td>
                                    <td style='font-family:Arial; font-size:12px;' align='left'>#NAME#</td>
                                    <td style='font-family:Arial; font-size:12px;' align='left'>File No:</td>
                                    <td style='font-family:Arial; font-size:12px;' align='left'>#EMPCODE#</td>
                                </tr>
                                <tr>
                                    <td style='font-family:Arial; font-size:12px;' align='left'>Rank:</td>
                                    <td style='font-family:Arial; font-size:12px;' align='left'>#RANKPOSTEDNAME#</td>
                                    <td colspan='2'></td>
                                </tr>
                                <tr>
                                    <td style='font-family:Arial; font-size:12px;' align='left'>Add 1:</td>
                                    <td style='font-family:Arial; font-size:12px;' align='left' colspan='3'>#ADDRESS1#</td>
                                </tr>
                                <tr>
                                    <td style='font-family:Arial; font-size:12px;' align='left'>Add 2:</td>
                                    <td style='font-family:Arial; font-size:12px;' align='left' colspan='3'>#ADDRESS2#</td>
                                </tr>
                                <tr>
                                    <td style='font-family:Arial; font-size:12px;' align='left'>Add 3:</td>
                                    <td style='font-family:Arial; font-size:12px;' align='left' colspan='3'>#ADDRESS3#</td>
                                </tr>
                                <tr>
                                    <td style='font-family:Arial; font-size:12px;' align='left'>City:</td>
                                    <td style='font-family:Arial; font-size:12px;' align='left'>#CITY#</td>
                                    <td style='font-family:Arial; font-size:12px;' align='left'>State:</td>
                                    <td style='font-family:Arial; font-size:12px;' align='left'>#STATE#</td>
                                </tr>
                                <tr>
                                    <td style='font-family:Arial; font-size:12px;' align='left'>Pin:</td>
                                    <td style='font-family:Arial; font-size:12px;' align='left' colspan='3'>#POSTALCODE#</td>
                                </tr>
                        </table>";

        string STR = "<TABLE>";

        DataTable dt = ds.Tables[0];
        for (int i = 0; i < dt.Rows.Count; i = i + 2)
        {
            STR = STR + "<TR><TD style='border:1px solid black;Height:150px'>";
            STR = STR + template.Replace("#NAME#", dt.Rows[i]["FLDNAME"].ToString()).Replace("#EMPCODE#", dt.Rows[i]["FLDEMPLOYEECODE"].ToString()).Replace("#RANKPOSTEDNAME#", dt.Rows[i]["FLDRANKPOSTEDNAME"].ToString()).Replace("#ADDRESS1#", dt.Rows[i]["FLDADDRESS1"].ToString()).Replace("#ADDRESS2#", dt.Rows[i]["FLDADDRESS2"].ToString()).Replace("#ADDRESS3#", dt.Rows[i]["FLDADDRESS3"].ToString()).Replace("#CITY#", dt.Rows[i]["FLDCITY"].ToString()).Replace("#STATE#", dt.Rows[i]["FLDSTATE"].ToString()).Replace("#POSTALCODE#", dt.Rows[i]["FLDPOSTALCODE"].ToString());
            STR = STR + "</TD><TD style='border:1px solid black; Height:150px' >";
            if (i + 1 < dt.Rows.Count)
            {
                STR = STR + template.Replace("#NAME#", dt.Rows[i + 1]["FLDNAME"].ToString()).Replace("#EMPCODE#", dt.Rows[i + 1]["FLDEMPLOYEECODE"].ToString()).Replace("#RANKPOSTEDNAME#", dt.Rows[i + 1]["FLDRANKPOSTEDNAME"].ToString()).Replace("#ADDRESS1#", dt.Rows[i + 1]["FLDADDRESS1"].ToString()).Replace("#ADDRESS2#", dt.Rows[i + 1]["FLDADDRESS2"].ToString()).Replace("#ADDRESS3#", dt.Rows[i + 1]["FLDADDRESS3"].ToString()).Replace("#CITY#", dt.Rows[i + 1]["FLDCITY"].ToString()).Replace("#STATE#", dt.Rows[i + 1]["FLDSTATE"].ToString()).Replace("#POSTALCODE#", dt.Rows[i + 1]["FLDPOSTALCODE"].ToString());
             }
            STR = STR + "</TD></TR>";
            
            
        }
        STR = STR + "</Table>";
        Response.Write(STR);
        Response.End();

    }
    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    } 
        
    }

