using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Purchase;

public partial class Purchase_PurchaseVendosPOAnalysis : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            radtodate.Text = DateTime.Now.Date.ToString();
            radfromdate.Text = DateTime.Now.AddDays(-7).ToString();
            ViewState["TOTAL"] = "";
            radstocktype.SelectedValue = "STORE";
        }
        Toolbar();
    }

    public void Toolbar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Purchase/PurchaseVendosPOAnalysis.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Purchase/PurchaseVendosPOAnalysis.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=3&reportcode=POREASONS&portid=" + ddlmulticolumnport.SelectedValue + "&fromdate=" + radfromdate.Text + "&todate=" + radtodate.Text + "&stocktype=" + radstocktype.SelectedValue + "&total="+ViewState["TOTAL"]+ "&showmenu=0&showword=NO&showexcel=NO'); return false;", "Reasons Report", "<i class=\"fas fa-file-pdf\"></i>", "REPORTS");
        toolbar.AddFontAwesomeButton("../Purchase/PurchaseVendosPOAnalysis.aspx", "Reasons Analysis as Excel", "<i class=\"fas fa-file-excel\"></i>", "REXCEL");

        MenuPhoenixQuery.MenuList = toolbar.Show();

    }
    protected void MenuPhoenixQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;



        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            if (!IsValidSearchDetails())
            {
                ucError.Visible = true;
                return;
            }
            gvQuery.Rebind();
        }

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();

        }
        if (CommandName.ToUpper().Equals("REXCEL"))
        {
            ShowReasonsExcel();

        }

    }
    protected void ShowExcel()
    {
       
        string[] alColumns = { "FLDNAME", "FLDAMOUNT", "FLDPERCENTAGE"};
        string[] alCaptions = { "Vendor", "Amount (USD)", "% of Total" };

        Decimal Total = 0;

        DataTable dt = PhoenixCommonPhoenixQuery.PhoenixPurchaseVendorPOAnalysis(General.GetNullableString(radstocktype.SelectedValue), General.GetNullableDateTime(radfromdate.Text), General.GetNullableDateTime(radtodate.Text),General.GetNullableInteger(ddlmulticolumnport.SelectedValue), ref Total);






        Response.AddHeader("Content-Disposition", "attachment; filename=Purchase Vendor PO Analysis.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Purchase Vendor PO Analysis</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("<tr><td> From </td><td>");
        Response.Write(radfromdate.Text + "</td>");
        Response.Write("<td> To </td><td>");
        Response.Write(radtodate.Text + "</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td> Port </td><td>");
        Response.Write(ddlmulticolumnport.Text + "</td>");
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
        foreach (DataRow dr in dt.Rows)
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
        Response.Write("<tr>");
        Response.Write("<td>");
        Response.Write("Total");
        Response.Write("</td>");
        Response.Write("<td>");
        Response.Write(Total.ToString());
        Response.Write("</td>");
        Response.Write("<td>");
        Response.Write("");
        Response.Write("</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.End();
    }
    private bool IsValidSearchDetails()
    {
        if (General.GetNullableDateTime(radfromdate.Text) != null & General.GetNullableDateTime(radtodate.Text) != null)
        {

            if (General.GetNullableDateTime(radfromdate.Text) > General.GetNullableDateTime(radtodate.Text))
            {

                ucError.ErrorMessage = "From date cannot be grater than to date.";
            }

        }
        

       

        return (!ucError.IsError);
    }


    protected void gvQuery_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        Decimal Total = 0;

        DataTable dt = PhoenixCommonPhoenixQuery.PhoenixPurchaseVendorPOAnalysis(General.GetNullableString(radstocktype.SelectedValue), General.GetNullableDateTime(radfromdate.Text), General.GetNullableDateTime(radtodate.Text), General.GetNullableInteger(ddlmulticolumnport.SelectedValue), ref Total);


        gvQuery.DataSource = dt;

        radtotal.Text = Total.ToString();
        ViewState["TOTAL"] = Total;
        Toolbar();
    }
    protected void ShowReasonsExcel()
    {
        

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSTOCKTYPE", "FLDPORTNAME", "FLDFORMNO", "FLDTITLE",   "FLDSUBACCOUNT", "FLDVENDORNAME", "FLDORDEREDVENDORQUOTERECEIVEDDATE", "ORDERAMOUNT", "FLDPRIORITYVENDOR",  "FLDPRIORITYVENDORQUOTERECEIVEDDATE", "FLDPRIORITYVENDORQUOTEDAMOUNT", "VENDORPRIORITY", "FLDREASON", "FLDREASONMSG","PERCENTAGE" };
        string[] alCaptions = { "Type", "Port", "Number", "Title", "Budget Code", "Vendor","Quote Received", "Amount(USD)", "Priority Vendor",  "Quote Received", "Amount(USD)", "Priority", "Reason", "Reason Message", "% of Total" };

       


        ds = PhoenixPurchasePOReasons.POReasonReportXL(
               General.GetNullableDateTime(radfromdate.Text), General.GetNullableDateTime(radtodate.Text), General.GetNullableInteger(ddlmulticolumnport.SelectedValue)
              , General.GetNullableString(radstocktype.SelectedValue)
              ,General.GetNullableDecimal(ViewState["TOTAL"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=PO Reasons Analysis.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>PO Reasons Analysis</center></h3></td>");
        Response.Write("</tr>");
        Response.Write("<tr><td> From </td><td>");
        Response.Write(radfromdate.Text + "</td>");
        Response.Write("<td> To </td><td>");
        Response.Write(radtodate.Text + "</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td> Port </td><td>");
        Response.Write(ddlmulticolumnport.Text + "</td>");
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
                if (alCaptions[i].ToUpper().Equals("FLDCREATEDDATE") || alCaptions[i].ToUpper().Equals("FLDPURCHASEAPPROVEDATE") || alCaptions[i].ToUpper().Equals("FLDORDEREDDATE") || alCaptions[i].ToUpper().Equals("FLDORDEREDVENDORQUOTERECEIVEDDATE") || alCaptions[i].ToUpper().Equals("FLDPRIORITYVENDORQUOTERECEIVEDDATE"))
                    Response.Write(General.GetDateTimeToString(dr[alColumns[i]]));
                else
                    Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
}