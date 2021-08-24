using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMaintenanceDone : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../Owners/OwnersMaintenanceDone.aspx", "Export to Excel", "<i class=\"fa fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrderReport')", "Print Grid", "<i class=\"fa fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Owners/OwnersMaintenanceDone.aspx", "Filter", "<i class=\"fa fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Owners/OwnersMaintenanceDone.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");

            MenuMaintenanceDone.AccessRights = this.ViewState;
            MenuMaintenanceDone.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddMonths(-6).ToString();
                txtToDate.Text = DateTime.Now.ToString();

                ddlUserVessel.SelectedVessel = Filter.SelectedOwnersReportVessel;
                DataSet ds = new DataSet();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["WORKORDERREPORTID"] = "";
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
                ViewState["FORMURL"] = string.Empty;
                ViewState["DONEID"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvWorkOrderReport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvWorkOrderReport.SelectedIndexes.Clear();
        gvWorkOrderReport.EditIndexes.Clear();
        gvWorkOrderReport.DataSource = null;
        gvWorkOrderReport.Rebind();
    }

    protected void MenuMaintenanceDone_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                if (!IsValidFilter(GetVessel()))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    Rebind();
                }
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtFromDate.Text = DateTime.Now.AddMonths(-6).ToString();
                txtToDate.Text = DateTime.Now.ToString();
                txtComponentName.Text = "";

                txtWorkorderName.Text = "";
                txtWorkordernumber.Text = "";
                ddlUserVessel.SelectedVessel = "";
                ddlVessel.SelectedVessel = "";
                ucDiscipline.SelectedDiscipline = "";
                chkDefect.Checked = false;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private string GetVessel()
    {
        string vessel = Filter.SelectedOwnersReportVessel;
        if (string.IsNullOrEmpty(Request.QueryString["p"]))
        {
            vessel = ddlVessel.SelectedVessel;
            ddlUserVessel.Visible = false;
        }
        else
        {
            vessel = ddlUserVessel.SelectedVessel;
            ddlVessel.Visible = false;
        }
        return vessel;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDWORKDONEDATE", "FLDJOBCLASS", "FLDWORKDURATION", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
        string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Done Date", "Job Class", "Total Duration", "Started", "Completed" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixOwnersPlannedMaintenance.OwnersWorkOrderReportLogSearch(General.GetNullableInteger(GetVessel()).HasValue ? int.Parse(GetVessel()) : 0
                    , General.GetNullableString(txtWorkordernumber.Text), General.GetNullableString(txtWorkorderName.Text), null, General.GetNullableString(txtComponentName.Text), General.GetNullableDateTime(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text)
                    , null, General.GetNullableInteger(ucDiscipline.SelectedDiscipline), null, null, null, null, null, null, null, General.GetNullableByte(chkDefect.Checked == true ? "1" : ""), null, null, null, null, null, null, null
                    , sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvWorkOrderReport.PageSize, ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=MaintenanceDone.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Maintenance Done</h3></td>");
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
    public bool IsValidFilter(string vessellist)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessellist.Equals("") || vessellist.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Select Vessel";
        }

        return (!ucError.IsError);
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDWORKDONEDATE", "FLDJOBCLASS", "FLDWORKDURATION" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Done Date", "Job Class", "Total Duration" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds = PhoenixOwnersPlannedMaintenance.OwnersWorkOrderReportLogSearch(General.GetNullableInteger(GetVessel()).HasValue ? int.Parse(GetVessel()) : 0
            , General.GetNullableString(txtWorkordernumber.Text), General.GetNullableString(txtWorkorderName.Text), null, General.GetNullableString(txtComponentName.Text), General.GetNullableDateTime(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text)
            , null, General.GetNullableInteger(ucDiscipline.SelectedDiscipline), null, null, null, null, null, null, null, General.GetNullableByte(chkDefect.Checked == true ? "1" : ""), null, null, null, null, null, null, null
            , sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvWorkOrderReport.PageSize, ref iRowCount, ref iTotalPageCount);

            gvWorkOrderReport.DataSource = ds;
            gvWorkOrderReport.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkOrderReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkOrderReport.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkOrderReport_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "HISTORY")
            {
                RadLabel formurl = (RadLabel)e.Item.FindControl("formurl");
                RadLabel HISTORY = (RadLabel)e.Item.FindControl("HISTORY");
                RadLabel REPORTHISTORY = (RadLabel)e.Item.FindControl("REPORTHISTORY");
                if (HISTORY.Text == "" && formurl.Text == "" && REPORTHISTORY.Text == "0")
                {
                    ucError.ErrorMessage = "No History available";
                    ucError.Visible = true;
                }
            }
            else
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
    protected void gvWorkOrderReport_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes["onclick"] = "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&u=1" + "'); return false;";
            }
            LinkButton cmdHistory = (LinkButton)e.Item.FindControl("cmdHistory");
            RadLabel workorderid = (RadLabel)e.Item.FindControl("workorderid");
            RadLabel formurl = (RadLabel)e.Item.FindControl("formurl");
            RadLabel doneid = (RadLabel)e.Item.FindControl("doneid");
            RadLabel REPORTHISTORY = (RadLabel)e.Item.FindControl("REPORTHISTORY");
            RadLabel HISTORY = (RadLabel)e.Item.FindControl("HISTORY");

            if (cmdHistory != null && formurl != null && formurl.Text != "")
                cmdHistory.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/StandardForm/" + formurl.Text + "?mode=view&workorderid=" + workorderid.Text + "&doneid=" + doneid.Text + "'); return false;");
            if (cmdHistory != null && REPORTHISTORY.Text == "1" && formurl.Text == "" && HISTORY.Text != "")
                cmdHistory.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Owners/OwnersWorkOrderReportlogHistory.aspx?mode=view&workorderid=" + workorderid.Text + "'); return false;");

            if (cmdHistory != null && HISTORY.Text == "" && formurl.Text == "")
                cmdHistory.Visible = false;
        }
    }

}
