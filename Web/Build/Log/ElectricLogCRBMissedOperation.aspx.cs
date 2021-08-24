using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogCRBMissedOperation : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
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
            txtOperationDate.Enabled = false;
        }

        string version = Telerik.Web.UI.Common.Version.GetVersion();
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

        DataTable dt = PhoenixMarpolLogCRB.TransactionEdit(usercode, txid);

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {

                if (Convert.ToInt32(row["FLDITEMID"]) == 78)
                {
                    txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                }

                if (Convert.ToInt32(row["FLDITEMID"]) == 79)
                {
                    RadComboBoxItem fromItem = ddlLogBookName.Items.FindItem(x => x.Value == row["FLDVALUE"].ToString());
                    if (fromItem != null)
                    {
                        fromItem.Selected = true;
                    }
                }
            }
            
            LoadIframe(true);
        }
    }

    private void logbookPopulate()
    {
        ddlLogBookName.DataSource = PhoenixMarpolLogCRB.CRBLogRegisterList();
        ddlLogBookName.DataTextField = "FLDNAME";
        ddlLogBookName.DataValueField = "FLDURL";
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

    private void MissedOperationInsert(Guid logTxId)
    {
        NameValueCollection nvc = Filter.MissedOperationalEntryCriteria;
        int logId = PhoenixMarpolLogCRB.GetCRBLogId(usercode, "AdditionalOperational");
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);


        PhoenixMarpolLogCRB.TransactionInsert(
                        usercode,
                        vesselId,
                        logId,
                        78,
                        logTxId,
                        entrydate,
                        txtOperationDate.SelectedDate.Value.ToString()
        );

        PhoenixMarpolLogCRB.TransactionInsert(
                        usercode,
                        vesselId,
                        logId,
                        79,
                        logTxId,
                        entrydate,
                        ddlLogBookName.SelectedItem.Value
        );

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

                if (txid != Guid.Empty)
                {
                    LogBookUpdate();

                }
                else
                {
                    if (Filter.MissedOperationalEntryCriteria != null)
                    {

                        int logId = PhoenixMarpolLogCRB.GetCRBLogId(usercode, "AdditionalOperational");
                        NameValueCollection nvc = Filter.MissedOperationalEntryCriteria;
                        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
                        int entryNo = PhoenixMarpolLogCRB.GetLogBookNextEntryNo(usercode, vesselId);
                        DateTime templateDate = Convert.ToDateTime(nvc["Date"]);
                        Guid logTxId = Guid.Parse(nvc["logTxId"]);
                        int logEntryCount = Convert.ToInt32(nvc["logBookEntry"]);


                        MissedOperationInsert(logTxId);


                        PhoenixMarpolLogCRB.BookEntryInsert(
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

                        int counter = 2;
                        for (int i = 1; i <= logEntryCount; i++)
                        {
                            PhoenixMarpolLogCRB.BookEntryInsert(
                                    usercode,
                                    vesselId,
                                    logTxId,
                                    logId,
                                    General.GetNullableDateTime(nvc["Date"]),
                                    General.GetNullableString(nvc["ReportCode" + i]),
                                    General.GetNullableString(nvc["ItemNo" + i]),
                                    General.GetNullableString(nvc["Record" + i]),
                                    counter
                        );
                            counter++;
                        }


                        PhoenixMarpolLogCRB.BookEntryInsert(
                                    usercode,
                                    vesselId,
                                    logTxId,
                                    logId,
                                    entrydate,
                                    null,
                                    null,
                                    PhoenixElog.GetSignatureName(General.GetNullableString(nvc["inchName"]), General.GetNullableString(nvc["inchRank"]), General.GetNullableDateTime(nvc["inchSignDate"])),
                                    counter + 1,
                                    true
                        );


                        // history insert
                        PhoenixMarpolLogCRB.LogCRBBookEntryStatusInsert(usercode
                                        , vesselId
                                        , logId
                                        , logTxId
                                        , General.GetNullableInteger(nvc["inchId"])
                                        , General.GetNullableString(nvc["inchRank"])
                                        , General.GetNullableString(nvc["InchargeSignature"])
                                        , General.GetNullableDateTime(nvc["inchSignDate"])
                                        , true
                                    );

                        Filter.MissedOperationalEntryCriteria = null;

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

    private void TranscationUpdate()
    {
        int logId = PhoenixMarpolLogCRB.GetCRBLogId(usercode, "AdditionalOperational");
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            121,
                            txid,
                            entrydate,
                            entrydate.ToString()
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                        usercode,
                        vesselId,
                        logId,
                        122,
                        txid,
                        entrydate,
                        ddlLogBookName.SelectedItem.Value
        );
    }


    private void LogBookUpdate()
    {

        if (Filter.MissedOperationalEntryCriteria != null)
        {
            int logId = PhoenixMarpolLogCRB.GetCRBLogId(usercode, "AdditionalOperational");
            NameValueCollection nvc = Filter.MissedOperationalEntryCriteria;
            int logEntryCount = Convert.ToInt32(nvc["logBookEntry"]);

            PhoenixMarpolLogCRB.BookEntryUpdate(usercode
                         , vesselId
                         , txid
                         , logId
                         , txtOperationDate.SelectedDate.Value
                         , lblRecord.Text
                         , 1 );

            int counter = 2;

            for (int i = 1; i <= logEntryCount; i++)
            {
                PhoenixMarpolLogCRB.BookEntryUpdate(usercode
                         , vesselId
                         , txid
                         , logId
                         , txtOperationDate.SelectedDate.Value
                         , General.GetNullableString(nvc["Record" + i])
                         , counter );
                counter++;
            }

            PhoenixMarpolLogCRB.BookEntryUpdate(usercode
                            , vesselId
                            , txid
                            , logId
                            , txtOperationDate.SelectedDate.Value
                            , PhoenixElog.GetSignatureName(General.GetNullableString(nvc["inchName"]), General.GetNullableString(nvc["inchRank"]), General.GetNullableDateTime(nvc["inchSignDate"]))
                            , counter + 1
                            );

            Filter.MissedOperationalEntryCriteria = null;
        }
    }

    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        setDefaultData();
    }


    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        NameValueCollection nvc = Filter.MissedOperationalEntryCriteria;

        if (nvc == null)
        {
            ucError.ErrorMessage = "Operational Log has to be completed before save";
        }

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
        string iframeSrc = txid == Guid.Empty ? string.Format("{0}/Log/{1}?isMissedOperation=true&missedOperationDate={2}", Session["sitepath"], LogName, entrydate.ToString()) : string.Format("{0}/Log/{1}?isMissedOperation=true&missedOperationDate={2}&TxnId={3}&missedOperationEdit={4}", Session["sitepath"], LogName, entrydate.ToString(), txid, isEdit);
        iframe.Src = iframeSrc;
        iframe.Visible = true;
    }

    protected void iframe_OnLoad(object sender, EventArgs e)
    {
        iframe.Attributes.Add("onLoad", "resizeIframe(this);");
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //BindLogData();
    }


    protected void btnEditLog_Click(object sender, EventArgs e)
    {
        //string LogName = ddlLogBookName.SelectedItem.Value;
        //string scriptpopup = String.Format("javascript:parent.openNewWindow('Log','','{0}/Log/{1}?isMissedOperation=true&TxnId={2}&missedOperationEdit=true');", Session["sitepath"], LogName, txid.ToString());
        //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
    }
}