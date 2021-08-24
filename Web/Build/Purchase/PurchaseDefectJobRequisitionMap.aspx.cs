using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
using System.Collections.Specialized;

public partial class PurchaseDefectJobRequisitionMap : PhoenixBasePage
{
    string vesselname;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddImageButton("../Purchase/PurchaseDefectJobRequisitionMap.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Purchase/PurchaseDefectJobRequisitionMap.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbar.AddImageButton("../Purchase/PurchaseDefectJobRequisitionMap.aspx", "Add Requisition", "add.png", "ADD");

            MenuNewRequisition.AccessRights = this.ViewState;
            MenuNewRequisition.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                
                MenuNewRequisition.SelectedMenuIndex = 0;
                if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
                    Response.Redirect("PhoenixLogout.aspx");
                if (Request.QueryString["pageno"] != null)
                {
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["pageno"].ToString());
                    rgvForm.CurrentPageIndex = int.Parse(Request.QueryString["pageno"].ToString()) - 1;
                }
                else
                    ViewState["PAGENUMBER"] = 1;

                if (Request.QueryString["DEFECTID"] != null && Request.QueryString["DEFECTID"].ToString() != "")
                    ViewState["DEFECTID"] = Request.QueryString["DEFECTID"].ToString();
                else
                    ViewState["DEFECTID"] = "";

                if (Request.QueryString["cID"] != null && Request.QueryString["cID"].ToString() != "")
                    ViewState["COMPONENTID"] = Request.QueryString["cID"].ToString();
                else
                    ViewState["COMPONENTID"] = "";

                if (Request.QueryString["Title"] != null && Request.QueryString["Title"].ToString() != "")
                    ViewState["TITLE"] = Request.QueryString["Title"].ToString();
                else
                    ViewState["TITLE"] = "";

                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != "")
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                else
                    ViewState["VESSELID"] = "";

                ddlStockType.SelectedValue = "SPARE";
                ddlStockType.Enabled = false;

                rgvForm.PageSize = General.ShowRecords(null);

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucVessel.Enabled = true;
                if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) !=null)
                {
                    ucVessel.SelectedVessel = ViewState["VESSELID"].ToString();
                    ucVessel.Enabled = false;
                    
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    

    
    protected void MenuNewRequisition_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                rgvForm.CurrentPageIndex = 0;
                rgvForm.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFormNo.Text = "";
                txtTitle.Text = "";
                ucVessel.SelectedVessel = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ddlStockType.SelectedValue = "";
                rgvForm.Rebind();
            }
            else if (CommandName.ToUpper().Equals("ADD"))
            {
                //setTimeout(function(){ top.openNewWindow('req', 'Requisition', '" + Session["sitepath"] + "/Purchase/PurchaseFormType.aspx?cID="+ucComponent.SelectedValue+"&DefectId=" + ViewState["DefectId"].ToString() + "&Title=" + txtTitle.Text + "')},1000)";

                Response.Redirect(Session["sitepath"] + "/Purchase/PurchaseFormType.aspx?cID=" + ViewState["COMPONENTID"].ToString() + "&DefectId=" + ViewState["DEFECTID"].ToString() + "&Title=" + ViewState["TITLE"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlStockType_TextChanged(object sender, EventArgs e)
    {
        rgvForm.Rebind();
    }

    protected void rgvForm_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSTOCKTYPE", "FLDFORMNO", "FLDTITLE", "FLDSUBACCOUNT", "FLDVENDORNAME", "FLDAMOUNTINUSD" };
        string[] alCaptions = { "Type", "Number", "Title", "Budget Code", "Vendor", "Committed(USD)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : ViewState["SORTEXPRESSION"].ToString();


        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (!string.IsNullOrEmpty(txtVessel.Text))
            vesselname = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        else
            vesselname = ucVessel.SelectedVessel.ToString();

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseOrderForm.DefectJobRequisitionSearch(General.GetNullableInteger(ucVessel.SelectedVessel), txtFormNo.Text.Trim(), txtTitle.Text.Trim()
                                                                , General.GetNullableDateTime(txtFromDate.Text)
                                                                , General.GetNullableDateTime(txtToDate.Text)
                                                                , General.GetNullableString(ddlStockType.SelectedValue)
                                                                , sortexpression
                                                                , sortdirection
                                                                , rgvForm.CurrentPageIndex+1
                                                                , rgvForm.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        rgvForm.DataSource = ds;
        rgvForm.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;


        
            
        
    }

    protected void rgvForm_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;           

        }
    }

    protected void rgvForm_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("MAP"))
            {
                GridDataItem item = (GridDataItem)e.Item;

                LinkButton formno = (LinkButton)item["NUMBER"].FindControl("lblFormNumber");

                if (formno ==null || General.GetNullableInteger(ucVessel.SelectedVessel)==null || General.GetNullableGuid(ViewState["DEFECTID"].ToString()) == null)
                {
                    ucError.ErrorMessage = "Please Select the Requisiton.";
                    ucError.Visible = true;
                    return;
                }

                PhoenixPurchaseOrderForm.RequisitionDefectMap(new Guid(ViewState["DEFECTID"].ToString())
                                    , int.Parse(ucVessel.SelectedVessel)
                                    , new Guid(item.GetDataKeyValue("FLDORDERID").ToString())
                                    , formno.Text
                                    );

                String script = String.Format("javascript:closeTelerikWindow('req','dsd');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            else if (e.CommandName.ToUpper() == "DETAILS")
            {
                GridDataItem item = (GridDataItem)e.Item;
                LinkButton formno = (LinkButton)item["NUMBER"].FindControl("lblFormNumber");

                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucVessel", item.GetDataKeyValue("FLDVESSELID").ToString());
                criteria.Add("ddlStockType", "SPARE");
                criteria.Add("txtNumber", formno.Text);
                Filter.CurrentOrderFormFilterCriteria = criteria;

                Filter.CurrentPurchaseDashboardCode = null;

                string script = "top.openNewWindow('detail', 'Requisition', '" + Session["sitepath"] + "/Purchase/PurchaseForm.aspx');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void rgvForm_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        rgvForm.Rebind();
    }

    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        rgvForm.Rebind();
    }

    protected void rgvForm_PreRender(object sender, EventArgs e)
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {

            GridColumn c = rgvForm.MasterTableView.GetColumn("COMMITTED");
            if (c != null)
                c.Visible = false;

            GridColumn c1 = rgvForm.MasterTableView.GetColumn("BUDGETCODE");
            if (c1 != null)
                c1.Visible = false;

            //foreach (GridColumn column in rgvForm.MasterTableView.Columns)
            //{
            //    if (column.UniqueName == "BUDGETCODE" ||column.UniqueName== "COMMITTED")
            //    {
            //        column.Visible = false;
            //    }
            //}
        }
        
    }
}
