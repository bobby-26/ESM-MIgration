using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;

public partial class Dashboard : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool flag = true;

        if (PhoenixSecurityContext.CurrentSecurityContext.PasswordChanged == 0)
        {
            lnkPasswordChange.Visible = true;
            flag = false;
        }

        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0 && (PhoenixGeneralSettings.CurrentGeneralSetting.SecurityQuestion == 0 || General.GetNullableString(PhoenixGeneralSettings.CurrentGeneralSetting.SecurityAnswer) == null))
        {
            lnkSecurityQuestion.Visible = true;
            flag = false;
        }

        //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
        //{
        //    DataTable dt = PhoenixDashboardOption.VesselCrewSignonDetailsEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //        , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        //    if (dt.Rows.Count > 0)
        //    {
        //        DataRow dr = dt.Rows[0];
        //        if (General.GetNullableInteger(dr["FLDSIGNONOFFID"].ToString()) != null)
        //        {
        //            int feedbackstatusyn = 0;
        //            PhoenixDashboardOption.VesselCrewFeedbackStatusGet(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //                , General.GetNullableInteger(dr["FLDVESSELID"].ToString())
        //                , General.GetNullableInteger(dr["FLDEMPLOYEEID"].ToString())
        //                , General.GetNullableInteger(dr["FLDSIGNONOFFID"].ToString())
        //                , ref feedbackstatusyn);

        //            if (feedbackstatusyn == 0)
        //            {
        //                lnkVesselCrewFeedbackForm.Visible = true;
        //                flag = false;
        //            }
        //            lnkSignoffFeedBack.Visible = false;
        //        }
        //    }
        //}

        if (flag == true)
        {
            Screen();
        }
    }

    private void Screen()
    {
		DataTable dt = new DataTable();
        if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper().Equals("SUPPLIER") ||
            PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper().Equals("AGENT") 
           // PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper().Equals("NEWAPPLICANT")
            )
        {
            Response.Redirect("DashboardHomeBlank.aspx", false);
        }

        else if ( PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper().Equals("NEWAPPLICANT"))
        {
            Response.Redirect("DashboardHome.aspx", false);
        }

        else
        {
            string officedashboard = "DashboardOfficeV2.aspx";
            bool Is30 = Session["is3.0"].ToString() == "0" ? false : true;
            if (Is30)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                    DataTable das = new DataTable();
                    if (ds.Tables.Count > 0)
                    {
                        string dashboard = string.Empty;
                        das = ds.Tables[0];
                        if (das.Rows.Count > 0)
                            dashboard = das.Rows[0]["FLDDASHBOARDNEW"].ToString().ToUpper();
                        if (dashboard.Contains("HSEQA"))
                            officedashboard = "DashboardOfficeV2.aspx";
                        else if (dashboard.Contains("TECHNICAL"))
                            officedashboard = "DashboardOfficeV2.aspx?type=t";
                        else if (dashboard.Contains("ACCOUNTS"))
                            officedashboard = "DashboardOfficeV2Accounts.aspx";
                        else if (dashboard.Contains("CREW"))
                            officedashboard = "DashboardOfficeV2Crew.aspx";
                    }
                }

                bool handedover = false;

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    DataSet ds = PhoenixRegistersVessel.EditVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDESMHANDOVERDATE"].ToString()) != null)
                            handedover = true;
                    }
                }

                Session["currPage"] = "Dashboard/" + (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? (handedover? "DashboardVesselDetails.aspx": "DashboardVessel.aspx") : officedashboard);

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && handedover)
                    Response.Redirect("../Dashboard/DashboardVesselDetails.aspx");
                else if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                    Response.Redirect("../Dashboard/DashboardVessel.aspx");
                else
                    Response.Redirect("../Dashboard/" + officedashboard);
            }
            else
            {
                dt = PhoenixDashboardOption.DashboardPreferenceSearch();
                if (dt.Rows.Count > 0)
                {
                    string preferenceCode;
                    preferenceCode = dt.Rows[0]["FLDPREFERENCECODE"].ToString();
                    if (preferenceCode == "1")
                        Response.Redirect("DashboardVesselParticulars.aspx", false);
                    if (preferenceCode == "2")
                        Response.Redirect("DashboardHomeBlank.aspx", false);

                    if (preferenceCode != "2" && preferenceCode != "1")
                        Response.Redirect("DashboardCommon.aspx", false);
                }
                else
                {
                    Response.Redirect("DashboardCommon.aspx", false);
                }
            }
        }
    }


    protected void JoningFeedback_Click(object sender, EventArgs e)
    {
        Response.Redirect("../VesselAccounts/VesselAccountsCrewFeedbackform.aspx", false);
    }
    protected void PasswordChange_Click(object sender, EventArgs e)
    { 
        Response.Redirect("../Options/OptionsChangePassword.aspx?FROM=DASHBOARD", false);
    }
    protected void SignoffFeedback_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Crew/CrewSignoffFeedback.aspx", false);
    }

    protected void lnkSecurityQuestion_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Options/OptionsChangeSecurityQuestion.aspx?FROM=DASHBOARD", false);
    }
}
