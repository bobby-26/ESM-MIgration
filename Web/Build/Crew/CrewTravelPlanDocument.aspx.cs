using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.CrewCommon;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewTravelPlanDocument : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarMenu = new PhoenixToolbar();
            MenuTab.AccessRights = this.ViewState;
            MenuTab.MenuList = toolbarMenu.Show();


            PhoenixToolbar toolbarmenu = new PhoenixToolbar();
            toolbarmenu.AddFontAwesomeButton("../Crew/CrewTravelPlanDocument.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarmenu.AddFontAwesomeButton("javascript:CallPrint('gvCrewTravelPlan')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuCrewTravelPlan.AccessRights = this.ViewState;
            MenuCrewTravelPlan.MenuList = toolbarmenu.Show();


            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelPlanDocument.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvHistory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelPlanDocument.aspx", "Add Travel History", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            CrewTravelHistory.AccessRights = this.ViewState;
            CrewTravelHistory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["PAGENUMBERHISTORY"] = 1;
                ViewState["SORTEXPRESSIONHISTORY"] = null;
                ViewState["SORTDIRECTIONHISTORY"] = null;

                gvCrewTravelPlan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvHistory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                SetEmployeePrimaryDetails();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                //txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
                //txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();               
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
            gvCrewTravelPlan.Rebind();
            gvHistory.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewTravelPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewTravelPlan.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELNAME", "FLDVESSELACCOUNTNAME", "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDTICKETNO", "FLDPNRNO", "FLDAIRLINECODE", "FLDCLASS", "FLDAMOUNT" };
            string[] alCaptions = { "Vessel", "Vessel Account", "Origin", "Destination", "Departure", "Arrival", "Ticket No.", "PNR No.", "Airline Code", "Class", "Amount" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixCrewTravelDocument.SearchCrewTravelPlanDocument(Convert.ToInt32(Filter.CurrentCrewSelection),
                                                                               sortexpression, sortdirection,
                                                                               (int)ViewState["PAGENUMBER"],
                                                                               gvCrewTravelPlan.PageSize,
                                                                               ref iRowCount,
                                                                               ref iTotalPageCount);

            General.SetPrintOptions("gvCrewTravelPlan", "Travel Plan", alCaptions, alColumns, ds);

            gvCrewTravelPlan.DataSource = ds;
            gvCrewTravelPlan.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
       
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCrewTravelPlan_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ADD"))
            {
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewTravelPlanHistoryAdd.aspx?empid=" + Filter.CurrentCrewSelection + "',false,700,450);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDVESSELNAME", "FLDVESSELACCOUNTNAME", "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDTICKETNO", "FLDPNRNO", "FLDAIRLINECODE", "FLDCLASS", "FLDAMOUNT" };
                string[] alCaptions = { "Vessel", "Vessel Account","Origin", "Destination", "Departure", "Arrival", "Ticket No.", "PNR No.", "Airline Code", "Class", "Amount" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTIO"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewTravelDocument.SearchCrewTravelPlanDocument(Convert.ToInt32(Filter.CurrentCrewSelection),
                                                                          sortexpression, sortdirection,
                                                                          (int)ViewState["PAGENUMBER"],
                                                                          iRowCount,
                                                                          ref iRowCount,
                                                                          ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Travel Plan", ds.Tables[0], alColumns, alCaptions, null, null);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewTravelPlan_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
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

    protected void gvCrewTravelPlan_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            RadLabel lblTravelHistoryId = (RadLabel)e.Item.FindControl("lblTravelHistoryId");
            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
                cme.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Crew/CrewTravelPlanHistoryAdd.aspx?TravelHistoryId=" + lblTravelHistoryId.Text + "&empid=" + Filter.CurrentCrewSelection + "',false,700,450);return false;");
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.TRAVEL + "&cmdname=TRAVELUPLOAD'); return false;");
            }

        }



        if (e.Item is GridFooterItem)
        {

        }

    }

    protected void gvHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERHISTORY"] = ViewState["PAGENUMBERHISTORY"] != null ? ViewState["PAGENUMBERHISTORY"] : gvHistory.CurrentPageIndex + 1;
            BindTravelHistory();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewTravelHistory_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("ADD"))
            {
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewTravelPlanHistoryAdd.aspx?empid=" + Filter.CurrentCrewSelection + "',false,700,450);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDVESSELNAME", "FLDVESSELACCOUNTNAME", "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDTICKETNO", "FLDPNRNO", "FLDAIRLINECODE", "FLDCLASS", "FLDAMOUNT" };
                string[] alCaptions = { "Vessel", "Vessel Account","Origin", "Destination", "Departure", "Arrival", "Ticket No.", "PNR No.", "Airline Code", "Class", "Amount" };

                string sortexpression = (ViewState["SORTEXPRESSIONHISTORY"] == null) ? null : (ViewState["SORTEXPRESSIONHISTORY"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTIONHISTORY"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTIONHISTORY"].ToString());
                
                if (ViewState["ROWCOUNTHISTORY"] == null || Int32.Parse(ViewState["ROWCOUNTHISTORY"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNTHISTORY"].ToString());

                DataSet ds = PhoenixCrewTravelDocument.SearchCrewTravelHistory(Convert.ToInt32(Filter.CurrentCrewSelection),
                                                                          sortexpression, sortdirection,
                                                                          (int)ViewState["PAGENUMBERHISTORY"],
                                                                          iRowCount,
                                                                          ref iRowCount,
                                                                          ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Travel History", ds.Tables[0], alColumns, alCaptions, null, null);
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void BindTravelHistory()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDVESSELNAME", "FLDVESSELACCOUNTNAME", "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDTICKETNO", "FLDPNRNO", "FLDAIRLINECODE", "FLDCLASS", "FLDAMOUNT" };
            string[] alCaptions = { "Vessel", "Vessel Account","Origin", "Destination", "Departure", "Arrival", "Ticket No.", "PNR No.", "Airline Code", "Class", "Amount" };

            string sortexpression = (ViewState["SORTEXPRESSIONHISTORY"] == null) ? null : (ViewState["SORTEXPRESSIONHISTORY"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONHISTORY"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONHISTORY"].ToString());


            DataSet ds = PhoenixCrewTravelDocument.SearchCrewTravelHistory(Convert.ToInt32(Filter.CurrentCrewSelection),
                                                                               sortexpression, sortdirection,
                                                                               (int)ViewState["PAGENUMBERHISTORY"],
                                                                               gvHistory.PageSize,
                                                                               ref iRowCount,
                                                                               ref iTotalPageCount);

            General.SetPrintOptions("gvHistory", "Travel History", alCaptions, alColumns, ds);

            gvHistory.DataSource = ds;
            gvHistory.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNTHISTORY"] = iRowCount;
          

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvHistory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
           
            RadLabel lblTravelHistoryId = (RadLabel)e.Item.FindControl("lblTravelHistoryId");
            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
                cme.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Crew/CrewTravelPlanHistoryAdd.aspx?TravelHistoryId=" + lblTravelHistoryId.Text + "&empid=" + Filter.CurrentCrewSelection + "',false,700,450);return false;");
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.TRAVEL + "&cmdname=TRAVELUPLOAD'); return false;");
            }

        }

      

        if (e.Item is GridFooterItem)
        {
          
        }
    }

    protected void gvHistory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERHISTORY"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }




    protected void gvCrewTravelPlan_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        //ViewState["SORTEXPRESSION"] = e.SortExpression;
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
}

