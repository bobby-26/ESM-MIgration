using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;

public partial class OwnersPurchaseOrderForm : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Owners/OwnersPurchaseOrderForm.aspx", "Export to Excel", "<i class=\"fa fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvFormDetails')", "Print Grid", "<i class=\"fa fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Owners/OwnersPurchaseOrderForm.aspx", "Filter", "<i class=\"fa fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Owners/OwnersPurchaseOrderForm.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucFormStatus.HardTypeCode = ((int)PhoenixHardTypeCode.FORMSTATUS).ToString();
                gvFormDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDSTOCKTYPE", "FLDCREATEDDATE", "FLDFORMTYPENAME", "FLDSTATUS", "FLDVENDORDELIVERYDATE" };
        string[] alCaptions = { "Number", "Form Title", "Type", "Created Date", "Form Type", "Status", "Received Date" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentOwnerPurchaseFilter;
        if (nvc != null)
        {
            ds = PhoenixOwnersPurchase.OwnersPurchaseSearch(General.GetNullableInteger(nvc.Get("ucVessel")), General.GetNullableDateTime(nvc.Get("createddate"))
                                    , General.GetNullableDateTime(nvc.Get("createdtodate")), General.GetNullableInteger(nvc.Get("ucFormType"))
                                    , General.GetNullableString(nvc.Get("txtFormType")), General.GetNullableInteger(nvc.Get("ucStatus")), sortexpression
                                    , sortdirection, (int)ViewState["PAGENUMBER"], iRowCount, ref iRowCount, ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixOwnersPurchase.OwnersPurchaseSearch(null, null, null, null, null, null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                                                            , iRowCount, ref iRowCount, ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=OrderForm.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Purchase order form</center></h3></td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void Rebind()
    {

        gvFormDetails.SelectedIndexes.Clear();
        gvFormDetails.EditIndexes.Clear();
        gvFormDetails.DataSource = null;
        gvFormDetails.Rebind();
    }
    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("createddate", txtCreatedDate.Text);
                criteria.Add("createdtodate", txtCreatedToDate.Text);
                criteria.Add("ucFormType", ucHard.SelectedHard);
                criteria.Add("txtFormType", txtFormNumber.Text);
                criteria.Add("ucStatus", ucFormStatus.SelectedHard);
                Filter.CurrentOwnerPurchaseFilter = criteria;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                NameValueCollection criteria = new NameValueCollection();
                Filter.CurrentOwnerPurchaseFilter = null;
                ucVessel.SelectedVessel = "";
                txtCreatedDate.Text = "";
                txtCreatedToDate.Text = "";
                ucHard.SelectedHard = "";
                txtFormNumber.Text = "";
                ucFormStatus.SelectedHard = "";
                Rebind();
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
        //int vesselid = -1;
        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDSTOCKTYPE", "FLDCREATEDDATE", "FLDFORMTYPENAME", "FLDSTATUS", "FLDVENDORDELIVERYDATE" };
        string[] alCaptions = { "Number", "Form Title", "Type", "Created Date", "Form Type", "Status", "Received Date" };
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

        NameValueCollection nvc = Filter.CurrentOwnerPurchaseFilter;
        if (nvc != null)
        {
            ds = PhoenixOwnersPurchase.OwnersPurchaseSearch(General.GetNullableInteger(nvc.Get("ucVessel")),
                                    General.GetNullableDateTime(nvc.Get("createddate")), General.GetNullableDateTime(nvc.Get("createdtodate")),
                                    General.GetNullableInteger(nvc.Get("ucFormType")), General.GetNullableString(nvc.Get("txtFormType")), General.GetNullableInteger(nvc.Get("ucStatus")), sortexpression, sortdirection,
                                    (int)ViewState["PAGENUMBER"], gvFormDetails.PageSize, ref iRowCount, ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixOwnersPurchase.OwnersPurchaseSearch(null, null, null, null, null, null, sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"], gvFormDetails.PageSize, ref iRowCount, ref iTotalPageCount);
        }
        gvFormDetails.DataSource = ds;
        gvFormDetails.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvFormDetails", "Order Form List", alCaptions, alColumns, ds);
    }
   

    protected void gvFormDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            //  LinkButton db = (LinkButton)e..FindControl("cmdDelete");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblStockItemCode");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("uclblStockItemCodeTT");
            if (lbtn != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbtn.ClientID;
            }

            lbtn = (RadLabel)e.Item.FindControl("lblVendorName");
            uct = (UserControlToolTip)e.Item.FindControl("ucVendorNameTT");
            if (lbtn != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbtn.ClientID;
            }

            RadLabel orderid = (RadLabel)e.Item.FindControl("lblOrderid");
            RadLabel vesselid = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel type = (RadLabel)e.Item.FindControl("lblType");
            RadLabel stocktype = (RadLabel)e.Item.FindControl("lblStockType");
            LinkButton imglineitems = (LinkButton)e.Item.FindControl("cmdLineItems");
            if (imglineitems != null)
            {
                imglineitems.Attributes.Add("onclick", "javascript:openNewWindow('Requisition','','" + Session["sitepath"] + "/Owners/OwnersPurchaseRequisitionDetails.aspx?orderid=" + orderid.Text + "&vesselid=" + vesselid.Text + "&type=" + type.Text + "'); return false;");
            }

            LinkButton imgPo = (LinkButton)e.Item.FindControl("cmdPO");

            if (drv["FLDFORMTYPE"].ToString() != "60")
            {
                imgPo.Visible = false;
            }
            else
            {
                imgPo.Attributes.Add("onclick", "javascript:openNewWindow('PO','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=3&reportcode=PURCHASEORDERFORM&frmowner=yes&orderid=" + orderid.Text + "&quotationid=&showactual=0&stocktype=" + stocktype.Text + "'); return false;");
            }
        }
    }
    protected void gvFormDetails_ItemCommand(object sender, GridCommandEventArgs e)
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
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFormDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFormDetails.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
}
