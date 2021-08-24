using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class VesselPosition_VesselPositionNoonReportListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        NoonReportFilterMain.AccessRights = this.ViewState;
        NoonReportFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            for (int i = 2005; i <= DateTime.Now.Year; i++)
            {
                ddlYear.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
            }
            ddlYear.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            BindVesselFleetList();
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
                {
                    ddlVessel.Enabled = false;
                    ddlFleet.Enabled = false;
                }
            }
            SetFeet();
        }
    }
    private void SetFeet()
    {
        if (General.GetNullableInteger(ddlVessel.SelectedVessel) != null)
        {
            DataSet ds = PhoenixRegistersVessel.EditVesselCrewRelatedInfo(Convert.ToInt32(ddlVessel.SelectedVessel));
            DataTable dt = ds.Tables[0];
            DataRow dr = dt.Rows[0];
            ddlFleet.SelectedValue = dr["FLDTECHFLEET"].ToString();
        }
        else
        {
            ddlFleet.SelectedIndex = 0;
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {

        NameValueCollection nvc = Filter.CurrentNoonReportListFilter;
        if (nvc != null && !IsPostBack)
        {
            //if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
            //{
                ddlVessel.SelectedVessel = (nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString();
                ddlFleet.SelectedValue = (nvc.Get("ddlFleet") == null) ? "" : nvc.Get("ddlFleet").ToString();
            //}
            txtReportFrom.Text = (nvc.Get("txtReportFrom") == null) ? "" : nvc.Get("txtReportFrom").ToString();
            txtReportTo.Text = (nvc.Get("txtReportTo") == null) ? "" : nvc.Get("txtReportTo").ToString();
            UcPortfrom.SelectedValue = (nvc.Get("UcPortfrom") == null) ? "" : nvc.Get("UcPortfrom").ToString();
            UcPortTo.SelectedValue = (nvc.Get("UcPortTo") == null) ? "" : nvc.Get("UcPortTo").ToString();
            ddlMonth.SelectedValue = (nvc.Get("ddlMonth") == null) ? "" : nvc.Get("ddlMonth").ToString();
            ddlYear.SelectedValue = (nvc.Get("ddlYear") == null) ? "" : nvc.Get("ddlYear").ToString();
            txtETAFrom.Text = (nvc.Get("txtETAFrom") == null) ? "" : nvc.Get("txtETAFrom").ToString();
            txtETATo.Text = (nvc.Get("txtETATo") == null) ? "" : nvc.Get("txtETATo").ToString();
         
       }
    }

    protected void NoonReportFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";
        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
            criteria.Add("ddlFleet", ddlFleet.SelectedValue);
            criteria.Add("txtReportFrom", txtReportFrom.Text);
            criteria.Add("txtReportTo", txtReportTo.Text);
            criteria.Add("UcPortfrom", UcPortfrom.SelectedValue);
            criteria.Add("UcPortTo", UcPortTo.SelectedValue);
            criteria.Add("ddlMonth", ddlMonth.SelectedValue);
            criteria.Add("ddlYear", ddlYear.SelectedValue);
            criteria.Add("txtETAFrom", txtETAFrom.Text);
            criteria.Add("txtETATo", txtETATo.Text);
            if (Request.QueryString["LaunchFromDB"] != null)
                criteria.Add("LaunchFromDB", "1");

            Filter.CurrentNoonReportListFilter = criteria;
        }

        if (Request.QueryString["LaunchFromDB"] != null)
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Filter', 'VesselPosition');", true);
        else
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        SetFeet();
    }
    private void BindVesselFleetList()
    {
        DataSet ds = PhoenixRegistersFleet.ListFleet();
        ddlFleet.Items.Add("select");
        ddlFleet.DataSource = ds;
        ddlFleet.DataTextField = "FLDFLEETDESCRIPTION";
        ddlFleet.DataValueField = "FLDFLEETID";
        ddlFleet.DataBind();
        ddlFleet.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
