using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogORB2CrudeOilWashing : PhoenixBasePage
{

    string ReportCode;
    string ItemNo = "9";
    //string ItemName = "Sludge";
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
        logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "CrudeOilWashing");

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

        if (IsPostBack == false)
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
        ReportCode = "D";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        txtItemNo1.Text = "10";
        txtItemNo2.Text = "11";
        txtItemNo3.Text = "12";
        txtItemNo4.Text = "13";
        txtItemNo5.Text = "14";
        txtItemNo6.Text = "15";
        txtItemNo7.Text = "16";
        txtItemNo8.Text = "17";

        TimeSpan startTime = txtstarttime.SelectedTime.HasValue == false ? new TimeSpan() : txtstarttime.SelectedTime.Value;
        TimeSpan stopTime = txtstoptime.SelectedTime.HasValue == false ? new TimeSpan() : txtstoptime.SelectedTime.Value;

        lblRecord.Text = string.Format(" <b> {0}</b> ", txtterminal.Text);
        lblrecord1.Text = string.Format(" <b> {0}</b> ", ddlTransferFrom.Text);
        lblrecord2.Text = string.Format(" <b> {0}</b> ", txtnoofcleaningmachine.Text);
        lblrecord3.Text = string.Format(" <b> {0}</b> ", startTime.ToString());
        lblrecord4.Text = string.Format(" <b> {0}</b> ", txtemployed.Text);
        lblrecord5.Text = string.Format(" <b> {0}</b>  Kg/cm2 ", txtpressure.Text);
        lblrecord6.Text = string.Format(" <b> {0}</b> ", stopTime.ToString());
        lblrecord7.Text = string.Format(" <b> {0}</b> ", txtstate.Text);
        lblrecord8.Text = string.Format(" <b> {0}</b> ", txtremarks.Text);
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

                    if (Convert.ToInt32(row["FLDITEMID"]) == 16)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 17)
                    {
                        txtterminal.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 18)
                    {
                        ddlTransferFrom.SelectedValue = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 19)
                    {
                        txtnoofcleaningmachine.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 20)
                    {
                        txtstarttime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());                        
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 21)
                    {                        
                        txtstoptime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 22)
                    {
                        txtemployed.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 23)
                    {
                        txtpressure.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 24)
                    {
                        txtstate.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 25)
                    {
                        txtremarks.Text = row["FLDVALUE"].ToString();
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

                if (isValidInput() == false )
                {
                    ucError.Visible = true;
                    return;
                }

                if (isValidateSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }


                //Validate transaction Capacity                
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
                                      , null
                                      , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                      , 10
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


    private void TransactionUpdate(int logId)
    {
        
        TimeSpan startTime = txtstarttime.SelectedTime.HasValue == false ? new TimeSpan() : txtstarttime.SelectedTime.Value;
        TimeSpan stopTime = txtstoptime.SelectedTime.HasValue == false ? new TimeSpan() : txtstoptime.SelectedTime.Value;

        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                    , vesselId
                                    , logId
                                    , 16
                                    , General.GetNullableGuid(ViewState["TXID"].ToString())
                                    , entrydate
                                    , General.GetNullableString(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                    , vesselId
                                    , logId
                                    , 17
                                    , General.GetNullableGuid(ViewState["TXID"].ToString())
                                    , entrydate
                                    , General.GetNullableString(txtterminal.Text)                                    
                                );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 18
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(ddlTransferFrom.SelectedValue)                            
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 19
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtnoofcleaningmachine.Text)                            
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 20
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(startTime.ToString())                           
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 21
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(stopTime.ToString())                           
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 22
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtemployed.Text)                            
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 23
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtpressure.Text)
                            
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 24
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtstate.Text)
                            
                        );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 25
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtremarks.Text)
                           
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
                                 , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                 , 10
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
            ucError.ErrorMessage = "Date of Operation    is required";
        }
        if (string.IsNullOrWhiteSpace(txtterminal.Text))
        {
            ucError.ErrorMessage = "Terminal position is required";
        }
        if (string.IsNullOrWhiteSpace(txtnoofcleaningmachine.Text))
        {
            ucError.ErrorMessage = "Number of Tank Cleaning machines in use is required";
        }

        if (txtstarttime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Start Time is required";
        }
        if (txtstoptime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Stop Time is required";
        }

        if (string.IsNullOrWhiteSpace(txtemployed.Text))
        {
            ucError.ErrorMessage = "Washing pattern employed is required";
        }

        if (string.IsNullOrWhiteSpace(txtpressure.Text))
        {
            ucError.ErrorMessage = "Washing line pressure is required";
        }

        if (string.IsNullOrWhiteSpace(txtstate.Text))
        {
            ucError.ErrorMessage = "State method is required";
        }

        if (string.IsNullOrWhiteSpace(txtremarks.Text))
        {
            ucError.ErrorMessage = "Remarks is required";
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
        TimeSpan startTime = txtstarttime.SelectedTime.HasValue == false ? new TimeSpan() : txtstarttime.SelectedTime.Value;
        TimeSpan stopTime = txtstoptime.SelectedTime.HasValue == false ? new TimeSpan() : txtstoptime.SelectedTime.Value;

        PhoenixMarbolLogORB2.TransactionInsert(usercode                                                                         
                                    , vesselId
                                    , logId
                                    , 16
                                    , TranscationId
                                    , entrydate
                                    , General.GetNullableString(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))                                    
                                );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                                    , vesselId
                                    , logId
                                    , 17
                                    , TranscationId
                                    , entrydate
                                    , General.GetNullableString(txtterminal.Text)
                                   
                                );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 18
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(ddlTransferFrom.SelectedValue)
                            
                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 19
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtnoofcleaningmachine.Text)
                            
                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 20
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(startTime.ToString())
                            
                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 21
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(stopTime.ToString())
                           
                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 22
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtemployed.Text)
                           
                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 23
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtpressure.Text)
                            
                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 24
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtstate.Text)
                            
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 25
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtremarks.Text)                            
                        );

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

    protected void txtOperationDate_SelectedDateChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void txtrefresh_TextChanged(object sender, EventArgs e)
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