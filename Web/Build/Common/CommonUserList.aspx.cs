using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web.Profile;

public partial class CommonUserList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Common/CommonUserList.aspx", "Find", "search.png", "FIND");
        MenuSecurityUsers.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            dgUser.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
    protected void Rebind()
    {
        dgUser.SelectedIndexes.Clear();
        dgUser.EditIndexes.Clear();
        dgUser.DataSource = null;
        dgUser.Rebind();
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
        DataSet ds = ds = PhoenixUser.UserSearch(txtSearch.Text, null, null, General.GetNullableString(ucDepartment.SelectedDepartment),
            txtFirstName.Text, txtMiddleName.Text, txtLastName.Text, null, sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            dgUser.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("dgUser", "User List", alCaptions, alColumns, ds);

        dgUser.DataSource = ds;
        dgUser.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void dgUser_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    protected void dgUser_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgUser.CurrentPageIndex + 1;
        BindData();
    }
}
