﻿using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;

public partial class Log_ElectricLogDutyEngineerSignature : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    string popupname = string.Empty;
    string popuptitle = string.Empty;
    bool isMissedOperation = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (string.IsNullOrWhiteSpace(Request.QueryString["popuptitle"]) == false)
        {
            popuptitle = Request.QueryString["popuptitle"];
            Page.Title = popuptitle;
        }

        if (string.IsNullOrWhiteSpace(Request.QueryString["isMissedOperation"]) == false)
        {
            isMissedOperation = Convert.ToBoolean(Request.QueryString["isMissedOperation"]);
            
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

                string rank = GetRankName(usercode);
                bool isDutyEngineer = PhoenixElog.validDutyEngineer(rank);
                bool isCheifEngineer = PhoenixElog.validCheifEngineer(rank);


                if (isDutyEngineer || isCheifEngineer)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    string name = dr["FLDFIRSTNAME"].ToString() + " " + dr["FLDMIDDLENAME"].ToString();
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("id", dr["FLDUSERCODE"].ToString());
                    nvc.Add("rank", rank);
                    nvc.Add("name", name);
                    nvc.Add("date", DateTime.Now.ToString());
                    //nvc.Add("isChiefEnginner", isCheifEngineer.ToString());
                    nvc.Add("isDutyEngineer", isDutyEngineer.ToString());
                    Filter.DutyEngineerSignatureFilterCriteria = nvc;
                    string script = string.Format("closeTelerikWindow('engineersign', '{0}')", popupname);
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUpClose", script, true);
                    //string script = isMissedOperation ? string.Format("closeTelerikWindow('engineersign', '{0}', '' ,'iframe')", popupname) : string.Format("closeTelerikWindow('engineersign', '{0}')", popupname);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUpClose", script, true);

                } else
                {
                    throw new ArgumentException("Only valid duty engineer can able to sign");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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