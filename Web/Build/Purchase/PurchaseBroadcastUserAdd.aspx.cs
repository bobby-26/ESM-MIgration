using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;

public partial class PurchaseBroadcastUserAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Purchase/PurchaseBroadcastUserAdd.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");

        MenuSecurityUsers.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            gvUser.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
            toolbarmain.AddButton("Add", "DONE",ToolBarDirection.Right);
           
            MenuUser.MenuList = toolbarmain.Show();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
    }

    protected void MenuUser_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            String scriptKeepPopupOpen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'yes');");

            if (CommandName.ToUpper().Equals("DONE"))
            {
                if (gvUser.Items.Count > 0)
                {
                    StringBuilder strUserId = new StringBuilder();

                    foreach (GridDataItem gr in gvUser.Items)
                    {
                        RadCheckBox chkSelect = (RadCheckBox)gr.FindControl("chkSelect");
                        Label lblUserCode = (Label)gr.FindControl("lblUserCode");

                        if (chkSelect.Checked == true)
                        {
                            strUserId.Append(lblUserCode.Text);
                            strUserId.Append(",");
                        }
                    }
                    if (strUserId.Length > 1)
                    {
                        strUserId.Remove(strUserId.Length - 1, 1);

                        PhoenixPurchaseBroadcast.InsertUser(PhoenixSecurityContext.CurrentSecurityContext.UserCode, strUserId.ToString());

                        ucStatus.Text = "Users added.";
                        ucStatus.Visible = true;
                    }
                    else
                    {
                        ucError.ErrorMessage = "Please select atleast one user.";
                        ucError.Visible = true;
                    }
                }
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookmarkScript", scriptKeepPopupOpen, true);
            }
            else if (CommandName.ToUpper().Equals("CANCEL"))
            {
                String scriptClosePopup = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookmarkScript", scriptClosePopup, true);
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
            BindData();
            gvUser.Rebind();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvUser.Rebind();
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
            txtSearch.Text, null, null,
            General.GetNullableString(ucDepartment.SelectedDepartment),
            txtFirstName.Text, txtMiddleName.Text, txtLastName.Text, null,
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvUser.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvUser", "User List", alCaptions, alColumns, ds);

        gvUser.DataSource = ds;
        gvUser.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvUser_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
    }

    protected void gvUser_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvUser_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUser.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvUser_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            Label lbl = (Label)e.Item.FindControl("lblUser");

            HtmlImage img = (HtmlImage)e.Item.FindControl("imgGroupList");
            img.Attributes.Add("onclick", "openNewWindow('codehelp2','Group List', '" + Session["sitepath"] + "/Options/OptionsMoreInfoGroupList.aspx?usercode=" + lbl.Text + "', 'medium')");
        }
    }

    protected void gvUser_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvUser_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
