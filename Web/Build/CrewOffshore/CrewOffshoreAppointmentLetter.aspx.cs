using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
public partial class CrewOffshoreAppointmentLetter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Show PDF", "SHOWPDF", ToolBarDirection.Right);
           // toolbar.AddButton("POEA", "POEA", ToolBarDirection.Right); -- POSH
            toolbar.AddButton("Issue Appointment Letter", "SAVE", ToolBarDirection.Right);
            if (Request.QueryString["redirectedfrom"] != null && Request.QueryString["redirectedfrom"].ToString() != "")
                toolbar.AddButton("Back", "BACK",ToolBarDirection.Right);
            
            
            //toolbar.AddButton("Approve", "APPROVE");                
            //toolbar.AddImageLink("javascript:Openpopup('codehelp1','','../Common/CommonApproval.aspx?docid=" + ViewState["crewplanid"].ToString() + "&mod=" + PhoenixModule.OFFSHORE + "&type=" + Offshoreapprovaltype
            //    + "&crewplanid=" + ViewState["crewplanid"].ToString() + "&appoinmentletterid=" + ViewState["appointmentletterid"].ToString()+ "'); return false;", "Approve", "", "APPROVE");
            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["crewplanid"] = "";
                ViewState["appointmentletterid"] = "";
                ViewState["employeeid"] = "";
                ViewState["vesselid"] = "";
                ViewState["PAGENUMBER"] = 1;

                if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"].ToString() != "")
                    ViewState["crewplanid"] = Request.QueryString["crewplanid"].ToString();

                if (Request.QueryString["appointmentletterid"] != null && Request.QueryString["appointmentletterid"].ToString() != "")
                    ViewState["appointmentletterid"] = Request.QueryString["appointmentletterid"].ToString();

                if (Request.QueryString["employeeid"] != null && Request.QueryString["employeeid"].ToString() != "")
                    ViewState["employeeid"] = Request.QueryString["employeeid"].ToString();

                SetEmployeePrimaryDetails();
                BindCrewPlan();
                ucBankAccount.EmployeeId = ViewState["employeeid"].ToString();
                ucBankAccount.BankAccountList = PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(int.Parse(ViewState["vesselid"].ToString()), General.GetNullableInteger(ViewState["employeeid"].ToString()));
                BindAppointmentLetter();
                gvOffshoreComponent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["employeeid"].ToString()));

            if (dt.Rows.Count > 0)
            {
                //txtEmployeeNumber.Text = dt.Rows[0]["FLDFILENO"].ToString();                
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                //txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindCrewPlan()
    {
        DataTable dt = PhoenixCrewOffshoreCrewChange.EditCrewPlan(General.GetNullableGuid(ViewState["crewplanid"].ToString()));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtRank.Text = dr["FLDRANKNAME"].ToString();            
            txtVessel.Text = dr["FLDVESSELNAME"].ToString();           
            ViewState["appointmentletterid"] = dr["FLDAPPOINTMENTLETTERID"].ToString();
            ViewState["vesselid"] = dr["FLDVESSELID"].ToString();
            lblcurrency.Text = "("+dr["FLDCURRENCYCODE"].ToString()+")";
            lblcurrencyAllowance.Text = "(" + dr["FLDCURRENCYCODE"].ToString() + ")";
            lblCurrencyid.Text = dr["FLDCURRENCYID"].ToString();
        }
    }
    protected void BindAppointmentLetter()
    {
        DataTable dt = PhoenixCrewOffshoreContract.EditAppointmentLetter(General.GetNullableGuid(ViewState["appointmentletterid"].ToString())
                                                                        ,General.GetNullableGuid(ViewState["crewplanid"].ToString()));
        if (dt.Rows.Count > 0)
        {
            txtAddress.Text = dt.Rows[0]["FLDADDRESS"].ToString();
            ucDOB.Text = dt.Rows[0]["FLDDATEOFBIRTH"].ToString();
            txtPlaceOfBirth.Text = dt.Rows[0]["FLDPLACEOFBIRTH"].ToString();
            txtNationality.Text = dt.Rows[0]["FLDNATIONALITYNAME"].ToString();            
            txtSeamanBook.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();            
            txtIMONo.Text = dt.Rows[0]["FLDIMONUMBER"].ToString();
            txtRegOwner.Text = dt.Rows[0]["FLDOWNER"].ToString();
            txtVesselAddress.Text = dt.Rows[0]["FLDOWNERADDRESS"].ToString();
            ddlSignOnSeaPort.SelectedSeaport = dt.Rows[0]["FLDSIGNONSEAPORTID"].ToString();
            ddlSignOffPort.SelectedSeaport = dt.Rows[0]["FLDSIGNOFFSEAPORTID"].ToString();
            ucContractStartDate.Text = dt.Rows[0]["FLDCONTRACTCOMMENCEMENTDATE"].ToString();
            txtContractPeriodDays.Text = dt.Rows[0]["FLDCONTRACTPERIODDAYS"].ToString();
            txtDailyWages.Text = dt.Rows[0]["FLDDAILYRATE"].ToString();
            txtDPAllowance.Text = dt.Rows[0]["FLDDPALLOWANCE"].ToString();
            txtSignOffTravelDays.Text = dt.Rows[0]["FLDTRAVELDURATIONDAYS"].ToString();
            txtPlusMinusPeriod.Text = dt.Rows[0]["FLDPLUSORMINUSRANGE"].ToString();
            ucContractCancellationDate.Text = dt.Rows[0]["FLDCANCELLATIONDATE"].ToString();
            ucBankAccount.SelectedBankAccount = dt.Rows[0]["FLDBANKACCOUNTID"].ToString();
            lblcurrency.Text= "(" + dt.Rows[0]["FLDCURRENCYCODE"].ToString() + ")";
            lblcurrencyAllowance.Text="(" + dt.Rows[0]["FLDCURRENCYCODE"].ToString() + ")";
            lblCurrencyid.Text = dt.Rows[0]["FLDCURRENCYID"].ToString();
        }
    }

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                if (string.IsNullOrEmpty(ViewState["appointmentletterid"].ToString()))
                {
                    Guid? newid = null;                    
                    PhoenixCrewOffshoreContract.InsertAppointmentLetter(new Guid(ViewState["crewplanid"].ToString())
                                                                    , int.Parse(ViewState["employeeid"].ToString())
                                                                    , int.Parse(ViewState["vesselid"].ToString())
                                                                    , General.GetNullableDateTime("") //signondate
                                                                    , General.GetNullableDateTime(ucContractStartDate.Text)
                                                                    , ref newid
                                                                    , General.GetNullableInteger(ddlSignOnSeaPort.SelectedSeaport)
                                                                    , General.GetNullableInteger(ddlSignOffPort.SelectedSeaport)
                                                                    , General.GetNullableInteger(txtSignOffTravelDays.Text)
                                                                    , General.GetNullableInteger(txtDailyWages.Text)
                                                                    , General.GetNullableInteger(txtDPAllowance.Text)
                                                                    , General.GetNullableInteger(txtContractPeriodDays.Text)
                                                                    , General.GetNullableInteger(txtPlusMinusPeriod.Text)
                                                                    , General.GetNullableDateTime(ucContractCancellationDate.Text)
                                                                    , General.GetNullableGuid(ucBankAccount.SelectedBankAccount)
                                                                   );
                    ViewState["appointmentletterid"] = newid.ToString();
                    ucStatus.Text = "Appointment Letter is updated.";
                }
                else
                {
                    PhoenixCrewOffshoreContract.UpdateAppointmentLetter(new Guid(ViewState["appointmentletterid"].ToString())
                                                                    , new Guid(ViewState["crewplanid"].ToString())
                                                                    , General.GetNullableDateTime("") //signondate
                                                                    , General.GetNullableDateTime(ucContractStartDate.Text)
                                                                    , General.GetNullableInteger(ddlSignOnSeaPort.SelectedSeaport)
                                                                    , General.GetNullableInteger(ddlSignOffPort.SelectedSeaport)
                                                                    , General.GetNullableInteger(txtSignOffTravelDays.Text)
                                                                    , General.GetNullableInteger(txtDailyWages.Text)
                                                                    , General.GetNullableInteger(txtDPAllowance.Text)
                                                                    , General.GetNullableInteger(txtContractPeriodDays.Text)
                                                                    , General.GetNullableInteger(txtPlusMinusPeriod.Text)
                                                                    , General.GetNullableDateTime(ucContractCancellationDate.Text)
                                                                    , General.GetNullableGuid(ucBankAccount.SelectedBankAccount)
                                                                    );
                    ucStatus.Text = "Appointment Letter is updated.";
                }
                BindAppointmentLetter();

                if (Request.QueryString["popup"] != null && Request.QueryString["popup"].ToString() != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "fnReloadList('chml', null, true);", true);
                }
            }
            else if (CommandName.ToUpper().Equals("BACK"))
            {
                
                Response.Redirect("../CrewOffshore/CrewOffshoreSuitabilityCheck.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), false);
                //Response.Redirect("../CrewOffshore/CrewOffshoreSuitabilityCheckWithDocument.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), false); --POSH
            }
            else if (CommandName.ToUpper().Equals("SHOWPDF"))
            {
                string querystring = Request.QueryString.ToString();
                querystring = querystring.Replace("applicationcode=11&reportcode=APPOINMENTLETTER&showmenu=0&showword=0&showexcel=0", "");
                if (!querystring.Contains("appointmentletterid"))
                    querystring = querystring + "&appointmentletterid=" + ViewState["appointmentletterid"].ToString(); 
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=APPOINMENTLETTER&showmenu=0&showword=0&showexcel=0"
                    + (querystring != string.Empty ? ("&" + querystring) : string.Empty), false);
            }
            else if(CommandName.ToUpper().Equals("POEA"))
            {
                string querystring = Request.QueryString.ToString();
                querystring = querystring.Replace("applicationcode=11&reportcode=POEALETTER&showmenu=0&showword=0&showexcel=0", "");
                if (!querystring.Contains("appointmentletterid"))
                    querystring = querystring + "&appointmentletterid=" + ViewState["appointmentletterid"].ToString();
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=POEALETTER&showmenu=0&showword=0&showexcel=0"
                    + (querystring != string.Empty ? ("&" + querystring) : string.Empty), false);
            }
            //else if (dce.CommandName.ToUpper().Equals("APPROVE"))
            //{
            //    string Offshoreapprovaltype = PhoenixCommonRegisters.GetHardCode(1, 98, "OAA");
            //    ucConfirm.Visible = true;
            //    ucConfirm.Text = "Appoinment letter will be locked and you will not be able to change anything. Are you sure want to continue?";
                
            //    //Response.Redirect("../Common/CommonApproval.aspx?docid=" + ViewState["crewplanid"].ToString() + "&mod=" + PhoenixModule.OFFSHORE + "&type=" + Offshoreapprovaltype
            //    //    + "&crewplanid=" + ViewState["crewplanid"].ToString() + "&appoinmentletterid=" + ViewState["appointmentletterid"].ToString(), false);
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(ucContractStartDate.Text)== null)
            ucError.ErrorMessage = "Contract commencement date is required.";

        if (General.GetNullableInteger(txtContractPeriodDays.Text) == null)
            ucError.ErrorMessage = "Tenure (Days) is required. Please provide the days in Proposal Screen.";

        if (General.GetNullableInteger(txtPlusMinusPeriod.Text) == null)
            ucError.ErrorMessage = "+/- period (days) is required. Please provide the +/- period in Proposal Screen.";

        if (General.GetNullableInteger(txtDailyWages.Text) == null)
            ucError.ErrorMessage = "Daily Wages(USD) is required. Please provide the wages period in Proposal Screen.";
 
        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {                
                string Offshoreapprovaltype = PhoenixCommonRegisters.GetHardCode(1, 98, "OAA");
                String scriptpopup = String.Format(
                     "javascript:Openpopup('codehelp1','','../Common/CommonApproval.aspx?docid=" + ViewState["crewplanid"].ToString() + "&mod=" + PhoenixModule.OFFSHORE + "&type=" + Offshoreapprovaltype
                    + "&crewplanid=" + ViewState["crewplanid"].ToString() + "&appoinmentletterid=" + ViewState["appointmentletterid"].ToString()+ "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    /*Component grid*/
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME", "FLDHOURS", "FLDREMARKS" };
        string[] alCaptions = { "Component", "Hours Per Week", "Remarks"};

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewOffshoreContract.SearchContractComponent(null, General.GetNullableGuid(ViewState["crewplanid"].ToString())
            ,null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreComponent", "Component", alCaptions, alColumns, ds);
        gvOffshoreComponent.DataSource = ds;
        gvOffshoreComponent.VirtualItemCount = iRowCount;
       

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
       
    }

    
  
    
   
    private bool IsValidData(string name, string hours)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(name) == null)
            ucError.ErrorMessage = "Component is required.";

        if (General.GetNullableString(hours) == null)
            ucError.ErrorMessage = "Hours is required.";
       

        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
       
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvOffshoreComponent.Rebind();
    }

   

    protected void gvOffshoreComponent_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOffshoreComponent.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshoreComponent_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
          
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtComponentNameAdd")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("txtHoursAdd")).Text
                ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreContract.InsertContractComponent(
                    General.GetNullableGuid(ViewState["appointmentletterid"].ToString()),
                    General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                    General.GetNullableInteger(ViewState["employeeid"].ToString()),
                    General.GetNullableInteger(ViewState["vesselid"].ToString()),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtComponentNameAdd")).Text),
                    General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("txtHoursAdd")).Text),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text));
                BindData();
                gvOffshoreComponent.Rebind();
                ((RadTextBox)e.Item.FindControl("txtComponentNameAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                int componentid = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDCONTRACTCOMPONENETID"].ToString());  //int.Parse(.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreContract.DeleteContractComponent(componentid);
                BindData();
                gvOffshoreComponent.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtComponentNameEdit")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("txtHoursEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string lblComponentId = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDCONTRACTCOMPONENETID"].ToString();//_gridView.DataKeys[nCurrentRow].Value.ToString();

                PhoenixCrewOffshoreContract.UpdateContractComponent(General.GetNullableInteger(lblComponentId)
                    , General.GetNullableGuid(ViewState["appointmentletterid"].ToString())
                    , General.GetNullableGuid(ViewState["crewplanid"].ToString())
                    , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtComponentNameEdit")).Text)
                    , General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("txtHoursEdit")).Text)
                    , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text)
                   );

               
                BindData();
                gvOffshoreComponent.Rebind();
            }
            
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshoreComponent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        

        if (e.Item is GridDataItem)
        {


            RadLabel lblComponentId = (RadLabel)e.Item.FindControl("lblComponentId");
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ab = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }
}
