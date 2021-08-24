using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;

public partial class UserControlDashboardMenu : System.Web.UI.UserControl
{
    public string _showfilter = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
			{
				cmdFilter.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Dashboard/DashboardFilter.aspx?filter=" + ShowFilter + "'); return false;");
			}
			else
			{
				cmdFilter.Attributes.Add("class", "filterDiv glyphicon glyphicon-refresh");
				cmdFilter.Attributes.Add("title", "Refresh");
				cmdFilter.Attributes.Add("onclick", "DashboardRefresh();");

			}
			SelectedOption();
		}        
    }
    private void SelectedOption()
    {
        DataSet ds = PhoenixDashboardOption.DashboardLastSelectedEdit();
        if (ds.Tables[0].Rows.Count > 0)
        {

            DataTable dt = GetOptions(ds.Tables[0].Rows[0]["FLDSELECTEDMENU"].ToString());           
            string strOpn = ds.Tables[0].Rows[0]["FLDSELECTEDOPTION"].ToString();
            DataRow[] dr = dt.Select("FLDVALUE='" + strOpn + "'");
            if(dr.Length > 0)
            {
                spnModule.InnerHtml = dr[0]["FLDMENU"].ToString();
            }
            if (strOpn == "VAC" || strOpn == "SYE" || strOpn == "VLT" || strOpn == "SYS" || strOpn == "GMP")
                cmdFilter.Visible = false;
            else
                cmdFilter.Visible = true;
        }
    }
    private DataTable GetOptions(string DashboardMenu)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("FLDMENU");
        dt.Columns.Add("FLDVALUE");

        switch (DashboardMenu)
        {
            case "TECH":
                AddNewRow(dt, "PUR", "Purchase");
                AddNewRow(dt, "INY", "Inventory");
                AddNewRow(dt, "PMS", "Planned Maintenance");
                AddNewRow(dt, "VTG", "Vetting");
                AddNewRow(dt, "INP", "Inspection");
                AddNewRow(dt, "WRH", "Work Rest Hours");
                AddNewRow(dt, "INV", "Invoice");
                AddNewRow(dt, "SYE", "Sync Error");
                AddNewRow(dt, "VLT", "Vessel List");
                AddNewRow(dt, "SYS", "Sync Status");
                AddNewRow(dt, "GMP", "Google Map");
                AddNewRow(dt, "CSY", "Certificates and Surveys");
                AddNewRow(dt, "PERFORM", "Performance");
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                    AddNewRow(dt, "VSP", "Vessel Particulars");
                break;

            case "CREW":
                AddNewRow(dt, "APP", "Appraisals");
                AddNewRow(dt, "CFR", "Crew Follow Up & Relief");
                AddNewRow(dt, "LGL", "Crew Legal Pending");
                AddNewRow(dt, "CNC", "Crew Not Contacted");
                AddNewRow(dt, "GMP", "Google Map");
                AddNewRow(dt, "VPN", "Ice Experience");
                AddNewRow(dt, "CIN", "Invoice");
                AddNewRow(dt, "ODE", "Onboard Document Expiry Status");
                AddNewRow(dt, "SYE", "Sync Error");
                AddNewRow(dt, "SYS", "Sync Status");
                AddNewRow(dt, "TCK", "Tickler");
                AddNewRow(dt, "VLT", "Vessel List");
                AddNewRow(dt, "CREWLIST", "Crew");
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                    AddNewRow(dt, "VSP", "Vessel Particulars");
                break;

            case "QUAL":
                AddNewRow(dt, "SYE", "Sync Error");
                AddNewRow(dt, "VLT", "Vessel List");
                AddNewRow(dt, "SYS", "Sync Status");
                AddNewRow(dt, "GMP", "Google Map");
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                    AddNewRow(dt, "VSP", "Vessel Particulars");
                break;

            case "ACCT":
                AddNewRow(dt, "VAC", "Vessel Accounting");
                AddNewRow(dt, "SYE", "Sync Error");
                AddNewRow(dt, "VLT", "Vessel List");
                AddNewRow(dt, "SYS", "Sync Status");
                AddNewRow(dt, "GMP", "Google Map");
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                    AddNewRow(dt, "VSP", "Vessel Particulars");
                break;

            default:
                break;
        }
        return dt;
    }

    private void AddNewRow(DataTable dt, string value, string text)
    {
        DataRow dr = dt.NewRow();
        dr["FLDMENU"] = text;
        dr["FLDVALUE"] = value;
        dt.Rows.Add(dr);
    }

    public string ShowFilter
    {
        set
        {
            _showfilter = value;
        }
        get
        {
            return _showfilter;
        }
    }

}
