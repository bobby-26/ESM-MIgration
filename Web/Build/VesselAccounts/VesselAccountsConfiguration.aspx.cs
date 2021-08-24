using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class VesselAccountsConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Configuration", "CONFIGURATION");
            toolbar.AddButton("Contract", "CONTRACT");
            toolbar.AddButton("Brought Forward", "BF");
            toolbar.AddButton("Employee Accounts Configuration", "EAC");
            //  toolbar.AddButton("Reimbursement/Deduction", "REIMBURSEMENT");
            Mainmenu.AccessRights = this.ViewState;
            Mainmenu.MenuList = toolbar.Show();
            Mainmenu.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvVAC.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvVAC.SelectedIndexes.Clear();
        gvVAC.EditIndexes.Clear();
        gvVAC.DataSource = null;
        gvVAC.Rebind();
    }


    protected void Mainmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CONFIGURATION"))
            {
                Response.Redirect("VesselAccountsConfiguration.aspx", false);
            }
            if (CommandName.ToUpper().Equals("CONTRACT"))
            {
                Response.Redirect("VesselAccountsConfigContract.aspx", false);
            }
            if (CommandName.ToUpper().Equals("BF"))
            {
                Response.Redirect("VesselAccountsConfigBoughtForward.aspx", false);
            }
            if (CommandName.ToUpper().Equals("REIMBURSEMENT"))
            {
                Response.Redirect("VesselAccountsConfigReimbursMonthChange.aspx", false);
            }
            if (CommandName.ToUpper().Equals("EAC"))
            {
                Response.Redirect("VesselAccountsEmployeeListQuery.aspx", false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 0; //DEFAULT DESC SORT
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {
            DataTable dt = PhoenixVesselAccountsEmployee.SearchVesselStoreInitialize(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : string.Empty)
                                                                       , null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvVAC.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount);
            gvVAC.DataSource = dt;
            gvVAC.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVAC_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                int vslid = int.Parse(((RadLabel)e.Item.FindControl("lblvesselid")).Text);
                string date = ((UserControlDate)e.Item.FindControl("txtDate")).Text;
                byte officelockyn = (((RadCheckBox)e.Item.FindControl("chkOfficeLockYN")).Checked) == true ? byte.Parse("1") : byte.Parse("0");
                byte officepbuploadyn = (((RadCheckBox)e.Item.FindControl("chkOfficePBUploadYN")).Checked) == true ? byte.Parse("1") : byte.Parse("0");
                string companyid = (((UserControlCompany)e.Item.FindControl("ucCompany")).SelectedCompany);
                string Currencyid = (((UserControlCurrency)e.Item.FindControl("ddlCurrency")).SelectedCurrency);
                byte officeconfirmreqyn = (((RadCheckBox)e.Item.FindControl("chkOfficeConfirmReqYN")).Checked) == true ? byte.Parse("1") : byte.Parse("0");
                if (!IsValidConfig(date))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsEmployee.InsertVesselStoreInitialize(vslid, DateTime.Parse(date), officelockyn, officepbuploadyn, General.GetNullableInteger(companyid), General.GetNullableInteger(Currencyid),officeconfirmreqyn);
                ucStatus.Text = "Vessel Accounting Initialized.";
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
    protected void gvVAC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVAC.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVAC_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdUpdate");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are You Sure You want to Initialize Vessel Accounting ?'); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            UserControlCompany ddlCompany = (UserControlCompany)e.Item.FindControl("ucCompany");
            if (ddlCompany != null)
                ddlCompany.SelectedCompany = drv["FLDCOMPANYID"].ToString();
            UserControlCurrency ddlcurrency = (UserControlCurrency)e.Item.FindControl("ddlCurrency");
            if (ddlcurrency != null)
                ddlcurrency.SelectedCurrency = drv["FLDCURRENCY"].ToString();
        }
    }

    private bool IsValidConfig(string date)
    {

        ucError.HeaderMessage = "Please update the following information";
        if (date == null)
        {
            ucError.ErrorMessage = "Vessel Accounting Start Date is required.";
        }
        return (!ucError.IsError);
    }
}
