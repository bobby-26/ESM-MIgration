using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class InspectionVesselRAJHAApprovalExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Approve", "APPROVE");
        toolbarmain.AddButton("Disapprove", "DISAPPROVE");
        MenuApprovalRemarks.AccessRights = this.ViewState;
        MenuApprovalRemarks.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {            
            if (Request.QueryString["RATEMPLATEID"] != null && Request.QueryString["RATEMPLATEID"].ToString() != string.Empty)
            {
                ViewState["RATEMPLATEID"] = Request.QueryString["RATEMPLATEID"].ToString();
            }

            if (Request.QueryString["TYPE"] != null && Request.QueryString["TYPE"].ToString() != string.Empty)
            {
                ViewState["TYPE"] = Request.QueryString["TYPE"].ToString();
            }
        }
    }

    protected void MenuApprovalRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;


            if (General.GetNullableString(txtApprovalRemarks.Text) == null)
            {
                lblMessage.Text = "Office Remarks is required.";
                return;
            }

            string Script = "";

            if (ViewState["RATEMPLATEID"] != null && ViewState["RATEMPLATEID"] != null)
            {
                if (dce.CommandName.ToUpper().Equals("APPROVE"))
                {
                    PhoenixInspectionRiskAssessment.ApproveRAWithOfficeRemarksExtn(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["RATEMPLATEID"].ToString()), int.Parse(ViewState["TYPE"].ToString()), txtApprovalRemarks.Text);

                    lblMessage.Text = "JHA Approved";

                }
                else if (dce.CommandName.ToUpper().Equals("DISAPPROVE"))
                {
                    PhoenixInspectionRiskAssessment.DisapproveRAWithOfficeRemarksExtn(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["RATEMPLATEID"].ToString()), int.Parse(ViewState["TYPE"].ToString()), txtApprovalRemarks.Text);

                    lblMessage.Text = "JHA Disapproved";
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
}
