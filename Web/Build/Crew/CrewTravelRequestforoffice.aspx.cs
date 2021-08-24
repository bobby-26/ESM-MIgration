using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CrewTravelRequestforoffice : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                ViewState["TRAVELID"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["REQUESTSTATUS"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 130, "TRQ");
                ViewState["TRQ"] = PhoenixCommonRegisters.GetHardCode(1, 130, "TRQ");
                ViewState["TPO"] = PhoenixCommonRegisters.GetHardCode(1, 130, "TPO");
                ViewState["TQY"] = PhoenixCommonRegisters.GetHardCode(1, 130, "TQY");
                ViewState["ISS"] = PhoenixCommonRegisters.GetHardCode(1, 130, "ISS");
                ViewState["CND"] = PhoenixCommonRegisters.GetHardCode(1, 130, "CND");
                ViewState["CUT"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 88, "CUT");
                
                ViewState["EDITTRAVELREQUEST"] = "1";
                ViewState["EDITROW"] = "0";

                if (Request.QueryString["TRAVELID"] != null)
                    ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();

                if (Request.QueryString["travelrequestedit"] != null)
                    ViewState["EDITTRAVELREQUEST"] = Request.QueryString["travelrequestedit"].ToString();

                gvCCT.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelRequestforoffice.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCCT')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelRequestFilter.aspx?office=1'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelRequestforoffice.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");           
            MenutravelList.AccessRights = this.ViewState;
            MenutravelList.MenuList = toolbar.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void MenutravelList_TabStripCommand(object sender, EventArgs e)
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
                Filter.CurrentOfficeTravelRequestFilter = null;
                ViewState["TRAVELID"] = null;
                ViewState["PAGENUMBER"] = 1;
                gvCCT.CurrentPageIndex = 0;
                BindData();
                gvCCT.Rebind();
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
        string[] alColumns = { "FLDREQUISITIONNO", "FLDVESSELNAME", "FLDDATEOFCREWCHANGE", "FLDPURPOSE", "FLDPURPOSEREMARKS", "FLDSTATUS" };
        string[] alCaptions = { "Request No", "Vessel", "Date", "Purpose", "Purpose Desc", "Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentOfficeTravelRequestFilter;

        ds = PhoenixCrewTravelRequest.SearchTravel(
                General.GetNullableInteger(nvc != null ? nvc.Get("vesselid") : string.Empty)
              , General.GetNullableInteger(nvc != null ? nvc.Get("ucpurpose") : string.Empty)
              , General.GetNullableDateTime(nvc != null ? nvc.Get("txtStartDate") : string.Empty)
              , General.GetNullableDateTime(nvc != null ? nvc.Get("txtEndDate") : string.Empty)
              , General.GetNullableString(nvc != null ? nvc.Get("uctravelstatus") : null)
              , General.GetNullableInteger(nvc != null ? nvc.Get("txtOrigin") : string.Empty)
              , General.GetNullableInteger(nvc != null ? nvc.Get("txtDestination") : string.Empty)
              , 1
              , General.GetNullableString(nvc != null ? nvc.Get("txtTravelRequestNo") : string.Empty)
              , General.GetNullableString(nvc != null ? nvc.Get("txtPassengerName") : string.Empty)
              , General.GetNullableInteger(nvc != null ? nvc.Get("ucZone") : string.Empty)
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvCCT.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.ShowExcel("Office Travel Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDREQUISITIONNO", "FLDVESSELNAME", "FLDDATEOFCREWCHANGE", "FLDPURPOSE", "FLDPURPOSEREMARKS", "FLDSTATUS" };
            string[] alCaptions = { "Request No", "Vessel", "Date", "Purpose", "Purpose Desc", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentOfficeTravelRequestFilter;

            DataSet ds = PhoenixCrewTravelRequest.SearchTravel(
                    General.GetNullableInteger(nvc != null ? nvc.Get("vesselid") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("ucpurpose") : string.Empty)
                  , General.GetNullableDateTime(nvc != null ? nvc.Get("txtStartDate") : string.Empty)
                  , General.GetNullableDateTime(nvc != null ? nvc.Get("txtEndDate") : string.Empty)
                  , General.GetNullableString(nvc != null ? nvc.Get("uctravelstatus") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("txtOrigin") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("txtDestination") : string.Empty)
                  , 1
                  , General.GetNullableString(nvc != null ? nvc.Get("txtTravelRequestNo") : string.Empty)
                  , General.GetNullableString(nvc != null ? nvc.Get("txtPassengerName") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("ucZone") : string.Empty)
                 , sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                gvCCT.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("gvCCT", "Office Travel Request", alCaptions, alColumns, ds);

            gvCCT.DataSource = ds;
            gvCCT.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["TRAVELID"] == null)
                {
                    ViewState["TRAVELID"] = ds.Tables[0].Rows[0]["FLDTRAVELID"].ToString();
                    Filter.CurrentTraveltoVesselName = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                    PhoenixCrewTravelRequest.RequestNo = ds.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString();
                    PhoenixCrewTravelRequest.OfficeTravelPassengerFrom = ds.Tables[0].Rows[0]["FLDPASSENGERFROM"].ToString();

                    if (ds.Tables[0].Rows[0]["FLDTRAVELSTATUS"].ToString() == ViewState["REQUESTSTATUS"].ToString())
                        ViewState["EDITTRAVELREQUEST"] = "1";
                    else
                        ViewState["EDITTRAVELREQUEST"] = "0";

                    SetRowSelection();
                }              
            }
            
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetRowSelection()
    {
        try
        {
            
            foreach (GridDataItem item in gvCCT.Items)
            {
                if (item.GetDataKeyValue("FLDTRAVELID").ToString() == ViewState["TRAVELID"].ToString())
                {
                    gvCCT.SelectedIndexes.Clear();

                    gvCCT.SelectedIndexes.Add(item.ItemIndex);
                    
                }
            }

            if (Session["ENTRYPAGE"] != null && Session["ENTRYPAGE"].ToString().Equals("1"))
            {
                if (PhoenixCrewTravelRequest.OfficeTravelPassengerFrom.ToString() == "1")
                    ifMoreInfo.Attributes["src"] = "../Crew/CrewTravelPassengersSeafarerEntry.aspx?travelid=" + ViewState["TRAVELID"] + "&travelrequestedit=1";
                else
                    ifMoreInfo.Attributes["src"] = "../Crew/CrewTravelPassengersEntry.aspx?travelid=" + ViewState["TRAVELID"] + "&travelrequestedit=1";
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Crew/CrewTravelPassengersList.aspx?travelid=" + ViewState["TRAVELID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"];
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
            ViewState["TRAVELID"] = null;
            ViewState["PAGENUMBER"] = 1;
            gvCCT.CurrentPageIndex = 0;

            BindData();
            gvCCT.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCCT_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCCT.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvCCT_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;

            if (e.CommandName.ToUpper() == "SELECT")
            {
                RadLabel lblTravelRequestId = (RadLabel)e.Item.FindControl("lblTravelRequestId");
                LinkButton lnkRequestNo = (LinkButton)e.Item.FindControl("lnkReqNo");
                RadLabel lblvesselname = (RadLabel)e.Item.FindControl("lnkName");
                RadLabel lblPassengerFrom = (RadLabel)e.Item.FindControl("lblPassengerFrom");
                RadLabel lblTravelRequeststatusId = (RadLabel)e.Item.FindControl("lblStatusID");

                PhoenixCrewTravelRequest.RequestNo = lnkRequestNo.Text;
                Filter.CurrentTraveltoVesselName = lblvesselname.Text;
                PhoenixCrewTravelRequest.OfficeTravelPassengerFrom = lblPassengerFrom.Text;
                ViewState["TRAVELREQUESTID"] = lblTravelRequestId.Text;
                ViewState["TRAVELID"] = lblTravelRequestId.Text;
                ViewState["REQUESTID"] = lblTravelRequestId.Text;

                if (lblTravelRequeststatusId.Text == ViewState["REQUESTSTATUS"].ToString())
                    ViewState["EDITTRAVELREQUEST"] = "1";
                else
                    ViewState["EDITTRAVELREQUEST"] = "0";

                if (Session["ENTRYPAGE"] != null && Session["ENTRYPAGE"].ToString().Equals("1"))
                {
                    if (PhoenixCrewTravelRequest.OfficeTravelPassengerFrom.ToString() == "1")
                        ifMoreInfo.Attributes["src"] = "../Crew/CrewTravelPassengersSeafarerEntry.aspx?travelid=" + ViewState["TRAVELID"] + "&travelrequestedit=1";
                    else
                        ifMoreInfo.Attributes["src"] = "../Crew/CrewTravelPassengersEntry.aspx?travelid=" + ViewState["TRAVELID"] + "&travelrequestedit=1";
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "../Crew/CrewTravelPassengersList.aspx?travelid=" + ViewState["TRAVELID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"];
                }

            }
            if (e.CommandName.ToUpper() == "APPROVE")
            {
                RadLabel lblTravelRequestId = (RadLabel)e.Item.FindControl("lblTravelRequestId");
                LinkButton lnkRequestNo = (LinkButton)e.Item.FindControl("lnkReqNo");
                if (lblTravelRequestId.Text != "")
                {
                    ViewState["TRAVELID"] = lblTravelRequestId.Text;
                    PhoenixCrewTravelRequest.RequestNo = lnkRequestNo.Text;

                    //Approve office travel request
                    PhoenixCrewTravelRequest.OfficeTravelApprove(new Guid(lblTravelRequestId.Text));

                    BindData();
                    gvCCT.Rebind();
                }
                
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
            return;
        }
    }


    protected void gvCCT_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton ticket = (LinkButton)e.Item.FindControl("cmdShowTicket");
            RadLabel lblstatus = (RadLabel)e.Item.FindControl("lblStatusID");
            RadLabel lblTravelId = (RadLabel)e.Item.FindControl("lblTravelRequestId");
            
            if (ticket != null)
            {
                ticket.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewTravelTicket.aspx?TRAVELID=" + lblTravelId.Text + "'); return false;");
                
                if ((lblstatus.Text.Equals(ViewState["TPO"].ToString())) || (lblstatus.Text.Equals(ViewState["ISS"].ToString())))
                    ticket.Visible = true;
                else
                    ticket.Visible = false;
            }


            LinkButton imgappove = (LinkButton)e.Item.FindControl("cmdApprove");
            RadLabel lblApprovedYN = (RadLabel)e.Item.FindControl("lblApprovedYN");

            if (lblApprovedYN != null && lblApprovedYN.Text == "1")
            {
                if (imgappove != null)
                    imgappove.Visible = false;
            }
            else
            {
                if (imgappove != null)
                {
                    imgappove.Visible = true;                
                }
            }
            
            LinkButton imgFlag = (LinkButton)e.Item.FindControl("imgFlag");

            if ((lblstatus.Text.Equals(ViewState["TRQ"].ToString())) || (lblstatus.Text.Equals(ViewState["TPO"].ToString())) || (lblstatus.Text.Equals(ViewState["TQY"].ToString())))
            {
                imgFlag.Visible = true;
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:orange\"><i class=\"fas fa-star-yellow\"></i></span>";
                imgFlag.Controls.Add(html);

            }
            else if ((lblstatus.Text.Equals(ViewState["ISS"].ToString())))
            {
                imgFlag.Visible = true;
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:green\"><i class=\"fas fa-star-blue\"></i></span>";
                imgFlag.Controls.Add(html);
            }
            else if ((lblstatus.Text.Equals(ViewState["CND"].ToString())))
            {
                imgFlag.Visible = true;
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:red\"><i class=\"fas fa-star-red\"></i></span>";
                imgFlag.Controls.Add(html);

                if (imgappove != null)
                {
                    imgappove.Visible = false;
                }
            }

            LinkButton ib = (LinkButton)e.Item.FindControl("cmdAttachment");
            RadLabel lb = (RadLabel)e.Item.FindControl("lblDtKey");
            if (ib != null && lb != null)
            {
                ib.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?type=TRAVEL&DTKEY=" + lb.Text + "&mod="
                        + PhoenixModule.CREW + "'); return false;");
            }

            RadLabel lblattachmentyn = (RadLabel)e.Item.FindControl("lblattachmentyn");
            if (lblattachmentyn != null && lb != null)
            {
                if (lblattachmentyn.Text.Equals("1"))
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-paperclip\"></i></span>";
                    ib.Controls.Add(html);
                }
                else
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    ib.Controls.Add(html);
                }

            }

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblPurpose");
            RadLabel lbltravelpurpose = (RadLabel)e.Item.FindControl("lblTravelPurpose");
            if (lbltravelpurpose.Text == ViewState["CUT"].ToString())
            {
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipCourse");
                if (lbtn != null)
                {
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lbtn.ClientID;
                }                
            }
       
            RadLabel lbtnDesc = (RadLabel)e.Item.FindControl("lblPurposeDesc");
            UserControlToolTip uctDesc = (UserControlToolTip)e.Item.FindControl("ucToolTipPurposeDesc");
            if (lbtnDesc != null)
            {
                uctDesc.Position = ToolTipPosition.TopCenter;
                uctDesc.TargetControlId = lbtnDesc.ClientID;
            }
            
        }
    }

    

}
