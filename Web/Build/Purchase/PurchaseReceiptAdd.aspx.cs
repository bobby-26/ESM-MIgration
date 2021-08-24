using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Data;

public partial class PurchaseReceiptAdd : PhoenixBasePage

{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuReceiptLineItemGeneral.AccessRights = this.ViewState;
        MenuReceiptLineItemGeneral.MenuList = toolbarmain.Show();
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            ViewState["RECEIPTID"] = null;
            ViewState["VESSELID"] = null;
            ViewState["STOCKTYPE"] = null;
            Guid receiptid = Guid.NewGuid();
            ViewState["RECEIPTID"] = General.GetNullableString(Request.QueryString["RECEIPTID"]) == null ? General.GetNullableString(receiptid.ToString()) : General.GetNullableString(Request.QueryString["RECEIPTID"]);
            gvRLine.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            if (string.IsNullOrEmpty(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) || PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() == "0")
            {
                ucVessel.Enabled = true;
            }
            else
            {
                ucVessel.Enabled = false;
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }
            binddata();
        }
        bindmenu();
    }

    private void bindmenu()
    {
        PhoenixToolbar toolbarMenu = new PhoenixToolbar();
        if (ViewState["RECEIPTID"] != null && ViewState["STOCKTYPE"]!=null)
            toolbarMenu.AddImageLink("javascript:openNewWindow('codehelp2', '', 'Purchase/PurchaseReceiptLineitemAdd.aspx?RECEIPTID=" + ViewState["RECEIPTID"] +"&STOCKTYPE="+ ViewState["STOCKTYPE"]+ "&VESSELID="+ViewState["VESSELID"]+"',false,'900','500'); return true;", "Add", "Add.png", "ADD");
        toolbarMenu.AddImageButton("../Purchase/PurchaseReceiptAdd.aspx", "export to excel", "icon_xls.png", "excel");
        toolbarMenu.AddImageLink("javascript:callprint('gvRLine')", "print grid", "icon_print.png", "print");
        MenuReceiptLineItem.AccessRights = this.ViewState;
        MenuReceiptLineItem.MenuList = toolbarMenu.Show();
    }
    protected void gvRLine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDORDERFORMNO", "FLDPARTNUMBER", "FLDNAME", "FLDORDEREDQUANTITY", "FLDRECIEVEDQUANTITY", "FLDBALANCEQUANTITY" };
        string[] alCaptions = { "PO No.", "Number", "Name", "Ordered Qty", "Received Qty", "Balance Qty" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : ViewState["SORTEXPRESSION"].ToString();


        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRLine.CurrentPageIndex + 1;

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseReceipt.ReceiptLineitemSearch(new Guid(ViewState["RECEIPTID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvRLine.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        gvRLine.DataSource = ds;
        gvRLine.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvRLine", "Receipt LineItem", alCaptions, alColumns, ds);

    }
    private void binddata()
    {
        try
        {
            DataSet ds = PhoenixPurchaseReceipt.ReceiptEdit(General.GetNullableGuid(ViewState["RECEIPTID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ucVessel.SelectedValue = int.Parse(dr["FLDVESSELID"].ToString());
                ucPort.SelectedValue = dr["FLDRECEIPTPORTID"].ToString();
                ucPort.Text = dr["FLDSEAPORTNAME"].ToString();
                ucReceiptDate.Text = dr["FLDRECEIPTDATE"].ToString();
                txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
                txtTitle.Text = dr["FLDTITLE"].ToString();
                txtReceiptNo.Text = dr["FLDRECEIPTNO"].ToString();
                ddlStockType.SelectedValue= dr["FLDSTOCKTYPE"].ToString();
                ddlStockType.Enabled = false;
                ViewState["STOCKTYPE"] = dr["FLDSTOCKTYPE"].ToString();
                ViewState["VESSELID"]= int.Parse(dr["FLDVESSELID"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRLine_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvRLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void MenuReceiptLineItem_TabStripCommand(object sender, EventArgs e)
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

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDORDERFORMNO", "FLDPARTNUMBER", "FLDNAME", "FLDORDEREDQUANTITY", "FLDRECIEVEDQUANTITY", "FLDBALANCEQUANTITY" };
        string[] alCaptions = { "PO No.", "Number", "Name", "Ordered Qty", "Received Qty", "Balance Qty" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixPurchaseReceipt.ReceiptLineitemSearch(new Guid(ViewState["RECEIPTID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvRLine.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ReceiptLineItem.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Receipt LineItem</h3></td>");
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
    protected void MenuReceiptLineItemGeneral_TabStripCommand(object sender, EventArgs e)
     {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                //Guid? invoiceid = null;
                if (!IsValidValues(General.GetNullableString(ddlStockType.SelectedValue),General.GetNullableInteger(ucPort.SelectedValue),General.GetNullableDateTime(ucReceiptDate.Text),General.GetNullableString(txtTitle.Text.Trim())))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPurchaseReceipt.ReceiptInsertUpdate(int.Parse(ucVessel.SelectedVessel)
                                                            , new Guid(ViewState["RECEIPTID"].ToString())
                                                            , int.Parse(ucPort.SelectedValue)
                                                            , General.GetNullableString(txtTitle.Text.Trim())
                                                            , DateTime.Parse(ucReceiptDate.Text)
                                                            , General.GetNullableString(txtDescription.Text.Trim())
                                                            ,General.GetNullableString(ddlStockType.SelectedValue));
                ViewState["STOCKTYPE"] = ddlStockType.SelectedValue;
                ddlStockType.Enabled = false;
                //ViewState["RECEIPTID"] = invoiceid;
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList('codehelp1','','');";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
                bindmenu();

                binddata();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidValues(string stocktype,int? port,DateTime? receiptdate,string title)
    {
        int result;
        ucError.HeaderMessage = "Please provide the following required information";

        if (int.TryParse(ucVessel.SelectedVessel, out result) == false)
            ucError.ErrorMessage = "Vessel is required.";
        if (string.IsNullOrEmpty(stocktype))
        {
            ucError.ErrorMessage = "Stock type is required.";
        }
        if (string.IsNullOrEmpty(port.ToString()))
        {
            ucError.ErrorMessage = "Port is required.";
        }
        if (string.IsNullOrEmpty(receiptdate.ToString()))
        {
            ucError.ErrorMessage = "Receipt date is required.";
        }
        if(string.IsNullOrEmpty(title))
        {
            ucError.ErrorMessage = "Title is required.";
        }
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
}