using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewHRTravelRequestList : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            if (!IsPostBack)
            {
                confirm.Attributes.Add("style", "display:none");
                cancel.Attributes.Add("style", "display:none");
                resendmail.Attributes.Add("style", "display:none");

                ViewState["TRAVELREQUESTID"] = "";
                ViewState["PERSONALINFOSN"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["travelrequestid"] != null && Request.QueryString["travelrequestid"].ToString() != "")
                    ViewState["TRAVELREQUESTID"] = Request.QueryString["travelrequestid"].ToString();
                else
                    ViewState["TRAVELREQUESTID"] = "";

                if (Request.QueryString["personalinfosn"] != null && Request.QueryString["personalinfosn"].ToString() != "")
                    ViewState["PERSONALINFOSN"] = Request.QueryString["personalinfosn"].ToString();
                else
                    ViewState["PERSONALINFOSN"] = "";
                gvOfficeTravelRequest.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewHRTravelRequestList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOfficeTravelRequest')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewHRTravelRequestListFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewHRTravelRequestList.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Crew/CrewHRTravelRequestGeneral.aspx", "New Travel Request", "<i class=\"fa fa-plus-circle\"></i>", "ADD");            
            MenuOfficeTravelRequest.AccessRights = this.ViewState;
            MenuOfficeTravelRequest.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void OfficeTravelRequest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentHRTravelRequestFilter = null;               
                BindData();
                gvOfficeTravelRequest.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDTRAVELREQUESTNO", "FLDVESSELNAME", "FLDDEPATURECITY", "FLDDESTINATIONCITY", "FLDTRAVELSTATUS", "FLDCREATEDDATE", "FLDISAPPROVED" };
        string[] alCaptions = { "Request No.", "Vessel", "Origin", "Destination", "Status", "Requested", "Approved Y/N" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentHRTravelRequestFilter;

        ds = PhoenixCrewHRTravelRequest.HRTravelRequestSearch(null
                 , General.GetNullableString(nvc != null ? nvc.Get("txtTravelRequestNo") : string.Empty)
                 , General.GetNullableDateTime(nvc != null ? nvc.Get("txtStartDate") : string.Empty)
                 , General.GetNullableDateTime(nvc != null ? nvc.Get("txtEndDate") : string.Empty)
                 , General.GetNullableString(nvc != null ? nvc.Get("uctravelstatus") : string.Empty)
                 , General.GetNullableInteger(nvc != null ? nvc["chkApprovedYN"] : string.Empty)
                 , null
                 , General.GetNullableInteger(nvc != null ? nvc.Get("txtOrigin") : string.Empty)
                 , General.GetNullableInteger(nvc != null ? nvc.Get("txtDestination") : string.Empty)
                 , sortexpression
                 , sortdirection
                 , (int)ViewState["PAGENUMBER"]
                 , gvOfficeTravelRequest.PageSize
                 , ref iRowCount
                 , ref iTotalPageCount);



        if (ds.Tables.Count > 0)
            General.ShowExcel("My Travel Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDTRAVELREQUESTNO", "FLDVESSELNAME", "FLDDEPATURECITY", "FLDDESTINATIONCITY", "FLDTRAVELSTATUS", "FLDCREATEDDATE", "FLDISAPPROVED" };
            string[] alCaptions = { "Request No.", "Vessel", "Origin", "Destination", "Status", "Requested", "Approved Y/N" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentHRTravelRequestFilter;


            DataSet ds = PhoenixCrewHRTravelRequest.HRTravelRequestSearch(null
                                                , General.GetNullableString(nvc != null ? nvc.Get("txtTravelRequestNo") : string.Empty)
                                                , General.GetNullableDateTime(nvc != null ? nvc.Get("txtStartDate") : string.Empty)
                                                , General.GetNullableDateTime(nvc != null ? nvc.Get("txtEndDate") : string.Empty)
                                                , General.GetNullableString(nvc != null ? nvc.Get("uctravelstatus") : string.Empty)
                                                , General.GetNullableInteger(nvc != null ? nvc["chkApprovedYN"] : string.Empty)
                                                , null
                                                , General.GetNullableInteger(nvc != null ? nvc.Get("txtOrigin") : string.Empty)
                                                , General.GetNullableInteger(nvc != null ? nvc.Get("txtDestination") : string.Empty)
                                                , sortexpression
                                                , sortdirection
                                                , (int)ViewState["PAGENUMBER"]
                                                , gvOfficeTravelRequest.PageSize
                                                , ref iRowCount
                                                , ref iTotalPageCount);

            General.SetPrintOptions("gvOfficeTravelRequest", "My Travel Request", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvOfficeTravelRequest.DataSource = ds;
                gvOfficeTravelRequest.VirtualItemCount = iRowCount;
                if (ViewState["TRAVELREQUESTID"] == null)
                {
                    ViewState["TRAVELREQUESTID"] = ds.Tables[0].Rows[0]["FLDTRAVELREQUESTID"].ToString();
                    ViewState["PERSONALINFOSN"] = ds.Tables[0].Rows[0]["FLDPERSONALINFOSN"].ToString();
                    PhoenixCrewTravelRequest.RequestNo = ds.Tables[0].Rows[0]["FLDTRAVELREQUESTNO"].ToString();
                   // gvOfficeTravelRequest.SelectedIndex = 0;
                }
            }
            else
            {
                gvOfficeTravelRequest.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void UcResendMail_Confirm(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewHRTravelRequest.HRTravelRequestResend(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["TRAVELREQUESTID"].ToString()));
            SendForApproval();
            
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Lock_Cancel(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewHRTravelRequest.HRTravelRequestCancel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["TRAVELREQUESTID"].ToString()));                
            BindData();
            gvOfficeTravelRequest.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Lock_Confirm(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewHRTravelRequest.HRTravelRequestConfirm(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["TRAVELREQUESTID"].ToString()));
            SendForApproval();            
            BindData();
            gvOfficeTravelRequest.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SendForApproval()
    {
        string emailid;
        try
        {
            int i = 0;
            DataTable dt = PhoenixCrewHRTravelRequest.HRTravelApprovaluserlist(new Guid(ViewState["TRAVELREQUESTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (i == 0)
                    {
                        string emailbodytext = "";
                        emailid = dr["APPROVALMAIL"].ToString();
                        try
                        {
                            {
                                emailbodytext = PrepareEmailBodyText(new Guid(dr["FLDDTKEY"].ToString()), dr["FLDAPPROVERSALUTATION"].ToString() + ' ' + dr["FLDAPPROVERNAME"].ToString(), dr["FLDREGARDSNAME"].ToString());
                                PhoenixCommoneProcessing.PrepareEmailMessage(emailid, "TRAVEL", new Guid(dr["FLDDTKEY"].ToString()), "", "", "", "MY TRAVEL REQUEST", emailbodytext, "", "");
                            }
                            ucStatus.Text = "Email sent to " + dr["FLDAPPROVERNAME"].ToString() + "\n";
                            //ucConfirm.ErrorMessage = "Email sent to " + dr["FLDAPPROVERNAME"].ToString() + "\n";
                        }
                        catch (Exception ex)
                        {

                            ucError.ErrorMessage = ex.Message + " for  " + dr["FLDAPPROVERNAME"].ToString() + "\n";
                            ucError.Visible = true;
                        }
                        i++;
                    }
                    else
                    {
                        break;
                    }
                    //ucConfirm.Visible = true;
                }
            }
            else
            {
                RadWindowManager1.RadConfirm("Email already sent", "confirm", 320, 150, null, "Confirm");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected string PrepareEmailBodyText(Guid quotationid, string approvername, string regards)
    {
        StringBuilder sbemailbody = new StringBuilder();
        try
        {
            sbemailbody.AppendLine();
            sbemailbody.Append("Dear " + approvername + ",");
            sbemailbody.AppendLine("\n Please find my below travel request:");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("Approve");
            sbemailbody.AppendLine("" + "\"<" + Session["sitepath"] + "/Crew/CrewHRMyTravelApproval.aspx?sessionid=" + quotationid.ToString() + "&App=1" + ">\"");
            sbemailbody.AppendLine("Reject");
            sbemailbody.AppendLine("" + "\"<" + Session["sitepath"] + "/Crew/CrewHRMyTravelApproval.aspx?sessionid=" + quotationid.ToString() + "&App=0" + ">\"");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine(" I request your confirmation / approval to go ahead with this travel.");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("Regards");
            sbemailbody.AppendLine(regards);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return sbemailbody.ToString();
    }
    
    private void SetRowSelection()
    {
        try
        {            
            for (int i = 0; i < gvOfficeTravelRequest.MasterTableView.Items.Count; i++)
            {
                if (gvOfficeTravelRequest.MasterTableView.Items[i].GetDataKeyValue("FLDTRAVELREQUESTID").ToString().Equals(ViewState["TRAVELREQUESTID"].ToString()))
                {
                    gvOfficeTravelRequest.MasterTableView.Items[i].Selected = true;
                }
            }
            //Title1.Text = "My Travel Request (" + PhoenixCrewTravelRequest.RequestNo + ")";
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["TRAVELREQUESTID"] = null;
            BindData();
            gvOfficeTravelRequest.Rebind();
            if (ViewState["TRAVELREQUESTID"] != null)
            {
                for (int i = 0; i < gvOfficeTravelRequest.MasterTableView.Items.Count; i++)
                {
                    if (gvOfficeTravelRequest.MasterTableView.Items[i].GetDataKeyValue("FLDTRAVELREQUESTID").ToString() == ViewState["TRAVELREQUESTID"].ToString())
                    {
                        gvOfficeTravelRequest.MasterTableView.Items[i].Selected = true;
                        break;
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

    protected void gvOfficeTravelRequest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOfficeTravelRequest.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvOfficeTravelRequest_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;
            else if (e.CommandName.ToUpper() == "SELECT")
            {
                ViewState["TRAVELREQUESTID"] = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                ViewState["PERSONALINFOSN"] = ((RadLabel)e.Item.FindControl("lblPersonalInfosn")).Text;
                LinkButton lnkRequestNo = (LinkButton)e.Item.FindControl("lnkRequestNo");
                PhoenixCrewTravelRequest.RequestNo = lnkRequestNo.Text;
                Response.Redirect("../Crew/CrewHRTravelPassengerList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString(), false);               
            }
            else if (e.CommandName.ToUpper() == "CONFIRM")
            {
                ViewState["TRAVELREQUESTID"] = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                ViewState["PERSONALINFOSN"] = ((RadLabel)e.Item.FindControl("lblPersonalInfosn")).Text;

                RadWindowManager1.RadConfirm("Are you sure you want to Send For Approval Travel Request?<br/> Further changes are not allowed.", "confirm", 320, 150, null, "Confirm");

            }
            else if (e.CommandName.ToUpper() == "CANCELED")
            {
                ViewState["TRAVELREQUESTID"] = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                ViewState["PERSONALINFOSN"] = ((RadLabel)e.Item.FindControl("lblPersonalInfosn")).Text;

                RadWindowManager1.RadConfirm("Are you sure you want to Cancel Travel Request?", "cancel", 320, 150, null, "Cancel");
            }
            else if (e.CommandName.ToUpper() == "RESEND")
            {
                ViewState["TRAVELREQUESTID"] = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                ViewState["PERSONALINFOSN"] = ((RadLabel)e.Item.FindControl("lblPersonalInfosn")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want Resend Mail?", "resendmail", 320, 150, null, "Resend Mail");
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOfficeTravelRequest_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {            
            if (e.Item  is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                LinkButton select = (LinkButton)e.Item.FindControl("cmdSelect");
                if (select != null) select.Visible = SessionUtil.CanAccess(this.ViewState, select.CommandName);
                LinkButton cmdresend = (LinkButton)e.Item.FindControl("cmdresend");
                if (cmdresend != null)
                {
                    cmdresend.Visible = SessionUtil.CanAccess(this.ViewState, select.CommandName);
                    cmdresend.Visible = false;
                    if (drv["FLDAPPROVEDYN"].ToString() == "2")
                        cmdresend.Visible = true;
                }
                LinkButton cmdapproval = (LinkButton)e.Item.FindControl("cmdapproval");
                if (cmdapproval != null)
                {
                    cmdapproval.Visible = SessionUtil.CanAccess(this.ViewState, select.CommandName);
                    cmdapproval.Visible = false;
                    if (drv["FLDAPPROVEDYN"].ToString() == "" || drv["FLDAPPROVEDYN"].ToString() == "0")
                        cmdapproval.Visible = true;
                }
                LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cmdCancel != null)
                {
                    cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, select.CommandName);
                    cmdCancel.Visible = false;
                    if (drv["FLDAPPROVEDYN"].ToString() != "1")
                        cmdCancel.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
