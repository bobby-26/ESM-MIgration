using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;

public partial class CrewWorkingGearOrderReceive : PhoenixBasePage
{
    private string Neededid = string.Empty;
    private string Agentid = string.Empty;

    private string empid = string.Empty;
    private string vslid = string.Empty;
    private string crewplanid = null;
    private string orderid = null;
    private string r = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"] != "")
                ViewState["planid"] = Request.QueryString["crewplanid"];
            if (Request.QueryString["vslid"] != null && Request.QueryString["vslid"] != "")
                ViewState["vslid"] = Request.QueryString["vslid"];
            if (Request.QueryString["empid"] != null && Request.QueryString["empid"] != "")
                ViewState["empid"] = Request.QueryString["empid"];
            if (Request.QueryString["Neededid"] != null && Request.QueryString["Neededid"] != "")
                ViewState["NEEDEDID"] = Request.QueryString["Neededid"];

            if (Request.QueryString["orderid"] != null && Request.QueryString["orderid"] != "")
                ViewState["ORDERID"] = Request.QueryString["orderid"];
            if (Request.QueryString["r"] != null && Request.QueryString["r"] != "")
                ViewState["r"] = Request.QueryString["r"];

            if (ViewState["empid"] != null)
                empid = ViewState["empid"].ToString();
            if (ViewState["vslid"] != null)
                vslid = ViewState["vslid"].ToString();
            if (ViewState["planid"] != null)
                crewplanid = ViewState["planid"].ToString();
            if (ViewState["NEEDEDID"] != null)
                Neededid = ViewState["NEEDEDID"].ToString();
            if (ViewState["ORDERID"] != null)
                orderid = ViewState["ORDERID"].ToString();
            if (ViewState["r"] != null)
                r = ViewState["r"].ToString();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            if (r != "1")
            {
                toolbarmain.AddButton("List", "LIST");
            }

            toolbarmain.AddButton("Request", "REQUEST");
            toolbarmain.AddButton("Quotation", "QUOTATION");
            toolbarmain.AddButton("Receive", "RECEIVE");

            worknggearmain.AccessRights = this.ViewState;
            worknggearmain.MenuList = toolbarmain.Show();
            if (r != "1")
            {
                worknggearmain.SelectedMenuIndex = 3;
            }
            else
            {
                worknggearmain.SelectedMenuIndex = 2;
            }

            if (Request.QueryString["Neededid"] != null)
            {
                Neededid = Request.QueryString["Neededid"];
                ViewState["Neededid"] = Request.QueryString["Neededid"];
            }

            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                btnconfirm.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ORDERSTATUS"] = "";

                DataSet dsCurrency = PhoenixRegistersCurrency.ListCurrency(1, "INR");

                ViewState["Neededid"] = Request.QueryString["Neededid"];
                ViewState["ACTIVE"] = "1";

                EditWorkGearOrder(General.GetNullableGuid(Neededid));

                gvWorkGearItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewWorkingGearOrderReceive.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWorkGearItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuWorkGearItem.AccessRights = this.ViewState;
            MenuWorkGearItem.MenuList = toolbar.Show();

            MainMenu();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void MainMenu()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkGearGeneral.AccessRights = this.ViewState;
        MenuWorkGearGeneral.MenuList = toolbarmain.Show();
    }

    protected void MenuWorkGearGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidsaveItem(txtReceivedDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewWorkingGearOrderForm.ConfirmOrderFormReceive(General.GetNullableGuid(Neededid)
                                                                        , General.GetNullableDateTime(txtReceivedDate.Text)
                                                                        , General.GetNullableString(txtRemarks.Text.Trim()));
                BindData();
                gvWorkGearItem.Rebind();

                EditWorkGearOrder(General.GetNullableGuid(Neededid));
                ucStatus.Visible = true;
                ucStatus.Text = "Saved Successfully.";
            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                if (!IsValidsaveItem(txtReceivedDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                RadWindowManager1.RadConfirm("Are you sure want to confirm?", "btnconfirm", 320, 150, null, "Confirm");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void btnconfirm_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewWorkingGearOrderForm.WorkingGearConfirm(General.GetNullableGuid(Neededid));
            BindData();
            gvWorkGearItem.Rebind();

            ucStatus.Visible = true;
            ucStatus.Text = "Confirmed Successfully.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void worknggearmain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("REQUEST"))
        {
            Response.Redirect("../Crew/CrewWorkGearNeededItem.aspx?Neededid=" + Neededid + "&empid=" + empid + "&vesselid=" + vslid + "&crewplanid=" + crewplanid + "&r=" + r, false);

        }
        if (CommandName.ToUpper().Equals("RECEIVE"))
        {
            Response.Redirect("../Crew/CrewWorkingGearOrderReceive.aspx?Neededid=" + Neededid + "&empid=" + empid + "&vesselid=" + vslid + "&crewplanid=" + crewplanid + "&r=" + r, false);
        }
        if (CommandName.ToUpper().Equals("QUOTATION"))
        {
            Response.Redirect("../Crew/CrewWGQuotationAgentDetail.aspx?Neededid=" + Neededid + "&empid=" + empid + "&vesselid=" + vslid + "&crewplanid=" + crewplanid + "&r=" + r, false);
        }
        if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../Crew/CrewWorkingGearOrderForm.aspx", false);
        }
    }

    private bool IsValidsaveItem(string receiveddate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (receiveddate == "" || receiveddate == null)
        {
            ucError.ErrorMessage = "Received Date is required.";
        }

        return (!ucError.IsError);
    }
    private void EditWorkGearOrder(Guid? needid)
    {

        DataTable dt = PhoenixCrewWorkingGearOrderForm.EditOrderForm(needid);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            txtRefNo.Text = dr["FLDREFERENCENO"].ToString();
            txtSuppliername.Text = dr["FLDNAME"].ToString();
            txtZone.Text = dr["FLDZONE"].ToString();
            txtReceivedDate.Text = dr["FLDRECEIVEDDATE"].ToString();
            txtTotalAmount.Text = dr["FLDAMOUNTPAID"].ToString();
            txtRemarks.Text = dr["FLDRECEIPTREMARKS"].ToString();
            txtOrderDate.Text = dr["FLDORDERDATE"].ToString();
        }
    }

    protected void MenuWorkGearItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDQUANTITY", "FLDRECEIVEDQUANTITY", "FLDUNITPRICE", "FLDTOTALAMOUNT" };
            string[] alCaptions = { "Working Gear", "Quantity", "Received Quantity", "Unit Price", "Total Price" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixCrewWorkingGearOrderForm.SearchOrderFormLineItem(General.GetNullableGuid(ViewState["Neededid"].ToString())
                , sortexpression, sortdirection,
               1, iRowCount,
               ref iRowCount,
               ref iTotalPageCount);

            General.ShowExcel("Received Item Details", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

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

            string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDQUANTITY", "FLDRECEIVEDQUANTITY", "FLDUNITPRICE", "FLDTOTALAMOUNT" };
            string[] alCaptions = { "Working Gear", "Quantity", "Received Quantity", "Unit Price", "Total Price" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewWorkingGearOrderForm.SearchOrderFormLineItem(General.GetNullableGuid(ViewState["Neededid"].ToString())
                                                                                  , sortexpression, sortdirection,
                                                                                 Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                                 , gvWorkGearItem.PageSize,
                                                                                 ref iRowCount,
                                                                                 ref iTotalPageCount);

            General.SetPrintOptions("gvWorkGearItem", "Received Item Details", alCaptions, alColumns, ds);

            gvWorkGearItem.DataSource = ds;
            gvWorkGearItem.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvWorkGearItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkGearItem.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvWorkGearItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, edit.CommandName)) edit.Visible = false;
            }
        }
    }

    protected void gvWorkGearItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvWorkGearItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string quantity = ((UserControlMaskNumber)e.Item.FindControl("txtReceiveQuantity")).Text;
            string Orderlineid = ((RadLabel)e.Item.FindControl("lblOrderLineId")).Text;

            if (!IsValidOrderLineItem(quantity))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCrewWorkingGearOrderForm.UpdateOrderFormLineItem(General.GetNullableGuid(Neededid), decimal.Parse(quantity), new Guid(Orderlineid));
            EditWorkGearOrder(General.GetNullableGuid(Neededid));

            BindData();
            gvWorkGearItem.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidOrderLineItem(string quantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (quantity == "" || quantity == null)
        {
            ucError.ErrorMessage = "Received Quantity is required.";
        }

        return (!ucError.IsError);
    }

    private bool IsValidOrder(string supplier, string orderdate, string discount, string receiveddate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(orderdate).HasValue)
        {
            ucError.ErrorMessage = "Order Date is required.";
        }
        else if (DateTime.TryParse(orderdate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Order Date should not be later than current date";
        }
        if (!General.GetNullableInteger(supplier).HasValue)
        {
            ucError.ErrorMessage = "Supplier Name is required.";
        }
        if (General.GetNullableDecimal(discount).HasValue && General.GetNullableDecimal(discount).Value > 100)
        {
            ucError.ErrorMessage = "Discount should be between 0 and 100";
        }
        if (General.GetNullableDateTime(receiveddate).HasValue && General.GetNullableDateTime(orderdate).HasValue)
        {
            if (DateTime.Parse(receiveddate) > DateTime.Today)
                ucError.ErrorMessage = "Received Date should not be greater than today Date.";

            if (DateTime.Parse(orderdate) > DateTime.Parse(receiveddate))
                ucError.ErrorMessage = "Received Date should not be less than Order Date.";
        }
        return (!ucError.IsError);
    }

    private bool IsValidConfirm(string supplier, string orderdate, string discount, string receiveddate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(orderdate).HasValue)
        {
            ucError.ErrorMessage = "Order Date is required.";
        }
        else if (DateTime.TryParse(orderdate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Order Date should be earlier than current date";
        }
        if (!General.GetNullableInteger(supplier).HasValue)
        {
            ucError.ErrorMessage = "Supplier Name is required.";
        }
        if (General.GetNullableDecimal(discount).HasValue && General.GetNullableDecimal(discount).Value > 100)
        {
            ucError.ErrorMessage = "Discount should be between 0 and 100";
        }
        if (!General.GetNullableDateTime(receiveddate).HasValue)
        {
            ucError.ErrorMessage = "Received Date is required.";
        }
        if (General.GetNullableDateTime(receiveddate).HasValue && General.GetNullableDateTime(orderdate).HasValue)
        {
            if (DateTime.Parse(receiveddate) > DateTime.Today)
                ucError.ErrorMessage = "Received Date should not be greater than today Date.";

            if (DateTime.Parse(orderdate) > DateTime.Parse(receiveddate))
                ucError.ErrorMessage = "Received Date should not be less than Order Date.";
        }

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvWorkGearItem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



}
