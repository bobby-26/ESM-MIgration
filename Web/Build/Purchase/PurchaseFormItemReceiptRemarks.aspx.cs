﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseFormItemReceiptRemarks : PhoenixBasePage
{
  

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
     
            if (Request.QueryString["View"]== null)
            {
                toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuLineItemDetail.AccessRights = this.ViewState;   
                MenuLineItemDetail.MenuList = toolbarmain.Show();
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["Orderlineid"] != null)
                {
                    ViewState["Orderlineid"] = Request.QueryString["Orderlineid"].ToString();
                    BindDataEdit(ViewState["Orderlineid"].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void BindDataEdit(string Orderlineid)
    {
         DataSet dsnotes = new DataSet();
         dsnotes = PhoenixPurchaseOrderLine.EditOrderLine(new Guid(Orderlineid));
        if (dsnotes.Tables[0].Rows.Count > 0)
        {
            txtItemDetails.Content = dsnotes.Tables[0].Rows[0]["FLDRECEIPTREMARKS"].ToString();
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
                if (ViewState["Orderlineid"] != null)
                    PhoenixPurchaseOrderLine.UpdateOrderLineReceiptRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["Orderlineid"].ToString()), txtItemDetails.Content);
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "parent.closeMoreInformation();";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterStartupScript(Page,typeof(Page), "BookMarkScript", Script,false);
                 
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    // protected void btnInsertPic_Click(object sender, EventArgs e)
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
   
