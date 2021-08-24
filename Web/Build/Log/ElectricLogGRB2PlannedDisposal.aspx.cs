using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogGRB2PlannedDisposal : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    Guid logBookId = Guid.Empty;
    int logId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        ShowToolBar();
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        logId = PhoenixMarpolLogGRB2.GetGRB2LogId(usercode, "PlannedDisposal");

        if (string.IsNullOrWhiteSpace(Request.QueryString["LogBookId"]) == false)
        {
            logBookId = Guid.Parse(Request.QueryString["LogBookId"]);
        }


        if (IsPostBack == false)
        {
            ViewState["dtKey"] = Guid.Empty;
            ViewState["isAttachmentAttached"] = false;
            showHideAttachment(false);
            dtPicker.SelectedDate = DateTime.Now;
            dtPicker.MaxDate = DateTime.Now;
            LoadNotes();
            LoadCategory();
            LoadMethod();
            BindData();
        }
    }

    private void LoadMethod()
    {
        string[] methods = new string[] { "Discharge into sea", "Discharge to reception"};
        ddlMethod.DataSource = methods;
        ddlMethod.DataBind();
    }

    private void checkAttachmentAttached()
    {

        DataTable dt = PhoenixCommonFileAttachment.AttachmentList(Guid.Parse(ViewState["dtKey"].ToString()));
        if (dt != null & dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            ViewState["isAttachmentAttached"] = true;
            attachmentIcon.Attributes.Add("class", "fas fa-paperclip");
            string icon = (bool)ViewState["isAttachmentAttached"] ? "fa-paperclip" : "fa-paperclip-na";
            lnkAttachment.Text = string.Format("<span class='icon'><i class='fas {0}'></i></span>", icon);
        }
        else
        {
            ViewState["isAttachmentAttached"] = false;
            attachmentIcon.Attributes.Add("class", "fas fa-paperclip-na");
            string icon = (bool)ViewState["isAttachmentAttached"] ? "fa-paperclip" : "fa-paperclip-na";
            lnkAttachment.Text = string.Format("<span class='icon'><i class='fas {0}'></i></span>", icon);
        }
    }

    private void setAttachment()
    {
        if (ViewState["dtKey"] != null && (Guid)ViewState["dtKey"] == Guid.Empty)
        {
            ViewState["dtKey"] = Guid.NewGuid();
        }
        lnkAttachment.Attributes.Add("onclick", "openNewWindow('attachment','','Common/CommonFileAttachment.aspx?dtkey=" + ViewState["dtKey"].ToString() + "&MOD=LOG&RefreshWindowName=Log" + "');return false;");
    }

    private void showHideAttachment(bool isVisible)
    {
        string display = isVisible ? "display:intial;" : "display:none;";
        string icon = (bool)ViewState["isAttachmentAttached"] ? "fa-paperclip" : "fa-paperclip-na";
        lnkAttachment.Attributes.Add("style", display);
        lnkAttachment.Text = string.Format("<span class='icon'><i class='fas {0}'></i></span>", icon);
    }

    private void LoadCategory()
    {
        ddlCategory.DataSource = PhoenixMarpolLogGRB2.GetCategoryList();
        ddlCategory.DataValueField = "FLDCODEID";
        ddlCategory.DataTextField = "FLDSHORTCODE";
        ddlCategory.DataBind();
    }

    private void LoadNotes()
    {
        DataTable dt = PhoenixMarpolLogGRB2.GRB2LogRegisterEdit(usercode, logId);
        if (dt.Rows.Count > 0)
        {
            lblnotes.Text = dt.Rows[0]["FLDNOTES"].ToString();
        }
        else
        {
            lblnotes.Text = "No Notes";
        }
    }

    private void BindData()
    {
        if (logBookId != Guid.Empty)
        {
            DataTable dt = PhoenixMarpolLogGRB2.BookEntryReportSearchById(usercode, vesselId, logBookId);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                dtPicker.SelectedDate = Convert.ToDateTime(row["FLDDATE"]);
                txtVesselName.Text = (string)row["FLDPORTPOSITION"];

                if (row["FLDDISCHARGEINTOSEA"] != DBNull.Value)
                {
                    setMethodDropDown("Discharge into sea", row["FLDDISCHARGEINTOSEA"].ToString());
                }
                else if (row["FLDDISCHARGEINTORF"] != DBNull.Value)
                {
                    setMethodDropDown("Discharge to reception", row["FLDDISCHARGEINTORF"].ToString());
                }
                else
                {
                    setMethodDropDown("Incineration", row["FLDINCINERATED"].ToString());
                }
                txtRemarks.Text = (string)row["FLDREMARKS"];

                RadComboBoxItem categoryItem = ddlCategory.Items.FindItem(x => x.Value == row["FLDCATEGORY"].ToString());
                if (categoryItem != null)
                {
                    categoryItem.Selected = true;
                }

                if (string.IsNullOrWhiteSpace(ddlMethod.SelectedItem.Text) == false && ddlMethod.SelectedItem.Text == "Discharge to reception")
                {
                    ViewState["dtKey"] = (Guid)row["FLDDTKEY"];
                    setAttachment();
                    showHideAttachment(true);
                    checkAttachmentAttached();
                }

                dtPicker.Enabled = false;
                ddlMethod.Enabled = false;
                ddlCategory.Enabled = false;
            }
        }
    }

    private void setMethodDropDown(string method, string value)
    {
        RadComboBoxItem methodItem = ddlMethod.Items.FindItem(x => x.Text == method);
        methodItem.Selected = true;
        txtValue.Text = value;
    }

    private void OnChiefOfficerSign()
    {
        if (Filter.DutyEngineerSignatureFilterCriteria != null && Filter.DutyEngineerSignatureFilterCriteria["rank"].ToLower() == "co")
        {
            NameValueCollection nvc = Filter.DutyEngineerSignatureFilterCriteria;

            DateTime signedDate = DateTime.Parse(nvc.Get("date"));
            lblincsign.Text = string.Format("{0} {1}",  nvc.Get("name"), signedDate.ToString("dd-MM-yyyy"));
            lblinchName.Text = nvc.Get("name");
            lblincRank.Text = nvc.Get("rank");
            lblinchId.Text = nvc.Get("id");
            lblincSignDate.Text = signedDate.ToString();
            Filter.DutyEngineerSignatureFilterCriteria = null;

            btnInchargeSign.Visible = false;
            lblincsign.Visible = true;

        }
    }

    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixMarpolLogGRB2.GetLastTransactionDate(vesselId, usercode);
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (isValidInput() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                if (logBookId == Guid.Empty)
                {
                    logBookEntryInsert();
                }
                else
                {
                    logBookEntryUpdate();
                }
                // close the popup and refresh the list
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
            }
        }
        catch (Exception ex)
        {
            if (ex.Message == "EXCEED85")
                RadWindowManager1.RadConfirm("Warning. Tank Reached its 85 percentage capacity", "confirm", 320, 150, null, "Exceed");
            else
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }

    private void logBookEntryUpdate()
    {
        decimal? value = General.GetNullableDecimal(txtValue.Text);
        decimal? incineration = ddlMethod.SelectedItem.Value == "Incineration" ? value : null;
        decimal? amtDischargeIntoReception = ddlMethod.SelectedItem.Value == "Discharge to reception" ? value : null;
        decimal? amtDischargeIntoSea = ddlMethod.SelectedItem.Value == "Discharge into sea" ? value : null;

        PhoenixMarpolLogGRB2.BookEntryUpdate(usercode, vesselId, logBookId, logId, dtPicker.SelectedDate.Value,
            txtVesselName.Text, ddlCategory.SelectedValue, amtDischargeIntoSea, amtDischargeIntoReception, incineration
            , txtRemarks.Text, null);

        PhoenixMarpolLogGRB2.LogGRB2BookEntryStatusUpdate(usercode, logId, logBookId, Convert.ToInt32(lblinchId.Text), lblinchName.Text, lblincRank.Text, Convert.ToDateTime(lblincSignDate.Text), null, null, null, null, (bool)ViewState["isAttachmentAttached"]);
    }

    private void logBookEntryInsert()
    {
        Guid logBookTransferId = Guid.Empty;
        decimal? value = General.GetNullableDecimal(txtValue.Text);
        decimal? incineration = ddlMethod.SelectedItem.Value == "Incineration" ? value : null;
        decimal? amtDischargeIntoReception = ddlMethod.SelectedItem.Value == "Discharge to reception" ? value : null;
        decimal? amtDischargeIntoSea = ddlMethod.SelectedItem.Value == "Discharge into sea" ? value : null;
        Guid? transferDtKey = (Guid)ViewState["dtKey"] == Guid.Empty ? null : (Guid?)Guid.Parse(ViewState["dtKey"].ToString());

        PhoenixMarpolLogGRB2.BookEntryInsert(usercode, vesselId, logId, dtPicker.SelectedDate.Value,
            txtVesselName.Text, ddlCategory.SelectedValue, amtDischargeIntoSea, amtDischargeIntoReception, incineration
            , txtRemarks.Text, null, false, transferDtKey, false, ref logBookTransferId);


            PhoenixMarpolLogGRB2.LogGRB2BookEntryStatusInsert(usercode, logId, logBookTransferId, Convert.ToInt32(lblinchId.Text), lblinchName.Text, lblincRank.Text, Convert.ToDateTime(lblincSignDate.Text), null, null, null, null, false, (bool)ViewState["isAttachmentAttached"]);

    }


    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (dtPicker.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date is required";
        }

        if (string.IsNullOrWhiteSpace(txtVesselName.Text))
        {
            ucError.ErrorMessage = "Position or Port / Vessel is Required ";
        }

        if (ddlCategory.SelectedItem == null)
        {
            ucError.ErrorMessage = "Category is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtValue.Text))
        {
            ucError.ErrorMessage = "Value is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtValue.Text) == false && (Convert.ToDecimal(txtValue.Text) <= 0))
        {
            ucError.ErrorMessage = "Value cannot be zero or negative";
        }


        if (ddlMethod.SelectedItem.Text == "Discharge to reception"
            && (bool)ViewState["isAttachmentAttached"] == false)
        {
            ucError.ErrorMessage = "Attachment is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtRemarks.Text))
        {
            ucError.ErrorMessage = "Remarks is Required ";
        }

        if (string.IsNullOrWhiteSpace(lblinchName.Text))
        {
            ucError.ErrorMessage = "Incharge Sign is Required ";
        }

        return (!ucError.IsError);
    }

    public bool isValidSignature()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (dtPicker.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date is required";
        }

        if (string.IsNullOrWhiteSpace(txtVesselName.Text))
        {
            ucError.ErrorMessage = "Position or Port / Vessel is Required ";
        }

        if (ddlCategory.SelectedItem == null)
        {
            ucError.ErrorMessage = "Category is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtValue.Text))
        {
            ucError.ErrorMessage = "Value is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtValue.Text) == false && (Convert.ToDecimal(txtValue.Text) <= 0))
        {
            ucError.ErrorMessage = "Value cannot be zero or negative";
        }


        if (ddlMethod.SelectedItem.Text == "Discharge to reception"
            && (bool)ViewState["isAttachmentAttached"] == false)
        {
            ucError.ErrorMessage = "Attachment is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtRemarks.Text))
        {
            ucError.ErrorMessage = "Remarks is Required ";
        }

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnChiefOfficerSign();
        checkAttachmentAttached();
    }

    protected void ddlMethod_TextChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ddlMethod.SelectedItem.Text) == false && ddlMethod.SelectedItem.Text == "Discharge to reception")
        {
            showHideAttachment(true);
            setAttachment();
        }
        else
        {
            showHideAttachment(false);
        }
    }

    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        if(isValidSignature()==false)
        {
            ucError.Visible = true;
            return;
        }
        string popupName = "Log";
        string popupTitle = "Chief Officer Sign";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogOfficerSignature.aspx?popupname={0}&rankName=co&popupTitle={1}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName, popupTitle);
        RadScriptManager.RegisterStartupScript(this, this.GetType(), popupName, script, true);
    }
}