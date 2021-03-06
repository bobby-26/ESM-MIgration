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
public partial class Inspection_InspectionIncidentCloseComment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuCloseComment.AccessRights = this.ViewState;
        MenuCloseComment.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {            
            if (Request.QueryString["REFERENCEID"] != null && Request.QueryString["REFERENCEID"].ToString() != string.Empty)
                 ViewState["REFERENCEID"] = Request.QueryString["REFERENCEID"].ToString();
            if (Request.QueryString["user"] != null && Request.QueryString["user"].ToString() != string.Empty)
                 ViewState["user"] = Request.QueryString["user"].ToString();

            ViewState["DASHBOARDYN"] = "";
            if (Request.QueryString["DASHBOARDYN"] != null && Request.QueryString["DASHBOARDYN"].ToString() != string.Empty)
            {
                ViewState["DASHBOARDYN"] = Request.QueryString["DASHBOARDYN"].ToString();
            }

            BindCloseComments();
        }
    }

    protected void MenuCloseComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (General.GetNullableString(txtcloseComments.Text) == null)
            {
                lblMessage.Text = "Close Comments is required.";
                return;
            }

            

            if (ViewState["REFERENCEID"] != null && ViewState["REFERENCEID"] != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixInspectionIncident.IncidentCloseCommentsUpdate(new Guid(ViewState["REFERENCEID"].ToString())
                                                                                , txtcloseComments.Text, ViewState["user"].ToString());
                    BindCloseComments();

                    if (ViewState["DASHBOARDYN"].ToString().Equals("1"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('NAFA', 'wo');", true);
                    }
                    else
                    {
                        string Script = "";
                        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                        Script += "fnReloadList('CI','ifMoreInfo');";
                        Script += "</script>" + "\n";
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
    private void BindCloseComments()
    {
        DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(ViewState["REFERENCEID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtcloseComments.Text = dr["FLDCLOSEREMARK"].ToString();
            txtCloseCommentedByName.Text = dr["FLDCLOSEDBYNAME"].ToString();
            ucCloseCommentDate.Text = dr["FLDCLOSEDDATE"].ToString();
        }
    }
}
