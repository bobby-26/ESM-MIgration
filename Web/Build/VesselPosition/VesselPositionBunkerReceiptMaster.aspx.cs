using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;

public partial class VesselPositionBunkerReceiptMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("General", "GENERAL");
                //toolbarmain.AddButton("Attachment", "ATTACHMENT");

                MenuOrderFormMain.AccessRights = this.ViewState;
                MenuOrderFormMain.MenuList = toolbarmain.Show();
                MenuOrderFormMain.SetTrigger(pnlOrderForm);

                ViewState["PAGEURL"] = null;
                

                if (Request.QueryString["consumptionid"] != null)
                    ViewState["CONSUMPTIONID"] = Request.QueryString["consumptionid"].ToString();
                else
                    ViewState["CONSUMPTIONID"] = "";

                if (Request.QueryString["oiltype"] != null)
                    ViewState["OILTYPE"] = Request.QueryString["oiltype"].ToString();
                else
                    ViewState["OILTYPE"] = "";
                Filter.CurrentBunkerReceiptSelection = null;
                ifMoreInfo.Attributes["src"] = "../VesselPosition/VesselPositionBunkerReceipt.aspx?consumptionid=" + ViewState["CONSUMPTIONID"] + "&oiltype=" + ViewState["OILTYPE"];
                
                MenuOrderFormMain.SelectedMenuIndex = 0;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("GENERAL"))
            {
                ifMoreInfo.Attributes["src"] = "../VesselPosition/VesselPositionBunkerReceipt.aspx?consumptionid=" + ViewState["CONSUMPTIONID"] + "&oiltype=" + ViewState["OILTYPE"];
            }
            if (dce.CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                if (Filter.CurrentBunkerReceiptSelection != null)
                {
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?dtkey=" + Filter.CurrentBunkerReceiptSelection + "&mod=" + PhoenixModule.VESSELPOSITION;
                }
            }
            else
                MenuOrderFormMain.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        
    }


    //private void BindPageURL(int rowindex)
    //{
    //    try
    //    {
    //        ViewState["projectbillingid"] = ((Label)gvFormDetails.Rows[rowindex].FindControl("lblProjectBillingid")).Text;

    //        ifMoreInfo.Attributes["src"] = "../Accounts/AccountsProjectBillingItemGeneral.aspx?projectbillingid=" + ViewState["projectbillingid"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}


}
