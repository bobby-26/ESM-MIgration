using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.IO;
using System.Net;
using Telerik.Web.UI;


public partial class CrewTravelAgentInvoiceGeneralInOffice : PhoenixBasePage
{
    decimal? value1, value2;
    int officeyn = 1;
    PhoenixToolbar toolbar;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar Historytoolbar = new PhoenixToolbar();
        Historytoolbar.AddButton("History", "HISTORY");
        InvoiceHistory.AccessRights = this.ViewState;
        InvoiceHistory.MenuList = Historytoolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        CrewMenu.AccessRights = this.ViewState;
        CrewMenu.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["EXCEPTION"] = null;
            ViewState["Status"] = null;
            ViewState["CANPOSTYN"] = 0;
            PopulateData();
            HideLables();

            gvList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }

        if (Request.QueryString["fromexception"] != null)
        {
            ViewState["EXCEPTION"] = Request.QueryString["fromexception"].ToString();
        }

        ViewState["AGENTINVOICEID"] = Request.QueryString["AGENTINVOICEID"];
        BindOwnerBudgetCode();

    }
    protected void InvoiceHistory_TablStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("HISTORY"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                "BookMarkScript", "javascript:openNewWindow('History','','" + Session["sitepath"] + "/Accounts/AccountsAirfarePaymentVoucherGenerateHistory.aspx?AGENTINVOICEID=" + ViewState["AGENTINVOICEID"].ToString() + "');", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid iDtKey = new Guid();
            Guid iRevenueDtKey = new Guid();
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!string.IsNullOrEmpty(txtadjustamount.Text.Trim()) && string.IsNullOrEmpty(txtremarks.Text.Trim()))
                {
                    ucError.ErrorMessage = "Remarks is required.";
                    ucError.Visible = true;
                }
                if (rblstatus.SelectedValue == "1168" && ViewState["EXCEPTION"] != null && ViewState["EXCEPTION"].ToString() == "1")
                {
                    if (txtremarks == null || txtremarks.Text == "")
                    {
                        ucError.ErrorMessage = "Remarks is required.";
                        ucError.Visible = true;
                    }
                }

                if (rblstatus.SelectedValue == "1168")
                {
                    if (!IsValidData(txtpayableamount.Text, ucBudgetCode.SelectedBudgetCode))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    else
                    {
                        DateTime date;
                        if (rblDepDate.SelectedValue == "1")
                            date = Convert.ToDateTime(txtinvdepdate.Text);
                        else
                            date = Convert.ToDateTime(txtdepdate.Text);

                        DataSet ds = PhoenixCrewTravelInvoice.DuplicateSectorCheck(new Guid(Request.QueryString["AGENTINVOICEID"]),
                                                    txtinvticketno.Text, General.GetNullableInteger(ViewState["EmployeeId"].ToString()), date);

                        DataRow dr = ds.Tables[0].Rows[0];
                        if (Int32.Parse(dr["FLDORIGINCOUNT"].ToString()) <= 1)
                        {
                            PhoenixCrewTravelInvoice.AgentInvoiceStatusUpdate(new Guid(Request.QueryString["AGENTINVOICEID"]),
                                                                                General.GetNullableDecimal(txtadjustamount.Text),
                                                                                General.GetNullableDecimal(txtpayableamount.Text),
                                                                                txtremarks.Text,
                                                                                Convert.ToInt32(ViewState["Status"].ToString()),
                                                                                General.GetNullableInteger(ucBudgetCode.SelectedBudgetCode),
                                                                                General.GetNullableInteger(rblDepDate.SelectedValue),
                                                                                General.GetNullableInteger(ddlvessel.SelectedVessel),
                                                                                General.GetNullableInteger(ddlAccountDetails.SelectedValue),
                                                                                General.GetNullableGuid(ucOwnerBudgetCode.SelectedValue),
                                                                                ref iDtKey,
                                                                                ref iRevenueDtKey,
                                                                                General.GetNullableInteger(ViewState["CANPOSTYN"].ToString()),
                                                                                int.Parse(chkNonVessel.Checked==true ? "1" : "0"),
                                                                                General.GetNullableInteger(ucBillToCompany.SelectedCompany),
                                                                                General.GetNullableInteger(ddlAccountCode.SelectedValue),
                                                                                General.GetNullableGuid(ddlSubAccount.SelectedValue)
                                                                                );
                        }
                        else
                        {
                            ucConfirmDuplicateSector.Visible = true;
                            ucConfirmDuplicateSector.Text = "Possible Duplicate Invoice.Would you like to Proceed?";
                            return;
                        }
                    }
                }
                else
                {
                    PhoenixCrewTravelInvoice.AgentInvoiceStatusUpdate(new Guid(Request.QueryString["AGENTINVOICEID"]),
                                                                        General.GetNullableDecimal(txtadjustamount.Text),
                                                                        General.GetNullableDecimal(txtpayableamount.Text),
                                                                        txtremarks.Text,
                                                                        Convert.ToInt32(ViewState["Status"].ToString()),
                                                                        General.GetNullableInteger(ucBudgetCode.SelectedBudgetCode),
                                                                        General.GetNullableInteger(rblDepDate.SelectedValue),
                                                                        General.GetNullableInteger(ddlvessel.SelectedVessel),
                                                                        General.GetNullableInteger(ddlAccountDetails.SelectedValue),
                                                                        General.GetNullableGuid(ucOwnerBudgetCode.SelectedValue),
                                                                        ref iDtKey,
                                                                        ref iRevenueDtKey,
                                                                        General.GetNullableInteger(ViewState["CANPOSTYN"].ToString()),
                                                                        int.Parse(chkNonVessel.Checked == true ? "1" : "0"),
                                                                        General.GetNullableInteger(ucBillToCompany.SelectedCompany),
                                                                        General.GetNullableInteger(ddlAccountCode.SelectedValue),
                                                                        General.GetNullableGuid(ddlSubAccount.SelectedValue)
                                                                        );
                }

                PopulateData();
                string Script = "";
                Script += "<script language=JavaScript id='BookMarkScript1'>" + "\n";
                Script += "fnReloadList();";
                Script += "</script>" + "\n";

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript1", Script);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidData(string payableamount, string budgetcode)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        decimal amount;
        if (!Decimal.TryParse(payableamount, out amount))
        {
            ucError.ErrorMessage = "Payable Amount Rquired";
        }

        if (chkNonVessel.Checked == true)
        {
            if (General.GetNullableInteger(ucBillToCompany.SelectedCompany) == null)
                ucError.ErrorMessage = "Bill to Company is required.";

            if (General.GetNullableInteger(ddlAccountCode.SelectedValue) == null)
                ucError.ErrorMessage = "Account code is required.";
        }
        else
        {
            if (General.GetNullableInteger(ucBillToCompany.SelectedCompany) == null)
                ucError.ErrorMessage = "Bill to Company is required.";

            if (General.GetNullableInteger(ddlvessel.SelectedVessel) == null)
                ucError.ErrorMessage = "Vessel Chargeable is required.";

            if (General.GetNullableInteger(ddlAccountDetails.SelectedValue) == null)
                ucError.ErrorMessage = "Vessel Account is required.";

            if (General.GetNullableInteger(ucBudgetCode.SelectedBudgetCode) == null)
                ucError.ErrorMessage = "Budget Code is required.";

            if (General.GetNullableGuid(ucOwnerBudgetCode.SelectedValue) == null)
                ucError.ErrorMessage = "Owner Budget Code is required.";
        }

        return (!ucError.IsError);
    }

    protected void gvList_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
    }


    private void PopulateData()
    {
        try
        {
            populateInvoiceData();
            populateTicketData();
            if (value1 != null && value2 != null)
                InvoiceDifference();
            TabControlBind();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void InvoiceDifference()
    {
        txtDifferencePercentage.Text = ((value1 - value2) / (value2 + (value2 / 100))).ToString();
    }
    private void populateInvoiceData()
    {
        DataSet ds = new DataSet();

        ds = PhoenixCrewTravelInvoice.AgentInvoiceDataEdit(new Guid(Request.QueryString["AGENTINVOICEID"]), officeyn);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtInvoiceNo.Text = dr["FLDINVOICENUMBER"].ToString();
            txtinvRequestno.Text = dr["FLDREQUISITIONNO"].ToString();
            txtinvpassname.Text = dr["FLDPASSENGERNAME"].ToString();
            txtinvticketno.Text = dr["FLDTICKETNO"].ToString();
            txtinvPNR.Text = dr["FLDPNRNO"].ToString();
            txtinvdepdate.Text = dr["FLDDEPARTUREDATE"].ToString();
            txtinvvessel.Text = dr["FLDVESSELNAME"].ToString();
            txtinvairlinecode.Text = dr["FLDAIRLINENUMBER"].ToString();
            txtinvbasic.Text = dr["FLDBASIC"].ToString();
            txtinvtax.Text = dr["FLDTOTALTAX"].ToString();
            txtinvdiscount.Text = dr["FLDDISCOUNT"].ToString();
            txtinvcancell.Text = dr["FLDCANCELLATIONCHARGE"].ToString();
            txtinvtotal.Text = dr["FLDTOTAL"].ToString();

            txtadjustamount.Text = dr["FLDADJUSTABLEAMOUNT"].ToString();
            txtpayableamount.Text = dr["FLDTOTAL"].ToString();
            txtremarks.Text = dr["FLDREMARKS"].ToString();
            txtAgentRemarks.Text = dr["FLDAGENTREMARKS"].ToString();
            txtSector1.Text = dr["FLDSECTOR1"].ToString();
            txtSector2.Text = dr["FLDSECTOR2"].ToString();
            txtSector3.Text = dr["FLDSECTOR3"].ToString();
            txtSector4.Text = dr["FLDSECTOR4"].ToString();

            value1 = General.GetNullableDecimal(dr["FLDTOTAL"].ToString());
            ViewState["Status"] = dr["FLDSTATUS"].ToString();
            System.Web.UI.WebControls.ListItem item = rblstatus.Items.FindByValue(dr["FLDSTATUS"].ToString());
            if (item != null)
                rblstatus.Items.FindByValue(dr["FLDSTATUS"].ToString()).Selected = true;
            ucToolTip.Text = dr["FLDAGENTREMARKS"].ToString();

            txtUser.Text = dr["FLDSTAFFNAME"].ToString();
            txtAgentRemarks.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTip.ToolTip + "', 'display');");
            txtAgentRemarks.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTip.ToolTip + "', 'none');");

            if (dr["FLDSTATUS"].ToString() == "1168")
            {
                CrewMenu.Visible = false;
                ucBillToCompany.CssClass = "input_mandatory";
                ddlAccountDetails.CssClass = "input_mandatory";
                ddlvessel.CssClass = "input_mandatory";
                ddlAccountCode.CssClass = "input_mandatory";
                ddlSubAccount.CssClass = "input_mandatory";
                ucBudgetCode.CssClass = "input_mandatory";
                ucOwnerBudgetCode.CssClass = "input_mandatory";
            }
            else
            {
                CrewMenu.Visible = true;

                ucBillToCompany.CssClass = "";
                ddlAccountDetails.CssClass = "";
                ddlvessel.CssClass = "";
                ddlAccountCode.CssClass = "";
                ddlSubAccount.CssClass = "";
                ucBudgetCode.CssClass = "";
                ucOwnerBudgetCode.CssClass = "";
            }
        }
    }
    private void populateTicketData()
    {
        DataSet ds = new DataSet();

        ds = PhoenixCrewTravelInvoice.AgentTicketDataEdit(new Guid(Request.QueryString["AGENTINVOICEID"]), officeyn);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtRequestNo.Text = dr["FLDREQUISITIONNO"].ToString();
            txtpassengername.Text = dr["FLDPASSENGERNAME"].ToString();
            txtticket.Text = dr["FLDTICKETNO"].ToString();
            txtpnr.Text = dr["FLDPNRNO"].ToString();
            txtdepdate.Text = dr["FLDDEPARTUREDATE"].ToString();
            txtvessel.Text = dr["FLDVESSELNAME"].ToString();
            txtairlinecode.Text = dr["FLDAIRLINENUMBER"].ToString();
            txtbasic.Text = dr["FLDBASIC"].ToString();
            txttax.Text = dr["FLDTOTALTAX"].ToString();
            txtdiscount.Text = dr["FLDDISCOUNT"].ToString();
            txtTktStatus.Text = dr["FLDTICKETSTATUS"].ToString();
            txtCancelReason.Text = dr["FLDCANCELLEDREASONNAME"].ToString();
            ucBudgetCode.SelectedBudgetCode = dr["FLDBUDGETID"].ToString();
            ViewState["VesselId"] = dr["FLDVESSELID"].ToString();
            ViewState["EmployeeId"] = dr["FLDEMPLOYEEID"].ToString();
            ViewState["BudgetCode"] = dr["FLDBUDGETID"].ToString();
            ViewState["TRAVELID"] = dr["FLDTRAVELID"].ToString();

            ddlvessel.SelectedVessel = ViewState["VesselId"].ToString();
            ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(General.GetNullableInteger(ViewState["VesselId"].ToString()), 1);
            ddlAccountDetails.DataBind();

            if (dr["FLDVESSELACCOUNTID"].ToString() != "")
                ddlAccountDetails.SelectedValue = dr["FLDVESSELACCOUNTID"].ToString();

            if (dr["FLDDATEFROMINVOICEYN"].ToString() == "1")
                rblDepDate.SelectedValue = "1";

            if (dr["FLDOWNERBUDGETCODEID"].ToString() != "")
            {
                ucOwnerBudgetCode.SelectedValue = dr["FLDOWNERBUDGETCODEID"].ToString();
                ucOwnerBudgetCode.Text = dr["FLDOWNERBUDGETCODE"].ToString();
            }

            if (ddlAccountDetails.Items.Count == 2)
            {
                ddlAccountDetails.SelectedIndex = 1;
            }

            txttotal.Text = dr["FLDTOTAL"].ToString();
            value2 = General.GetNullableDecimal(dr["FLDTOTAL"].ToString());


            chkNonVessel.Checked = dr["FLDNONVESSELYN"].ToString() == "1" ? true : false;
            ucBillToCompany.SelectedCompany = dr["FLDBILLTOCOMPANY"].ToString();
            ucPayingCompany.SelectedCompany = dr["FLDPAYINGCOMPANY"].ToString();

            BindAccountCode();
            if (dr["FLDNONVESSELACCOUNTID"].ToString() != "")
                ddlAccountCode.SelectedValue = dr["FLDNONVESSELACCOUNTID"].ToString();

            BindSubAccount();
            if (dr["FLDNONVESSELSUBACCOUNTMAPID"].ToString() != "")
                ddlSubAccount.SelectedValue = dr["FLDNONVESSELSUBACCOUNTMAPID"].ToString();

            DataSet dsaccount = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(General.GetNullableInteger(Convert.ToString(ViewState["VesselId"])), 1);
            if (dsaccount.Tables[0].Rows.Count > 0)
            {
                Getprincipal(Convert.ToInt32(dsaccount.Tables[0].Rows[0]["FLDACCOUNTID"]));
            }

            CheckData();
            EnableControls();
        }
    }

    private void populateTicketList()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string depDate;
        if (rblDepDate.SelectedValue == "1")
        {
            depDate = txtinvdepdate.Text;
        }
        else
        {
            depDate = txtdepdate.Text;
        }
        if (ViewState["EmployeeId"] != null && ViewState["EmployeeId"].ToString() != "")
        {
            DataSet ds = PhoenixCrewTravelInvoice.TicketIssuedList(new Guid(ViewState["AGENTINVOICEID"].ToString()),
                                                                    Int32.Parse(ViewState["EmployeeId"].ToString()),
                                                                    Convert.ToDateTime(depDate),
                                                                    (int)ViewState["PAGENUMBER"],
                                                                    gvList.PageSize,
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount);


            gvList.DataSource = ds;
            gvList.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
    }


    protected void gvList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvList.CurrentPageIndex + 1;

        populateTicketList();
    }

    protected void gvList_ItemDataBound1(object sender, GridItemEventArgs e)
    {

    }

    protected void gvList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void CheckData()
    {
        if ((txtinvRequestno.Text.Trim()).Replace(" ", "") != (txtRequestNo.Text.Trim()).Replace(" ", ""))
            txtinvRequestno.BorderColor = System.Drawing.Color.Red;
        if (txtinvdepdate.Text.ToString() != txtdepdate.Text.ToString())
            txtinvdepdate.CssClass = "input_mandatory";
        if (txtinvvessel.Text.ToString().Trim() != txtvessel.Text.ToString().Trim())
            txtinvvessel.BorderColor = System.Drawing.Color.Red;
    }
    private void HideLables()
    {
        lbl1.Attributes.Add("style", "display:none");
        lbl2.Attributes.Add("style", "display:none");
        lbl5.Attributes.Add("style", "display:none");
        lbl6.Attributes.Add("style", "display:none");
    }

    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        ucOwnerBudgetCode.SelectedValue = "";
        ucOwnerBudgetCode.Text = "";

        ddlAccountDetails.DataSource = "";
        ddlAccountDetails.SelectedValue = "";
        ddlAccountDetails.Text = "";
        
        ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
            General.GetNullableInteger(ddlvessel.SelectedVessel) == null ? 0 : General.GetNullableInteger(ddlvessel.SelectedVessel), 1);
        ddlAccountDetails.DataBind();
        if (ddlAccountDetails.Items.Count == 2)
        {
            ddlAccountDetails.SelectedIndex = 1;
        }
        ViewState["VesselId"] = ddlvessel.SelectedVessel;
        DataSet dsaccount = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(General.GetNullableInteger(Convert.ToString(ViewState["VesselId"])), 1);
        if (dsaccount.Tables[0].Rows.Count > 0)
        {
            Getprincipal(Convert.ToInt32(dsaccount.Tables[0].Rows[0]["FLDACCOUNTID"]));
        }
        BindOwnerBudgetCode();
    }

    protected void ucBudgetCode_Changed(object sender, EventArgs e)
    {
        ucOwnerBudgetCode.SelectedValue = "";
        ucOwnerBudgetCode.Text = "";

        ViewState["BudgetCode"] = ucBudgetCode.SelectedBudgetCode;
        BindOwnerBudgetCode();
    }

    private void BindOwnerBudgetCode()
    {
        if (Convert.ToString(ViewState["PRINCIPAL"]) != "" && ViewState["BudgetCode"].ToString() != "Dummy")
        {
            ucOwnerBudgetCode.OwnerId = ViewState["PRINCIPAL"].ToString();
            ucOwnerBudgetCode.BudgetId = ViewState["BudgetCode"].ToString();

            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? iownerid = 0;
            DataSet ds1 = PhoenixCommonRegisters.InternalBillingOwnerBudgetCodeSearch(null
                                                                                     , null
                                                                                     , General.GetNullableInteger(Convert.ToString(ViewState["PRINCIPAL"]))
                                                                                     , null
                                                                                     , General.GetNullableInteger(ViewState["BudgetCode"].ToString())
                                                                                     , null, null
                                                                                     , 1
                                                                                     , General.ShowRecords(null)
                                                                                     , ref iRowCount
                                                                                     , ref iTotalPageCount
                                                                                     , ref iownerid);

            if (ds1.Tables[0].Rows.Count > 0)
                ViewState["OwnerBudgetCode"] = "1";
            else
                ViewState["OwnerBudgetCode"] = "";
            if (ds1.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds1.Tables[0].Rows[0];
                ucOwnerBudgetCode.SelectedValue = dr["FLDOWNERBUDGETCODEMAPID"].ToString();
                ucOwnerBudgetCode.Text = dr["FLDOWNERBUDGETCODE"].ToString();
            }
        }
    }
    protected void rblstatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Status"] = rblstatus.SelectedValue.ToString();
        if (rblstatus.SelectedValue == "1168")
        {
            ucBillToCompany.CssClass = "input_mandatory";
            ddlAccountDetails.CssClass = "input_mandatory";
            ddlvessel.CssClass = "input_mandatory";
            ddlAccountCode.CssClass = "input_mandatory";
            ddlSubAccount.CssClass = "input_mandatory";
            ucBudgetCode.CssClass = "input_mandatory";
            ucOwnerBudgetCode.CssClass = "input_mandatory";

            TabControlBind();
        }
        else
        {
            ucBillToCompany.CssClass = "";
            ddlAccountDetails.CssClass = "";
            ddlvessel.CssClass = "";
            ddlAccountCode.CssClass = "";
            ddlSubAccount.CssClass = "";
            ucBudgetCode.CssClass = "";
            ucOwnerBudgetCode.CssClass = "";
        }
    }

    protected void btnConfirmDuplicateSector_Click(object sender, EventArgs e)
    {
        try
        {

            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                Guid iDtkey = new Guid();
                Guid iRevenueDtKey = new Guid();
                PhoenixCrewTravelInvoice.AgentInvoiceStatusUpdate(new Guid(Request.QueryString["AGENTINVOICEID"]),
                                                                General.GetNullableDecimal(txtadjustamount.Text),
                                                                General.GetNullableDecimal(txtpayableamount.Text),
                                                                txtremarks.Text,
                                                                Convert.ToInt32(ViewState["Status"].ToString()),
                                                                General.GetNullableInteger(ucBudgetCode.SelectedBudgetCode),
                                                                General.GetNullableInteger(rblDepDate.SelectedValue),
                                                                General.GetNullableInteger(ddlvessel.SelectedVessel),
                                                                General.GetNullableInteger(ddlAccountDetails.SelectedValue),
                                                                General.GetNullableGuid(ucOwnerBudgetCode.SelectedValue),
                                                                ref iDtkey,
                                                                ref iRevenueDtKey,
                                                                General.GetNullableInteger(ViewState["CANPOSTYN"].ToString()),
                                                                int.Parse(chkNonVessel.Checked == true ? "1" : "0"),
                                                                General.GetNullableInteger(ucBillToCompany.SelectedCompany),
                                                                General.GetNullableInteger(ddlAccountCode.SelectedValue),
                                                                General.GetNullableGuid(ddlSubAccount.SelectedValue)
                                                                );

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void CreateReport(Guid agentInvoiceId, Guid dtkey)
    {
        DataSet ds = PhoenixCrewTravelInvoice.TravelPostReport(agentInvoiceId);
        if (ds.Tables.Count > 0)
        {
            AddAttachment(ds, dtkey);
        }
    }
    private string AddAttachment(DataSet ds, Guid dtkey)
    {
        FontFactory.RegisterDirectories();
        Font fontNormal = new Font(FontFactory.GetFont("Arial", 12, Font.NORMAL));

        string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts/";

        //string path = HttpContext.Current.Request.MapPath("~/Attachments/Accounts/");
        string filefullpath = path + dtkey + ".pdf";

        ConvertToPdf(HtmlTableDataContent(ds), filefullpath);
        return filefullpath;
    }
    private string HtmlTableDataContent(DataSet ds)
    {
        StringBuilder sbHtmlContent = new StringBuilder();

        sbHtmlContent.Append("<div align='left'>");
        sbHtmlContent.Append("<table ID='tbl1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<img src='http://" + Request.Url.Authority + Session["images"] + "/UnisonTravel_Logo.png" + "' />");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td colspan='8'>");
        sbHtmlContent.Append("<b>");
        sbHtmlContent.Append("<font style=\"font-family:Calibri (Body);font-size:20px;\">");
        sbHtmlContent.Append("Unison Universals Travel Limited");
        sbHtmlContent.Append("</b>");
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("</div>");
        sbHtmlContent.Append("<br/>");

        sbHtmlContent.Append("<table width='100%'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='7' valign='top'>");
        sbHtmlContent.Append("<table width='100%'>");
        sbHtmlContent.Append("<tr>");

        sbHtmlContent.Append("<td width='40%' valign='top' bgcolor='#FAFAD2' border='1' style=\"font-family:times new roman;font-size:10px;\">");

        sbHtmlContent.Append("<b>");
        sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:12px;\">");
        sbHtmlContent.Append("BILL TO");
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("</b>");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDCOMPANYNAME"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDCOMPANYADDRESS"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDADDRESS2"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDADDRESS3"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDADDRESS4"].ToString());
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("<td width=\"20%\" align='center' valign='middle'>");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("<td width='40%' bgcolor='#FAFAD2' border='1' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>INVOICE NO:</b>");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDREVENUEVOUCHERNUMBER"].ToString());
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<b>DATE:</b>");
        sbHtmlContent.Append(General.GetDateTimeToString(ds.Tables[1].Rows[0]["FLDINVOICEDATE"].ToString()));
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<b>ORDER:</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("REF :" + ds.Tables[1].Rows[0]["FLDTARGETCOPURCHASEINVOICENO"].ToString());
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<br />");

        sbHtmlContent.Append("<table ID='tbl2' width='100%' border='1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td width=\"5%\" style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>S.NO.</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td colspan='2' width='35%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>DESCRIPTION</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='20%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>FLIGHT DATE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='20%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>BASE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='20%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>TAX</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='20%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>TOTAL FARE (USD)</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td valign='top' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("1");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' colspan='2' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDDESCRIPTION"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(General.GetDateTimeToString(ds.Tables[1].Rows[0]["FLDDEPARTUREDATE"].ToString()));
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDBASE"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTALTAX"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTAL"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        sbHtmlContent.Append("<table ID='tbl4' width='100%' border='1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td  style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>Our Banking Details:</b>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("The Hongkong and Shanghai Bank");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("Swift Code: HSBCHKHK");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("Account Number: 801-042771-838");
        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("<td width='50px' rowspan='3'  style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<table ID='tbl3' width='100%' border='0'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='2' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>BASE FARE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>USD</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDBASE"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='2' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>TAXES AND SURCHARGES</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>USD</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTALTAX"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");


        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='2' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>AMOUNT PAYABLE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>USD</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTAL"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<footer>");
        sbHtmlContent.Append("<p align=\"center\">");
        sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("Suite B, 11th Floor, Hang Seng Causeway Bay Building, 28 Yee Wo Street, Causeway Bay, Hong Kong");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("E-mail: travel@unisonuniversals.com");
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("</p>");
        sbHtmlContent.Append("</footer>");
        return sbHtmlContent.ToString();
    }
    public void ConvertToPdf(string HTMLString, string fileLocation)
    {
        Document document = new Document(new Rectangle(595f, 842f));
        document.SetMargins(36f, 36f, 36f, 0f);

        PdfWriter.GetInstance(document, new FileStream(fileLocation, FileMode.Create));
        document.Open();

        StyleSheet styles = new StyleSheet();

        ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);

        for (int k = 0; k < htmlarraylist.Count; k++)
        {
            document.Add((IElement)htmlarraylist[k]);
        }

        document.Close();
    }
    private void TabControlBind()
    {
        toolbar = new PhoenixToolbar();
        if (txtTktStatus.Text == "Cancelled")
        {
            if (rblstatus.SelectedValue == "1168")
            {
                decimal iCancelAmount = 0;
                PhoenixCrewTravelQuoteLine.checkCancellationAmount(General.GetNullableInteger(txtinvcancell.Text), ref iCancelAmount);

                if (Convert.ToDecimal(txtpayableamount.Text) <= iCancelAmount)
                {
                    toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                    ViewState["CANPOSTYN"] = 1;
                }
                else
                {
                    toolbar.AddLinkButton("javascript:openNewWindow('codehelp2', '', '" + Session["sitepath"] + "/Crew/CrewTravelExceedTicketCancellation.aspx?TravelId=" + ViewState["TRAVELID"].ToString() + "&TicketNo=" + txtinvticketno.Text + "&AGENTINVOICEID=" + Request.QueryString["AGENTINVOICEID"] + "')", "Save", "SAVE", ToolBarDirection.Right);
                    ViewState["CANPOSTYN"] = 0;
                }
            }
            else
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                ViewState["CANPOSTYN"] = 1;
            }
        }

        else
        {
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            ViewState["CANPOSTYN"] = 1;
        }

        CrewMenu.AccessRights = this.ViewState;
        CrewMenu.MenuList = toolbar.Show();

    }
    protected void chkNonVessel_CheckedChanged(object sender, EventArgs e)
    {
        EnableControls();
    }
    protected void EnableControls()
    {
        if (chkNonVessel.Checked == true)
        {
            lblAccountCode.Visible = true;
            ddlAccountCode.Visible = true;
            lblSubAccount.Visible = true;
            ddlSubAccount.Visible = true;

            lblVesselChargeable.Visible = false;
            ddlvessel.Visible = false;
            lblVesselAccount.Visible = false;
            ddlAccountDetails.Visible = false;
            lblBudgetCode.Visible = false;
            ucBudgetCode.Visible = false;
            lblOwnerBudget.Visible = false;
            ucOwnerBudgetCode.Visible = false;
        }
        else
        {
            lblAccountCode.Visible = false;
            ddlAccountCode.Visible = false;
            lblSubAccount.Visible = false;
            ddlSubAccount.Visible = false;

            lblVesselChargeable.Visible = true;
            ddlvessel.Visible = true;
            lblVesselAccount.Visible = true;
            ddlAccountDetails.Visible = true;
            lblBudgetCode.Visible = true;
            ucBudgetCode.Visible = true;
            lblOwnerBudget.Visible = true;
            ucOwnerBudgetCode.Visible = true;
        }
    }
    protected void BindBillToCompany()
    {
        ucBillToCompany.bind();
        ucBillToCompany.DataBind();

        DataSet ds = PhoenixCommonAccountsAirfareNonVessel.AirfareNonvesselregisterListByAccount(General.GetNullableInteger(ddlAccountCode.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ucBillToCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDBILLTOCOMPANYID"].ToString();
        }

    }
    protected void BindAccountCode()
    {
        ddlAccountCode.DataSource = "";

        ddlAccountCode.DataTextField = "FLDDESCRIPTION";
        ddlAccountCode.DataValueField = "FLDACCOUNTID";
        ddlAccountCode.DataSource = PhoenixCommonAccountsAirfareNonVessel.AirfareNonvesselregisterList(null);
        ddlAccountCode.DataBind();
        ddlAccountCode.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ucBillToCompany_TextChangedEvent(object sender, EventArgs e)
    {
        //BindAccountCode();
    }
    protected void ddlAccountCode_TextChanged(object sender, EventArgs e)
    {
        BindBillToCompany();
        BindSubAccount();
    }
    protected void BindSubAccount()
    {
        ddlSubAccount.DataTextField = "FLDDESCRIPTION";
        ddlSubAccount.DataValueField = "FLDSUBACCOUNTMAPID";

        if (General.GetNullableInteger(ddlAccountCode.SelectedValue) != null)
        {
            DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(ddlAccountCode.SelectedValue));

            ddlSubAccount.DataSource = "";
            ddlSubAccount.Text = "";
            ddlSubAccount.SelectedValue = "";

            int iRowCount = 0;
            int iTotalPageCount = 0;
            ddlSubAccount.DataSource = PhoenixCommonRegisters.SubAccountSearch(General.GetNullableInteger(ucBillToCompany.SelectedCompany)
                                      , General.GetNullableInteger(ddlAccountCode.SelectedValue)
                                      , General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDACCOUNTUSAGE"].ToString())
                                      , null
                                      , null
                                      , null
                                      , null
                                      , 1
                                      , 100000
                                      , ref iRowCount
                                      , ref iTotalPageCount
                                      );
            ddlSubAccount.DataBind();
        }
        ddlSubAccount.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

    }
    public void Getprincipal(int accountid)
    {
        try
        {
            DataSet ds = null;
            ds = PhoenixRegistersAccount.EditAccount(accountid);
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["PRINCIPAL"] = Convert.ToString(dr["FLDPRINCIPALID"]);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
