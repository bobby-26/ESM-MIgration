using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PayRoll;

public partial class PayRoll_PayRollGeneral : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    int Id = 0;
   // Guid Id = Guid.Empty;
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

	 private void GetEditData()
    {
        if (Id != 0)
        {
            DataSet ds = PhoenixPayRollIndia.PayRollGeneralDetail(usercode, Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count  > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtFirstName.Text = dr["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dr["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dr["FLDLASTNAME"].ToString();
                txtPAN.Text =  dr["FLDPANNUMBER"].ToString();
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
                txtAadharNo.Text = dr["FLDAADHAARNUMBER"].ToString();
                txtEnrollmentId.Text = dr["FLDAADHAARENROLLMENTID"].ToString();

            }
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
                PhoenixPayRollIndia.PayRollGeneralInsert(usercode, txtFirstName.Text, txtMiddleName.Text, txtLastName.Text, 
                    txtPAN.Text, txtDoorNo.Text, txtPremises.Text, txtStreet.Text, txtLocality.Text, txtTown.Text, state, country, 
                    txtPinCode.Text, txtEmail1.Text, txtEmail2.Text, txtMobileNo1.Text, txtMobileNo2.Text, txtSTDISDCode.Text, txtOfficeNo.Text,
                    txtResidenceNo.Text, txtAadharNo.Text, txtEnrollmentId.Text
                    );
            }

			if (CommandName.ToUpper().Equals("UPDATE"))
            {
               // int employeeId = Convert.ToInt32(txtEmployeeId);

                PhoenixPayRollIndia.PayRollGeneralUpdate(usercode, Id, txtFirstName.Text, txtMiddleName.Text, txtLastName.Text,
                  txtPAN.Text, txtDoorNo.Text, txtPremises.Text, txtStreet.Text, txtLocality.Text, txtTown.Text, state, country,
                  txtPinCode.Text, txtEmail1.Text, txtEmail2.Text, txtMobileNo1.Text, txtMobileNo2.Text, txtSTDISDCode.Text, txtOfficeNo.Text,
                  txtResidenceNo.Text, txtAadharNo.Text, txtEnrollmentId.Text
                );
            }

			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList();", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidReport()
    {
        ucError.HeaderMessage = "Please add the following details";

        if (string.IsNullOrWhiteSpace(txtFirstName.Text))
        {
            ucError.ErrorMessage = "First Name is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtLastName.Text))
        {
            ucError.ErrorMessage = "Last Name is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtPAN.Text))
        {
            ucError.ErrorMessage = "PAN is mandatory";
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

        //if (string.IsNullOrWhiteSpace(txtState.SelectedState))
        //{
        //    ucError.ErrorMessage = "State is mandatory";
        //}

        //if (string.IsNullOrWhiteSpace(txtCountry.SelectedCountry))
        //{
        //    ucError.ErrorMessage = "Country. is mandatory";
        //}

        if (string.IsNullOrWhiteSpace(txtPinCode.Text))
        {
            ucError.ErrorMessage = "Pincode is mandatory";
        }


        if (string.IsNullOrWhiteSpace(txtEmail1.Text))
        {
            ucError.ErrorMessage = "Email 1 is mandatory";
        }

        //if (string.IsNullOrWhiteSpace(txtEmail2.Text))
        //{
        //    ucError.ErrorMessage = "Email 2 is mandatory";
        //}

        if (string.IsNullOrWhiteSpace(txtMobileNo1.Text))
        {
            ucError.ErrorMessage = "Mobile No 1 is mandatory";
        }

        //if (string.IsNullOrWhiteSpace(txtMobileNo2.Text))
        //{
        //    ucError.ErrorMessage = "Mobile No 2 is mandatory";
        //}

        if (string.IsNullOrWhiteSpace(txtSTDISDCode.Text))
        {
            ucError.ErrorMessage = "STD/ISD Code is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtOfficeNo.Text))
        {
            ucError.ErrorMessage = "Office No. is mandatory";
        }


        //if (string.IsNullOrWhiteSpace(txtResidenceNo.Text))
        //{
        //    ucError.ErrorMessage = "Residence No. is mandatory";
        //}

        if (string.IsNullOrWhiteSpace(txtAadharNo.Text))
        {
            ucError.ErrorMessage = "Aadhar No. is mandatory";
        }

        return ucError.IsError;
    }

    private void LoadState()
    {
        ddlState.SelectedState = "";
        ddlState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlCountry.SelectedCountry));
    }

    protected void txtCountry_TextChangedEvent(object sender, EventArgs e)
    {
        LoadState();
    }
}