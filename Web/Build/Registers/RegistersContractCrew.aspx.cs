using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;

public partial class RegistersContractCrew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersContractCrew.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('filter','','" + Session["sitepath"] + "/Registers/RegistersCrewWageComponentsList.aspx?" + "');return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuRegistersCity.AccessRights = this.ViewState;
            MenuRegistersCity.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RegistersCity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {

                string[] alColumns = { "FLDSHORTCODE", "FLDCOMPONENTNAME", "FLDWAGECOMPONENTNAME", "FLDCALCULATIONBASISNAME", "FLDPAYABLEBASISNAME", "FLDDESCRIPTION", "FLDINCLUDEDONBOARDYNNAME", "FLDEARNINGDEDUCTIONNAME", "FLDSUBACCOUNT", "FLDCHARGINGSUBACCOUNT", "FLDACTIVEYNNAME", "FLDSHOWTOOWNERDESC", "FLDSHOWTOOWNERSIGNOFFDESC", "FLDISCHECKOFFERLETTERDESC" };
                string[] alCaptions = { "Code", "Component", "Global Wage Component", "Calculation Basis", "Payable Basis", "Description", "Included Onboard", "Earning/Deduction", "Posting Budget Code", "Charging Budget Code", "Active", "Contract Setting", "Show to owner at sign off", "Is Check Offer Letter" };
                DataTable dt = PhoenixRegistersContract.ListContractCrew(null);
                General.ShowExcel("Crew Wage Components", dt, alColumns, alCaptions, null, string.Empty);
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
        string[] alColumns = { "FLDSHORTCODE", "FLDCOMPONENTNAME", "FLDWAGECOMPONENTNAME", "FLDCALCULATIONBASISNAME", "FLDPAYABLEBASISNAME", "FLDDESCRIPTION", "FLDINCLUDEDONBOARDYNNAME", "FLDEARNINGDEDUCTIONNAME", "FLDSUBACCOUNT", "FLDCHARGINGSUBACCOUNT", "FLDACTIVEYNNAME", "FLDSHOWTOOWNERDESC", "FLDSHOWTOOWNERSIGNOFFDESC" , "FLDISCHECKOFFERLETTERDESC" };
        string[] alCaptions = { "Code", "Component", "Global Wage Component", "Calculation Basis", "Payable Basis", "Description", "Included Onboard", "Earning/Deduction", "Posting Budget Code", "Charging Budget Code", "Active", "Contract Setting", "Show to owner at sign off" ,"Is Check Offer Letter"};

        DataTable dt = PhoenixRegistersContract.ListContractCrew(null);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvCrew", "Crew Wage Components", alCaptions, alColumns, ds);
        gvCrew.DataSource = dt;
        gvCrew.VirtualItemCount = dt.Rows.Count;
    }
    protected void Rebind()
    {
        gvCrew.SelectedIndexes.Clear();
        gvCrew.EditIndexes.Clear();
        gvCrew.DataSource = null;
        gvCrew.Rebind();
    }

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string componentid = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
                PhoenixRegistersContract.DeleteContractCrew(new Guid(componentid));
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrew_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);


            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                eb.Attributes.Add("onclick", "openNewWindow('filter', '', '" + Session["sitepath"] + "/Registers/RegistersCrewWageComponentsList.aspx?&compid=" + drv["FLDCOMPONENTID"].ToString() + "');return false;");
            }
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
}
