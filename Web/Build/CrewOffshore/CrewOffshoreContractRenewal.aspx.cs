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

public partial class CrewOffshore_CrewOffshoreContractRenewal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Show PDF", "SHOWPDF", ToolBarDirection.Right);
            toolbar.AddButton("Renew Contract", "SAVE", ToolBarDirection.Right);
            //if (Request.QueryString["redirectedfrom"] != null && Request.QueryString["redirectedfrom"].ToString() != "")
            //    toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);

            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["crewplanid"] = "";
                ViewState["appointmentletterid"] = "";
                ViewState["employeeid"] = "";
                ViewState["vesselid"] = "";
                ViewState["PAGENUMBER"] = 1;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["CVSL"] = -1;
                ViewState["CRNK"] = -1;
                ViewState["Charterer"] = "";
                ViewState["trainingmatrixid"] = "";
                ViewState["offsignerid"] = "";
                ViewState["reliefdate"] = "";
                ViewState["rankid"] = "";
                ViewState["vesselid"] = "";
                ViewState["vsltype"] = "";
                ViewState["crewplanid"] = "";
                ViewState["calledfrom"] = "";
                ViewState["signonid"] = "";
                ViewState["signondate"] = "";
                ViewState["port"] = "";
               
                ViewState["approval"] = "";
                ViewState["rowindex"] = "";
                ViewState["waivedyn"] = "";
                ViewState["edititem"] = "0";

                ViewState["rankexprowindex"] = "";
                ViewState["rankexpwaivedyn"] = "";
                ViewState["rankexpedititem"] = "0";

                ViewState["vtexprowindex"] = "";
                ViewState["vtexpwaivedyn"] = "";
                ViewState["vtexpedititem"] = "0";

                ViewState["planstatus"] = "";

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
                BindCharter();

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
            txtrankname.Text = dr["FLDRANKNAME"].ToString();
            txtVessel.Text = dr["FLDVESSELNAME"].ToString();
            txtvesselname.Text = dr["FLDVESSELNAME"].ToString();
            ViewState["appointmentletterid"] = dr["FLDAPPOINTMENTLETTERID"].ToString();
            ViewState["vesselid"] = dr["FLDVESSELID"].ToString();
            ViewState["rankid"] = dr["FLDRANKID"].ToString();
            lblcurrency.Text = "(" + dr["FLDCURRENCYCODE"].ToString() + ")";
            lblcurrencyAllowance.Text = "(" + dr["FLDCURRENCYCODE"].ToString() + ")";
            lblCurrencyid.Text = dr["FLDCURRENCYID"].ToString();
        }
    }
    protected void BindAppointmentLetter()
    {
        DataTable dt = PhoenixCrewOffshoreContract.EditAppointmentLetter(General.GetNullableGuid(ViewState["appointmentletterid"].ToString())
                                                                        , General.GetNullableGuid(ViewState["crewplanid"].ToString()));
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
            // ucContractCancellationDate.Text = dt.Rows[0]["FLDCANCELLATIONDATE"].ToString();
            ucsignonDate.Text = dt.Rows[0]["FLDSIGNONDATE"].ToString();
            ucBankAccount.SelectedBankAccount = dt.Rows[0]["FLDBANKACCOUNTID"].ToString();
            lblcurrency.Text = "(" + dt.Rows[0]["FLDCURRENCYCODE"].ToString() + ")";
            lblcurrencyAllowance.Text = "(" + dt.Rows[0]["FLDCURRENCYCODE"].ToString() + ")";
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
                int? contactperiod = int.Parse(txtContractPeriodDays.Text) + int.Parse(txtrenewdays.Text);
                PhoenixCrewOffshoreContract.RenewAppointmentLetter(new Guid(ViewState["appointmentletterid"].ToString())
                                                                   , new Guid(ViewState["crewplanid"].ToString())
                                                                   , General.GetNullableDateTime("") //signondate
                                                                   , General.GetNullableDateTime(ucContractStartDate.Text)
                                                                   , General.GetNullableInteger(ddlSignOnSeaPort.SelectedSeaport)
                                                                   , General.GetNullableInteger(ddlSignOffPort.SelectedSeaport)
                                                                   , General.GetNullableInteger(txtSignOffTravelDays.Text)
                                                                   , General.GetNullableInteger(txtDailyWages.Text)
                                                                   , General.GetNullableInteger(txtDPAllowance.Text)
                                                                   , contactperiod
                                                                   , General.GetNullableInteger(txtPlusMinusPeriod.Text)
                                                                   , null
                                                                   , General.GetNullableGuid(ucBankAccount.SelectedBankAccount)
                                                                   );
                ucStatus.Text = "Contract renewed successfully.";
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

        if (General.GetNullableDateTime(ucContractStartDate.Text) == null)
            ucError.ErrorMessage = "Contract commencement date is required.";

        if (General.GetNullableInteger(txtrenewdays.Text) == null)
            ucError.ErrorMessage = "Renew days is required.";

        if (General.GetNullableInteger(txtPlusMinusPeriod.Text) == null)
            ucError.ErrorMessage = "+/- period (days) is required. ";

        if (General.GetNullableInteger(txtDailyWages.Text) == null)
            ucError.ErrorMessage = "Daily Wages(USD) is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    public void BindCharter()
    {
        try
        {
            ddlchartername.Items.Clear();

            DataTable dt = new DataTable();
            dt = PhoenixCrewOffshoreSuitabilityCheckAllDocuments.ListCharterName(General.GetNullableInteger(ViewState["vesselid"].ToString()));
            ddlchartername.DataTextField = "FLDCHARTERCOMMENCEDATE";
            ddlchartername.DataValueField = "FLDCHARTERID";

            if (dt.Rows.Count > 0)
            {
                ddlchartername.DataSource = dt;
            }

            ddlchartername.DataBind();
            ddlchartername.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            //ddlchartername.SelectedIndex=0;



        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSuitability_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    public void BindData()
    {
        int issuitable = 1;
        int renewdays = General.GetNullableInteger(txtrenewdays.Text) == null ? 0 : int.Parse(txtrenewdays.Text);
        DateTime renewaldate = Convert.ToDateTime(ucsignonDate.Text).AddDays(int.Parse(txtContractPeriodDays.Text)+renewdays+ int.Parse(txtPlusMinusPeriod.Text));
        txtExtendeddate.Text = renewaldate.ToString();
        DataSet ds = PhoenixCrewOffshoreSuitabilityCheckAllDocuments.CrewSuitabilityForVesselRank(
            General.GetNullableInteger(ViewState["vesselid"].ToString())
            , General.GetNullableInteger(ViewState["rankid"].ToString()) == null ? 0 : General.GetNullableInteger(ViewState["rankid"].ToString())
            , int.Parse(ViewState["employeeid"].ToString())
            , General.GetNullableDateTime(ucsignonDate.Text)
            , ref issuitable
            , 1
            , General.GetNullableGuid(ViewState["crewplanid"].ToString())
            , General.GetNullableInteger(ddlchartername.SelectedValue)
            , null
            , renewaldate
            );

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtchartername.Text = ds.Tables[0].Rows[0]["FLDCHARTER"].ToString();
            txtcompanyname.Text = ds.Tables[0].Rows[0]["FLDMANAGER"].ToString();
            txtvesseltype.Text = ds.Tables[0].Rows[0]["FLDVESSELTYPE"].ToString();
            txtflagname.Text = ds.Tables[0].Rows[0]["FLDFLAG"].ToString();

            // txtmaxtourday.Text = ds.Tables[0].Rows[0]["FLDMAXTOURDAYS"].ToString();
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            gvSuitability.DataSource = ds.Tables[1];


            string planstatus = "";
            foreach (GridItem item in gvSuitability.MasterTableView.Items)
            {
                if (item is GridItem)
                {
                    GridDataItem dataItem = (GridDataItem)item;
                    RadLabel lblPlanStatus = (RadLabel)item.FindControl("lblPlanStatus");
                    planstatus = lblPlanStatus.Text;
                }
            }

            if (!string.IsNullOrEmpty(planstatus))
                ViewState["planstatus"] = planstatus;
            if (string.IsNullOrEmpty(planstatus))
                gvSuitability.Columns[7].Visible = true;

        }
        else
        {
            //  gvSuitability.DataSource = ds.Tables[0];
        }
    }

    protected void txtrenewdays_TextChangedEvent(object o, EventArgs e)
    {
        BindData();
        gvSuitability.Rebind();
    }

    protected void gvSuitability_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {


            RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
            RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");
            RadLabel lblDocumentName = (RadLabel)e.Item.FindControl("lblDocumentName");
            RadLabel lblExpiryDate = (RadLabel)e.Item.FindControl("lblExpiryDate");
            RadLabel lblNationality = (RadLabel)e.Item.FindControl("lblNationality");
            LinkButton lnkName = (LinkButton)e.Item.FindControl("lnkName");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblAttachmenttype = (RadLabel)e.Item.FindControl("lblAttachmenttype");
            RadLabel lblStage = (RadLabel)e.Item.FindControl("lblStage");
            RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
            CheckBox chkWaivedYN = (CheckBox)e.Item.FindControl("chkWaivedYN");
            RadTextBox txtReason = (RadTextBox)e.Item.FindControl("txtReason");
            RadLabel lblVerifiedYN = (RadLabel)e.Item.FindControl("lblVerifiedYN");
            RadLabel lblReqDocumentName = (RadLabel)e.Item.FindControl("lblReqDocumentName");
            RadLabel lblStageid = (RadLabel)e.Item.FindControl("lblStageid");
            RadLabel lblProposalStageid = (RadLabel)e.Item.FindControl("lblProposalStageid");
            CheckBox chkCanbeWaivedYN = (CheckBox)e.Item.FindControl("chkCanbeWaivedYN");
            RadLabel lblPlanStatus = (RadLabel)e.Item.FindControl("lblPlanStatus");
            RadLabel lblDocumentType = (RadLabel)e.Item.FindControl("lblDocumentType");
            RadLabel lblAuthenticatedYN = (RadLabel)e.Item.FindControl("lblAuthenticatedYN");


            UserControlToolTip uctDate = (UserControlToolTip)e.Item.FindControl("ucToolTipDate");
            uctDate.Position = ToolTipPosition.TopCenter;
            uctDate.TargetControlId = chkWaivedYN.ClientID;
            //chkWaivedYN.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctDate.ToolTip + "', 'visible');");
            //    chkWaivedYN.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctDate.ToolTip + "', 'hidden');");

            CheckBox chkWaivedconfirmYN = (CheckBox)e.Item.FindControl("chkWaivedconfirmYN");
            UserControlToolTip ucWaiveToolTipDate = (UserControlToolTip)e.Item.FindControl("ucWaiveToolTipDate");
            ucWaiveToolTipDate.Position = ToolTipPosition.TopCenter;
            ucWaiveToolTipDate.TargetControlId = chkWaivedconfirmYN.ClientID;
            LinkButton cmdApprove = (LinkButton)e.Item.FindControl("cmdApprove");

            //if (chkWaivedYN != null)
            //{

            //    if (chkWaivedYN.Checked == true)
            //    {
            //        if (cmdApprove != null) cmdApprove.Visible = true;
            //    }
            //    else
            //    {
            //        if (cmdApprove != null) cmdApprove.Visible = false;
            //    }
            //}
            if (chkWaivedconfirmYN != null)
            {
                if (chkWaivedconfirmYN.Checked == true)
                {
                    if (cmdApprove != null) cmdApprove.Visible = false;
                    //if (imgReject != null) imgReject.Visible = true;
                }
            }

            if (cmdApprove != null)
            {
                //cmdApprove.Visible = SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName);
                cmdApprove.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Waive ?')");
            }
            //if (General.GetNullableInteger(ViewState["rowindex"].ToString()) != null && General.GetNullableInteger(ViewState["rowindex"].ToString()) == e.Item.DataSetIndex)
            //{
            //    if (chkWaivedYN != null)
            //    {
            //        if (General.GetNullableInteger(ViewState["waivedyn"].ToString()) != null && General.GetNullableInteger(ViewState["waivedyn"].ToString()) == 1)
            //        {
            //            chkWaivedYN.Checked = true;
            //            if (txtReason != null) txtReason.CssClass = "input_mandatory";
            //        }
            //        else
            //        {
            //            chkWaivedYN.Checked = false;
            //            if (txtReason != null) txtReason.CssClass = "input";
            //        }
            //        ViewState["rowindex"] = "";
            //        ViewState["waivedyn"] = "";
            //        ViewState["edititem"] = "0";
            //    }
            //}

            if (lblStageid != null && lblProposalStageid != null)
            {
                if (lblStageid.Text.Equals(lblProposalStageid.Text) && DataBinder.Eval(e.Item.DataItem, "FLDWAIVEDYN").ToString().Equals("1")) // enabled only for proposal stage and if the document can be waived.
                {
                    if (chkCanbeWaivedYN != null)
                    {
                        chkCanbeWaivedYN.Enabled = true;
                    }
                }
            }

            if (lblExpiredYN.Text.Trim() == "1" || lblMissingYN.Text.Trim() == "1" || lblVerifiedYN.Text.Trim() == "0")
            {
                //e.Item.BackColor= System.Drawing.Color.Red;
                //lblDocumentName.ForeColor = System.Drawing.Color.Red;
                //lblExpiryDate.ForeColor = System.Drawing.Color.Red;
                //lblNationality.ForeColor = System.Drawing.Color.Red;
                //lnkName.ForeColor = System.Drawing.Color.Red;
                //lblStage.ForeColor = System.Drawing.Color.Red;
                //lblStatus.ForeColor = System.Drawing.Color.Red;
                lblReqDocumentName.Attributes.Add("style", "color:red !important;");
                if (lblMissingYN.Text.Trim() == "1")
                {
                    //if (lblReqDocumentName != null) lblReqDocumentName.ForeColor = System.Drawing.Color.Red;
                    if (lblDocumentName != null) lblDocumentName.Visible = false;
                }
            }
            if (lblDTKey != null && !string.IsNullOrEmpty(lblDTKey.Text) && lblMissingYN.Text.Trim() == "0")
            {
                if (lblDocumentName != null) lblDocumentName.Visible = false;
                if (lnkName != null) lnkName.Visible = true;
                if (lblExpiredYN.Text.Trim() == "0" && lblMissingYN.Text.Trim() == "0" && lblVerifiedYN.Text.Trim() == "1")
                    lnkName.ForeColor = System.Drawing.Color.Black;
            }
            if (lnkName != null)
            {
                if (Request.QueryString["popup"] != null && Request.QueryString["popup"].ToString() != "")
                {
                    lnkName.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + lblAttachmenttype.Text + "&U=1'); return false;");
                }
                else
                {
                    lnkName.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/ Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + lblAttachmenttype.Text + "&U=1'); return false;");
                }
            }

            //if (chkWaivedYN != null)
            //    chkWaivedYN.Enabled = DataBinder.Eval(e.Item.DataItem, "FLDWAIVEDYN").ToString().Equals("1") ? true : false;

            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            LinkButton cmdSave = (LinkButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);

        }
    }
}
