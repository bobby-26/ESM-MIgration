using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogMissedOperationalEntry : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    string itemNo = null;
    string code = "I";
    Guid txid = Guid.Empty;
    string ReportCode = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            txid = Guid.Parse(Request.QueryString["TxnId"]);
            ddlLogBookName.Enabled = false;
        }
        

        if (IsPostBack == false)
        {
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            ShowToolBar();
            setDefaultData();
            logbookPopulate();
            BindData();
        }
    }

    private void BindData()
    {
        if (txid == Guid.Empty) return;
        DataSet missoperationDataSet = PhoenixElog.MissedOperationEntrySearch(usercode, vesselId, txid);
        Filter.MissedOperationalEntryCriteria = null;

        if (missoperationDataSet != null && missoperationDataSet.Tables.Count > 0)
        {
            if (missoperationDataSet != null && missoperationDataSet.Tables[0].Rows.Count > 0)
            {
                DataRow missedOperationRow = missoperationDataSet.Tables[0].Rows[0];
                RadComboBoxItem log = ddlLogBookName.Items.FindItem(x => x.Text == missedOperationRow["FLDLOGNAME"].ToString());
                if (log != null)
                {
                    log.Selected = true;
                    LoadIframe(true);
                }
                txtOperationDate.SelectedDate = Convert.ToDateTime(missedOperationRow["FLDMISSEDOPERATIONDATE"]);
                txtOperationDate.Enabled = false;
            }
        }
    }

    private void logbookPopulate()
    {
        ddlLogBookName.DataSource = PhoenixElog.GetLogs().Where(n => n.Key != "Missed Operational Entry");
        ddlLogBookName.DataTextField = "Key";
        ddlLogBookName.DataValueField = "Value";
        ddlLogBookName.DataBind();
    }

    private void setDefaultData()
    {
        txtCode.Text = code;
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy") : DateTime.Now.ToString("dd-MM-yyyy");
        lblRecord.Text = string.Format("Entry Pertaining to an earlier missed operational entry");
    }
    public void ShowToolBar()
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

                string logName = "Missed Operational Entry";
             


                if (txid != Guid.Empty)
                {
                    NameValueCollection nvc = Filter.MissedOperationalEntryCriteria;
                    int logEntryCount = Convert.ToInt32(nvc["logBookEntry"]);
                    bool isWeeklyEntry = Convert.ToBoolean(nvc["isWeeklyEntry"]);

                    if (isWeeklyEntry)
                    {
                        WeeklyEntryBookEntryUpdate(nvc, logEntryCount);
                    }
                    else
                    {
                        LogBookUpdate();
                    }

                } else
                {
                    if (Filter.MissedOperationalEntryCriteria != null)
                    {

                        NameValueCollection nvc = Filter.MissedOperationalEntryCriteria;
                        int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselId);
                        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
                        int logEntryCount = Convert.ToInt32(nvc["logBookEntry"]);

                        bool isWeeklyEntry = Convert.ToBoolean(nvc["isWeeklyEntry"]);
                        bool isTranscation = Convert.ToBoolean(nvc["isTransaction"]);
                        bool isAttachementRequired = Convert.ToBoolean(nvc["isAttachmentRequired"]);


                        if (isWeeklyEntry)
                        {

                            int sludgeTankCount = Convert.ToInt32(nvc["sludgeTankCount"]);
                            int bilgeTankCount = Convert.ToInt32(nvc["bilgeTankCount"]);
                            WeeklyEntryBookEntryInsert(nvc, logName, ref entryNo, ref entrydate, isTranscation,  logEntryCount, isAttachementRequired);
                        }
                        else
                        {

                            Guid TranscationId = Guid.Parse(nvc["txId"]);

                            Guid logId = Guid.Parse(nvc["logId"]);

                            PhoenixElog.MissedOperationStatusUpdate(usercode
                                                        , vesselId
                                                        , logId
                                                        , true
                                                        , entrydate
                                                        , General.GetNullableInteger(nvc["inchId"])
                                                        , General.GetNullableString(nvc["inchRank"])
                                                        , General.GetNullableString(nvc["inchName"])
                                                        , General.GetNullableDateTime(nvc["inchSignDate"])
                                                        , General.GetNullableInteger(null)
                                                        , General.GetNullableString(null)
                                                        , General.GetNullableString(null)
                                                        , General.GetNullableDateTime(null)
                                                        );


                            PhoenixElog.LogBookEntryStatusInsert(usercode
                                                          , vesselId
                                                          , logId
                                                          , General.GetNullableInteger(nvc["inchId"])
                                                          , General.GetNullableString(nvc["inchRank"])
                                                          , General.GetNullableString(nvc["InchargeSignature"])
                                                          , General.GetNullableDateTime(nvc["inchSignDate"])
                                                          , General.GetNullableInteger(null)
                                                          , General.GetNullableString(null)
                                                          , General.GetNullableString(null)
                                                          , General.GetNullableDateTime(null)
                                                            );

                            PhoenixElog.LogBookEntryInsert(usercode
                                                        , entrydate
                                                        , itemNo
                                                        , lblRecord.Text
                                                        , txtCode.Text
                                                        , TranscationId
                                                        , 1
                                                        , null
                                                        , null
                                                        , General.GetNullableString(nvc["InchargeSignature"])
                                                        , vesselId
                                                        , isTranscation
                                                        , null
                                                        , logName
                                                        , isAttachementRequired
                                                        , entryNo
                                                        , logId
                                                    );

                            int counter = 2;
                            for (int i = 1; i <= logEntryCount; i++)
                            {
                                PhoenixElog.LogBookEntryInsert(usercode
                                                          , General.GetNullableDateTime(nvc["Date"])
                                                          , General.GetNullableString(nvc["ItemNo" + i])
                                                          , General.GetNullableString(nvc["Record" + i])
                                                          , General.GetNullableString(nvc["ReportCode" + i])
                                                          , TranscationId
                                                          , counter
                                                          , null
                                                          , null
                                                          , General.GetNullableString(nvc["inchRank"])
                                                          , vesselId
                                                          , isTranscation
                                                          , null
                                                          , logName
                                                          , isAttachementRequired
                                                          , entryNo
                                                          , logId
                                                      );
                                counter++;
                            }

                            PhoenixElog.LogBookEntryInsert(usercode
                                                      , General.GetNullableDateTime(nvc["Date"])
                                                      , null
                                                      , PhoenixElog.GetSignatureName(General.GetNullableString(nvc["inchName"]), General.GetNullableString(nvc["inchRank"]), General.GetNullableDateTime(nvc["inchSignDate"]))
                                                      , null
                                                      , TranscationId
                                                      , counter + 1
                                                      , false
                                                      , null
                                                      , General.GetNullableString(nvc["inchRank"])
                                                      , vesselId
                                                      , isTranscation
                                                      , null
                                                      , logName
                                                      , isAttachementRequired
                                                      , entryNo
                                                      , logId
                                                );

                            PhoenixElog.LogBookEntryInsert(usercode
                                                      , General.GetNullableDateTime(nvc["Date"])
                                                      , null
                                                      , PhoenixElog.GetSignatureName(General.GetNullableString(""), General.GetNullableString(""), General.GetNullableDateTime(""), true)
                                                      , null
                                                      , TranscationId
                                                      , counter + 2
                                                      , true
                                                      , null
                                                      , General.GetNullableString(nvc["inchRank"])
                                                      , vesselId
                                                      , isTranscation
                                                      , null
                                                      , logName
                                                      , isAttachementRequired
                                                      , entryNo
                                                      , logId
                                                );
                        }
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void WeeklyEntryBookEntryInsert(NameValueCollection nvc, string logName, ref int entryNo, ref DateTime entrydate, bool isTranscation,  int sludgeTankCount, bool isAttachementRequired = false)
    {
        int txCounter = 1;
        for (int i = 1; i <= sludgeTankCount; i++)
        {
            Guid TranscationId = Guid.Parse(nvc["txId" + txCounter]);
            Guid logId = Guid.Parse(nvc["logId" + txCounter]);
            entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
            entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselId);


            PhoenixElog.MissedOperationStatusUpdate(usercode
                                        , vesselId
                                        , logId
                                        , true
                                        , Convert.ToDateTime(nvc["Date"])
                                        , General.GetNullableInteger(nvc["inchId"])
                                        , General.GetNullableString(nvc["inchRank"])
                                        , General.GetNullableString(nvc["inchName"])
                                        , General.GetNullableDateTime(nvc["inchSignDate"])
                                        , General.GetNullableInteger(null)
                                        , General.GetNullableString(null)
                                        , General.GetNullableString(null)
                                        , General.GetNullableDateTime(null)
                                        );


            PhoenixElog.LogBookEntryStatusInsert(usercode
                                          , vesselId
                                          , logId
                                          , General.GetNullableInteger(nvc["inchId"])
                                          , General.GetNullableString(nvc["inchRank"])
                                          , General.GetNullableString(nvc["InchargeSignature"])
                                          , General.GetNullableDateTime(nvc["inchSignDate"])
                                          , General.GetNullableInteger(null)
                                          , General.GetNullableString(null)
                                          , General.GetNullableString(null)
                                          , General.GetNullableDateTime(null)
                                            );

            PhoenixElog.LogBookEntryInsert(usercode
                                       , General.GetNullableDateTime(nvc["Date"]) != null ? General.GetNullableDateTime(nvc["Date"]) : entrydate
                                       , itemNo
                                       , lblRecord.Text
                                       , txtCode.Text
                                       , TranscationId
                                       , 1
                                       , null
                                       , null
                                       , General.GetNullableString(nvc["InchargeSignature"])
                                       , vesselId
                                       , isTranscation
                                       , null
                                       , logName
                                       , isAttachementRequired
                                       , entryNo
                                       , logId
                                   );


            PhoenixElog.LogBookEntryInsert(usercode
                                          , entrydate
                                          , General.GetNullableString(nvc["ItemNo" + txCounter])
                                          , General.GetNullableString(nvc["Record" + txCounter])
                                          , General.GetNullableString(nvc["ReportCode" + txCounter])
                                          , TranscationId
                                          , 2
                                          , null
                                          , null
                                          , General.GetNullableString(nvc["inchRank"])
                                          , vesselId
                                          , isTranscation
                                          , null
                                          , logName
                                          , isAttachementRequired
                                          , entryNo
                                          , logId
                                      );

            PhoenixElog.LogBookEntryInsert(usercode
                                          , entrydate
                                          , General.GetNullableString(nvc["ItemNo" + (txCounter + 1)])
                                          , General.GetNullableString(nvc["Record" + (txCounter + 1)])
                                          , General.GetNullableString(nvc["ReportCode" + (txCounter + 1)])
                                          , TranscationId
                                          , 3
                                          , null
                                          , null
                                          , General.GetNullableString(nvc["inchRank"])
                                          , vesselId
                                          , isTranscation
                                          , null
                                          , logName
                                          , isAttachementRequired
                                          , entryNo
                                          , logId
                                      );


            PhoenixElog.LogBookEntryInsert(usercode
                                         , entrydate
                                         , General.GetNullableString(nvc["ItemNo" + (txCounter + 2)])
                                         , General.GetNullableString(nvc["Record" + (txCounter + 2)])
                                         , General.GetNullableString(nvc["ReportCode" + (txCounter + 2)])
                                         , TranscationId
                                         , 4
                                         , null
                                         , null
                                         , General.GetNullableString(nvc["inchRank"])
                                         , vesselId
                                         , isTranscation
                                         , null
                                         , logName
                                         , isAttachementRequired
                                         , entryNo
                                         , logId
                                     );


            PhoenixElog.LogBookEntryInsert(usercode
                                     , entrydate
                                     , null
                                     , PhoenixElog.GetSignatureName(General.GetNullableString(nvc["inchName"]), General.GetNullableString(nvc["inchRank"]), General.GetNullableDateTime(nvc["inchSignDate"]))
                                     , null
                                     , TranscationId
                                     , 5
                                     , false
                                     , null
                                     , General.GetNullableString(nvc["inchRank"])
                                     , vesselId
                                     , isTranscation
                                     , null
                                     , logName
                                     , isAttachementRequired
                                     , entryNo
                                     , logId
                               );

            PhoenixElog.LogBookEntryInsert(usercode
                                      , entrydate
                                      , null
                                      , PhoenixElog.GetSignatureName(General.GetNullableString(""), General.GetNullableString(""), General.GetNullableDateTime(""), true)
                                      , null
                                      , TranscationId
                                      , 6
                                      , true
                                      , null
                                      , General.GetNullableString(nvc["inchRank"])
                                      , vesselId
                                      , isTranscation
                                      , null
                                      , logName
                                      , isAttachementRequired
                                      , entryNo
                                      , logId
                                );

            txCounter = txCounter + 3;
        }
    }


    private void WeeklyEntryBookEntryUpdate(NameValueCollection nvc, int logEntryCount)
    {
        int txCounter = 1;
        for (int i = 1; i <= logEntryCount; i++)
        {

            PhoenixElog.LogBookEntryUpdate(usercode
                     , lblRecord.Text
                     , txid
                     , 1
                     , null);

            PhoenixElog.LogBookEntryUpdate(usercode
                      , General.GetNullableString(nvc["Record" + txCounter])
                      , txid
                      , 2
                      , null);

            PhoenixElog.LogBookEntryUpdate(usercode
                      , General.GetNullableString(nvc["Record" + (txCounter + 1)])
                      , txid
                      , 3
                      , null);

            PhoenixElog.LogBookEntryUpdate(usercode
                      , General.GetNullableString(nvc["Record" + (txCounter + 2)])
                      , txid
                      , 4
                      , null);

            PhoenixElog.LogBookEntryUpdate(usercode
                     , PhoenixElog.GetSignatureName(General.GetNullableString(nvc["inchName"]), General.GetNullableString(nvc["inchRank"]), General.GetNullableDateTime(nvc["inchSignDate"]))
                     , txid
                     , 5
                     , false);

            PhoenixElog.LogBookEntryUpdate(usercode
                     , PhoenixElog.GetSignatureName("", "", null, true)
                     , txid
                     , 6
                     , true);

            txCounter = txCounter + 3;
        }
    }


    private void LogBookUpdate()
    {

        if (Filter.MissedOperationalEntryCriteria != null)
        {

            NameValueCollection nvc = Filter.MissedOperationalEntryCriteria;
            int logEntryCount = Convert.ToInt32(nvc["logBookEntry"]);

            PhoenixElog.MissedOperationalEntryUpdate(usercode
                                            , vesselId
                                            , txid
                                            , General.GetNullableInteger(nvc["inchId"])
                                            , General.GetNullableString(nvc["inchRank"])
                                            , General.GetNullableString(nvc["InchargeSignature"])
                                            , General.GetNullableDateTime(nvc["inchSignDate"])
                                            , General.GetNullableInteger(null)
                                            , General.GetNullableString(null)
                                            , General.GetNullableString(null)
                                            , General.GetNullableDateTime(null)
                );


            PhoenixElog.LogBookEntryUpdate(usercode
                         , lblRecord.Text
                         , txid
                         , 1
                         , null);

            int counter = 2;

            for (int i = 1; i <= logEntryCount; i++)
            {
                PhoenixElog.LogBookEntryUpdate(usercode
                         , General.GetNullableString(nvc["Record" + i])
                         , txid
                         , counter
                         , null);
                counter++;
            }

            PhoenixElog.LogBookEntryUpdate(usercode
                            , PhoenixElog.GetSignatureName(General.GetNullableString(nvc["inchName"]), General.GetNullableString(nvc["inchRank"]), General.GetNullableDateTime(nvc["inchSignDate"]))
                            , txid
                            , counter + 1
                            , false);

            PhoenixElog.LogBookEntryUpdate(usercode
                            , PhoenixElog.GetSignatureName("", "", null, true)
                            , txid
                            , counter + 2
                            , true);
        }
    }

    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        setDefaultData();
    }

    
    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (string.IsNullOrWhiteSpace(lblinchId.Text))
        //{
        //    ucError.ErrorMessage = "Duty Engineer Signature is Required Before Save";
        //}

        //if (Filter.MissedOperationalEntryCriteria == null)
        //{
        //    ucError.ErrorMessage = "Please fill the operational log Before Save";
        //}


        return (!ucError.IsError);
    }

    protected void txt_TextChanged(object sender, EventArgs e)
    {
        setDefaultData();
    }

    protected void ddlLogBookName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        LoadIframe(false);
    }

    private void LoadIframe(bool isEdit)
    {
        string LogName = ddlLogBookName.SelectedItem.Value;
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
        string iframeSrc = txid == Guid.Empty ? string.Format("{0}/Log/{1}?isMissedOperation=true&missedOperationDate={2}", Session["sitepath"], LogName, entrydate.ToString()) :   string.Format("{0}/Log/{1}?isMissedOperation=true&missedOperationDate={2}&TxnId={3}&missedOperationEdit={4}", Session["sitepath"], LogName, entrydate.ToString(), txid, isEdit); 
        iframe.Src = iframeSrc;
        iframe.Visible = true;
    }

    protected void iframe_OnLoad(object sender, EventArgs e)
    {
        iframe.Attributes.Add("onLoad", "resizeIframe(this);");
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindLogData();
    }

    private void BindLogData()
    {
        if (Filter.MissedOperationalEntryCriteria != null)
        {
            NameValueCollection nvc = Filter.MissedOperationalEntryCriteria;
            lblTempDate0.Text = nvc["Date"];
            int recordCount = Convert.ToInt32(nvc["logBookEntry"]);
            for (int i = 1; i <= recordCount; i++)
            {
                RadLabel ctrlLblTempRecord = (RadLabel)template.FindControl("lblTempRecord" + i);
                RadLabel ctrlLblTempCode = (RadLabel)template.FindControl("lblTempCode" + i);
                RadLabel ctrlLblItemNo = (RadLabel)template.FindControl("lblTempItemNo" + i);
                ctrlLblTempRecord.Text = nvc["Record" + i];
                ctrlLblTempCode.Text = nvc["ReportCode" + i];
                ctrlLblItemNo.Text = nvc["ItemNo" + i];
                HtmlTableRow row = (HtmlTableRow)template.FindControl("temprecord" + i);
                row.Visible = true;
            }

            Guid logId = Guid.Parse(nvc["logId"]);
            Guid TranscationId = Guid.Empty;
            DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

            PhoenixElog.MissedOperationalEntryInsert(usercode
                                                , vesselId
                                                , logId
                                                , nvc["logBookName"]
                                                , ref TranscationId
                                                , entrydate
                                                , Convert.ToDateTime(nvc["Date"]) 
                                                , false
                            );

        }
    }

    protected void btnEditLog_Click(object sender, EventArgs e)
    {
        string LogName = ddlLogBookName.SelectedItem.Value;
        string scriptpopup = String.Format("javascript:parent.openNewWindow('MissedOperationEntry','','{0}/Log/{1}?isMissedOperation=true&TxnId={2}&missedOperationEdit=true');", Session["sitepath"], LogName, txid.ToString());
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
    }
}