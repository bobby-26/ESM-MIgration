using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;

public partial class Inspection_InspectionDashboardShipRiskProfile : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["Vesselid"] = null;
            ViewState["Portid"] = null;
            ViewState["Portname"] = "";
            if (!string.IsNullOrEmpty(Request.QueryString["Vesselid"]))
            {
                ViewState["Vesselid"] = Request.QueryString["Vesselid"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["Portid"]))
            {
                ViewState["Portid"] = Request.QueryString["Portid"];
            }
            if (Request.QueryString["Portname"] != null)
                ViewState["Portname"] = Request.QueryString["Portname"].ToString();
            else
                ViewState["Portname"] = "";

            BindShipRiskProfile();
        }
    }
    protected void BindShipRiskProfile()
    {
        if ((ViewState["Portid"] != null && ViewState["Vesselid"] != null))
        {
            DataSet ds = PhoenixInspectionPSCMOUMatrix.ShipRiskProfileInfoEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ViewState["Portid"].ToString()), General.GetNullableInteger(ViewState["Vesselid"].ToString()));

            if (ds.Tables[1].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[1].Rows[0];
                lblshipriskresult.Text = dr["FLDRISK"].ToString();
                if (dr["FLDRISKLEVELID"].ToString() == "3")
                {
                    lblshipriskresult.ForeColor = System.Drawing.Color.Orange;
                }
                else if (dr["FLDRISKLEVELID"].ToString() == "2")
                {
                    lblshipriskresult.ForeColor = System.Drawing.Color.ForestGreen;
                }
                else if (dr["FLDRISKLEVELID"].ToString() == "1")
                {
                    lblshipriskresult.ForeColor = System.Drawing.Color.Red;
                }
                //ucFlag.SelectedFlag = dr["FLDFLAGID"].ToString();
                txtpriority.Text = dr["FLDPRIORITY"].ToString();

                if (dr["FLDPRIORITYID"].ToString() == "1")
                {
                    txtpriority.ForeColor = System.Drawing.Color.Red;
                }
                else if (dr["FLDPRIORITYID"].ToString() == "2")
                {
                    txtpriority.ForeColor = System.Drawing.Color.Orange;
                }
                else if (dr["FLDPRIORITYID"].ToString() == "3")
                {
                    txtpriority.ForeColor = System.Drawing.Color.ForestGreen;
                }
                lbllastinspdate.Text = ds.Tables[0].Rows[0]["FLDLASTINSPECTION"].ToString();
                lblshipriskresult.PostBackUrl = "javascript:top.openNewWindow('maint','Ship Risk Profile','Inspection/InspectionPSCMOUShipRiskProfile.aspx?Portid=" + General.GetNullableInteger(ViewState["Portid"].ToString()) 
                                + "&Vesselid=" + General.GetNullableInteger(ViewState["Vesselid"].ToString()) + "&Portname=" + General.GetNullableString(ViewState["Portname"].ToString()) + "');";
                lbllastinspdate.PostBackUrl = "javascript:top.openNewWindow('maint','Inspection Log','Inspection/InspectionAuditRecordGeneral.aspx?reffrom=log&AUDITSCHEDULEID=" + General.GetNullableGuid(ds.Tables[0].Rows[0]["FLDREVIEWSCHEDULEID"].ToString()) + "');";

            }
            lblnote.Text = "<b>Note:</b> Priority I: ships must be inspected because the time window has closed. <br/> Priority II: ships may be inspected because they are within the time window of inspection.";
        }
    }

}