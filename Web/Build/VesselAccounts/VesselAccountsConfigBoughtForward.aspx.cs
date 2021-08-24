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

public partial class VesselAccountsConfigBoughtForward : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../VesselAccounts/VesselAccountsConfigBoughtForward.aspx", "Find", "<i class=\"fas fa-search\"></i>", "Search");
            MenuBF.AccessRights = this.ViewState;
            MenuBF.MenuList = toolbar1.Show();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Configuration", "CONFIGURATION");
            toolbar.AddButton("Contract", "CONTRACT");
            toolbar.AddButton("Brought Forward", "BF");
            //    toolbar.AddButton("Reimbursement/Deduction", "REIMBURSEMENT");
            Mainmenu.AccessRights = this.ViewState;
            Mainmenu.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ddlYear.SelectedYear = DateTime.Now.Year;
                ddlMonth.SelectedMonth = DateTime.Now.Month.ToString();
                gvBF.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvBF.SelectedIndexes.Clear();
        gvBF.EditIndexes.Clear();
        gvBF.DataSource = null;
        gvBF.Rebind();
    }
    protected void MenuBF_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindDataBroughtForward()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 0; //DEFAULT DESC SORT
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixVesselAccountsEarningDeduction.VesselEarningDeductionBroughForwardSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                       , (short?)General.GetNullableInteger(ddlMonth.SelectedMonth), General.GetNullableInteger(ddlYear.SelectedYear.ToString()), null
                                                                       , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvBF.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount);
            gvBF.DataSource = dt;
            gvBF.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBF_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton de = (LinkButton)e.Item.FindControl("cmdDelete");
            if (de != null)
            {
                de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName);
                de.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
        }
    }
    protected void gvBF_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBF.CurrentPageIndex + 1;
            BindDataBroughtForward();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
 
    protected void gvBF_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                //Guid id = new Guid(_gridView.DataKeys[nCurrenRow].Value.ToString());
                //string month = ((Label)_gridView.Rows[nCurrenRow].FindControl("lblMonth")).Text;
                //string year = ((Label)_gridView.Rows[nCurrenRow].FindControl("lblYear")).Text;
                //string amount = ((TextBox)_gridView.Rows[nCurrenRow].FindControl("txtAmount")).Text;
                //if (!IsValidReimbursement(month, year, amount))
                //{
                //    ucError.Visible = true;
                //    return;
                //}
                //PhoenixVesselAccountsEarningDeduction.UpdateEarningDeductionMonth(PhoenixSecurityContext.CurrentSecurityContext.VesselID, id, int.Parse(month), int.Parse(year), decimal.Parse(amount));
                //_gridView.EditIndex = -1;
                //BindDataBroughtForward();
                //Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                Guid id = new Guid(((RadLabel)e.Item.FindControl("lblearningdeductionid")).Text);                
                PhoenixVesselAccountsEarningDeduction.VesselEarningDeductionDelete(id);
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
  
    private bool IsValidReimbursement(string month, string year, string amount)
    {

        ucError.HeaderMessage = "Please update the following information";
        if (!General.GetNullableInteger(month).HasValue)
            ucError.ErrorMessage = "Month is required";
        if (!General.GetNullableInteger(year).HasValue)
            ucError.ErrorMessage = "Year is required";
        if (!General.GetNullableDecimal(amount).HasValue)
            ucError.ErrorMessage = "Amount is required.";
        return (!ucError.IsError);
    }

}
