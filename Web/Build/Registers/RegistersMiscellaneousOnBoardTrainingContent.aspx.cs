using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersMiscellaneousOnBoardTrainingContent : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersMiscellaneousOnBoardTrainingContent.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOnBoardTrainingContents')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        MenuRegistersOnBoardTrainingContents.AccessRights = this.ViewState;
        MenuRegistersOnBoardTrainingContents.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            lblTitle.Text = "Contents for the Subject " + Request.QueryString["SubjectName"].ToString();
            if (Request.QueryString["SubjectId"] != null)
            {
                ViewState["SubjectId"] = Request.QueryString["SubjectId"].ToString();
            }
            else
                ViewState["SubjectId"] = "0";
            gvOnBoardTrainingContents.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.Title = lblTitle.Text;
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
        
        DataSet ds = PhoenixRegistersMiscellaneousOnBoardTrainingContent.MiscellaneousOnBoardTrainingContentSearch(""
            , subjectid, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvOnBoardTrainingContents.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=MiscellaneousOnBoardTrainingContent.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Onboard Training</h3></td>");
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

    protected void RegistersOnBoardTrainingContents_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvOnBoardTrainingContents.Rebind();
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

        DataSet ds = PhoenixRegistersMiscellaneousOnBoardTrainingContent.MiscellaneousOnBoardTrainingContentSearch(""
            , subjectid, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvOnBoardTrainingContents.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvOnBoardTrainingContents", "Onboard Training", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOnBoardTrainingContents.DataSource = ds;
            gvOnBoardTrainingContents.VirtualItemCount = iRowCount;
        }
        else
        {
            gvOnBoardTrainingContents.DataSource = "";
        }
    }
    
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvOnBoardTrainingContents.Rebind();
    }

    private void InsertOnBoardTrainingContents(string Contents, int subjectid)
    {
        if (!IsValidOnBoardTraining(Contents))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersMiscellaneousOnBoardTrainingContent.InsertMiscellaneousOnBoardTrainingContent(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Contents, subjectid);
    }

    private void UpdateOnBoardTrainingContents(int Contentsid, string Contents, int subjectid)
    {
        if (!IsValidOnBoardTraining(Contents))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersMiscellaneousOnBoardTrainingContent.UpdateMiscellaneousOnBoardTrainingContent(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Contentsid, Contents, subjectid);
        ucStatus.Text = "OnBoard Topic content updated";
    }

    private void DeleteOnBoardTrainingContents(int Contentsid)
    {
        PhoenixRegistersMiscellaneousOnBoardTrainingContent.DeleteMiscellaneousOnBoardTrainingContent(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Contentsid);
    }

    private bool IsValidOnBoardTraining(string Contents)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (Contents.Trim().Equals(""))
            ucError.ErrorMessage = "Contents is required.";

        return (!ucError.IsError);
    }
    

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvOnBoardTrainingContents_ItemCommand(object sender, GridCommandEventArgs e)
    {
        int subjectid;
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        
        else if (e.CommandName.ToUpper().Equals("ADD"))
        {
            subjectid = Int16.Parse(ViewState["SubjectId"].ToString());
            InsertOnBoardTrainingContents(
                ((RadTextBox)e.Item.FindControl("txtContentsAdd")).Text,
                subjectid);
            BindData();
            gvOnBoardTrainingContents.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            subjectid = Int16.Parse(ViewState["SubjectId"].ToString());
            UpdateOnBoardTrainingContents(
                Int16.Parse(((RadLabel)e.Item.FindControl("lblContentsIdEdit")).Text),
                ((RadTextBox)e.Item.FindControl("txtContentsEdit")).Text,
                subjectid);
            BindData();
            gvOnBoardTrainingContents.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeleteOnBoardTrainingContents(Int32.Parse(((RadLabel)e.Item.FindControl("lblContentsId")).Text));
            BindData();
            gvOnBoardTrainingContents.Rebind();
        }        
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvOnBoardTrainingContents_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOnBoardTrainingContents.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvOnBoardTrainingContents_ItemDataBound(object sender, GridItemEventArgs e)
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
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvOnBoardTrainingContents_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
}
