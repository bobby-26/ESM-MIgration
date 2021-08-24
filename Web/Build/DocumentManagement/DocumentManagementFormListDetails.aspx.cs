using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class DocumentManagement_DocumentManagementFormListDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Formid"] != null && Request.QueryString["Formid"] != "")
                ViewState["Formid"] = Request.QueryString["Formid"].ToString();

            BindData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void BindData()
    {


        string Formid;

        if (ViewState["Formid"] != null)
        {
            Formid = ViewState["Formid"].ToString();
        }
        else
        {
            Formid = null;
        }

        DataSet ds = PhoenixDocumentManagementForm.FormEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["Formid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lbltFileNo.Text = dr["FLDFILENO"].ToString();
            lbltPrimaryOwnerShip.Text = dr["FLDPRIMARYRANK"].ToString();
            lbltSecondaryOwnerShip.Text = dr["FLDSECONDARYRANK"].ToString();
            lbltOtherParticipants.Text = dr["FLDOTHERPARTICIPANTS"].ToString();
            lbltPrimaryOwnerOffice.Text = dr["FLDPRIMARYOWNEROFFICE"].ToString();
            lbltSecondaryOwnerOffice.Text = dr["FLDSECONDARYOWNEROFFICE"].ToString();            
            lbltShipDept.Text = dr["FLDSHIPDEPT"].ToString();
            lbltOfficeDept.Text = dr["FLDOFFICEDEPT"].ToString();
            lbltShipType.Text = dr["FLDSHIPTYPE"].ToString();
            lbltOfficeDept.Text = dr["FLDOFFICEDEPARTMENT"].ToString();
            lbltTimeInterval.Text = dr["FLDTIMEINTERVAL"].ToString();
            lbltcountry.Text = dr["FLDCOUNTRYPORT"].ToString();
            lbltEqMaker.Text = dr["FLDEQUIPMENTMAKER"].ToString();
            lbltJHA.Text = dr["FLDJHA"].ToString();
            lbltPMSComp.Text = dr["FLDPMSCOMPONENT"].ToString();
            lbltProcedure.Text = dr["FLDPROCEDURE"].ToString();
            lbtRA.Text = dr["FLDRA"].ToString();
            lbltActivity.Text = dr["FLDACTIVITY"].ToString();
        }
    }
}