using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseBulkPOPartPaid : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem r in gvBulkPOPartPaid.Items)
        {
            if (r is GridDataItem)
            {
                Page.ClientScript.RegisterForEventValidation(gvBulkPOPartPaid.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseBulkPOPartPaid.aspx?OrderId=" + Request.QueryString["OrderId"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvBulkPOPartPaid')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["ORDERID"] = "";
                if (Request.QueryString["ORDERID"] != null && Request.QueryString["ORDERID"].ToString() != "")
                    ViewState["ORDERID"] = Request.QueryString["ORDERID"].ToString();
                if (Request.QueryString["FORMNO"] != null && Request.QueryString["FORMNO"].ToString() != "")
                    txtOrderNumber.Text = Request.QueryString["FORMNO"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDESCRIPTION", "FLDAMOUNT", "FLDEXCHANGERATE", "FLDVOUCHERNUMBER", "FLDVOUCHERDATE" };
        string[] alCaptions = { "Description", "Amount", "Exchange Rate", "Voucher Number", "Voucher Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());    

        DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOPartPaidSearch(
              General.GetNullableGuid(ViewState["ORDERID"].ToString())
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount);

        gvBulkPOPartPaid.DataSource = ds;
        gvBulkPOPartPaid.VirtualItemCount = iRowCount;
      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvBulkPOPartPaid", "Part Paid", alCaptions, alColumns, ds);
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDDESCRIPTION", "FLDAMOUNT", "FLDEXCHANGERATE", "FLDVOUCHERNUMBER", "FLDVOUCHERDATE" };
        string[] alCaptions = { "Description", "Amount", "Exchange Rate", "Voucher Number", "Voucher Date" };
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseBulkPurchase.BulkPOPartPaidSearch(
              General.GetNullableGuid(ViewState["ORDERID"].ToString())
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Part_Paid.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Part Paid</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void InsertOrderPartPaid(Guid orderid, decimal amount, string description)
    {

        PhoenixPurchaseBulkPurchase.InsertBulkPOPartPaid(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            orderid, amount, description);

    }

    private void UpdateOrderPartPaid(Guid orderpartpaidid, Guid orderid, decimal amount, string description)
    {
        PhoenixPurchaseBulkPurchase.UpdateBulkPOPartPaid(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            orderpartpaidid, orderid, amount, description);
        ucStatus.Text = "Order Part Paid information updated";

    }

    private bool IsValidOrderPartPaid(string amount, string description)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }

    private void DeleteOrderPartPaid(Guid orderpartpaidid)
    {
        PhoenixPurchaseBulkPurchase.DeleteBulkPOPartPaid(PhoenixSecurityContext.CurrentSecurityContext.UserCode, orderpartpaidid);
    }

    private void ApprovedRequestAdvance(Guid orderpartpaidid)
    {
        try
        {
            string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";

            PhoenixPurchaseBulkPurchase.ApproveBulkPOPartPaid(
                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , orderpartpaidid
                , new Guid(strorderid));

            DataSet ds = PhoenixPurchaseBulkPurchase.EditBulkPOPartPaid(orderpartpaidid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                PhoenixAccountsAdvancePayment.AdvancePaymentInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , int.Parse(dr["FLDVENDORID"].ToString())
                    , decimal.Parse(dr["FLDAMOUNT"].ToString())
                    , DateTime.Parse(DateTime.Now.ToString())
                    , int.Parse(dr["FLDCURRENCYID"].ToString())
                    , null
                    , dr["FLDFORMNUMBER"].ToString()
                    , null
                    , new Guid(strorderid)
                    , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 124, "SAS")),
                     General.GetNullableInteger(dr["FLDBILLTOCOMPANYID"].ToString()), // companyid
                     General.GetNullableInteger(dr["FLDBUDGETID"].ToString()), // budget code
                     General.GetNullableInteger(dr["FLDVESSELID"].ToString()), // vessel id
                     General.GetNullableInteger("") // bankid
                     );
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
   
    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    protected void gvBulkPOPartPaid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvBulkPOPartPaid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            Label lb = (Label)e.Item.FindControl("lblstatus");
            LinkButton cmdApprove = (LinkButton)e.Item.FindControl("cmdApprove");
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton lnkDescription = (LinkButton)e.Item.FindControl("lnkDescription");
            if (cmdApprove != null && lb != null && lb.Text == "APP")
            {
                cmdApprove.Visible = false;
                if (cmdDelete != null)
                    cmdDelete.Visible = false;
                if (edit != null)
                    edit.Visible = false;
                if (lnkDescription != null)
                    lnkDescription.CommandName = "";
                e.Item.Attributes["onclick"] = "";
            }
        }
    }

    protected void gvBulkPOPartPaid_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList(null, 'yes', 'yes');";
            Script += "</script>" + "\n";

            GridFooterItem footerItem = (GridFooterItem)gvBulkPOPartPaid.MasterTableView.GetItems(GridItemType.Footer)[0];

            string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";

            Guid orderid = new Guid(strorderid);

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidOrderPartPaid(
                    ((UserControlDecimal)footerItem.FindControl("txtAmountAdd")).Text,
                    ((RadTextBox)footerItem.FindControl("txtDescriptionAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertOrderPartPaid(
                    orderid,
                    decimal.Parse(((UserControlDecimal)footerItem.FindControl("txtAmountAdd")).Text),
                    ((RadTextBox)footerItem.FindControl("txtDescriptionAdd")).Text
                );
                ((RadTextBox)footerItem.FindControl("txtDescriptionAdd")).Focus();
                BindData();
                gvBulkPOPartPaid.Rebind();

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidOrderPartPaid(
                    ((UserControlDecimal)e.Item.FindControl("txtAmountEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string strorderpartpaidid = ((Label)e.Item.FindControl("lblOrderPartPaidIdEdit")).Text;

                Guid orderpartpaidid = new Guid(strorderpartpaidid);

                UpdateOrderPartPaid(
                    orderpartpaidid,
                     orderid,
                    decimal.Parse(((UserControlDecimal)e.Item.FindControl("txtAmountEdit")).Text),
                    ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text
                 );
                gvBulkPOPartPaid.SelectedIndexes.Clear();
                BindData();
                gvBulkPOPartPaid.Rebind();

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteOrderPartPaid(new Guid(((Label)e.Item.FindControl("lblOrderPartPaidId")).Text));
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                ApprovedRequestAdvance(new Guid(((Label)e.Item.FindControl("lblOrderPartPaidId")).Text));
                BindData();
                gvBulkPOPartPaid.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBulkPOPartPaid_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvBulkPOPartPaid_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList(null, 'yes', 'yes');";
            Script += "</script>" + "\n";

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            
            if (!IsValidOrderPartPaid(
                    ((UserControlDecimal)eeditedItem.FindControl("txtAmountEdit")).Text,
                    ((RadTextBox)eeditedItem.FindControl("txtDescriptionEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";
            string strorderpartpaidid = ((Label)eeditedItem.FindControl("lblOrderPartPaidIdEdit")).Text;

            Guid orderid = new Guid(strorderid);
            Guid orderpartpaidid = new Guid(strorderpartpaidid);

            UpdateOrderPartPaid(
                orderpartpaidid,
                 orderid,
                decimal.Parse(((UserControlDecimal)eeditedItem.FindControl("txtAmountEdit")).Text),
                ((RadTextBox)eeditedItem.FindControl("txtDescriptionEdit")).Text
             );
            gvBulkPOPartPaid.SelectedIndexes.Clear();
            BindData();
            gvBulkPOPartPaid.Rebind();

            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
