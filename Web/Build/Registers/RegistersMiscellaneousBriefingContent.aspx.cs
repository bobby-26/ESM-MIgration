using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersMiscellaneousBriefingContent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersMiscellaneousBriefingContent.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBriefingContents')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        MenuRegistersBriefingContents.AccessRights = this.ViewState;
        MenuRegistersBriefingContents.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            lblContentsHeading.Text = "Contents for the Subject " + Request.QueryString["SubjectName"].ToString();
            if (Request.QueryString["SubjectId"] != null)
            {
                ViewState["SubjectId"] = Request.QueryString["SubjectId"].ToString();
            }
            else
                ViewState["SubjectId"] = "0";
            gvBriefingContents.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.Title = lblContentsHeading.Text;
        MenuTitle.MenuList = toolbar.Show();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? subjectid;

        string[] alColumns = { "FLDCONTENTNAME" };
        string[] alCaptions = { "Contents" };
        string sortexpression;
        int sortdirection;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        subjectid = Int16.Parse(ViewState["SubjectId"].ToString());

        DataSet ds = PhoenixRegistersMiscellaneousBriefingContent.MiscellaneousBriefingContentSearch(""
            , subjectid, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvBriefingContents.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=MiscellaneousBriefingContent.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Briefing Content</h3></td>");
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

    protected void RegistersBriefingContents_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvBriefingContents.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? subjectid;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int sortdirection;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        string[] alColumns = { "FLDCONTENTNAME" };
        string[] alCaptions = { "Contents" };
        
        subjectid = Int16.Parse(ViewState["SubjectId"].ToString());

        DataSet ds = PhoenixRegistersMiscellaneousBriefingContent.MiscellaneousBriefingContentSearch("", subjectid
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvBriefingContents.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvBriefingContents", "Briefing Content", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBriefingContents.DataSource = ds;
            gvBriefingContents.VirtualItemCount = iRowCount;
        }
        else
        {
            gvBriefingContents.DataSource = "";
        }
    }
    
    //protected void gvBriefingContents_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    gvBriefingContents.SelectedIndex = -1;
    //    gvBriefingContents.EditIndex = -1;

    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;

    //    BindData();
    //}
    //protected void gvBriefingContents_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.ToUpper().Equals("SORT"))
    //        return;
    //    int subjectid;
    //    GridView _gridView = (GridView)sender;

    //    subjectid = Int16.Parse(ViewState["SubjectId"].ToString());

    //    int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //    if (e.CommandName.ToUpper().Equals("ADD"))
    //    {
    //        InsertBriefingContents(
    //            ((RadTextBox)_gridView.FooterRow.FindControl("txtContentsAdd")).Text,
    //            subjectid);
    //        BindData();
    //        ((RadTextBox)_gridView.FooterRow.FindControl("txtContentsAdd")).Focus();

    //    }
    //    else if (e.CommandName.ToUpper().Equals("SAVE"))
    //    {
    //        UpdateBriefingContents(
    //            Int16.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblContentsIdEdit")).Text),
    //            ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtContentsEdit")).Text,
    //            subjectid);
    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
       
    //    else if (e.CommandName.ToUpper().Equals("DELETE"))
    //    {
    //        DeleteBriefingContents(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblContentsId")).Text));
    //    }
    //    else if (e.CommandName.ToUpper().Equals("UPDATE"))
    //    {
    //        UpdateBriefingContents(
    //            Int16.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblContentsIdEdit")).Text),
    //            ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtContentsEdit")).Text,
    //            subjectid);
    //        _gridView.EditIndex = -1;
    //        BindData();
    //        SetPageNavigator();
    //    }
    //    SetPageNavigator();
    //}
    //protected void gvBriefingContents_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        int subjectid;
    //        subjectid = Int16.Parse(ViewState["SubjectId"].ToString());
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        UpdateBriefingContents(
    //            Int16.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblContentsIdEdit")).Text),
    //            ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtContentsEdit")).Text,
    //            subjectid);
    //        _gridView.EditIndex = -1;
    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void gvBriefingContents_ItemDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //          ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //          if (db != null)
    //          {
    //              db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //              db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //          }

    //          Label l = (Label)e.Row.FindControl("lblContentsId");

    //          ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
    //          if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

    //          ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
    //          if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

    //          ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
    //          if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
    //    }

    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
    //        if (db != null)
    //        {
    //            if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
    //                db.Visible = false;
    //        }
    //    }
    //}
    

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvBriefingContents.Rebind();
    }

    private void InsertBriefingContents(string Contents, int subjectid)
    {
        if (!IsValidBriefing(Contents))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersMiscellaneousBriefingContent.InsertMiscellaneousBriefingContent(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Contents, subjectid);
    }

    private void UpdateBriefingContents(int Contentsid, string Contents, int subjectid)
    {
        if (!IsValidBriefing(Contents))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersMiscellaneousBriefingContent.UpdateMiscellaneousBriefingContent(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Contentsid, Contents, subjectid);

    }

    private void DeleteBriefingContents(int Contentsid)
    {
        PhoenixRegistersMiscellaneousBriefingContent.DeleteMiscellaneousBriefingContent(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Contentsid);
        ucStatus.Text = "Briefing Topic content updated";
    }
    
    private bool IsValidBriefing(string Contents)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (Contents.Trim().Equals(""))
            ucError.ErrorMessage = "Content is required.";
        
        return (!ucError.IsError);
    }
    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvBriefingContents_ItemCommand(object sender, GridCommandEventArgs e)
    {
        int subjectid;
        subjectid = Int16.Parse(ViewState["SubjectId"].ToString());
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
       else if (e.CommandName.ToUpper().Equals("ADD"))
        {
            InsertBriefingContents(
                ((RadTextBox)e.Item.FindControl("txtContentsAdd")).Text,
                subjectid);
            BindData();
            gvBriefingContents.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            UpdateBriefingContents(
                Int16.Parse(((RadLabel)e.Item.FindControl("lblContentsIdEdit")).Text),
                ((RadTextBox)e.Item.FindControl("txtContentsEdit")).Text,
                subjectid);           
            BindData();
            gvBriefingContents.Rebind();
        }

        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeleteBriefingContents(Int32.Parse(((RadLabel)e.Item.FindControl("lblContentsId")).Text));
            BindData();
            gvBriefingContents.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            UpdateBriefingContents(
                Int16.Parse(((RadLabel)e.Item.FindControl("lblContentsIdEdit")).Text),
                ((RadTextBox)e.Item.FindControl("txtContentsEdit")).Text,
                subjectid);
            BindData();
            gvBriefingContents.Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvBriefingContents_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBriefingContents.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvBriefingContents_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvBriefingContents_SortCommand(object sender, GridSortCommandEventArgs e)
    {

        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
        gvBriefingContents.Rebind();
    }
}
