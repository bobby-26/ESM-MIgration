using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using System.Data;
using Telerik.Web.UI;

public partial class AccountsSignoffConfirmationList : PhoenixBasePage
{
    string confirmtext;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsSignoffConfirmationList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSignoffList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsSignoffConfirmationList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsSignoffConfirmationList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            //toolbargrid.AddFontAwesomeButton("../Accounts/AccountsSignoffConfirmationList.aspx", "Bulk Confirm", "<i class=\"fas fa-thumbs-up\"></i>", "BCONFIRM");


            MenuRegisterSignOffList.AccessRights = this.ViewState;
            MenuRegisterSignOffList.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbargridHeader = new PhoenixToolbar();
            toolbargridHeader.AddButton("Sign off", "CONFIRMSIGNOFF", ToolBarDirection.Right);
            toolbargridHeader.AddButton("Sign on", "CONFIRMSIGNON", ToolBarDirection.Right);


            MenuOrderFormHeader.AccessRights = this.ViewState;
            MenuOrderFormHeader.MenuList = toolbargridHeader.Show();

            MenuOrderFormHeader.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                Session["New"] = "N";
                if (Session["CHECKED_ITEMS"] != null)
                    Session.Remove("CHECKED_ITEMS");
                ViewState["SIGNONOFFID"] = "";
                ViewState["EMPLOYEEID"] = "";

                gvSignoffList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSignoffList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixAccountsSignonoffConfirm.SignoffConfirmList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableInteger(ddlVessel.SelectedVessel));

        string[] alCaptions = { "File No.", "Employee", "Rank", "Vessel", "Nationality", "Sign on", "Relief Due", "Sign Off", "Sign Off Reason" };
        string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDNATIONALITY", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDSIGNOFFDATE", "FLDSIGNOFFREASON" };

        General.SetPrintOptions("gvSignoffList", "Sign off Confirmation List", alCaptions, alColumns, ds);

        gvSignoffList.DataSource = ds;
        gvSignoffList.VirtualItemCount = ds.Tables[0].Rows.Count;
    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alCaptions = { "File No.", "Employee", "Rank", "Vessel", "Nationality", "Sign on", "Relief Due", "Sign Off", "Sign Off Reason" };
        string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDNATIONALITY", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDSIGNOFFDATE", "FLDSIGNOFFREASON" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsSignonoffConfirm.SignoffConfirmList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableInteger(ddlVessel.SelectedVessel));


        Response.AddHeader("Content-Disposition", "attachment; filename=SignOffConfirmationList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Sign off Confirmation List</h3></td>");
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;

        gvSignoffList.Rebind();

    }

    protected void gvSignoffList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            ViewState["SIGNONOFFID"] = ((RadLabel)e.Item.FindControl("lblSignonOff")).Text.Trim();
            ViewState["EMPLOYEEID"] = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text.Trim();
            ViewState["VESSELID"] = ((RadLabel)e.Item.FindControl("lblVesselId")).Text.Trim();

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEditDetails");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsSignOffMovementUpdate.aspx?SIGNOFFID=" + ViewState["SIGNONOFFID"].ToString() + "&EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString() + "',false,400,500); return false;");

            }
        }
    }

    protected void gvSignoffList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("APPROVE"))
        {
            ViewState["SIGNONOFFID"] = ((RadLabel)e.Item.FindControl("lblSignonOff")).Text.Trim();

            confirmtext = "Are you sure you want confirm ?";
            RadWindowManager1.RadConfirm(confirmtext, "DeleteRecord", 320, 150, null, "Confirm Sign on");

        }

    }

    protected void MenuRegisterSignOffList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvSignoffList.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlVessel.SelectedVessel = string.Empty;
                gvSignoffList.Rebind();

            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixAccountsSignonoffConfirm.ConfirmSignoff(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , int.Parse(ViewState["SIGNONOFFID"].ToString()));

            gvSignoffList.Rebind();
        }
        catch (Exception ex)
        {
            //ucError.ErrorMessage = ex.Message;
            //ucError.Visible = true;
            RadWindowManager1.RadAlert(ex.Message, 400, 150, null, null);
            return;
        }
    }

    protected void MenuOrderFormHeader_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("CONFIRMSIGNOFF"))
            {
                Response.Redirect("AccountsSignoffConfirmationList.aspx?");
            }
            else if (CommandName.ToUpper().Equals("CONFIRMSIGNON"))
            {
                Response.Redirect("AccountsSignonConfirmationList.aspx?");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}