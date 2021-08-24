using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;


public partial class Log_ElectricLogORB2BallastWaterDischargeReception : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    Guid txid = Guid.Empty;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    int logId = 0;
    DateTime? missedOperationDate = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "BallastReceptionFacility");

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            txid = Guid.Parse(Request.QueryString["TxnId"]);
            txtOperationDate.Enabled = false;
        }

        if (string.IsNullOrWhiteSpace(Request.QueryString["isMissedOperation"]) == false)
        {
            isMissedOperation = Convert.ToBoolean(Request.QueryString["isMissedOperation"]);
        }

        if (string.IsNullOrWhiteSpace(Request.QueryString["missedOperationEdit"]) == false)
        {
            isMissedOperationEdit = Convert.ToBoolean(Request.QueryString["missedOperationEdit"]);
        }

        if (string.IsNullOrWhiteSpace(Request.QueryString["missedOperationDate"]) == false)
        {
            missedOperationDate = Convert.ToDateTime(Request.QueryString["missedOperationDate"]);
        }
        ShowToolBar();
        if (IsPostBack == false)
        {
            ViewState["lastTranscationDate"] = null;
            ViewState["dtkey"] = Guid.NewGuid().ToString();
            SetDefaultData();
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            GetLastTranscationDate();            
            BindData();
            checkAttachmentAttached();
        }
    }

    private void checkAttachmentAttached()
    {
        DataTable dt= PhoenixCommonFileAttachment.AttachmentList(Guid.Parse((string)ViewState["dtkey"] ));
        if (dt != null & dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];

            lblrecord3.Text = "File Attached";
            ddlAttachment.DataSource = dt;
            ddlAttachment.DataTextField = "FLDFILENAME";
            ddlAttachment.DataValueField = "FLDDTKEY";
            ddlAttachment.DataBind();
            ddlAttachment.Visible = true;
            lnkfilename.Visible = true;

            lnkfilename.NavigateUrl = "../common/download.aspx?dtkey=" + row["FLDDTKEY"];

            attachmentIcon.Attributes.Add("class", "fas fa-paperclip");
        } else
        {
            lblrecord3.Text = "No File Attached";
            attachmentIcon.Attributes.Add("class", "fas fa-paperclip-na");
            ddlAttachment.Items.Clear();
            ddlAttachment.Visible = false;
            lnkfilename.Visible = false;
        }
    }

    private void BindData()
    {
        if (txid != Guid.Empty)
        {
            DataTable dt = PhoenixMarbolLogORB2.TransactionEdit(usercode, txid);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    if (Convert.ToInt32(row["FLDITEMID"]) == 177)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 178)
                    {
                        txtPortName.Text = (string)row["FLDVALUE"];

                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 179)
                    {
                        txtFacility.Text = (string)row["FLDVALUE"];

                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 180)
                    {
                        txtTotalQuantity.Text = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 181)
                    {
                        ViewState["dtkey"] = (string)row["FLDVALUE"];
                    }


                    SetDefaultData();
                }
            }
        }
    }

    private void OnDutyEngineerSign()
    {
        if (Filter.DutyEngineerSignatureFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.DutyEngineerSignatureFilterCriteria;

            DateTime signedDate = DateTime.Parse(nvc.Get("date"));
            lblincsign.Text = string.Format("{0}-{1} {2}", nvc.Get("rank"), nvc.Get("name"), signedDate.ToString("dd-MM-yyyy"));
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
        ViewState["lastTranscationDate"] = PhoenixMarbolLogORB2.GetLogLastTranscationDate(vesselId, usercode);
    }
    
    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false : true;
    }

    private void SetDefaultData()
    {
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtCode.Text = "R";

        txtItemNo.Text = "82";
        txtItemNo1.Text = "83";
        txtItemNo2.Text = "84";
        txtItemNo3.Text = "85";
        
        lblRecord.Text = string.Format("<b>{0}</b>", txtPortName.Text);
        lblrecord1.Text = string.Format("<b>{0}</b>", txtFacility.Text);
        lblrecord2.Text = string.Format("{0}  m3 of Ballast water discharged to shore reception facility", txtTotalQuantity.Text);
        //lblrecord3.Text = string.Format("<b>{0}</b>", txtMethodCleaning.Text);
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

                if (isValidateSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                SaveTransaction();

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

    private void SaveTransaction()
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
        SetDefaultData();
        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid logTxId = Guid.NewGuid();
            TranscationInsert(entrydate, logTxId, logId);
            MissedOperationalEntryTemplateUpdate(logTxId);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TransactionUpdate(logId, entrydate);
            MissedOperationalEntryTemplateUpdate(txid);
        }
        else if (txid != Guid.Empty && isMissedOperationEdit == false)
        {
            TransactionUpdate(logId, entrydate);
            LogBookUpdate(logId);
        }
        else
        {
            Guid logTxId = Guid.NewGuid();

            TranscationInsert(entrydate, logTxId, logId);

            // book entry insert
            PhoenixMarbolLogORB2.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            txtCode.Text,
                            txtItemNo.Text,
                            lblRecord.Text,
                            1,
                            false,
                            Guid.Parse((string)ViewState["dtkey"])
                );

            PhoenixMarbolLogORB2.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo1.Text,
                            lblrecord1.Text,
                            2,
                            false,
                            Guid.Parse((string)ViewState["dtkey"])
                );

            PhoenixMarbolLogORB2.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo2.Text,
                            lblrecord2.Text,
                            3
                );

            PhoenixMarbolLogORB2.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo3.Text,
                            lblrecord3.Text,
                            4,
                            false

                );

            PhoenixMarbolLogORB2.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            null,
                            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), false),
                            6,
                            true
                );

            // history insert
            PhoenixMarbolLogORB2.LogORB2BookEntryStatusInsert(usercode
                            , vesselId
                            , logId
                            , logTxId
                            , General.GetNullableInteger(lblinchId.Text)
                            , General.GetNullableString(lblincRank.Text)
                            , General.GetNullableString(lblincsign.Text)
                            , General.GetNullableDateTime(lblincSignDate.Text)
                        );


        }
    }

    private void MissedOperationalEntryTemplateUpdate(Guid logTxId)
    {
        NameValueCollection nvc = new NameValueCollection();
        // store transaction 
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblrecord1.Text);
        nvc.Add("Record3", lblrecord2.Text);
        nvc.Add("Record4", lblrecord3.Text);

        nvc.Add("ReportCode1", txtCode.Text);

        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", txtItemNo1.Text);
        nvc.Add("ItemNo3", txtItemNo2.Text);
        nvc.Add("ItemNo4", txtItemNo3.Text);

        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "4");
        nvc.Add("logId", logId.ToString());
        nvc.Add("logTxId", logTxId.ToString());
        nvc.Add("dtkey", ViewState["dtkey"].ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void LogBookUpdate(int logId)
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);


        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblRecord.Text,
            1
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord1.Text,
            2
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord2.Text,
            3
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord3.Text,
            4
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true),
            5
        );

        PhoenixMarbolLogORB2.LogORB2BookEntryStatusUpdate(usercode
                                    , vesselId
                                    , logId
                                    , txid
                                    , General.GetNullableInteger(lblinchId.Text)
                                    , General.GetNullableString(lblincRank.Text)
                                    , General.GetNullableString(lblincsign.Text)
                                    , General.GetNullableDateTime(lblincSignDate.Text)
                                );
    }

    private void TransactionUpdate(int logId, DateTime entrydate)
    {

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        177,
        txid,
        entrydate,
        entrydate.ToString()
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        178,
        txid,
        entrydate,
        txtPortName.Text
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        179,
        txid,
        entrydate,
        txtFacility.Text
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        180,
        txid,
        entrydate,
        txtTotalQuantity.Text
        );

    }

    private void TranscationInsert(DateTime entrydate, Guid logTxId, int logId)
    {


        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        177,
        logTxId,
        entrydate,
        entrydate.ToString()
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        178,
        logTxId,
        entrydate,
        txtPortName.Text
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        179,
        logTxId,
        entrydate,
        txtFacility.Text
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        180,
        logTxId,
        entrydate,
        txtTotalQuantity.Text
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        181,
        logTxId,
        entrydate,
        (string)ViewState["dtkey"]
        );
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }

        if (string.IsNullOrWhiteSpace(txtPortName.Text))
        {
            ucError.ErrorMessage = "Port Name is Required";
        }

        if (string.IsNullOrWhiteSpace(txtFacility.Text))
        {
            ucError.ErrorMessage = "Reception Facility is Required";
        }

        if (string.IsNullOrWhiteSpace(txtTotalQuantity.Text))
        {
            ucError.ErrorMessage = "Total Quantity is Required";
        }

        if (ddlAttachment.Items.Count == 0)
        {
            ucError.ErrorMessage = "Attachment is Required";
        }
        
        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false && txid == Guid.Empty)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }


        return (!ucError.IsError);
    }

    protected void txtOperationDate_SelectedDateChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void txtAfrTrnsROBFrom_TextChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
        SetDefaultData();
        checkAttachmentAttached();
        if (isMissedOperation || isMissedOperationEdit)
        {
            SaveTransaction();
        }
    }

    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        if (isValidInput() == false)
        {
            ucError.Visible = true;
            return;
        }

        //string popupName = "Log";
        string popupName = isMissedOperation || isMissedOperationEdit ? "Log~iframe" : "Log";
        string rank = "co";
        string popupTitle = "Chief Officer Signature";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogOfficerSignature.aspx?popupname={0}&rankName={1}&popupTitle={2}&isMissedOperation={3}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName, rank, popupTitle, isMissedOperation);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
    }

    private bool isValidateSignature()
    {

        if (string.IsNullOrWhiteSpace(lblinchName.Text))
        {
            ucError.ErrorMessage = "Chief Officer Signature is Required Before Save";
        }

        return (!ucError.IsError);
    }


    protected void ddlWaterTransferedFrom_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        SetDefaultData();
    }

    protected void cmdAttachment_Click(object sender, EventArgs e)
    {
        string popupName = isMissedOperation || isMissedOperationEdit ? "Log~iframe" : "Log";        
        string script = string.Format("openNewWindow('NAFA', '', 'Common/CommonFileAttachment.aspx?dtkey={0}&MOD=LOG&RefreshWindowName={1}',false,700,500);", (string)ViewState["dtkey"], popupName);
        //lblid1.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionTMSAMatrixObjectiveEvidenceUpdate.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "',false,500,300); return false;");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
    }

    protected void ddlAttachment_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        lnkfilename.NavigateUrl = "../common/download.aspx?dtkey=" + ddlAttachment.SelectedItem.Value.ToString();
    }
}