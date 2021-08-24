using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
public partial class VesselAccounts_VesselAccountsEmployeeOC28Assessment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsEmployeeOC28Assessment.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvOC28')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuSupdtConcerns.AccessRights = this.ViewState;
            MenuSupdtConcerns.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["Employeeid"] = string.IsNullOrEmpty(Request.QueryString["Employeeid"]) ? "" : Request.QueryString["Employeeid"];
                SetEmployeePrimaryDetails();
                gvOC28.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Back", "BACK",ToolBarDirection.Right);
            MainMenuSupdtConcerns.AccessRights = this.ViewState;
            MainMenuSupdtConcerns.MenuList = toolbar.Show();
         
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
            if (!ViewState["Employeeid"].ToString().Equals(""))
            {
                DataTable dt = PhoenixVesselAccountsEmployee.EditVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , Convert.ToInt32(ViewState["Employeeid"].ToString()));

                if (dt.Rows.Count > 0)
                {
                    txtEmployeeNumber.Text = dt.Rows[0]["FLDFILENO"].ToString();
                    txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                    txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                    txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                    txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                }
            }
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
            string[] alColumns = { "FLDVESSELNAME", "FLDTYPEDESCRIPTION", "FLDDATEOFJOINING", "FLDSIGNOFFDATE", "FLDASSESSMENTDATE", "FLDTOTALSCORE", "FLDTOTALSUPTSCORE", "FLDSCOREVARIENCE", "FLDSUPTNAME", "FLDSUPTCOMMENTS" };
            string[] alCaptions = { "Vessel", "Vessel Type", "Joining Date", "Relief Due", "Assessment Date", "Self Assessment", "Superintendent Assessment", "Difference", "Assessment By", "Assessment Comments" };


            DataSet ds = PhoenixCrewEmployeeSuptAssessment.SearchEmployeeSuptAssessment(Convert.ToInt32(ViewState["Employeeid"].ToString()),
                                                                               (int)ViewState["PAGENUMBER"],
                                                                               gvOC28.PageSize,
                                                                               ref iRowCount,
                                                                               ref iTotalPageCount,
                                                                               PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            General.SetPrintOptions("gvOC28", "OC 28", alCaptions, alColumns, ds);
            gvOC28.DataSource = ds;
            gvOC28.VirtualItemCount = iRowCount;



            //GridViewRow row1 = ((GridViewRow)gvOC28.Controls[0].Controls[1]);
            //row1.Attributes.Add("style", "position:static");
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string[] alColumns = { "FLDVESSELNAME", "FLDTYPEDESCRIPTION", "FLDDATEOFJOINING", "FLDSIGNOFFDATE", "FLDASSESSMENTDATE", "FLDTOTALSCORE", "FLDTOTALSUPTSCORE", "FLDSCOREVARIENCE", "FLDSUPTNAME", "FLDSUPTCOMMENTS" };
            string[] alCaptions = { "Vessel", "Vessel Type", "Joining Date", "Relief Due", "Assessment Date", "Self Assessment", "Superintendent Assessment", "Difference", "Assessment By", "Assessment Comments" };

            DataSet ds = PhoenixCrewEmployeeSuptAssessment.SearchEmployeeSuptAssessment(Convert.ToInt32(ViewState["Employeeid"].ToString()),
                                                                                1,
                                                                                iRowCount,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount,
                                                                                PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            General.ShowExcel("OC 28", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MainMenuSupdtConcerns_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsEmployeeAppraisalQuery.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSupdtConcerns_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOC28_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;

            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 4;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Assessment";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 6;
            HeaderGridRow.Cells.Add(HeaderCell);
            HeaderCell = new TableCell();

            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);
            gvOC28.Controls[0].Controls.AddAt(0, HeaderGridRow);

            GridViewRow row1 = ((GridViewRow)gvOC28.Controls[0].Controls[0]);
            row1.Attributes.Add("style", "position:static");
        }
    }
    
    protected void gvOC28_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOC28.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOC28_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucRemarks");
                RadLabel lb = (RadLabel)e.Item.FindControl("lblRemarks");
                LinkButton cmdCrewExport2XL = (LinkButton)e.Item.FindControl("cmdCrewExport2XL");
                LinkButton cmdSuptExport2XL = (LinkButton)e.Item.FindControl("cmdSuptExport2XL");
                RadLabel lblRank = (RadLabel)e.Item.FindControl("lblRank");
                if (lb != null)
                {
                    lb.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
                    lb.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
                }
                cmdCrewExport2XL.Visible = SessionUtil.CanAccess(this.ViewState, cmdCrewExport2XL.CommandName);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOC28_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
          
         
            RadLabel lblAssessmentId = (RadLabel)e.Item.FindControl("lblAssessmentId");
            RadLabel lblSignonOffId = (RadLabel)e.Item.FindControl("lblSignonOffId");
            RadLabel lblRank = (RadLabel)e.Item.FindControl("lblRank");
            if (e.CommandName.ToUpper().Equals("CREWEXPORT2XL"))
            {
                PhoenixCrew2XL.Export2XLCrewAssessment(Convert.ToInt32(ViewState["Employeeid"].ToString())
                                                        , int.Parse(lblSignonOffId.Text.Trim())
                                                        , General.GetNullableGuid(lblAssessmentId.Text.Trim())
                                                        , lblRank.Text.Trim());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
