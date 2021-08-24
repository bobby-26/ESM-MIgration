using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Web.Profile;
using System.Text;
using Telerik.Web.UI;
public partial class CommonPicklistMultipleUser : PhoenixBasePage
{
    string activeyn = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        activeyn = Request.QueryString["activeyn"];

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Common/CommonPicklistMultipleUser.aspx?activeyn=" + Request["activeyn"], "Find", "search.png", "FIND");

        MenuSecurityUsers.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbarmain.AddButton("Done", "DONE", ToolBarDirection.Right);

        MenuUser.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            dgUser.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void MenuUser_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string Script = "";
            NameValueCollection nvc;

            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
            Script += "</script>" + "\n";

            if (CommandName.ToUpper().Equals("DONE"))
            {
                if (dgUser.Items.Count > 0)
                {
                    StringBuilder strUserId = new StringBuilder();
                    StringBuilder strUserName = new StringBuilder();

                    foreach (GridDataItem row in dgUser.MasterTableView.Items)
                    {
                        RadCheckBox chkSelect = (RadCheckBox)row.FindControl("chkSelect");
                        RadLabel lblFirstName = (RadLabel)row.FindControl("lblFirstName");
                        RadLabel lblLastName = (RadLabel)row.FindControl("lblLastName");
                        RadLabel lblUserCode = (RadLabel)row.FindControl("lblUserCode");

                        if (chkSelect.Checked.Equals(true))
                        {
                            strUserId.Append(lblUserCode.Text);
                            strUserId.Append(",");

                            strUserName.Append(lblFirstName.Text + " " + lblLastName.Text);
                            strUserName.Append(", ");
                        }
                    }
                    if (strUserId.Length > 1)
                    {
                        strUserId.Remove(strUserId.Length - 1, 1);
                    }
                    if (strUserName.Length > 1)
                    {
                        strUserName.Remove(strUserName.Length - 2, 2);
                    }

                    nvc = Filter.CurrentPickListSelection;

                    nvc.Set(nvc.GetKey(1), strUserName.ToString());

                    nvc.Set(nvc.GetKey(2), strUserId.ToString());

                    Filter.CurrentPickListSelection = nvc;
                }
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }
            else if (CommandName.ToUpper().Equals("CANCEL"))
            {
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSecurityUsers_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDUSERCODE", "FLDUSERNAME", "FLDGROUPLIST", "FLDACTIVEYN", "FLDFIRSTNAME", "FLDLASTNAME", "FLDMIDDLENAME", "FLDEFFECTIVEFROM" };
        string[] alCaptions = { "User code", "User Name", "Group List", "Active YN", "Firstname", "Lastname", "middlename", "Effective From" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixUser.UserSearch(
            txtSearch.Text, General.GetNullableInteger(activeyn), null,
            General.GetNullableString(ucDepartment.SelectedDepartment),
            txtFirstName.Text, txtMiddleName.Text, txtLastName.Text, null,
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
    protected void dgUser_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void dgUser_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblUser");

            HtmlImage img = (HtmlImage)e.Item.FindControl("imgGroupList");
            img.Attributes.Add("onclick", "showMoreInformation(ev,'" + Session["sitepath"] + "/Options/OptionsMoreInfoGroupList.aspx?usercode=" + lbl.Text + "')");
        }
    }

    protected void dgUser_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgUser.CurrentPageIndex + 1;
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
        dgUser.SelectedIndexes.Clear();
        dgUser.EditIndexes.Clear();
        dgUser.DataSource = null;
        dgUser.Rebind();
    }
}
