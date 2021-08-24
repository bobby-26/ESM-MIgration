using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web;
using System.Text;
using Telerik.Web.UI;

public partial class DryDock_DrydockCostIncurred : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            Guid? Projectid = General.GetNullableGuid(Request.QueryString["projectid"]);
            int? vesselid = General.GetNullableInteger(Request.QueryString["vesselid"]);
            ViewState["ORDERID"] = General.GetNullableGuid(Request.QueryString["projectid"]);
            ViewState["VESSELID"] = General.GetNullableInteger(Request.QueryString["vesselid"]);
            Guid? Quotationid = Guid.Empty;

            PhoenixDryDockOrder.DrydockProgressQuotation(General.GetNullableGuid(ViewState["ORDERID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()), ref Quotationid);
            ViewState["QUOTATIONID"] = Quotationid;
            DataTable dt = PhoenixDryDockOrder.Drydockdetails(Projectid);
            if (dt.Rows.Count > 0)
            {
                ddlvessellist.SelectedVessel = vesselid.ToString();
                ddlvessellist.Enabled = false;
                radlblproject.Text = dt.Rows[0]["FLDTITLE"].ToString();
                radlblyard.Text = dt.Rows[0]["FLDYARD"].ToString();
                radestcost.Text = dt.Rows[0]["FLDESTIMATE"].ToString();
                if (General.GetNullableString(dt.Rows[0]["FLDYARD"].ToString()) == null)
                {
                    RadLabel1.Visible = false;
                }
                SessionUtil.PageAccessRights(this.ViewState);
               
                gvCostincurred.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
    }

    protected void gvCostincurred_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        Guid? Projectid = General.GetNullableGuid(ViewState["ORDERID"].ToString());
        DataTable dt = PhoenixDryDockOrder.DrydockCostIncurredSearch(Projectid,General.GetNullableGuid(ViewState["QUOTATIONID"].ToString()),
                                                   gvCostincurred.CurrentPageIndex + 1,
                                               gvCostincurred.PageSize,
                                               ref iRowCount,
                                               ref iTotalPageCount);




        gvCostincurred.DataSource = dt;
        gvCostincurred.VirtualItemCount = iRowCount;
    }

    protected void gvCostincurred_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                

                LinkButton date = ((LinkButton)item.FindControl("date"));
                string datetext = date.Text;
                if (date != null)
                {
                    date.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filte1','Cost Inccured','Drydock/DrydockCostIncurredDetails.aspx?orderid=" + ViewState["ORDERID"] + "&vslid="+ ViewState["VESSELID"] + "&quotationid=" + ViewState["QUOTATIONID"] + "&date="+ datetext+ "','false','800px','420px');return false");

                }

            }

            }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    protected void gvCostincurred_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
}