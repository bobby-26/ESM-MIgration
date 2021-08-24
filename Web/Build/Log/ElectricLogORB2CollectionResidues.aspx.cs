using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogORB2CollectionResidues : PhoenixBasePage
{
    string ReportCode;
    string ItemNo = "55";
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
        logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "CollectionTransfer");

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            txid = Guid.Parse(Request.QueryString["TxnId"]);
            ddlTransferFrom.Enabled = false;
            ddlMethodOfTransfer.Enabled = false;
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
            ddlCargoPopulate();
            ddlMethodOfTransferPoulate();
            ddlMixedCargoTankPopulate();
            ddlTransferredToPopulate();
            SetDefaultData();
            showHideControls();
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            GetLastTranscationDate();
            BindData();
            LoadNotes();
        }
    }

    private void LoadNotes()
    {
        DataTable dt = PhoenixMarbolLogORB2.ORB2LogRegisterEdit(usercode, General.GetNullableInteger(logId.ToString()));
        if (dt.Rows.Count > 0)
        {
            lblnotes.Text = dt.Rows[0]["FLDNOTES"].ToString();
        }
    }

    private void showHideControls()
    {

        if (ddlMethodOfTransfer.SelectedItem == null) return;
        

        if (ddlMethodOfTransfer.SelectedItem.Text == "57.1")
        {
            ShowHide571(true);
            ShowHide572(false);
            ShowHide573(false);
            ShowHide574(false);

        }
        else if(ddlMethodOfTransfer.SelectedItem.Text == "57.2")
        {
            ShowHide571(false);
            ShowHide572(true);
            ShowHide573(false);
            ShowHide574(false);
        }
        else if (ddlMethodOfTransfer.SelectedItem.Text == "57.3")
        {
            ShowHide571(false);
            ShowHide572(false);
            ShowHide573(true);
            ShowHide574(false);

        } else
        {
            ShowHide571(false);
            ShowHide572(false);
            ShowHide573(false);
            ShowHide574(true);
        }
    }

    private void ShowHide574(bool show)
    {
        row574One.Visible = show;
        row574Two.Visible = show;
    }

    private void ShowHide573(bool show)
    {
        row573One.Visible = show;
        row573Two.Visible = show;
        row573Three.Visible = show;
    }

    private void ShowHide572(bool show)
    {
        row572One.Visible = show;
        row572Two.Visible = show;
    }

    private void ShowHide571(bool show)
    {
        lblShoreDisposal.Visible = show;
        lblBarageName.Visible = show;
        txtBarageName.Visible = show;
        row57One.Visible = show;
        row57Two.Visible = show;
    }

    private void ddlTransferredToPopulate()
    {
        ddlTransferredTo.DataSource = PhoenixMarbolLogORB2.ElogLocationDropDown(vesselId, usercode, "CRO");
        ddlTransferredTo.DataBind();
        if (ddlTransferredTo.Items.Count > 0)
        {
            ddlTransferredTo.Items[0].Selected = true;
        }
    }

    private void ddlMixedCargoTankPopulate()
    {
        ddlMixedCargoTank.DataSource = PhoenixMarbolLogORB2.ElogLocationDropDown(vesselId, usercode, "CRO");
        ddlMixedCargoTank.DataBind();
        if (ddlMixedCargoTank.Items.Count > 0)
        {
            ddlMixedCargoTank.Items[0].Selected = true;
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

                    if (Convert.ToInt32(row["FLDITEMID"]) == 80)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 81)
                    {
                        RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (fromItem != null)
                        {
                            fromItem.Selected = true;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 82)
                    {
                        txtQuantityTransfered.Text= (string)row["FLDVALUE"];

                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 83)
                    {
                        RadComboBoxItem fromItem = ddlMethodOfTransfer.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (fromItem != null)
                        {
                            fromItem.Selected = true;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 84)
                    {
                        txtQuantityReatinedDisposal.Text = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 85)
                    {
                        txtBarageName.Text = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 86)
                    {
                        txtPort.Text = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 87)
                    {
                        RadComboBoxItem fromItem = ddlMixedCargoTank.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (fromItem != null)
                        {
                            fromItem.Selected = true;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 88)
                    {
                        RadComboBoxItem fromItem = ddlTransferredTo.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (fromItem != null)
                        {
                            fromItem.Selected = true;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 89)
                    {
                        txtQuantityRetained.Text = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 90)
                    {
                        txtStateMethod.Text = (string)row["FLDVALUE"];
                    }

                }
                showHideControls();
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
        ViewState["lastTranscationDate"] = PhoenixMarbolLogORB2.GetLogLastTranscationDate(vesselId, usercode);
    }

    private void ddlCargoPopulate()
    {
        ddlTransferFrom.DataSource = PhoenixMarbolLogORB2.ElogLocationDropDown(vesselId, usercode, "CRO");
        ddlTransferFrom.DataBind();
        if (ddlTransferFrom.Items.Count > 0)
        {
            ddlTransferFrom.Items[0].Selected = true;
        }
    }
    
    private void ddlMethodOfTransferPoulate()
    {
        List<string> list = new List<string>();
        list.Add("57.1");
        list.Add("57.2");
        list.Add("57.3");
        list.Add("57.4");
        ddlMethodOfTransfer.DataSource = list;
        ddlMethodOfTransfer.DataBind();
        if (ddlMethodOfTransfer.Items.Count > 0)
        {
            ddlMethodOfTransfer.Items[0].Selected = true;
        }
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
        if (ddlTransferFrom.SelectedItem == null) return;

        ReportCode = "J";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtCode.Text = ReportCode;
        txtItemNo.Text = ItemNo;
        txtItemNo1.Text = "56";
        txtItemNo2.Text = ddlMethodOfTransfer.SelectedItem.Text;

        string template571 = string.Format("<b>{0} at {1} {2} m3</b>", txtBarageName.Text, txtPort.Text, txtQuantityTransfered.Text);
        string template572 = string.Format("<b>Mixed with Cargo in Tank {0}, , Quantity mixed  {1} m3</b> ", ddlMixedCargoTank.SelectedItem.Text, txtQuantityMixed.Text);
        string template573 = string.Format("<b>Transferred to {0}  ; Quantity transferred {1} m3 ; Total Quantity in {2} m3</b>", ddlTransferredTo.SelectedItem.Text, txtQuantityTransfered.Text, txtQuantityRetained.Text);
        string template574 = string.Format("<b>Other Method of Disposal {0} ; Quantity Disposed {1} m3</b>", txtStateMethod.Text, txtQuantityTransfered.Text);
        string result = string.Empty;
        switch (ddlMethodOfTransfer.SelectedItem.Text)
        {
            case "57.1":
                result = template571;
                break;
            case "57.2":
                result = template572;
                break;
            case "57.3":
                result = template573;
                break;
            case "57.4":
                result = template574;
                break;
        }

        lblRecord.Text = string.Format("<b>{0}</b>", ddlTransferFrom.SelectedItem.Text);
        lblrecord1.Text = string.Format("<b>{0}</b> m3 disposed of  <b>{1}</b> m3, Retained", txtQuantityTransfered.Text , txtQuantityReatinedDisposal.Text);
        lblrecord2.Text = string.Format("<b>{0}</b>", result);

        txtQuantityDischarge.Text = txtQuantityTransfered.Text;
        txtQuantityMixed.Text = txtQuantityTransfered.Text;
        txtTransferQty.Text = txtQuantityTransfered.Text;
        txtQuantityDisposed.Text = txtQuantityTransfered.Text;

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
            PhoenixMarbolLogORB2.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            txtCode.Text,
                            txtItemNo.Text,
                            lblRecord.Text,
                            1
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
                            2
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
                            null,
                            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), false),
                            4,
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

        nvc.Add("ReportCode1", txtCode.Text);

        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", txtItemNo1.Text);
        nvc.Add("ItemNo3", txtItemNo2.Text);

        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "3");
        nvc.Add("logId", logId.ToString());
        nvc.Add("logTxId", logTxId.ToString());
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
            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true),
            4
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

    private void TransactionUpdate(int logId)
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        PhoenixMarbolLogORB2.TransactionUpdate(
                         usercode,
                         vesselId,
                         logId,
                         80,
                         txid,
                         entrydate,
                         entrydate.ToString()
         );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            81,
                            txid,
                            entrydate,
                            ddlTransferFrom.SelectedItem.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            82,
                            txid,
                            entrydate,
                            txtQuantityTransfered.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            83,
                            txid,
                            entrydate,
                            ddlMethodOfTransfer.SelectedItem.Text
            );


        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            84,
                            txid,
                            entrydate,
                            txtQuantityReatinedDisposal.Text
            );


        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            85,
                            txid,
                            entrydate,
                            txtBarageName.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            86,
                            txid,
                            entrydate,
                            txtPort.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            87,
                            txid,
                            entrydate,
                            ddlMixedCargoTank.SelectedItem.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            88,
                            txid,
                            entrydate,
                            ddlTransferredTo.SelectedItem.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            89,
                            txid,
                            entrydate,
                            txtQuantityRetained.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            90,
                            txid,
                            entrydate,
                            txtStateMethod.Text
            );
    }

    private void TranscationInsert(DateTime entrydate, Guid logTxId, int logId)
    {
        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            80,
                            logTxId,
                            entrydate,
                            entrydate.ToString()
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            81,
                            logTxId,
                            entrydate,
                            ddlTransferFrom.SelectedItem.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            82,
                            logTxId,
                            entrydate,
                            txtQuantityTransfered.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            83,
                            logTxId,
                            entrydate,
                            ddlMethodOfTransfer.SelectedItem.Text
            );


        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            84,
                            logTxId,
                            entrydate,
                            txtQuantityReatinedDisposal.Text
            );


        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            85,
                            logTxId,
                            entrydate,
                            txtBarageName.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            86,
                            logTxId,
                            entrydate,
                            txtPort.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            87,
                            logTxId,
                            entrydate,
                            ddlMixedCargoTank.SelectedItem.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            88,
                            logTxId,
                            entrydate,
                            ddlTransferredTo.SelectedItem.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            89,
                            logTxId,
                            entrydate,
                            txtQuantityRetained.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            90,
                            logTxId,
                            entrydate,
                            txtStateMethod.Text
            );


    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }

        if (string.IsNullOrWhiteSpace(txtQuantityTransfered.Text))
        {
            ucError.ErrorMessage = "Quantity Transferred is required";
        }
        
        if (ddlMethodOfTransfer.SelectedItem.Text == "57.1")
        {
            if (string.IsNullOrWhiteSpace(txtBarageName.Text))
            {
                ucError.ErrorMessage = "Barge Name is required";
            }

            if (string.IsNullOrWhiteSpace(txtPort.Text))
            {
                ucError.ErrorMessage = "Port or Anchorage Name is required";
            }
        }

        if (ddlMethodOfTransfer.SelectedItem.Text == "57.2")
        {
            if (string.IsNullOrWhiteSpace(ddlMixedCargoTank.SelectedItem.Text))
            {
                ucError.ErrorMessage = "Mixed with cargo tank is required";
            }
        }

        if (ddlMethodOfTransfer.SelectedItem.Text == "57.3")
        {
            if (string.IsNullOrWhiteSpace(txtQuantityRetained.Text))
            {
                ucError.ErrorMessage = "Quantity Retained is required";
            }

            if (string.IsNullOrWhiteSpace(txtTransferQty.Text))
            {
                ucError.ErrorMessage = "Transfered Quantity is required";
            }
        }

        if (ddlMethodOfTransfer.SelectedItem.Text == "57.4")
        {
            if (string.IsNullOrWhiteSpace(txtStateMethod.Text))
            {
                ucError.ErrorMessage = "State Method is required";
            }
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
        if (isMissedOperation || isMissedOperationEdit)
        {
            SaveTransaction();
        }
    }

    protected void ddlWaterTransferedFrom_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        SetDefaultData();
    }


    protected void ddlMethodOfTransfer_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        showHideControls();
        SetDefaultData();
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
}