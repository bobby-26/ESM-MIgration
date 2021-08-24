using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DashboardVesselFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO");
        toolbar.AddButton("Cancel", "CANCEL");
        if (Request.QueryString["qcalfrom"] != null && Request.QueryString["qcalfrom"] != string.Empty)
            ViewState["CALLFROM"] = Request.QueryString["qcalfrom"];
        if (Request.QueryString["qcallfrom"] != null && Request.QueryString["qcallfrom"] != string.Empty)
            ViewState["CALFROM"] = Request.QueryString["qcallfrom"];

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {           
            BindVesselFleetList();
            BindVesselList();
            BindFilterVessel();
        }
    }
 
    private void BindVesselList()
    {
		//DataSet ds = PhoenixRegistersVessel.ListVessel(null, null, 1, null, null);
		DataSet ds = PhoenixDashboardOption.DashboarVesselList();
		chkVesselList.Items.Add("select");
        chkVesselList.DataSource = ds;
        chkVesselList.DataTextField = "FLDVESSELNAME";
        chkVesselList.DataValueField = "FLDVESSELID";
        chkVesselList.DataBind();
    }

    private void BindVesselFleetList()
    {
        DataSet ds = PhoenixRegistersFleet.ListFleet();
        chkFleetList.Items.Add("select");
        chkFleetList.DataSource = ds;
        chkFleetList.DataTextField = "FLDFLEETDESCRIPTION";
        chkFleetList.DataValueField = "FLDFLEETID";
        chkFleetList.DataBind();
    }

    protected void chkFleetList_Changed(object sender, EventArgs e)
    {
        StringBuilder strfleetlist = new StringBuilder();

        foreach (ListItem item in chkFleetList.Items)
        {
            if (item.Selected == true)
            {
                strfleetlist.Append(item.Value.ToString());
                strfleetlist.Append(",");
            }
        }
        if (strfleetlist.Length > 1)
        {
            strfleetlist.Remove(strfleetlist.Length - 1, 1);
        }
        if (strfleetlist.ToString().Contains("Dummy"))
        {
            strfleetlist = new StringBuilder();
            strfleetlist.Append("0");
        }
        if (strfleetlist.ToString() == null || strfleetlist.ToString() == "")
            strfleetlist.Append("-1");

        DataSet ds = PhoenixRegistersVessel.ListFleetWiseVessel(null, null, null, null, strfleetlist.ToString() == "0" ? null : strfleetlist.ToString());

        
        foreach (ListItem item in chkVesselList.Items)
            item.Selected = false;

        if (ds.Tables[0].Rows.Count > 0)
        {
            string vesselid = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                vesselid = dr["FLDVESSELID"].ToString();
				for (int i = 0; i < chkVesselList.Items.Count; i++)
				{
					if (chkVesselList.Items[i].Value == vesselid)
					{
						chkVesselList.Items.FindByValue(vesselid).Selected = true;
					}
				}
         
            }
        }
    }

    protected void cmdSelect_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in chkVesselList.Items)
        {
            if (ddlCriteria.SelectedValue.Equals("LIKE"))
            {
                if (item.Text.ToUpper().Contains(txtFilter.Text.ToUpper().Trim()))
                {
                    item.Selected = true;
                }
            }

            if (ddlCriteria.SelectedValue.Equals("START"))
            {
                if (item.Text.ToUpper().StartsWith(txtFilter.Text.ToUpper().Trim()))
                {
                    item.Selected = true;
                }
            }

            if (ddlCriteria.SelectedValue.Equals("END"))
            {
                if (item.Text.ToUpper().EndsWith(txtFilter.Text.ToUpper().Trim()))
                {
                    item.Selected = true;
                }
            }
        }
    }


    protected void cmdClear_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in chkVesselList.Items)
        {
            if (ddlCriteria.SelectedValue.Equals("LIKE"))
            {
                if (item.Text.ToUpper().Contains(txtFilter.Text.ToUpper().Trim()))
                {
                    item.Selected = false;
                }
            }

            if (ddlCriteria.SelectedValue.Equals("START"))
            {
                if (item.Text.ToUpper().StartsWith(txtFilter.Text.ToUpper().Trim()))
                {
                    item.Selected = false;
                }
            }

            if (ddlCriteria.SelectedValue.Equals("END"))
            {
                if (item.Text.ToUpper().EndsWith(txtFilter.Text.ToUpper().Trim()))
                {
                    item.Selected = false;
                }
            }
        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
        Script += "</script>" + "\n";

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            Filter.CurrentDashboardLastSelection["VesselList"] = General.ReadCheckBoxList(chkVesselList);
            PhoenixDashboardOption.UservesselListFilterUpdate(General.GetNullableString(General.ReadCheckBoxList(chkVesselList)));
        }
        else if (dce.CommandName.ToUpper().Equals("CANCEL"))
        {

        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    private void BindFilterVessel()
    {
        DataTable dt;
        dt = PhoenixDashboardOption.UservesselListFilter();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string vesselid = "";
                vesselid = dr["FLDVESSELID"].ToString();
				for (int i = 0; i < chkVesselList.Items.Count; i++)
				{
					if (chkVesselList.Items[i].Value == vesselid)
					{
						chkVesselList.Items.FindByValue(vesselid).Selected = true;
					}
				}
				//chkVesselList.Items.FindByValue(vesselid).Selected = true;
            }
        }
    }



}



