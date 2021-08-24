﻿using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionCancelReasonUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuCancelReason.AccessRights = this.ViewState;
        MenuCancelReason.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {           

            if (Request.QueryString["REFERENCEID"] != null && Request.QueryString["REFERENCEID"].ToString() != string.Empty)
            {
                ViewState["REFERENCEID"] = Request.QueryString["REFERENCEID"].ToString();
            }

            if (Request.QueryString["TYPE"] != null && Request.QueryString["TYPE"].ToString() != string.Empty)
            {
                ViewState["TYPE"] = Request.QueryString["TYPE"].ToString();
            }

            ViewState["DASHBOARDYN"] = "";
            if (Request.QueryString["DASHBOARDYN"] != null && Request.QueryString["DASHBOARDYN"].ToString() != string.Empty)
            {
                ViewState["DASHBOARDYN"] = Request.QueryString["DASHBOARDYN"].ToString();
            }
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


            if (ViewState["REFERENCEID"] != null && ViewState["REFERENCEID"] != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixInspectionIncident.CancelReasonUpdate(
                                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , int.Parse(ViewState["TYPE"].ToString())
                                                                   , new Guid(ViewState["REFERENCEID"].ToString())
                                                                   , txtCancelReason.Text);

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
}
