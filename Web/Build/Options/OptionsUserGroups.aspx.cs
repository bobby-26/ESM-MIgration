using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class OptionsUserGroups : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("DB Measure", "MEASURE", ToolBarDirection.Right);
            toolbar.AddButton("DB Module", "MODULE", ToolBarDirection.Right);
            toolbar.AddButton("Access", "FUNCTION", ToolBarDirection.Right);
            toolbar.AddButton("Menus", "MENU", ToolBarDirection.Right);          
            toolbar.AddButton("Application", "APPLICATION", ToolBarDirection.Right);
            toolbar.AddButton("User Groups", "USERGROUP", ToolBarDirection.Right);
           
            MenuSecurityAccessRights.MenuList = toolbar.Show();
            MenuSecurityAccessRights.SelectedMenuIndex = 5;

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Options/OptionsUserGroups.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('dgUserGroups')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Options/OptionsUserGroups.aspx", "<b>Find</b>", "search.png", "FIND");

            MenuRegistersUserGroups.MenuList = toolbar.Show();
            //MenuRegistersUserGroups.SetTrigger(pnlUserGroupsEntry);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                dgUserGroups.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            txtSearch.Focus();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void Rebind()
    {
        dgUserGroups.SelectedIndexes.Clear();
        dgUserGroups.EditIndexes.Clear();
        dgUserGroups.DataSource = null;
        dgUserGroups.Rebind();
    }
    protected void SecurityAccessRights_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.Equals("SAVE"))
            {

            }
            else if (CommandName.Equals("USERGROUP"))
                Response.Redirect("OptionsUserGroups.aspx");
            else if (CommandName.Equals("APPLICATION"))
                Response.Redirect("OptionsApplication.aspx");
            else if (CommandName.Equals("MENU"))
                Response.Redirect("OptionsMenuAccess.aspx");
            else if (CommandName.Equals("FUNCTION"))
                Response.Redirect("OptionsFunctionAccess.aspx");
            else if (CommandName.Equals("MODULE"))
                Response.Redirect("OptionsModule.aspx");
            else if (CommandName.Equals("MEASURE"))
                Response.Redirect("OptionsMeasure.aspx");
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
        string[] alColumns = { "FLDGROUPCODE", "FLDGROUPNAME" };
        string[] alCaptions = { "Group Code", "Group name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = SessionUtil.UserGroupSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtSearch.Text, sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        General.ShowExcel("User Group List", ds.Tables[0], alColumns, alCaptions, null, null);
    }

    protected void RegistersUserGroups_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDGROUPCODE", "FLDGROUPNAME" };
        string[] alCaptions = { "Group Code", "Group name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = SessionUtil.UserGroupSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtSearch.Text, sortexpression, sortdirection,
           Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            dgUserGroups.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("dgUserGroups", "User Group List", alCaptions, alColumns, ds);


        dgUserGroups.DataSource = ds;
        dgUserGroups.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    private void InsertUserGroups(string Groupname)
    {
        if (!IsValidUserGroups(Groupname))
        {
            ucError.Visible = true;
            return;
        }
        SessionUtil.UserGroupInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Groupname);
    }

    private void UpdateUserGroups(int Groupcode, string Groupname)
    {
        if (!IsValidUserGroups(Groupname))
        {
            ucError.Visible = true;
            return;
        }
        SessionUtil.UserGroupUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Groupcode, Groupname);
    }

    private bool IsValidUserGroups(string Groupname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        // GridView _gridView = dgUserGroups;

        if (Groupname.Trim().Equals(""))
            ucError.ErrorMessage = "Group name is required.";

        return (!ucError.IsError);
    }

    private void DeleteUserGroups(int Groupcode)
    {
        SessionUtil.UserGroupDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Groupcode);
    }


    protected void dgUserGroups_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertUserGroups(
                 ((RadTextBox)e.Item.FindControl("txtGroupNameAdd")).Text
                );
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtGroupNameAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                UpdateUserGroups(
                    Int16.Parse(((RadLabel)e.Item.FindControl("lblGroupcodeEdit")).Text),
                    ((RadTextBox)e.Item.FindControl("txtGroupNameEdit")).Text
                  );

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("COPYUSERGROUP"))
            {
                SessionUtil.UserGroupCopy(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Int16.Parse(((RadLabel)e.Item.FindControl("lblGroupcode")).Text),
                    null);
                Rebind();

            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteUserGroups(Int32.Parse(((RadLabel)e.Item.FindControl("lblGroupcode")).Text));
                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else
            {
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void dgUserGroups_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

         //   RadLabel l = (RadLabel)e.Item.FindControl("lblGroupcode");

          //  LinkButton lb = (LinkButton)e.Item.FindControl("lnkGroupName");

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCopyUserGroup");
            if (cb != null)
            {
                cb.Attributes.Add("onclick", "return fnConfirmDelete(event, 'This will create a copy of the user group with the name COPY OF " + ((LinkButton)e.Item.FindControl("lnkGroupName")).Text.ToUpper() + ". Do you want to copy?'); return false;");
                cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            }

            LinkButton pb = (LinkButton)e.Item.FindControl("cmdPermission");
            if (pb != null)
            {
              //  pb.Attributes.Add("onclick", "return openNewWindow('codehelp1', 'Permissions', '../Options/OptionsUserAccess.aspx?caption=" + ((LinkButton)e.Item.FindControl("lnkGroupName")).Text + "&accessid=" + ((RadLabel)e.Item.FindControl("lblGroupAccessId")).Text + "');");
               // pb.Attributes.Add("onclick", "openNewWindow('Permissions', '','" + Session["sitepath"] + "/Options/OptionsUserAccess.aspx?caption=" + ((LinkButton)e.Item.FindControl("lnkGroupName")).Text + "&accessid=" + ((RadLabel)e.Item.FindControl("lblGroupAccessId")).Text + "');return false;");
                pb.Attributes.Add("onclick", "openNewWindow('Permissions', '','" + Session["sitepath"] + "/Options/OptionsUserAccess.aspx?caption=" + ((LinkButton)e.Item.FindControl("lnkGroupName")).Text + "&accessid=" + ((RadLabel)e.Item.FindControl("lblGroupAccessId")).Text + "&GroupId=" + ((RadLabel)e.Item.FindControl("lblGroupcode")).Text + "');return false;");
            }

            // Get the LinkButton control in the first cell
            //     LinkButton _doubleClickButton = (LinkButton)e.Item.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            //      string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            //      e.Item.Attributes["ondblclick"] = _jsDouble;
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
    }

    protected void dgUserGroups_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgUserGroups.CurrentPageIndex + 1;
        BindData();
    }
}
