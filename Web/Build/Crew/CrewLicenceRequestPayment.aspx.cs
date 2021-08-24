using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;
public partial class CrewLicenceRequestPayment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "BACK");
            toolbar.AddButton("Licence Detail", "REQUEST");
            toolbar.AddButton("Payment", "PAYMENT");
            CrewLicReq.AccessRights = this.ViewState;
            CrewLicReq.MenuList = toolbar.Show();
            CrewLicReq.SelectedMenuIndex = 2;

            PhoenixToolbar toolbarLicence = new PhoenixToolbar();
            toolbarLicence = new PhoenixToolbar();
            toolbarLicence.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuLicence.AccessRights = this.ViewState;
            MenuLicence.MenuList = toolbarLicence.Show();

            if (!Page.IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                btnconfirm.Attributes.Add("style", "display:none");

                ViewState["ADVANCEPAYMENT"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 212, "AVP");
                ViewState["INVOICE"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 212, "ROI");

                ViewState["ADDRESSCODE"] = string.Empty;
                ViewState["FLAGID"] = string.Empty;
                ViewState["BANK"] = string.Empty;
                ViewState["REQUESTID"] = null;
                if (Request.QueryString["rid"] != null && Request.QueryString["rid"].ToString() != string.Empty)
                    ViewState["REQUESTID"] = Request.QueryString["rid"].ToString();

                rblPaymentBy.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 89, 0, "CMP,EMP");
                rblPaymentBy.DataBind();
                SetEmployeeDetails();
                BankBind();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["FEPAGENUMBER"] = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public void OnClickBankInfo(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["ADDRESSCODE"].ToString() != null && ViewState["FLAGID"].ToString() != "")
            {
                if (ddlBank.SelectedValue != "" && ddlBank.SelectedValue != "Dummy")
                {
                    String scriptpopup = String.Format("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewLicencePaymentBankInfo.aspx?Addresscode= " + ViewState["ADDRESSCODE"].ToString() + " &Flagid= " + ViewState["FLAGID"].ToString() + "&Bankid= " + ddlBank.SelectedValue + " ');");

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

                }
                else
                {
                    ucError.ErrorMessage = "Bank is Required";
                    ucError.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void OnClickAdvancePayment(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidLicencePayment(ddlLiabilitycompany.SelectedCompany, ucBudgetCode.SelectedBudgetCode, rblPaymentBy.SelectedValue, ddlBank.SelectedValue))
            {
                ucError.Visible = true;
                return;
            }

            RadWindowManager1.RadConfirm("Are you sure you want to make advance payment?", "btnconfirm", 320, 150, null, "Confirm");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void btnconfirm_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewLicenceRequest.UpdateLicenceRequestAdvancePayement(new Guid(ViewState["REQUESTID"].ToString()), General.GetNullableInteger(ViewState["ADVANCEPAYMENT"].ToString()), General.GetNullableInteger(rblPaymentBy.SelectedValue), General.GetNullableInteger(ddlBank.SelectedValue), General.GetNullableInteger(ddlLiabilitycompany.SelectedCompany), General.GetNullableInteger(ucBudgetCode.SelectedBudgetCode));

            PhoenixAccountsAdvancePayment.AdvancePaymentInsert(
                          PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                          int.Parse(ViewState["ADDRESSCODE"].ToString()), // supplier
                          decimal.Parse(ViewState["AMOUNT"].ToString()), // amount
                          DateTime.Parse(DateTime.Now.ToString()), // pay date
                          int.Parse(ViewState["CURRENCYID"].ToString()), //currency
                          null, //remarks
                           ViewState["REQUESTNO"].ToString(), // reference document
                          General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 127, "APD")), // payment status -- draft
                          General.GetNullableGuid(ViewState["REQUESTID"].ToString()), // order id -- licence process id
                          General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 124, "LIC")), // type - licence req.
                          General.GetNullableInteger(ddlLiabilitycompany.SelectedCompany), // companyid
                          General.GetNullableInteger(ucBudgetCode.SelectedBudgetCode), // budget code
                          General.GetNullableInteger(ViewState["Vesselid"].ToString()), // vessel id
                          General.GetNullableInteger(ddlBank.SelectedValue) // bankid
                          );
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void MenuLicence_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() == "SAVE")
            {
                if (!IsValidLicenceSave(ddlLiabilitycompany.SelectedCompany, ucBudgetCode.SelectedBudgetCode, rblPaymentBy.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewLicenceRequest.UpdateLicenceRequestPayement(new Guid(ViewState["REQUESTID"].ToString()), General.GetNullableInteger(rblPaymentBy.SelectedValue), General.GetNullableInteger(ddlBank.SelectedValue), General.GetNullableInteger(ddlLiabilitycompany.SelectedCompany), General.GetNullableInteger(ucBudgetCode.SelectedBudgetCode));

                ucStatus.Text = "Saved Successfully.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void CrewLicReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("REQUEST"))
            {
                Response.Redirect("../Crew/CrewLicenceRequestDetailList.aspx?rid=" + ViewState["REQUESTID"].ToString());
            }
            if (CommandName.ToUpper().Equals("PAYMENT"))
            {
                Response.Redirect("../Crew/CrewLicenceRequestPayment.aspx?rid=" + ViewState["REQUESTID"].ToString());
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Crew/CrewLicenceRequestList.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidLicenceSave(string Company, string Budget, string PaymentBy)
    {
        int resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!int.TryParse(Company, out resultInt))
            ucError.ErrorMessage = "Comapany is required";

        if (!int.TryParse(Budget, out resultInt))
            ucError.ErrorMessage = "Budget is required";

        if (!int.TryParse(PaymentBy, out resultInt))
            ucError.ErrorMessage = "Payment By is required";

        return (!ucError.IsError);
    }


    private bool IsValidLicencePayment(string Company, string Budget, string PaymentBy, string Bank)
    {
        int resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!int.TryParse(Company, out resultInt))
            ucError.ErrorMessage = "Comapany is required";

        if (!int.TryParse(Budget, out resultInt))
            ucError.ErrorMessage = "Budget is required";

        if (!int.TryParse(PaymentBy, out resultInt))
            ucError.ErrorMessage = "Payment By is required";

        if (!int.TryParse(Bank, out resultInt))
            ucError.ErrorMessage = "Bank is required";

        return (!ucError.IsError);
    }

    protected void SetEmployeeDetails()
    {
        try
        {
            if (ViewState["REQUESTID"] != null)
            {
                DataTable dt = PhoenixCrewLicenceRequest.EditCrewLicenceRequest(new Guid(ViewState["REQUESTID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    ViewState["REQUESTNO"] = dt.Rows[0]["FLDREQUISITIONNUMBER"].ToString();
                    txtReferenceNo.Text = dt.Rows[0]["FLDREQUISITIONNUMBER"].ToString();
                    txtEmployeeName.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
                    txtEmployeeNumber.Text = dt.Rows[0]["FLDFILENO"].ToString();
                    txtFlag.Text = dt.Rows[0]["FLDFLAGNAME"].ToString();
                    txtVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                    ViewState["Empid"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                    ViewState["Date"] = dt.Rows[0]["FLDCREWCHANGEDATE"].ToString();
                    txtDate.Text = dt.Rows[0]["FLDCREWCHANGEDATE"].ToString();
                    ViewState["Rankid"] = dt.Rows[0]["FLDRANKID"].ToString();
                    txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                    ViewState["Vesselid"] = dt.Rows[0]["FLDVESSELID"].ToString();
                    ViewState["FLAGID"] = dt.Rows[0]["FLDFLAGID"].ToString();
                    ViewState["ADDRESSCODE"] = dt.Rows[0]["FLDADDRESSCODE"].ToString();
                    txtConsulate.Text = dt.Rows[0]["FLDCONSULATENAME"].ToString();
                    ViewState["BANK"] = dt.Rows[0]["FLDBANKID"].ToString();
                    rblPaymentBy.SelectedValue = dt.Rows[0]["FLDPAYMENTBY"].ToString();
                    ddlLiabilitycompany.SelectedCompany = dt.Rows[0]["FLDBILLTOCOMPANY"].ToString();
                    ucBudgetCode.SelectedBudgetCode = dt.Rows[0]["FLDBUDGETID"].ToString();
                    ViewState["AMOUNT"] = dt.Rows[0]["FLDAMOUNT"].ToString();
                    txtCurrency.Text = dt.Rows[0]["FLDCURRENCYNAME"].ToString();
                    ViewState["CURRENCYID"] = dt.Rows[0]["FLDCURRENCYID"].ToString();

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BankBind()
    {
        DataSet ds = PhoenixRegistersAddress.ListBankAddress(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString()), null);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBank.DataSource = ds;
            ddlBank.DataTextField = "FLDBANKNAME";
            ddlBank.DataValueField = "FLDBANKID";
            ddlBank.DataBind();
            ddlBank.SelectedValue = ViewState["BANK"].ToString();
        }
        else
        {
            ddlBank.Items.Clear();
            ddlBank.DataBind();
            ddlBank.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

        ViewState["PAGENUMBER"] = 1;
    }
    protected void BindNatData()
    {
        try
        {
            string strEmployeeId = ViewState["Empid"].ToString();
            string strRankId = ViewState["Rankid"].ToString();
            string strVesselId = ViewState["Vesselid"].ToString();
            string strCrewChangeDate = ViewState["Date"].ToString();

            DataTable dt = PhoenixCrewLicence.NationalLicenceMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), DateTime.Parse(strCrewChangeDate));

            gvMissingLicence.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMissingLicence_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindNatData();

    }

    protected void gvMissingLicence_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
            RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");

            if (lblMissingYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/blue.png";
            }
            else if (lblExpiredYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/red.png";
            }
        }

    }


    protected void gvFlag_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindFlagData();
    }


    protected void BindFlagData()
    {
        try
        {
            string strEmployeeId = ViewState["Empid"].ToString();
            string strRankId = ViewState["Rankid"].ToString();
            string strVesselId = ViewState["Vesselid"].ToString();
            string strCrewChangeDate = ViewState["Date"].ToString();

            DataTable dt = PhoenixCrewLicence.FlagLicenceMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), DateTime.Parse(strCrewChangeDate), General.GetNullableGuid(ViewState["REQUESTID"].ToString()));

            gvFlag.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFlag_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
            RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");
            if (lblMissingYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/blue.png";
            }
            else if (lblExpiredYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/red.png";
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
            }
        }
    }

    protected void gvFlag_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string requestdetailid = ((RadLabel)e.Item.FindControl("lblRequestDetailIdEdit")).Text;
            string amt = ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text;

            if (!IsValidLicenceDocument(requestdetailid))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewLicenceRequest.UpdateLicenceAmount(new Guid(requestdetailid)
                                                          , General.GetNullableDecimal(amt));

            BindFlagData();
            gvFlag.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvDCE_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDCEData();
    }


    protected void BindDCEData()
    {
        try
        {
            string strEmployeeId = ViewState["Empid"].ToString();
            string strRankId = ViewState["Rankid"].ToString();
            string strVesselId = ViewState["Vesselid"].ToString();
            string strCrewChangeDate = ViewState["Date"].ToString();

            DataTable dt = PhoenixCrewLicence.DCEMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), DateTime.Parse(strCrewChangeDate), General.GetNullableGuid(ViewState["REQUESTID"].ToString()));

            gvDCE.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDCE_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
            RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");
            if (lblMissingYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/blue.png";
            }
            else if (lblExpiredYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/red.png";
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
            }
        }
    }

    protected void gvDCE_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string requestdetailid = ((RadLabel)e.Item.FindControl("lblRequestDetailIdEdit")).Text;
            string amt = ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text;

            if (!IsValidLicenceDocument(requestdetailid))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewLicenceRequest.UpdateLicenceAmount(new Guid(requestdetailid)
                                                          , General.GetNullableDecimal(amt));


            BindDCEData();
            gvDCE.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }



    protected void gvSeamanBook_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindSeamansBookData();
    }



    protected void BindSeamansBookData()
    {
        try
        {
            string strEmployeeId = ViewState["Empid"].ToString();
            string strRankId = ViewState["Rankid"].ToString();
            string strVesselId = ViewState["Vesselid"].ToString();
            string strCrewChangeDate = ViewState["Date"].ToString();

            DataTable dt = PhoenixCrewLicence.SeamansMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), DateTime.Parse(strCrewChangeDate), General.GetNullableGuid(ViewState["REQUESTID"].ToString()));

            gvSeamanBook.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSeamanBook_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
            RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");
            if (lblMissingYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/blue.png";
            }
            else if (lblExpiredYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/red.png";
            }
            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
            }
        }
    }

    protected void gvSeamanBook_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string requestdetailid = ((RadLabel)e.Item.FindControl("lblRequestDetailIdEdit")).Text;
            string amt = ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text;

            if (!IsValidLicenceDocument(requestdetailid))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewLicenceRequest.UpdateLicenceAmount(new Guid(requestdetailid)
                                                          , General.GetNullableDecimal(amt));

            BindSeamansBookData();
            gvSeamanBook.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCourse_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCourseData();
    }

    protected void BindCourseData()
    {
        try
        {
            string strEmployeeId = ViewState["Empid"].ToString();
            string strRankId = ViewState["Rankid"].ToString();
            string strVesselId = ViewState["Vesselid"].ToString();
            string strCrewChangeDate = ViewState["Date"].ToString();

            DataTable dt = PhoenixCrewLicence.CourseMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), DateTime.Parse(strCrewChangeDate), General.GetNullableGuid(ViewState["REQUESTID"].ToString()));

            gvCourse.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCourse_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
            RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");
            if (lblMissingYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/blue.png";
            }
            else if (lblExpiredYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/red.png";
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
            }
        }
    }

    protected void gvCourse_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string requestdetailid = ((RadLabel)e.Item.FindControl("lblRequestDetailIdEdit")).Text;
            string amt = ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text;

            if (!IsValidLicenceDocument(requestdetailid))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewLicenceRequest.UpdateLicenceAmount(new Guid(requestdetailid)
                                                          , General.GetNullableDecimal(amt));
            BindCourseData();
            gvCourse.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    private bool IsValidLicenceDocument(string requestdetailid)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (requestdetailid.Trim() == "")
            ucError.ErrorMessage = "Selected Licence Not Requested";

        return (!ucError.IsError);
    }


}