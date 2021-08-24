using System;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Common;
using System.Data;

public partial class Dashboard_DashboardBlank : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Request.QueryString["display"] != null)
        //    return;

        try
        {
            bool flag = true;
            string url = "";

            if (PhoenixSecurityContext.CurrentSecurityContext.PasswordChanged == 0)
            {
                lnkPasswordChange.Visible = true;
                flag = false;
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
            {
                DataTable dt = PhoenixDashboardOption.VesselCrewSignonDetailsEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    if (General.GetNullableInteger(dr["FLDSIGNONOFFID"].ToString()) != null)
                    {
                        ViewState["FLDEMPLOYEEID"] = dr["FLDEMPLOYEEID"].ToString();
                        int feedbackstatusyn = 0;
                        PhoenixDashboardOption.VesselCrewFeedbackStatusGet(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , General.GetNullableInteger(dr["FLDVESSELID"].ToString())
                            , General.GetNullableInteger(dr["FLDEMPLOYEEID"].ToString())
                            , General.GetNullableInteger(dr["FLDSIGNONOFFID"].ToString())
                            , ref feedbackstatusyn);

                        if (feedbackstatusyn == 0)
                        {
                            lnkVesselCrewFeedbackForm.Visible = true;
                            flag = false;
                        }
                        lnkSignoffFeedBack.Visible = false;
                    }
                }
            }
            if (flag == true)
            {
                pnlPreRequests.Visible = false;
                DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 214);

                BindUrl(ref url);

                if (ds.Tables[0].Rows[0]["FLDSHORTNAME"].ToString() == "1")
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserType.Equals(""))
                    {
                        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                        {
                            if (url != "")
                                Response.Redirect(url, false);
                        }
                        else if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == -1)
                            Response.Redirect("DashboardSupport.aspx", false);
                        else
                        {
                            if (url != "")
                                Response.Redirect(url, false);
                        }
                    }
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserType.Equals("USER"))
                    {
                        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                        {
                            if (url != "")
                                Response.Redirect(url, false);
                        }
                        else if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == -1)
                            Response.Redirect("DashboardSupport.aspx", false);
                        else
                        {
                            if (url != "")
                                Response.Redirect(url, false);
                        }
                    }
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserType.Equals("ADMIN"))
                    {
                        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                        {
                            if (url != "")
                                Response.Redirect(url, false);
                        }
                        else if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == -1)
                            Response.Redirect("DashboardSupport.aspx", false);
                        else
                        {
                            if (url != "")
                                Response.Redirect(url, false);
                        }
                    }
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserType.Equals("OWNER"))
                    {
                        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                        {
                            if (url != "")
                                Response.Redirect(url, false);
                        }
                        else if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == -1)
                            Response.Redirect("DashboardSupport.aspx", false);
                        else
                        {
                            if (url != "")
                                Response.Redirect(url, false);
                        }
                    }
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserType.Equals("SUPPLIER"))
                        Response.Redirect("DashboardSupplier.aspx", false);
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserType.Equals("NEWAPPLICANT"))
                    {
                        Filter.CurrentMenuCodeSelection = "";

                        if (Filter.CurrentNewApplicantSelection != null)
                        {
                            DataTable dt = PhoenixDashboardOption.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["FLDEMPLOYEECODE"].ToString() == "")
                                {
                                    Response.Redirect("../Crew/CrewNewApplicantPersonalGeneral.aspx", false);
                                }
                                else
                                {
                                    Response.Redirect("../Crew/CrewPersonalGeneral.aspx", false);
                                }
                            }

                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void JoningFeedback_Click(object sender, EventArgs e)
    {
        Response.Redirect("../VesselAccounts/VesselAccountsCrewFeedbackform.aspx", false);
    }
    protected void SignoffFeedback_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Crew/CrewSignoffFeedback.aspx", false);
    }
    protected void PasswordChange_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Options/OptionsChangePassword.aspx", false);
    }

    protected void BindUrl(ref string url)
    {
        DataSet dspage = new DataSet();

        dspage = PhoenixCommonDashboard.DashboardPreferencesList(PhoenixSecurityContext.CurrentSecurityContext.UserType
                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , null);

        if (dspage.Tables[0].Rows.Count > 0)
        {
            url = dspage.Tables[0].Rows[0]["FLDURL"].ToString();
        }
    }
}
