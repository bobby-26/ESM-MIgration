using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;


public partial class Owners_OwnersReportSignature : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    string popupname = string.Empty;
    string popuptitle = string.Empty;
    string userdesignation = string.Empty;
    string remarks = string.Empty;
    DateTime selectedDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = int.Parse(Filter.SelectedOwnersReportVessel);

        if (string.IsNullOrWhiteSpace(Request.QueryString["popuptitle"]) == false)
        {
            popuptitle = Request.QueryString["popuptitle"];
            Page.Title = popuptitle;
        }

        if (Filter.SelectedOwnersReportDate != null)
        {
            selectedDate = Convert.ToDateTime(Filter.SelectedOwnersReportDate);
        }
        else
        {
            selectedDate = DateTime.Now;
        }



        if (string.IsNullOrWhiteSpace(Request.QueryString["designation"]) == false)
        {
            userdesignation= Request.QueryString["designation"];
        }

        if (string.IsNullOrWhiteSpace(Request.QueryString["remarks"]) == false)
        {
            remarks = Request.QueryString["remarks"];
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string script = "closeTelerikWindow('engineersign')";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUpClose", script, true);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            if (isValidSignInput() == false)
            {
                ucError.Visible = true;
                return;
            }

            if (CheckUserLogin() == false)
            {
                throw new ArgumentException("Please enter a valid password");
            }



            ds = PhoenixElog.UserDetails(usercode);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(Request.QueryString["popupname"]) == false)
                {
                    popupname = Request.QueryString["popupname"];
                }
                DataRow dr = ds.Tables[0].Rows[0];
                string name = dr["FLDFIRSTNAME"].ToString() + " " + dr["FLDMIDDLENAME"].ToString() + " " + dr["FLDLASTNAME"].ToString();

                // Get logged in vessel supt, marine, fleet
                DataTable dt = PhoenixOwnerReport.VesselInchargeSearch(usercode, vesselId);
                if (dt.Rows.Count == 0)
                {
                    if (userdesignation != "OWNER")
                    {
                        throw new ArgumentException("Technical , Marine SuperIntedent  or Fleet Manager is not configured on vessel Master Correctly. Please Configure and try again");
                    }
                }

                DataRow row = dt.Rows[0];
                int suptUserId = Convert.ToInt32(row["FLDSUPT"] == DBNull.Value ? 0 : row["FLDSUPT"]);
                int marineSuptUserId = Convert.ToInt32(row["FLDMARINESUPT"] == DBNull.Value ? 0 : row["FLDMARINESUPT"]);
                int fleetMagerUserId = Convert.ToInt32(row["FLDFLEETMANAGER"] == DBNull.Value ? 0 : row["FLDFLEETMANAGER"]);

                if ((suptUserId == 0 || marineSuptUserId == 0 || fleetMagerUserId == 0) && (userdesignation != "OWNER"))
                {
                    throw new ArgumentException("Technical , Marine SuperIntedent  or Fleet Manager is not configured on vessel Master Correctly. Please Configure and try again");
                }
                
                // compare then with designation and user code from db
                if (userdesignation == "TECHNICAL" && usercode == suptUserId)
                {
                    PhoenixOwnerReport.OwnersReportRemarksInsert(vesselId, usercode, name, remarks, selectedDate);
                    ClosePopup();
                }
                else if (userdesignation == "MARINE" && usercode == marineSuptUserId)
                {
                    PhoenixOwnerReport.OwnersReportRemarksUpdate(vesselId, usercode, name, remarks, selectedDate, userdesignation);
                    ClosePopup();
                }
                else if (userdesignation == "FLEETMANAGER" && usercode == fleetMagerUserId)
                {
                    PhoenixOwnerReport.OwnersReportRemarksUpdate(vesselId, usercode, name, remarks, selectedDate, userdesignation);
                    ClosePopup();
                }

                else if (userdesignation == "OWNER")
                {
                     PhoenixOwnerReport.OwnersReportOwnerRemarksUpdate(vesselId, usercode, name, remarks, selectedDate, userdesignation);
                     ClosePopup();
                }
                // if yes close popup and proceed
                // else throw error
                else
                {
                    string errorMessage = string.Format("Only valid user can able to sign", userdesignation);
                    throw new ArgumentException(errorMessage);
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClosePopup()
    {
        string script = string.Format("fnReloadList('codehelp1', 'ifMoreInfo', null);", popupname);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUpClose", script, true);
    }

    private bool CheckUserLogin()
    {
        bool isLoggedIn = false;
        DataSet userData = PhoenixElog.UserLogin(PhoenixSecurityContext.CurrentSecurityContext.UserName, txtPassword.Text);

        if (userData != null && userData.Tables[0].Rows.Count > 0)
        {
            isLoggedIn = true;
        }
        return isLoggedIn;
    }

    private string GetRankName(int usercode)
    {
        DataSet ds = PhoenixElog.GetSeaFarerRankName(usercode);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0]["FLDRANKCODE"].ToString();
        }
        return "";
    }

    private bool isValidSignInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            ucError.ErrorMessage = "Password is required";
        }

        return (!ucError.IsError);
    }
}