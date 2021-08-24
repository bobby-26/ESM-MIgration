using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;

public partial class Owners_OwnersWorkOrderReportlogHistory :PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (Request.QueryString["WORKORDERID"] != null && Request.QueryString["WORKORDERID"].ToString() != "")
            {
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
                txtJobDetail.Content = Request.QueryString["WORKORDERID"].ToString();
                Reporthistory(ViewState["WORKORDERID"].ToString());
            }
            else
            {
                ViewState["WORKORDERID"] = "";
            }

        }
    }

    private void Reporthistory(string WORKORDERID)
    {
        DataSet ds = PhoenixOwnersPlannedMaintenance.ShowreportHistory(General.GetNullableGuid(WORKORDERID));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtJobDetail.Content = dr["FLDHISTORY"].ToString();
        }
    }  
}
