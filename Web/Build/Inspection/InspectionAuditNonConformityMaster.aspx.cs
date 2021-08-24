using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class InspectionAuditNonConformityMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (!IsPostBack)
        {
            if (Request.QueryString["currenttab"] != null && Request.QueryString["currenttab"].ToString() != "" && Request.QueryString["currenttab"].ToString() == "ncobservation")
            {
                ViewState["currenttab"] = Request.QueryString["currenttab"].ToString();
                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Observation", "OBSERVATION");                
                //toolbar.AddButton("Action", "ACTION");
                toolbar.AddButton("Defect Work Order", "WORKREQUEST");
                toolbar.AddButton("Requisition", "REQUISITION");
                toolbar.AddButton("Close", "CLOSE");
                MenuInspectionObsGeneral.AccessRights = this.ViewState;
                MenuInspectionObsGeneral.MenuList = toolbar.Show();
                MenuInspectionObsGeneral.SelectedMenuIndex = 0;
            }
            else
            {
                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Non Conformity", "NONCONFORMITY");
                toolbar.AddButton("RCA", "MSCAT");
                toolbar.AddButton("CAR", "CAR");
                //toolbar.AddButton("Action", "ACTION");
                toolbar.AddButton("Defect Work Order", "WORKREQUEST");
                toolbar.AddButton("Requisition", "REQUISITION");
                toolbar.AddButton("Close", "CLOSE");
                MenuInspectionObsGeneral.AccessRights = this.ViewState;
                MenuInspectionObsGeneral.MenuList = toolbar.Show();
                MenuInspectionObsGeneral.SelectedMenuIndex = 0;
            }
            ViewState["NCID"] = null;
            if (Request.QueryString["RECORDRESPONSEID"] != null && Request.QueryString["RECORDRESPONSEID"].ToString() != "")
            {
                ViewState["RECORDRESPONSEID"] = Request.QueryString["RECORDRESPONSEID"].ToString();
                EditNonConformity(new Guid(ViewState["RECORDRESPONSEID"].ToString()));
                EditObservation(new Guid(ViewState["RECORDRESPONSEID"].ToString()));
            }
            if (Request.QueryString["CHECKLISTID"] != null && Request.QueryString["CHECKLISTID"].ToString() != "")
            {
                ViewState["CHECKLISTID"] = Request.QueryString["CHECKLISTID"].ToString();
            }
            if (Request.QueryString["checklistrefno"] != null && Request.QueryString["checklistrefno"].ToString() != "")
                ViewState["checklistrefno"] = Request.QueryString["checklistrefno"].ToString();      
            if (Request.QueryString["currenttab"] != null && Request.QueryString["currenttab"].ToString() != "")
                SetSelectedTab(Request.QueryString["currenttab"].ToString());           
            if (ViewState["URL"] == null || ViewState["URL"].ToString() == string.Empty)
                ViewState["URL"] = "../Inspection/InspectionNonConformity.aspx?RecordResponseId=";
        }
        if (ViewState["RECORDRESPONSEID"] != null)
        {
            EditNonConformity(new Guid(ViewState["RECORDRESPONSEID"].ToString()));
            EditObservation(new Guid(ViewState["RECORDRESPONSEID"].ToString()));
        }
    }

    protected void SetSelectedTab(string currenttab)
    {
        if (currenttab != null && currenttab.ToString() != string.Empty)
        {
            if (currenttab == "nonconformity")
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionNonConformity.aspx?RecordResponseId=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + ViewState["RECORDRESPONSEID"] + "&NCID=" + ViewState["NCID"] + "&checklistrefno=" + ViewState["checklistrefno"]+"&CHECKLISTID=" + ViewState["CHECKLISTID"];
                MenuInspectionObsGeneral.SelectedMenuIndex = 0;
            }
            else if (currenttab == "ncobservation")
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionAuditNonConformityObservation.aspx?RecordResponseId=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + ViewState["RECORDRESPONSEID"] + "&NCID=" + ViewState["NCID"] + "&checklistrefno=" + ViewState["checklistrefno"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"];
                MenuInspectionObsGeneral.SelectedMenuIndex = 0;
            }
            else if (currenttab == "causeanaysis")
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionNonConformityCauseAnalysis.aspx?RecordResponseId=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + ViewState["RECORDRESPONSEID"] + "&NCID=" + ViewState["NCID"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"];
                MenuInspectionObsGeneral.SelectedMenuIndex = 1;
            }
            else if (currenttab == "action")
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionNonConformityCorrectiveAction.aspx?RecordResponseId=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + ViewState["RECORDRESPONSEID"] + "&NCID=" + ViewState["NCID"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"];
                MenuInspectionObsGeneral.SelectedMenuIndex = 2;                
            }
            else if (currenttab == "workrequest")
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionWorkRequest.aspx?callfrom=audit&RecordResponseId=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + ViewState["RECORDRESPONSEID"] + "&SelectedChecklistId=" + ViewState["SELECTEDCHECKLISTID"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"];
                if (ViewState["currenttab"] != null && ViewState["currenttab"].ToString() == "ncobservation")
                    MenuInspectionObsGeneral.SelectedMenuIndex = 1;
                else
                    MenuInspectionObsGeneral.SelectedMenuIndex = 3;
            }
            else if (currenttab == "requisition")
            {
                Response.Redirect("../Inspection/InspectionAuditPurchaseForm.aspx?callfrom=" + Request.QueryString["currenttab"] + "&REVIEWDNC=" + ViewState["NCID"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"]);
            }
        }
    }

    protected void InspectionObsGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("RESPONSE"))
            {
                Response.Redirect("../Inspection/InspectionAuditRecordAndResponse.aspx?RecordResponseId=" + ViewState["RECORDRESPONSEID"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"], true);
            }
            else if (dce.CommandName.ToUpper().Equals("NONCONFORMITY"))
            {             
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionNonConformity.aspx?RecordResponseId=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + ViewState["RECORDRESPONSEID"] + "&NCID=" + ViewState["NCID"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"];
                MenuInspectionObsGeneral.SelectedMenuIndex = 0;
            }
            else if (dce.CommandName.ToUpper().Equals("OBSERVATION"))
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionAuditNonConformityObservation.aspx?RecordResponseId=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + ViewState["RECORDRESPONSEID"] + "&NCID=" + ViewState["NCID"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"];
                MenuInspectionObsGeneral.SelectedMenuIndex = 0;
            }
            else if (dce.CommandName.ToUpper().Equals("CAR"))
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionNonConformityCauseAnalysis.aspx?RecordResponseId=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + ViewState["RECORDRESPONSEID"] + "&NCID=" + ViewState["NCID"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"];
                MenuInspectionObsGeneral.SelectedMenuIndex = 1;
            }
            else if (dce.CommandName.ToUpper().Equals("WORKREQUEST"))
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionAuditWorkRequest.aspx?RecordResponseId=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + ViewState["RECORDRESPONSEID"] + "&SelectedChecklistId=" + ViewState["SELECTEDCHECKLISTID"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"];
                if (ViewState["currenttab"] != null && ViewState["currenttab"].ToString() == "ncobservation")
                    MenuInspectionObsGeneral.SelectedMenuIndex = 1;
                else
                    MenuInspectionObsGeneral.SelectedMenuIndex = 2;
            }
            else if (dce.CommandName.ToUpper().Equals("REQUISITION"))
            {
                Response.Redirect("../Inspection/InspectionAuditPurchaseForm.aspx?callfrom=" + Request.QueryString["currenttab"] + "&REVIEWDNC=" + ViewState["NCID"] + "&RECORDRESPONSEID=" + ViewState["RECORDRESPONSEID"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"]);
            }
            if (dce.CommandName.ToUpper().Equals("CLOSE"))
            {
                Response.Redirect("../Inspection/InspectionAuditRecordAndResponse.aspx?RecordResponseId=" + ViewState["RECORDRESPONSEID"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"], true);
            }
            if (dce.CommandName.ToUpper().Equals("MSCAT"))
            {
                ifMoreInfo.Attributes["src"] = "../Inspection/InspectionNonConformityMSCAT.aspx?RecordResponseId=" + ViewState["RECORDRESPONSEID"] + "&NCID=" + ViewState["NCID"];
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void InspectionGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {            
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SCHEDULE"))
            {
                Response.Redirect("../Inspection/InspectionAuditRecordMaster.aspx?RecordResponseId=" + ViewState["RECORDRESPONSEID"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"], true);
            }
            else if (dce.CommandName.ToUpper().Equals("RECORD"))
            {
                Response.Redirect("../Inspection/InspectionAuditRecordAndResponse.aspx?RecordResponseId=" + ViewState["RECORDRESPONSEID"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"], true);
            }
            else if (dce.CommandName.ToUpper().Equals("EXPENSE"))
            {
                Response.Redirect("../Inspection/InspectionAuditExpense.aspx?RecordResponseId=" + ViewState["RECORDRESPONSEID"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"], true);
            }
            else if (dce.CommandName.ToUpper().Equals("ATTACHMENTS"))
            {
                Response.Redirect("../Inspection/InspectionAuditAttachment.aspx?RecordResponseId=" + ViewState["RECORDRESPONSEID"] + "&INSPECTIONDTKEY=" + ViewState["INSPECTIONDTKEY"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"], true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }    

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void EditNonConformity(Guid  RecordResponseId)
    {
        if (RecordResponseId != null)
        {
            DataSet ds = PhoenixInspectionAuditNonConformity.EditAuditNonConformity(RecordResponseId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["AUDITNCDTKEY"] = dr["FLDDTKEY"].ToString();
                ViewState["NCID"] = dr["FLDREVIEWNONCONFORMITYID"].ToString();
            }
        }  
    }

    protected void EditObservation(Guid RecordResponseId)
    {
        if (RecordResponseId != null)
        {
            DataSet ds = PhoenixInspectionAuditNonConformity.EditNonConformityObservation(RecordResponseId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["AUDITNCDTKEY"] = dr["FLDDTKEY"].ToString();
                ViewState["NCID"] = dr["FLDREVIEWNCOBSERVATIONID"].ToString();
            }
        }
    } 

    private bool IsValidObservation()
    {
        ucError.HeaderMessage = "Please provide the following required information";        
                    
        return (!ucError.IsError);

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }   
}
