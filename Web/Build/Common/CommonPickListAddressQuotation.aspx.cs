using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;
public partial class CommonPickListAddressQuotation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            gvAddress.PageSize = General.ShowRecords(null);
            if ((Request.QueryString["orderid"] != null) && (Request.QueryString["orderid"] != ""))
                ViewState["orderid"] =  Request.QueryString["orderid"].ToString();
            else
                Response.Redirect("../PhoenixUnderConstruction.aspx");

            if (Request.QueryString["txtsupcode"] != null)
            {
                txtCode.Text = Request.QueryString["txtsupcode"].ToString(); 
            }
            if (Request.QueryString["txtsupname"] != null)
            {
                txtNameSearch.Text = Request.QueryString["txtsupname"].ToString();
            }

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
    }

    protected void gvAddress_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixPurchaseQuotation.QuotationAddressSearch(new Guid(ViewState["orderid"].ToString()), txtCode.Text
                , txtNameSearch.Text, sortexpression, sortdirection,
                gvAddress.CurrentPageIndex+1,
                gvAddress.PageSize,
                ref iRowCount,
                ref iTotalPageCount);
            gvAddress.DataSource = ds;
            gvAddress.VirtualItemCount = iRowCount;
            
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAddress_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        string Script = "";
        if(e.CommandName.ToUpper().Equals("SELECT")|| e.CommandName.ToUpper().Equals("ROWCLICK"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();
                RadLabel lblCode = (RadLabel)item.FindControl("lblCode");
                nvc.Add(lblCode.ID, lblCode.Text);
                LinkButton lb = (LinkButton)item.FindControl("lnkAddressName");
                nvc.Add(lb.ID, lb.Text.ToString());
                RadLabel lbl = (RadLabel)item.FindControl("lblAddressCode");
                nvc.Add(lbl.ID, lbl.Text.ToString());

            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblCode = (RadLabel)item.FindControl("lblCode");
                nvc.Set(nvc.GetKey(1), lblCode.Text);

                LinkButton lb = (LinkButton)item.FindControl("lnkAddressName");
                nvc.Set(nvc.GetKey(2), lb.Text.ToString());

                RadLabel lbl = (RadLabel)item.FindControl("lblAddressCode");
                nvc.Set(nvc.GetKey(3), lbl.Text);

                RadLabel lbquotation = (RadLabel)item.FindControl("lblQuotation");
                nvc.Set(nvc.GetKey(4), lbquotation.Text);
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page,typeof(Page), "BookMarkScript", Script,false);
        }
        
    }
}
