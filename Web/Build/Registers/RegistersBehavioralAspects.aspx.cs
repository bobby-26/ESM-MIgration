using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersBehavorialAspects : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersBehavioralAspects.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBehavorialAspects')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuRegistersBehavorialAspects.AccessRights = this.ViewState;
            MenuRegistersBehavorialAspects.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Assessment", "ASSESSMENT");
            toolbar.AddButton("Behavioural Aspects", "BEHAVIOURALASPECTS");
            MenuAssessment.AccessRights = this.ViewState;
            MenuAssessment.MenuList = toolbar.Show();
            MenuAssessment.SelectedMenuIndex = 1;
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvBehavorialAspects.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
	protected void Assessment_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("ASSESSMENT"))
		{
			Response.Redirect("../Registers/RegistersAssessment.aspx", false);
		}

	}
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCODE", "FLDNAME"};
        string[] alCaptions = { "Code", "Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersBehavorialAspects.BehavorialSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sortexpression, sortdirection,
                                                                (int)ViewState["PAGENUMBER"],gvBehavorialAspects.PageSize,ref iRowCount,ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=BehavorialAspects.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Behavorial Aspects</h3></td>");
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


    protected void RegistersBehavorialAspects_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns1 = { "FLDCODE", "FLDNAME" };
        string[] alCaptions1 = { "Code", "Description" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixRegistersBehavorialAspects.BehavorialSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sortexpression, sortdirection,
                                                                 (int)ViewState["PAGENUMBER"],gvBehavorialAspects.PageSize, ref iRowCount, ref iTotalPageCount);


        General.SetPrintOptions("gvBehavorialAspects", "Behavorial Aspects", alCaptions1, alColumns1, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBehavorialAspects.DataSource = ds;
            gvBehavorialAspects.VirtualItemCount = iRowCount;
        }
        else
        {
            gvBehavorialAspects.DataSource = "";
        }
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        gvBehavorialAspects.Rebind();
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvBehavorialAspects.Rebind();
    }
    private void InsertBehavorialAspects(string code, string name)
    {

        PhoenixRegistersBehavorialAspects.BehavorialInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                code, name);
    }

    private void UpdateAssessment(int behavorialid, string code, string name)
    {
        if (!IsValidBehavorial(code, name))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersBehavorialAspects.BehavorialUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                behavorialid, code, name);
        ucStatus.Text = "Behavioral Aspects information updated";
    }

    private bool IsValidBehavorial(string code, string description)
    {
        ucError.HeaderMessage = "Please provide the following required informaltion";
        
        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        if (code.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";


        return (!ucError.IsError);
    }

    private void DeleteAssessment(int behavorialid)
    {
        PhoenixRegistersBehavorialAspects.BehavorialDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, behavorialid);
    }
    
    protected void gvBehavorialAspects_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;          
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidBehavorial(((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertBehavorialAspects(
                    ((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text
                );
                BindData();
                gvBehavorialAspects.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidBehavorial(((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                   ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateAssessment(
                     Int32.Parse(((RadLabel)e.Item.FindControl("lblBehavorialIdEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text);              
                BindData();
                gvBehavorialAspects.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteAssessment(Int32.Parse(((RadLabel)e.Item.FindControl("lblBehavorialId")).Text));
                BindData();
                gvBehavorialAspects.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                UpdateAssessment(
                     Convert.ToInt32(((RadLabel)e.Item.FindControl("lblBehavorialIdEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text);              
                BindData();
                gvBehavorialAspects.Rebind();

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

    protected void gvBehavorialAspects_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBehavorialAspects.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvBehavorialAspects_ItemDataBound(object sender, GridItemEventArgs e)
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
}
