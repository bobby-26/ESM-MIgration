using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseMultipleQuotation : PhoenixBasePage
{
    string vesselname;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["pageno"] != null)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["pageno"].ToString());
                else
                    ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (Request.QueryString["vendorid"] != null && Request.QueryString["vesselid"] != null)
                {
                    ViewState["vendorid"] = Request.QueryString["vendorid"].ToString();
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
                }
                gvQuotationFormDetails.PageSize = General.ShowRecords(null);
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseMultipleQuotation.aspx", "Send Query", "<i class=\"fas fa-envelope\"></i>", "RFQ");
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseMultipleQuotation.aspx", "Delivery Instruction", "<i class=\"fas fa-plane-departure\"></i>", "DELIVERYINSTRUCTION");
            MenuVendorList.AccessRights = this.ViewState;
            MenuVendorList.MenuList = toolbargrid.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuVendorList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("RFQ"))
            {
                string orderid = "";
                string quotationid = "";
                string flag;
                flag = "F";

                foreach (GridDataItem item in gvQuotationFormDetails.Items)
                {
                    
                    orderid = item.GetDataKeyValue("FLDORDERID").ToString();
                    quotationid = item.GetDataKeyValue("FLDQUOTATIONID").ToString();
                    ViewState["orderid"] = item.GetDataKeyValue("FLDORDERID").ToString();

                    PhoenixPurchaseOrderForm.FormNumber = ((LinkButton)item.FindControl("lnkFormNumberName")).Text;
                    Filter.CurrentPurchaseVesselSelection = int.Parse(item.GetDataKeyValue("FLDVESSELID").ToString());
                    Filter.CurrentPurchaseStockType = ((RadLabel)item.FindControl("lblStockType")).Text;
                    Filter.CurrentPurchaseStockClass = ((RadLabel)item.FindControl("lblStockId")).Text;
                    SendForQuotation(orderid, quotationid);
                    flag = "T";
                    
                }
                if (flag == "T")
                {
                    ucConfirm.ErrorMessage = "RFQ sent for the selected Requisitions. \n";
                    ucConfirm.Visible = true;
                }
                else
                {
                    ucConfirm.ErrorMessage = "Please Add Requisitions to send mail";
                    ucConfirm.Visible = true;
                }
            }
            if (CommandName.ToUpper().Equals("DELIVERYINSTRUCTION"))
            {
                string quotationid = "";
                string flag;
                flag = "F";

                foreach (GridDataItem item in gvQuotationFormDetails.Items)
                {
                    quotationid = quotationid + "," + item.GetDataKeyValue("FLDQUOTATIONID").ToString();
                    flag = "T";
                }
                if (flag == "T")
                {
                    String scriptpopup = String.Format(
                            "javascript:parent.openNewWindow('codehelp1', '', 'Purchase/PurchaseQuotationDeliveryInstructionBulk.aspx?quotationid=" + quotationid +
                            "');");
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                else
                {
                    ucConfirm.ErrorMessage = "Please Add Requisitions to update Delivery Instructions";
                    ucConfirm.Visible = true;
                }
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixCommonPurchase.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    private void SendForQuotation(string orderid, string quotationid)
    {
        string emailid;
        try
        {
            string selectedvendors = ",";

            try
            {
                DataTable dt = null;

                dt = PhoenixPurchaseQuotation.QuotationXml(General.GetNullableGuid(quotationid));

                if (dt.Rows.Count > 0)
                {

                    string orderformxml = dt.Rows[0]["FLDORDERFORMXML"].ToString();
                    string orderlinexml = dt.Rows[0]["FLDORDERLINEXML"].ToString();
                    string addressxml = dt.Rows[0]["FLDADDRESSXML"].ToString();

                    PhoenixServiceWrapper.SubmitPurchaseQuery(orderformxml, orderlinexml, addressxml);
                }
            }
            catch { }

            selectedvendors = selectedvendors + quotationid + ",";

            if (selectedvendors.Length <= 1)
                selectedvendors = null;
            DataSet dsvendor = PhoenixPurchaseQuotation.ListQuotationToSend(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(orderid), selectedvendors, Int64.Parse(DateTime.Now.ToString("yyyyMMddhhmm")));
            if (dsvendor.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in dsvendor.Tables[0].Rows)
                {
                    string emailbodytext = "";
                    string supemail = "";
                    if (dr["FLDEMAIL1"].ToString().Contains(";"))
                        emailid = dr["FLDEMAIL1"].ToString().Replace(";", ",");
                    else
                        emailid = dr["FLDEMAIL1"].ToString();

                    if (!dr["FLDEMAIL2"].ToString().Equals(""))
                    {
                        emailid = emailid + "," + dr["FLDEMAIL2"].ToString().Replace(";", ",");
                    }
                    /* 21675 */
                    //if (dr["FLDSUPTEMAIL"].ToString().Equals("") || dr["FLDSUPPLIEREMAIL"].ToString().Equals(""))
                    //    supemail = dr["FLDSUPTEMAIL"].ToString() + dr["FLDSUPPLIEREMAIL"].ToString();
                    //else
                    //    supemail = dr["FLDSUPTEMAIL"].ToString() + "," + dr["FLDSUPPLIEREMAIL"].ToString();

                    supemail = dr["FLDSUPPLIEREMAIL"].ToString();

                    DataSet dscontact;
                    dscontact = PhoenixPurchaseQuotation.QuotationContactsGetEmail(General.GetNullableInteger(dr["FLDVENDORID"].ToString()), Filter.CurrentPurchaseStockType.ToString(), Filter.CurrentPurchaseVesselSelection);
                    if (!dscontact.Tables[0].Rows[0]["FLDEMALTO"].ToString().Trim().Equals(""))
                    {
                        emailid = emailid + dscontact.Tables[0].Rows[0]["FLDEMALTO"].ToString().Trim();
                    }
                    if (!dscontact.Tables[0].Rows[0]["FLDEMALCC"].ToString().Trim().Equals(""))
                    {
                        supemail = supemail + dscontact.Tables[0].Rows[0]["FLDEMALCC"].ToString().Trim();
                    }

                    try
                    {
                        if (dr["FLDRFQPREFERENCE"].ToString().Equals("WEB"))
                        {
                            PhoenixPurchaseQuotation.InsertWebSession(new Guid(dr["FLDQUOTATIONID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, "RFQ", PhoenixPurchaseOrderForm.FormNumber);
                            emailbodytext = PrepareEmailBodyText(new Guid(dr["FLDQUOTATIONID"].ToString()), dr["FLDFORMNO"].ToString(), dr["FLDFROMEMAIL"].ToString());
                            PhoenixCommoneProcessing.PrepareEmailMessage(
                                emailid, "RFQ", new Guid(dr["FLDQUOTATIONID"].ToString()),
                                "", supemail.Equals("") ? dr["FLDFROMEMAIL"].ToString() : supemail + "," + dr["FLDFROMEMAIL"].ToString(),
                                dr["FLDNAME"].ToString(), dr["FLDVESSELNAME"].ToString() + " - RFQ for " + dr["FLDFORMNO"].ToString() + "" + (dr["FLDTITLE"].ToString() == "" ? "" : "-") + dr["FLDTITLE"].ToString() + "" + (dr["FLDSEAPORTNAME"].ToString() == "" ? "" : "-") + dr["FLDSEAPORTNAME"].ToString(), emailbodytext, "", "");
                            PhoenixPurchaseQuotation.UpdateQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()), new Guid(dr["FLDQUOTATIONID"].ToString()));

                        }
                        else
                        {
                            PhoenixPurchaseQuotation.QuotationRFQExcelInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(dr["FLDORDERID"].ToString()), new Guid(dr["FLDQUOTATIONID"].ToString()), int.Parse(dr["FLDVENDORID"].ToString()));
                        }
                    }
                    catch (Exception ex)
                    {
                        ucConfirm.ErrorMessage = ex.Message + " for  " + dr["FLDNAME"].ToString() + "\n";
                    }
                }
                InsertOrderFormHistory();  
            }
            else
            {
                ucConfirm.ErrorMessage = "Email already sent";
                ucConfirm.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertOrderFormHistory()
    {
        PhoenixPurchaseOrderForm.InsertOrderFormHistory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
    }

    protected string PrepareEmailBodyText(Guid quotationid, string orderformnumber, string frommailid)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixPurchaseQuotation.GetQuotationDataForEmailBody(PhoenixSecurityContext.CurrentSecurityContext.UserCode, quotationid, "RFQ");
        DataRow dr = ds.Tables[0].Rows[0];

        sbemailbody.Append(dr["FLDVENDORNAME"].ToString() + "             " + orderformnumber);
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Sir");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Executive Ship Management Pte Ltd hereby requests you to provide your BEST quotation for the following items to be delivered to our vessel");
        sbemailbody.AppendLine(dr["FLDVESSELNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.Append("Request your IT department to kindly allow access to this URL for submitting quotes.");
        sbemailbody.AppendLine();
        sbemailbody.Append("Please click on the link below and key in the relevant fields indicated. If the link is wrapped, please copy and paste it on the address bar of your browser");
        sbemailbody.AppendLine();
        string url =  "\"<" + Session["sitepath"] + "/" + dr["URL"].ToString();
        if (Filter.CurrentPurchaseStockType == null || Filter.CurrentPurchaseStockType.Equals(string.Empty))
            sbemailbody.AppendLine(url + "?SESSIONID=" + dr["SESSIONID"].ToString() + ">");
        else
            sbemailbody.AppendLine(url + "?SESSIONID=" + dr["SESSIONID"].ToString() + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType + ">\"");

        sbemailbody.AppendLine();
        sbemailbody.AppendLine();

        if (dr["FLDEXPIRYDATE"].ToString() != "")
        {
            sbemailbody.AppendLine("We request you to submit your bid by");
            sbemailbody.Append(dr["FLDEXPIRYDATE"].ToString());
            sbemailbody.Append(", failing which your offer will NOT be accepted. If you wish to decline to bid, please advise us by email with your reasons for declining.");
        }
        else
        {
            sbemailbody.AppendLine("We request you to submit your bid, failing which your offer will NOT be accepted. If you wish to decline to bid, please advise us by email with your reasons for declining.");
        }

        sbemailbody.AppendLine();
        sbemailbody.Append("Note: In our continual effort to keep correct records of your address and contact information, we appreciate your time to verify and correct it where necessary. Please click on the link below to view/correct the address.");
        sbemailbody.AppendLine();

        DataSet dsaddress = PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ViewState["vendorid"].ToString()));
        DataRow draddress = dsaddress.Tables[0].Rows[0];

        sbemailbody.AppendLine("\n" + "\"<" + Session["sitepath"] + "/Purchase/PurchaseVendorAddressEdit.aspx?sessionid=" + draddress["FLDDTKEY"].ToString() + ">\"");

        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.Append(dr["FLDUSERNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("For and on behalf of " + dr["FLDCOMPANYNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("(As Agents for Owners)");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Contact: " + frommailid);
        sbemailbody.AppendLine();

        return sbemailbody.ToString();

    }


    protected void gvQuotationFormDetails_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridDataItem item = (GridDataItem)e.Item;
            Filter.CurrentPurchaseStockType = ((RadLabel)item.FindControl("lblStockType")).Text;
            string QuotationId = item.GetDataKeyValue("FLDQUOTATIONID").ToString();

            PhoenixPurchaseQuotation.DeleteQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(QuotationId));
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true; 
        }
        
    }

    protected void gvQuotationFormDetails_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        gvQuotationFormDetails.Rebind();
    }

    protected void gvQuotationFormDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        
        DataSet ds = PhoenixPurchaseOrderForm.QuotationSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ViewState["vesselid"].ToString()),
               int.Parse(ViewState["vendorid"].ToString()), null,
               sortexpression, sortdirection, gvQuotationFormDetails.CurrentPageIndex+1, gvQuotationFormDetails.PageSize,
                   ref iRowCount, ref iTotalPageCount);

        gvQuotationFormDetails.DataSource = ds;
        gvQuotationFormDetails.VirtualItemCount = iRowCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            vesselname = dr["FLDVESSELNAME"].ToString();

            if (ViewState["orderid"] == null)
            {
                ViewState["orderid"] = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
            }

        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvQuotationFormDetails_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void gvQuotationFormDetails_EditCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void gvQuotationFormDetails_ItemDataBound1(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton db = (LinkButton)item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        
    }

    protected void gvQuotationFormDetails_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                Filter.CurrentPurchaseStockType = ((RadLabel)item.FindControl("lblStockType")).Text;
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
            

                
    }

    protected void gvQuotationFormDetails_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
}
