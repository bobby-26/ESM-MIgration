using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewInactiveTransferToOffice : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../Crew/CrewInactiveTransferToOffice.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarsub.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewInactiveTransferToOfficeFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../Crew/CrewInactiveTransferToOffice.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbarsub.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void CrewQueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;              
                BindData();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentTransferToOfficeFilter = null;
                gvCrewSearch.CurrentPageIndex = 0;
                BindData();
                gvCrewSearch.Rebind();
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

        string[] alColumns = { "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDINACTIVEDATE", "FLDINACTIVEREMARKS", "FLDSTATUSNAME" };
        string[] alCaptions = { "File No", "Name", "Rank", "Date", "Reason", "Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentTransferToOfficeFilter;

        ds = PhoenixCommonCrew.CrewTransferToOffice(int.Parse(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 53, "TTO"))
                                                                   , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
                                                                   , nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
                                                                   , nvc != null ? nvc.Get("lstRank") : null
                                                                   , nvc != null ? nvc.Get("ddlVesselType") : null
                                                                   , nvc != null ? int.Parse(nvc.Get("chkIncludepastexp")) : 0
                                                                   , sortexpression, sortdirection
                                                                   , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                   , gvCrewSearch.PageSize
                                                                   , ref iRowCount, ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("Transfer to Office", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDINACTIVEDATE", "FLDINACTIVEREMARKS", "FLDSTATUSNAME" };
        string[] alCaptions = { "File No", "Name", "Rank", "Inactive Date", "Inactive Reason", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataSet ds = new DataSet();

            NameValueCollection nvc = Filter.CurrentTransferToOfficeFilter;

            ds = PhoenixCommonCrew.CrewTransferToOffice(int.Parse(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 53, "TTO"))
                                                                       , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
                                                                       , nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
                                                                       , nvc != null ? nvc.Get("lstRank") : null
                                                                       , nvc != null ? nvc.Get("ddlVesselType") : null
                                                                       , nvc != null ? int.Parse(nvc.Get("chkIncludepastexp")) : 0
                                                                       , sortexpression, sortdirection
                                                                       , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                       , gvCrewSearch.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvCrewSearch", "Transfer to Office", alCaptions, alColumns, ds);

            gvCrewSearch.DataSource = ds;
            gvCrewSearch.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;

        BindData();

    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
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

    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
                cme.Attributes.Add("onclick", "javascript:openNewWindow('edit', '', '" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "');return false;");
            }
            
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkEployeeName");
            if (lbr != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lbr.CommandName)) lbr.Enabled = false;
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
            }

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton sg = (LinkButton)e.Item.FindControl("imgActivity");
            if (sg != null)
            {
                sg.Visible = SessionUtil.CanAccess(this.ViewState, sg.CommandName);
                sg.Attributes.Add("onclick", "openNewWindow('codehelpactivity', '', '" + Session["sitepath"] + "/Crew/CrewActivityGeneral.aspx?empid=" + empid.Text + "&sg=0');return false;");
            }
            
            LinkButton pd = (LinkButton)e.Item.FindControl("cmdPDForm");
            if (pd != null)
            {
                pd.Visible = SessionUtil.CanAccess(this.ViewState, pd.CommandName);
                pd.Attributes.Add("onclick", "openNewWindow('PDForm', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + empid.Text + "&showmenu=0');return false;");
            }


        }

    }
    protected void gvCrewSearch_SortCommand(object sender, GridSortCommandEventArgs e)
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
        gvCrewSearch.Rebind();
    }

    private void ResetFormControlValues(Control parent)
    {
        try
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    ResetFormControlValues(c);
                }
                else
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = "";
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadioButton)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.DropDownList":
                            ((DropDownList)c).SelectedIndex = 0;
                            break;
                        case "System.Web.UI.WebControls.ListBox":
                            ((ListBox)c).SelectedIndex = 0;
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

}
