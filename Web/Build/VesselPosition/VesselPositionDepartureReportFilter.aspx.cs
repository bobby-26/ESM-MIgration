using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class VesselPosition_VesselPositionDepartureReportFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();     
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        DepartureReportFilterMain.AccessRights = this.ViewState;
        DepartureReportFilterMain.MenuList = toolbar.Show();

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
                SetFeet();
                if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
                {
                    ddlVessel.Enabled = false;
                    ddlFleet.Enabled = false;
                }
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVoyage.VesselId = General.GetNullableInteger(ViewState["VESSELID"].ToString());
            }
            else
            {
                ViewState["VESSELID"] = ddlVessel.SelectedVessel;
                ucVoyage.VesselId = 0;
            }
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
        NameValueCollection nvc;
        if (Request.QueryString["type"] == "departure")
        {
             nvc = Filter.CurrentDepartureReportFilter;
        }
        else
        {
            nvc = Filter.CurrentShiftingReportFilter;
        }
      
        if (nvc != null && !IsPostBack)
        {
            txtReportFrom.Text = (nvc.Get("txtReportFrom") == null) ? "" : nvc.Get("txtReportFrom").ToString();
            txtReportTo.Text = (nvc.Get("txtReportTo") == null) ? "" : nvc.Get("txtReportTo").ToString();
            UcCurrentPort.SelectedValue = (nvc.Get("UcCurrentPort") == null) ? "" : nvc.Get("UcCurrentPort").ToString();
            UcNextPort.SelectedValue = (nvc.Get("UcNextPort") == null) ? "" : nvc.Get("UcNextPort").ToString();
            ddlMonth.SelectedValue = (nvc.Get("ddlMonth") == null) ? "" : nvc.Get("ddlMonth").ToString();
            ddlYear.SelectedValue = (nvc.Get("ddlYear") == null) ? "" : nvc.Get("ddlYear").ToString();
            ucVoyage.SelectedVoyage = (nvc.Get("ucVoyage") == null) ? "" : nvc.Get("ucVoyage").ToString();
            //if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
            {
                ddlVessel.SelectedVessel = (nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString();
                ddlFleet.SelectedValue = (nvc.Get("ddlFleet") == null) ? "" : nvc.Get("ddlFleet").ToString();
            }
         

        }
    }

    protected void DepartureReportFilterMain_TabStripCommand(object sender, EventArgs e)
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
            criteria.Add("UcCurrentPort", UcCurrentPort.SelectedValue);
            criteria.Add("UcNextPort", UcNextPort.SelectedValue);
            criteria.Add("ddlMonth", ddlMonth.SelectedValue);
            criteria.Add("ddlYear", ddlYear.SelectedValue);
            criteria.Add("ucVoyage", ucVoyage.SelectedVoyage);
            if (Request.QueryString["LaunchFromDB"] != null)
                criteria.Add("LaunchFromDB", "1");

            if (Request.QueryString["type"] == "departure")
            {
                Filter.CurrentDepartureReportFilter = criteria;
            }
            else
            {
                Filter.CurrentShiftingReportFilter = criteria;
            }
          
        }
        if (Request.QueryString["LaunchFromDB"] != null)
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Filter', 'VesselPosition');", true);
        else
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        ucVoyage.VesselId = General.GetNullableInteger(ddlVessel.SelectedVessel);
        ucVoyage.bind();
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
