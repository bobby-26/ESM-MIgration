using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PlannedMaintenanceReportSpareStoreRequisition : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            txtVendorId.Attributes.Add("style", "visibility:hidden");
            txtBudgetId.Attributes.Add("style", "visibility:hidden");
            txtDeliveryLocationId.Attributes.Add("style", "visibility:hidden");
            txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceReportSpareStoreRequisition.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceReportSpareStoreRequisition.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            
            MenugvFormDetails.AccessRights = this.ViewState;
            MenugvFormDetails.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ucFormStatus.HardTypeCode = ((int)PhoenixHardTypeCode.FORMSTATUS).ToString();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["orderid"] = null;
                ViewState["PAGEURL"] = null;
                RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            cmdShowMaker.Attributes.Add("onclick", "javascript:return showPickList('spnPickListVendor', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131', true);");
            cmdShowLocation.Attributes.Add("onclick", "javascript:return showPickList('spnDLocation', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDeliveryLocation.aspx', true);");
            btnShowBudget.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMainBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?hardtypecode=30&isvalidate=1', true);");

        }
        catch (Exception ex)
        {

            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvFormDetails_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                RadGrid1.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFormNoFrom.Text = String.Empty;
                txtFormNoTo.Text = String.Empty;
                txtNameTitle.Text = String.Empty;
                txtVendorId.Text = String.Empty;
                txtVendorNumber.Text = String.Empty;
                txtVenderName.Text = String.Empty;
                txtDeliveryLocationId.Text = String.Empty;
                txtDeliveryLocationCode.Text = String.Empty;
                txtDeliveryLocationName.Text = String.Empty;
                txtBudgetId.Text = String.Empty;
                txtBudgetCode.Text = String.Empty;
                txtBudgetgroupId.Text = String.Empty;
                txtBudgetName.Text = String.Empty;
                txtFinanceYear.Text = String.Empty;
                RadGrid1.Rebind();
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
        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDVENDORNAME", "FLDFORMTYPENAME", "FLDFORMSTATUSNAME", "FLDBUDGETNAME" };
        string[] alCaptions = { "Number", "Form Title", "Vendor", "Form Type", "Form Status", "Budget" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());


        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentOrderFormFilterCriteria;

        ds = PhoenixPlannedMaintenanceReportSpareStoreRequisition.RequisitionReportSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableString(txtFormNoFrom.Text)
                , General.GetNullableString(txtFormNoTo.Text)
                , General.GetNullableString(txtNameTitle.Text.Trim())
                , General.GetNullableInteger(txtVendorId.Text.Trim())
                , General.GetNullableInteger(txtDeliveryLocationId.Text)
                , General.GetNullableInteger(txtBudgetId.Text.Trim())
                , General.GetNullableInteger(txtFinanceYear.Text.TrimEnd("0000".ToCharArray()))
                , General.GetNullableString(ucFormStatus.SelectedHard)
                , sortexpression, sortdirection
                , RadGrid1.CurrentPageIndex + 1
                , RadGrid1.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

        RadGrid1.DataSource = ds;
        RadGrid1.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }



    protected string exceloptions()
    {
        DataSet ds = new DataSet();

        ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 181);

        string options = "";

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["FLDSHORTNAME"].ToString().Equals("EXL"))
                options += "&showexcel=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("WRD"))
                options += "&showword=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("PDF"))
                options += "&showpdf=" + dr["FLDHARDNAME"].ToString();
        }
        return options;
    }


    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("REPORT"))
        {
            string prams = "";

            prams += exceloptions();

            Response.Redirect("../Reports/ReportsView.aspx?applicationcode=6&reportcode=REQUISITIONFORM&formno=" + e.CommandArgument.ToString() + prams);
        }
    }
    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }


    protected void cmdVendorClear_Click(object sender, ImageClickEventArgs e)
    {
        txtVendorNumber.Text = "";
        txtVenderName.Text = "";
        txtVendorId.Text = "";
    }

    protected void cmdLocationClear_Click(object sender, ImageClickEventArgs e)
    {
        txtDeliveryLocationCode.Text = "";
        txtDeliveryLocationName.Text = "";
        txtDeliveryLocationId.Text = "";
    }

    protected void cmdBudgetClear_Click(object sender, ImageClickEventArgs e)
    {
        txtBudgetCode.Text = "";
        txtBudgetName.Text = "";
        txtBudgetId.Text = "";
        txtBudgetgroupId.Text = "";
    }

}
