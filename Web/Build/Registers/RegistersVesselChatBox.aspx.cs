using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersVesselChatBox : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (Request.QueryString["launchedFrom"] == null)
        {
            toolbar.AddButton("Course", "COURSE");
            toolbar.AddButton("Licence", "LICENCE");
            toolbar.AddButton("Correspondence", "CORRESPONDENCE");
        }
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            toolbar.AddButton("Communication", "CHATBOX");
        }
        if (Request.QueryString["vesselid"] != null)
        {
            ucVesselSearch.Visible = false;
            txtVesselName.Visible = true;
            string vesselid = Request.QueryString["vesselid"] != null ? Request.QueryString["vesselid"].ToString() : Filter.CurrentVesselMasterFilter;
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(vesselid));
            if (ds.Tables[0].Rows.Count > 0)
                txtVesselName.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselChatBox.aspx?vesselid=" + Request.QueryString["vesselid"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVesselChatbox')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuChatBox.AccessRights = this.ViewState;
            MenuChatBox.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselChatBox.aspx?vesselid=" + Request.QueryString["vesselid"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvComments')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuComments.AccessRights = this.ViewState;
            MenuComments.MenuList = toolbar.Show();
        }
        else
        {
            ucVesselSearch.Visible = true;
            txtVesselName.Visible = false;
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselChatBox.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVesselChatbox')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuChatBox.AccessRights = this.ViewState;
            MenuChatBox.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselChatBox.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvComments')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuComments.AccessRights = this.ViewState;
            MenuComments.MenuList = toolbar.Show();
        }

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["chatboxid"] = "";                      // "00000000-0000-0000-0000-000000000000";
        }
    }
    protected void ChatBox_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void Comments_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcelComments();
        }
    }
    protected void ShowExcelComments()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string chatboxid = ViewState["chatboxid"].ToString();

        string[] alColumns = { "DESCRIPTION", "NAME", "POSTEDDATE" };
        string[] alCaptions = { "Comments", "Posted By", "Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(new Guid(chatboxid), null, null, null
              , 1, iRowCount, ref iRowCount, ref iTotalPageCount, PhoenixCrewConstants.VESSELCHATBOX);

        Response.AddHeader("Content-Disposition", "attachment; filename=ChatBoxTaskComments.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Task Comments</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDREMARKS", "FLDTASKNUMBER", "FLDCOMPLETEDYESNO" };
        string[] alCaptions = { "Vessel", "Task Description", "Task Number", "Completed" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersVesselChatBox.SearchVesselChatBox(
            General.GetNullableInteger(Request.QueryString["vesselid"] != null ? Request.QueryString["vesselid"].ToString() : ucVesselSearch.SelectedVessel)
            , General.GetNullableInteger(ddlCompleted.SelectedValue)
            , General.GetNullableInteger(ddlUserCreated.SelectedUser)
            , sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"], iRowCount
            , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ChatBoxTasks.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Task List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void VesselTabs_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("LICENCE"))
        {
            Response.Redirect("../Registers/RegistersDocumentRequiredLicence.aspx?launchedfrom=VESSEL", false);
        }
        else if (CommandName.ToUpper().Equals("MEDICAL"))
        {
            Response.Redirect("../Registers/RegistersDocumentRequiredMedical.aspx?launchedfrom=VESSEL", false);
        }
        else if (CommandName.ToUpper().Equals("CORRESPONDENCE"))
        {
            Response.Redirect("../Registers/RegistersVesselCorrespondence.aspx", false);
        }
        else if (CommandName.ToUpper().Equals("COURSE"))
        {
            Response.Redirect("../Registers/RegistersDocumentRequiredCourse.aspx?launchedfrom=VESSEL", false);
        }
    }
    private void BindDataComments()
    {
        DataSet ds;
        int iRowCount = 10;
        int iTotalPageCount = 0;
        string[] alColumns = { "DESCRIPTION", "NAME", "POSTEDDATE" };
        string[] alCaptions = { "Comments", "Posted By", "Date" };
        string chatboxid = ViewState["chatboxid"].ToString();
        if (General.GetNullableGuid(chatboxid) != null)
        {
            ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(new Guid(chatboxid), null, null, null
              , 1, iRowCount, ref iRowCount, ref iTotalPageCount, PhoenixCrewConstants.VESSELCHATBOX);

            General.SetPrintOptions("gvComments", "Task Comments", alCaptions, alColumns, ds);
            gvComments.DataSource = ds.Tables[0];

        }
    }
    private void BindData()
    {
        DataSet ds;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDREMARKS", "FLDTASKNUMBER", "FLDCOMPLETEDYESNO" };
        string[] alCaptions = { "Vessel", "Task Description", "Task Number", "Completed" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        ViewState["vesselid"] = ucVesselSearch.SelectedVessel;

        ds = PhoenixRegistersVesselChatBox.SearchVesselChatBox(
            General.GetNullableInteger(Request.QueryString["vesselid"] != null ? Request.QueryString["vesselid"].ToString() : ucVesselSearch.SelectedVessel)
            , General.GetNullableInteger(ddlCompleted.SelectedValue)
            , General.GetNullableInteger(ddlUserCreated.SelectedUser)
            , sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"], gvVesselChatbox.PageSize
            , ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvVesselChatbox", "Task List", alCaptions, alColumns, ds);
        gvVesselChatbox.DataSource = ds.Tables[0];
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!IsPostBack)
            {
                string lblChatBoxId = ds.Tables[0].Rows[0]["FLDVESSELCHATBOXID"].ToString();
                ViewState["chatboxid"] = lblChatBoxId;
            }
        }
        else ViewState["chatboxid"] = "";                       // "00000000-0000-0000-0000-000000000000";
        gvVesselChatbox.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void gvVesselChatbox_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselChatbox.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvComments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindDataComments();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVesselChatbox_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsRemarksValid(
                    ((UserControlCommonVessel)e.Item.FindControl("ucVesselAdd")).SelectedVessel,
                    ((RadTextBox)e.Item.FindControl("txtCommentsAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersVesselChatBox.InsertVesselChatBox(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(((UserControlCommonVessel)e.Item.FindControl("ucVesselAdd")).SelectedVessel),
                    ((RadTextBox)e.Item.FindControl("txtCommentsAdd")).Text);
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                string lblChatBoxId = ((RadLabel)e.Item.FindControl("lblChatBoxId")).Text;
                ViewState["chatboxid"] = lblChatBoxId;
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string lblChatBoxIdEdit = ((RadLabel)e.Item.FindControl("lblChatBoxIdEdit")).Text;
                int completedyn = ((RadCheckBox)e.Item.FindControl("chkCompletedYN")).Checked == true ? 1 : 0;

                PhoenixRegistersVesselChatBox.UpdateVesselChatBox(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , new Guid(lblChatBoxIdEdit), completedyn);
                Rebind();
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
    protected void Rebind()
    {
        gvVesselChatbox.SelectedIndexes.Clear();
        gvVesselChatbox.EditIndexes.Clear();
        gvVesselChatbox.DataSource = null;
        gvVesselChatbox.Rebind();
        gvComments.SelectedIndexes.Clear();
        gvComments.EditIndexes.Clear();
        gvComments.DataSource = null;
        gvComments.Rebind();
    }
    protected void gvVesselChatbox_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            LinkButton cmdSave = (LinkButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);
            LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null) cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);

            UserControlCommonVessel ucVesselAdd = (UserControlCommonVessel)e.Item.FindControl("ucVesselAdd");
            if (ucVesselAdd != null)
                ucVesselAdd.VesselList = PhoenixRegistersVessel.ListCommonVessels(null, null, null, null, 1, null, null, null, 1, "VSL");
            ViewState["vesselid"] = ucVesselSearch.SelectedVessel;

            if (ViewState["vesselid"] != null)
            {
                ucVesselAdd.SelectedVessel = ViewState["vesselid"].ToString();
                ucVesselAdd.Enabled = false;
            }

        }
    }
    protected void gvComments_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string strComment = ((RadTextBox)e.Item.FindControl("txtCommentsAdd")).Text;
                string chatboxid = ViewState["chatboxid"].ToString();

                if (strComment.Trim().Equals(""))
                {
                    ucError.HeaderMessage = "Please provide the following information";
                    ucError.ErrorMessage = "Comment is required.";
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableGuid(chatboxid) != null)
                {
                    PhoenixCommonDiscussion.TransTypeDiscussionInsert(new Guid(chatboxid), PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , strComment.Trim(), PhoenixCrewConstants.VESSELCHATBOX);
                }
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvComments_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridFooterItem)
        {
            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null) cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);

            UserControlCommonVessel ucVesselAdd = (UserControlCommonVessel)e.Item.FindControl("ucVesselAdd");
            if (ucVesselAdd != null) ucVesselAdd.VesselList = PhoenixRegistersVessel.ListCommonVessels(null, null, null, null, 1, null, null, null, 1, "VSL");


        }
    }
    private bool IsRemarksValid(string vesselid, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vesselid) == null)
            ucError.ErrorMessage = "Vessel is required";

        if (remarks.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        return (!ucError.IsError);
    }
    protected void GetRemarks(object sender, EventArgs e)
    {
        BindData();
    }
}
