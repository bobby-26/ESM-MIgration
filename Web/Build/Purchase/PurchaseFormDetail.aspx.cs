using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

public partial class PurchaseFormDetail : PhoenixBasePage
{
  

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            MenuFormDetail.Title = "Details     (  " + PhoenixPurchaseOrderForm.FormNumber + "     )";
            if (Request.QueryString["launchedfrom"] == null)
            {
                toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuFormDetail.AccessRights = this.ViewState;
                MenuFormDetail.MenuList = toolbarmain.Show();
                
            }
            if (!IsPostBack)
            {

                
                if ((Request.QueryString["orderid"] != null) && (Request.QueryString["orderid"] != ""))
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    ViewState["dtkey"] = string.Empty;
                    BindData(Request.QueryString["orderid"].ToString());
                }
                if (Request.QueryString["launchedfrom"] == null)
                {
                    if (!Filter.CurrentPurchaseVesselSendDateSelection.ToUpper().Equals("") && !(Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null))
                    {
                        MenuFormDetail.Visible = false;
                    }
                }

            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }

    private void BindData(string orderid)
    {

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseOrderForm.EditOrderForm(new Guid(orderid));

        if (ds.Tables[0].Rows.Count > 0)
        {           
            DataRow dr = ds.Tables[0].Rows[0];
            txtFormDetails.Content = HttpUtility.HtmlDecode(dr["FLDNOTES"].ToString());
            ViewState["dtkey"] = dr["FLDDTKEY"].ToString();
        }        
    }
    protected void MenuFormDetail_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if ((Request.QueryString["orderid"] != null) && (Request.QueryString["orderid"] != ""))
                {
                    PhoenixPurchaseOrderForm.UpdateOrderFormNotes(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()), txtFormDetails.Content);
                    ucStatus.Text = "Data has been Saved";
                    ucStatus.Visible = true; 
                }
              //  Response.Redirect("../Purchase/PurchaseFormGeneral.aspx?orderid=" + ViewState["orderid"].ToString());
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
 
    }
    //protected void btnInsertPic_Click(object sender, EventArgs e)
    //{
    //    try
    //    {            
    //        if (Request.Files.Count > 0 && txtFormDetails.ActiveMode == AjaxControlToolkit.HTMLEditor.ActiveModeType.Design)
    //        {
    //            Guid gFileName = PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.PURCHASE, null, ".jpg,.png,.gif");
    //            DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["dtkey"].ToString()));
    //            DataRow[] dr = dt.Select("FLDDTKEY = '" + gFileName.ToString() + "'");
    //            if (dr.Length > 0)
    //               txtFormDetails.Content = txtFormDetails.Content + "<img src=\"" + HttpContext.Current.Session["sitepath"] + "/attachments/" + dr[0]["FLDFILEPATH"].ToString() +"\" />";
    //        }
    //        else
    //        {
    //            ucError.Text = Request.Files.Count > 0 ? "You are not in design mode" : "No Picture selected.";
    //            ucError.Visible = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }       
    //}
}
