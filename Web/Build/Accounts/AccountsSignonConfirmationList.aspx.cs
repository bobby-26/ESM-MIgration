using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using System.Data;
using System.Collections;

public partial class AccountsSignonConfirmationList : PhoenixBasePage
{
    string confirmtext;
    string header = "";//, error = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsSignonConfirmationList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSignonList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsSignonConfirmationList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsSignonConfirmationList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsSignonConfirmationList.aspx", "Bulk Confirm", "<i class=\"fas fa-thumbs-up\"></i>", "BCONFIRM");


            MenuRegisterSignOnList.AccessRights = this.ViewState;
            MenuRegisterSignOnList.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbargridHeader = new PhoenixToolbar();
            toolbargridHeader.AddButton("Sign off", "CONFIRMSIGNOFF", ToolBarDirection.Right);
            toolbargridHeader.AddButton("Sign on", "CONFIRMSIGNON", ToolBarDirection.Right);


            MenuOrderFormHeader.AccessRights = this.ViewState;
            MenuOrderFormHeader.MenuList = toolbargridHeader.Show();

            MenuOrderFormHeader.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                Session["New"] = "N";
                if (Session["CHECKED_ITEMS"] != null)
                    Session.Remove("CHECKED_ITEMS");
                ViewState["SIGNONOFFID"] = "";

                gvSignonList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSignonList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixAccountsSignonoffConfirm.SignonConfirmList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableInteger(ddlVessel.SelectedVessel));

        string[] alCaptions = { "File No.", "Employee", "Rank", "Vessel", "Nationality", "Sign on Date", "Sign on Port" };
        string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDNATIONALITY", "FLDSIGNONDATE", "FLDSIGNONSEAPORTNAME" };

        General.SetPrintOptions("gvSignonList", "Sign on Confirmation List", alCaptions, alColumns, ds);

        gvSignonList.DataSource = ds;
        gvSignonList.VirtualItemCount = ds.Tables[0].Rows.Count;
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alCaptions = { "File No.", "Employee", "Rank", "Vessel", "Nationality", "Sign on Date","Sign on Port" };
        string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDNATIONALITY", "FLDSIGNONDATE", "FLDSIGNONSEAPORTNAME" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsSignonoffConfirm.SignonConfirmList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableInteger(ddlVessel.SelectedVessel));


        Response.AddHeader("Content-Disposition", "attachment; filename=SignOnConfirmationList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Sign on Confirmation List</h3></td>");
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

        gvSignonList.Rebind();

    }
    protected void gvSignonList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            ViewState["SIGNONOFFID"] = ((RadLabel)e.Item.FindControl("lblSignonOff")).Text.Trim();
            ViewState["EMPLOYEEID"] = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text.Trim();
            ViewState["VESSELID"] = ((RadLabel)e.Item.FindControl("lblVesselId")).Text.Trim();

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEditDetails");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsSignOnMovementUpdate.aspx?SIGNONID=" + ViewState["SIGNONOFFID"].ToString() + "&EMPLOYEEID=" + ViewState["EMPLOYEEID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString() + "',false,400,500); return false;");

            }
        }
    }

    protected void gvSignonList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("APPROVE"))
        {
            ViewState["SIGNONOFFID"] = ((RadLabel)e.Item.FindControl("lblSignonOff")).Text.Trim();

            confirmtext = "Are you sure you want confirm ?";
            RadWindowManager1.RadConfirm(confirmtext, "DeleteRecord", 320, 150, null, "Confirm Sign on");

        }
    }

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixAccountsSignonoffConfirm.ConfirmSignon(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                , int.Parse(ViewState["SIGNONOFFID"].ToString()));

            gvSignonList.Rebind();
        }
        catch (Exception ex)
        {
            // ucError.ErrorMessage = ex.Message;
            // ucError.Visible = true;
            RadWindowManager1.RadAlert(ex.Message, 400, 150, header, null);
            return;
        }
    }

    protected void MenuRegisterSignOnList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvSignonList.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlVessel.SelectedVessel = string.Empty;
                gvSignonList.Rebind();

            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("BCONFIRM"))
            {
                ArrayList SelectedSignonIds = new ArrayList();
                string selectedOthers = ",";
                if (Session["CHECKED_ITEMS"] != null)
                {
                    SelectedSignonIds = (ArrayList)Session["CHECKED_ITEMS"];
                }

                if (SelectedSignonIds.Count > 0)
                {
                    foreach (string index in SelectedSignonIds)
                    {
                        selectedOthers = selectedOthers + index + ",";
                    }

                    ViewState["SIGNONOFFIDS"] = selectedOthers;
                    confirmtext = "Are you sure you want confirm ?";
                    RadWindowManager1.RadConfirm(confirmtext, "BulkConfirm", 320, 150, null, "Confirm Sign on");
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvSignonList$ctl00$ctl02$ctl01$chkAllSignon")
        {
            ArrayList SelectedSignonIds = new ArrayList();
            string signonId = "";
            RadCheckBox chkAll = new RadCheckBox();
            foreach (GridHeaderItem headerItem in gvSignonList.MasterTableView.GetItems(GridItemType.Header))
            {
                chkAll = (RadCheckBox)headerItem["Listcheckbox"].FindControl("chkAllSignon"); // Get the header checkbox
            }
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                SelectedSignonIds = (ArrayList)Session["CHECKED_ITEMS"];
            foreach (GridItem gvrow in gvSignonList.Items)
            {
                signonId = ((RadLabel)gvrow.FindControl("lblSignonOff")).Text;
                RadCheckBox cbSelected = (RadCheckBox)gvrow.FindControl("chkSelect");
                if (cbSelected != null)
                {
                    if (chkAll.Checked == true)
                    {
                        cbSelected.Checked = true;
                        if (!SelectedSignonIds.Contains(signonId))
                        {
                            SelectedSignonIds.Add(signonId);
                        }
                    }
                    else
                    {
                        cbSelected.Checked = false;
                        if (SelectedSignonIds.Contains(signonId))
                        {
                            SelectedSignonIds.Remove(signonId);
                        }
                    }
                }
            }
            if (SelectedSignonIds != null && SelectedSignonIds.Count > 0)
                Session["CHECKED_ITEMS"] = SelectedSignonIds;

        }
    }
    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        try
        {
            ArrayList SelectedSignonIds = new ArrayList();

            string SignonId = "";

            foreach (GridItem gvrow in gvSignonList.Items)
            {
                bool result = false;
                SignonId = ((RadLabel)gvrow.FindControl("lblSignonOff")).Text;
                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    SelectedSignonIds = (ArrayList)Session["CHECKED_ITEMS"];

                if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;
                }

                if (result)
                {
                    if (!SelectedSignonIds.Contains(SignonId))
                        SelectedSignonIds.Add(SignonId);
                }
                else
                {
                    SelectedSignonIds.Remove(SignonId);

                }
            }
            if (SelectedSignonIds != null && SelectedSignonIds.Count > 0)
                Session["CHECKED_ITEMS"] = SelectedSignonIds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    protected void ucBulkConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixAccountsSignonoffConfirm.BulkConfirmSignon(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , ViewState["SIGNONOFFIDS"].ToString());

            gvSignonList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}