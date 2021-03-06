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

public partial class OptionsAlertInvoiceStatus : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string tasktype = Request.QueryString["tasktype"].ToString();
            Filter.CurrentAlertTaskType = tasktype;

            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
        GetAlertItems(Filter.CurrentAlertTaskType.ToString());
    }

    private void GetAlertItems(string tasktype)
    {
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (tasktype.Equals("14"))
        {
            gvAlertsTask.DataSource = PhoenixRegistersAlerts.InvoiceStatusAlert(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? -1 : PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            gvAlertsTask.DataBind();
        }
    }

    protected void gvAlertsTask_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvAlertsTask.SelectedIndex = -1;
        gvAlertsTask.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        GetAlertItems(Filter.CurrentAlertTaskType.ToString());
    }

    protected void gvAlertsTask_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        string tasktype = Request.QueryString["tasktype"].ToString();

        if (int.Parse(tasktype) == 14 )
        {
            int nRow = int.Parse(e.CommandArgument.ToString());
            GridView _gridView = (GridView)sender;

            //PhoenixRegistersAlerts.AlertViewHistoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            //    int.Parse(tasktype), ((Label)_gridView.Rows[nRow].FindControl("lblTaskKey")).Text);

            NameValueCollection nvc = new NameValueCollection();
            string invoicenumber = ((Label)_gridView.Rows[nRow].FindControl("lblInvoiceNumber")).Text;
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

    protected void gvAlertsTask_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
