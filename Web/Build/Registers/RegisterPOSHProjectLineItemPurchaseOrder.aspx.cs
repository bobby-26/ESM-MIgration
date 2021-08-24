using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Registers_RegisterPOSHProjectLineItemPurchaseOrder : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                ViewState["PROJECTID"] = null;
                ViewState["ACCOUNTID"] = "";

                if (Request.QueryString["id"] != null)
                {
                    ViewState["PROJECTID"] = Request.QueryString["id"].ToString();
                }
                if (Request.QueryString["accountid"] != null)
                {
                    ViewState["ACCOUNTID"] = Request.QueryString["accountid"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["TABVISIBILITY"] = "1";
            }

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Registers/RegisterPOSHProjectLineItemPurchaseOrder.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvLineItem')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageLink("../Registers/RegisterPOSHProjectLineItemPurchaseOrder.aspx", "Filter", "search.png", "FIND");
            toolbargrid.AddImageButton("../Registers/RegisterPOSHProjectLineItemPurchaseOrder.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbargrid.AddImageButton("../Registers/RegisterPOSHProjectLineItemPurchaseOrder.aspx", "Add", "Add.png", "ADD");

            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();

            SetTabDisable();
            ProjectCodeEdit();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SetTabDisable()
    {
        PhoenixToolbar toolmain = new PhoenixToolbar();
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        //if (Request.QueryString["accountid"] != "" && ViewState["TABVISIBILITY"].ToString() != "0")
        //{
        //    toolmain.AddButton("Fund Request", "FUNDREQUEST", ToolBarDirection.Right);
        //    toolmain.AddButton("Line Item", "LINEITEM", ToolBarDirection.Right);
        //    toolmain.AddButton("Project", "PROJECTCODE", ToolBarDirection.Right);
        //    Menu.AccessRights = this.ViewState;
        //    Menu.MenuList = toolmain.Show();
        //    Menu.SelectedMenuIndex = 1;

        //    toolbarmain.AddButton("Vouchers", "VOUCHERREGISTER", ToolBarDirection.Right);
        //    toolbarmain.AddButton("Purchase Order", "PO", ToolBarDirection.Right);
        //    toolbarmain.AddButton("Awaiting Confirmation", "AWAITINGORDER", ToolBarDirection.Right);

        //    MenuLineItem.AccessRights = this.ViewState;
        //    MenuLineItem.MenuList = toolbarmain.Show();
        //    MenuLineItem.SelectedMenuIndex = 1;
        //}
        //else
        //{
            //toolmain.AddButton("Line Item", "LINEITEM", ToolBarDirection.Right);
            //toolmain.AddButton("Project", "PROJECTCODE", ToolBarDirection.Right);
            //Menu.AccessRights = this.ViewState;
            //Menu.MenuList = toolmain.Show();
            //Menu.SelectedMenuIndex = 0;
            //Menu.Visible = false;

            toolbarmain.AddButton("Purchase Order", "PO", ToolBarDirection.Right);

            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbarmain.Show();
            //MenuLineItem.SelectedMenuIndex = 0;
            //ViewState["TABVISIBILITY"] = "0";
        //}
    }

    protected void Menu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (ViewState["PROJECTID"] != null)
            {
                if (CommandName.ToUpper().Equals("PROJECTCODE"))
                {
                    Response.Redirect("../Accounts/AccountsProjectList.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString());
                }
                if (CommandName.ToUpper().Equals("LINEITEM"))
                {
                    Response.Redirect("../Accounts/AccountsProjectLineItemConfirmation.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString(), true);
                }
                if (CommandName.ToUpper().Equals("FUNDREQUEST"))
                {
                    Response.Redirect("../Accounts/AccountsProjectCodeFundRequest.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString(), true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (ViewState["PROJECTID"] != null)
            {
                if (CommandName.ToUpper().Equals("AWAITINGORDER"))
                {
                    Response.Redirect("../Accounts/AccountsProjectLineItemConfirmation.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString());
                }
                else if (CommandName.ToUpper().Equals("PO"))
                {
                    Response.Redirect("../Registers/RegisterPOSHProjectLineItemPurchaseOrder.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString());
                }
                else if (CommandName.ToUpper().Equals("VOUCHERREGISTER"))
                {
                    Response.Redirect("../Accounts/AccountsProjectLineItemVoucherRegister.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("ADD"))
            {
                String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegisterPOSHProjectLineItemPurchaseOrderAdd.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString() + ", false ');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                if (ViewState["PROJECTID"] != null)
                {
                    Response.Redirect("../Registers/RegistersProjectLineItemPurchaseOrderFilter.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString());
                }
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.ProjectCodePurchaseOrderListFilter = null;
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDPONUMBER", "FLDSUPPLIERCODE", "FLDSUPPLIERNAME", "FLDCOMMITTEDDATE", "FLDAMOUNTINUSD", "FLDBUDGETCODE", "FLDOWNERBUDGETCODE" };
                string[] alCaptions = { "PO Number", "Supplier Code", "Supplier Name", "Committed Date", "Committed Amount", "Budget Code", "Owner Budget Code" };

                string sortexpression;
                int? sortdirection = 1;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                NameValueCollection nvc = Filter.ProjectCodePurchaseOrderListFilter;

                DataTable dt = PhoenixRegisterPOSHProjectCode.PurchaseLineItemSearch(General.GetNullableInteger(ViewState["PROJECTID"].ToString())
                                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtPONumber")) : null
                                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
                                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlPOType")) : null
                                                                    , sortexpression, sortdirection
                                                                    , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                    , General.ShowRecords(null)
                                                                    , ref iRowCount, ref iTotalPageCount
                                                                    );

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Purchase Order", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDPONUMBER", "FLDSUPPLIERCODE", "FLDSUPPLIERNAME", "FLDCOMMITTEDDATE", "FLDAMOUNTINUSD", "FLDBUDGETCODE", "FLDOWNERBUDGETCODE" };
            string[] alCaptions = { "PO Number", "Supplier Code", "Supplier Name", "Committed Date", "Committed Amount", "Budget Code", "Owner Budget Code" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (ViewState["PROJECTID"] != null)
            {
                NameValueCollection nvc = Filter.ProjectCodePurchaseOrderListFilter;

                DataTable dt = PhoenixRegisterPOSHProjectCode.PurchaseLineItemSearch(General.GetNullableInteger(ViewState["PROJECTID"].ToString())
                                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtPONumber")) : null
                                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
                                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlPOType")) : null
                                                                    , sortexpression, sortdirection
                                                                    , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                    , General.ShowRecords(null)
                                                                    , ref iRowCount, ref iTotalPageCount
                                                                    );

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                General.SetPrintOptions("gvLineItem", "Purchase Order", alCaptions, alColumns, ds);


                gvLineItem.DataSource = dt;
                gvLineItem.VirtualItemCount = iRowCount;
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton ib = (ImageButton)sender;

            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid _gridView = (RadGrid)sender;
        //  int nCurrentRow = e.Item.ItemIndex;

        if (e.CommandName.ToUpper() == "DELETE")
        {
            string lblPurchaseLineItemId = ((RadLabel)e.Item.FindControl("lblPurchaseLineItemId")).Text;

            if (General.GetNullableGuid(lblPurchaseLineItemId) != null)
            {
                PhoenixRegisterPOSHProjectCode.DeletePurchaseLineiTem(new Guid(lblPurchaseLineItemId));
            }

            Rebind();
            ProjectCodeEdit();
        }

        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        ImageButton db1 = (ImageButton)e.Item.FindControl("cmdDelete");
        if (db1 != null)
        {
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblPONumber");

            db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Please confirm that " + lbl.Text + " is to be untagged from the current project. Please change the Budget Code and Owner Budget Code manually under Committed Costs.')");
            db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);
        }
    }

    protected void ProjectCodeEdit()
    {
        if (ViewState["PROJECTID"] != null)
        {
            DataTable dt = PhoenixRegisterPOSHProjectCode.ProjectEdit(int.Parse(ViewState["PROJECTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                lbltotal.Text = dt.Rows[0]["FLDPURCHASEAMOUNT"].ToString();
                Title1.Text = "Project Code (" + dt.Rows[0]["FLDPROJECTCODE"].ToString() + ")";

                PendingPoEdit();
            }
        }
    }

    protected void PendingPoEdit()
    {
        if (ViewState["PROJECTID"] != null)
        {
            DataTable dt = PhoenixRegisterPOSHProjectCode.PurchasePoPendingAmount(int.Parse(ViewState["PROJECTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                lblPending.Text = dt.Rows[0]["FLDPURCHASEPENDINGPO"].ToString();
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvLineItem.SelectedIndexes.Clear();
        gvLineItem.EditIndexes.Clear();
        gvLineItem.DataSource = null;
        gvLineItem.Rebind();
    }

    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}