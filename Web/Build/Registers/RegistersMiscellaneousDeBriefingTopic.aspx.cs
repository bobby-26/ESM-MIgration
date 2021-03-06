using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class RegistersMiscellaneousDeBriefingTopic : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersMiscellaneousDeBriefingTopic.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDeBriefingTopics')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        MenuRegistersDeBriefingTopics.AccessRights = this.ViewState;
        MenuRegistersDeBriefingTopics.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvDeBriefingTopics.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;


        string[] alColumns = { "FLDDEBRIEFINGFEEDBACK"};
        string[] alCaptions = { "De-Briefing"};
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

        DataSet ds = PhoenixRegistersMiscellaneousDeBriefingTopic.MiscellaneousDeBriefingTopicSearch( ""
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvDeBriefingTopics.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=MiscellaneousDeBriefingTopic.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Debriefing Topics</h3></td>");
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

    protected void RegistersDeBriefingTopics_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvDeBriefingTopics.Rebind();
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

        string[] alColumns = { "FLDDEBRIEFINGFEEDBACK" };
        string[] alCaptions = { "De-Briefing" };
        
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int sortdirection;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        DataSet ds = PhoenixRegistersMiscellaneousDeBriefingTopic.MiscellaneousDeBriefingTopicSearch( "", sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"], gvDeBriefingTopics.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvDeBriefingTopics", "Debriefing Topics", alCaptions, alColumns, ds);
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDeBriefingTopics.DataSource = ds;
            gvDeBriefingTopics.VirtualItemCount = iRowCount;
        }
        else
        {
            gvDeBriefingTopics.DataSource = "";
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        gvDeBriefingTopics.Rebind();
    }
    
  
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvDeBriefingTopics.Rebind();
    }

    private void InsertDeBriefingTopics(string DeBriefingFeedback)
    {
        if (!IsValidDeBriefingFeedback(DeBriefingFeedback))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersMiscellaneousDeBriefingTopic.InsertMiscellaneousDeBriefingTopic(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, DeBriefingFeedback);
    }
    private void UpdateDeBriefingTopics(int DeBriefingFeedbackid, string DeBriefingFeedback)
    {
        if (!IsValidDeBriefingFeedback(DeBriefingFeedback))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersMiscellaneousDeBriefingTopic.UpdateMiscellaneousDeBriefingTopic(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, DeBriefingFeedbackid, DeBriefingFeedback);
        ucStatus.Text = "Debriefing Topics information updated";
    }
    private void DeleteDeBriefingTopics(int DeBriefingFeedbackid)
    {
        PhoenixRegistersMiscellaneousDeBriefingTopic.DeleteMiscellaneousDeBriefingTopic(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, DeBriefingFeedbackid);
    }

    private bool IsValidDeBriefingFeedback(string DeBriefingFeedback)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (DeBriefingFeedback.Trim().Equals(""))
            ucError.ErrorMessage = "DeBriefing is required.";        

        return (!ucError.IsError);
    }
    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvDeBriefingTopics_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        else if (e.CommandName.ToUpper().Equals("ADD"))
        {
            InsertDeBriefingTopics(
                ((RadTextBox)e.Item.FindControl("txtDeBriefingFeedbackAdd")).Text);
            BindData();
            gvDeBriefingTopics.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            UpdateDeBriefingTopics(
                Int16.Parse(((RadLabel)e.Item.FindControl("lblDeBriefingFeedbackIdEdit")).Text),
                ((RadTextBox)e.Item.FindControl("txtDeBriefingFeedbackEdit")).Text);            
            BindData();
            gvDeBriefingTopics.Rebind();
        }

        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeleteDeBriefingTopics(Int32.Parse(((RadLabel)e.Item.FindControl("lblDeBriefingFeedbackId")).Text));
            BindData();
            gvDeBriefingTopics.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            UpdateDeBriefingTopics(
            Int16.Parse(((RadLabel)e.Item.FindControl("lblDeBriefingFeedbackIdEdit")).Text),
            ((RadTextBox)e.Item.FindControl("txtDeBriefingFeedbackEdit")).Text);
            BindData();
            gvDeBriefingTopics.Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvDeBriefingTopics_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDeBriefingTopics.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDeBriefingTopics_ItemDataBound(object sender, GridItemEventArgs e)
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

    protected void gvDeBriefingTopics_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
}
