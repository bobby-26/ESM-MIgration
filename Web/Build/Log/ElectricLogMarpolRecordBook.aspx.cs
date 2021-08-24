using SouthNests.Phoenix.Framework;
using System;

public partial class Log_ElectricLogMarpolRecordBook : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            EventLog("ElectricLogMarpolRecordBook", "Login Marpol Log Book", "Login", "MARPOL");
        }
        btnOilRecordBookP1.Visible = SessionUtil.CanAccess(this.ViewState, "ORB1");
        btnOilRecordBookP2.Visible = SessionUtil.CanAccess(this.ViewState, "ORB2");
        btnCargoRecordBook.Visible = SessionUtil.CanAccess(this.ViewState, "CRB");
        btnGarbageRecordBookP1.Visible = SessionUtil.CanAccess(this.ViewState, "GRB");
        btnGarbageRecordBookP2.Visible = SessionUtil.CanAccess(this.ViewState, "GRB2");
        btnAnnexure.Visible = SessionUtil.CanAccess(this.ViewState, "ANNEXURE");
        AssignPopUp();
           
    }

    private void AssignPopUp()
    {
        btnOilRecordBookP1.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('oilLog','','{0}/Log/ElectronicLogOperationReport.aspx',null, null, null, null, null, {{model:true}} );", Session["sitepath"]) );
        btnOilRecordBookP2.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('oilLog','','{0}/Log/ElectricLogORB2OperationList.aspx',null, null, null, null, null, {{model:true}} );", Session["sitepath"]));
        btnCargoRecordBook.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('oilLog','','{0}/Log/ElectricLogCRBOperationList.aspx',null, null, null, null, null, {{model:true}} );", Session["sitepath"]));
        btnGarbageRecordBookP1.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('oilLog','','{0}/Log/ElectricLogGRBOperationList.aspx',null, null, null, null, null, {{model:true}} );", Session["sitepath"]));
        btnGarbageRecordBookP2.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('oilLog','','{0}/Log/ElectricLogGRB2OperationList.aspx',null, null, null, null, null, {{model:true}} );", Session["sitepath"]));
        btnAnnexure.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('AnnexLog','','{0}/Log/ElectricLogAnnexureOperationList.aspx',null, null, null, null, null, {{model:true}} );", Session["sitepath"]));
    }

    private void EventLog(string url, string Caption, string CommandName, string logbook)
    {
        PhoenixElogCommon.MarpolEventLogInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                url,
                                                CommandName,
                                                Caption,
                                                logbook,
                                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                General.GetNullableDateTime(null), General.GetNullableDateTime(null));
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        EventLog("ElectricLogMarpolRecordBook", "Logout Marpol Log Book", "Logout", "MARPOL");
    }

}