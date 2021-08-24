using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Text;

public partial class PurchaseFalApprovalLevel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           

            ViewState["OrderId"] = Request.QueryString["orderid"].ToString();
            ViewState["QuotationId"] = Request.QueryString["quotationid"].ToString();
            ViewState["Vesselid"] = Request.QueryString["Vesselid"].ToString();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                gvPurchaseFalApprove.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPurchaseFalApprove_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPurchaseFalApprove.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataTable ds = PhoenixPurchaseFalApprovalLevel.PurchaseFalApprovalLevelSearch(
                 PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                  General.GetNullableInteger(ViewState["Vesselid"].ToString()),
                   General.GetNullableGuid(ViewState["QuotationId"].ToString()),
                 General.GetNullableGuid(ViewState["OrderId"].ToString()),
                  gvPurchaseFalApprove.CurrentPageIndex + 1,
                  gvPurchaseFalApprove.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

            gvPurchaseFalApprove.DataSource = ds;
            gvPurchaseFalApprove.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvPurchaseFalApprove_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridDataItem item = e.Item as GridDataItem;
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                int Rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? Quotationid = General.GetNullableGuid(ViewState["QuotationId"].ToString());
                Guid? Orderid = General.GetNullableGuid(ViewState["OrderId"].ToString());
                int? Vesselid = General.GetNullableInteger(ViewState["Vesselid"].ToString());
                Guid? Quotationapprovalid = General.GetNullableGuid(item.OwnerTableView.DataKeyValues[item.ItemIndex]["FLDQUOTATIONLVLAPPROVALID"].ToString());
                PhoenixCommonPurchase.UpdateQuotationFALApproval(Rowusercode, Quotationid, Quotationapprovalid, Orderid, Vesselid);
                ucStatus.Text = "Approved";
                ucStatus.Visible = true;
                if (Session["POQAPPROVE"] != null && ((DataSet)Session["POQAPPROVE"]).Tables.Count > 0)
                {
                    DataSet ds = (DataSet)Session["POQAPPROVE"];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string emailbodytext = PrepareApprovalText(ds.Tables[0], 0);
                        DataRow dr = ds.Tables[0].Rows[0];
                        PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                            dr["FLDFROMEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                            null,
                            dr["FLDSUBJECT"].ToString() + "     " + dr["FLDFORMNO"].ToString(),
                            emailbodytext,
                            true,
                            System.Net.Mail.MailPriority.Normal,
                            "",
                            null,
                            null);
                    }
                    Session["POQAPPROVE"] = null;
                }

                RadScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "CloseWindow('Filter');fnReloadList('approval','filterandsearch',null);", true);
               
            }


        }
        catch (Exception ex)
        {
            
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
           
        }
     }

    protected string PrepareApprovalText(DataTable dt, int approved)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataRow dr = dt.Rows[0];
        sbemailbody.Append("<pre>");
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Purchaser");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        if (approved == 1)
            sbemailbody.AppendLine("Purchase approval is cancelled.");
        else
            sbemailbody.AppendLine("Purchase order is approved");

        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.Append(dr["FLDAPPROVEDBY"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("For and on behalf of Executive Ship Management Pte Ltd.");
        sbemailbody.AppendLine();

        return sbemailbody.ToString();

    }


    protected void gvPurchaseFalApprove_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = (DataRowView)item.DataItem;
                int? isapproved = General.GetNullableInteger(drv["FLDISAPPROVED"].ToString());
                ImageButton Approve = (ImageButton)e.Item.FindControl("cmdApprove");
                ImageButton Approvallimit = (ImageButton)e.Item.FindControl("btnapprovallimit");
                ImageButton Reasons = (ImageButton)e.Item.FindControl("btnreasons");

                ImageButton Rollback = (ImageButton)e.Item.FindControl("cmdRollback");
                Rollback.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','Purchase/PurchaseFalApprovalRollback.aspx?approvalid=" + drv["FLDQUOTATIONLVLAPPROVALID"] + " ','false','400px','220px');return false");
                Approvallimit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','Purchase/PurchaseQuotationFAL.aspx?approvalid=" + drv["FLDQUOTATIONLVLAPPROVALID"] + " ','false','800px','320px');return false");
                Reasons.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','Purchase/PurchaseQuotationApprovalRollbackReasons.aspx?approvalid=" + drv["FLDQUOTATIONLVLAPPROVALID"] + " ','false','800px','320px');return false");

                RadLabel approvedtext = (RadLabel)e.Item.FindControl("radlblapprove");
                if (isapproved == 0)
                {
                    Approve.Visible = true;
                    Rollback.Visible = true;
                    Reasons.Visible = true;
                    Approvallimit.Visible = true;
                }
                else
                {
                    string username = (drv["FLDUSERNAME"].ToString());
                    approvedtext.Text = "Approved by " + username;
                    approvedtext.Visible = true;
                }
                LinkButton imgFlag = (LinkButton)e.Item.FindControl("imgFlag");
                if (imgFlag != null)
                {
                    if (drv["FLDISADDITIONALLEVEL"].ToString() == "1")
                    {
                        imgFlag.Visible = true;
                        DataSet ds = PhoenixPurchaseQuotation.QuotationsValidate(General.GetNullableGuid(drv["FLDORDERID"].ToString()), General.GetNullableGuid(drv["FLDQUOTATIONID"].ToString()));
                        DataRow dr = ds.Tables[0].Rows[0];
                        if (dr["FLDRFQLESSTHAN3VENDOR"].ToString() == "1" || dr["FLDSELECTEDHIGHERQUOTE"].ToString() == "1" || dr["FLDOEMREASON"].ToString() == "0")
                        {

                            imgFlag.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Purchase/PurchaseQuotationReason.aspx?orderid=" + drv["FLDORDERID"].ToString() + "&quoationid=" + drv["FLDQUOTATIONID"].ToString() + "&minvendor=" + dr["FLDRFQLESSTHAN3VENDOR"].ToString() + "&higquote=" + dr["FLDSELECTEDHIGHERQUOTE"].ToString() + "&configquote=" + dr["FLDCONFIGUREDQUOTES"].ToString() + "&OEM=" + dr["FLDOEMREASON"].ToString() + "&launch=APPROVAL" + "');");

                        }
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvPurchaseFalApprove.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}