using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersVesselCommunicationDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuVesselCommunicationDetails.AccessRights = this.ViewState;
        MenuVesselCommunicationDetails.MenuList = toolbar1.Show();
        //MenuVesselCommunicationDetails.SelectedMenuIndex = 8;

        PhoenixToolbar toolbar = new PhoenixToolbar();

       // toolbar.AddButton("Vessel Search", "VESSELSEARCH", ToolBarDirection.Right);
        toolbar.AddButton("History", "HISTORY", ToolBarDirection.Right);

        toolbar.AddButton("Crew Docs", "DOCUMENTSREQUIRED", ToolBarDirection.Right);
        toolbar.AddButton("Budget", "BUDGET", ToolBarDirection.Right);
        toolbar.AddButton("Manning", "MANNINGSCALE", ToolBarDirection.Right);
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            toolbar.AddButton("Office Admin", "OFFICEADMIN", ToolBarDirection.Right);
        toolbar.AddButton("Admin", "ADMIN", ToolBarDirection.Right);
        //toolbar.AddButton("Certificates", "CERTIFICATES", ToolBarDirection.Right);
        toolbar.AddButton("Commn Equipments", "COMMUNICATIONDETAILS", ToolBarDirection.Right); // Bug Id: 8910
        toolbar.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);
        
        MenuVesselList.AccessRights = this.ViewState;
        MenuVesselList.MenuList = toolbar.Show();
        
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            MenuVesselList.SelectedMenuIndex = 6;
        else
            MenuVesselList.SelectedMenuIndex = 5;
        
        if (!IsPostBack)
        {
            EditVesselCommunicationDetails(Int16.Parse(Filter.CurrentVesselMasterFilter));
        }
    }

    protected void EditVesselCommunicationDetails(int vesselId)
    {
        DataSet ds = PhoenixRegistersVesselCommunicationDetails.EditCommunicationDetails(vesselId);

        DataSet dsVessel = PhoenixRegistersVessel.EditVessel(vesselId);

        DataRow drVessel = dsVessel.Tables[0].Rows[0];

        txtVesselName.Text = drVessel["FLDVESSELNAME"].ToString();

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtAPhone.Text = dr["FLDAPHONE"].ToString();
            txtAFax.Text = dr["FLDAFAX"].ToString();
            txtBPhone.Text = dr["FLDBPHONE"].ToString();
            txtBFax.Text = dr["FLDBFAX"].ToString();
            txtEmail.Text = dr["FLDEMAIL"].ToString();
            //txtAccAdministratorEmail.Text = dr["FLDACCOUNTADMINISTRATOREMAIL"].ToString();
            txtNotificationEmail.Text = dr["FLDNOTIFICATIONEMAIL"].ToString();
            //txtFleetManagerEmail.Text = dr["FLDFLEETMANAGEREMAIL"].ToString();
            txtAccInchargeEmail.Text = dr["FLDACCOUNTINCHARGEEMAIL"].ToString();

            txtPhone.Text = dr["FLDPHONE"].ToString();
            txtFax.Text = dr["FLDFAX"].ToString();
            txtMobileNumber.Text = dr["FLDMOBILENUMBER"].ToString();
            txtFPhone.Text = dr["FLDFPHONE"].ToString();
            txtFFax.Text = dr["FLDFFAX"].ToString();
            txtCTalex.Text = dr["FLDCTALEX"].ToString();
            txtvcgtechemail.Text= dr["FLDVCGTECHMAIL"].ToString();
        }
    }

    protected void VesselCommunicationDetails_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidVesselCommunicationDetails())
                {

                    if ((PhoenixRegistersVesselCommunicationDetails.EditCommunicationDetails(Int16.Parse(Filter.CurrentVesselMasterFilter))).Tables[0].Rows.Count > 0)
                    {
                        PhoenixRegistersVesselCommunicationDetails.UpdateCommunicationDetails(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , Int16.Parse(Filter.CurrentVesselMasterFilter)
                            , txtAPhone.Text
                            , txtAFax.Text
                            , txtBPhone.Text
                            , txtBFax.Text
                            , txtEmail.Text
                            , txtNotificationEmail.Text
                            , txtAccInchargeEmail.Text
                            , txtPhone.Text
                            , txtFax.Text
                            , (txtMobileNumber.Text == null) ? "" : txtMobileNumber.Text
                            , General.GetNullableString(txtFPhone.Text)
                            , General.GetNullableString(txtFFax.Text)
                            , General.GetNullableString(txtCTalex.Text)
                            , General.GetNullableString(txtvcgtechemail.Text)
                            );
                        ucStatus.Text = "Vessel Communication details updated.";
                        ucStatus.Visible = true;
                    }
                    else
                    {
                        PhoenixRegistersVesselCommunicationDetails.InsertCommunicationDetails(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , Int16.Parse(Filter.CurrentVesselMasterFilter)
                            , txtAPhone.Text
                            , txtAFax.Text
                            , txtBPhone.Text
                            , txtBFax.Text
                            , txtEmail.Text
                            , txtNotificationEmail.Text
                            , txtAccInchargeEmail.Text
                            , txtPhone.Text
                            , txtFax.Text
                            , (txtMobileNumber.Text == null) ? "" : txtMobileNumber.Text
                            , General.GetNullableString(txtFPhone.Text)
                            , General.GetNullableString(txtFFax.Text)
                            , General.GetNullableString(txtCTalex.Text)
                            , General.GetNullableString(txtvcgtechemail.Text)
                            );
                        ucStatus.Text = "Vessel Communication details inserted.";
                        ucStatus.Visible = true;
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = false;
        }
    }

    private bool IsValidVesselCommunicationDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.IsvalidEmail(txtEmail.Text))
            ucError.ErrorMessage = "Valid Email is required.";
        if (!General.IsvalidEmail(txtNotificationEmail.Text) && txtNotificationEmail.Text != "")
            ucError.ErrorMessage = "Valid Notification Email is required.";
        //if (!General.IsvalidEmail(txtAccAdministratorEmail.Text) && txtAccAdministratorEmail.Text != "")
        //    ucError.ErrorMessage = "Valid Account Administrator Email is required.";
        //if (!General.IsvalidEmail(txtFleetManagerEmail.Text) && txtFleetManagerEmail.Text != "")
        //    ucError.ErrorMessage = "Valid Fleet Manager Email is required.";
        if (!General.IsvalidEmail(txtAccInchargeEmail.Text) && txtAccInchargeEmail.Text != "")
            ucError.ErrorMessage = "Valid Account Incharge Email is required.";

        return (!ucError.IsError);
    }

    protected void MenuVesselList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (Filter.CurrentVesselMasterFilter == null)
        {
            if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                if (Session["NEWMODE"] != null && Session["NEWMODE"].ToString() == "1")
                {
                    Session["NEWMODE"] = 0;
                    //Response.Redirect( "../Registers/RegistersVessel.aspx";
                    return;
                }
            }
        }
        else
        {
            if (CommandName.ToUpper().Equals("ADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselParticulars.aspx");
            }
            else if (CommandName.ToUpper().Equals("OFFICEADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselOfficeAdmin.aspx");
            }
            else if (CommandName.ToUpper().Equals("COMMUNICATIONDETAILS"))
            {
                Response.Redirect("../Registers/RegistersVesselCommunicationDetails.aspx");
            }
            else if (CommandName.ToUpper().Equals("CERTIFICATES"))
            {
                Response.Redirect("../Registers/RegisterVesselCertificate.aspx");
            }
            else if (CommandName.ToUpper().Equals("MANNINGSCALE"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                    Response.Redirect("../Registers/RegistersOffshoreVesselManningScale.aspx");
                else
                    Response.Redirect("../Registers/RegistersVesselManningScale.aspx");
            }
            else if (CommandName.ToUpper().Equals("BUDGET"))
            {
                Response.Redirect("../Registers/RegistersVesselBudget.aspx");
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTSREQUIRED"))
            {
                Response.Redirect("../Registers/RegistersDocumentRequiredCourse.aspx?launchedfrom=VESSEL");
            }
            else if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                Response.Redirect("../Registers/RegistersVessel.aspx");
            }
            //else if (dce.CommandName.ToUpper().Equals("CORRESPONDENCE"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselCorrespondence.aspx";
            //}
            //else if (dce.CommandName.ToUpper().Equals("CHATBOX"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselChatBox.aspx?vesselid=" + Filter.CurrentVesselMasterFilter;
            //}
            else if (CommandName.ToUpper().Equals("FINANCIALYEAR"))
            {
                Response.Redirect("../Registers/RegistersVesselFinancialYear.aspx");
            }
            else if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("../Registers/RegistersVesselHistory.aspx");
            }
            else if (CommandName.ToUpper().Equals("VESSELSEARCH"))
            {
                Response.Redirect("../Registers/RegistersVesselNameSearch.aspx");
            }
            else
                Response.Redirect("../Registers/RegistersVesselList.aspx");
        }
    }
}