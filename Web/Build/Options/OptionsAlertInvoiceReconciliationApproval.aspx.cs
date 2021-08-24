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
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;

public partial class OptionsAlertInvoiceReconciliationApproval : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string tasktype = Request.QueryString["tasktype"].ToString();
            Filter.CurrentAlertTaskType = tasktype;

            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["PAGENUMBER"] = 1;
        }
        GetAlertItems(Filter.CurrentAlertTaskType.ToString());
    }

    private void GetAlertItems(string tasktype)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null; 

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixRegistersAlerts.UserEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode);

        if (tasktype.Equals("17"))
        {
            if (ds.Tables[0].Rows[0]["FLDDEPARTMENTID"].ToString().Equals("8"))
            {
                gvAlertsInvoiceReconciliation.DataSource = PhoenixRegistersAlerts.InvoiceApprovedForSupdtOrPurchaser(
                            General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 60, "RWA"))
                            , null, General.GetNullableString(PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString())
                            , sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
                gvAlertsInvoiceReconciliation.DataBind();
            }
        }
        else if (tasktype.Equals("18"))
        {
            gvAlertsInvoiceReconciliation.DataSource = PhoenixRegistersAlerts.InvoiceReconciliationStatusAlert(
                        General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 60, "OMA"))
                        , sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            gvAlertsInvoiceReconciliation.DataBind();
        }
        else if (tasktype.Equals("28"))
        {
            if (ds.Tables[0].Rows[0]["FLDDEPARTMENTID"].ToString().Equals("6"))
            {
                gvAlertsInvoiceReconciliation.DataSource = PhoenixRegistersAlerts.InvoiceApprovedForSupdtOrPurchaser(
                            General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 60, "RPO"))
                            ,General.GetNullableString(PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString()),null
                            , sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
                gvAlertsInvoiceReconciliation.DataBind();
            }
        }
    }

    protected void gvAlertsInvoiceReconciliation_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvAlertsInvoiceReconciliation.SelectedIndex = -1;
        gvAlertsInvoiceReconciliation.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        GetAlertItems(Filter.CurrentAlertTaskType.ToString());
    }


    protected void gvAlertsInvoiceReconciliation_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GetAlertItems(Filter.CurrentAlertTaskType.ToString());
    }

    protected void gvAlertsInvoiceReconciliation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        string tasktype = Request.QueryString["tasktype"].ToString();

        if (int.Parse(tasktype) == 17 || int.Parse(tasktype) == 18)
        {
            int nRow = int.Parse(e.CommandArgument.ToString());
            GridView _gridView = (GridView)sender;
            
            NameValueCollection nvc = new NameValueCollection();
            string invoicenumber = ((Label)_gridView.Rows[nRow].FindControl("lblInvoiceid")).Text;
            if (!string.IsNullOrEmpty(invoicenumber))
            {
                nvc.Add("txtInvoiceNumberSearch", invoicenumber);
                Filter.CurrentInvoiceSelection = nvc;
            }

            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "parent.OpenSearchPage('Accounts/AccountsInvoiceMaster.aspx', 'ACC-IRG');";
            Script += "</script>" + "\n";

            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
    }

    protected void gvAlertsInvoiceReconciliation_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }
}
