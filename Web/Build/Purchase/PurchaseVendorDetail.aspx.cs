using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

public partial class PurchaseVendorDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() == "VENDOR")
            {
                if (PhoenixSecurityContext.CurrentSecurityContext == null)
                    PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;
            }
               toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
               MenuFormDetail.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                if ((Request.QueryString["quotationid"] != null) && (Request.QueryString["quotationid"] != ""))
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                    ViewState["dtkey"] = string.Empty;
                    BindData(Request.QueryString["quotationid"].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData(string quotationid)
    {

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseQuotation.EditQuotation(new Guid(quotationid));
        if (ds.Tables[0].Rows.Count > 0)
        {           
            DataRow dr = ds.Tables[0].Rows[0];
            txtFormDetails.Content = HttpUtility.HtmlDecode(dr["FLDREMARKS"].ToString());            
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
                if ((Request.QueryString["quotationid"] != null) && (Request.QueryString["quotationid"] != ""))
                {
                    PhoenixPurchaseQuotation.UpdateQuotationComments(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationid"].ToString()), txtFormDetails.Content);
                    ucStatus.Text = "Data has been Saved";
                    ucStatus.Visible = true; 
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
