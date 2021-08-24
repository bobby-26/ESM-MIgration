using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersThirdPartyLinks : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersThirdPartyLinks.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvThirdPartyLinks')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersThirdPartyLinks.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");            
            MenuRegistersThirdPartyLinks.AccessRights = this.ViewState;
            MenuRegistersThirdPartyLinks.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                gvThirdPartyLinks.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
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
        string[] alColumns = { "FLDNATIONALITYNAME" ,"FLDLINKCODE", "FLDLINKNAME" };
        string[] alCaptions = { "Nationality", "Checker", "Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersThirdPartyLinks.ThirdPartyLinksSearch(txtCode.Text, txtSearch.Text, General.GetNullableInteger(ddlNationality.SelectedNationality)
            , sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        General.ShowExcel("Third Party Links", ds.Tables[0], alColumns, alCaptions, null, null);
    }

    protected void MenuRegistersThirdPartyLinks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvThirdPartyLinks.Rebind();
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

        string[] alColumns = { "FLDNATIONALITYNAME", "FLDLINKCODE", "FLDLINKNAME" };
        string[] alCaptions = { "Nationality", "Checker", "Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersThirdPartyLinks.ThirdPartyLinksSearch(txtCode.Text, txtSearch.Text, General.GetNullableInteger(ddlNationality.SelectedNationality),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvThirdPartyLinks.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        General.SetPrintOptions("gvThirdPartyLinks", "Third Party Links", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvThirdPartyLinks.DataSource = ds;
            gvThirdPartyLinks.VirtualItemCount = iRowCount;
        }
        else
        {
            gvThirdPartyLinks.DataSource = "";
        }
    }
    
    private bool IsValidLink(string code, string name, string nationality)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (code.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (!General.GetNullableInteger(nationality).HasValue)
            ucError.ErrorMessage = "Nationality is required.";

        return (!ucError.IsError);
    }

    private void DeleteLink(int linkid)
    {
        PhoenixRegistersThirdPartyLinks.DeleteThirdPartyLinks(PhoenixSecurityContext.CurrentSecurityContext.UserCode, linkid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvThirdPartyLinks_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string code = ((RadComboBox)e.Item.FindControl("ddlCodeAdd")).Text;
                string name = ((RadTextBox)e.Item.FindControl("txtLinkNameAdd")).Text;
                string nationality = ((UserControlNationality)e.Item.FindControl("ddlNationalityAdd")).SelectedNationality;

                if (!IsValidLink(code, name, nationality))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersThirdPartyLinks.InsertThirdPartyLinks(code, name, int.Parse(nationality));
                ucStatus.Text = "Third Party link added successfully";
                BindData();
                gvThirdPartyLinks.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string linkid = ((RadLabel)e.Item.FindControl("lblLinkidEdit")).Text;
                string code = ((RadComboBox)e.Item.FindControl("ddlCodeEdit")).SelectedValue;
                string name = ((RadTextBox)e.Item.FindControl("txtLinkNameEdit")).Text;
                string nationality = ((UserControlNationality)e.Item.FindControl("ddlNationalityEdit")).SelectedNationality;

                if (!IsValidLink(code, name, nationality))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersThirdPartyLinks.UpdateThirdPartyLinks(int.Parse(linkid), code, name, int.Parse(nationality));
                ucStatus.Text = "Third Party link updated successfully";
                BindData();
                gvThirdPartyLinks.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteLink(Int32.Parse(((RadLabel)e.Item.FindControl("lblLinkid")).Text));
                BindData();
                gvThirdPartyLinks.Rebind();
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

    protected void gvThirdPartyLinks_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvThirdPartyLinks.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvThirdPartyLinks_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            RadComboBox ddl = (RadComboBox)e.Item.FindControl("ddlCodeEdit");
            if (ddl != null)
                ddl.SelectedValue = drv["FLDLINKCODE"].ToString();

            UserControlNationality nat = (UserControlNationality)e.Item.FindControl("ddlNationalityEdit");
            if (nat != null)
                nat.SelectedNationality = drv["FLDNATIONALITY"].ToString();
        }
    }

    protected void gvThirdPartyLinks_SortCommand(object sender, GridSortCommandEventArgs e)
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
            
        
