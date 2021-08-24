using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionDailyWorkPlanList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            //toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDailyWorkPlanActivity.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDailyWorkPlanList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDeficiency')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDailyWorkPlanList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
            MenuDeficiency.AccessRights = this.ViewState;
            MenuDeficiency.MenuList = toolbargrid.Show();
            // MenuDeficiency.SetTrigger(pnlDeficiency);         

            if (!IsPostBack)
            {
                VesselConfiguration();
                ViewState["PAGENUMBER"] = string.Empty;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["COMPANYID"] = "";
                ViewState["VESSELID"] = string.Empty;
                ViewState["FLEET"] = string.Empty;
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }
                VesselConfiguration();
                gvDeficiency.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["Page"] != null && Request.QueryString["Page"].ToString() != "")
                {
                    ViewState["PAGENUMBER"] = Request.QueryString["Page"].ToString();
                }
                else
                    ViewState["PAGENUMBER"] = 1;


                if (Filter.CurrentDailyWorkPlan != null)
                {
                    NameValueCollection nvcFilter = Filter.CurrentDailyWorkPlan;
                    ViewState["VESSELID"] = nvcFilter["ucVessel"];
                    ViewState["FLEET"] = nvcFilter["ucFleet"];

                    ucFleet.SelectedFleet = ViewState["FLEET"].ToString();
                    ucVessel.SelectedVessel = ViewState["VESSELID"].ToString();
                }
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.bind();
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
                ucFleet.SelectedFleet = "";
                ucFleet.Enabled = false;
            }           
            
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void DeficiencyGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionDailyWorkPlanList.aspx");
            }
            if (CommandName.ToUpper().Equals("WORKPLAN"))
            {
                Response.Redirect("../Inspection/InspectionDailyWorkPlanActivity.aspx", false);
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                NameValueCollection criteria = new NameValueCollection();

                criteria.Add("ucFleet", ucFleet.SelectedFleet);
                criteria.Add("ucVessel", ucVessel.SelectedVessel);

                Filter.CurrentDailyWorkPlan = criteria;

                ViewState["PAGENUMBER"] = 1;
                //BindData();
                gvDeficiency.Rebind();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? vesselid = null;

            string[] alColumns = { "FLDVESSELNAME", "FLDDATE" };
            string[] alCaptions = { "Vessel", "Date" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            //NameValueCollection nvc = Filter.CurrentDeficiencyFilter;
            DataSet ds;

            //if (nvc != null)
            //    vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

            NameValueCollection nvc = Filter.CurrentDailyWorkPlan;

            if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

            if (nvc != null && General.GetNullableInteger(nvc["ucVessel"]) == null)
            {
                nvc["ucVessel"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }

            ds = PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanSearch(
                  nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : vesselid,
                  nvc != null ? General.GetNullableInteger(nvc["ucFleet"]) : null,
                  1,
                  iRowCount,
                  ref iRowCount,
                  ref iTotalPageCount,
                  sortexpression, sortdirection,
                  General.GetNullableInteger(ViewState["COMPANYID"].ToString()));


            General.ShowExcel("Daily Work Plan", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Deficiency_TabStripCommand(object sender, EventArgs e)
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
                Filter.CurrentDeficiencyFilter = null;

                //BindData();
                ViewState["PAGENUMBER"] = 1;
                gvDeficiency.Rebind();
                //SetPageNavigator();
            }
            else if (CommandName.ToUpper().Equals("ADD"))
            {
                Response.Redirect("../Inspection/InspectionDailyWorkPlanActivity.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("SEARCH"))
            {
                NameValueCollection criteria = new NameValueCollection();

                criteria.Add("ucFleet", ucFleet.SelectedFleet);
                criteria.Add("ucVessel", ucVessel.SelectedVessel);

                Filter.CurrentDailyWorkPlan = criteria;

                ViewState["PAGENUMBER"] = 1;
                //BindData();
                gvDeficiency.Rebind();
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
            int iRowCount = 10;
            int iTotalPageCount = 10;
            int? vesselid = null;

            string[] alColumns = { "FLDVESSELNAME", "FLDDATE" };
            string[] alCaptions = { "Vessel", "Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            // NameValueCollection nvc = Filter.CurrentDeficiencyFilter;

            NameValueCollection nvc = Filter.CurrentDailyWorkPlan;
            DataSet ds;
            //if (nvc != null)
            //    vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

            if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

            if (nvc != null && General.GetNullableInteger(nvc["ucVessel"]) == null)
            {
                nvc["ucVessel"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }

            ds = PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanSearch(
                  nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : vesselid,
                  nvc != null ? General.GetNullableInteger(nvc["ucFleet"]) : null,
                  int.Parse(ViewState["PAGENUMBER"].ToString()),
                  gvDeficiency.PageSize,
                  ref iRowCount,
                  ref iTotalPageCount,
                  sortexpression, sortdirection,
                  General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

            General.SetPrintOptions("gvDeficiency", "Daily Work Plan", alCaptions, alColumns, ds);
            gvDeficiency.DataSource = ds;
            gvDeficiency.VirtualItemCount = iRowCount;
            if (ds.Tables[0].Rows.Count > 0)
            {
               
                if (Filter.CurrentSelectedDeficiency == null)
                {
                    Filter.CurrentSelectedDeficiency = ds.Tables[0].Rows[0][0].ToString();

                }

                //SetRowSelection();
            }
          
           
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = de.RowIndex;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;


        BindData();
    }

    //protected void gvDeficiency_SelectIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    gvDeficiency.SelectedIndex = se.NewSelectedIndex;
    //    BindData();
    //}

    //protected void gvDeficiency_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("SELECT"))
    //        {
    //            //BindPageURL(nCurrentRow);
    //            //SetRowSelection();
    //            Label lblWorkPlanId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblWorkPlanId"));
    //            if (lblWorkPlanId != null)
    //                Response.Redirect("../Inspection/InspectionDailyWorkPlanActivity.aspx?WORKPLANID=" + lblWorkPlanId.Text.ToString(), false);
    //        }

    //        if (e.CommandName.ToUpper().Equals("EDIT"))
    //        {

    //            Label lblWorkPlanId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblWorkPlanId"));
    //            if (lblWorkPlanId != null)
    //                Response.Redirect("../Inspection/InspectionDailyWorkPlanActivity.aspx?WORKPLANID=" + lblWorkPlanId.Text.ToString(), false);
    //            //BindPageURL(nCurrentRow);
    //            //SetRowSelection();
    //        }
    //        if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            Label lblWorkPlanId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblWorkPlanId"));
    //            if (lblWorkPlanId != null)
    //            {
    //                PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanDelete(General.GetNullableGuid(lblWorkPlanId.Text.ToString()));
    //            }
    //        }
    //        //if (e.CommandName.ToUpper().Equals("COPY"))
    //        //{
    //        //    Label lblWorkPlanId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblWorkPlanId"));
    //        //    if (lblWorkPlanId != null)
    //        //    {
    //        //        PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanCopy(General.GetNullableGuid(lblWorkPlanId.Text.ToString()));
    //        //    }
    //        //}
    //        BindData();
    //        // SetPageNavigator();
    //    }

    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}



    //protected void gvDeficiency_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    try
    //    {

    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            if (ViewState["SORTEXPRESSION"] != null)
    //            {
    //                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //                if (img != null)
    //                {
    //                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                        img.Src = Session["images"] + "/arrowUp.png";
    //                    else
    //                        img.Src = Session["images"] + "/arrowDown.png";

    //                    img.Visible = true;
    //                }
    //            }
    //        }
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //            {
    //                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //                if (db != null)
    //                {
    //                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
    //                        db.Visible = false;
    //                    db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you Sure you want to Delete Work Plan and Work Plan Activities?'); return false;");
    //                }

    //                Label lblvessel = (Label)e.Row.FindControl("lblVesselid");

    //                Label lblWorkPlanId = (Label)e.Row.FindControl("lblWorkPlanId");

    //                ImageButton db1 = (ImageButton)e.Row.FindControl("imgCopy");
    //                if (db1 != null)
    //                {
    //                    if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName))
    //                        db1.Visible = false;
    //                    db1.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','../Inspection/InspectionDailyWorkPlanCopy.aspx?vesselid=" + lblvessel.Text + "&lblWorkPlanId=" + lblWorkPlanId.Text + "'); return false;");
    //                }

    //                ImageButton imgReport = (ImageButton)e.Row.FindControl("imgWorkPlan");

    //                Label lblWorkPlanID = (Label)e.Row.FindControl("lblWorkPlanId");

    //                if (imgReport != null)
    //                {
    //                    if (!SessionUtil.CanAccess(this.ViewState, imgReport.CommandName))
    //                        imgReport.Visible = false;
    //                    imgReport.Attributes.Add("onclick", "javascript:Openpopup('PO','','../Reports/ReportsView.aspx?applicationcode=9&reportcode=DAILYWORKPLAN&workplanid=" + lblWorkPlanID.Text + "&showmenu=0&showword=NO&showexcel=NO'); return false;");
    //                }

    //                ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
    //                if (cmdEdit != null)
    //                {
    //                    if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName))
    //                        cmdEdit.Visible = false;
    //                }
    //            }
    //        }            
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {

        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
            gv.Rows[0].Attributes["onclick"] = "";
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
            //Filter.CurrentSelectedDeficiency = null;
            BindData();
            //SetRowSelection();
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
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
    protected void gvDeficiency_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    protected void gvDeficiency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDeficiency.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        try
        {


            if (e.Item is GridDataItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you Sure you want to Delete Work Plan and Work Plan Activities?'); return false;");
                }

                RadLabel lblvessel = (RadLabel)e.Item.FindControl("lblVesselid");

                RadLabel lblWorkPlanId = (RadLabel)e.Item.FindControl("lblWorkPlanId");

                LinkButton db1 = (LinkButton)e.Item.FindControl("imgCopy");
                if (db1 != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName))
                        db1.Visible = false;
                    db1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionDailyWorkPlanCopy.aspx?vesselid=" + lblvessel.Text + "&lblWorkPlanId=" + lblWorkPlanId.Text + "'); return false;");
                }

                LinkButton imgReport = (LinkButton)e.Item.FindControl("imgWorkPlan");

                RadLabel lblWorkPlanID = (RadLabel)e.Item.FindControl("lblWorkPlanId");

                if (imgReport != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgReport.CommandName))
                        imgReport.Visible = false;
                    imgReport.Attributes.Add("onclick", "javascript:openNewWindow('PO','','"+Session["sitepath"]+"/Reports/ReportsView.aspx?applicationcode=9&reportcode=DAILYWORKPLAN&workplanid=" + lblWorkPlanID.Text + "&showmenu=0&showword=NO&showexcel=NO'); return false;");
                }

                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName))
                        cmdEdit.Visible = false;
                }
                
            }
            //if (e.Item is GridPagerItem)
            //{
            //    TextBox lblPageNum = (TextBox)e.Item.FindControl("gvLongTermAction_ctl00_ctl03_ctl01_GoToPageTextBox");
            //    lblPageNum.Text = ViewState["PAGENUMBER"].ToString();
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                //BindPageURL(nCurrentRow);
                //SetRowSelection();
                RadLabel lblWorkPlanId = ((RadLabel)e.Item.FindControl("lblWorkPlanId"));
                if (lblWorkPlanId != null)
                    Response.Redirect("../Inspection/InspectionDailyWorkPlanActivity.aspx?WORKPLANID=" + lblWorkPlanId.Text.ToString(), false);
            }

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {

                RadLabel lblWorkPlanId = ((RadLabel)e.Item.FindControl("lblWorkPlanId"));
                int Page = gvDeficiency.CurrentPageIndex + 1;
                if (lblWorkPlanId != null)
                    Response.Redirect("../Inspection/InspectionDailyWorkPlanActivity.aspx?WORKPLANID=" + lblWorkPlanId.Text.ToString()+"&Page="+Page , false);

                //BindPageURL(nCurrentRow);
                //SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblWorkPlanId = ((RadLabel)e.Item.FindControl("lblWorkPlanId"));
                if (lblWorkPlanId != null)
                {
                    PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanDelete(General.GetNullableGuid(lblWorkPlanId.Text.ToString()));

                    BindData();
                    gvDeficiency.Rebind();
                }
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            //if (e.CommandName.ToUpper().Equals("COPY"))
            //{
            //    Label lblWorkPlanId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblWorkPlanId"));
            //    if (lblWorkPlanId != null)
            //    {
            //        PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanCopy(General.GetNullableGuid(lblWorkPlanId.Text.ToString()));
            //    }
            //}

            // SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetFilter()
    {
        NameValueCollection criteria = new NameValueCollection();

        criteria.Add("ucFleet", ucFleet.SelectedFleet);
        criteria.Add("ucVessel", ucVessel.SelectedVessel);

        Filter.CurrentDailyWorkPlan = criteria;
    }
  
}
