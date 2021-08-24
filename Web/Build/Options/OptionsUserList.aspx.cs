using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class OptionsUserList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Options/OptionsUserList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('dgUser')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Options/OptionsUserList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Options/OptionsUser.aspx'); return false;", "Find", "<i class=\"fa fa-plus-circle\"></i>", "FIND");

            MenuSecurityUsers.MenuList = toolbar.Show();
            //  MenuSecurityUsers.SetTrigger(RadAjaxPanel1);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ddlUserStatus.DataSource = PhoenixUser.UserStatusList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ddlUserStatus.DataBind();
                dgUser.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ddlUserStatus.Items.Insert(0, new DropDownListItem("--Select--", "--Dummy--"));

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
        dgUser.SelectedIndexes.Clear();
        dgUser.EditIndexes.Clear();
        dgUser.DataSource = null;
        dgUser.Rebind();
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDUSERCODE", "FLDUSERNAMEEX", "FLDGROUPLISTNAME", "FLDACTIVEYNTEXT", "FLDFIRSTNAME", "FLDLASTNAME", "FLDMIDDLENAME", "FLDEFFECTIVEFROM" };
        string[] alCaptions = { "User code", "User Name", "Group List", "Active YN", "Firstname", "Lastname", "middlename", "Effective From" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string usertype = (ucUserType.SelectedUserType == "USERS") ? null : ucUserType.SelectedUserType;

        ds = PhoenixUser.MappedUserSearch(
            General.GetNullableString(txtSearch.Text),
            General.GetNullableInteger(ddlUserStatus.SelectedValue), null,
            General.GetNullableString(ucDepartment.SelectedDepartment),
            null, General.GetNullableString(txtFullName.Text), null, General.GetNullableString(usertype),
            General.GetNullableString(ucUserGroup.SelectedUserGroup),
            General.GetNullableInteger(ucZone.SelectedZone),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            dgUser.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=User.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>User Register</h3></td>");
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

    protected void cmdTest_Click(object sender, EventArgs e)
    {

    }

    protected void MenuSecurityUsers_TabStripCommand(object sender, EventArgs e)
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();

    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDUSERCODE", "FLDUSERNAMEEX", "FLDGROUPLISTNAME", "FLDACTIVEYNTEXT", "FLDFIRSTNAME", "FLDLASTNAME", "FLDMIDDLENAME", "FLDEFFECTIVEFROM" };
        string[] alCaptions = { "User code", "User Name", "Group List", "Active Y/N", "First Name", "Last Name", "Middle Name", "Effective From" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string usertype = General.GetNullableString(ucUserType.SelectedUserType);


        DataSet ds = PhoenixUser.MappedUserSearch(
                txtSearch.Text, General.GetNullableInteger(ddlUserStatus.SelectedValue), null,
                General.GetNullableString(ucDepartment.SelectedDepartment),
                null, txtFullName.Text, null, usertype,
                General.GetNullableString(ucUserGroup.SelectedUserGroup),
                General.GetNullableInteger(ucZone.SelectedZone),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                dgUser.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("dgUser", "User List", alCaptions, alColumns, ds);
        dgUser.DataSource = ds;
        dgUser.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ucUserGroup_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();

    }
    protected void dgUser_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    private void DeleteUser(int usercode)
    {

        PhoenixUser.UserDelete(usercode);
    }
    protected void dgUser_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblUser");

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkUserName");
            if (lb != null)
            {
                lb.Attributes.Add("onclick", "openNewWindow('OptionsUser', '','" + Session["sitepath"] + "/Options/OptionsUser.aspx?usercode=" + lbl.Text + "'); return false;");
            }


            HtmlImage img = (HtmlImage)e.Item.FindControl("imgGroupList");
            img.Attributes.Add("onclick", "openNewWindow('OptionsUser', '','" + Session["sitepath"] + "/Options/OptionsMoreInfoGroupList.aspx?usercode=" + lbl.Text + "'); return false;");


            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "openNewWindow('OptionsUser', '','" + Session["sitepath"] + "/Options/OptionsUser.aspx?usercode=" + lbl.Text + "'); return false;");
            }

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSecurity");
            if (sb != null)
            {
                sb.Attributes.Add("onclick", "openNewWindow('OptionsUser', '','" + Session["sitepath"] + "/Options/OptionsUserCommandAccess.aspx?usercode=" + lbl.Text + "'); return false;");
            }

            LinkButton pb = (LinkButton)e.Item.FindControl("cmdDashboardPreference");
            if (pb != null)
            {
                pb.Attributes.Add("onclick", "openNewWindow('OptionsUser', '','" + Session["sitepath"] + "/Dashboard/DashboardUserPreferences.aspx?usercode=" + lbl.Text + "'); return false;");
            }

            LinkButton sv = (LinkButton)e.Item.FindControl("cmdSendtoVessel");
            if (sv != null)
            {
                sv.Attributes.Add("onclick", "openNewWindow('OptionsUser', '', '" + Session["sitepath"] + "/Options/OptionsUserVesselSend.aspx?usercode=" + lbl.Text + "'); return false;");
            }
        }
    }
    protected void dgUser_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("LOGINRESET"))
            {
                PhoenixUser.UserReset(int.Parse(((RadLabel)e.Item.FindControl("lblUserCode")).Text));

            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteUser(Int32.Parse(((RadLabel)e.Item.FindControl("lblUserCode")).Text));
                BindData();
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
    protected void dgUser_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgUser.CurrentPageIndex + 1;
        BindData();
    }
}
