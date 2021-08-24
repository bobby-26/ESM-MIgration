using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderRAList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["WORKORDERID"] = null;
            if (Request.QueryString["WORKORDERID"] != null)
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
            ViewState["VESSELID"] = "0";
            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"];
            else
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        BindData();
    }

    private void BindData()
    {

		DataTable dt = new DataTable();
        dt = PhoenixPlannedMaintenanceWorkOrderReport.ListWorkOrderRA(int.Parse(ViewState["VESSELID"].ToString()), new Guid(ViewState["WORKORDERID"].ToString()));

        if (dt.Rows.Count > 0)
        {
			RadGrid1.DataSource = dt;
			
        }
        else
        {
            RadGrid1.DataSource = "";
        }

    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            ImageButton RAjob = (ImageButton)e.Item.FindControl("RAjob");
            if (RAjob != null)
            {
                if (drv["FLDISNEWRA"].ToString() == "1")
                    RAjob.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERYNEW&machineryid=" + drv["FLDRAID"] + " & showmenu=0&showexcel=NO'); return false;");
                else
                    RAjob.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + drv["FLDRAID"] + " & showmenu=0&showexcel=NO'); return false;");
            }
        }
    }
}

