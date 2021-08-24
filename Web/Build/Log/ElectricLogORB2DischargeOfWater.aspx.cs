using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogORB2DischargeOfWater : PhoenixBasePage
{
    string ReportCode;
    string ItemNo = "41";
    int usercode = 0;
    int vesselId = 0;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    int logId = 0;
    DateTime? missedOperationDate = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "DischargeWaterSlopTank");

        ViewState["TXID"] = "";

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            ViewState["TXID"] = Guid.Parse(Request.QueryString["TxnId"]);
            ddlTransferFrom.Enabled = false;
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

        if (!IsPostBack)
        {
            ViewState["lastTranscationDate"] = null;
            ddlFromPopulate();
            SetDefaultData();
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
    private void SetDefaultData()
    {
        ReportCode = "I";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        string StartPosistion = txtStartPosistionLat.Text + " ,  " + txtStartPosistionLog.Text;
        string StopPosistion = txtStopPosistionLat.Text + " ,  " + txtStopPosistionLog.Text;
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        // lblRecord.Text = string.Format("Name of the Tank: <b> {0}</b> ", ddlTransferTo.SelectedItem.Text);
        txtItemNo1.Text = "42";
        txtItemNo2.Text = "43";
        txtItemNo3.Text = "44";
        txtItemNo4.Text = "45";
        txtItemNo5.Text = "46";
        txtItemNo6.Text = "47";
        txtItemNo7.Text = "48";
        txtItemNo8.Text = "49";
        txtItemNo9.Text = "50";
        txtItemNo10.Text = "51";
        txtItemNo11.Text = "52";
        txtItemNo12.Text = "53";
        txtItemNo13.Text = "54";

        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStopTime.SelectedTime.Value;

        TimeSpan utc = txtutc.SelectedTime.HasValue == false ? new TimeSpan() : txtutc.SelectedTime.Value;
        TimeSpan utc2 = txtutc2.SelectedTime.HasValue == false ? new TimeSpan() : txtutc2.SelectedTime.Value;

        lblRecord.Text = string.Format(" <b>{0}</b> ", ddlTransferFrom.Text);
        lblrecord1.Text = string.Format("<b>{0}</b> Hours", txttimeentry.Text);
        lblrecord2.Text = string.Format("<b>{0}</b> Hours", txtdischarge.Text);
        lblrecord3.Text = string.Format("Start at <b>{0}</b> LT, <b>{1}</b> UTC  <b>{2}, {3}</b> ", startTime.ToString(), utc.ToString(), txtStartPosistionLat.Text, txtStartPosistionLog.Text);
        lblrecord4.Text = string.Format("<b>{0}</b> mtr", txtullagedischarge.Text);
        lblrecord5.Text = string.Format("<b>{0}</b> mtr ", txtullagestart.Text);
        lblrecord6.Text = string.Format("<b>{0}</b> m3 at <b>{1}</b> m3/hr ", txtbulkquantity.Text, txtrtdischarge.Text);
        lblrecord7.Text = string.Format("<b>{0}</b> m3 at <b>{1}</b> m3/hr ", txtfinalqtydischarge.Text, txtrtdischarge2.Text);
        lblrecord8.Text = string.Format("Start at <b>{0}</b> LT, <b>{1}</b> UTC,  <b>{2}, {3}</b> ", stopTime.ToString(), utc2.ToString(), txtStopPosistionLat.Text, txtStopPosistionLog.Text);
        lblrecord9.Text = string.Format("<b>{0}</b> ", txtduringdischarge.Text);
        lblrecord10.Text = string.Format("<b>{0}</b> mtr ", txtullagecompletion.Text);
        lblrecord11.Text = string.Format("<b>{0}</b> knots ", txtshipspeed.Text);
        lblrecord12.Text = string.Format("<b>{0}</b> ", txtregularcheckdischsrge.Text);
        lblrecord13.Text = string.Format("<b>{0}</b> ", txtconfirmvalves.Text);

        DataTable dt = PhoenixMarbolLogORB2.ORB2LogRegisterEdit(usercode, General.GetNullableInteger(logId.ToString()));
        if (dt.Rows.Count > 0)
        {
            lblnotes.Text = dt.Rows[0]["FLDNOTES"].ToString();
        }

    }
    protected void MenuOperationApproval_TabStripCommand(object sender, EventArgs e)
    {

    }

    private void ddlFromPopulate()
    {
        ddlTransferFrom.DataSource = PhoenixMarbolLogORB2.ElogLocationDropDown(vesselId, usercode, "CRO");
        ddlTransferFrom.DataBind();
        if (ddlTransferFrom.Items.Count > 0)
        {
            ddlTransferFrom.Items[0].Selected = true;
        }
    }

    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixMarbolLogORB2.GetLogLastTranscationDate(vesselId, usercode);
    }

    private void BindData()
    {
        if (ViewState["TXID"].ToString() != "")
        {
            DataTable dt = PhoenixMarbolLogORB2.TransactionEdit(usercode, new Guid(ViewState["TXID"].ToString()));

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    if (Convert.ToInt32(row["FLDITEMID"]) == 59)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 60)
                    {
                        ddlTransferFrom.SelectedValue = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 61)
                    {
                        txttimeentry.Text = row["FLDVALUE"].ToString();                        
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 62)
                    {
                        txtdischarge.Text = row["FLDVALUE"].ToString();
                        
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 63)
                    {
                        txtStartTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());
                    
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 64)
                    {
                        txtutc.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());                        
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 65)
                    {
                        txtStartPosistionLat.Text = row["FLDVALUE"].ToString();                        
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 66)
                    {
                        txtStartPosistionLog.Text = row["FLDVALUE"].ToString();                        
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 67)
                    {
                        txtStopTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());                        
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 68)
                    {
                        txtutc2.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());                        
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 69)
                    {
                        txtStopPosistionLat.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 70)
                    {
                        txtStopPosistionLog.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 71)
                    {
                        txtbulkquantity.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 72)
                    {
                        txtrtdischarge.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 73)
                    {
                        txtfinalqtydischarge.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 74)
                    {
                        txtrtdischarge2.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 75)
                    {
                        txtullagedischarge.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 76)
                    {
                        txtullagestart.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 77)
                    {
                        txtshipspeed.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 78)
                    {
                        txtullagecompletion.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 79)
                    {
                        if (row["FLDVALUE"].ToString() != "")
                        {
                            txtduringdischarge.SelectedValue = row["FLDVALUE"].ToString();
                        }
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 130)
                    {
                        if (row["FLDVALUE"].ToString() != "")
                        {
                            txtregularcheckdischsrge.SelectedValue = row["FLDVALUE"].ToString();
                        }
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 131)
                    {
                        if (row["FLDVALUE"].ToString() != "")
                        {
                            txtconfirmvalves.SelectedValue = row["FLDVALUE"].ToString();
                        }
                    }
                }
                SetDefaultData();
            }
        }
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false : true;
    }


    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string rank = PhoenixElog.GetRankName(usercode);

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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
            MissedOperationalEntryTemplateUpdate(new Guid(ViewState["TXID"].ToString()));
        }
        else if (ViewState["TXID"].ToString() != "" && isMissedOperationEdit == false)
        {
            TransactionUpdate(logId);
            LogBookUpdate(logId);
        }
        else
        {
            Guid TranscationId = Guid.NewGuid();
            TranscationInsert(entrydate, TranscationId, logId);

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                     , vesselId
                                     , TranscationId
                                     , logId
                                     , entrydate
                                     , txtCode.Text
                                     , txtItemNo.Text
                                     , lblRecord.Text
                                     , 1
                                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                     , vesselId
                                     , TranscationId
                                     , logId
                                     , entrydate
                                     , null
                                     , txtItemNo1.Text
                                     , lblrecord1.Text
                                     , 2
                                 );


            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                    , vesselId
                                    , TranscationId
                                    , logId
                                    , entrydate
                                    , null
                                    , txtItemNo2.Text
                                    , lblrecord2.Text
                                    , 3
                                );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo3.Text
                     , lblrecord3.Text
                     , 4
                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo4.Text
                     , lblrecord4.Text
                     , 5
                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo5.Text
                     , lblrecord5.Text
                     , 6
                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo6.Text
                     , lblrecord6.Text
                     , 7
                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo7.Text
                     , lblrecord7.Text
                     , 8

                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo8.Text
                     , lblrecord8.Text
                     , 9
                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo9.Text
                     , lblrecord9.Text
                     , 10
                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo10.Text
                     , lblrecord10.Text
                     , 11
                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo11.Text
                     , lblrecord11.Text
                     , 12
                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo12.Text
                     , lblrecord12.Text
                     , 13
                 );


            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo13.Text
                     , lblrecord13.Text
                     , 14
                 );


            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                      , vesselId
                                      , TranscationId
                                      , logId
                                      , entrydate
                                      , null
                                      , null
                                      , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                      , 15
                                      , true
                                );

            PhoenixMarbolLogORB2.LogORB2BookEntryStatusInsert(usercode
                                          , vesselId
                                          , logId
                                          , TranscationId
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
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblrecord1.Text);
        nvc.Add("Record3", lblrecord2.Text);
        nvc.Add("Record4", lblrecord3.Text);
        nvc.Add("Record5", lblrecord4.Text);
        nvc.Add("Record6", lblrecord5.Text);
        nvc.Add("Record7", lblrecord6.Text);
        nvc.Add("Record8", lblrecord7.Text);
        nvc.Add("Record9", lblrecord8.Text);
        nvc.Add("Record10", lblrecord9.Text);
        nvc.Add("Record11", lblrecord10.Text);
        nvc.Add("Record12", lblrecord11.Text);
        nvc.Add("Record13", lblrecord12.Text);
        nvc.Add("Record14", lblrecord13.Text);

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
        nvc.Add("ItemNo10", txtItemNo9.Text);
        nvc.Add("ItemNo11", txtItemNo10.Text);
        nvc.Add("ItemNo12", txtItemNo11.Text);
        nvc.Add("ItemNo13", txtItemNo12.Text);
        nvc.Add("ItemNo14", txtItemNo13.Text);

        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "14");
        nvc.Add("logId", logId.ToString());
        nvc.Add("logTxId", logTxId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }


    private void TransactionUpdate(int logId)
    {
        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStopTime.SelectedTime.Value;
        TimeSpan utc = txtutc.SelectedTime.HasValue == false ? new TimeSpan() : txtutc.SelectedTime.Value;
        TimeSpan utc2 = txtutc2.SelectedTime.HasValue == false ? new TimeSpan() : txtutc2.SelectedTime.Value;
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                    , vesselId
                                    , logId
                                    , 59
                                    , General.GetNullableGuid(ViewState["TXID"].ToString())
                                    , entrydate
                                    , General.GetNullableString(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                    , vesselId
                                    , logId
                                    , 60
                                    , General.GetNullableGuid(ViewState["TXID"].ToString())
                                    , entrydate
                                    , General.GetNullableString(ddlTransferFrom.SelectedValue)
                                );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 61
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txttimeentry.Text)
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 62
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtdischarge.Text)
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 63
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(startTime.ToString())
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 64
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(utc.ToString())
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 65
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtStartPosistionLat.Text)
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 66
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtStartPosistionLog.Text)

                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 67
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(stopTime.ToString())

                        );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 68
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(utc2.ToString())
                            );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 69
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtStopPosistionLat.Text)
                            );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 70
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtStopPosistionLog.Text)
                            );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 71
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtbulkquantity.Text)
                            );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 72
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtrtdischarge.Text)

                        );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 73
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtfinalqtydischarge.Text)

                        );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 74
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtrtdischarge2.Text)

                        );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 75
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtullagedischarge.Text)

                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 76
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtullagestart.Text)
                             );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 77
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtshipspeed.Text)
                             );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 78
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtullagecompletion.Text)
                             );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 79
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtduringdischarge.SelectedValue)
                             );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 130
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtregularcheckdischsrge.SelectedValue)
                             );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 131
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtconfirmvalves.SelectedValue)

                        );

    }

    private void LogBookUpdate(int logId)
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                  , vesselId
                                  , General.GetNullableGuid(ViewState["TXID"].ToString())
                                  , logId
                                  , entrydate
                                  , lblRecord.Text
                                  , 1
                              );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord1.Text
                                 , 2
                             );


        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord2.Text
                                 , 3
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord3.Text
                                 , 4
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord4.Text
                                 , 5
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord5.Text
                                 , 6
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord6.Text
                                 , 7
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord7.Text
                                 , 8
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord8.Text
                                 , 9
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                         , vesselId
                         , General.GetNullableGuid(ViewState["TXID"].ToString())
                         , logId
                         , entrydate
                         , lblrecord9.Text
                         , 10
                     );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                         , vesselId
                         , General.GetNullableGuid(ViewState["TXID"].ToString())
                         , logId
                         , entrydate
                         , lblrecord10.Text
                         , 11
                     );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord11.Text
                                 , 12
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord12.Text
                                 , 13
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord13.Text
                                 , 14
                             );


        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                 , 15
                             );



        PhoenixMarbolLogORB2.LogORB2BookEntryStatusUpdate(usercode
                                      , vesselId
                                      , logId
                                      , General.GetNullableGuid(ViewState["TXID"].ToString())
                                      , General.GetNullableInteger(lblinchId.Text)
                                      , General.GetNullableString(lblincRank.Text)
                                      , General.GetNullableString(lblincsign.Text)
                                      , General.GetNullableDateTime(lblincSignDate.Text)
                                        );

    }


    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }

        if (string.IsNullOrWhiteSpace(txtStartPosistionLat.Text) || string.IsNullOrWhiteSpace(txtStartPosistionLog.Text))
        {
            ucError.ErrorMessage = "Start Position is required";
        }

        if (string.IsNullOrWhiteSpace(txtStopPosistionLat.Text) || string.IsNullOrWhiteSpace(txtStopPosistionLog.Text))
        {
            ucError.ErrorMessage = "Stop Position is required";
        }

        if (txtStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Start Time is required";
        }
        if (txtStopTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Stop Time is required";
        }

        if (txtStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "UTC Start time is required";
        }
        if (txtStopTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "UTC Stop Time is required";
        }

        if (string.IsNullOrWhiteSpace(txttimeentry.Text))
        {
            ucError.ErrorMessage = "Time of settling from last entry is required";
        }
        if (string.IsNullOrWhiteSpace(txtdischarge.Text))
        {
            ucError.ErrorMessage = "Time of settling from last discharge is required";
        }

        if (string.IsNullOrWhiteSpace(txtbulkquantity.Text))
        {
            ucError.ErrorMessage = "Bulk quantity is required";
        }

        if (string.IsNullOrWhiteSpace(txtrtdischarge.Text) || string.IsNullOrWhiteSpace(txtrtdischarge2.Text))
        {
            ucError.ErrorMessage = "Rate of discharge is required";
        }

        if (string.IsNullOrWhiteSpace(txtfinalqtydischarge.Text))
        {
            ucError.ErrorMessage = "Final quantity discharged is required";
        }

        if (string.IsNullOrWhiteSpace(txtrtdischarge2.Text))
        {
            ucError.ErrorMessage = "Total ROB in Tank or Shore is required";
        }

        if (string.IsNullOrWhiteSpace(txtullagedischarge.Text))
        {
            ucError.ErrorMessage = "Ullage of total contents at the start of discharge?";
        }

        if (string.IsNullOrWhiteSpace(txtullagestart.Text))
        {
            ucError.ErrorMessage = "Ullage of oil/water interface at start of discharge is required";
        }

        if (string.IsNullOrWhiteSpace(txtshipspeed.Text))
        {
            ucError.ErrorMessage = "Ships Speed is required";
        }

        if (string.IsNullOrWhiteSpace(txtullagecompletion.Text))
        {
            ucError.ErrorMessage = "Ullage of oil/water interface on completion of discharge is required";
        }

        if (string.IsNullOrWhiteSpace(txtduringdischarge.Text))
        {
            ucError.ErrorMessage = "Was the discharge monitoring and control system in operation during discharge?";
        }

        if (string.IsNullOrWhiteSpace(txtregularcheckdischsrge.Text))
        {
            ucError.ErrorMessage = "Was a regular check carried out on the effluent and the surface of the water in the locality of ghe discharge?";
        }

        if (string.IsNullOrWhiteSpace(txtconfirmvalves.Text))
        {
            ucError.ErrorMessage = "Confirm that all applicable valves in the ships pipping system have been closed on completion of discharge from the slop tanks";
        }

        if (PhoenixMarbolLogORB2.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false && string.IsNullOrWhiteSpace(ViewState["TXID"].ToString()))
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        return (!ucError.IsError);
    }

    private void TranscationInsert(DateTime entrydate, Guid TranscationId, int logId)
    {
        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan utc = txtutc.SelectedTime.HasValue == false ? new TimeSpan() : txtutc.SelectedTime.Value;
        TimeSpan utc2 = txtutc2.SelectedTime.HasValue == false ? new TimeSpan() : txtutc2.SelectedTime.Value;

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                                    , vesselId
                                    , logId
                                    , 59
                                    , TranscationId
                                    , entrydate
                                    , General.GetNullableString(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                                    , vesselId
                                    , logId
                                    , 60
                                    , TranscationId
                                    , entrydate
                                    , General.GetNullableString(ddlTransferFrom.SelectedValue)

                                );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 61
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txttimeentry.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 62
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtdischarge.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 63
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(startTime.ToString())

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 64
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(utc.ToString())

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 65
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtStartPosistionLat.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 66
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtStartPosistionLog.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 67
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(stopTime.ToString())

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 68
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(utc2.ToString())

                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 69
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtStopPosistionLat.Text)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 70
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtStopPosistionLog.Text)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 71
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtbulkquantity.Text)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 72
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtrtdischarge.Text)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 73
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtfinalqtydischarge.Text)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 74
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtrtdischarge2.Text)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 75
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtullagedischarge.Text)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 76
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtullagestart.Text)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 77
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtshipspeed.Text)
                        );


        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 78
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtullagecompletion.Text)
                        );


        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 79
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtduringdischarge.SelectedValue)
                        );


        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 130
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtregularcheckdischsrge.SelectedValue)
                        );


        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 131
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtconfirmvalves.SelectedValue)
                        );


    }

    private void OnDutyEngineerSign()
    {
        if (Filter.DutyEngineerSignatureFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.DutyEngineerSignatureFilterCriteria;
            DateTime signedDate = DateTime.Parse(nvc.Get("date"));
            lblincRank.Text = nvc.Get("rank");
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
    protected void txt_selectedindexchaged(object sender, EventArgs e)
    {

    }
    protected void txtOperationDate_TextChangedEvent(object sender, EventArgs e)
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

    protected void txt_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        SetDefaultData();
    }

    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void txtrefresh_TextChanged(object sender, EventArgs e)
    {
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