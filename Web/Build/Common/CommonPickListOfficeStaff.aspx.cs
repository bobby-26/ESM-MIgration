using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CommonPickListOfficeStaff : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Common/CommonPickListOfficeStaff.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRegistersOfficeStaff.AccessRights = this.ViewState;
            MenuRegistersOfficeStaff.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                ddlactive.SelectedValue = "1";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                gvOfficeStaff.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }       
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersOfficeStaff_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvOfficeStaff.Rebind();
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
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        
        DataSet ds = PhoenixRegistersOfficeStaff.OfficeStaffSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableString(txtEmployeeNumber.Text),
            txtFirstName.Text,
            sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvOfficeStaff.PageSize,
            ref iRowCount,
            ref iTotalPageCount, int.Parse(ddlactive.SelectedValue), General.GetNullableInteger(ucZone.SelectedZone));

        gvOfficeStaff.DataSource = ds;
        gvOfficeStaff.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    
    protected void gvOfficeStaff_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOfficeStaff.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvOfficeStaff_ItemDataBound1(object sender, GridItemEventArgs e)
    {

    }

    protected void gvOfficeStaff_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                string Script = "";
                NameValueCollection nvc = Filter.CurrentPickListSelection;

                RadLabel id = (RadLabel)e.Item.FindControl("lblEmployeeId");

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkEployeeName");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                nvc.Set(nvc.GetKey(2), id.Text.ToString());

                Filter.CurrentPickListSelection = nvc;
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
                
            }

            if (e.CommandName == "Page")
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

    protected void gvOfficeStaff_SortCommand(object sender, GridSortCommandEventArgs e)
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
