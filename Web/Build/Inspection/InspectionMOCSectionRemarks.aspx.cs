using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionMOCSectionRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

        if (!IsPostBack)
        {
            ViewState["SUPPORTREQID"] = "";
            ViewState["MOCSECTIONID"] = "";

            if (Request.QueryString["SUPPORTREQID"] != null && Request.QueryString["SUPPORTREQID"].ToString() != string.Empty)
            {
                ViewState["SUPPORTREQID"] = Request.QueryString["SUPPORTREQID"].ToString();
                ViewState["MOCSECTIONID"] = Request.QueryString["MOCSECTIONID"].ToString();
            }
            Bind();
            Binductittle();
            MenuApprovalRemarks.AccessRights = this.ViewState;
            MenuApprovalRemarks.MenuList = toolbarmain.Show();

        }
    }

    private void Bind()
    {
        if ((ViewState["MOCSECTIONID"].Equals("10")))
        {
            DataSet ds;
            ds = PhoenixInspectionMOCRequestForChange.MOCSupportRequiredEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["SUPPORTREQID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblsupport.Text = "From Whom ?";

                DataRow dr = ds.Tables[0].Rows[0];
                txtApprovalRemarks.Text = dr["FLDSUPPORTREQUIREDDETAILS"].ToString();
                ViewState["MOCID"] = dr["FLDMOCID"].ToString();
                ViewState["Vesselid"] = dr["FLDVESSELID"].ToString();
                ViewState["questionid"] = dr["FLDSUPPORTREQUIREDQUESTIONID"].ToString();
                ViewState["supportreqyn"] = dr["FLDSUPPORTREQUIREDYN"].ToString();
                ViewState["otherdetails"] = dr["FLDSUPPORTREQUIREDDETAILS"].ToString();
                txtSectionName.Text = dr["FLDSUPPORTREQUIREDNAME"].ToString();
            }
        }
        else if ((ViewState["MOCSECTIONID"].Equals("11")))
        {

            DataSet ds = PhoenixInspectionMOCRequestForChange.MOCExternalApprovalRequiredEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["SUPPORTREQID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblsupport.Text = "From Whom ?";

                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["FLDEXTERNALAPPROVALDETAILS"].ToString() != "")
                    txtApprovalRemarks.Text = dr["FLDEXTERNALAPPROVALDETAILS"].ToString();
                ViewState["MOCID"] = dr["FLDMOCID"].ToString();
                ViewState["Vesselid"] = dr["FLDVESSELID"].ToString();
                ViewState["questionid"] = dr["FLDEXTERNALAPPROVALQUESTIONID"].ToString();
                ViewState["supportreqyn"] = dr["FLDEXTERNALAPPROVALYN"].ToString();
                ViewState["otherdetails"] = dr["FLDEXTERNALAPPROVALDETAILS"].ToString();
                txtSectionName.Text = dr["FLDSUPPORTREQUIREDNAME"].ToString();
            }
        }
        else if ((ViewState["MOCSECTIONID"].Equals("12")))
        {
            DataSet ds = PhoenixInspectionMOCRequestForChange.MOCSupportItemRequiredEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["SUPPORTREQID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblsupport.Text = "From Whom ?";

                DataRow dr = ds.Tables[0].Rows[0];
                txtApprovalRemarks.Text = dr["FLDMOCSUPPORTITEMREQUIREDDETAILS"].ToString();
                ViewState["MOCID"] = dr["FLDMOCID"].ToString();
                ViewState["Vesselid"] = dr["FLDVESSELID"].ToString();
                ViewState["questionid"] = dr["FLDMOCSUPPORTITEMQUESTIONID"].ToString();
                ViewState["supportreqyn"] = dr["FLDMOCSUPPORTITEMREQUIREDYN"].ToString();
                ViewState["otherdetails"] = dr["FLDMOCSUPPORTITEMREQUIREDDETAILS"].ToString();
                txtSectionName.Text = dr["FLDSUPPORTREQUIREDNAME"].ToString();
            }
        }
        else if ((ViewState["MOCSECTIONID"].Equals("13")))
        {
            DataSet ds = PhoenixInspectionMOCRequestForChange.MOCShipboardPersonnelAffectedEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["SUPPORTREQID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblsupport.Text = "Dept";

                DataRow dr = ds.Tables[0].Rows[0];
                txtApprovalRemarks.Text = dr["FLDSBPERSONNELAFFECTEDDETAILS"].ToString();
                ViewState["MOCID"] = dr["FLDMOCID"].ToString();
                ViewState["Vesselid"] = dr["FLDVESSELID"].ToString();
                ViewState["questionid"] = dr["FLDSBPERSONNELAFFECTEDQUESTIONID"].ToString();
                ViewState["supportreqyn"] = dr["FLDSBPERSONNELAFFECTEDYN"].ToString();
                ViewState["otherdetails"] = dr["FLDSBPERSONNELAFFECTEDDETAILS"].ToString();
                txtSectionName.Text = dr["FLDSUPPORTREQUIREDNAME"].ToString();
            }
        }
        else if ((ViewState["MOCSECTIONID"].Equals("14")))
        {
            DataSet ds = PhoenixInspectionMOCRequestForChange.MOCShoreBasedPersonnelAffectedEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["SUPPORTREQID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblsupport.Text = "Dept / Entity";

                DataRow dr = ds.Tables[0].Rows[0];
                txtApprovalRemarks.Text = dr["FLDSBPERSONNELAFFECTEDDETAILS"].ToString();
                ViewState["MOCID"] = dr["FLDMOCID"].ToString();
                ViewState["Vesselid"] = dr["FLDVESSELID"].ToString();
                ViewState["questionid"] = dr["FLDSBPERSONNELAFFECTEDQUESTIONID"].ToString();
                ViewState["supportreqyn"] = dr["FLDSBPERSONNELAFFECTEDYN"].ToString();
                ViewState["otherdetails"] = dr["FLDSBPERSONNELAFFECTEDDETAILS"].ToString();
                txtSectionName.Text = dr["FLDSUPPORTREQUIREDNAME"].ToString();
            }
        }
    }

    protected void MenuApprovalRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (General.GetNullableString(txtApprovalRemarks.Text) == null)
            {
                lblMessage.Text = "Remarks is required.";
                return;
            }

            string Script = "";

            if (ViewState["MOCSECTIONID"] != null && ViewState["MOCSECTIONID"] != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    if ((ViewState["MOCSECTIONID"].Equals("10")))
                    {
                        PhoenixInspectionMOCRequestForChange.MOCSupportRequiredInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(ViewState["SUPPORTREQID"].ToString())
                                                                                    , new Guid((ViewState["MOCID"]).ToString())
                                                                                    , int.Parse(ViewState["Vesselid"].ToString())
                                                                                    , Int32.Parse(ViewState["questionid"].ToString())
                                                                                    , Int32.Parse(ViewState["supportreqyn"].ToString())
                                                                                    , txtApprovalRemarks.Text
                                                                                    , ViewState["otherdetails"].ToString()
                                                                                    );
                    }
                    if ((ViewState["MOCSECTIONID"].Equals("11")))
                    {
                        PhoenixInspectionMOCRequestForChange.MOCExternalApprovalRequired(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(ViewState["SUPPORTREQID"].ToString())
                                                                                    , new Guid((ViewState["MOCID"]).ToString())
                                                                                    , int.Parse(ViewState["Vesselid"].ToString())
                                                                                    , Int32.Parse(ViewState["questionid"].ToString())
                                                                                    , Int32.Parse(ViewState["supportreqyn"].ToString())
                                                                                    , txtApprovalRemarks.Text
                                                                                    , ViewState["otherdetails"].ToString()
                                                                                    );
                    }
                    if ((ViewState["MOCSECTIONID"].Equals("12")))
                    {
                        PhoenixInspectionMOCRequestForChange.MOCSupportItemRequiredInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(ViewState["SUPPORTREQID"].ToString())
                                                                                    , new Guid((ViewState["MOCID"]).ToString())
                                                                                    , int.Parse(ViewState["Vesselid"].ToString())
                                                                                    , Int32.Parse(ViewState["questionid"].ToString())
                                                                                    , Int32.Parse(ViewState["supportreqyn"].ToString())
                                                                                    , txtApprovalRemarks.Text
                                                                                    , ViewState["otherdetails"].ToString()
                                                                                    );
                    }
                    if ((ViewState["MOCSECTIONID"].Equals("13")))
                    {
                        PhoenixInspectionMOCRequestForChange.MOCShipboardPersonnelAffectedInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(ViewState["SUPPORTREQID"].ToString())
                                                                                    , new Guid((ViewState["MOCID"]).ToString())
                                                                                    , int.Parse(ViewState["Vesselid"].ToString())
                                                                                    , Int32.Parse(ViewState["questionid"].ToString())
                                                                                    , Int32.Parse(ViewState["supportreqyn"].ToString())
                                                                                    , txtApprovalRemarks.Text
                                                                                    , ViewState["otherdetails"].ToString()
                                                                                    );
                    }
                    if ((ViewState["MOCSECTIONID"].Equals("14")))
                    {

                        PhoenixInspectionMOCRequestForChange.MOCShoreBasedPersonnelAffectedInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(ViewState["SUPPORTREQID"].ToString())
                                                                                    , new Guid((ViewState["MOCID"]).ToString())
                                                                                    , int.Parse(ViewState["Vesselid"].ToString())
                                                                                    , Int32.Parse(ViewState["questionid"].ToString())
                                                                                    , Int32.Parse(ViewState["supportreqyn"].ToString())
                                                                                    , txtApprovalRemarks.Text
                                                                                    , ViewState["otherdetails"].ToString()
                                                                                    );
                    }
                    if ((ViewState["MOCSECTIONID"].Equals("15")))
                    {
                    }
                }

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }

    private void Binductittle()
    {
        if ((ViewState["MOCSECTIONID"]).ToString() == "10")
        {
            MenuApprovalRemarks.Title = "Support Required (By Office/ Superintendent, assistance by external parties such as workshop/technician etc) ";
        }
        if ((ViewState["MOCSECTIONID"]).ToString() == "11")
        {
            MenuApprovalRemarks.Title = "Identify any external approvals required (Regulatory Authorities/Classification Society/Owner)";
        }
        if ((ViewState["MOCSECTIONID"]).ToString() == "12")
        {
            MenuApprovalRemarks.Title = "Identify any Equipment, Stores and Spares, Man Power, Document, Manuals, Drawings/ Plans, Phoenix Modules, etc";
        }
        if ((ViewState["MOCSECTIONID"]).ToString() == "13")
        {
            MenuApprovalRemarks.Title = "Shipboard Personnel affected by change";
        }
        if ((ViewState["MOCSECTIONID"]).ToString() == "14")
        {
            MenuApprovalRemarks.Title = "Shore Based Personnel affected by change";
        }
    }
}
