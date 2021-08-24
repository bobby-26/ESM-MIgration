using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewContractPersonal : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Contract Information", "CONTRACTINFORMATION");
            toolbar.AddButton("CBA ", "CBACOMPONENT");
            toolbar.AddButton("Standard ", "STANDARDCOMPONENT");
            toolbar.AddButton("Crew Agreed", "CREWAGREECOMPONENT");
            toolbar.AddButton("Contract Letter", "CONTRACT");
            toolbar.AddButton("Back", "LIST");

            MenuCrewContract.AccessRights = this.ViewState;
            MenuCrewContract.MenuList = toolbar.Show();
            MenuCrewContract.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                EditContractDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditContractDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EditCrewContractDetails(new Guid(Request.QueryString["Contractid"].ToString()));
            if (dt.Rows.Count > 0)
            {
                ViewState["EMPLOYEEID"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                txtunion.Text = dt.Rows[0]["FLDCBAAPPLIED"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtMiddlename.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtSeamanBook.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
                txtNationality.Text = dt.Rows[0]["FLDNATIONALITYNAME"].ToString();
                txtVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                txtAddress.Text = dt.Rows[0]["FLDADDRESS"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtPBDate.Text = dt.Rows[0]["FLDPBDATE"].ToString();
                txtCompany.Text = dt.Rows[0]["FLDCOMPANYNAME"].ToString();
                txtCBARevision.Text = dt.Rows[0]["FLDREVISIONNODESC"].ToString();
                txtSeniority.Text = dt.Rows[0]["FLDSCALENAME"].ToString();
                txtSeaPort.Text = dt.Rows[0]["FLDSEAPORTNAME"].ToString();
                txtDate.Text = dt.Rows[0]["FLDPAYDATE"].ToString();
                txtContractPeriod.Text = dt.Rows[0]["FLDCONTRACTTENURE"].ToString();
                txtPlusMinusPeriod.Text = dt.Rows[0]["FLDPLUSMINUSPERIOD"].ToString();
                txtRankMonth.Text = (int.Parse(dt.Rows[0]["FLDRANKEXPERIENCE"].ToString()) / 12).ToString();

                if (dt.Rows[0]["FLDBPPOOL"].ToString() == "1")
                {
                    ViewState["APP"] = dt.Rows[0]["FLDNEXTINCREMENTDATE"].ToString() == string.Empty ? "0" : "1";
                    txtNextIncrementDate.Text = dt.Rows[0]["FLDNEXTINCREMENTDATE"].ToString();
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewContract_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("CONTRACTINFORMATION"))
            {
                Response.Redirect("CrewContractPersonal.aspx?Contractid=" + Request.QueryString["Contractid"].ToString() + "&empid = " + Request.QueryString["empid"] , false);
            }
            if (dce.CommandName.ToUpper().Equals("CBACOMPONENT"))
            {
                Response.Redirect("CrewContractCBAdetails.aspx?Contractid=" + Request.QueryString["Contractid"].ToString() + "&empid = " + Request.QueryString["empid"] , false);
            } if (dce.CommandName.ToUpper().Equals("STANDARDCOMPONENT"))
            {
                Response.Redirect("CrewContractStandardComponent.aspx?Contractid=" + Request.QueryString["Contractid"].ToString() + "&empid = " + Request.QueryString["empid"], false);
            } if (dce.CommandName.ToUpper().Equals("CREWAGREECOMPONENT"))
            {
                Response.Redirect("CrewContractAgreedComponent.aspx?Contractid=" + Request.QueryString["Contractid"].ToString() + "&empid = " + Request.QueryString["empid"], false);
            } if (dce.CommandName.ToUpper().Equals("CONTRACT"))
            {
                Response.Redirect("CrewContractDetails.aspx?Contractid=" + Request.QueryString["Contractid"].ToString()+ "&empid = " + Request.QueryString["empid"], false);
            } if (dce.CommandName.ToUpper().Equals("LIST"))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                    Response.Redirect("../Crew/CrewContractHistory.aspx?empid=" + Request.QueryString["empid"].ToString(), false);
                else
                    Response.Redirect("../Crew/CrewContractHistory.aspx", false);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
