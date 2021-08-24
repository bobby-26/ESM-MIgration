using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Accounts_AccountsProjectLineItemVoucherRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolmain = new PhoenixToolbar();

            toolmain.AddButton("Fund Request", "FUNDREQUEST", ToolBarDirection.Right);
            toolmain.AddButton("Line Item", "LINEITEM", ToolBarDirection.Right);
            toolmain.AddButton("Project", "PROJECTCODE", ToolBarDirection.Right);

            Menu.AccessRights = this.ViewState;
            Menu.MenuList = toolmain.Show();
            Menu.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarmain = new PhoenixToolbar();


            toolbarmain.AddButton("Vouchers", "VOUCHERREGISTER", ToolBarDirection.Right);
            toolbarmain.AddButton("Purchase Order", "PO", ToolBarDirection.Right);
            toolbarmain.AddButton("Awaiting Confirmation", "AWAITINGORDER", ToolBarDirection.Right);

            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbarmain.Show();
            MenuLineItem.SelectedMenuIndex = 0;

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

                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../Accounts/AccountsProjectLineItemVoucherRegister.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvLineItem')", "Print Grid", "icon_print.png", "PRINT");
                toolbargrid.AddImageLink("../Accounts/AccountsProjectLineItemVoucherRegister.aspx", "Filter", "search.png", "FIND");
                toolbargrid.AddImageButton("../Accounts/AccountsProjectLineItemVoucherRegister.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
                toolbargrid.AddImageButton("../Accounts/AccountsProjectLineItemVoucherRegister.aspx", "Add", "Add.png", "ADD");

                MenuOrderLineItem.AccessRights = this.ViewState;
                MenuOrderLineItem.MenuList = toolbargrid.Show();
                //  MenuOrderLineItem.SetTrigger(pnlVoucherItemEntry);
            }
            //   BindData();
            ProjectCodeEdit();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
                    Response.Redirect("../Accounts/AccountsProjectLineItemVoucherRegister.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString(), true);
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
                    Response.Redirect("../Accounts/AccountsProjectLineItemPurchaseOrder.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString());
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
                String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsProjectLineItemVoucherRegisterAdd.aspx?id=" + ViewState["PROJECTID"].ToString() + " ');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                if (ViewState["PROJECTID"] != null)
                {
                    Response.Redirect("../Accounts/AccountsProjectLineItemVoucherRegisterFilter.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString());
                }
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.ProjectCodeVoucherListFilter = null;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDVOUCHERDATE", "FLDVOUCHERNUMBER", "FLDREPORTAMOUNT", "FLDBUDGETCODE", "FLDOWNERBUDGETCODE", "FLDSOAREFERENCENUMBER" };
                string[] alCaptions = { "Voucher Date", "Voucher Register Number", "Report Amount", "Budget Code", "Owner Budget Code", "SOA Reference" };

                string sortexpression;
                int? sortdirection = 1;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                NameValueCollection nvc = Filter.ProjectCodeVoucherListFilter;

                DataTable dt = PhoenixAccountProjectVoucherLineItem.VoucherLineItemSearch(General.GetNullableInteger(ViewState["PROJECTID"].ToString())
                                 , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNo")) : null
                                 , nvc != null ? General.GetNullableDateTime(nvc.Get("txtCreatedDate")) : null
                                 , sortexpression, sortdirection
                                 , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                 , General.ShowRecords(null)
                                 , ref iRowCount, ref iTotalPageCount
                                 );

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Voucher Register", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ProjectCodeEdit()
    {
        if (ViewState["PROJECTID"] != null)
        {
            DataTable dt = PhoenixAccountProject.ProjectEdit(int.Parse(ViewState["PROJECTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                lbltotal.Text = dt.Rows[0]["FLDVOUCHERAMOUNT"].ToString();
                Title1.Text = "Project Code (" + dt.Rows[0]["FLDPROJECTCODE"].ToString() + ")";
            }
        }
    }


    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVOUCHERDATE", "FLDVOUCHERNUMBER", "FLDREPORTAMOUNT", "FLDBUDGETCODE", "FLDOWNERBUDGETCODE", "FLDSOAREFERENCENUMBER" };
            string[] alCaptions = { "Voucher Date", "Voucher Register Number", "Report Amount", "Budget Code", "Owner Budget Code", "SOA Reference" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (ViewState["PROJECTID"] != null)
            {
                NameValueCollection nvc = Filter.ProjectCodeVoucherListFilter;

                DataTable dt = PhoenixAccountProjectVoucherLineItem.VoucherLineItemSearch(General.GetNullableInteger(ViewState["PROJECTID"].ToString())
                                                                             , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNo")) : null
                                                                             , nvc != null ? General.GetNullableDateTime(nvc.Get("txtCreatedDate")) : null
                                                                             , sortexpression, sortdirection
                                                                             , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                             , General.ShowRecords(null)
                                                                             , ref iRowCount, ref iTotalPageCount
                                                                            );

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                General.SetPrintOptions("gvLineItem", "Voucher Register", alCaptions, alColumns, ds);

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

    protected void gvLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "DELETE")
        {
            string lblVoucherLineItemId = ((RadLabel)e.Item.FindControl("lblVoucherLineItemId")).Text;

            if (General.GetNullableGuid(lblVoucherLineItemId) != null)
            {
                PhoenixAccountProjectVoucherLineItem.DeleteVoucherLineiTem(new Guid(lblVoucherLineItemId));
                ProjectCodeEdit();
            }
            Rebind();
        }
    }

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        ImageButton db1 = (ImageButton)e.Item.FindControl("cmdDelete");
        if (db1 != null)
        {
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblVoucherNumber");

            db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Please confirm that " + lbl.Text + " is to be untagged from the current project')");
            db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);
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