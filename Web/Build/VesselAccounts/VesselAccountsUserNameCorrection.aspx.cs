using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsUserNameCorrection : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsUserNameCorrection.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVesselUserCorrection')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsUserNameCorrection.aspx", "Refresh List", "<i class=\"fas sync-alt\"></i>", "REFRESH");
            }
            MenuVesselUserCorrection.AccessRights = this.ViewState;
            MenuVesselUserCorrection.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindRankCode();
                gvVesselUserCorrection.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDRANKUSERNAME", "FLDRANKNAME", "FLDEMPLOYEENAME", "FLDVACEMPLOYEENAME" };
        string[] alCaptions = { "User Name", "Rank Name", "Rest Hour Employee", "Vessel Accounting Employee" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixVesselAccountsUserNameCorrection.VesselUsernameCorrectionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
            , General.GetNullableString(ddlRank.SelectedValue), sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=UserCorrection.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>User Correction</h3></td>");
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
    private void BindRankCode()
    {
        DataSet ds = new DataSet();
        ds = PhoenixRegistersRank.ListRank();
        ddlRank.DataSource = ds;
        ddlRank.DataTextField = "FLDRANKNAME";
        ddlRank.DataValueField = "FLDRANKCODE";
        ddlRank.DataBind();
        ddlRank.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDRANKUSERNAME", "FLDRANKNAME", "FLDRHEMPLOYEEREFERENCE", "FLDVACEMPLOYEEREFERENCE" };
        string[] alCaptions = { "User Name", "Rank Name", "Rest Hour Employee", "Vessel Accounting Employee" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataSet ds = PhoenixVesselAccountsUserNameCorrection.VesselUsernameCorrectionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
            , General.GetNullableString(ddlRank.SelectedValue), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
            , gvVesselUserCorrection.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvVesselUserCorrection", "username correction", alCaptions, alColumns, ds);
        gvVesselUserCorrection.DataSource = ds;
        gvVesselUserCorrection.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvVesselUserCorrection_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            //  LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton clb = (LinkButton)e.Item.FindControl("cmdClear");
            if (clb != null) clb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton imgrhemployee = (LinkButton)e.Item.FindControl("imgRHEmployee");
            if (imgrhemployee != null)
            {
                imgrhemployee.Attributes.Add("onclick",
                   "return showPickList('spnPickListRHEmployee', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPicklistRHManualCrewList.aspx?VesselId="
                   + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "', true); ");
            }

            LinkButton imgvacemployee = (LinkButton)e.Item.FindControl("imgVACEmployee");
            if (imgvacemployee != null)
            {
                imgvacemployee.Attributes.Add("onclick",
                   "return showPickList('spnPickListVACEmployee', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId="
                   + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "', true); ");
            }

            RadTextBox txtrhEmployeeId = (RadTextBox)e.Item.FindControl("txtRHEmployeeId");
            if (txtrhEmployeeId != null)
            {
                txtrhEmployeeId.Attributes.Add("style", "display:none;");
                txtrhEmployeeId.Text = drv["FLDRHEMPLOYEEREFERENCE"].ToString();
            }

            RadTextBox txtVACEmployeeId = (RadTextBox)e.Item.FindControl("txtVACEmployeeId");
            if (txtVACEmployeeId != null)
            {
                txtVACEmployeeId.Attributes.Add("style", "display:none;");
                txtVACEmployeeId.Text = drv["FLDVACEMPLOYEEREFERENCE"].ToString();
            }

            RadTextBox txtRHEmployeeName = (RadTextBox)e.Item.FindControl("txtRHEmployeeName");
            if (txtRHEmployeeName != null)
                txtRHEmployeeName.Text = drv["FLDEMPLOYEENAME"].ToString();
            RadTextBox txtVACEmployeeName = (RadTextBox)e.Item.FindControl("txtVACEmployeeName");
            if (txtVACEmployeeName != null)
                txtVACEmployeeName.Text = drv["FLDVACEMPLOYEENAME"].ToString();
            RadTextBox txtRHRankName = (RadTextBox)e.Item.FindControl("txtRHRankName");
            if (txtRHRankName != null)
                txtRHRankName.Text = drv["FLDRHRANKNAME"].ToString();
            RadTextBox txtVACRankName = (RadTextBox)e.Item.FindControl("txtVACRankName");
            if (txtVACRankName != null)
                txtVACRankName.Text = drv["FLDVACRANKNAME"].ToString();
        }
    }
    protected void VesselUserCorrection_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("REFRESH"))
            {
                RefreshVesselUserNameCorrection();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private void InsertVesselUserNameCorrection(string username, int vesselid, int? rhemployeeid, int? vacemployeeid)
    {
        PhoenixVesselAccountsUserNameCorrection.InsertVesselUserNameCorrection(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            username, vesselid, rhemployeeid, vacemployeeid);
    }
    private void RefreshVesselUserNameCorrection()
    {
        PhoenixVesselAccountsUserNameCorrection.RefreshVesselUserNameCorrection(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    }
    protected void gvVesselUserCorrection_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                InsertVesselUserNameCorrection(
                    ((RadLabel)e.Item.FindControl("lblUserNameEdit")).Text,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtRHEmployeeId")).Text),
                    General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtVACEmployeeId")).Text));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("CLEAR"))
            {
                PhoenixVesselAccountsUserNameCorrection.ClearUserNameSelection(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((RadLabel)e.Item.FindControl("lblUserNameCorrectionId")).Text),
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID);
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
    protected void gvVesselUserCorrection_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselUserCorrection.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvVesselUserCorrection.SelectedIndexes.Clear();
        gvVesselUserCorrection.EditIndexes.Clear();
        gvVesselUserCorrection.DataSource = null;
        gvVesselUserCorrection.Rebind();
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }

}
