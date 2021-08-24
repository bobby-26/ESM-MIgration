using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewCostEvaluationChatQuoteDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SESSIONID"] = null;

                if (Request.QueryString["REQUESTID"] != null)
                {
                    ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();
                }
                if (Request.QueryString["QUOTEID"] != null)
                {
                    ViewState["QUOTEID"] = Request.QueryString["QUOTEID"].ToString();
                }
                ViewState["PAGENUMBER"] = 1;

                gvLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewCostEvaluationQuote.CrewCostQuoteLineItemSearch(new Guid(ViewState["REQUESTID"].ToString())
           , General.GetNullableGuid(ViewState["QUOTEID"] == null ? "" : ViewState["QUOTEID"].ToString())
           , (int)ViewState["PAGENUMBER"], gvLineItem.PageSize, ref iRowCount, ref iTotalPageCount);

        gvLineItem.DataSource = ds;
        gvLineItem.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }


    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
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

    private decimal TotalAmount = 0;

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            decimal.TryParse(drv["FLDTOTALAMOUNT"].ToString(), out TotalAmount);
        }

        if (e.Item is GridFooterItem)
        {
            GridFooterItem footer = (GridFooterItem)e.Item;
            (footer["AMOUNT"].FindControl("lblTotalA") as RadLabel).Text = TotalAmount.ToString();
            
        }
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

}
