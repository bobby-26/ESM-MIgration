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

public partial class InspectionSealRequisitionCancel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuCancelReason.AccessRights = this.ViewState;
            MenuCancelReason.MenuList = toolbarmain.Show();            
            if (Request.QueryString["REQUESTID"] != null && Request.QueryString["REQUESTID"].ToString() != string.Empty)
            {
                ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();
            }
            BindData();
        }
    }

    protected void BindData()
    {
        DataTable dt = PhoenixInspectionSealRequisition.EditSealRequesition(new Guid(ViewState["REQUESTID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            txtCancelReason.Text = dt.Rows[0]["FLDCANCELREASON"].ToString();
            txtName.Text = dt.Rows[0]["FLDCANCELLEDBYNAME"].ToString();
            txtDesignation.Text = dt.Rows[0]["FLDCANCELLEDBYDESIGNATION"].ToString();
            ucCancelDate.Text = dt.Rows[0]["FLDCANCELDATE"].ToString();
        }
    }

    protected void MenuCancelReason_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (General.GetNullableString(txtCancelReason.Text) == null)
            {
                lblMessage.Text = "Cancel Reason is required.";
                return;
            }

            if (ViewState["REQUESTID"] != null && ViewState["REQUESTID"] != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixInspectionSealRequisition.CancelSealRequisition(new Guid(ViewState["REQUESTID"].ToString())
                                                    , General.GetNullableString(txtCancelReason.Text));
                    BindData();

                    //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    //Script += "fnReloadList('CancelReq','ifMoreInfo');";
                    //Script += "</script>" + "\n";
                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('CancelReq');", true);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }  
}
