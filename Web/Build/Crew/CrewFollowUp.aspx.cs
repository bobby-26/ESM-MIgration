using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewFollowUp : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            PhoenixToolbar toolbar = new PhoenixToolbar();         
            toolbar.AddFontAwesomeButton("../Crew/CrewFollowUp.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewFollowUp')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewFollowUpFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewFollowUp.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Crew/CrewFollowUp.aspx", "Stop Follow Up", "<i class=\"fas fa-bell-slash\"></i>", "STOP");

            MenuCrewFollowUpGrid.AccessRights = this.ViewState;
            MenuCrewFollowUpGrid.MenuList = toolbar.Show();
          

            if (!IsPostBack)
            {
               
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvCrewFollowUp.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuCrewFollowUp_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void OnClick_AssignFollowUp(object sender, EventArgs e)
    {
        try
        {
            string csvDtkey = string.Empty, csvType = string.Empty;
            foreach (GridDataItem gv in gvCrewFollowUp.Items)
            {
                RadCheckBox ck = (RadCheckBox)gv.FindControl("chkSelect");
                if (ck.Checked== true && ck.Enabled == true)
                {
                    csvDtkey += ((RadLabel)gv.FindControl("lbldtkey")).Text + ",";

                }
            }
            if (!IsValidAssignFollowUp(txtuserid.Text, csvDtkey.TrimEnd(',')))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewManagement.CrewFollowupUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(txtuserid.Text),
                csvDtkey.TrimEnd(',').ToString(), 2);

            ucStatus.Text = "Successfully Stopped follow ups";
            BindData();
            gvCrewFollowUp.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidStopFollowUp(string csvDtkey)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(csvDtkey))
            ucError.ErrorMessage = "Select atleast one or more Employee for stopping follow ups.";

        return (!ucError.IsError);
    }

    private bool IsValidAssignFollowUp(string userid, string csvdtkey)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(csvdtkey))
            ucError.ErrorMessage = "Select atleast one or more Employee for follow ups to be transferred.";

        if (General.GetNullableInteger(userid) == null)
            ucError.ErrorMessage = "Please select the User,the follow ups to be transferred to.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvCrewFollowUp.Rebind();
    }


    protected void MenuCrewFollowUpGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CrewFollowUpFilterSelection = null;
                ViewState["PAGENUMBER"] = 1;
                txtuserid.Text = "";
                txtMentorName.Text = "";
                txtuserDesignation.Text = "";
                txtuserEmailHidden.Text = "";
                gvCrewFollowUp.CurrentPageIndex = 0;
                BindData();
                gvCrewFollowUp.Rebind();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }
            else if (CommandName.ToUpper().Equals("STOP"))
            {
                string csvDtkey = string.Empty, csvType = string.Empty;
                foreach (GridDataItem gv in gvCrewFollowUp.Items)
                {
                    RadCheckBox ck = (RadCheckBox)gv.FindControl("chkSelect");
                    if (ck.Checked == true && ck.Enabled == true)
                    {
                        csvDtkey += ((RadLabel)gv.FindControl("lbldtkey")).Text + ",";

                    }
                }
                if (!IsValidStopFollowUp(csvDtkey.TrimEnd(',')))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewManagement.CrewFollowupUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null,
                    csvDtkey.TrimEnd(',').ToString(), 1);

                ucStatus.Text = "Successfully Stopped follow ups";
                BindData();
                gvCrewFollowUp.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEECODE", "FLDNAME", "FLDRANKCODE", "FLDZONE", "FLDUSERNAME", "FLDFOLLOWUPDATE" };
        string[] alCaptions = { "S.No", "File No", "Name", "Rank", "Zone", "By", "Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        ViewState["PAGENUMBER"] = 1;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CrewFollowUpFilterSelection;

        DataSet ds = new DataSet();
        ds = PhoenixCrewManagement.CrewFollowupSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                  , (nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : null)
                                  , (nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate")) : null)
                                  , (nvc != null ? General.GetNullableDateTime(nvc.Get("ucLastContactFromDate")) : null)
                                  , (nvc != null ? General.GetNullableDateTime(nvc.Get("ucLastContactToDate")) : null)
                                  , (nvc != null ? General.GetNullableString(nvc.Get("lstRank")) : null)
                                  , (nvc != null ? General.GetNullableInteger(nvc.Get("ddlUser")) : null)
                                  //, General.GetNullableInteger(ddlUser.SelectedUser)
                                  , sortexpression, sortdirection
                                  , (int)ViewState["PAGENUMBER"]
                                  , iRowCount
                                  , ref iRowCount
                                  , ref iTotalPageCount
                                  , (nvc != null ? General.GetNullableString(nvc.Get("lstVesselType")) : null)
                                  );

        if (ds.Tables.Count > 0)
            General.ShowExcel("Crew Follow Up", ds.Tables[0], alColumns, alCaptions, null, "");

    }

    private void BindData()
    {
        try
            {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEECODE", "FLDNAME", "FLDRANKCODE", "FLDZONE", "FLDUSERNAME", "FLDFOLLOWUPDATE" };
            string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Zone", "Follow up By", "Follow up Date" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CrewFollowUpFilterSelection;
            DataSet ds = new DataSet();


            ds = PhoenixCrewManagement.CrewFollowupSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate")) : null)
                                 , (nvc != null ? General.GetNullableDateTime(nvc.Get("ucLastContactFromDate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("ucLastContactToDate")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("lstRank")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("ddlUser")) : null)
                                //, General.GetNullableInteger(ddlUser.SelectedUser)
                                , sortexpression, sortdirection
                                 , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                , gvCrewFollowUp.PageSize
                                , ref iRowCount
                                , ref iTotalPageCount
                                , (nvc != null ? General.GetNullableString(nvc.Get("lstVesselType")) : null)
                                );


            General.SetPrintOptions("gvCrewFollowUp", "Crew Follow Up", alCaptions, alColumns, ds);

            gvCrewFollowUp.DataSource = ds;
            gvCrewFollowUp.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewFollowUp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewFollowUp.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCrewFollowUp_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("STOPFOLLOWUP"))
            {
                string dtkey = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                PhoenixCrewManagement.CrewFollowupUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null,
                  dtkey, 1);

                ucStatus.Text = "Successfully Stopped follow ups";
                BindData();
                gvCrewFollowUp.Rebind();
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


    protected void gvCrewFollowUp_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblEmpid = (RadLabel)e.Item.FindControl("lblEmpid");
            RadLabel lblEmpcode = (RadLabel)e.Item.FindControl("lblFileNo");

            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkEmployeeName");

            bool str = lblEmpcode.Text.StartsWith("N");

            if (str == true)
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + lblEmpid.Text + "&remarks=1'); return false;");
            }
            else
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblEmpid.Text + "&remarks=1'); return false;");
            }


            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdStopFollowup");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure want to stop follow Up?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }
            
        }
    }



    protected void gvCrewFollowUp_SortCommand(object sender, GridSortCommandEventArgs e)
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
        BindData();
    }


}
