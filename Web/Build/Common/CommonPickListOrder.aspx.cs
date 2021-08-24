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
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CommonPickListOrder : PhoenixBasePage
{
	 

	protected void Page_Load(object sender, EventArgs e)
	{
        
		if (!IsPostBack)
		{
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
            gvAddress.PageSize = General.ShowRecords(null);

            if ((Request.QueryString["ispopup"] != null) && (Request.QueryString["ispopup"] != ""))
                ViewState["ISPOPUP"] = Request.QueryString["ispopup"].ToString();
            else
                ViewState["ISPOPUP"] = string.Empty;
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

            int? vesselid = null;
            if (Request.QueryString["VESSELID"] != null)
                vesselid = Int32.Parse(Request.QueryString["VESSELID"].ToString());

            string stocktype = (Request.QueryString["STOCKTYPE"] == null) ? null : (Request.QueryString["STOCKTYPE"].ToString());
            string formtype = (Request.QueryString["FORMTYPE"] == null) ? null : (Request.QueryString["FORMTYPE"].ToString());

            ds = PhoenixCommonPurchase.OrderFormSearchForCommonpickList(vesselid
                        , txtOrderNumber.Text
                        , txtTitle.Text
                        , formtype
                        , General.GetNullableDateTime(ucFromDate.Text)
                        , General.GetNullableDateTime(ucToDate.Text)
                        , stocktype
                        , sortexpression
                        , sortdirection
                        , gvAddress.CurrentPageIndex+1
                        , gvAddress.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

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
        if (e.CommandName.ToUpper().Equals("SELECT") || e.CommandName.ToUpper().Equals("ROWCLICK"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                RadLabel lblOrderNumber = (RadLabel)item.FindControl("lblOrderNumber");
                nvc.Add(lblOrderNumber.ID, lblOrderNumber.Text.ToString());
                RadLabel lblTitle = (RadLabel)item.FindControl("lblTitle");
                nvc.Add(lblTitle.ID, lblTitle.Text.ToString());
                RadLabel lblOrderId = (RadLabel)item.FindControl("lblOrderId");
                nvc.Add(lblOrderId.ID, lblOrderId.Text);

            }
            else
            {

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblOrderNumber = (RadLabel)item.FindControl("lblOrderNumber");
                nvc.Set(nvc.GetKey(1), lblOrderNumber.Text.ToString());
                RadLabel lblTitle = (RadLabel)item.FindControl("lblTitle");
                nvc.Set(nvc.GetKey(2), lblTitle.Text.ToString());
                RadLabel lblOrderId = (RadLabel)item.FindControl("lblOrderId");
                nvc.Set(nvc.GetKey(3), lblOrderId.Text.ToString());


            }

            //Filter.CurrentPickListSelection = nvc;
            //RadScriptManager.RegisterStartupScript(Page,typeof(Page), "BookMarkScript", Script,false);
            Filter.CurrentPickListSelection = nvc;
            if (!ViewState["ISPOPUP"].ToString().Equals(""))
            {
                string[] popup = ViewState["ISPOPUP"].ToString().Split(',');
                string refreshname = popup.Length > 1 ? popup[1] : string.Empty;

                Script = "top.closeTelerikWindow('" + popup[0] + "'," + (refreshname == string.Empty ? "null" : "'" + refreshname + "'") + ");";

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, true);
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //            "BookMarkScript", "top.closeTelerikWindow('" + popup[0] + "'," + (refreshname == string.Empty ? "null" : "'" + refreshname + "'") + ");", true);
            }
            else
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
    }
}
