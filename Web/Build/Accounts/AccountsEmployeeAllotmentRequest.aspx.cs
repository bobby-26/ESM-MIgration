
using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsEmployeeAllotmentRequest : PhoenixBasePage
{
    NameValueCollection nvc = new NameValueCollection();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (!IsPostBack)
            {
                ViewState["EMPLOYEEID"] = null;
                ViewState["VESSELID"] = null;
                ViewState["MONTH"] = null;
                ViewState["YEAR"] = null;
                ddlMonth.SelectedMonth = Request.QueryString["MONTH"] != null ? Request.QueryString["MONTH"].ToString() : DateTime.Now.Month.ToString();
                ddlYear.SelectedYear = int.Parse(Request.QueryString["YEAR"] != null ? Request.QueryString["YEAR"].ToString() : DateTime.Now.Year.ToString());
                ViewState["VESSELID"] = Request.QueryString["VESSELID"] != null ? Request.QueryString["VESSELID"].ToString() : null;
                ddlVessel.SelectedVessel = Request.QueryString["VESSELID"] != null ? Request.QueryString["VESSELID"].ToString() : string.Empty;


            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("On board", "ONBOARD");
            toolbarmain.AddButton("On Leave", "ONLEAVEORR");
            MenuMainAllotment.AccessRights = this.ViewState;
            MenuMainAllotment.MenuList = toolbarmain.Show();
            MenuMainAllotment.SelectedMenuIndex = 0;
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsEmployeeAllotmentRequest.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvAllotment')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("../Accounts/AccountsEmployeeAllotmentRequest.aspx", "Filter", "search.png", "FIND");
            toolbar.AddImageButton("../Accounts/AccountsEmployeeAllotmentRequest.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbar.AddImageButton("../Accounts/AccountsEmployeeAllotmentRequest.aspx", "Refresh List", "refresh.png", "REFRESH");
            MenuAllotment.AccessRights = this.ViewState;
            MenuAllotment.MenuList = toolbar.Show();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvAllotment.SelectedIndexes.Clear();
        gvAllotment.EditIndexes.Clear();
        gvAllotment.DataSource = null;
        gvAllotment.Rebind();
    }
    protected void MenuMainAllotment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("ONBOARD"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsEmployeeAllotmentRequest.aspx";
            }
            else if (CommandName.ToUpper().Equals("ONLEAVEORR"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsEmployeeAllotmentRequestForOnLeave.aspx";
            }
            Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?MONTH = " + ddlMonth.SelectedMonth + " & YEAR = " + ddlYear.SelectedYear + " & VESSELID = " + ((ddlVessel.SelectedVessel == "" || ddlVessel.SelectedVessel != "Dummy" || Request.QueryString["VESSELID"] == null) ? (Request.QueryString["VESSELID"] != null ? Request.QueryString["VESSELID"].ToString() : ddlVessel.SelectedVessel) : Request.QueryString["VESSELID"].ToString()));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllotment_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {

            LinkButton reb = (LinkButton)e.Item.FindControl("cmdReimRec");
            LinkButton crf = (LinkButton)e.Item.FindControl("cmdRefresh");

            if (reb != null)
            {
                reb.Attributes.Add("onclick", "javascript:openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsPendingReimbursementRecoveries.aspx?EMPLOYEEID=" + drv["FLDEMPLOYEEID"].ToString() + "&VESSELID=" + drv["FLDVESSELID"].ToString() + "&MONTH=" + drv["FLDMONTH"].ToString() + "&YEAR=" + drv["FLDYEAR"].ToString() + "'); return false;");
            }

            UserControlCommonToolTip ttip = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTip");
            if (ttip != null)
            {
                if (drv["FLDCOMPLETEYN"].ToString() == "1" || drv["FLDSIDELETTERYN"].ToString() == "1")
                {
                    ttip.Screen = "Accounts/AccountsEmployeeAllotmentRequestToolTip.aspx?sideletteryn=" + drv["FLDSIDELETTERYN"].ToString() + "&EMPID=" + drv["FLDEMPLOYEEID"] + "&MONTH=" + ddlMonth.SelectedMonth + "&YEAR=" + ddlYear.SelectedYear.ToString() + "&VESSELID=" + ((ddlVessel.SelectedVessel == "" || ddlVessel.SelectedVessel != "Dummy" || Request.QueryString["VESSELID"] == null) ? (Request.QueryString["VESSELID"] != null ? Request.QueryString["VESSELID"].ToString() : ddlVessel.SelectedVessel) : Request.QueryString["VESSELID"].ToString());
                }
                else
                    ttip.Visible = false;
            }
        }


    }

    protected void MenuAllotment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearFilter();
            }
            else if (CommandName.ToUpper().Equals("REFRESH"))
            {
                if (!IsValidCheck())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsEmployeeAllotmentRequest.AllotmentRequestVesselList(null, int.Parse(ddlVessel.SelectedVessel), int.Parse(ddlMonth.SelectedMonth), int.Parse(ddlYear.SelectedYear.ToString()));
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        try
        {
            string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDTOTALAMOUNT" };
            string[] alCaptions = { "File No.", "Name", "Rank", "Amount" };
            DataSet ds = PhoenixAccountsEmployeeAllotmentRequest.EmployeeAllotmentRequestSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                                General.GetNullableInteger((ddlVessel.SelectedVessel == "" || ddlVessel.SelectedVessel != "Dummy" || Request.QueryString["VESSELID"] == null) ? (Request.QueryString["VESSELID"] != null ? Request.QueryString["VESSELID"].ToString() : ddlVessel.SelectedVessel) : Request.QueryString["VESSELID"].ToString()),
                                                                                                General.GetNullableInteger(ddlMonth.SelectedMonth),
                                                                                                General.GetNullableInteger(ddlYear.SelectedYear.ToString()));
            gvAllotment.DataSource = ds;

            General.SetPrintOptions("gvAllotment", "Allotment Employee", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDTOTALAMOUNT" };
            string[] alCaptions = { "File No.", "Name", "Rank", "Amount" };

            DataSet ds = PhoenixAccountsEmployeeAllotmentRequest.EmployeeAllotmentRequestSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                                General.GetNullableInteger(ddlVessel.SelectedVessel),
                                                                                                General.GetNullableInteger(ddlMonth.SelectedMonth),
                                                                                                General.GetNullableInteger(ddlYear.SelectedYear.ToString()));

            Response.AddHeader("Content-Disposition", "attachment; filename=Employee_Allotment_Request" + ".xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
            Response.Write("<h3><center>Employee Allotment Request</center></h3></td>");
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
    private void ClearFilter()
    {
        ddlVessel.SelectedVessel = "";
        ddlMonth.SelectedMonth = "";
        ddlYear.SelectedYear = 0;
        Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCheck()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlVessel.SelectedVessel == "Dummy")
            ucError.ErrorMessage = "Vessel is required";
        if (ddlMonth.SelectedMonth == "")
            ucError.ErrorMessage = "Month is required";
        if (ddlYear.SelectedYear.ToString() == "")
            ucError.ErrorMessage = "Month is required";

        return (!ucError.IsError);
    }
    protected void gvAllotment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblEmployeeId = (RadLabel)e.Item.FindControl("lblEmployeeId");
            RadLabel lblSignonOff = (RadLabel)e.Item.FindControl("lblSignonOff");
            if (e.CommandName.ToUpper().Equals("ALLOTMENTREQUEST"))
            {
                Response.Redirect("AccountsEmployeeAllotmentRequestDetails.aspx?EMPLOYEEID=" + lblEmployeeId.Text + "&VESSELID=" + ddlVessel.SelectedVessel + "&MONTH=" + ddlMonth.SelectedMonth + "&YEAR=" + ddlYear.SelectedYear.ToString() + "&VESSELNAME=" + ddlVessel.SelectedVesselName + "&SIGNONOFFID=" + lblSignonOff.Text, false);
            }
            else if (e.CommandName.ToUpper().Equals("REFRESH"))
            {

                if (!IsValidCheck())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsEmployeeAllotmentRequest.AllotmentRequestVesselList(General.GetNullableInteger(lblEmployeeId.Text), int.Parse(ddlVessel.SelectedVessel), int.Parse(ddlMonth.SelectedMonth), int.Parse(ddlYear.SelectedYear.ToString()));
                Rebind();
            }
            else if (e.CommandName == "Page")
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
    protected void gvAllotment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAllotment.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
