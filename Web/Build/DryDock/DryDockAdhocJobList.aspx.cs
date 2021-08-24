using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class DryDock_DryDockAdhocJobList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockAdhocJobList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvAdhocJob')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockAdhocJobList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                toolbargrid.AddFontAwesomeButton("../DryDock/DryDockAdhocjobList.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            }
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockAdhocjobList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuJob.AccessRights = this.ViewState;
            MenuJob.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "LIST");

            toolbarmain.AddButton("Details", "DETAIL");
        
            MenuAdocJob.AccessRights = this.ViewState;
            MenuAdocJob.MenuList = toolbarmain.Show();
            MenuAdocJob.SelectedMenuIndex = 0;


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = string.IsNullOrEmpty(Request.QueryString["pno"]) ? 1 : int.Parse(Request.QueryString["pno"]);
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["JOBID"] = null;
                if (Request.QueryString["ADHOCJOBID"] != null)
                {
                    ViewState["JOBID"] = Request.QueryString["ADHOCJOBID"].ToString();
                }

                ddlJobType.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(7);
                ddlJobType.DataTextField = "FLDNAME";
                ddlJobType.DataValueField = "FLDMULTISPECID";
                ddlJobType.DataBind();
               // ddlJobType.Items.Insert(0, new ListItem("--Select--", ""));

                DataSet ds = PhoenixDryDockAdhocJob.DryDockProjectList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ddlproject.DataSource = ds;
                ddlproject.DataTextField = "FLDTITLE";
                ddlproject.DataValueField = "FLDORDERID";
                ddlproject.DataBind();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlproject.SelectedValue = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
                }

                gvAdhocJob.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            if (ViewState["JOBID"] == null)
            {
                MenuAdocJob.SelectedMenuIndex = 0;
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AdhocJob_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("DETAIL"))
            {
                if (ViewState["JOBID"] == null)
                {
                    ShowError();
                    return;
                }
                Response.Redirect("../DryDock/DryDockAdhocJob.aspx?ADHOCJOBID=" + ViewState["JOBID"].ToString() + "&pno=" + ViewState["PAGENUMBER"], false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowError()
    {
        ucError.HeaderMessage = "Navigation Error";
        ucError.ErrorMessage = "Please select Adhoc Job and then click details.";
        ucError.Visible = true;
    }

    protected void Job_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvAdhocJob.Rebind();
               // SetPageNavigator();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                Response.Redirect("../DryDock/DryDockAdhocJob.aspx", false);
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtJobNumber.Text = "";
                txtJobTitle.Text = "";
                ddlJobType.ClearSelection(); 
                ddlproject.ClearSelection();

                gvAdhocJob.Rebind();
                //SetPageNavigator();
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

            string[] alColumns = { "FLDPROJECTNAME","FLDNUMBER", "FLDTITLE", "FLDJOBTYPE", "FLDJOBDESCRIPTION","FLDSTATUSNAME" };
            string[] alCaptions = { "Project Name","Number", "Title", "Job Type", "Job Description","Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            byte? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = byte.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixDryDockAdhocJob.DryDockAdhocJobSearch(General.GetNullableGuid(ddlproject.SelectedValue), General.GetNullableString(txtJobNumber.Text), General.GetNullableString(txtJobTitle.Text),
                General.GetNullableInteger(ddlJobType.SelectedValue), null, sortexpression, sortdirection, PhoenixSecurityContext.CurrentSecurityContext.VesselID, 
                gvAdhocJob.CurrentPageIndex+1,
                 gvAdhocJob.PageSize, ref iRowCount, ref iTotalPageCount);


            General.SetPrintOptions("gvAdhocJob", "Adhoc Job", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAdhocJob.DataSource = ds;
               

                if (ViewState["JOBID"] == null || ViewState["JOBID"].ToString() == "")
                {
                    ViewState["JOBID"] = ds.Tables[0].Rows[0][2].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                   // gvAdhocJob.SelectedIndex = 0;
                }
            }
            else
            {

                gvAdhocJob.DataSource = ds;
            }
            gvAdhocJob.VirtualItemCount = iRowCount;
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

    protected void ShowExcel()
    {
        try
        {

            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDPROJECTNAME","FLDNUMBER", "FLDTITLE", "FLDJOBTYPE", "FLDJOBDESCRIPTION","FLDSTATUSNAME" };
            string[] alCaptions = { "Project Name","Number", "Title", "Job Type", "Job Description" ,"Status"};
            string sortexpression;
            byte? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = byte.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixDryDockAdhocJob.DryDockAdhocJobSearch(General.GetNullableGuid(ddlproject.SelectedValue), General.GetNullableString(txtJobNumber.Text), General.GetNullableString(txtJobTitle.Text),
                General.GetNullableInteger(ddlJobType.SelectedValue), null, sortexpression, sortdirection, PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                1,
                 General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Adhoc Job", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvAdhocJob_RowDataBound(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.ToString().ToUpper() == "SORT")
    //        return;

    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = int.Parse(e.CommandArgument.ToString());

    //    if (e.CommandName.ToString().ToUpper() == "EDIT")
    //    {
    //        try
    //        {

    //            string jobid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJobid")).Text;
    //            Response.Redirect("../DryDock/DryDockAdhocJob.aspx?ADHOCJOBID=" + jobid + "&pno=" + ViewState["PAGENUMBER"], false);

    //        }
    //        catch (Exception ex)
    //        {
    //            ucError.ErrorMessage = ex.Message;
    //            ucError.Visible = true;

    //        }
    //    }
    //    else if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
    //    {
    //        string jobid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJobid")).Text;
    //        string dtkey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lbldtkey")).Text;
    //        Response.Redirect("../DryDock/DryDockJobAttachments.aspx?ADHOCJOBID=" + jobid + "&DTKEY=" + dtkey + "&MOD=" + PhoenixModule.DRYDOCK + "&pno=" + ViewState["PAGENUMBER"], false);
    //    }
    //}


    protected void gvAdhocJob_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
    }

    //protected void gvAdhocJob_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        string jobid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJobid")).Text;
    //        string projectid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblProjectid")).Text;

    //        PhoenixDryDockAdhocJob.DeleteDryDockAdhocJob(General.GetNullableGuid(jobid), General.GetNullableGuid(projectid),
    //         PhoenixSecurityContext.CurrentSecurityContext.VesselID);

    //        _gridView.EditIndex = -1;
    //        BindData();
    //        ViewState["JOBID"] = null;
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    
    //protected void gvAdhocJob_SelectIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    gvAdhocJob.SelectedIndex = se.NewSelectedIndex;
    //    ViewState["JOBID"] = ((Label)gvAdhocJob.Rows[se.NewSelectedIndex].FindControl("lblJobid")).Text;
    //    ViewState["DTKEY"] = ((Label)gvAdhocJob.Rows[se.NewSelectedIndex].FindControl("lbldtkey")).Text;
    //    BindData();
    //}

    //protected void gvAdhocJob_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //            {
    //                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //                if (db != null)
    //                {
    //                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
    //                }

    //            }
    //            Label lblTitle = (Label)e.Row.FindControl("lblTitle");
    //            if (lblTitle != null)
    //            {
    //                UserControlToolTip ucToolTipTitle = (UserControlToolTip)e.Row.FindControl("ucToolTipTitle");
    //                lblTitle.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipTitle.ToolTip + "', 'visible');");
    //                lblTitle.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipTitle.ToolTip + "', 'hidden');");
    //            }
    //            Label lblJobDesc = (Label)e.Row.FindControl("lblJobDesc");
    //            if (lblJobDesc != null)
    //            {
    //                lblJobDesc.Text = (drv["FLDJOBDESCRIPTION"].ToString().Length > 100 ? General.SanitizeHtml(drv["FLDJOBDESCRIPTION"].ToString()).Substring(0, 100) : General.SanitizeHtml(drv["FLDJOBDESCRIPTION"].ToString()));
    //                UserControlToolTip ucToolTipJobDesc = (UserControlToolTip)e.Row.FindControl("ucToolTipJobDesc");
    //                ucToolTipJobDesc.Text = General.SanitizeHtml(drv["FLDJOBDESCRIPTION"].ToString());
    //                lblJobDesc.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipJobDesc.ToolTip + "', 'visible');");
    //                lblJobDesc.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipJobDesc.ToolTip + "', 'hidden');");
    //            }

    //            ImageButton attachments = (ImageButton)e.Row.FindControl("cmdAttachments");
    //            if (attachments != null)
    //            {
    //                if (drv["FLDISATTACHMENT"].ToString() == "0")
    //                    attachments.ImageUrl = Session["images"] + "/no-attachment.png";
    //                else
    //                    attachments.ImageUrl = Session["images"] + "/attachment.png";
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    try
    //    {
    //        gvAdhocJob.SelectedIndex = -1;
    //        gvAdhocJob.EditIndex = -1;
    //        if (ce.CommandName == "prev")
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //        else
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //        ViewState["JOBID"] = null;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int result;
    //        if (Int32.TryParse(txtnopage.Text, out result))
    //        {
    //            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //            if (0 >= Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = 1;

    //            if ((int)ViewState["PAGENUMBER"] == 0)
    //                ViewState["PAGENUMBER"] = 1;

    //            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //        }
    //        ViewState["JOBID"] = null;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private void SetPageNavigator()
    //{

    //    try
    //    {
    //        cmdPrevious.Enabled = IsPreviousEnabled();
    //        cmdNext.Enabled = IsNextEnabled();
    //        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

 


    protected void gvAdhocJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvAdhocJob_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        try
        {


            if (e.Item is GridDataItem)
            {
               
                    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                        if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                    }

             
                RadLabel lblTitle = (RadLabel)e.Item.FindControl("lblTitle");
                if (lblTitle != null)
                {
                    UserControlToolTip ucToolTipTitle = (UserControlToolTip)e.Item.FindControl("ucToolTipTitle");
                    lblTitle.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipTitle.ToolTip + "', 'visible');");
                    lblTitle.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipTitle.ToolTip + "', 'hidden');");
                }
                RadLabel lblJobDesc = (RadLabel)e.Item.FindControl("lblJobDesc");
                if (lblJobDesc != null)
                {
                    lblJobDesc.Text = (DataBinder.Eval(e.Item.DataItem, "FLDJOBDESCRIPTION").ToString().Length > 100 ? General.SanitizeHtml(DataBinder.Eval(e.Item.DataItem, "FLDJOBDESCRIPTION").ToString()).Substring(0, 100) : General.SanitizeHtml(DataBinder.Eval(e.Item.DataItem, "FLDJOBDESCRIPTION").ToString()));
                    UserControlToolTip ucToolTipJobDesc = (UserControlToolTip)e.Item.FindControl("ucToolTipJobDesc");
                    ucToolTipJobDesc.Text = General.SanitizeHtml(DataBinder.Eval(e.Item.DataItem, "FLDJOBDESCRIPTION").ToString());
                    lblJobDesc.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipJobDesc.ToolTip + "', 'visible');");
                    lblJobDesc.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipJobDesc.ToolTip + "', 'hidden');");
                }

                ImageButton attachments = (ImageButton)e.Item.FindControl("cmdAttachments");
                if (attachments != null)
                {
                    if (DataBinder.Eval(e.Item.DataItem, "FLDISATTACHMENT").ToString() == "0")
                        attachments.ImageUrl = Session["images"] + "/no-attachment.png";
                    else
                        attachments.ImageUrl = Session["images"] + "/attachment.png";
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAdhocJob_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "SORT")
            return;

      

        if (e.CommandName.ToString().ToUpper() == "EDIT")
        {
            try
            {

                string jobid = ((RadLabel)e.Item.FindControl("lblJobid")).Text;
                Response.Redirect("../DryDock/DryDockAdhocJob.aspx?ADHOCJOBID=" + jobid + "&pno=" + ViewState["PAGENUMBER"], false);

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;

            }
        }
        else if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
        {
            string jobid = ((RadLabel)e.Item.FindControl("lblJobid")).Text;
            string dtkey = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
            Response.Redirect("../DryDock/DryDockJobAttachments.aspx?ADHOCJOBID=" + jobid + "&DTKEY=" + dtkey + "&MOD=" + PhoenixModule.DRYDOCK + "&pno=" + ViewState["PAGENUMBER"], false);
        }

        else if(e.CommandName.ToUpper().Equals("DELETE"))
        {
            string jobid = ((RadLabel)e.Item.FindControl("lblJobid")).Text;
            string projectid = ((RadLabel)e.Item.FindControl("lblProjectid")).Text;

            PhoenixDryDockAdhocJob.DeleteDryDockAdhocJob(General.GetNullableGuid(jobid), General.GetNullableGuid(projectid),
             PhoenixSecurityContext.CurrentSecurityContext.VesselID);

           
            gvAdhocJob.Rebind();
            ViewState["JOBID"] = null;
        }

    }
}
