using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;
public partial class PurchaseFormCopyStoreRequisition : PhoenixBasePage
{ 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (Request.QueryString["FORMNO"] != null)
                MenuComment.Title = "Copy ( " + Request.QueryString["FORMNO"].ToString() + " )";
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
                toolbarmain.AddButton("Copy", "COPY",ToolBarDirection.Right);
                
                MenuComment.AccessRights = this.ViewState;
                MenuComment.MenuList = toolbarmain.Show();
            
            if (!IsPostBack)
            {
                if (Request.QueryString["VESSELID"] != null)
                {
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"];
                    ucVesselFrom.SelectedVessel = ViewState["VESSELID"].ToString();
                 }
                if (Request.QueryString["ORDERID"] != null)
                {
                    ViewState["ORDERID"] = Request.QueryString["ORDERID"];
                }
                if (Request.QueryString["STOCKTYPE"] != null)
                {
                    ViewState["STOCKTYPE"] = Request.QueryString["STOCKTYPE"];
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   

    protected void MenuComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("COPY"))
            {
                if (string.IsNullOrEmpty(ucVesselTo.SelectedVessel) || ucVesselTo.SelectedVessel.ToString().ToUpper() == "DUMMY")
                {
                    ucError.ErrorMessage = "To Vessel is required.";
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ORDERID"] != null && ViewState["VESSELID"] != null && ViewState["STOCKTYPE"]!=null)
                {
                    if (ViewState["STOCKTYPE"].ToString() == "STORE")
                    {
                        PhoenixPurchaseOrderForm.CopyStoreRequisition(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["ORDERID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        int.Parse(ucVesselTo.SelectedVessel)
                        );
                    }
                    else if (ViewState["STOCKTYPE"].ToString() == "SPARE")
                    {
                        PhoenixPurchaseOrderForm.CopySpareRequisition(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["ORDERID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        int.Parse(ucVesselTo.SelectedVessel)
                        );
                    }else if (ViewState["STOCKTYPE"].ToString() == "SERVICE")
                    {
                        PhoenixPurchaseOrderForm.CopyServiceRequisition(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["ORDERID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        int.Parse(ucVesselTo.SelectedVessel)
                        );
                    }
                    ucStatus.Text = "Requisition Copied Successfully";
                }              
            }           

                //string Script = "";
                //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                //Script += "parent.CloseCodeHelpWindow('MoreInfo');";
                //Script += "</script>" + "\n";
                //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, true);

                String script = String.Format("javascript:fnReloadList();");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
