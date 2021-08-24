using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsEmployeeBondRequisitionLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["ORDERID"] = Request.QueryString["ORDERID"];
                ViewState["ISSTOCKYN"] = Request.QueryString["ISSTOCKYN"].ToString();
                ViewState["NEWPROCESS"] = Request.QueryString["NEWPROCESS"] == null ? null : Request.QueryString["NEWPROCESS"];
                lblIssueType.Visible = false;
                ddlHard.HardTypeCode = "97";
                ddlHard.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 97, 0, "BND");
                ddlHard.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 97, "BND");
                ddlHard.Visible = false;
                ViewState["TOTALAMOUNT"] = string.Empty;
                ViewState["STOREITEMID"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                txtRefNo.Text = Request.QueryString["REQNO"] == null ? "" : Request.QueryString["REQNO"].ToString();
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("General", "GENERAL");
            toolbar.AddButton("Line Item", "LINEITEM");
            toolbar.AddButton("List", "LIST");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbar.Show();
            MenuOrderForm.SelectedMenuIndex = 1;
            CreateMenu();
            ResetMenu();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void Rebind()
    {
        gvCrewSearch.SelectedIndexes.Clear();
        gvCrewSearch.EditIndexes.Clear();
        gvCrewSearch.DataSource = null;
        gvCrewSearch.Rebind();
    }

    private void ResetMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("General", "GENERAL");
        if (ViewState["ORDERID"] != null)
            toolbar.AddButton("Line Item", "LINEITEM");
        if (ViewState["ISSTOCKYN"].ToString() == "0")
            toolbar.AddButton("Employee List", "EMPSTOCK");
        toolbar.AddButton("List", "LIST");
        MenuOrderForm.AccessRights = this.ViewState;
        MenuOrderForm.MenuList = toolbar.Show();
        MenuOrderForm.SelectedMenuIndex = 2;
    }
    protected void MenuBondIssue_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {

                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDEMPLOYEENAME", "FLDSTORENAME", "FLDUNITNAME", "FLDORDEREDQUANTITY", "FLDUNITPRICE", "FLDTOTALPRICE", "FLDISSUEDDATE" };
                string[] alCaptions = { "Employee", "Item Name", "Unit", "Quantity", "Unit Price", "Amount", "Issued On" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixVesselAccountsOrderForm.SearchEmployeeOrderLineitem(General.GetNullableInteger(ddlEmployee.SelectedEmployee),new Guid(ViewState["ORDERDID"].ToString())
                                                                        , sortexpression, sortdirection
                                                                        , 1, iRowCount
                                                                        , ref iRowCount, ref iTotalPageCount);
                General.ShowExcel("Issue of Bonded Stores", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            string[] alColumns = { "FLDEMPLOYEENAME", "FLDSTORENAME", "FLDUNITNAME", "FLDORDEREDQUANTITY", "FLDUNITPRICE", "FLDTOTALPRICE", "FLDISSUEDDATE" };
            string[] alCaptions = { "Employee", "Item Name", "Unit", "Quantity", "Unit Price", "Amount", "Issued On" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsOrderForm.SearchEmployeeOrderLineitem(General.GetNullableInteger(ddlEmployee.SelectedEmployee), new Guid(ViewState["ORDERID"].ToString())
                                                                        , sortexpression, sortdirection
                                                                        , Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), gvCrewSearch.PageSize
                                                                        , ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvCrewSearch", "Requisition LineItems", alCaptions, alColumns, ds);
            gvCrewSearch.DataSource = ds;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                ViewState["TOTALAMOUNT"] = ds.Tables[0].Rows[0]["FLDTOTALPRICE"].ToString();
            else
                ViewState["TOTALAMOUNT"] = "";
            gvCrewSearch.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {

                string id = ((RadLabel)e.Item.FindControl("lblorderlineid")).Text.Trim();
                string quantity = ((UserControlMaskNumber)e.Item.FindControl("txtQuantityEdit")).Text;
                string price = ((UserControlMaskNumber)e.Item.FindControl("txtUnitPriceEdit")).Text;
                string orderid = ((RadLabel)e.Item.FindControl("lblorderidedit")).Text.Trim();
                string storeitemid = ((RadLabel)e.Item.FindControl("lblstoreitemidedit")).Text.Trim();

                if (!IsValidOrderLineItem(price, quantity))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselAccountsOrderForm.UpdateEmployeeOrderFormLineItem(new Guid(id),new Guid(storeitemid),new Guid(orderid), decimal.Parse(quantity), General.GetNullableDecimal(price));

                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                string id = ((RadLabel)e.Item.FindControl("lblorderlineid")).Text.Trim();
                string orderid = ((RadLabel)e.Item.FindControl("lblorderid")).Text.Trim();
                string storeitemid = ((RadLabel)e.Item.FindControl("lblstoreitemid")).Text.Trim();

                PhoenixVesselAccountsOrderForm.DeleteEmployeeOrderFormLineItem(new Guid(id),new Guid(storeitemid),new Guid(orderid));
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
    private bool IsValidOrderLineItem(string price, string quantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Quantity is required.";
        }

        return (!ucError.IsError);
    }
    protected void gvCrewSearch_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
            {
                cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
            }
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }
        if (e.Item is GridFooterItem)
        {
            e.Item.HorizontalAlign = HorizontalAlign.Right;
            e.Item.Font.Bold = true;
            ((RadLabel)e.Item.FindControl("lblAmount")).Text = ViewState["TOTALAMOUNT"].ToString();
        }
    }

    private bool IsValidIssue(string employeeid, string storeitem, string date, string quantity, string oldqty)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        //decimal resultDecimal;
        decimal varience = decimal.Parse(String.IsNullOrEmpty(oldqty) ? "0" : oldqty) - decimal.Parse(String.IsNullOrEmpty(quantity) ? "0" : quantity);

        if (!General.GetNullableInteger(employeeid).HasValue)
        {
            ucError.ErrorMessage = "Staff Name is required.";
        }
        if (string.IsNullOrEmpty(storeitem))
            ucError.ErrorMessage = "Item is required.";
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Issue Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }
        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Quantity is required.";
        }
        else if (decimal.Parse(quantity) <= 0)
        {
            ucError.ErrorMessage = "Quantity should not be zero or negative";
        }
        return (!ucError.IsError);
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.EmployeeRequisitionPickListSelection;
            string stockitemid = nvc.Get("StoreItemId").ToString();
            string qty = nvc.Get("Quantity");
            string unitprice = nvc.Get("UnitPrice");
            if (General.GetNullableInteger(ddlEmployee.SelectedEmployee).HasValue && stockitemid.Trim() != string.Empty)
            {
                PhoenixVesselAccountsOrderForm.InsertEmployeeRequisitionItem(PhoenixSecurityContext.CurrentSecurityContext.VesselID,new Guid(ViewState["ORDERID"].ToString())
                    ,int.Parse(ddlEmployee.SelectedEmployee)
                    , stockitemid.Replace('¿', ','), qty.Replace('¿', ','), unitprice.Replace('¿', ','));
                Rebind();
            }
            else
            {
                string message = "Select a Staff to add Items";
                if (stockitemid.Trim() == string.Empty)
                    message = "No Items selected.";
                ucError.ErrorMessage = message;
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void CreateMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsEmployeeBondRequisitionLineItem.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:showPickList('spnPickListStore', 'codehelp1', '', '../VesselAccounts/VesselAccountsEmployeeStoreSelection.aspx?storeclass=" + ddlHard.SelectedHard + "&accountfor=" + ddlEmployee.SelectedEmployee + "', true); return false;", "Store Item", "<i class=\"fa fa-plus-circle\"></i>", "ADDSTORE");
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsEmployeeBondRequisitionLineItem.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        MenuBondIssue.AccessRights = this.ViewState;
        MenuBondIssue.MenuList = toolbar.Show();
    }
    protected void ddlEmployee_TextChangedEvent(object sender, EventArgs e)
    {
        CreateMenu();
        Rebind();
    }
    protected void OrderForm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
                Response.Redirect("../VesselAccounts/VesselAccountsOrderFormGeneral.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"] + "&NewProcess=" + ViewState["NEWPROCESS"], false);
            else if (CommandName.ToUpper().Equals("LINEITEM"))
                Response.Redirect("../VesselAccounts/VesselAccountsOrderFormLineItem.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"] + "&ISSTOCKYN=" + ViewState["ISSTOCKYN"].ToString() + "&NewProcess=" + ViewState["NEWPROCESS"], false);
            else if (CommandName.ToUpper().Equals("LIST"))
                Response.Redirect("../VesselAccounts/VesselAccountsOrderForm.aspx?storeclass=" + Request.QueryString["storeclass"].ToString(), false);
            else if (CommandName.ToUpper().Equals("EMPSTOCK"))
                Response.Redirect("../VesselAccounts/VesselAccountsEmployeeBondRequisitionLineItem.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"] + "&ISSTOCKYN=" + ViewState["ISSTOCKYN"].ToString() + "&NewProcess=" + ViewState["NEWPROCESS"], false);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
