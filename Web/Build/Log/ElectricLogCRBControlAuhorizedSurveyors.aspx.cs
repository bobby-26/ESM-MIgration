using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using System.Collections.Generic;
using SouthNests.Phoenix.Common;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Web.SessionState;

using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Text;

public partial class Log_ElectricLogCRBControlAuhorizedSurveyors : PhoenixBasePage
{
    string ReportCode;
    string ItemNo = "29";
    int usercode = 0;
    int vesselId = 0;
    Guid txid = Guid.Empty;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    int logId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        logId = PhoenixMarpolLogCRB.GetCRBLogId(usercode, "ControlAuhorized");

       
      


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

        ShowToolBar();

        if (IsPostBack == false)
        {
            ViewState["dtkey"] = Guid.NewGuid().ToString();
            ddlFromPopulate();
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
        DataTable dt = PhoenixCommonFileAttachment.AttachmentList(Guid.Parse((string)ViewState["dtkey"]));
        if (dt != null & dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];

            lblrecord7.Text = "File Attached";
            ddlAttachment.DataSource = dt;
            ddlAttachment.DataTextField = "FLDFILENAME";
            ddlAttachment.DataValueField = "FLDDTKEY";
            ddlAttachment.DataBind();
            ddlAttachment.Visible = true;
            lnkfilename.Visible = true;

            lnkfilename.NavigateUrl = "../common/download.aspx?dtkey=" + row["FLDDTKEY"];

            attachmentIcon.Attributes.Add("class", "fas fa-paperclip");
        }
        else
        {
            lblrecord7.Text = "No File Attached";
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
            DataTable dt = PhoenixMarpolLogCRB.TransactionEdit(usercode, txid);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    if (Convert.ToInt32(row["FLDITEMID"]) == 66)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }


                    if (Convert.ToInt32(row["FLDITEMID"]) == 67)
                    {
                        txtterminal.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 68)
                    {
                        RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (fromItem != null)
                        {
                            fromItem.Selected = true;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 69)
                    {
                        txtSubstanceName.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 70)
                    {
                        txtCategory.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 71)
                    {
                        //RadComboBoxItem fromItem = ddlTanksempty.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        //if (fromItem != null)
                        //{
                        //    fromItem.Selected = true;
                        //}

                        ddlTanksempty.SelectedValue = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 72)
                    {
                        //RadComboBoxItem fromItem = ddlprewashtin.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        //if (fromItem != null)
                        //{
                        //    fromItem.Selected = true;
                        //}
                        ddlprewashtin.SelectedValue = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 73)
                    {
                        //RadComboBoxItem fromItem = ddltankwashing.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        //if (fromItem != null)
                        //{
                        //    fromItem.Selected = true;
                        //}
                        ddltankwashing.SelectedValue = row["FLDVALUE"].ToString();

                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 74)
                    {
                        //RadComboBoxItem fromItem = ddlexemptiongranted.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        //if (fromItem != null)
                        //{
                        //    fromItem.Selected = true;
                        //}
                        ddlexemptiongranted.SelectedValue = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 75)
                    {
                        txtreasonexemption.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 76)
                    {
                        //txtsurveyor.Text = row["FLDVALUE"].ToString();
                        ViewState["dtkey"] = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 77)
                    {
                        txtorganisation.Text = row["FLDVALUE"].ToString();
                    }
                }
                SetDefaultData();
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
        ViewState["lastTranscationDate"] = PhoenixMarpolLogCRB.GetLogLastTranscationDate(vesselId, usercode);
    }

    private void ddlFromPopulate()
    {
        ddlTransferFrom.DataSource = PhoenixMarpolLogCRB.ElogLocationDropDown(vesselId, usercode, null);
        ddlTransferFrom.DataBind();
        if (ddlTransferFrom.Items.Count > 0)
        {
            ddlTransferFrom.Items[0].Selected = true;
        }
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false: true;
    }

    private void SetDefaultData()
    {
        if (ddlTransferFrom.SelectedItem == null) return;

        ReportCode = "J";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        txtItemNo1.Text = "30";
        txtItemNo2.Text = "31";
        txtItemNo3.Text = "32";
        txtItemNo4.Text = "33";
        txtItemNo5.Text = "34";
        txtItemNo6.Text = "35";
        txtItemNo7.Text = "36";
        txtItemNo8.Text = "37";
        lblRecord.Text = string.Format("<b>{0}</b>", txtterminal.Text);
        lblrecord1.Text = string.Format("<b>{0}, {1}, {2}</b>", ddlTransferFrom.SelectedItem.Text,txtSubstanceName.Text, txtCategory.Text);
        lblrecord2.Text = string.Format("<b>{0}</b>", ddlTanksempty.SelectedValue);
        lblrecord3.Text = string.Format("<b>{0}</b>", ddlprewashtin.SelectedValue);
        lblrecord4.Text = string.Format("<b>{0}</b>", ddltankwashing.SelectedValue);
        lblrecord5.Text = string.Format("<b>{0}</b>", ddlexemptiongranted.SelectedValue);

        if (ddlexemptiongranted.SelectedValue == "Yes")
        {
            lblrecord6.Text = string.Format("<b>{0}</b>", txtreasonexemption.Text);
        }
        else
        {
            lblrecord6.Text = "<b>NA</b>";
        }
        //lblrecord7.Text = string.Format("<b>{0}</b>", txtsurveyor.Text);
        lblrecord8.Text = string.Format("<b>{0}</b>", txtorganisation.Text);
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (isValidInput() == false || IsValidSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                SaveTxin();
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

    private void SaveTxin()
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid logTxId = Guid.NewGuid();
            TranscationInsert(entrydate, logTxId, logId);
            MissedOperationalEntryTemplateUpdate(logTxId);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TransactionUpdate(logId);
            MissedOperationalEntryTemplateUpdate(txid);
        }
        else if (txid != Guid.Empty && isMissedOperationEdit == false)
        {
            TransactionUpdate(logId);
            LogBookUpdate(logId);
        }
        else
        {
            Guid logTxId = Guid.NewGuid();

            TranscationInsert(entrydate, logTxId, logId);

            // book entry insert

            PhoenixMarpolLogCRB.BookEntryInsert(
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

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo1.Text,
                            lblrecord1.Text,
                            2
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
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

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo3.Text,
                            lblrecord3.Text,
                            4
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo4.Text,
                            lblrecord4.Text,
                            5
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo5.Text,
                            lblrecord5.Text,
                            6
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo6.Text,
                            lblrecord6.Text,
                            7
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo7.Text,
                            lblrecord7.Text,
                            8
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo8.Text,
                            lblrecord8.Text,
                            9
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            null,
                            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), false),
                            10,
                            true
                );

            // history insert
            PhoenixMarpolLogCRB.LogCRBBookEntryStatusInsert(usercode
                            , vesselId
                            , logId
                            , logTxId
                            , General.GetNullableInteger(lblinchId.Text)
                            , General.GetNullableString(lblincRank.Text)
                            , General.GetNullableString(lblincsign.Text)
                            , General.GetNullableDateTime(lblincSignDate.Text)
                        );

            // history insert
            PhoenixMarpolLogCRB.LogCRBHistoryInsert(usercode
                             , vesselId
                             , logId
                             , logTxId
                             , General.GetNullableString(lblincRank.Text)
                             , General.GetNullableDateTime(lblincSignDate.Text)
                             , txtCode.Text
                             , txtItemNo.Text
                             , 1
                             , lblRecord.Text
                             , lblRecord.Text
                             , lblrecord1.Text
                             , lblrecord2.Text
                             , lblrecord3.Text
                             , lblrecord4.Text
                             , lblrecord5.Text
                             , lblrecord6.Text
                             , lblrecord7.Text
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
        nvc.Add("Record5", lblrecord4.Text);
        nvc.Add("Record6", lblrecord5.Text);
        nvc.Add("Record7", lblrecord6.Text);
        nvc.Add("Record8", lblrecord7.Text);
        nvc.Add("Record9", lblrecord8.Text);

        nvc.Add("ReportCode1", txtCode.Text);

        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", txtItemNo1.Text);
        nvc.Add("ItemNo3", txtItemNo2.Text);
        nvc.Add("ItemNo4", txtItemNo3.Text);
        nvc.Add("ItemNo5", txtItemNo4.Text);
        nvc.Add("ItemNo6", txtItemNo5.Text);
        nvc.Add("ItemNo7", txtItemNo6.Text);
        nvc.Add("ItemNo8", txtItemNo7.Text);
        nvc.Add("ItemNo9", txtItemNo8.Text);

        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "9");
        nvc.Add("logId", logId.ToString());
        nvc.Add("logTxId", logTxId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void LogBookUpdate(int logId)
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblRecord.Text,
            1
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord1.Text,
            2
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord2.Text,
            3
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord3.Text,
            4
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord4.Text,
            5
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord5.Text,
            6
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord6.Text,
            7
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord7.Text,
            8
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord8.Text,
            9
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true),
            10
        );

        // history update
        PhoenixMarpolLogCRB.LogCRBBookEntryStatusUpdate(usercode
                        , vesselId
                        , logId
                        , txid
                        , General.GetNullableInteger(lblinchId.Text)
                        , General.GetNullableString(lblincRank.Text)
                        , General.GetNullableString(lblincsign.Text)
                        , General.GetNullableDateTime(lblincSignDate.Text)
                    );


        PhoenixMarpolLogCRB.LogCRBHistoryUpdate(usercode
                             , vesselId
                             , logId
                             , txid
                             , General.GetNullableString(lblincRank.Text)
                             , General.GetNullableDateTime(lblincSignDate.Text)
                             , txtCode.Text
                             , txtItemNo.Text
                             , 1
                             , lblRecord.Text
                             , lblRecord.Text
                             , lblrecord1.Text
                             , lblrecord2.Text
                             , lblrecord3.Text
                             , lblrecord4.Text
                             , lblrecord5.Text
                             , lblrecord6.Text
                             , lblrecord7.Text
                       );
    }

    private void TransactionUpdate(int logId)
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                66,
                txid,
                entrydate,
                entrydate.ToString());

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                67,
                txid,
                entrydate,
                txtterminal.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                68,
                txid,
                entrydate,
                ddlTransferFrom.SelectedItem.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                69,
                txid,
                entrydate,
                txtSubstanceName.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                70,
                txid,
                entrydate,
                txtCategory.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                71,
                txid,
                entrydate,
                ddlTanksempty.SelectedItem.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                72,
                txid,
                entrydate,
                ddlprewashtin.SelectedItem.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                73,
                txid,
                entrydate,
                ddltankwashing.SelectedItem.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                74,
                txid,
                entrydate,
                ddlexemptiongranted.SelectedItem.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                75,
                txid,
                entrydate,
                txtreasonexemption.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                77,
                txid,
                entrydate,
                txtorganisation.Text);
    }

    private void TranscationInsert(DateTime entrydate, Guid logTxId, int logId)
    {

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            66,
                            logTxId,
                            entrydate,
                            entrydate.ToString()
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            67,
                            logTxId,
                            entrydate,
                            txtterminal.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            68,
                            logTxId,
                            entrydate,
                            ddlTransferFrom.SelectedItem.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            69,
                            logTxId,
                            entrydate,
                            txtSubstanceName.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            70,
                            logTxId,
                            entrydate,
                            txtCategory.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            71,
                            logTxId,
                            entrydate,
                            ddlTanksempty.SelectedValue
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            72,
                            logTxId,
                            entrydate,
                            ddlprewashtin.SelectedValue
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            73,
                            logTxId,
                            entrydate,
                            ddltankwashing.SelectedValue
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            74,
                            logTxId,
                            entrydate,
                            ddlexemptiongranted.SelectedValue
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            75,
                            logTxId,
                            entrydate,
                            txtreasonexemption.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            76,
                            logTxId,
                            entrydate,
                            (string)ViewState["dtkey"]
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            77,
                            logTxId,
                            entrydate,
                            txtorganisation.Text
            );
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }
        

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        ValidateInput();

        return (!ucError.IsError);
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(txtterminal.Text))
        {
            ucError.ErrorMessage = "Identity of the terminal is required";
        }

        if (string.IsNullOrWhiteSpace(txtSubstanceName.Text))
        {
            ucError.ErrorMessage = "Name of the Substance is required";
        }


        if (string.IsNullOrWhiteSpace(txtCategory.Text))
        {
            ucError.ErrorMessage = "Category is required";
        }

        if (ddlAttachment.Items.Count == 0)
        {
            ucError.ErrorMessage = "Attachment is required for authorized surveyor";
        }

        if (string.IsNullOrWhiteSpace(txtreasonexemption.Text))
        {
            ucError.ErrorMessage = "Reasons for exemption is required";
        }

        if (string.IsNullOrWhiteSpace(txtorganisation.Text))
        {
            ucError.ErrorMessage = "Organization for which surveyor works is required";
        }

        return (!ucError.IsError);
    }

    private bool IsValidSignature()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrWhiteSpace(lblinchName.Text))
        {
            ucError.ErrorMessage = "Chief Officer Signature is Required Before Save";
        }
        return (!ucError.IsError);
    }

    protected void txtOperationDate_SelectedDateChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
        if (isMissedOperation || isMissedOperationEdit)
        {
            SaveTxin();
        }
        SetDefaultData();
        checkAttachmentAttached();
    }
    protected void Refresh_TextChangedEvent(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void ddlCleaningTank_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        SetDefaultData();
    }

    protected void cmdAttachment_Click(object sender, EventArgs e)
    {
        string popupName = isMissedOperation || isMissedOperationEdit ? "Log~iframe" : "Log";
        string script = string.Format("openNewWindow('NAFA', '', 'Common/CommonFileAttachment.aspx?dtkey={0}&MOD=LOG&RefreshWindowName={1}');", (string)ViewState["dtkey"], popupName);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
    }
    protected void ddlAttachment_ItemSelected(object sender, DropDownListEventArgs e)
    {
        lnkfilename.NavigateUrl = "/common/download.aspx?dtkey=" + ddlAttachment.SelectedItem.Value.ToString();
    }


    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        if (isValidInput() == false)
        {
            ucError.Visible = true;
            return;
        }


        string popupName = isMissedOperation ? "Log~iframe" : "Log";
        string rank = "co";
        string popupTitle = "Chief Officer Sign";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogOfficerSignature.aspx?popupname={0}&rankName={1}&popupTitle={2}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName, rank, popupTitle);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "bookmark", script, true);
    }

    protected void ddldischargeballast_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }



    public void ConvertToPdf(string HTMLString)
    {
        try
        {
            if (HTMLString != "")
            {
                using (var ms = new MemoryStream())
                {
                    iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle( 842f,595f));
                    document.SetMargins(36f, 36f, 36f, 0f);
                    
                    string filefullpath = "CRB- CODE J - Control by authorized Surveyors" + ".pdf";
                    PdfWriter.GetInstance(document, ms);
                    document.Open();
                    string imageURL = "http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png";

                    StyleSheet styles = new StyleSheet();
                   
                    ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);

                    for (int k = 0; k < htmlarraylist.Count; k++)
                    {
                        document.Add((iTextSharp.text.IElement)htmlarraylist[k]);

                    }
                    document.Close();
                    Response.Buffer = true;
                    var bytes = ms.ToArray();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filefullpath);
                    Response.OutputStream.Write(bytes, 0, bytes.Length);
                    Response.End();
                    // HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void exportpdf_Click(object sender, EventArgs e)
    {
        if (ValidateInput())
        {
            ConvertToPdf(PrepareHtmlDoc());
        }else
        {
            ucError.Visible = true;
        }
    }

    private string PrepareHtmlDoc()
    {
        StringBuilder DsHtmlcontent = new StringBuilder();
        DsHtmlcontent.Append("<div>");
        DsHtmlcontent.Append("<table ID='headertable' class='headertable' cellpadding='5' cellspacing='5' width=\"100%\"  border-collapse='collapse' font-size='small' padding: '0 50px' > ");
        DsHtmlcontent.Append("<tr border='1' border-collapse='collapse'>");
        DsHtmlcontent.Append("<th colspan='2' border='1' border-collapse='collapse' >Date</th>");
        DsHtmlcontent.Append("<th colspan='1' border='1' border-collapse='collapse' align= 'center'>Code</th>");
        DsHtmlcontent.Append("<th colspan='2' border='1' border-collapse='collapse' align ='center'>Item No.</th>");
        DsHtmlcontent.Append("<th colspan='7' border='1' border-collapse='collapse'>Record of operation </th>");
        DsHtmlcontent.Append("</tr>");
        DsHtmlcontent.Append("<tr border='1' border-collapse='collapse'>");
        DsHtmlcontent.Append("<td colspan='2' >"+ txtDate.Text + " </td>");
        DsHtmlcontent.Append("<td colspan='1'align= 'center' >" + txtCode.Text + " </td>");
        DsHtmlcontent.Append("<td colspan='2'  align ='center'>" + txtItemNo.Text + " </td>");
        DsHtmlcontent.Append("<td colspan='7' >" + lblRecord.Text + " </td>");
        DsHtmlcontent.Append("</tr>");
        DsHtmlcontent.Append("<tr>");
        DsHtmlcontent.Append("<td colspan='2'> </td>");
        DsHtmlcontent.Append("<td colspan='1'> </td>");
        DsHtmlcontent.Append("<td colspan='2'  border='1' border-collapse='collapse' align= 'center' >" + txtItemNo1.Text + " </td>");
        DsHtmlcontent.Append("<td colspan='7'  border='1' border-collapse='collapse'>" + lblrecord1.Text + " </td>");
        DsHtmlcontent.Append("</tr>");
        DsHtmlcontent.Append("<tr>");
        DsHtmlcontent.Append("<td colspan='2'> </td>");
        DsHtmlcontent.Append("<td colspan='1'> </td>");
        DsHtmlcontent.Append("<td colspan='2' border='1' border-collapse='collapse' align= 'center' >" + txtItemNo2.Text + " </td>");
        DsHtmlcontent.Append("<td colspan='7' border='1' border-collapse='collapse'>" + lblrecord2.Text + " </td>");
        DsHtmlcontent.Append("</tr>");
        DsHtmlcontent.Append("<tr>");
        DsHtmlcontent.Append("<td colspan='2'> </td>");
        DsHtmlcontent.Append("<td colspan='1'> </td>");
        DsHtmlcontent.Append("<td colspan='2' border='1' border-collapse='collapse' align= 'center' >" + txtItemNo3.Text + " </td>");
        DsHtmlcontent.Append("<td colspan='7' border='1' border-collapse='collapse'>" + lblrecord3.Text + " </td>");
        DsHtmlcontent.Append("</tr>");
        DsHtmlcontent.Append("<tr>");
        DsHtmlcontent.Append("<td colspan='2'> </td>");
        DsHtmlcontent.Append("<td colspan='1'> </td>");
        DsHtmlcontent.Append("<td colspan='2' border='1' border-collapse='collapse' align= 'center' >" + txtItemNo4.Text + " </td>");
        DsHtmlcontent.Append("<td colspan='7' border='1' border-collapse='collapse'>" + lblrecord4.Text + " </td>");
        DsHtmlcontent.Append("</tr>");
        DsHtmlcontent.Append("<tr>");
        DsHtmlcontent.Append("<td colspan='2'> </td>");
        DsHtmlcontent.Append("<td colspan='1'> </td>");
        DsHtmlcontent.Append("<td colspan='2' border='1' border-collapse='collapse' align= 'center' >" + txtItemNo5.Text + " </td>");
        DsHtmlcontent.Append("<td colspan='7' border='1' border-collapse='collapse'>" + lblrecord5.Text + " </td>");
        DsHtmlcontent.Append("</tr>");
        DsHtmlcontent.Append("<tr>");
        DsHtmlcontent.Append("<td colspan='2'> </td>");
        DsHtmlcontent.Append("<td colspan='1'> </td>");
        DsHtmlcontent.Append("<td colspan='2' border='1' border-collapse='collapse' align= 'center' >" + txtItemNo6.Text + " </td>");
        DsHtmlcontent.Append("<td colspan='7' border='1' border-collapse='collapse'>" + lblrecord6.Text + " </td>");
        DsHtmlcontent.Append("</tr >");
        DsHtmlcontent.Append("<tr>");
        DsHtmlcontent.Append("<td colspan='2'> </td>");
        DsHtmlcontent.Append("<td colspan='1'> </td>");
        DsHtmlcontent.Append("<td colspan='2' border='1' border-collapse='collapse' align= 'center' >Name of Surveyor </td>");
        DsHtmlcontent.Append("<td colspan='2' border='1' border-collapse='collapse'  > </td>");
        DsHtmlcontent.Append("<td colspan='2' border='1' border-collapse='collapse' align= 'center' >Signature of Surveyor </td>");
        DsHtmlcontent.Append("<td colspan='3' border='1' border-collapse='collapse'  > </td>");
        
        DsHtmlcontent.Append("</tr >");


        DsHtmlcontent.Append("<tr>");
        DsHtmlcontent.Append("<td colspan='2'> </td>");
        DsHtmlcontent.Append("<td colspan='1'> </td>");
        DsHtmlcontent.Append("<td colspan='2' border='1' border-collapse='collapse' align= 'center' >" + txtItemNo8.Text + " </td>");
        DsHtmlcontent.Append("<td colspan='7' border='1' border-collapse='collapse'>" + lblrecord8.Text + " </td>");
        DsHtmlcontent.Append("</tr>");
        DsHtmlcontent.Append("</table>");
        DsHtmlcontent.Append("</div>");
        return DsHtmlcontent.ToString();

    }

}