using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseFormTransactionHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseFormTransactionHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('rgvHistory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "Print");
            MenuOrderForm.AccessRights = this.ViewState;   
            MenuOrderForm.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;               
                ViewState["CURRENTINDEX"] = 1;
                ViewState["orderid"] = null;
                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                }
               
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
        string[] alColumns = { "FLDMODIFIEDDATE", "FLDCHANGEFIELD", "FLDOLDVALUES", "FLDNEWVALUE", "FLDMODIFIEDBY" };
        string[] alCaptions = { "Modification Date", "Changed Field", "Old Value", "New Value", "Modify By" };
   
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixPurchaseOrderForm.GetFromTransactionHistory(General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection,
           (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OrderFormHistory - " + PhoenixPurchaseOrderForm.FormNumber + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Purchase order form transaction Hisory</h3></td>");
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                rgvHistory.Rebind();
            }
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
    protected void rgvHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDMODIFIEDDATE", "FLDCHANGEFIELD", "FLDOLDVALUES", "FLDNEWVALUE", "FLDMODIFIEDBY" };
        string[] alCaptions = { "Modification Date", "Changed Field", "Old Value", "New Value", "Modify By" };
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataSet ds = new DataSet();
        ds = PhoenixPurchaseOrderForm.GetFromTransactionHistory(General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection,
             rgvHistory.CurrentPageIndex+1, rgvHistory.PageSize, ref iRowCount, ref iTotalPageCount);

        rgvHistory.DataSource = ds;
        rgvHistory.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("rgvHistory", "Form Transaction Hisory - " + PhoenixPurchaseOrderForm.FormNumber, alCaptions, alColumns, ds);
    }
    protected void rgvHistory_PreRender(object sender, EventArgs e)
    {
    }
    

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        rgvHistory.Rebind();
        if (Session["New"].ToString() == "Y")
        {
            Session["New"] = "N";
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)rgvHistory.Items[rowindex];
            ViewState["orderid"] = item.GetDataKeyValue("FLDORDERID");
            ViewState["DTKEY"] = item.GetDataKeyValue("FLDDTKEY");
            Filter.CurrentPurchaseVesselSelection = int.Parse(item.GetDataKeyValue("FLDVESSELID").ToString());

            //PhoenixPurchaseOrderForm.FormNumber = ((LinkButton)gvFormDetails.Rows[rowindex].FindControl("lnkFormNumberName")).Text;

           

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    
    

}
