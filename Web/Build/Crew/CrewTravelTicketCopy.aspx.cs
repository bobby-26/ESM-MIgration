using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CrewTravelTicketCopy : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //SessionUtil.PageAccessRights(this.ViewState);
            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            if (!IsPostBack)
            {
                ViewState["VIEWONLY"] = null;
                if (Request.QueryString["VIEWONLY"] != null && Request.QueryString["VIEWONLY"] != "")
                {
                    ViewState["VIEWONLY"] = Request.QueryString["VIEWONLY"].ToString();
                }                
                BindField();
            }
            if (Request.QueryString["VIEWONLY"] == "false")
            {
                note.Visible = true;
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Save", "SUBMIT", ToolBarDirection.Right);
                toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
                
                //MenuComment.AccessRights = this.ViewState;                        
                MenuComment.MenuList = toolbarmain.Show();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindField()
    {
        if ((Request.QueryString["ROUTEID"] != null) && (Request.QueryString["ROUTEID"] != ""))
        {
            DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelQuotationTicketcopySearch(General.GetNullableGuid((Request.QueryString["ROUTEID"])),null,1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvTicketList.DataSource = ds;
                //repTicketList.DataBind();
            }
        }
    }

    protected void MenuComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("NEW"))
            {
                txtComment.Visible = true;

            }
            if (CommandName.ToUpper().Equals("SUBMIT"))
            {
                if (General.GetNullableString(txtComment.Text.Trim(new Char[]{'\r','\n'}).Trim()) == null)
                {
                 
                    ucError.ErrorMessage = "Please attach valid ticket";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixCrewTravelQuoteLine.UpdateQuotationTicket(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Request.QueryString["ROUTEID"].ToString())
                                                                        , General.GetNullableString(txtComment.Text));

                    string Script = "";
                    Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList();";
                    Script += "</script>" + "\n";
                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
                }
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTicketList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindField();
    }
}
