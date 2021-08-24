using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PayRoll;

public partial class PayRoll_PayRollEmployeePersonalDetailSingapore : System.Web.UI.Page
{
    int usercode = 0;
    int vesselId = 0;
    int Id = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        if (string.IsNullOrWhiteSpace(Request.QueryString["id"]) == false)
        {
            Id = int.Parse(Request.QueryString["id"]);
        }
        ShowToolBar();

        if (IsPostBack == false)
        {
            GetEditData();
        }
    }
    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        if (Id == 0)
        {
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        }
        else
        {
            toolbarmain.AddButton("Update", "UPDATE", ToolBarDirection.Right);
        }
        gvTabStrip.MenuList = toolbarmain.Show();
    }

    private void GetEditData()
    {
        if (Id != 0)
        {
            DataSet ds = PhoenixPayRollSingapore.EmployeeSingaporeDetail(usercode, Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtFirstName.Text = dr["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dr["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dr["FLDLASTNAME"].ToString();
                txtPAN.Text = dr["FLDPANNUMBER"].ToString();
                txtEmployeeId.Text = dr["FLDEMPLOYEEID"].ToString();

                txtDoorNo.Text = dr["FLDFLATDOORBLOCKNUMBER"].ToString();
                txtPremises.Text = dr["FLDNAMEOFPREMISES"].ToString();
                txtStreet.Text = dr["FLDSTREET"].ToString();
                txtLocality.Text = dr["FLDAREALOCALITY"].ToString();

                txtTown.Text = dr["FLDTOWNCITYDISTRICT"].ToString();
                ddlCountry.SelectedCountry = "";
                ddlCountry.SelectedCountry = dr["FLDCOUNTRY"].ToString();
                ddlState.SelectedState = "";
                LoadState();

                ddlState.SelectedState = dr["FLDSTATE"].ToString();

                txtPinCode.Text = dr["FLDPINCODE"].ToString();

                txtEmail1.Text = dr["FLDEMAILADDRESS1"].ToString();
                txtEmail2.Text = dr["FLDEMAILADDRESS2"].ToString();
                txtMobileNo1.Text = dr["FLDMOBILENUMBER1"].ToString();
                txtMobileNo2.Text = dr["FLDMOBILENUMBER2"].ToString();

                txtSTDISDCode.Text = dr["FLDSTDISDCODE"].ToString();
                txtOfficeNo.Text = dr["FLDOFFICENUMBER"].ToString();
                txtResidenceNo.Text = dr["FLDRESIDENCENUMBER"].ToString();

                if (ds.Tables[0].Rows[0]["FLDISSINGAPORECITZEN"].ToString() == "1")
                    chksingapore.Checked = true;
                if (ds.Tables[0].Rows[0]["FLDISSPR"].ToString() == "1")
                    chkspr.Checked = true;
                if (ds.Tables[0].Rows[0]["FLDISSPROBTAINING"].ToString() == "1")
                    chksprobtaining.Checked = true;

            }
        }
    }
    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (IsValidReport())
            {
                ucError.Visible = true;
                return;
            }
            int state = Convert.ToInt32(ddlState.SelectedState);
            int country = Convert.ToInt32(ddlCountry.SelectedCountry);


            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixPayRollSingapore.EmployeeSingaporeInsert(usercode, txtFirstName.Text, txtMiddleName.Text, txtLastName.Text,
                    txtPAN.Text, txtDoorNo.Text, txtPremises.Text, txtStreet.Text, txtLocality.Text, txtTown.Text, state, country,
                    txtPinCode.Text, txtEmail1.Text, txtEmail2.Text, txtMobileNo1.Text, txtMobileNo2.Text, txtSTDISDCode.Text, txtOfficeNo.Text,
                    txtResidenceNo.Text, chksingapore.Checked.Equals(true) ? 1 : 0, chkspr.Checked.Equals(true) ? 1 : 0,
                    chksprobtaining.Checked.Equals(true) ? 1 : 0
                    );
            }

            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                //int employeeId = Convert.ToInt32(txtEmployeeId.Text);

                PhoenixPayRollSingapore.EmployeeSingaporeUpdate(usercode, Id, txtFirstName.Text, txtMiddleName.Text, txtLastName.Text,
                  txtPAN.Text, txtDoorNo.Text, txtPremises.Text, txtStreet.Text, txtLocality.Text, txtTown.Text, state, country,
                  txtPinCode.Text, txtEmail1.Text, txtEmail2.Text, txtMobileNo1.Text, txtMobileNo2.Text, txtSTDISDCode.Text, txtOfficeNo.Text,
                  txtResidenceNo.Text, chksingapore.Checked.Equals(true) ? 1 : 0, chkspr.Checked.Equals(true) ? 1 : 0,
                  chksprobtaining.Checked.Equals(true) ? 1 : 0

                );
            }

            ucMessage.HeaderMessage = "Saved Successfully";
            ucMessage.Visible = true;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void txtCountry_TextChangedEvent(object sender, EventArgs e)
    {
        LoadState();
    }
    private void LoadState()
    {
        ddlState.SelectedState = "";
        ddlState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlCountry.SelectedCountry));
    }
    private bool IsValidReport()
    {
        ucError.HeaderMessage = "Please add the following details";
        if (string.IsNullOrWhiteSpace(txtFirstName.Text))
        {
            ucError.ErrorMessage = "First Name is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtPAN.Text))
        {
            ucError.ErrorMessage = "Pan No. is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtDoorNo.Text))
        {
            ucError.ErrorMessage = "Door No. is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtPremises.Text))
        {
            ucError.ErrorMessage = "Premises is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtStreet.Text))
        {
            ucError.ErrorMessage = "Street is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtLocality.Text))
        {
            ucError.ErrorMessage = "Locality is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtTown.Text))
        {
            ucError.ErrorMessage = "Town is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ddlCountry.SelectedCountry))
        {
            ucError.ErrorMessage = "Country is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ddlState.SelectedState))
        {
            ucError.ErrorMessage = "State is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtPinCode.Text))
        {
            ucError.ErrorMessage = "Pincode is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtEmail1.Text))
        {
            ucError.ErrorMessage = "Email1 is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtMobileNo1.Text))
        {
            ucError.ErrorMessage = "MobileNo 1 is mandatory";
        }
        return ucError.IsError;
    }


}