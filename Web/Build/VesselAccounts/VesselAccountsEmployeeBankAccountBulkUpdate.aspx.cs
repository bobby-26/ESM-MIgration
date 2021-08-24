using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Data;
using SouthNests.Phoenix.Export2XL;
using Telerik.Web.UI;


public partial class VesselAccountsEmployeeBankAccountBulkUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsEmployeeBankAccountBulkUpdate.aspx", "Add", "add.png", "ADD");
            MenuExcelUploadItem.AccessRights = this.ViewState;
            MenuExcelUploadItem.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvEmployeeBank.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
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
        gvEmployeeBank.SelectedIndexes.Clear();
        gvEmployeeBank.EditIndexes.Clear();
        gvEmployeeBank.DataSource = null;
        gvEmployeeBank.Rebind();
    }

    protected void MenuExcelUploadItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("ADD"))
            {

                PhoenixVesselAccountsEmployeeBankAccount.EmployeeBankBulkUpdateInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                Rebind();
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
        int? sortdirection = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataTable dt = PhoenixVesselAccountsEmployeeBankAccount.SearchEmployeeBankAccountBulkUpdate(sortdirection
                                                                                           , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                           , General.ShowRecords(null)
                                                                                           , ref iRowCount, ref iTotalPageCount);


        gvEmployeeBank.DataSource = dt;
        gvEmployeeBank.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }


    protected void gvEmployeeBank_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            else if (e.CommandName.ToUpper().Equals("EXCEL"))
            {
                RadLabel id = ((RadLabel)e.Item.FindControl("lblid"));
                PhoenixVesselAccounts2XL.Export2XLEmployeeBankDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(id.Text));
            }
            else if (e.CommandName.ToUpper().Equals("SEARCH"))
            {
                RadLabel status = ((RadLabel)e.Item.FindControl("lblstatus"));
                RadLabel id = ((RadLabel)e.Item.FindControl("lblid"));
                Response.Redirect("../VesselAccounts/VesselAccountsEmployeeBankAccountBulkUpdateDetails.aspx?ID=" + id.Text + "&Status=" + status.Text);
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel id = ((RadLabel)e.Item.FindControl("lblid"));
                PhoenixVesselAccountsEmployeeBankAccount.EmployeeBankBulkUpdateCancel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(id.Text));
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

    protected void gvEmployeeBank_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            ImageButton excel = (ImageButton)e.Item.FindControl("cmdExcel");

            if (drv["FLDSTATUS"].ToString() == "0")
            {
                del.Visible = false;
                excel.Visible = false;
            }
            else
            {

                del.Visible = true;
                excel.Visible = true;
            }
        }
    }

    protected void gvEmployeeBank_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvEmployeeBank.CurrentPageIndex + 1;
        BindData();
    }
}