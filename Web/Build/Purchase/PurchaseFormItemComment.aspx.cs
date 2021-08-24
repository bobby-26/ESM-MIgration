using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;


public partial class PurchaseFormItemComment: PhoenixBasePage
{
  

    protected void Page_Load(object sender, EventArgs e)
    {
        //SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            
            if (!IsPostBack)
            {
                ViewState["NAME"] = "";
                if (Request.QueryString["orderlineid"] != null)
                {
                    ViewState["orderlineid"] = Request.QueryString["orderlineid"].ToString();
                    BindDataEdit(ViewState["orderlineid"].ToString());
                }
            }
            if (Request.QueryString["viewonly"] == null)
            {
                MenuLineItemDetail.Title = "Details ( " + ViewState["NAME"].ToString() + " )";
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuLineItemDetail.AccessRights = this.ViewState;
                MenuLineItemDetail.MenuList = toolbarmain.Show();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void BindDataEdit(string orderlineid)
    {
         DataSet dsnotes = new DataSet();
        dsnotes = PhoenixPurchaseOrderLine.EditOrderLine(new Guid(orderlineid));
        if (dsnotes.Tables[0].Rows.Count > 0)
        {
            
             txtItemDetails.Content = HttpUtility.HtmlDecode(dsnotes.Tables[0].Rows[0]["FLDNOTES"].ToString());
            ViewState["NAME"] = dsnotes.Tables[0].Rows[0]["FLDNAME"].ToString();
             ViewState["dtkey"] = dsnotes.Tables[0].Rows[0]["FLDDTKEY"].ToString(); 

        }
    }


    protected void MenuLineItemDetail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["orderlineid"] != null)
                {
                    PhoenixPurchaseOrderLine.UpdateOrderLineItemNotes(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderlineid"].ToString()),
                        txtItemDetails.Content.ToString());
                    ucStatus.Text = "Data has been Saved";
                    ucStatus.Visible = true;
                    String script = String.Format("javascript:parent.fnReloadList('code1');");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
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
    //        if (Request.Files.Count > 0 && txtItemDetails.ActiveMode == AjaxControlToolkit.HTMLEditor.ActiveModeType.Design)
    //        {
    //            Guid gFileName = PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.PURCHASE, null, ".jpg,.png,.gif");
    //            DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["dtkey"].ToString()));
    //            DataRow[] dr = dt.Select("FLDDTKEY = '" + gFileName.ToString() + "'");
    //            if (dr.Length > 0)
    //                txtItemDetails.Content = txtItemDetails.Content + "<img src=\"" + HttpContext.Current.Session["sitepath"] + "/attachments/" + dr[0]["FLDFILEPATH"].ToString() + "\" />";
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
