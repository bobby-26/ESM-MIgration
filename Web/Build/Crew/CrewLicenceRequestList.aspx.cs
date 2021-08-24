using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using System.Drawing;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewLicenceRequestList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!Page.IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 0;

                if (Request.QueryString["nl"] != null)
                {
                    ucError.ErrorMessage = "Application Form Not Available.";
                    ucError.Visible = true;
                }
                if (Request.QueryString["err"] != null)
                {
                    ucError.ErrorMessage = "Seperate Application Form for COC is not required for this flag.";
                    ucError.Visible = true;
                }
                if (Request.QueryString["cl"] != null)
                {
                    ucError.ErrorMessage = "Covering Letter Not Available.";
                    ucError.Visible = true;
                }
                if (Request.QueryString["serv"] != null)
                {
                    ucError.ErrorMessage = "Sea Service Not Available for this flag.";
                    ucError.Visible = true;
                }

                ViewState["REQCANCELLED"] = PhoenixCommonRegisters.GetHardCode(1, 123, "CCL");

                gvLicReq.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewLicenceRequestList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvLicReq')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewLicenceRequestListFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewLicenceRequestList.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewLicenceRequestList.aspx", "Covering Letter", " <i class=\"fas fa-envelope-open-text\"></i>", "COVERINGLETTER");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewLicenceRequestList.aspx", "Application Form", " <i class=\"fas fa-file-contract-af\"></i>", "APPLICATION");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewLicenceRequestList.aspx", "CRA Form", " <i class=\"fa fa-file-signature-ac\"></i>", "CRAFORM");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewLicenceRequestAdd.aspx", "Add Licence Request", " <i class=\"fas fa-plus-circle\"></i>", "ADD");

            MenuLicenceList.AccessRights = this.ViewState;
            MenuLicenceList.MenuList = toolbargrid.Show();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuLicenceList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentCrewLicenceRequestFilter = null;
                gvLicReq.CurrentPageIndex = 0;
                cmdHiddenSubmit_Click(null, null);
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDROWNUMBER", "FLDREQUISITIONNUMBER", "FLDNAME", "FLDFILENO", "FLDFLAGNAME", "FLDVESSELNAME", "FLDCONSULATE", "FLDREQUESTSENTSTATUS", "FLDRECEIVEDSTATUS", "FLDAMOUNT", "FLDCURRENCYCODE", "FLDPAYMENTTYPE", "FLDPAYMENTNUMBER", "FLDREQSTATUS" };
                string[] alCaptions = { "S.No.", "Request No.", "Name", "File No.", "Flag", "Vessel", "Consulate", "Requested", "Received", "Amount", "Currency", "Payment", "Invoice/Voucher No", "Status" };
                string sortexpression;
                int? sortdirection = 1;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                NameValueCollection nvc = Filter.CurrentCrewLicenceRequestFilter;
                DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceRequestSearch(nvc != null ? General.GetNullableInteger(nvc.Get("ddlFlag")) : null
                                                                           , nvc != null ? General.GetNullableString(nvc.Get("ddlVessel")) : null
                                                                           , nvc != null ? General.GetNullableString(nvc.Get("txtReqNo")) : null
                                                                           , sortexpression, sortdirection
                                                                           , (int)ViewState["PAGENUMBER"], gvLicReq.PageSize
                                                                           , ref iRowCount, ref iTotalPageCount
                                                                           , nvc != null ? General.GetNullableString(nvc.Get("txtName")) : null
                                                                           , nvc != null ? General.GetNullableString(nvc.Get("txtFileno")) : null
                                                                           , nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : null
                                                                           , null
                                                                           );
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Licence Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("COVERINGLETTER"))
            {
                if (ViewState["CURRENTINDEX"] == null )
                {
                    ucError.ErrorMessage = "Select one Licence Request";
                    ucError.Visible = true;
                    return;
                }

                GridDataItem item = gvLicReq.Items[int.Parse(ViewState["CURRENTINDEX"].ToString())];                
                string processid = "";
                if (Filter.CurrentLicenceReqCovLetterFilter != null)
                    processid = Filter.CurrentLicenceReqCovLetterFilter;
                else
                    processid = ((RadLabel)item.FindControl("lblRequestId")).Text;
                string flagid = ((RadLabel)item.FindControl("lblFlagId")).Text;
                string strRefNo = string.Empty;
                if (processid != "")
                {
                    Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=LICENCENEW&flagid=" + flagid + "&processid=" + processid + "&refno=" + strRefNo, false);
                }
            }
            else if (CommandName.ToUpper().Equals("APPLICATION"))
            {
                if (ViewState["CURRENTINDEX"] == null)
                {
                    ucError.ErrorMessage = "Select one Licence Request";
                    ucError.Visible = true;
                    return;
                }

                GridDataItem item = gvLicReq.Items[int.Parse(ViewState["CURRENTINDEX"].ToString())];

                string processid = ((RadLabel)item.FindControl("lblRequestId")).Text;
                string flagid = ((RadLabel)item.FindControl("lblFlagId")).Text;
                string strRefNo = string.Empty;
                if (processid != "")
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&showword=0&showexcel=0&reportcode=LICENCEAPPNEW&flagid=" + flagid + "&processid=" + processid + "&refno=" + strRefNo);
                }
            }
            else if (CommandName.ToUpper().Equals("CRAFORM"))
            {
                if (ViewState["CURRENTINDEX"] == null )
                {
                    ucError.ErrorMessage = "Select one Licence Request";
                    ucError.Visible = true;
                    return;
                }

                GridDataItem item = gvLicReq.Items[int.Parse(ViewState["CURRENTINDEX"].ToString())];
                
                string processid = ((RadLabel)item.FindControl("lblRequestId")).Text;
                string flagid = ((RadLabel)item.FindControl("lblFlagId")).Text;
                string strRefNo = string.Empty;
                if (flagid.Equals("94"))
                {
                    Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=LICENCECRANEW&flagid=" + flagid + "&processid=" + processid + "&refno=" + strRefNo);
                }
                else
                {
                    ucError.ErrorMessage = "CRA Form Not Available";
                    ucError.Visible = true;
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvLicReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLicReq.CurrentPageIndex + 1;

        BindData();
    }


    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDREQUISITIONNUMBER", "FLDNAME", "FLDFILENO", "FLDFLAGNAME", "FLDVESSELNAME", "FLDCONSULATE", "FLDREQUESTSENTSTATUS", "FLDRECEIVEDSTATUS", "FLDAMOUNT", "FLDCURRENCYCODE", "FLDPAYMENTTYPE", "FLDPAYMENTNUMBER", "FLDREQSTATUS" };
        string[] alCaptions = { "S.No.", "Request No.", "Name", "File No.", "Flag", "Vessel", "Consulate", "Requested", "Received", "Amount", "Currency", "Payment", "Invoice/Voucher No.", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {
            NameValueCollection nvc = Filter.CurrentCrewLicenceRequestFilter;
            DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceRequestSearch(nvc != null ? General.GetNullableInteger(nvc.Get("ddlFlag")) : null
                                                                       , nvc != null ? General.GetNullableString(nvc.Get("ddlVessel")) : null
                                                                       , nvc != null ? General.GetNullableString(nvc.Get("txtReqNo")) : null
                                                                       , sortexpression, sortdirection
                                                                       , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                       , gvLicReq.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount
                                                                       , nvc != null ? General.GetNullableString(nvc.Get("txtName")) : null
                                                                       , nvc != null ? General.GetNullableString(nvc.Get("txtFileno")) : null
                                                                       , nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : null
                                                                       , null
                                                                      );

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvLicReq", "Licence Request", alCaptions, alColumns, ds);

            gvLicReq.DataSource = dt;
            gvLicReq.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvLicReq_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkEmployee");
            if (drv["FLDNEWAPP"].ToString() == "1")
            {
                lb.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&licence=1'); return false;");
            }
            else
            {
                lb.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&licence=1'); return false;");
            }

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null)
            {
                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
                cancel.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel this Request?')");
            }

            RadLabel lblstatus = (RadLabel)e.Item.FindControl("lblReqstatid");

            if (lblstatus != null)
            {
                if ((lblstatus.Text.Equals(ViewState["REQCANCELLED"].ToString())))
                {
                    LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                    if (cmdEdit != null) cmdEdit.Visible = false;

                    LinkButton cmdGeneratePO = (LinkButton)e.Item.FindControl("cmdGeneratePO");
                    if (cmdGeneratePO != null) cmdGeneratePO.Visible = false;

                    LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
                    if (cmdCancel != null) cmdCancel.Visible = false;
                }
            }
        }
    }

    protected void gvLicReq_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SORT") return;

            if (e.CommandName.ToString().ToUpper() == "LICENCEDETAIL")
            {
                RadLabel lblRID = ((RadLabel)e.Item.FindControl("lblRequestId"));
                Response.Redirect("../Crew/CrewLicenceRequestDetailList.aspx?rid=" + lblRID.Text, false);
            }
            if (e.CommandName.ToString().ToUpper() == "MAKEPAYMENT")
            {
                RadLabel lblRID = ((RadLabel)e.Item.FindControl("lblRequestId"));
                Response.Redirect("../Crew/CrewLicenceRequestPayment.aspx?rid=" + lblRID.Text, false);

            }
            if (e.CommandName.ToString().ToUpper() == "CANCELREQUEST")
            {
                RadLabel lblRID = ((RadLabel)e.Item.FindControl("lblRequestId"));
                PhoenixCrewLicenceRequest.LicenceRequestCancel(new Guid(lblRID.Text));

                BindData();
                gvLicReq.Rebind();
            }
            if (e.CommandName.ToString().ToUpper() == "MOREINFO")
            {
                RadLabel lblRID = ((RadLabel)e.Item.FindControl("lblRequestId"));
                RadLabel lblDtkey = ((RadLabel)e.Item.FindControl("lblDtkey"));
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewLicenceRequestExtraInfo.aspx?rid= " + lblRID.Text + "&dtkey=" + lblDtkey.Text + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (e.CommandName.ToUpper() == "ROWCLICK" || e.CommandName.ToUpper() == "SELECT")
            {
                gvLicReq.SelectedIndexes.Clear();
                gvLicReq.SelectedIndexes.Add(e.Item.ItemIndex);
                ViewState["CURRENTINDEX"] = e.Item.ItemIndex;
                
            }
            if (e.CommandName == "Page")
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

    protected void gvLicReq_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string reqid = ((RadLabel)e.Item.FindControl("lblRequestEditId")).Text;
            string requestsentyn = ((RadLabel)e.Item.FindControl("lblRequestYN")).Text;
            RadCheckBox chk = ((RadCheckBox)e.Item.FindControl("chkRequestedEdit"));
            string Requestedyn = chk.Checked == true ? "1" : "0";

            if (Requestedyn == "1" && requestsentyn != "1")
            {
                PhoenixCrewLicenceRequest.LicenceRequestSentUpdate(new Guid(reqid)
                    , byte.Parse(Requestedyn)
                   );
            }

            RadCheckBox chkReceived = ((RadCheckBox)e.Item.FindControl("chkReceivedEdit"));
            string ReceivedDoc = ((RadLabel)e.Item.FindControl("lblReceivedYN")).Text;
            string Receivedyn = chkReceived.Checked == true ? "1" : "0";
            if (Receivedyn == "1" && requestsentyn == "1" && ReceivedDoc != "1")
            {
                PhoenixCrewLicenceRequest.LicenceRequestStatusUpdate(new Guid(reqid)
                    , byte.Parse(Receivedyn)
                   );
            }

            BindData();
            gvLicReq.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLicReq_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvLicReq.Rebind();
    }

}