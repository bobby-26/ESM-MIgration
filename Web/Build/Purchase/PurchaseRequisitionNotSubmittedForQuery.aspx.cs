using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class PurchaseRequisitionNotSubmittedForQuery : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
         
            toolbar.AddImageButton("../Purchase/PurchaseRequisitionNotSubmittedForQuery.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Purchase/PurchaseRequisitionNotSubmittedForQuery.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuRegistersholiday.AccessRights = this.ViewState;
            MenuRegistersholiday.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                DateTime firstDayOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                ucfromdatesearch.Text = firstDayOfTheMonth.ToString();
                uctodatesearch.Text = DateTime.Now.ToString();
                uclimit.Text = "3";
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
        DataSet ds = new DataSet();
        ds = PhoenixPurchaseRequisitionNotSubmittedForQuery.SearchRequisitionNotSubmittedForQuery(
                                                DateTime.Parse(ucfromdatesearch.Text),
                                                DateTime.Parse(uctodatesearch.Text),
                                                General.GetNullableString(txtfromnosearch.Text),
                                                General.GetNullableInteger(UcVessel.SelectedVessel),
                                                General.GetNullableInteger(ucZonesearch.SelectedZone),
                                                int.Parse(uclimit.Text)
                                                );



        gvRequisition.DataSource = ds;
        //  gvRequisition.DataBind();
    }
    protected void Rebind()
    {
        gvRequisition.SelectedIndexes.Clear();
        gvRequisition.EditIndexes.Clear();
        gvRequisition.DataSource = null;
        gvRequisition.Rebind();
    }

    protected void Registersholiday_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvRequisition.Rebind();

            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucfromdatesearch.Text = "";
                uctodatesearch.Text = "";
                txtfromnosearch.Text = "";
                UcVessel.SelectedVessel = "";
                ucZonesearch.SelectedZone = "";
                uclimit.Text = "";
                //Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void UcVessel_TextChangedEvent(object sender, EventArgs e)
    {

        gvRequisition.Rebind();
    }
    protected void ucZone_Changed(object sender, EventArgs e)
    {

        gvRequisition.Rebind();
    }
    protected void gvRequisition_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
}