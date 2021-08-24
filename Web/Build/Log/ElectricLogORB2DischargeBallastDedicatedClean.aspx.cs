using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogORB2DischargeBallastDedicatedClean : PhoenixBasePage
{
    //string ReportCode;
    //string ItemNo = "27";
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
        logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "DischargeBallastClean");

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            txid = Guid.Parse(Request.QueryString["TxnId"]);
            ddlTank.Enabled = false;
            ddlContext.Enabled = false;
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
            LoadContext();
            ddlWaterTransferedFromPoulate();
            SetDefaultData();
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            GetLastTranscationDate();
            BindData();
            showHideControls();
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

                    if (Convert.ToInt32(row["FLDITEMID"]) == 151)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 152)
                    {
                        RadComboBoxItem fromItem = ddlTank.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (fromItem != null)
                        {
                            fromItem.Selected = true;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 153)
                    {
                        txtDischargeStartTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 154)
                    {
                        txtDischargeStopTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);

                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 155)
                    {
                        txtUTCStartTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);

                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 156)
                    {
                        txtUTCStopTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);

                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 157)
                    {
                        txtIndication.Text = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 158)
                    {
                        txtMonitored.Text = (string)row["FLDVALUE"];

                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 159)
                    {
                        txtTimeWhenSeperating.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 160)
                    {
                        txtSepratingLat.Text = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 161)
                    {
                        txtSepratingLong.Text = (string)row["FLDVALUE"];

                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 162)
                    {
                        txtStartLat.Text = (string)row["FLDVALUE"];

                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 163)
                    {
                        txtStartLong.Text = (string)row["FLDVALUE"];

                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 164)
                    {
                        txtCompleteLat.Text = (string)row["FLDVALUE"];

                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 165)
                    {
                        txtCompleteLong.Text = (string)row["FLDVALUE"];

                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 182)
                    {
                        RadComboBoxItem fromItem = ddlContext.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (fromItem != null)
                        {
                            fromItem.Selected = true;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 166)
                    {
                        txtQuantityDischarged.Text = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 167)
                    {
                        txtQunatityDischargedFacility.Text = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 168)
                    {
                        txtfacility.Text = (string)row["FLDVALUE"];
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
        ViewState["lastTranscationDate"] = PhoenixMarbolLogORB2.GetLogLastTranscationDate(vesselId, usercode);
    }

    private void ddlCargoPopulate()
    {
        ddlTank.DataSource = PhoenixMarbolLogORB2.ElogLocationDropDown(vesselId, usercode, "CRO");
        ddlTank.DataBind();
        if (ddlTank.Items.Count > 0)
        {
            ddlTank.Items[0].Selected = true;
        }
    }


    private void ddlWaterTransferedFromPoulate()
    {
        //List<string> list = new List<string>();
        //list.Add("Slops");
        //list.Add("Reception");
        //ddlWaterTransferedFrom.DataSource = list;
        //ddlWaterTransferedFrom.DataBind();
        //if (ddlWaterTransferedFrom.Items.Count > 0)
        //{
        //    ddlWaterTransferedFrom.Items[0].Selected = true;
        //}
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
        if (ddlTank.SelectedItem == null) return;

        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtCode.Text = "L";

        txtItemNo.Text = "63";
        txtItemNo1.Text = "64";
        txtItemNo2.Text = "65";
        txtItemNo4.Text = "67";
        txtItemNo5.Text = "68";
        txtItemNo6.Text = "69";

        if (ddlContext.SelectedItem != null && ddlContext.SelectedItem.Text.ToLower() == "66.1")
        {
            txtItemNo3.Text = "66.1";
        }
        else
        {
            txtItemNo3.Text = "66.2";
        }


        TimeSpan startTime = txtDischargeStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtDischargeStartTime.SelectedTime.Value;
        TimeSpan stopTime = txtDischargeStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtDischargeStopTime.SelectedTime.Value;
        TimeSpan utcStartTime = txtUTCStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtUTCStartTime.SelectedTime.Value;
        TimeSpan utcStopTime = txtUTCStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtUTCStopTime.SelectedTime.Value;
        TimeSpan sepeartionTime = txtTimeWhenSeperating.SelectedTime.HasValue == false ? new TimeSpan() : txtTimeWhenSeperating.SelectedTime.Value;

        lblRecord.Text = string.Format("<b>{0}</b>", ddlTank.SelectedItem.Text);
        lblRecord1.Text = string.Format("<b>Discharge Started at {0} LT ({1} UTC) Hours & Position {2}, {3} </b>", startTime.ToString(), utcStartTime.ToString(), txtStartLat.Text, txtStartLong.Text);
        lblRecord2.Text = string.Format("<b>Discharge Completed at {0} LT ({1} UTC) Hours & Position {2}, {3} </b>", stopTime.ToString(), utcStopTime.ToString(), txtCompleteLat.Text, txtCompleteLong.Text);
        lblRecord4.Text = string.Format("<b>{0}</b>", txtIndication.Text);
        lblRecord5.Text = string.Format("<b>{0}</b>", txtMonitored.Text);
        lblRecord6.Text = string.Format("<b>At {0}  Hours & Position {1}  valves separating the dedicated clean ballast tank from cargo & stripping lines were closed</b>", sepeartionTime.ToString(), txtSepratingLat.Text, txtSepratingLong.Text);

        string context611 = string.Format("<b>{0} m3 Discharged to Sea</b>", txtQuantityDischarged.Text);
        string context612 = string.Format("<b>{0} m3 Discharged to Reception Facility at {1}</b>", txtQunatityDischargedFacility.Text, txtfacility.Text);
        string record3 = ddlContext.SelectedItem.Text.ToLower() == "66.1" ? context611 : context612;
        lblRecord3.Text = record3;
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
                            lblRecord1.Text,
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
                            lblRecord2.Text,
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
                            lblRecord3.Text,
                            4
                );

            PhoenixMarbolLogORB2.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo4.Text,
                            lblRecord4.Text,
                            5
                );

            PhoenixMarbolLogORB2.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo5.Text,
                            lblRecord5.Text,
                            6
                );

            PhoenixMarbolLogORB2.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo6.Text,
                            lblRecord6.Text,
                            7
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
                            8,
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
        nvc.Add("Record2", lblRecord1.Text);
        nvc.Add("Record3", lblRecord2.Text);
        nvc.Add("Record4", lblRecord3.Text);
        nvc.Add("Record5", lblRecord4.Text);
        nvc.Add("Record6", lblRecord5.Text);
        nvc.Add("Record7", lblRecord6.Text);

        nvc.Add("ReportCode1", txtCode.Text);

        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", txtItemNo1.Text);
        nvc.Add("ItemNo3", txtItemNo2.Text);
        nvc.Add("ItemNo4", txtItemNo3.Text);
        nvc.Add("ItemNo5", txtItemNo4.Text);
        nvc.Add("ItemNo6", txtItemNo5.Text);
        nvc.Add("ItemNo7", txtItemNo6.Text);

        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "7");
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
            lblRecord1.Text,
            2
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblRecord2.Text,
            3
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblRecord3.Text,
            4
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblRecord4.Text,
            5
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblRecord5.Text,
            6
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblRecord6.Text,
            7
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true),
            8
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
        151,
        txid,
        entrydate,
        entrydate.ToString()
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        152,
        txid,
        entrydate,
        ddlTank.SelectedItem.Text
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        153,
        txid,
        entrydate,
        txtDischargeStartTime.SelectedDate.Value.ToString()
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        154,
        txid,
        entrydate,
        txtDischargeStopTime.SelectedDate.Value.ToString()
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        155,
        txid,
        entrydate,
        txtUTCStartTime.SelectedDate.Value.ToString()
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        156,
        txid,
        entrydate,
        txtUTCStopTime.SelectedDate.Value.ToString()
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        157,
        txid,
        entrydate,
        txtIndication.Text
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        158,
        txid,
        entrydate,
        txtMonitored.Text
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        159,
        txid,
        entrydate,
        txtTimeWhenSeperating.SelectedDate.Value.ToString()
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        160,
        txid,
        entrydate,
        txtSepratingLat.Text
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        161,
        txid,
        entrydate,
        txtSepratingLong.Text
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        162,
        txid,
        entrydate,
        txtStartLat.Text
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        163,
        txid,
        entrydate,
        txtStartLong.Text
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        164,
        txid,
        entrydate,
        txtCompleteLat.Text
        );

        PhoenixMarbolLogORB2.TransactionUpdate(
        usercode,
        vesselId,
        logId,
        165,
        txid,
        entrydate,
        txtCompleteLong.Text
        );

        if (ddlContext.SelectedItem.Text == "66.1")
        {
            PhoenixMarbolLogORB2.TransactionUpdate(
            usercode,
            vesselId,
            logId,
            166,
            txid,
            entrydate,
            txtQuantityDischarged.Text
            );
        }
        else
        {
            PhoenixMarbolLogORB2.TransactionUpdate(
            usercode,
            vesselId,
            logId,
            167,
            txid,
            entrydate,
            txtQunatityDischargedFacility.Text
            );

            PhoenixMarbolLogORB2.TransactionUpdate(
            usercode,
            vesselId,
            logId,
            168,
            txid,
            entrydate,
            txtfacility.Text
            );
        }

        PhoenixMarbolLogORB2.TransactionUpdate(
            usercode,
            vesselId,
            logId,
            182,
            txid,
            entrydate,
            ddlContext.SelectedItem.Text
            );

    }

    private void TranscationInsert(DateTime entrydate, Guid logTxId, int logId)
    {

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        151,
        logTxId,
        entrydate,
        entrydate.ToString()
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        152,
        logTxId,
        entrydate,
        ddlTank.SelectedItem.Text
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        153,
        logTxId,
        entrydate,
        txtDischargeStartTime.SelectedDate.Value.ToString()
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        154,
        logTxId,
        entrydate,
        txtDischargeStopTime.SelectedDate.Value.ToString()
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        155,
        logTxId,
        entrydate,
        txtUTCStartTime.SelectedDate.Value.ToString()
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        156,
        logTxId,
        entrydate,
        txtUTCStopTime.SelectedDate.Value.ToString()
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        157,
        logTxId,
        entrydate,
        txtIndication.Text
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        158,
        logTxId,
        entrydate,
        txtMonitored.Text
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        159,
        logTxId,
        entrydate,
        txtTimeWhenSeperating.SelectedDate.Value.ToString()
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        160,
        logTxId,
        entrydate,
        txtSepratingLat.Text
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        161,
        logTxId,
        entrydate,
        txtSepratingLong.Text
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        162,
        logTxId,
        entrydate,
        txtStartLat.Text
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        163,
        logTxId,
        entrydate,
        txtStartLong.Text
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        164,
        logTxId,
        entrydate,
        txtCompleteLat.Text
        );

        PhoenixMarbolLogORB2.TransactionInsert(
        usercode,
        vesselId,
        logId,
        165,
        logTxId,
        entrydate,
        txtCompleteLong.Text
        );

        if (ddlContext.SelectedItem.Text == "66.1")
        {
            PhoenixMarbolLogORB2.TransactionInsert(
            usercode,
            vesselId,
            logId,
            166,
            logTxId,
            entrydate,
            txtQuantityDischarged.Text
            );
        }
        else
        {
            PhoenixMarbolLogORB2.TransactionInsert(
            usercode,
            vesselId,
            logId,
            167,
            logTxId,
            entrydate,
            txtQunatityDischargedFacility.Text
            );

            PhoenixMarbolLogORB2.TransactionInsert(
            usercode,
            vesselId,
            logId,
            168,
            logTxId,
            entrydate,
            txtfacility.Text
            );
        }

        PhoenixMarbolLogORB2.TransactionInsert(
            usercode,
            vesselId,
            logId,
            182,
            logTxId,
            entrydate,
            ddlContext.SelectedItem.Text
            );

    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }

        if (string.IsNullOrWhiteSpace(ddlTank.SelectedItem.Text))
        {
            ucError.ErrorMessage = "Identity of Tank value is required";
        }

        if (txtDischargeStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Discharge Start Time is required";
        }

        if (txtDischargeStopTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Discharge Stop Time is required";
        }

        if (txtUTCStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "UTC Start Time is required";
        }

        if (txtUTCStopTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "UTC Stop Time is required";
        }

        if (txtTimeWhenSeperating.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Time when valves separating the dedicated clean ballast is required";
        }

        if (string.IsNullOrWhiteSpace(txtSepratingLat.Text))
        {
            ucError.ErrorMessage = "Position when valves separating the dedicated clean ballast tank Latitude is Required";
        }

        if (string.IsNullOrWhiteSpace(txtSepratingLong.Text))
        {
            ucError.ErrorMessage = "Position when valves separating the dedicated clean ballast tank Longitude is Required";
        }

        if (string.IsNullOrWhiteSpace(txtStartLat.Text))
        {
            ucError.ErrorMessage = "Start Position Latitude is Required";
        }

        if (string.IsNullOrWhiteSpace(txtStartLong.Text))
        {
            ucError.ErrorMessage = "Stop Position Longitude is Required";
        }

        if (string.IsNullOrWhiteSpace(txtCompleteLat.Text))
        {
            ucError.ErrorMessage = "Completed Position Latitude is Required";
        }

        if (string.IsNullOrWhiteSpace(txtCompleteLong.Text))
        {
            ucError.ErrorMessage = "Completed Position Longitude is Required";
        }

        if (string.IsNullOrWhiteSpace(txtIndication.Text))
        {
            ucError.ErrorMessage = "Was there any indication of oil contamination is required";
        }

        if (string.IsNullOrWhiteSpace(txtMonitored.Text))
        {
            ucError.ErrorMessage = "Was the discharge monitored is required";
        }

        if (ddlContext.SelectedItem.Text == "66.1")
        {
            if (string.IsNullOrWhiteSpace(txtQuantityDischarged.Text))
            {
                ucError.ErrorMessage = "Quantity discharged in m3 to sea is required";
            }
        } else
        {
            if (string.IsNullOrWhiteSpace(txtQunatityDischargedFacility.Text))
            {
                ucError.ErrorMessage = "Quantity discharged in m3 to Reception Facility is required";
            }

            if (string.IsNullOrWhiteSpace(txtfacility.Text))
            {
                ucError.ErrorMessage = "Name of Reception Facility/Port is required";

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

    private void LoadContext()
    {
        string[] items = { "66.1", "66.2" };
        ddlContext.DataSource = items;
        ddlContext.DataBind();
        if (ddlContext.Items.Count > 0)
        {
            ddlContext.Items[0].Selected = true;
        }
    }

    private void showHideControls()
    {
        if (ddlContext.SelectedItem.Text == "66.1")
        {
            lblQuantityDischarged.Visible = true;
            txtQuantityDischarged.Visible = true;
            lblQuantityDischargedUnit.Visible = true;

            lblQunatityDischargedFacility.Visible = false;
            txtQunatityDischargedFacility.Visible = false;

            lblFacility.Visible = false;
            txtfacility.Visible = false;


        }
        else
        {
            lblQuantityDischarged.Visible = false;
            txtQuantityDischarged.Visible = false;
            lblQuantityDischargedUnit.Visible = false;

            lblQunatityDischargedFacility.Visible = true;
            txtQunatityDischargedFacility.Visible = true;

            lblFacility.Visible = true;
            txtfacility.Visible = true;

        }
    }

    protected void ddlContext_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        showHideControls();
        SetDefaultData();
    }

    protected void txtIndication_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
}