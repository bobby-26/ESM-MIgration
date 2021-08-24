using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersAssessment : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersAssessment.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAssessment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersAssessment.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");            
			MenuRegistersAssessment.AccessRights = this.ViewState;
			MenuRegistersAssessment.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Assessment", "ASSESSMENT");
            toolbar.AddButton("Behavioural Aspects", "BEHAVIOURALASPECTS");
            MenuAssessment.AccessRights = this.ViewState;
            MenuAssessment.MenuList = toolbar.Show();
            MenuAssessment.SelectedMenuIndex = 0;
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();

            if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
                gvAssessment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        if (CommandName.ToUpper().Equals("BEHAVIOURALASPECTS"))
		{
			Response.Redirect("../Registers/RegistersBehavioralAspects.aspx", false);
		}
	
	}

	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds = new DataSet();
		string[] alColumns = { "FLDCODE", "FLDNAME","FLDACTIVEYN" };
		string[] alCaptions = { "Code", "Name","Active Y/N" };
		string sortexpression;
		int? sortdirection = null;
		
		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
		ds = PhoenixRegistersAssessment.AssessmentSearch(General.GetNullableString(txtAssessmentCode.Text),General.GetNullableString(txtSearch.Text)
			, sortexpression, sortdirection,
		(int)ViewState["PAGENUMBER"],
		gvAssessment.PageSize,
		ref iRowCount,
		ref iTotalPageCount);


		Response.AddHeader("Content-Disposition", "attachment; filename=Assessment.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Assessment Register</h3></td>");
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


	protected void RegistersAssessment_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
			{				
				ViewState["PAGENUMBER"] = 1;
				BindData();
                gvAssessment.Rebind();
			}
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

		string[] alColumns = { "FLDCODE", "FLDNAME", "FLDACTIVEYN" };
		string[] alCaptions = { "Code", "Name", "Active Y/N" };
		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		
		DataSet ds = PhoenixRegistersAssessment.AssessmentSearch(General.GetNullableString(txtAssessmentCode.Text), General.GetNullableString(txtSearch.Text)
			, sortexpression, sortdirection,
		(int)ViewState["PAGENUMBER"],
		gvAssessment.PageSize,
		ref iRowCount,
		ref iTotalPageCount);
		
		General.SetPrintOptions("gvAssessment", "Assessment", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvAssessment.DataSource = ds;
            gvAssessment.VirtualItemCount = iRowCount;
		}
		else
		{
            gvAssessment.DataSource = "";
        }
	}
    
	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;
		BindData();
        gvAssessment.Rebind();
	}

	protected void cmdSearch_Click(object sender, EventArgs e)
	{		
		BindData();
        gvAssessment.Rebind();
	}
    

	private void InsertAssessment(string code, string name, int? activeyn, int? otheryn)
	{

		PhoenixRegistersAssessment.InsertAssessment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
								code, name, activeyn, otheryn);
	}

	private void UpdateAssessment(int Assessmentid, string code, string name, int? activeyn, int? otheryn)
	{
		if (!IsValidAssessment(code, name))
		{
			ucError.Visible = true;
			return;
		}
		PhoenixRegistersAssessment.UpdateAssessment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
								Assessmentid, code, name, activeyn, otheryn);
		ucStatus.Text = "Assessment information updated";
	}

	private bool IsValidAssessment(string Assessmentcode, string Assessmentname)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (Assessmentname.Trim().Equals(""))
			ucError.ErrorMessage = "Name is required.";

		if (Assessmentcode.Trim().Equals(""))
			ucError.ErrorMessage = "Code is required.";


		return (!ucError.IsError);
	}

	private void DeleteAssessment(int Assessmentcode)
	{
		PhoenixRegistersAssessment.DeleteAssessment(0, Assessmentcode);
	}

	public StateBag ReturnViewState()
	{
		return ViewState;
	}

    protected void gvAssessment_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;            
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidAssessment(((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertAssessment(
                    ((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                    (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked==true) ? 1 : 0,
                    null
                );
                BindData();
                gvAssessment.Rebind();               
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidAssessment(((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                   ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateAssessment(
                     Int32.Parse(((RadLabel)e.Item.FindControl("lblAssessmentIDEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                     (((RadCheckBox) e.Item.FindControl("chkActiveYNEdit")).Checked == true) ? 1 : 0,
                     null
                 );
                BindData();
                gvAssessment.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteAssessment(Int32.Parse(((RadLabel)e.Item.FindControl("lblAssessmentID")).Text));
                BindData();
                gvAssessment.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                UpdateAssessment(
                       Int32.Parse(((RadLabel)e.Item.FindControl("lblAssessmentIDEdit")).Text),
                       ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                       ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                       (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked == true) ? 1 : 0,
                        null

                   );         
                BindData();
                gvAssessment.Rebind();
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
    
    protected void gvAssessment_ItemDataBound(object sender, GridItemEventArgs e)
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

    protected void gvAssessment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAssessment.CurrentPageIndex + 1;
        BindData();
    }
}
